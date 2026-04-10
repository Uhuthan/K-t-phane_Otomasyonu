using System;

namespace KutuphaneOtomasyonu.Models
{
    public class Uye
    {
        public int UyeId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string UyeNo { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public decimal ToplamCeza { get; set; }
        public DateTime OlusturmaTarihi { get; set; }

        public Uye()
        {
            OlusturmaTarihi = DateTime.Now;
            ToplamCeza = 0;
        }

        public string AdSoyad
        {
            get { return $"{Ad} {Soyad}"; }
        }

        public override string ToString()
        {
            return AdSoyad;
        }
    }
}
