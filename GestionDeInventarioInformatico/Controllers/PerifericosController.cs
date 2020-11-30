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
    public class PerifericosController : Controller
    {
        private gestionDBEntities db = new gestionDBEntities();

        // GET: Perifericos

        public ActionResult Nuevo()
        {
            TempData["perifericoID"] = db.perifericos.Count() + 1;
            TempData["proveedores"] = db.proveedores.ToList();
            TempData["marcas"] = db.marcas.ToList();
            TempData["tipoPerifericos"] = db.tipoPerifericos.ToList();
            return View();
        }


        [HttpPost]
        public ActionResult Guardar(int perifericoID,string perifericoNombre, int perifericoMarca, int perifericoEstado, string perifericoModelo, int perifericoTipo, int perifericoProveedor, DateTime perifericoFecCompra, DateTime? perifericoFecGarantia, string perifericoCaracteristicas)
        {
            if (ModelState.IsValid)
            {
                db.perifericos.Add(new perifericos()
                {
                    idPeriferico = perifericoID,
                    nombre = perifericoNombre,
                    modelo = perifericoModelo,
                    idMarca = perifericoMarca,
                    estado = perifericoEstado,
                    idTipoPeriferico = perifericoTipo,
                    idProveedor = perifericoProveedor,
                    fecCompra = perifericoFecCompra,
                    garantia = perifericoFecGarantia,
                    caracteristicas = perifericoCaracteristicas
                });;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View("New");
        }


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
