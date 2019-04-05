using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AlbumChampions.Models
{
    public class AlbumMostrar
    {
        [Display(Name = "Tipo de Estampa")]
        public string TipoEstampa{get;set;}
        public string Faltantes { get; set; }
    }
}