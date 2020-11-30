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
    public class HomeController : Controller
    {
        private gestionDBEntities db = new gestionDBEntities();
        public ActionResult Index()
        {
           var equipos = db.equipos.Include(e => e.departamentos).Include(e => e.unidadAlmacenamiento).Include(e => e.marcas)
                                   .Include(e => e.proveedores).Include(e => e.ramtipo).Include(e => e.tipoEquipos).Include(e => e.unidadAlmacenamiento1);
            ViewData["equipos"] = equipos.ToList();
            ViewData["perifericos"] = db.perifericos.Include(e => e.tipoPerifericos).ToList();
            ViewData["cantidadPerifericos"] = db.perifericos.Count();
           return View();
        }
        public ActionResult proveedores()
        {
            ViewData["proveedores"] = db.proveedores.ToList();
            return View();
        }
    }
}