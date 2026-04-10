using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using KutuphaneOtomasyonu.Data;
using KutuphaneOtomasyonu.Models;

namespace KutuphaneOtomasyonu.Forms
{
    public partial class RaporlarForm : Form
    {
        private VeritabaniIslemleri _db;
        private TabControl tabControl;
        private Button buttonKapat;

        public RaporlarForm(VeritabaniIslemleri db)
        {
            _db = db;
            InitializeComponent();
        }

        private void RaporlarForm_Load(object sender, EventArgs e)
        {
            GecikmiKitaplarRaporuGoster();
        }

        private void GecikmiKitaplarRaporuGoster()
        {
            try
            {
                List<OduncIslemi> islemler = _db.TumOduncIslemleriniGetir();
                List<Kitap> kitaplar = _db.TumKitaplariGetir();
                List<Uye> uyeler = _db.TumUyeleriGetir();

                var gecikmiIslemler = islemler.Where(i => i.Gecikti && i.Durum == "Aktif").ToList();

                var veri = new List<dynamic>();
                foreach (var islem in gecikmiIslemler)
                {
                    var kitap = kitaplar.Find(k => k.KitapId == islem.KitapId);
                    var uye = uyeler.Find(u => u.UyeId == islem.UyeId);

                    veri.Add(new
                    {
                        KitapAdi = kitap?.Ad ?? "Bilinmiyor",
                        UyeAdi = uye?.AdSoyad ?? "Bilinmiyor",
                        OduncTarihi = islem.OduncTarihi.ToShortDateString(),
                        IadeTarihi = islem.IadeTarihi.ToShortDateString(),
                        GecikmeGunu = islem.GecikmeGunu,
                        TahminiCeza = (islem.GecikmeGunu * 5).ToString("C2")
                    });
                }

                DataGridView dgv = (DataGridView)tabControl.TabPages[0].Controls[0];
                dgv.DataSource = veri;
                dgv.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Rapor oluşturulurken hata: " + ex.Message);
            }
        }

        private void EnCokOduncAlinanKitaplarRaporuGoster()
        {
            try
            {
                List<OduncIslemi> islemler = _db.TumOduncIslemleriniGetir();
                List<Kitap> kitaplar = _db.TumKitaplariGetir();

                var kitapOduncSayilari = islemler.GroupBy(i => i.KitapId)
                    .Select(g => new { KitapId = g.Key, OduncSayisi = g.Count() })
                    .OrderByDescending(x => x.OduncSayisi)
                    .Take(10)
                    .ToList();

                var veri = new List<dynamic>();
                foreach (var item in kitapOduncSayilari)
                {
                    var kitap = kitaplar.Find(k => k.KitapId == item.KitapId);
                    veri.Add(new
                    {
                        KitapAdi = kitap?.Ad ?? "Bilinmiyor",
                        Yazar = kitap?.Yazar ?? "Bilinmiyor",
                        OduncSayisi = item.OduncSayisi
                    });
                }

                DataGridView dgv = (DataGridView)tabControl.TabPages[1].Controls[0];
                dgv.DataSource = veri;
                dgv.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Rapor oluşturulurken hata: " + ex.Message);
            }
        }

        private void EnAktifUyelerRaporuGoster()
        {
            try
            {
                List<OduncIslemi> islemler = _db.TumOduncIslemleriniGetir();
                List<Uye> uyeler = _db.TumUyeleriGetir();

                var uyeOduncSayilari = islemler.GroupBy(i => i.UyeId)
                    .Select(g => new { UyeId = g.Key, OduncSayisi = g.Count() })
                    .OrderByDescending(x => x.OduncSayisi)
                    .Take(10)
                    .ToList();

                var veri = new List<dynamic>();
                foreach (var item in uyeOduncSayilari)
                {
                    var uye = uyeler.Find(u => u.UyeId == item.UyeId);
                    veri.Add(new
                    {
                        UyeAdi = uye?.AdSoyad ?? "Bilinmiyor",
                        UyeNo = uye?.UyeNo ?? "Bilinmiyor",
                        OduncSayisi = item.OduncSayisi
                    });
                }

                DataGridView dgv = (DataGridView)tabControl.TabPages[2].Controls[0];
                dgv.DataSource = veri;
                dgv.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Rapor oluşturulurken hata: " + ex.Message);
            }
        }

