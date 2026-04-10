using System;

namespace KutuphaneOtomasyonu.Models
{
    public class Kitap
    {
        public int KitapId { get; set; }
        public string Ad { get; set; }
        public string Yazar { get; set; }
        public string Yayinevi { get; set; }
        public int YayinYili { get; set; }
        public string ISBN { get; set; }
        public string Kategori { get; set; }
        public string Durum { get; set; }
        public DateTime OlusturmaTarihi { get; set; }

        public Kitap()
        {
            OlusturmaTarihi = DateTime.Now;
            Durum = "Mevcut";
        }

        public override string ToString()
        {
            return $"{Ad} - {Yazar}";
        }
    }
}
