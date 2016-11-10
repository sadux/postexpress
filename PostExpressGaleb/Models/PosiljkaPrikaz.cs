using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostExpressGaleb.Models
{
    class PosiljkaPrikaz
    {
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public string PostanskiBroj { get; set; }
        public string Mesto { get; set; }
        public string KontaktOsoba { get; set; }
        public string Telefon { get; set; }
        public int PosiljkaId { get; set; }
        public int PrimalacId { get; set; }
        public decimal Vrednost { get; set; }
        public decimal Otkupnina { get; set; }
        public decimal Masa { get; set; }
        public string Sadrzaj { get; set; }
        public string PAK { get; set; }
        public DateTime DatumVreme { get; set; }
    }
}
