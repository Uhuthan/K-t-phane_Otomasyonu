using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KutuphaneOtomasyonu.Data;
using KutuphaneOtomasyonu.Models;

namespace KutuphaneOtomasyonu.Forms
{
    public partial class UyelerForm : Form
    {
        private VeritabaniIslemleri _db;
        private DataGridView dataGridViewUyeler;
        private Button buttonEkle;
        private Button buttonDuzenle;
        private Button buttonSil;
        private Button buttonKapat;

        public UyelerForm(VeritabaniIslemleri db)
        {
            _db = db;
            InitializeComponent();
        }

        private void UyelerForm_Load(object sender, EventArgs e)
        {
            UyeleriYukle();
        }

        private void UyeleriYukle()
        {
            try
            {
                List<Uye> uyeler = _db.TumUyeleriGetir();
                dataGridViewUyeler.DataSource = uyeler;
                dataGridViewUyeler.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Üyeler yüklenirken hata: " + ex.Message);
            }
        }

        private void buttonEkle_Click(object sender, EventArgs e)
        {
            UyeEkleForm ekleForm = new UyeEkleForm(_db, null);
            if (ekleForm.ShowDialog() == DialogResult.OK)
            {
                UyeleriYukle();
            }
        }

        private void buttonDuzenle_Click(object sender, EventArgs e)
        {
            if (dataGridViewUyeler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlemek için bir üye seçiniz.");
                return;
            }

            Uye seciliUye = (Uye)dataGridViewUyeler.SelectedRows[0].DataBoundItem;
            UyeEkleForm ekleForm = new UyeEkleForm(_db, seciliUye);
            if (ekleForm.ShowDialog() == DialogResult.OK)
            {
                UyeleriYukle();
            }
        }

        private void buttonSil_Click(object sender, EventArgs e)
        {
            if (dataGridViewUyeler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir üye seçiniz.");
                return;
            }

            if (MessageBox.Show("Seçili üyeyi silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    Uye seciliUye = (Uye)dataGridViewUyeler.SelectedRows[0].DataBoundItem;
                    _db.UyeSil(seciliUye.UyeId);
                    MessageBox.Show("Üye başarıyla silindi.");
                    UyeleriYukle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Üye silinirken hata: " + ex.Message);
                }
            }
        }

        private void buttonKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.Text = "Üyeler";
            this.Size = new System.Drawing.Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            dataGridViewUyeler = new DataGridView();
            dataGridViewUyeler.Dock = DockStyle.Fill;
            dataGridViewUyeler.AllowUserToAddRows = false;
            dataGridViewUyeler.AllowUserToDeleteRows = false;
            dataGridViewUyeler.ReadOnly = true;
            dataGridViewUyeler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUyeler.MultiSelect = false;

            Panel panelButonlar = new Panel();
            panelButonlar.Height = 50;
            panelButonlar.Dock = DockStyle.Bottom;
            panelButonlar.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

            buttonEkle = new Button();
            buttonEkle.Text = "Ekle";
            buttonEkle.Location = new System.Drawing.Point(10, 10);
            buttonEkle.Size = new System.Drawing.Size(100, 30);
            buttonEkle.BackColor = System.Drawing.Color.FromArgb(30, 90, 160);
            buttonEkle.ForeColor = System.Drawing.Color.White;
            buttonEkle.Click += buttonEkle_Click;

            buttonDuzenle = new Button();
            buttonDuzenle.Text = "Düzenle";
            buttonDuzenle.Location = new System.Drawing.Point(120, 10);
            buttonDuzenle.Size = new System.Drawing.Size(100, 30);
            buttonDuzenle.BackColor = System.Drawing.Color.FromArgb(30, 90, 160);
            buttonDuzenle.ForeColor = System.Drawing.Color.White;
            buttonDuzenle.Click += buttonDuzenle_Click;

            buttonSil = new Button();
            buttonSil.Text = "Sil";
            buttonSil.Location = new System.Drawing.Point(230, 10);
            buttonSil.Size = new System.Drawing.Size(100, 30);
            buttonSil.BackColor = System.Drawing.Color.FromArgb(200, 50, 50);
            buttonSil.ForeColor = System.Drawing.Color.White;
            buttonSil.Click += buttonSil_Click;

            buttonKapat = new Button();
            buttonKapat.Text = "Kapat";
            buttonKapat.Location = new System.Drawing.Point(870, 10);
            buttonKapat.Size = new System.Drawing.Size(100, 30);
            buttonKapat.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            buttonKapat.ForeColor = System.Drawing.Color.White;
            buttonKapat.Click += buttonKapat_Click;

            panelButonlar.Controls.Add(buttonEkle);
            panelButonlar.Controls.Add(buttonDuzenle);
            panelButonlar.Controls.Add(buttonSil);
            panelButonlar.Controls.Add(buttonKapat);

            this.Controls.Add(dataGridViewUyeler);
            this.Controls.Add(panelButonlar);
        }
    }
}
