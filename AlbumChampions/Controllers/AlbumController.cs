using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using AlbumChampions.Helpers;
using System.Dynamic;

namespace AlbumChampions.Models
{
    public class AlbumController : Controller
    {
        // GET: Album
        public ActionResult IndexEquipo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IndexEquipo(HttpPostedFileBase PostConseguidas)
        {
            string archivoConseguidas = string.Empty;
            if (PostConseguidas != null)
            {
                string ArchivoEstampas = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(ArchivoEstampas))
                {
                    Directory.CreateDirectory(ArchivoEstampas);
                }
                archivoConseguidas = ArchivoEstampas + Path.GetFileName(PostConseguidas.FileName);
                string extension = Path.GetExtension(PostConseguidas.FileName);
                PostConseguidas.SaveAs(archivoConseguidas);
                string csvData = System.IO.File.ReadAllText(archivoConseguidas);
                int numeroAux = 0;
                foreach (string fila in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(fila))
                    {
                        bool EstampaColeccionada=false;
                        if (numeroAux !=0)
                        {
                            String[] campos = fila.Split('|');
                            string identificador = campos[0];
                            string coleccionada = campos[1];
                            if (coleccionada=="true\r")
                            {
                                EstampaColeccionada = true;
                            }
                            else if (coleccionada=="false\r")
                            {
                                EstampaColeccionada = false;
                            }
                            Datos.Instance.diccionarioColeccionada.Add(identificador, EstampaColeccionada);
                        }                        
                        numeroAux++;
                    }
                }
            }
            return View();
        }
        public ActionResult IndexAlbum()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IndexAlbum(HttpPostedFileBase PostChampions)
        {
            string ArchivoChampions = string.Empty;
            if (PostChampions != null)
            {
                string Archivo = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(Archivo))
                {
                    Directory.CreateDirectory(Archivo);
                }
                ArchivoChampions = Archivo + Path.GetFileName(PostChampions.FileName);
                string extension = Path.GetExtension(PostChampions.FileName);
                PostChampions.SaveAs(ArchivoChampions);
                string csvData = System.IO.File.ReadAllText(ArchivoChampions);
                int numeroAux = 0;
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        if (numeroAux != 0)
                        {
                            String[] fields = row.Split('|');
                            string Llave = fields[0];
                            string Faltantes = fields[1];
                            if (!string.IsNullOrEmpty(Faltantes))
                            {
                                foreach (var item in Faltantes.Split(','))
                                {
                                    int Valor = Convert.ToInt32(item);
                                    Datos.Instance.ListaFaltantesCont.Add(Valor);
                                }
                            }
                            string Coleccionadas = fields[2];
                            if (!string.IsNullOrEmpty(Coleccionadas))
                            {
                                foreach (var item in Coleccionadas.Split(','))
                                {
                                    int Valor = Convert.ToInt32(item);
                                    Datos.Instance.ListaColeccionadasCont.Add(Valor);
                                }
                            }
                            string Cambios = fields[3];
                            if (!string.IsNullOrEmpty(Cambios))
                            {
                                foreach (var item in Cambios.Split(','))
                                {
                                    int Valor = Convert.ToInt32(item);
                                    Datos.Instance.ListaCambioCont.Add(Valor);
                                }
                            }
                            dynamic expandObject = new ExpandoObject();
                            expandObject.ListaFaltantes = Datos.Instance.ListaFaltantesCont;
                            expandObject.ListaColeccionadas = Datos.Instance.ListaColeccionadasCont;
                            expandObject.ListaCambios = Datos.Instance.ListaCambioCont;
                            Datos.Instance.diccionarioEstampasAlbum.Add(Llave,expandObject);
                        }
                        numeroAux++;
                    }
                }
            }
            return RedirectToAction("IndexEquipo");
        }
        public ActionResult TablaAlbum()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TablaAlbum(FormCollection collection)
        {
            return View();
        }
        // GET: Album/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Album/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Album/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Album/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: Album/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Album/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Album/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
