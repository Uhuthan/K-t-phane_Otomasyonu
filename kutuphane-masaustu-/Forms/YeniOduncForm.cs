using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KutuphaneOtomasyonu.Data;
using KutuphaneOtomasyonu.Models;

namespace KutuphaneOtomasyonu.Forms
{
    public partial class YeniOduncForm : Form
    {
        private VeritabaniIslemleri _db;
        private ComboBox comboBoxKitaplar;
        private ComboBox comboBoxUyeler;
        private DateTimePicker dateTimePickerIadeTarihi;
        private Button buttonKaydet;
        private Button buttonIptal;
        private List<Kitap> _kitaplar;
        private Label labelKitap;
        private Label labelUye;
        private Label labelIadeTarihi;
        private List<Uye> _uyeler;

        public YeniOduncForm(VeritabaniIslemleri db)
        {
            _db = db;
            InitializeComponent();
            Yukle(); 
        }


        private void YeniOduncForm_Load(object sender, EventArgs e)
        {
            try
            {
                _kitaplar = _db.TumKitaplariGetir();
                comboBoxKitaplar.DataSource = _kitaplar;
                comboBoxKitaplar.DisplayMember = "Ad";
                comboBoxKitaplar.ValueMember = "KitapId";

                _uyeler = _db.TumUyeleriGetir();
                comboBoxUyeler.DataSource = _uyeler;
                comboBoxUyeler.DisplayMember = "AdSoyad";
                comboBoxUyeler.ValueMember = "UyeId";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Yukle()
        {
            _kitaplar = _db.TumKitaplariGetir();
            comboBoxKitaplar.DataSource = null;
            comboBoxKitaplar.DataSource = _kitaplar;
            comboBoxKitaplar.DisplayMember = "Ad";
            comboBoxKitaplar.ValueMember = "KitapId";

            _uyeler = _db.TumUyeleriGetir();
            comboBoxUyeler.DataSource = null;
            comboBoxUyeler.DataSource = _uyeler;
            comboBoxUyeler.DisplayMember = "AdSoyad";
            comboBoxUyeler.ValueMember = "UyeId";
        }

        private void buttonKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                Kitap secilenKitap = (Kitap)comboBoxKitaplar.SelectedItem;
                Uye secilenUye = (Uye)comboBoxUyeler.SelectedItem;

                if (secilenKitap == null || secilenUye == null)
                {
                    MessageBox.Show("Lütfen kitap ve üye seçiniz.");
                    return;
                }

                OduncIslemi islem = new OduncIslemi();
                islem.KitapId = secilenKitap.KitapId;
                islem.UyeId = secilenUye.UyeId;
                islem.OduncTarihi = DateTime.Now;
                islem.IadeTarihi = dateTimePickerIadeTarihi.Value;

                _db.OduncIslemEkle(islem);

                secilenKitap.Durum = "Ödünçte";
                _db.KitapGuncelle(secilenKitap);

                MessageBox.Show("Ödünç işlemi başarıyla oluşturuldu.");
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

        private void InitializeComponent()
        {
            labelKitap = new Label();
            comboBoxKitaplar = new ComboBox();
            labelUye = new Label();
            comboBoxUyeler = new ComboBox();
            labelIadeTarihi = new Label();
            dateTimePickerIadeTarihi = new DateTimePicker();
            buttonKaydet = new Button();
            buttonIptal = new Button();
            SuspendLayout();
            
            labelKitap.Location = new System.Drawing.Point(20, 20);
            labelKitap.Name = "labelKitap";
            labelKitap.Size = new System.Drawing.Size(100, 20);
            labelKitap.TabIndex = 0;
            labelKitap.Text = "Kitap:";
             
            comboBoxKitaplar.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxKitaplar.Location = new System.Drawing.Point(130, 20);
            comboBoxKitaplar.Name = "comboBoxKitaplar";
            comboBoxKitaplar.Size = new System.Drawing.Size(330, 28);
            comboBoxKitaplar.TabIndex = 1;
            
            labelUye.Location = new System.Drawing.Point(20, 60);
            labelUye.Name = "labelUye";
            labelUye.Size = new System.Drawing.Size(100, 20);
            labelUye.TabIndex = 2;
            labelUye.Text = "Üye:";
             
            comboBoxUyeler.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxUyeler.Location = new System.Drawing.Point(130, 60);
            comboBoxUyeler.Name = "comboBoxUyeler";
            comboBoxUyeler.Size = new System.Drawing.Size(330, 28);
            comboBoxUyeler.TabIndex = 3;
           
            labelIadeTarihi.Location = new System.Drawing.Point(20, 100);
            labelIadeTarihi.Name = "labelIadeTarihi";
            labelIadeTarihi.Size = new System.Drawing.Size(100, 20);
            labelIadeTarihi.TabIndex = 4;
            labelIadeTarihi.Text = "İade Tarihi:";
           
            dateTimePickerIadeTarihi.Location = new System.Drawing.Point(130, 100);
            dateTimePickerIadeTarihi.Name = "dateTimePickerIadeTarihi";
            dateTimePickerIadeTarihi.Size = new System.Drawing.Size(330, 27);
            dateTimePickerIadeTarihi.TabIndex = 5;
           
            buttonKaydet.BackColor = System.Drawing.Color.FromArgb(30, 90, 160);
            buttonKaydet.ForeColor = System.Drawing.Color.White;
            buttonKaydet.Location = new System.Drawing.Point(200, 220);
            buttonKaydet.Name = "buttonKaydet";
            buttonKaydet.Size = new System.Drawing.Size(100, 35);
            buttonKaydet.TabIndex = 6;
            buttonKaydet.Text = "Kaydet";
            buttonKaydet.UseVisualStyleBackColor = false;
            buttonKaydet.Click += buttonKaydet_Click;
            
            buttonIptal.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            buttonIptal.ForeColor = System.Drawing.Color.White;
            buttonIptal.Location = new System.Drawing.Point(310, 220);
            buttonIptal.Name = "buttonIptal";
            buttonIptal.Size = new System.Drawing.Size(100, 35);
            buttonIptal.TabIndex = 7;
            buttonIptal.Text = "İptal";
            buttonIptal.UseVisualStyleBackColor = false;
            buttonIptal.Click += buttonIptal_Click;
            
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(482, 253);
            Controls.Add(labelKitap);
            Controls.Add(comboBoxKitaplar);
            Controls.Add(labelUye);
            Controls.Add(comboBoxUyeler);
            Controls.Add(labelIadeTarihi);
            Controls.Add(dateTimePickerIadeTarihi);
            Controls.Add(buttonKaydet);
            Controls.Add(buttonIptal);
            Name = "YeniOduncForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Yeni Ödünç İşlemi";
            Load += YeniOduncForm_Load_1;
            ResumeLayout(false);
        }

        private void YeniOduncForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
