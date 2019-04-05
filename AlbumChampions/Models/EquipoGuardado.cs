using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumChampions.Models
{
    public class EquipoGuardado : IComparable
    {
        public string Equipo { get; set; }
        public string Estampas { get; set; }

        public static Comparison<EquipoGuardado> PorNombre = delegate (EquipoGuardado s1, EquipoGuardado s2)
        {
            return s1.Equipo.CompareTo(s2.Equipo);
        };
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}