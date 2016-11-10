namespace PostExpressGaleb.Models
{
    class Primalac
    {
        public int PrimalacId { get; set; }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public string PostanskiBroj { get; set; }
        public string Mesto { get; set; }
        public string KontaktOsoba { get; set; }
        public string Telefon { get; set; }

        public override string ToString()
        {
            return Naziv;
        }
    }
}