        private void CezaliUyelerRaporuGoster()
        {
            try
            {
                List<Uye> uyeler = _db.TumUyeleriGetir();

                var cezaliUyeler = uyeler.Where(u => u.ToplamCeza > 0)
                    .OrderByDescending(u => u.ToplamCeza)
                    .ToList();

                var veri = new List<dynamic>();
                foreach (var uye in cezaliUyeler)
                {
                    veri.Add(new
                    {
                        UyeAdi = uye.AdSoyad,
                        UyeNo = uye.UyeNo,
                        Email = uye.Email,
                        ToplamCeza = uye.ToplamCeza.ToString("C2")
                    });
                }

                DataGridView dgv = (DataGridView)tabControl.TabPages[3].Controls[0];
                dgv.DataSource = veri;
                dgv.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Rapor oluşturulurken hata: " + ex.Message);
            }
        }

        private void GeelIstatistiklerRaporuGoster()
        {
            try
            {
                List<Kitap> kitaplar = _db.TumKitaplariGetir();
                List<Uye> uyeler = _db.TumUyeleriGetir();
                List<OduncIslemi> islemler = _db.TumOduncIslemleriniGetir();

                int mevcutKitap = kitaplar.Count(k => k.Durum == "Mevcut");
                int odunctekiKitap = kitaplar.Count(k => k.Durum == "Ödünçte");
                int cezaliUye = uyeler.Count(u => u.ToplamCeza > 0);
                decimal toplamCeza = uyeler.Sum(u => u.ToplamCeza);
                int aktifOdunc = islemler.Count(i => i.Durum == "Aktif");
                int gecikmiOdunc = islemler.Count(i => i.Gecikti && i.Durum == "Aktif");

                var veri = new List<dynamic>
                {
                    new { Metrik = "Toplam Kitap Sayısı", Değer = kitaplar.Count },
                    new { Metrik = "Mevcut Kitap Sayısı", Değer = mevcutKitap },
                    new { Metrik = "Ödünçte Olan Kitap Sayısı", Değer = odunctekiKitap },
                    new { Metrik = "Toplam Üye Sayısı", Değer = uyeler.Count },
                    new { Metrik = "Cezalı Üye Sayısı", Değer = cezaliUye },
                    new { Metrik = "Toplam Ceza Tutarı", Değer = toplamCeza.ToString("C2") },
                    new { Metrik = "Aktif Ödünç İşlemi", Değer = aktifOdunc },
                    new { Metrik = "Gecikmiş Ödünç İşlemi", Değer = gecikmiOdunc }
                };

                DataGridView dgv = (DataGridView)tabControl.TabPages[4].Controls[0];
                dgv.DataSource = veri;
                dgv.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Rapor oluşturulurken hata: " + ex.Message);
            }
        }

        private void buttonKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.Text = "Raporlar";
            this.Size = new System.Drawing.Size(1100, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            string[] tabBasliklari = { "Gecikmiş Kitaplar", "En Çok Ödünç Alınan", "En Aktif Üyeler", "Cezalı Üyeler", "Genel İstatistikler" };

            foreach (string baslik in tabBasliklari)
            {
                TabPage tab = new TabPage(baslik);
                DataGridView dgv = new DataGridView();
                dgv.Dock = DockStyle.Fill;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.ReadOnly = true;
                tab.Controls.Add(dgv);
                tabControl.TabPages.Add(tab);
            }

            tabControl.SelectedIndexChanged += (s, e) =>
            {
                if (tabControl.SelectedIndex == 0)
                    GecikmiKitaplarRaporuGoster();
                else if (tabControl.SelectedIndex == 1)
                    EnCokOduncAlinanKitaplarRaporuGoster();
                else if (tabControl.SelectedIndex == 2)
                    EnAktifUyelerRaporuGoster();
                else if (tabControl.SelectedIndex == 3)
                    CezaliUyelerRaporuGoster();
                else if (tabControl.SelectedIndex == 4)
                    GeelIstatistiklerRaporuGoster();
            };

            Panel panelButonlar = new Panel();
            panelButonlar.Height = 50;
            panelButonlar.Dock = DockStyle.Bottom;
            panelButonlar.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

            buttonKapat = new Button();
            buttonKapat.Text = "Kapat";
            buttonKapat.Location = new System.Drawing.Point(1000, 10);
            buttonKapat.Size = new System.Drawing.Size(100, 30);
            buttonKapat.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            buttonKapat.ForeColor = System.Drawing.Color.White;
            buttonKapat.Click += buttonKapat_Click;

            panelButonlar.Controls.Add(buttonKapat);

            this.Controls.Add(tabControl);
            this.Controls.Add(panelButonlar);
        }
    }
}
