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
           var equipos = db.equipos.Include(e => e.departamentos).Include(e => e.unidadAlmacenamiento).Include(e => e.marcas).Include(e => e.proveedores).Include(e => e.ramtipo).Include(e => e.tipoEquipos).Include(e => e.unidadAlmacenamiento1);
           ViewData["equipos"] = equipos.ToList();
           return View();
        }
        public ActionResult proveedores()
        {
            ViewData["proveedores"] = db.proveedores.ToList();
            return View();
        }
        public ActionResult Historial(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            equipos equipos = db.equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return View(equipos);
        }
    }
}