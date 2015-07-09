using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Llprk.DataAccess.Models;

namespace Llprk.Web.UI.Areas.Admin.Controllers.api
{
    public class PicturesController : ApiController
    {
        private Entities db = new Entities();

        // GET api/Pictures
        public IEnumerable<Picture> GetPictures()
        {
            return db.Pictures.AsEnumerable();
        }

        // GET api/Pictures/5
        public Picture GetPicture(Guid id)
        {
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return picture;
        }

        // PUT api/Pictures/5
        public HttpResponseMessage PutPicture(Guid id, Picture picture)
        {
            if (ModelState.IsValid && id == picture.Id)
            {
                db.Entry(picture).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Pictures
        public HttpResponseMessage PostPicture(Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Pictures.Add(picture);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, picture);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = picture.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Pictures/5
        public HttpResponseMessage DeletePicture(Guid id)
        {
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Pictures.Remove(picture);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, picture);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}