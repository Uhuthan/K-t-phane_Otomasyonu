using System;
using System.Windows.Forms;
using KutuphaneOtomasyonu.Data;
using KutuphaneOtomasyonu.Models;

namespace KutuphaneOtomasyonu.Forms
{
    public partial class KitapEkleForm : Form
    {
        private VeritabaniIslemleri _db;
        private Kitap _kitap;
        private TextBox textBoxAd;
        private TextBox textBoxYazar;
        private TextBox textBoxYayinevi;
        private TextBox textBoxYayinYili;
        private TextBox textBoxISBN;
        private ComboBox comboBoxKategori;
        private Button buttonKaydet;
        private Button buttonIptal;

        public KitapEkleForm(VeritabaniIslemleri db, Kitap kitap)
        {
            _db = db;
            _kitap = kitap;
            InitializeComponent();
        }

        private void KitapEkleForm_Load(object sender, EventArgs e)
        {
            if (_kitap != null)
            {
                this.Text = "Kitap Düzenle";
                textBoxAd.Text = _kitap.Ad;
                textBoxYazar.Text = _kitap.Yazar;
                textBoxYayinevi.Text = _kitap.Yayinevi;
                textBoxYayinYili.Text = _kitap.YayinYili.ToString();
                textBoxISBN.Text = _kitap.ISBN;
                comboBoxKategori.SelectedItem = _kitap.Kategori;
            }
        }

        private void buttonKaydet_Click(object sender, EventArgs e)
        {
            if (!FormDogrula())
                return;

            try
            {
                if (_kitap == null)
                {
                    _kitap = new Kitap();
                }

                _kitap.Ad = textBoxAd.Text;
                _kitap.Yazar = textBoxYazar.Text;
                _kitap.Yayinevi = textBoxYayinevi.Text;
                _kitap.YayinYili = int.Parse(textBoxYayinYili.Text);
                _kitap.ISBN = textBoxISBN.Text;
                _kitap.Kategori = comboBoxKategori.SelectedItem.ToString();

                if (_kitap.KitapId == 0)
                {
                    _db.KitapEkle(_kitap);
                    MessageBox.Show("Kitap başarıyla eklendi.");
                }
                else
                {
                    _db.KitapGuncelle(_kitap);
                    MessageBox.Show("Kitap başarıyla güncellendi.");
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void buttonIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool FormDogrula()
        {
            if (string.IsNullOrWhiteSpace(textBoxAd.Text))
            {
                MessageBox.Show("Kitap adı boş olamaz.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxYazar.Text))
            {
                MessageBox.Show("Yazar adı boş olamaz.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxYayinevi.Text))
            {
                MessageBox.Show("Yayınevi boş olamaz.");
                return false;
            }
            if (!int.TryParse(textBoxYayinYili.Text, out int yil) || yil < 1800 || yil > 2100)
            {
                MessageBox.Show("Geçerli bir yayın yılı giriniz (1800-2100).");
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxISBN.Text))
            {
                MessageBox.Show("ISBN boş olamaz.");
                return false;
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.Text = "Yeni Kitap Ekle";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;

            Label labelAd = new Label();
            labelAd.Text = "Kitap Adı:";
            labelAd.Location = new System.Drawing.Point(20, 20);
            labelAd.Size = new System.Drawing.Size(100, 20);

            textBoxAd = new TextBox();
            textBoxAd.Location = new System.Drawing.Point(130, 20);
            textBoxAd.Size = new System.Drawing.Size(330, 20);

            Label labelYazar = new Label();
            labelYazar.Text = "Yazar:";
            labelYazar.Location = new System.Drawing.Point(20, 60);
            labelYazar.Size = new System.Drawing.Size(100, 20);

            textBoxYazar = new TextBox();
            textBoxYazar.Location = new System.Drawing.Point(130, 60);
            textBoxYazar.Size = new System.Drawing.Size(330, 20);

            Label labelYayinevi = new Label();
            labelYayinevi.Text = "Yayınevi:";
            labelYayinevi.Location = new System.Drawing.Point(20, 100);
            labelYayinevi.Size = new System.Drawing.Size(100, 20);

            textBoxYayinevi = new TextBox();
            textBoxYayinevi.Location = new System.Drawing.Point(130, 100);
            textBoxYayinevi.Size = new System.Drawing.Size(330, 20);

            Label labelYayinYili = new Label();
            labelYayinYili.Text = "Yayın Yılı:";
            labelYayinYili.Location = new System.Drawing.Point(20, 140);
            labelYayinYili.Size = new System.Drawing.Size(100, 20);

            textBoxYayinYili = new TextBox();
            textBoxYayinYili.Location = new System.Drawing.Point(130, 140);
            textBoxYayinYili.Size = new System.Drawing.Size(330, 20);

            Label labelISBN = new Label();
            labelISBN.Text = "ISBN:";
            labelISBN.Location = new System.Drawing.Point(20, 180);
            labelISBN.Size = new System.Drawing.Size(100, 20);

            textBoxISBN = new TextBox();
            textBoxISBN.Location = new System.Drawing.Point(130, 180);
            textBoxISBN.Size = new System.Drawing.Size(330, 20);

            Label labelKategori = new Label();
            labelKategori.Text = "Kategori:";
            labelKategori.Location = new System.Drawing.Point(20, 220);
            labelKategori.Size = new System.Drawing.Size(100, 20);

            comboBoxKategori = new ComboBox();
            comboBoxKategori.Location = new System.Drawing.Point(130, 220);
            comboBoxKategori.Size = new System.Drawing.Size(330, 20);
            comboBoxKategori.Items.AddRange(new string[] { "Roman", "Hikaye", "Şiir", "Deneme", "Bilim", "Tarih", "Felsefe", "Çocuk", "Gençlik", "Ders Kitabı", "Referans", "Diğer" });
            comboBoxKategori.SelectedIndex = 0;

            buttonKaydet = new Button();
            buttonKaydet.Text = "Kaydet";
            buttonKaydet.Location = new System.Drawing.Point(200, 320);
            buttonKaydet.Size = new System.Drawing.Size(100, 35);
            buttonKaydet.BackColor = System.Drawing.Color.FromArgb(30, 90, 160);
            buttonKaydet.ForeColor = System.Drawing.Color.White;
            buttonKaydet.Click += buttonKaydet_Click;

            buttonIptal = new Button();
            buttonIptal.Text = "İptal";
            buttonIptal.Location = new System.Drawing.Point(310, 320);
            buttonIptal.Size = new System.Drawing.Size(100, 35);
            buttonIptal.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            buttonIptal.ForeColor = System.Drawing.Color.White;
            buttonIptal.Click += buttonIptal_Click;

            this.Controls.Add(labelAd);
            this.Controls.Add(textBoxAd);
            this.Controls.Add(labelYazar);
            this.Controls.Add(textBoxYazar);
            this.Controls.Add(labelYayinevi);
            this.Controls.Add(textBoxYayinevi);
            this.Controls.Add(labelYayinYili);
            this.Controls.Add(textBoxYayinYili);
            this.Controls.Add(labelISBN);
            this.Controls.Add(textBoxISBN);
            this.Controls.Add(labelKategori);
            this.Controls.Add(comboBoxKategori);
            this.Controls.Add(buttonKaydet);
            this.Controls.Add(buttonIptal);
        }
    }
}
