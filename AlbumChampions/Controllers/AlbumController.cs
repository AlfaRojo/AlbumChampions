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
        public ActionResult Menu()
        {
            return View();
        }
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
                        bool EstampaColeccionada = false;
                        if (numeroAux != 0)
                        {
                            String[] campos = fila.Split('|');
                            string identificador = campos[0];
                            string coleccionada = campos[1];
                            if (coleccionada == "true\r")
                            {
                                EstampaColeccionada = true;
                            }
                            else if (coleccionada == "false\r")
                            {
                                EstampaColeccionada = false;
                            }
                            Datos.Instance.diccionarioColeccionada.Add(identificador, EstampaColeccionada);
                        }
                        numeroAux++;
                    }
                }
            }
            return RedirectToAction("Menu");
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
                            ViewBag.Llaves = Llave.Count();
                            string Faltantes = fields[1];
                            var ListRemaining = new List<int>();
                            var ListCollected = new List<int>();
                            var TradingList = new List<int>();
                            if (!string.IsNullOrEmpty(Faltantes))
                            {
                                foreach (var item in Faltantes.Split(','))
                                {
                                    int Valor = Convert.ToInt32(item);
                                    ListRemaining.Add(Valor);
                                }
                            }
                            string Coleccionadas = fields[2];
                            if (!string.IsNullOrEmpty(Coleccionadas))
                            {
                                foreach (var item in Coleccionadas.Split(','))
                                {
                                    int Valor = Convert.ToInt32(item);
                                    ListCollected.Add(Valor);
                                }
                            }
                            string Cambios = fields[3];
                            if (!string.IsNullOrEmpty(Cambios))
                            {
                                foreach (var item in Cambios.Split(','))
                                {
                                    int Valor = Convert.ToInt32(item);
                                    TradingList.Add(Valor);
                                }
                            }
                            Datos.Instance.diccionarioEstampasAlbum.Add(Llave, new Album
                            {
                                ListaFaltantes = ListRemaining,
                                ListaColeccionadas = ListCollected,
                                ListaCambios = TradingList,
                            });
                        }
                        numeroAux++;
                    }
                }
            }
            return RedirectToAction("IndexEquipo");
        }
        public ActionResult BusquedaTipo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BusquedaTipo(Busqueda collection)
        {
            string Estampa = collection.Identificador;
            return RedirectToAction("BusquedaTipoEstampa", new { TipoEstampaABuscar = Estampa });
        }
        public ActionResult BusquedaTipoEstampa(string TipoEstampaABuscar)
        {
            try
            {
                foreach (var author in Datos.Instance.diccionarioEstampasAlbum)
                {
                    if (author.Key == TipoEstampaABuscar)
                    {
                        var result = String.Join(",", author.Value.ListaFaltantes.ToArray());
                        Datos.Instance.ListaAlbumMostrar.Add(new AlbumMostrar
                        {
                            TipoEstampa = author.Key,
                            Faltantes = result
                        });
                    }
                }
                return RedirectToAction("TablaEstadoAlbum");
            }
            catch (Exception)
            {
                throw new DriveNotFoundException();
            }
        }
        public ActionResult TablaEstadoAlbum()
        {
            return View(Datos.Instance.ListaAlbumMostrar);
        }
        public ActionResult BusquedaColec()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BusquedaColec(Busqueda collection)
        {            
            string Estampa = collection.Identificador;
            return RedirectToAction("BusquedaColeccion", new { TipoEstampaABuscar = Estampa });
        }
        public ActionResult BusquedaColeccion(string TipoEstampaABuscar)
        {
            try
            {
                foreach (KeyValuePair<string, bool> author in Datos.Instance.diccionarioColeccionada)
                {
                    if (author.Key.Contains(TipoEstampaABuscar))
                    {
                        if (author.Value)
                        {
                            Datos.Instance.ListaEquipoMostrar.Add(new EquipoMostrar
                            {
                                TipoEstampa = author.Key,
                                YaEnColeccion = "true"
                            });
                        }
                        else
                        {
                            Datos.Instance.ListaEquipoMostrar.Add(new EquipoMostrar
                            {
                                TipoEstampa = author.Key,
                                YaEnColeccion = "false"
                            });
                        }
                    }
                }
                return RedirectToAction("TablaColeccionMostrar");
            }
            catch (Exception)
            {
                throw new DriveNotFoundException();
            }
        }
        public ActionResult TablaColeccionMostrar()
        {
            return View(Datos.Instance.ListaEquipoMostrar);
        }
        public ActionResult AgregarEstampa()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AgregarEstampa(Busqueda collection)
        {
            string TipoEstampa = collection.Identificador;
            string Numero = collection.NumeroEstampa;
            return RedirectToAction("AgregarEstampaALista", new { TipoEstampaABuscar = TipoEstampa, NumeroEstampa=Numero });
        }
        public ActionResult AgregarEstampaALista(string TipoEstampaABuscar, string NumeroEstampa)
        {
            try
            {
                foreach (var author in Datos.Instance.diccionarioEstampasAlbum)
                {
                    if (author.Key == TipoEstampaABuscar)
                    {
                        int NumeroABuscar = Convert.ToInt32(NumeroEstampa);
                        if (author.Value.ListaFaltantes.Contains(NumeroABuscar))
                        {
                            author.Value.ListaFaltantes.Remove(NumeroABuscar);
                            author.Value.ListaColeccionadas.Add(NumeroABuscar);
                            author.Value.ListaColeccionadas.Sort();
                        }
                        var result = String.Join(",", author.Value.ListaFaltantes.ToArray());
                        foreach (var item in Datos.Instance.ListaAlbumMostrar)
                        {
                            if (item.TipoEstampa == TipoEstampaABuscar)
                            {
                                item.Faltantes = result;
                            }                           
                        }
                        
                    }
                }
                string IdentificadorEstampaEspecifica = TipoEstampaABuscar + "_" + NumeroEstampa;
                if (Datos.Instance.diccionarioColeccionada.ContainsKey(IdentificadorEstampaEspecifica))
                {
                    Datos.Instance.diccionarioColeccionada.Remove(IdentificadorEstampaEspecifica);
                    Datos.Instance.diccionarioColeccionada.Add(IdentificadorEstampaEspecifica, true);
                }
                foreach (var item in Datos.Instance.ListaEquipoMostrar)
                {
                    if (item.TipoEstampa== IdentificadorEstampaEspecifica)
                    {
                        item.YaEnColeccion = "true";
                    }
                }
                return RedirectToAction("TablaEstadoAlbum");
            }
            catch (Exception)
            {
                throw new DriveNotFoundException();
            }
        }
    }
}
