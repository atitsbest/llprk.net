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
        public ActionResult Create(Picture picture)
        {
            if (ModelState.IsValid)
            {
                var thumbnailStream = new MemoryStream();
                var resizedStream = new MemoryStream();
                _ResizePictures(resizedStream, 400, 400);
                _ResizePictures(thumbnailStream, 100, 100);

                picture.Id = Guid.NewGuid();

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);

                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("pictures");
                _PutPicture(resizedStream, picture.Id.ToString(),  container);
                _PutPicture(thumbnailStream, picture.Id.ToString()+"_t", container);

                db.Pictures.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(picture);
        }

        //
        // GET: /Pictures/Edit/5

        public ActionResult Edit(Guid id)
        {
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        //
        // POST: /Pictures/Edit/5

        [HttpPost]
        public ActionResult Edit(Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(picture);
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

        private void _ResizePictures(MemoryStream outStream, int width, int height)
        {
            using (var src = Image.FromStream(Request.Files.Get(0).InputStream))
            {
                using (var dst = new Bitmap(width, height))
                {
                    using (var g = Graphics.FromImage(dst))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(src, 0, 0, dst.Width, dst.Height);
                    }

                    dst.Save(outStream, ImageFormat.Png);
                }
            }
        }
    }
}