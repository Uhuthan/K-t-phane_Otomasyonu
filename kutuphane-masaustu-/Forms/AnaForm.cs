using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KutuphaneOtomasyonu.Data;
using KutuphaneOtomasyonu.Models;

namespace KutuphaneOtomasyonu.Forms
{
    public partial class AnaForm : Form
    {
        private VeritabaniIslemleri _db;

        public AnaForm()
        {
            InitializeComponent();
            _db = new VeritabaniIslemleri();
        }

        private void AnaForm_Load(object sender, EventArgs e)
        {
            try
            {
                _db.VeritabaniOlustur();
                IstatistikleriGuncelle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IstatistikleriGuncelle()
        {
            try
            {
                List<Kitap> kitaplar = _db.TumKitaplariGetir();
                List<Uye> uyeler = _db.TumUyeleriGetir();
                List<OduncIslemi> islemler = _db.TumOduncIslemleriniGetir();

                int mevcutKitap = 0;
                int odunctekiKitap = 0;
                foreach (var kitap in kitaplar)
                {
                    if (kitap.Durum == "Mevcut")
                        mevcutKitap++;
                    else
                        odunctekiKitap++;
                }

                int cezaliUye = 0;
                foreach (var uye in uyeler)
                {
                    if (uye.ToplamCeza > 0)
                        cezaliUye++;
                }

                int gecikmiIslem = 0;
                int aktifIslem = 0;
                foreach (var islem in islemler)
                {
                    if (islem.Durum == "Aktif")
                    {
                        aktifIslem++;
                        if (islem.Gecikti)
                            gecikmiIslem++;
                    }
                }

                labelToplamKitap.Text = kitaplar.Count.ToString();
                labelMevcutKitap.Text = mevcutKitap.ToString();
                labelOdunctekiKitap.Text = odunctekiKitap.ToString();
                labelToplamUye.Text = uyeler.Count.ToString();
                labelCezaliUye.Text = cezaliUye.ToString();
                labelAktifOdunc.Text = aktifIslem.ToString();
                labelGecikmiOdunc.Text = gecikmiIslem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("İstatistikler güncellenirken hata: " + ex.Message);
            }
        }

        private void buttonKitaplar_Click(object sender, EventArgs e)
        {
            KitaplarForm kitaplarForm = new KitaplarForm(_db);
            kitaplarForm.ShowDialog();
            IstatistikleriGuncelle();
        }

        private void buttonUyeler_Click(object sender, EventArgs e)
        {
            UyelerForm uyelerForm = new UyelerForm(_db);
            uyelerForm.ShowDialog();
            IstatistikleriGuncelle();
        }

        private void buttonOdunc_Click(object sender, EventArgs e)
        {
            OduncIslemleriForm oduncForm = new OduncIslemleriForm(_db);
            oduncForm.ShowDialog();
            IstatistikleriGuncelle();
        }

        private void buttonRaporlar_Click(object sender, EventArgs e)
        {
            RaporlarForm raporlarForm = new RaporlarForm(_db);
            raporlarForm.ShowDialog();
        }

        private void InitializeComponent()
        {
            this.Text = "Kütüphane Otomasyonu";
            this.Size = new System.Drawing.Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.White;

            Panel panelBaslik = new Panel();
            panelBaslik.BackColor = System.Drawing.Color.FromArgb(30, 90, 160);
            panelBaslik.Height = 60;
            panelBaslik.Dock = DockStyle.Top;

            Label labelBaslik = new Label();
            labelBaslik.Text = "Kütüphane Otomasyonu Sistemi";
            labelBaslik.ForeColor = System.Drawing.Color.White;
            labelBaslik.Font = new System.Drawing.Font("Arial", 18, System.Drawing.FontStyle.Bold);
            labelBaslik.Dock = DockStyle.Fill;
            labelBaslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            panelBaslik.Controls.Add(labelBaslik);

            Panel panelIstatistikler = new Panel();
            panelIstatistikler.Height = 150;
            panelIstatistikler.Dock = DockStyle.Top;
            panelIstatistikler.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

            Label labelBaslikIstatistik = new Label();
            labelBaslikIstatistik.Text = "Genel İstatistikler";
            labelBaslikIstatistik.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            labelBaslikIstatistik.Location = new System.Drawing.Point(10, 10);
            labelBaslikIstatistik.Size = new System.Drawing.Size(200, 20);

            Label labelKitapBaslik = new Label();
            labelKitapBaslik.Text = "Kitaplar:";
            labelKitapBaslik.Location = new System.Drawing.Point(10, 40);
            labelKitapBaslik.Size = new System.Drawing.Size(100, 20);

            labelToplamKitap = new Label();
            labelToplamKitap.Text = "0";
            labelToplamKitap.Location = new System.Drawing.Point(120, 40);
            labelToplamKitap.Size = new System.Drawing.Size(50, 20);

            labelMevcutKitap = new Label();
            labelMevcutKitap.Text = "0";
            labelMevcutKitap.Location = new System.Drawing.Point(180, 40);
            labelMevcutKitap.Size = new System.Drawing.Size(50, 20);

            labelOdunctekiKitap = new Label();
            labelOdunctekiKitap.Text = "0";
            labelOdunctekiKitap.Location = new System.Drawing.Point(240, 40);
            labelOdunctekiKitap.Size = new System.Drawing.Size(50, 20);

            Label labelUyeBaslik = new Label();
            labelUyeBaslik.Text = "Üyeler:";
            labelUyeBaslik.Location = new System.Drawing.Point(10, 70);
            labelUyeBaslik.Size = new System.Drawing.Size(100, 20);

            labelToplamUye = new Label();
            labelToplamUye.Text = "0";
            labelToplamUye.Location = new System.Drawing.Point(120, 70);
            labelToplamUye.Size = new System.Drawing.Size(50, 20);

            labelCezaliUye = new Label();
            labelCezaliUye.Text = "0";
            labelCezaliUye.Location = new System.Drawing.Point(180, 70);
            labelCezaliUye.Size = new System.Drawing.Size(50, 20);

            Label labelOduncBaslik = new Label();
            labelOduncBaslik.Text = "Ödünçler:";
            labelOduncBaslik.Location = new System.Drawing.Point(10, 100);
            labelOduncBaslik.Size = new System.Drawing.Size(100, 20);

            labelAktifOdunc = new Label();
            labelAktifOdunc.Text = "0";
            labelAktifOdunc.Location = new System.Drawing.Point(120, 100);
            labelAktifOdunc.Size = new System.Drawing.Size(50, 20);

            labelGecikmiOdunc = new Label();
            labelGecikmiOdunc.Text = "0";
            labelGecikmiOdunc.Location = new System.Drawing.Point(180, 100);
            labelGecikmiOdunc.Size = new System.Drawing.Size(50, 20);

            panelIstatistikler.Controls.Add(labelBaslikIstatistik);
            panelIstatistikler.Controls.Add(labelKitapBaslik);
            panelIstatistikler.Controls.Add(labelToplamKitap);
            panelIstatistikler.Controls.Add(labelMevcutKitap);
            panelIstatistikler.Controls.Add(labelOdunctekiKitap);
            panelIstatistikler.Controls.Add(labelUyeBaslik);
            panelIstatistikler.Controls.Add(labelToplamUye);
            panelIstatistikler.Controls.Add(labelCezaliUye);
            panelIstatistikler.Controls.Add(labelOduncBaslik);
            panelIstatistikler.Controls.Add(labelAktifOdunc);
            panelIstatistikler.Controls.Add(labelGecikmiOdunc);

            Panel panelButonlar = new Panel();
            panelButonlar.Height = 80;
            panelButonlar.Dock = DockStyle.Top;
            panelButonlar.BackColor = System.Drawing.Color.White;

            buttonKitaplar = new Button();
            buttonKitaplar.Text = "Kitaplar";
            buttonKitaplar.Location = new System.Drawing.Point(20, 15);
            buttonKitaplar.Size = new System.Drawing.Size(120, 35);
            buttonKitaplar.BackColor = System.Drawing.Color.FromArgb(30, 90, 160);
            buttonKitaplar.ForeColor = System.Drawing.Color.White;
            buttonKitaplar.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            buttonKitaplar.Click += buttonKitaplar_Click;

            buttonUyeler = new Button();
            buttonUyeler.Text = "Üyeler";
            buttonUyeler.Location = new System.Drawing.Point(160, 15);
            buttonUyeler.Size = new System.Drawing.Size(120, 35);
            buttonUyeler.BackColor = System.Drawing.Color.FromArgb(30, 90, 160);
            buttonUyeler.ForeColor = System.Drawing.Color.White;
            buttonUyeler.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            buttonUyeler.Click += buttonUyeler_Click;

            buttonOdunc = new Button();
            buttonOdunc.Text = "Ödünç İşlemleri";
            buttonOdunc.Location = new System.Drawing.Point(300, 15);
            buttonOdunc.Size = new System.Drawing.Size(120, 35);
            buttonOdunc.BackColor = System.Drawing.Color.FromArgb(30, 90, 160);
            buttonOdunc.ForeColor = System.Drawing.Color.White;
            buttonOdunc.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            buttonOdunc.Click += buttonOdunc_Click;

            buttonRaporlar = new Button();
            buttonRaporlar.Text = "Raporlar";
            buttonRaporlar.Location = new System.Drawing.Point(440, 15);
            buttonRaporlar.Size = new System.Drawing.Size(120, 35);
            buttonRaporlar.BackColor = System.Drawing.Color.FromArgb(30, 90, 160);
            buttonRaporlar.ForeColor = System.Drawing.Color.White;
            buttonRaporlar.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            buttonRaporlar.Click += buttonRaporlar_Click;

            panelButonlar.Controls.Add(buttonKitaplar);
            panelButonlar.Controls.Add(buttonUyeler);
            panelButonlar.Controls.Add(buttonOdunc);
            panelButonlar.Controls.Add(buttonRaporlar);

            this.Controls.Add(panelButonlar);
            this.Controls.Add(panelIstatistikler);
            this.Controls.Add(panelBaslik);
        }

        private Button buttonKitaplar;
        private Button buttonUyeler;
        private Button buttonOdunc;
        private Button buttonRaporlar;
        private Label labelToplamKitap;
        private Label labelMevcutKitap;
        private Label labelOdunctekiKitap;
        private Label labelToplamUye;
        private Label labelCezaliUye;
        private Label labelAktifOdunc;
        private Label labelGecikmiOdunc;
    }
}
