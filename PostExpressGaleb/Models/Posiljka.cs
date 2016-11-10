using System;

namespace PostExpressGaleb.Models
{
    class Posiljka
    {
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
