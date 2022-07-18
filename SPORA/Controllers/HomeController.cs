using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SPORA.Controllers
{
    public class HomeController : Controller
    {
        BDSPORA db = new BDSPORA();
        
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Home");
        }

        
        public ActionResult Login()
        {

            var usuario = db.Usuario.OrderByDescending(u => u.id).FirstOrDefault();
            
            if (usuario == null)
            {
                ViewBag.Correo = "";
            }
            else
            {
                ViewBag.Correo = usuario.Correo;
                ViewBag.Image = "../Image/" + usuario.Imagen; 
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email)
        {
            try
            {
                using (db)
                {
                    var oUser = (from u in db.Usuario
                                 where u.Correo == email
                                 select u).FirstOrDefault();
                    if (oUser == null)
                    {
                        return RedirectToAction("Registrar", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Menu", "Home");
                    }

                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }


        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(string Nombre, string Apellido, int? Edad, string Telefono, string Correo, HttpPostedFileBase Imagen)
        {
            
            
            var errores = "-";

           

            if (Telefono != "" && Apellido != "" && Nombre != "" && Correo != "" && Edad != null || Imagen != null)
            {
                if (!Regex.IsMatch(Telefono, @"/\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})/"))
                {
                    if (!Regex.IsMatch(Correo, @"/^[^\s@]+@[^\s@]+\.[^\s@]+$/"))
                    {
                        var correoRepetido = db.Usuario.FirstOrDefault(u => u.Correo == Correo);

                        if (correoRepetido == null)
                        {
                            try
                            {
                                var fileName = Path.GetFileName(Imagen.FileName);
                                var Ruta = string.Format("/Image/{0}", Imagen.FileName);
                                string path = Server.MapPath(Ruta);

                                Imagen.SaveAs(path);


                                Usuario u = new Usuario()
                                {

                                    Nombre = Nombre,
                                    Apellido = Apellido,
                                    Correo = Correo,
                                    Edad = Edad,
                                    Telefono = Telefono,
                                    Imagen = Imagen.FileName
                            };


                                db.Usuario.Add(u);
                                db.SaveChanges();

                                return RedirectToAction("Menu", "Home");

                            }
                            catch
                            {
                                return View();
                            }
                        }
                        else
                        {
                            errores = "Estas dirección de correo electronica ya ha sido usada";
                        }

                    }
                    else
                    {
                        errores = "El correo es incorrecto";
                    }
                }
                else
                {
                    errores = "El teléfono es incorrecto";
                }
            }
            else
            {
                errores = "Campo vacio";
            }

            ViewBag.Alert = "alert alert-danger";
            ViewBag.Error = errores;
            
            return View();
            
        }

        public ActionResult Menu()
        {

            if (db.Usuario.Count() == 0)
            {
                return RedirectToAction("Registrar", "Home");
            }
            else
            {
                var usuarios = from u in db.Usuario
                               select u;

                ViewBag.usuarios = usuarios.ToList();

                return View();

            }


        }


        public ActionResult ElimnarUsuario(string id)
        {

            int idUsuario = Convert.ToInt32(id);
            var usuario = db.Usuario.SingleOrDefault(u => u.id == idUsuario);
            db.Usuario.Remove(usuario);
            db.SaveChanges();

            return RedirectToAction("Menu","Home");
            

        }


    }
}
