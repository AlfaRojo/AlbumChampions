using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlbumChampions.Models;
namespace AlbumChampions.Helpers
{
    public class Datos
    {
        private static Datos _instance = null;
        public static Datos Instance
        {
            get
            {
                if (_instance == null) _instance = new Datos();
                {
                    return _instance;
                }
            }
        }
        //public List<int> ListaFaltantesCont = new List<int>();
        //public List<int> ListaColeccionadasCont = new List<int>();
        //public List<int> ListaCambioCont = new List<int>();
        public Dictionary<string,Album> diccionarioEstampasAlbum = new Dictionary<string, Album>();
        public Dictionary<string, bool> diccionarioColeccionada = new Dictionary<string, bool>();
        public List<AlbumMostrar> ListaAlbumMostrar = new List<AlbumMostrar>();
        public List<EquipoMostrar> ListaEquipoMostrar = new List<EquipoMostrar>();

    }
}