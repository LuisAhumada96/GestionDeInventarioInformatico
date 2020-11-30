using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDeInventarioInformatico;
using GestionDeInventarioInformatico.Models;

namespace GestionDeInventarioInformatico.Controllers
{
    public class equiposController : Controller
    {
        private gestionDBEntities db = new gestionDBEntities();

        // GET: equipos
        private static equipos equipo { get; set; }

        #region Historial de Cambios
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
            if(equipo == null)
            {
                equipo = db.equipos.Find(id);
                if (equipo == null)
                {
                    return HttpNotFound();
                }
            }
            TempData.Keep("perifericosDisponibles");
            TempData["perifericosDisponibles"] = db.perifericos.Where(p => p.estado == (int)EstadoPeriferico.Disponible).ToList();
            return View(equipo);
        }
        public ActionResult AgregarPeriferico(int? idPerifericoSeleccionado)
        {
            db.perifericos.FirstOrDefault(p => p.idPeriferico == idPerifericoSeleccionado).estado = (int)EstadoPeriferico.Ocupado;
            db.SaveChanges();
            equipo.perifericos.Add(db.perifericos.FirstOrDefault(p => p.idPeriferico == idPerifericoSeleccionado));
            return RedirectToAction("NuevoCambio", "equipos", new { id = equipo.idEquipo });
        }
        public ActionResult QuitarPeriferico(int? idPeriferico)
        {
            db.perifericos.FirstOrDefault(p => p.idPeriferico == idPeriferico).estado = (int)EstadoPeriferico.Disponible;
            List<perifericos> aux = new List<perifericos>();
            foreach (var item in equipo.perifericos.ToList())
            {
                if (item.idPeriferico != idPeriferico) aux.Add(item);
            }
            equipo.perifericos = aux;
            
            
            db.SaveChanges();
            return RedirectToAction("NuevoCambio", "equipos", new { id = equipo.idEquipo });
        }

        public ActionResult GuardarCambio()
        {
            if (ModelState.IsValid)
            {
                db.equipos.Add(new equipos()
                {

                });
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View("New");
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
