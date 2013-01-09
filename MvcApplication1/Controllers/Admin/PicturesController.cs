using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;
using System.Configuration;
using System.IO;

namespace MvcApplication1.Controllers.Admin
{
    [Authorize]
    public class PicturesController : Controller
    {
        private ShopDb db = new ShopDb();

        //
        // GET: /Pictures/

        public ActionResult Index()
        {
            return View(db.Pictures.ToList());
        }

        //
        // GET: /Pictures/Details/5

        public ActionResult Details(Guid id)
        {
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        //
        // GET: /Pictures/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Pictures/Create

        [HttpPost]
        public ActionResult Create(IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null || files.Count() == 0) {
                ModelState.AddModelError("Picture", "You need to select at least one picture!");
            }
            else
            {
                foreach (var file in files) {
                    if (file != null && file.ContentLength > 0) {
                        _RequestToPicture(file);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return Redirect("index");
        }

        //
        // GET: /Pictures/Delete/5
        public ActionResult Delete(Guid id)
        {
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        //
        // POST: /Pictures/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Picture picture = db.Pictures.Find(id);
            
            // Remove Picture from Blob Storage
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("pictures");

            var blob = container.GetBlockBlobReference(picture.Id.ToString()+".png");
            blob.DeleteIfExists();
            blob = container.GetBlockBlobReference(picture.Id.ToString() + "_t.png");
            blob.DeleteIfExists();
            
            db.Pictures.Remove(picture);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private static void _PutPicture(Stream data, string name, Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container)
        {
            var blob = container.GetBlockBlobReference(name + ".png");
            data.Seek(0, SeekOrigin.Begin);
            blob.UploadFromStream(data);
        }

        /// <summary>
        /// Bild verkleinern und dabei die Seitenverhältnisse beibehalten.
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="outStream"></param>
        /// <param name="width">Maximale Höhe</param>
        /// <param name="height">Maximale Breite</param>
        private void _ResizePictures(Stream inStream, MemoryStream outStream, int width, int height)
        {
            using (var src = Image.FromStream(inStream))
            {
                // Proportionen beibehalten.
                var ratioX = width / (double)src.Width;
                var ratioY = height / (double)src.Height;
                var ratio = ratioX < ratioY ? ratioX : ratioY;
                var newHeight = (int)(src.Height * ratio);
                var newWidth = (int)(src.Width * ratio);
                var moveX = 0; // (width - newWidth) / 2;
                var moveY = 0; // (height - newHeight) / 2;

                using (var dst = new Bitmap(width, height))
                {
                    using (var g = Graphics.FromImage(dst))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(src, moveX, moveY, dst.Width, dst.Height);
                    }

                    dst.Save(outStream, ImageFormat.Png);
                }
            }
        }

        private void _RequestToPicture(HttpPostedFileBase file)
        {
            var thumbnailStream = new MemoryStream();
            var resizedStream = new MemoryStream();
            _ResizePictures(file.InputStream, resizedStream, 400, 400);
            _ResizePictures(file.InputStream, thumbnailStream, 100, 100);

            var picture = new Picture() {
                Id = Guid.NewGuid(),
                Name = Path.GetFileName(file.FileName)
            };

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("pictures");
            _PutPicture(resizedStream, picture.Id.ToString(), container);
            _PutPicture(thumbnailStream, picture.Id.ToString() + "_t", container);

            db.Pictures.Add(picture);
        }
    }
}