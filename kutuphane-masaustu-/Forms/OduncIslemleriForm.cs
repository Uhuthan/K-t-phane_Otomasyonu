using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KutuphaneOtomasyonu.Data;
using KutuphaneOtomasyonu.Models;

namespace KutuphaneOtomasyonu.Forms
{
    public partial class OduncIslemleriForm : Form
    {
        private VeritabaniIslemleri _db;
        private DataGridView dataGridViewIslemler;
        private Button buttonYeniOdunc;
        private Button buttonIadeAl;
        private Button buttonKapat;

        public OduncIslemleriForm(VeritabaniIslemleri db)
        {
            _db = db;
            InitializeComponent();
        }

        private void OduncIslemleriForm_Load(object sender, EventArgs e)
        {
            IslemleriYukle();
        }

        private void IslemleriYukle()
        {
            try
            {
                List<OduncIslemi> islemler = _db.TumOduncIslemleriniGetir();
                List<Kitap> kitaplar = _db.TumKitaplariGetir();
                List<Uye> uyeler = _db.TumUyeleriGetir();

                var veri = new List<dynamic>();
                DateTime bugun = DateTime.Today;
                decimal gunlukCeza = 5m;

                foreach (var islem in islemler)
                {
                 
                    decimal hesaplananCeza = 0m;
                    bool geciktiMiLocal = false;

                    if (islem.Gecikti && islem.IadeTarihi.Date < bugun)
                    {
                        int gecikenGun = (bugun - islem.IadeTarihi.Date).Days;
                        hesaplananCeza = gecikenGun * gunlukCeza;
                        geciktiMiLocal = true;
                    }


                    var kitap = kitaplar.Find(k => k.KitapId == islem.KitapId);
                    var uye = uyeler.Find(u => u.UyeId == islem.UyeId);

                    veri.Add(new
                    {
                        IslemId = islem.IslemId,
                        KitapAdi = kitap?.Ad ?? "Bilinmiyor",
                        UyeAdi = uye?.AdSoyad ?? "Bilinmiyor",
                        OduncTarihi = islem.OduncTarihi.ToShortDateString(),
                        IadeTarihi = islem.IadeTarihi.ToShortDateString(),
                        Durum = islem.Durum,
                        Ceza = hesaplananCeza.ToString("₺0.00"),
                        Gecikti = geciktiMiLocal ? "Evet" : "Hayır"

                    });
                }

                dataGridViewIslemler.DataSource = veri;
                dataGridViewIslemler.AutoResizeColumns();
               

            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlemler yüklenirken hata: " + ex.Message);
            }
        }

        private void buttonYeniOdunc_Click(object sender, EventArgs e)
        {
            YeniOduncForm yeniForm = new YeniOduncForm(_db);
            if (yeniForm.ShowDialog() == DialogResult.OK)
            {
                IslemleriYukle();
            }
        }

        private void buttonIadeAl_Click(object sender, EventArgs e)
        {
            if (dataGridViewIslemler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen iade almak için bir işlem seçiniz.");
                return;
            }

            try
            {
                int islemId = Convert.ToInt32(
                    dataGridViewIslemler.SelectedRows[0].Cells["IslemId"].Value);

                List<OduncIslemi> islemler = _db.TumOduncIslemleriniGetir();
                OduncIslemi islem = islemler.Find(i => i.IslemId == islemId);

                if (islem != null && islem.Durum == "Aktif")
                {
                    islem.GercekIadeTarihi = DateTime.Now;
                    islem.Durum = "Tamamlandı";

                    _db.OduncIslemGuncelle(islem);

                    List<Kitap> kitaplar = _db.TumKitaplariGetir();
                    Kitap kitap = kitaplar.Find(k => k.KitapId == islem.KitapId);
                    if (kitap != null)
                    {
                        kitap.Durum = "Mevcut";
                        _db.KitapGuncelle(kitap);
                    }

                    MessageBox.Show("İade alındı.");
                    IslemleriYukle();
                }
                else
                {
                    MessageBox.Show("Bu işlem zaten tamamlanmış.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İade alınırken hata: " + ex.Message);
            }
        }

        private void buttonKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.Text = "Ödünç İşlemleri";
            this.Size = new System.Drawing.Size(1100, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            dataGridViewIslemler = new DataGridView();
            dataGridViewIslemler.Dock = DockStyle.Fill;
            dataGridViewIslemler.AllowUserToAddRows = false;
            dataGridViewIslemler.AllowUserToDeleteRows = false;
            dataGridViewIslemler.ReadOnly = true;
            dataGridViewIslemler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewIslemler.MultiSelect = false;

            Panel panelButonlar = new Panel();
            panelButonlar.Height = 50;
            panelButonlar.Dock = DockStyle.Bottom;

            buttonYeniOdunc = new Button();
            buttonYeniOdunc.Text = "Yeni Ödünç";
            buttonYeniOdunc.Location = new System.Drawing.Point(10, 10);
            buttonYeniOdunc.Size = new System.Drawing.Size(120, 30);
            buttonYeniOdunc.Click += buttonYeniOdunc_Click;

            buttonIadeAl = new Button();
            buttonIadeAl.Text = "İade Al";
            buttonIadeAl.Location = new System.Drawing.Point(140, 10);
            buttonIadeAl.Size = new System.Drawing.Size(120, 30);
            buttonIadeAl.Click += buttonIadeAl_Click;

            buttonKapat = new Button();
            buttonKapat.Text = "Kapat";
            buttonKapat.Location = new System.Drawing.Point(970, 10);
            buttonKapat.Size = new System.Drawing.Size(100, 30);
            buttonKapat.Click += buttonKapat_Click;

            panelButonlar.Controls.Add(buttonYeniOdunc);
            panelButonlar.Controls.Add(buttonIadeAl);
            panelButonlar.Controls.Add(buttonKapat);

            this.Controls.Add(dataGridViewIslemler);
            this.Controls.Add(panelButonlar);
        }
    }
}
