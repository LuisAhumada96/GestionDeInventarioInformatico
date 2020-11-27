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
    public class EquiposController : Controller
    {
        private gestionDBEntities1 db = new gestionDBEntities1();

        // GET: Equipos
        public ActionResult Index()
        {
            var equipos = db.equipos.Include(e => e.departamentos).Include(e => e.unidadAlmacenamiento).Include(e => e.marcas).Include(e => e.proveedores).Include(e => e.ramtipo).Include(e => e.unidadAlmacenamiento1);
            return View(equipos.ToList());
        }

        // GET: Equipos/Details/5
        public ActionResult Details(int? id)
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

        // GET: Equipos/Create
        public ActionResult Create()
        {
            ViewBag.idDepartamento = new SelectList(db.departamentos, "idDepartamento", "nombre");
            ViewBag.hddUnidad = new SelectList(db.unidadAlmacenamiento, "idUnidad", "descripcion");
            ViewBag.idMarca = new SelectList(db.marcas, "idMarca", "descripcion");
            ViewBag.idProveedor = new SelectList(db.proveedores, "idProveedor", "cuit");
            ViewBag.idRamTipo = new SelectList(db.ramtipo, "idRamTipo", "descripcion");
            ViewBag.ssdUnidad = new SelectList(db.unidadAlmacenamiento, "idUnidad", "descripcion");
            return View();
        }

        // POST: Equipos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEquipo,nombre,descripcion,modelo,fecCompra,garantia,ram,idRamTipo,idMarca,idDepartamento,idProveedor,motherboard,cpu,gpu,hdd,ssd,hddUnidad,ssdUnidad")] equipos equipos)
        {
            if (ModelState.IsValid)
            {
                db.equipos.Add(equipos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idDepartamento = new SelectList(db.departamentos, "idDepartamento", "nombre", equipos.idDepartamento);
            ViewBag.hddUnidad = new SelectList(db.unidadAlmacenamiento, "idUnidad", "descripcion", equipos.hddUnidad);
            ViewBag.idMarca = new SelectList(db.marcas, "idMarca", "descripcion", equipos.idMarca);
            ViewBag.idProveedor = new SelectList(db.proveedores, "idProveedor", "cuit", equipos.idProveedor);
            ViewBag.idRamTipo = new SelectList(db.ramtipo, "idRamTipo", "descripcion", equipos.idRamTipo);
            ViewBag.ssdUnidad = new SelectList(db.unidadAlmacenamiento, "idUnidad", "descripcion", equipos.ssdUnidad);
            return View(equipos);
        }

        // GET: Equipos/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.idDepartamento = new SelectList(db.departamentos, "idDepartamento", "nombre", equipos.idDepartamento);
            ViewBag.hddUnidad = new SelectList(db.unidadAlmacenamiento, "idUnidad", "descripcion", equipos.hddUnidad);
            ViewBag.idMarca = new SelectList(db.marcas, "idMarca", "descripcion", equipos.idMarca);
            ViewBag.idProveedor = new SelectList(db.proveedores, "idProveedor", "cuit", equipos.idProveedor);
            ViewBag.idRamTipo = new SelectList(db.ramtipo, "idRamTipo", "descripcion", equipos.idRamTipo);
            ViewBag.ssdUnidad = new SelectList(db.unidadAlmacenamiento, "idUnidad", "descripcion", equipos.ssdUnidad);
            return View(equipos);
        }

        // POST: Equipos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEquipo,nombre,descripcion,modelo,fecCompra,garantia,ram,idRamTipo,idMarca,idDepartamento,idProveedor,motherboard,cpu,gpu,hdd,ssd,hddUnidad,ssdUnidad")] equipos equipos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idDepartamento = new SelectList(db.departamentos, "idDepartamento", "nombre", equipos.idDepartamento);
            ViewBag.hddUnidad = new SelectList(db.unidadAlmacenamiento, "idUnidad", "descripcion", equipos.hddUnidad);
            ViewBag.idMarca = new SelectList(db.marcas, "idMarca", "descripcion", equipos.idMarca);
            ViewBag.idProveedor = new SelectList(db.proveedores, "idProveedor", "cuit", equipos.idProveedor);
            ViewBag.idRamTipo = new SelectList(db.ramtipo, "idRamTipo", "descripcion", equipos.idRamTipo);
            ViewBag.ssdUnidad = new SelectList(db.unidadAlmacenamiento, "idUnidad", "descripcion", equipos.ssdUnidad);
            return View(equipos);
        }

        // GET: Equipos/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            equipos equipos = db.equipos.Find(id);
            db.equipos.Remove(equipos);
            db.SaveChanges();
            return RedirectToAction("Index");
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
