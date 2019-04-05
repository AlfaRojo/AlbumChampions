using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AlbumChampions.Models
{
    public class EquipoMostrar
    {
        [Display(Name = "Nombre de Estampa")]
        public string TipoEstampa { get; set; }
        [Display(Name = "Coleccionada")]
        public string YaEnColeccion { get; set; }
    }
}