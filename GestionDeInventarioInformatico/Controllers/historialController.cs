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
    public class HistorialController : Controller
    {
        private gestionDBEntities db = new gestionDBEntities();

        // GET: equipos
        private static equipos equipo { get; set; }

        public static TipoPerifericos tipoPerifericoSeleccionado;

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
            else
            {
                TempData["cambios"] = equipo.historialCambios.ToList();
            }
            return View(equipo);
        }
        #region Cambios
        public ActionResult NuevoCambio(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (equipo == null)
            {
                equipo = db.equipos.Find(id);
                TempData["perifericosDisponibles"] = db.perifericos.Where(p => p.estado == (int)EstadoPeriferico.Disponible).ToList();
                TempData["tipoPerifericoSeleccionado"] = tipoPerifericoSeleccionado;
                TempData.Keep("perifericosDisponibles");
                TempData.Keep("tipoPerifericoSeleccionado");
                if (equipo == null)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                TempData["perifericosDisponibles"] = db.perifericos.Where(p => p.estado == (int)EstadoPeriferico.Disponible).ToList();
                TempData["tipoPerifericoSeleccionado"] = tipoPerifericoSeleccionado;
                TempData.Keep("perifericosDisponibles");
                TempData.Keep("tipoPerifericoSeleccionado");
            }

            return View(equipo);
        }
        public ActionResult AgregarPeriferico(int? idPerifericoSeleccionado)
        {
            if (idPerifericoSeleccionado != null)
            {
                var periferico = db.perifericos.FirstOrDefault(p => p.idPeriferico == idPerifericoSeleccionado);
                periferico.estado = (int)EstadoPeriferico.Ocupado;
                periferico.idEquipo = equipo.idEquipo;
                equipo.perifericos.Add(db.perifericos.FirstOrDefault(p => p.idPeriferico == idPerifericoSeleccionado));
                db.SaveChanges();
                return RedirectToAction("NuevoCambio", "Historial", new { id = equipo.idEquipo });
            }
            return RedirectToAction("NuevoCambio", "Historial", new { id = equipo.idEquipo });
        }
        public ActionResult QuitarPeriferico(int? idPeriferico)
        {
            var periferico = db.perifericos.FirstOrDefault(p => p.idPeriferico == idPeriferico);
            periferico.estado = (int)EstadoPeriferico.Disponible;
            periferico.idEquipo = null;
            List<perifericos> aux = new List<perifericos>();
            foreach (var item in equipo.perifericos.ToList())
            {
                if (item.idPeriferico != idPeriferico) aux.Add(item);
                else item.estado = (int)EstadoPeriferico.Disponible;
            }
            equipo.perifericos = aux;
            db.SaveChanges();
            renovarKeyMarcas();
            return RedirectToAction("NuevoCambio", "Historial", new { id = equipo.idEquipo });
        }
        public ActionResult BuscarTipoPeriferico(int? tipoDePeriferico)
        {
            var v = db.perifericos.Where(p => p.estado == (int)EstadoPeriferico.Disponible && p.tipoPerifericos.idTipoPeriferico == tipoDePeriferico).ToList();
            TempData["perifericosDisponibles"] = v;
            TempData.Keep("perifericosDisponibles");
            tipoPerifericoSeleccionado = (TipoPerifericos)Enum.Parse(typeof(TipoPerifericos), tipoDePeriferico.ToString());
            return RedirectToAction("NuevoCambio", "Historial", new { id = equipo.idEquipo });
        }
        public ActionResult GuardarCambio(FormCollection formCollection)
        {
            historialCambios cambio = new historialCambios();
            cambio.idEquipo = equipo.idEquipo;
            cambio.idHistorialCambio = equipo.historialCambios.Count + 1;
            cambio.descripcion = formCollection["descripcion"];
            cambio.observaciones = formCollection["observaciones"];
            cambio.fecha = DateTime.Parse(formCollection["fechaCambio"]);
            cambio.idTipoCambio = Int32.Parse(formCollection["tipoCambio"]);

          

            if (ModelState.IsValid)
            {
                db.equipos.FirstOrDefault(e => e.idEquipo == cambio.idEquipo).idEquipo = cambio.idEquipo;
                db.historialCambios.Add(cambio);
                db.SaveChanges();
                return Finalizar();
            }
            return RedirectToAction("NuevoCambio", "Historial", new { id = equipo.idEquipo });
        }
        public ActionResult Detalle(int? idCambio, int? idEquipo)
        {
            equipo = db.equipos.Find(idEquipo);
            TempData.Keep("cambio");
            TempData["cambio"] = equipo.historialCambios.FirstOrDefault(h => h.idHistorialCambio == idCambio);
            TempData["perifericos"] = db.perifericos.Where(p => p.idEquipo == equipo.idEquipo);
            return RedirectToAction("Historial", "Historial", new { id = idEquipo });
        }
        #endregion
        public ActionResult Cancelar()
        {
            //foreach (var periferico in equipo.perifericos)
            //{
            //    var equipo = db.perifericos.FirstOrDefault(p => p.idEquipo == periferico.idEquipo);
            //    equipo.estado = (int)EstadoPeriferico.Disponible;
            //    equipo.idEquipo = null;
            //}
            //db.SaveChanges();
            return Finalizar();
        }
        public ActionResult Finalizar()
        {
            db.Dispose();
            equipo = null;
            tipoPerifericoSeleccionado = 0;
            return RedirectToAction("Index", "Home");
        }
        #endregion
        private void renovarKeyMarcas()
        {
            TempData["marcas"] = db.marcas.ToList();
            TempData.Keep("marcas");

        }


    }
}
