using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.DataAccess.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;
using System.Configuration;
using System.IO;
using Llprk.Web.UI.Controllers;
using Llprk.Web.UI.Areas.Admin.Models;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class PicturesController : ApplicationController
    {
        //
        // GET: /Pictures/

        public virtual ActionResult Index()
        {
            return View(db.Pictures.OrderBy(p => p.Name).ToList());
        }

        //
        // GET: /Pictures/Details/5

        public virtual ActionResult Details(Guid id)
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

        public virtual ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Pictures/Create

        [HttpPost]
        public virtual ActionResult Create(IEnumerable<HttpPostedFileBase> files)
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
        public virtual ActionResult Delete(Guid id)
        {
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }

            return View(new PictureDelete() {
                Picture = picture,
                Products = from p in db.Products
                           where p.Pictures.Any(pi => pi.Id == picture.Id)
                           select p
            });
        }

        //
        // POST: /Pictures/Delete/5

        [HttpPost, ActionName("Delete")]
        public virtual ActionResult DeleteConfirmed(Guid id)
        {
            Picture picture = db.Pictures.Find(id);
            
            // Remove Picture from Blob Storage
            var storageAccount = _getStorageAccount();

            var blobClient = storageAccount.CreateCloudBlobClient();
#if DEBUG
            var container = blobClient.GetContainerReference("picturestest");
#else
            var container = blobClient.GetContainerReference("pictures");
#endif

            // Erst die Bilder aus der DB löschen...
            db.Pictures.Remove(picture);
            db.SaveChanges();

            // ...und dann vom Blob-Storage.
            var blob = container.GetBlockBlobReference(picture.Id.ToString()+".png");
            blob.DeleteIfExists();
            blob = container.GetBlockBlobReference(picture.Id.ToString() + "_t.png");
            blob.DeleteIfExists();
            
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Bild auf Blob-Storage hochladen.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <param name="container"></param>
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
                var ratio = Math.Min(ratioX, ratioY);
                var newWidth = (int)(src.Width * ratio);
                var newHeight = (int)(src.Height * ratio);
                var moveX = (width - newWidth) / 2;
                var moveY = (height - newHeight) / 2;

                using (var dst = new Bitmap(width, height))
                {
                    using (var g = Graphics.FromImage(dst))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(src, moveX, moveY, newWidth, newHeight);
                    }

                    dst.Save(outStream, ImageFormat.Png);
                }
            }
        }

        /// <summary>
        /// Von einem hochgeladenen File, Bild und Thumbnail speichern.
        /// </summary>
        /// <param name="file"></param>
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

            var storageAccount = _getStorageAccount();

            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("pictures");
            _PutPicture(resizedStream, picture.Id.ToString(), container);
            _PutPicture(thumbnailStream, picture.Id.ToString() + "_t", container);

            db.Pictures.Add(picture);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        private static CloudStorageAccount _getStorageAccount()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnection"]);
            return storageAccount;
        }
    }
}