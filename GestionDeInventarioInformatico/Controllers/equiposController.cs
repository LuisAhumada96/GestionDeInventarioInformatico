using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDeInventarioInformatico;

namespace GestionDeInventarioInformatico.Controllers
{
    public class equiposController : Controller
    {
        private gestionDBEntities db = new gestionDBEntities();

        // GET: equipos
        private equipos equipo { get; set; }





        #region MyRegion
        public ActionResult Historial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            equipo = db.equipos.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            TempData["cambios"] = equipo.historialCambios.ToList();
            return View(equipo);
        }
        public ActionResult NuevoCambio(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            equipo = db.equipos.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            TempData.Keep("perifericos");
            TempData["perifericos"] = db.perifericos.Where(p => p.idEquipo == equipo.idEquipo);

            return View(equipo);
        }
        public ActionResult Cambio(int? idCambio, int? idEquipo)
        {
            equipo = db.equipos.Find(idEquipo);
            TempData.Keep("cambio");
            TempData["cambio"] = equipo.historialCambios.FirstOrDefault(h => h.idHistorialCambio == idCambio);
            TempData["perifericos"] = db.perifericos.Where(p => p.idEquipo == equipo.idEquipo);
            return RedirectToAction("Historial", "equipos", new { id = idEquipo });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
