using AutoMapper;
using dccs.Data;
using Dioezese.Schulamt.Application.DTOs.Responses;
using Dioezese.Schulamt.Application.Params;
using Dioezese.Schulamt.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace Dioezese.Schulamt.Application
{
    public class PfarreService : ServiceBase
    {
        /// <summary>
        ///  Pfarre suchen
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        protected Result<ENTITY> GetForGrid<ENTITY>(IQueryable<ENTITY> y, PfarreParams ps) where ENTITY : class,IView_Pfarre
        {
            return base.GetForGrid<IView_Pfarre, ENTITY, PfarreParams>(y, ps, (pfarren) =>
            {
                if (ps.Query != null)
                {
                    if (ps.Query.DekanatId != default(int))
                    {
                        pfarren = pfarren.Where(p => p.DekanatId.Value == ps.Query.DekanatId);
                    }
                    if (!string.IsNullOrWhiteSpace(ps.Query.Query))
                    {
                        var q = ps.Query.Query.ToUpper();

//                        Func<IView_Pfarre, bool> qry_test = (p) => p.Pfarrname.ToUpper().Contains(q) || (p.Pfarrnummer.HasValue && p.Pfarrnummer.Value.ToString() == q);


                        pfarren = pfarren.Where(p => p.Pfarrname.ToUpper().Contains(q)
                            || (p.Pfarrnummer.HasValue && p.Pfarrnummer.Value.ToString().Contains(q)));

                        //var pp = pfarren.Where(qry_test);
                        //pfarren = pp.AsQueryable();

                    }
                }

                return pfarren;
            });
        }

        public Result<View_Pfarre_Export> GetForGridExport(PfarreParams ps)
        {
            var db = new SchulamtEntities();
            // Beim Export wollen wir ALLE Seiten exportieren.
            ps.Count = int.MaxValue;
            ps.Page = 1;
            return GetForGrid<View_Pfarre_Export>(db.View_Pfarre_Export.AsQueryable(), ps);
        }

        public Result<View_Pfarre> GetForGrid(PfarreParams ps)
        {
            var db = new SchulamtEntities();
            return GetForGrid(db.View_Pfarre.AsQueryable(), ps);
        }

        public Result<View_Pfarre> TestGetForGrid(PfarreParams ps)
        {
            var db = new SchulamtEntities();
           
            var result = GetForGrid(db.View_Pfarre.AsQueryable<IView_Pfarre>(), ps);

            var convertedData = result.Data.Cast<View_Pfarre>();
            var convertedResult = new Result<View_Pfarre>(ps);
            convertedResult.SetData(convertedData.AsQueryable());
            convertedResult.Total = result.Total;
            convertedResult.Page = result.Page;
            convertedResult.OrderBy = result.OrderBy;
            convertedResult.Desc = result.Desc;
            convertedResult.Count = result.Count;


            return convertedResult;
        }
       


  
        //public IEnumerable GetForExport(PfarreParams ps)
        //{
        //    var db = new SchulamtEntities();
            

        //    return base.GetForGrid(db.View_Pfarre, ps, (pfarren) =>
        //    {
        //        if (ps.Query != null)
        //        {
        //            if (ps.Query.DekanatId != default(int))
        //            {
        //                pfarren = pfarren.Where(p => p.DekanatId.Value == ps.Query.DekanatId);
        //            }
        //            if (!string.IsNullOrWhiteSpace(ps.Query.Query))
        //            {
        //                var q = ps.Query.Query.ToUpper();
        //                pfarren = pfarren.Where(p => p.Pfarrname.ToUpper().Contains(q)
        //                    || (p.PfarreNummer.HasValue && p.PfarreNummer.Value.ToString() == q));
        //            }
        //        }

        //        return pfarren;
        //    });
            
        //}

        /// <summary>
        /// Erstellt eine neue Pfarre.
        /// </summary>
        /// <param name="response"></param>
        public int CreatePfarre(NewPfarreRequest response)
        {
            var db = new SchulamtEntities();

            return CreateEntity(db, response, db.Pfarres, "Pfarre").PfarreId;
        }

        /// <summary>
        /// Ändert eine bestehende Pfarre.
        /// </summary>
        /// <param name="response"></param>
        public void UpdatePfarre(UpdatePfarreRequest response)
        {
            var db = new SchulamtEntities();

            // Get
            var entity = _GetPfarreById(db, response.PfarreId);

            UpdateEntity(db, entity, response, "Pfarre");
        }

        /// <summary>
        /// Liefert die Infors für eine Pfarre, um sie zu bearbeiten.
        /// </summary>
        /// <param name="id"></param>
        public EditPfarreResponse GetPfarreForEdit(int pfarreId)
        { 
            var db = new SchulamtEntities();

            // Get
            var entity = _GetPfarreById(db, pfarreId);

            // Map
            return Mapper.Map<DTOs.Responses.EditPfarreResponse>(entity);
        }

        /// <summary>
        /// Pfarre löschen.
        /// </summary>
        /// <param name="pfarreId"></param>
        public void DeletePfarre(int pfarreId)
        {
            var db = new SchulamtEntities();

            var toRemove = _GetPfarreById(db, pfarreId);

            DeleteEntity(db, toRemove, db.Pfarres);
        }

        /// <summary>
        /// Liste der zur Pfarre zugeordneten Schulen.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public Result<View_PfarreSchulen> GetPfarreSchulenForGrid(int id, dccs.Data.Params ps)
        {
            var db = new SchulamtEntities();

            return new Result<View_PfarreSchulen>(ps, db.View_PfarreSchulen.Where(p => p.PfarreId == id));
        }

        /// <summary>
        /// Liefert die Pfarre nach der Id.
        /// Wird die Pfarre nicht gefunden wird eine ApplicationException geworfen.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="pfarreId"></param>
        /// <returns></returns>
        private Pfarre _GetPfarreById(SchulamtEntities db, int pfarreId)
        {
            var entity = db.Pfarres.SingleOrDefault(p => p.PfarreId == pfarreId);
            if (entity == null) throw new ApplicationException(string.Format("Keine Pfarre mit der Id {0} gefunden!", pfarreId));
            return entity;
        }
    }
}
