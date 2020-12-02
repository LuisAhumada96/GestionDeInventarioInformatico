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
        private static gestionDBEntities db = new gestionDBEntities();
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
                renovarKeyMarcasYproveedores();
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
                renovarKeyMarcasYproveedores();
            }
            return View(equipo);
        }
        public ActionResult AgregarPeriferico(int? idPerifericoSeleccionado)
        {
            if(idPerifericoSeleccionado != null)
            {
                var periferico = db.perifericos.FirstOrDefault(p => p.idPeriferico == idPerifericoSeleccionado);
                periferico.estado = (int)EstadoPeriferico.Ocupado;
                periferico.idEquipo = equipo.idEquipo;
                equipo.perifericos.Add(db.perifericos.FirstOrDefault(p => p.idPeriferico == idPerifericoSeleccionado));
                db.SaveChanges();
                return RedirectToAction("Nuevo", "Equipos", new { id = equipo.idEquipo });
            }
            return RedirectToAction("Nuevo", "Equipos", new { id = equipo.idEquipo });
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
                else item.estado = (int) EstadoPeriferico.Disponible;
            }
            equipo.perifericos = aux;
            db.SaveChanges();
            renovarKeyMarcasYproveedores();
            return RedirectToAction("Nuevo", "Equipos", new { id = equipo.idEquipo });
        }
        public ActionResult BuscarTipoPeriferico(int? tipoDePeriferico)
        {
            var v = db.perifericos.Where(p => p.estado == (int)EstadoPeriferico.Disponible && p.tipoPerifericos.idTipoPeriferico == tipoDePeriferico).ToList();
            TempData["perifericosDisponibles"] = v;
            TempData.Keep("perifericosDisponibles");
            tipoPerifericoSeleccionado = (TipoPerifericos) Enum.Parse(typeof(TipoPerifericos), tipoDePeriferico.ToString());
            return RedirectToAction("Nuevo", "Equipos", new { id = equipo.idEquipo });
        }
        public ActionResult GuardarEquipo(FormCollection formCollection)
        {
            equipo.nombre = formCollection["nombre"];
            equipo.fecCompra = DateTime.Parse(formCollection["fecCompra"]);
            equipo.garantia = DateTime.Parse(formCollection["fecGarantia"]);
            equipo.idProveedor = Int32.Parse(formCollection["proveedor"]);
            equipo.ram = Int32.Parse(formCollection["ram"]);
            equipo.ramtipo = new ramtipo();
            equipo.ramtipo.idRamTipo = (short) Int32.Parse(formCollection["ramTipo"]);
            equipo.hdd = Int32.Parse(formCollection["hdd"]);
            equipo.hddUnidad = (short)Int32.Parse(formCollection["unidadHDD"]);
            equipo.motherboard = formCollection["motherboard"];
            equipo.marcas = new marcas();
            equipo.marcas.idMarca = Int32.Parse(formCollection["marca"]);
            equipo.modelo = formCollection["modelo"];
            equipo.cpu = formCollection["cpu"];
            equipo.ssd = Int32.Parse(formCollection["ssd"]);
            equipo.ssdUnidad = (short)Int32.Parse(formCollection["unidadSSD"]);
            equipo.idTipoEquipo = Int32.Parse(formCollection["tipoEquipo"]);

            db.equipos.Add(equipo);
            db.SaveChanges();
            return Finalizar();
        }
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

        private void renovarKeyMarcasYproveedores()
        {
            TempData["marcas"] = db.marcas.ToList();
            TempData["proveedores"] = db.proveedores.ToList();
            TempData.Keep("marcas");
            TempData.Keep("proveedores");
        }
    }
}
