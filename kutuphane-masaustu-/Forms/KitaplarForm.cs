using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KutuphaneOtomasyonu.Data;
using KutuphaneOtomasyonu.Models;

namespace KutuphaneOtomasyonu.Forms
{
    public partial class KitaplarForm : Form
    {
        private VeritabaniIslemleri _db;
        private DataGridView dataGridViewKitaplar;
        private Button buttonEkle;
        private Button buttonDuzenle;
        private Button buttonSil;
        private Button buttonKapat;

        public KitaplarForm(VeritabaniIslemleri db)
        {
            _db = db;
            InitializeComponent();
        }

        private void KitaplarForm_Load(object sender, EventArgs e)
        {
            KitaplariYukle();
        }

        private void KitaplariYukle()
        {
            try
            {
                List<Kitap> kitaplar = _db.TumKitaplariGetir();
                dataGridViewKitaplar.DataSource = kitaplar;
                dataGridViewKitaplar.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitaplar yüklenirken hata: " + ex.Message);
            }
        }

        private void buttonEkle_Click(object sender, EventArgs e)
        {
            KitapEkleForm ekleForm = new KitapEkleForm(_db, null);
            if (ekleForm.ShowDialog() == DialogResult.OK)
            {
                KitaplariYukle();
            }
        }

        private void buttonDuzenle_Click(object sender, EventArgs e)
        {
            if (dataGridViewKitaplar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlemek için bir kitap seçiniz.");
                return;
            }

            Kitap seciliKitap = (Kitap)dataGridViewKitaplar.SelectedRows[0].DataBoundItem;
            KitapEkleForm ekleForm = new KitapEkleForm(_db, seciliKitap);
            if (ekleForm.ShowDialog() == DialogResult.OK)
            {
                KitaplariYukle();
            }
        }

        private void buttonSil_Click(object sender, EventArgs e)
        {
            if (dataGridViewKitaplar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir kitap seçiniz.");
                return;
            }

            if (MessageBox.Show("Seçili kitabı silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    Kitap seciliKitap = (Kitap)dataGridViewKitaplar.SelectedRows[0].DataBoundItem;
                    _db.KitapSil(seciliKitap.KitapId);
                    MessageBox.Show("Kitap başarıyla silindi.");
                    KitaplariYukle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kitap silinirken hata: " + ex.Message);
                }
            }
        }

        private void buttonKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.Text = "Kitaplar";
            this.Size = new System.Drawing.Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;

            dataGridViewKitaplar = new DataGridView();
            dataGridViewKitaplar.Dock = DockStyle.Fill;
            dataGridViewKitaplar.AllowUserToAddRows = false;
            dataGridViewKitaplar.AllowUserToDeleteRows = false;
            dataGridViewKitaplar.ReadOnly = true;
            dataGridViewKitaplar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewKitaplar.MultiSelect = false;

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

            this.Controls.Add(dataGridViewKitaplar);
            this.Controls.Add(panelButonlar);
        }
    }
}
