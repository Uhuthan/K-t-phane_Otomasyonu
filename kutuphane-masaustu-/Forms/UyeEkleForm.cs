using System;
using System.Windows.Forms;
using KutuphaneOtomasyonu.Data;
using KutuphaneOtomasyonu.Models;

namespace KutuphaneOtomasyonu.Forms
{
    public partial class UyeEkleForm : Form
    {
        private VeritabaniIslemleri _db;
        private Uye _uye;
        private TextBox textBoxAd;
        private TextBox textBoxSoyad;
        private TextBox textBoxUyeNo;
        private TextBox textBoxTelefon;
        private TextBox textBoxEmail;
        private Button buttonKaydet;
        private Button buttonIptal;

        public UyeEkleForm(VeritabaniIslemleri db, Uye uye)
        {
            _db = db;
            _uye = uye;
            InitializeComponent();
        }

        private void UyeEkleForm_Load(object sender, EventArgs e)
        {
            if (_uye != null)
            {
                this.Text = "Üye Düzenle";
                textBoxAd.Text = _uye.Ad;
                textBoxSoyad.Text = _uye.Soyad;
                textBoxUyeNo.Text = _uye.UyeNo;
                textBoxTelefon.Text = _uye.Telefon;
                textBoxEmail.Text = _uye.Email;
            }
        }

        private void buttonKaydet_Click(object sender, EventArgs e)
        {
            if (!FormDogrula())
                return;

            try
            {
                if (_uye == null)
                {
                    _uye = new Uye();
                }

                _uye.Ad = textBoxAd.Text;
                _uye.Soyad = textBoxSoyad.Text;
                _uye.UyeNo = textBoxUyeNo.Text;
                _uye.Telefon = textBoxTelefon.Text;
                _uye.Email = textBoxEmail.Text;

                if (_uye.UyeId == 0)
                {
                    _db.UyeEkle(_uye);
                    MessageBox.Show("Üye başarıyla eklendi.");
                }
                else
                {
                    _db.UyeGuncelle(_uye);
                    MessageBox.Show("Üye başarıyla güncellendi.");
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
                MessageBox.Show("Ad boş olamaz.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxSoyad.Text))
            {
                MessageBox.Show("Soyad boş olamaz.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxUyeNo.Text))
            {
                MessageBox.Show("Üye numarası boş olamaz.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxTelefon.Text) || textBoxTelefon.Text.Length < 10)
            {
                MessageBox.Show("Geçerli bir telefon numarası giriniz (en az 10 haneli).");
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxEmail.Text) || !IsValidEmail(textBoxEmail.Text))
            {
                MessageBox.Show("Geçerli bir e-posta adresi giriniz.");
                return false;
            }
            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Yeni Üye Ekle";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.White;

            Label labelAd = new Label();
            labelAd.Text = "Ad:";
            labelAd.Location = new System.Drawing.Point(20, 20);
            labelAd.Size = new System.Drawing.Size(100, 20);

            textBoxAd = new TextBox();
            textBoxAd.Location = new System.Drawing.Point(130, 20);
            textBoxAd.Size = new System.Drawing.Size(330, 20);

            Label labelSoyad = new Label();
            labelSoyad.Text = "Soyad:";
            labelSoyad.Location = new System.Drawing.Point(20, 60);
            labelSoyad.Size = new System.Drawing.Size(100, 20);

            textBoxSoyad = new TextBox();
            textBoxSoyad.Location = new System.Drawing.Point(130, 60);
            textBoxSoyad.Size = new System.Drawing.Size(330, 20);

            Label labelUyeNo = new Label();
            labelUyeNo.Text = "Üye Numarası:";
            labelUyeNo.Location = new System.Drawing.Point(20, 100);
            labelUyeNo.Size = new System.Drawing.Size(100, 20);

            textBoxUyeNo = new TextBox();
            textBoxUyeNo.Location = new System.Drawing.Point(130, 100);
            textBoxUyeNo.Size = new System.Drawing.Size(330, 20);

            Label labelTelefon = new Label();
            labelTelefon.Text = "Telefon:";
            labelTelefon.Location = new System.Drawing.Point(20, 140);
            labelTelefon.Size = new System.Drawing.Size(100, 20);

            textBoxTelefon = new TextBox();
            textBoxTelefon.Location = new System.Drawing.Point(130, 140);
            textBoxTelefon.Size = new System.Drawing.Size(330, 20);

            Label labelEmail = new Label();
            labelEmail.Text = "E-posta:";
            labelEmail.Location = new System.Drawing.Point(20, 180);
            labelEmail.Size = new System.Drawing.Size(100, 20);

            textBoxEmail = new TextBox();
            textBoxEmail.Location = new System.Drawing.Point(130, 180);
            textBoxEmail.Size = new System.Drawing.Size(330, 20);

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
            this.Controls.Add(labelSoyad);
            this.Controls.Add(textBoxSoyad);
            this.Controls.Add(labelUyeNo);
            this.Controls.Add(textBoxUyeNo);
            this.Controls.Add(labelTelefon);
            this.Controls.Add(textBoxTelefon);
            this.Controls.Add(labelEmail);
            this.Controls.Add(textBoxEmail);
            this.Controls.Add(buttonKaydet);
            this.Controls.Add(buttonIptal);
        }
    }
}
