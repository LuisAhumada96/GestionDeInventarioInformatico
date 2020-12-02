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
    public class EquiposController : Controller
    {
        private gestionDBEntities db = new gestionDBEntities();
        public static TipoPerifericos tipoPerifericoSeleccionado;
        private static equipos equipo { get; set; }
        public ActionResult Nuevo()
        {
            if (equipo == null)
            {
                TempData["perifericosDisponibles"] = db.perifericos.Where(p => p.estado == (int)EstadoPeriferico.Disponible).ToList();
                TempData["tipoPerifericoSeleccionado"] = tipoPerifericoSeleccionado;
                TempData.Keep("perifericosDisponibles");
                TempData.Keep("tipoPerifericoSeleccionado");
                renovarKeyMarcas();
                if (equipo == null)
                {

                    equipo = new equipos();
                    equipo.idEquipo = db.equipos.Count() + 1;
                }
            }
            else
            {
                TempData["perifericosDisponibles"] = db.perifericos.Where(p => p.estado == (int)EstadoPeriferico.Disponible && p.tipoPerifericos.idTipoPeriferico == (int) tipoPerifericoSeleccionado).ToList();
                TempData["tipoPerifericoSeleccionado"] = tipoPerifericoSeleccionado;
                TempData.Keep("perifericosDisponibles");
                TempData.Keep("tipoPerifericoSeleccionado");
                renovarKeyMarcas();
            }
            return View(equipo);
        }
        public ActionResult AgregarPeriferico(int? idPerifericoSeleccionado)
        {
            if(idPerifericoSeleccionado != null)
            {
                db.perifericos.FirstOrDefault(p => p.idPeriferico == idPerifericoSeleccionado).estado = (int)EstadoPeriferico.Ocupado;
                equipo.perifericos.Add(db.perifericos.FirstOrDefault(p => p.idPeriferico == idPerifericoSeleccionado));
                db.SaveChanges();
                return RedirectToAction("Nuevo", "Equipos", new { id = equipo.idEquipo });
            }
            return RedirectToAction("Nuevo", "Equipos", new { id = equipo.idEquipo });
        }
        public ActionResult QuitarPeriferico(int? idPeriferico)
        {
            db.perifericos.FirstOrDefault(p => p.idPeriferico == idPeriferico).estado = (int)EstadoPeriferico.Disponible;
            List<perifericos> aux = new List<perifericos>();
            foreach (var item in equipo.perifericos.ToList())
            {
                if (item.idPeriferico != idPeriferico) aux.Add(item);
                else item.estado = (int) EstadoPeriferico.Disponible;
            }
            equipo.perifericos = aux;
            db.SaveChanges();
            renovarKeyMarcas();
            return RedirectToAction("Nuevo", "Equipos", new { id = equipo.idEquipo });
        }
        public ActionResult BuscarTipoPeriferico(int? tipoDePeriferico)
        {
            var v = db.perifericos.Where(p => p.estado == (int)EstadoPeriferico.Disponible && p.tipoPerifericos.idTipoPeriferico == tipoDePeriferico).ToList();
            TempData["perifericosDisponibles"] = v;
            TempData.Keep("perifericosDisponibles");
            tipoPerifericoSeleccionado = (TipoPerifericos) Enum.Parse(typeof(TipoPerifericos), tipoDePeriferico.ToString());
            db.SaveChanges();
            return RedirectToAction("Nuevo", "Equipos", new { id = equipo.idEquipo });
        }
        public ActionResult Cancelar()
        {
            foreach (var periferico in equipo.perifericos)
            {
                var equipo = db.perifericos.FirstOrDefault(p => p.idEquipo == periferico.idEquipo);
                equipo.estado = (int)EstadoPeriferico.Disponible;
                equipo.idEquipo = null;
            }
            db.SaveChanges();
            return Finalizar();
        }
        public ActionResult Finalizar()
        {
            db.Dispose();
            equipo = null;
            return RedirectToAction("Index", "Home");
        }

        private void renovarKeyMarcas()
        {
            TempData["marcas"] = db.marcas.ToList();
            TempData.Keep("marcas");
        }
    }
}
