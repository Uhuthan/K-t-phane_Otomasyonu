using System;

namespace KutuphaneOtomasyonu.Models
{
    public class OduncIslemi
    {
        public int IslemId { get; set; }
        public int KitapId { get; set; }
        public int UyeId { get; set; }
        public DateTime OduncTarihi { get; set; }
        public DateTime IadeTarihi { get; set; }
        public DateTime? GercekIadeTarihi { get; set; }
        public decimal Ceza { get; set; }
        public string Durum { get; set; }

        public OduncIslemi()
        {
            OduncTarihi = DateTime.Now;
            Ceza = 0;
            Durum = "Aktif";
        }

        public bool Gecikti
        {
            get
            {
                if (GercekIadeTarihi.HasValue)
                    return false;
                return DateTime.Now > IadeTarihi;
            }
        }

        public int GecikmeGunu
        {
            get
            {
                if (!Gecikti)
                    return 0;
                TimeSpan fark = DateTime.Now - IadeTarihi;
                return fark.Days;
            }
        }
    }
}
