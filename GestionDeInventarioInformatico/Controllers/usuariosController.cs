using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionDeInventarioInformatico;
using System.Data.SqlClient;

namespace GestionDeInventarioInformatico.Controllers
{

    public class usuariosController : Controller
    {
        private static datosUsuariosController datosUsuarios;
        private static gestionDBEntities db;
        // GET: usuarios
        public ActionResult Index(int? idEquipo)
        {
            if(datosUsuarios == null)
            {
                datosUsuarios = new datosUsuariosController();
                db = new gestionDBEntities();
                datosUsuarios.equipo = db.equipos.FirstOrDefault(e => e.idEquipo == idEquipo);
                datosUsuarios.departamentos = db.departamentos.ToList();
                if(datosUsuarios.equipo.usuarios.Count > 0)
                {
                    datosUsuarios.usuarios = datosUsuarios.equipo.usuarios.ToList();
                }
            }
            return View(datosUsuarios);
        }
        public ActionResult AsignarDepartamento(int idDepartamento)
        {
            datosUsuarios.equipo.departamentos = db.departamentos.FirstOrDefault(d => d.idDepartamento == idDepartamento);
            datosUsuarios.equipo.usuarios.Clear();
            if(idDepartamento > 0) datosUsuarios.usuarios = db.usuarios.Where(u => u.idDepartamento == idDepartamento).ToList();
            return RedirectToAction("Index", datosUsuarios.equipo.idEquipo);
        }
        public ActionResult AsignarUsuario(int idUsuario)
        {
            datosUsuarios.equipo.usuarios.Add(datosUsuarios.usuarios.FirstOrDefault(u => u.idUsuario == idUsuario));
            return RedirectToAction("Index", datosUsuarios.equipo.idEquipo);
        }
        public ActionResult QuitarUsuario(int idUsuario)
        {
            List<usuarios> aux = new List<usuarios>();
            foreach (var usuario in datosUsuarios.equipo.usuarios.ToList())
            {
                if (usuario.idUsuario != idUsuario) aux.Add(usuario);
            }
            datosUsuarios.equipo.usuarios = aux;

            return RedirectToAction("Index", datosUsuarios.equipo.idEquipo);
        }
        public ActionResult GuardarAsignacion()
        {
            if (ModelState.IsValid)
            {
                var equipo = db.equipos.FirstOrDefault(e => e.idEquipo == datosUsuarios.equipo.idEquipo);
                equipo.usuarios.ToList().AddRange(datosUsuarios.usuarios);
                equipo.departamentos = datosUsuarios.equipo.departamentos;
                db.SaveChanges();
                db.Dispose();
            }
            return Finalizar();
        }
        public ActionResult Finalizar()
        {
            db.Dispose();
            Clear();
            return RedirectToAction("Index", "Home");
        }
        public void Clear()
        {
            datosUsuarios = null;
        }
    }
    public class datosUsuariosController
    {
        public List<usuarios> usuarios = new List<usuarios>();
        public equipos equipo;
        public departamentos depSeleccionado;
        public List<departamentos> departamentos = new List<departamentos>();
    }
}
