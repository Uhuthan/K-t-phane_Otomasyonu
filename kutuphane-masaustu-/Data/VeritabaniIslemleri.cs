using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using KutuphaneOtomasyonu.Models;

namespace KutuphaneOtomasyonu.Data
{
    public class VeritabaniIslemleri
    {
        private string _dbPath;
        private string _connectionString;

        public VeritabaniIslemleri()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appData, "KutuphaneOtomasyonu");
            
            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);

            _dbPath = Path.Combine(appFolder, "kutuphane.db");
            _connectionString = $"Data Source={_dbPath};Version=3;";
            VeritabaniOlustur();

        }

        public void VeritabaniOlustur()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();

                    string[] komutlar = new string[]
                    {
                        @"CREATE TABLE IF NOT EXISTS Kitaplar (
                            KitapId INTEGER PRIMARY KEY AUTOINCREMENT,
                            Ad TEXT NOT NULL,
                            Yazar TEXT NOT NULL,
                            Yayinevi TEXT NOT NULL,
                            YayinYili INTEGER NOT NULL,
                            ISBN TEXT UNIQUE NOT NULL,
                            Kategori TEXT NOT NULL,
                            Durum TEXT NOT NULL DEFAULT 'Mevcut',
                            OlusturmaTarihi TEXT NOT NULL
                        )",

                        @"CREATE TABLE IF NOT EXISTS Uyeler (
                            UyeId INTEGER PRIMARY KEY AUTOINCREMENT,
                            Ad TEXT NOT NULL,
                            Soyad TEXT NOT NULL,
                            UyeNo TEXT UNIQUE NOT NULL,
                            Telefon TEXT NOT NULL,
                            Email TEXT NOT NULL,
                            ToplamCeza REAL NOT NULL DEFAULT 0,
                            OlusturmaTarihi TEXT NOT NULL
                        )",

                        @"CREATE TABLE IF NOT EXISTS OduncIslemleri (
                            IslemId INTEGER PRIMARY KEY AUTOINCREMENT,
                            KitapId INTEGER NOT NULL,
                            UyeId INTEGER NOT NULL,
                            OduncTarihi TEXT NOT NULL,
                            IadeTarihi TEXT NOT NULL,
                            GercekIadeTarihi TEXT,
                            Ceza REAL NOT NULL DEFAULT 0,
                            Durum TEXT NOT NULL DEFAULT 'Aktif',
                            FOREIGN KEY(KitapId) REFERENCES Kitaplar(KitapId),
                            FOREIGN KEY(UyeId) REFERENCES Uyeler(UyeId)
                        )"
                    };

                    foreach (string komut in komutlar)
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Veritabanı oluşturulurken hata: " + ex.Message);
            }
        }

        public List<Kitap> TumKitaplariGetir()
        {
            List<Kitap> kitaplar = new List<Kitap>();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = "SELECT * FROM Kitaplar ORDER BY Ad";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Kitap kitap = new Kitap
                                {
                                    KitapId = Convert.ToInt32(reader["KitapId"]),
                                    Ad = reader["Ad"].ToString(),
                                    Yazar = reader["Yazar"].ToString(),
                                    Yayinevi = reader["Yayinevi"].ToString(),
                                    YayinYili = Convert.ToInt32(reader["YayinYili"]),
                                    ISBN = reader["ISBN"].ToString(),
                                    Kategori = reader["Kategori"].ToString(),
                                    Durum = reader["Durum"].ToString(),
                                    OlusturmaTarihi = DateTime.Parse(reader["OlusturmaTarihi"].ToString())
                                };
                                kitaplar.Add(kitap);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Kitaplar getirilirken hata: " + ex.Message);
            }

            return kitaplar;
        }

        public void KitapEkle(Kitap kitap)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = @"INSERT INTO Kitaplar (Ad, Yazar, Yayinevi, YayinYili, ISBN, Kategori, Durum, OlusturmaTarihi)
                                     VALUES (@Ad, @Yazar, @Yayinevi, @YayinYili, @ISBN, @Kategori, @Durum, @OlusturmaTarihi)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ad", kitap.Ad);
                        cmd.Parameters.AddWithValue("@Yazar", kitap.Yazar);
                        cmd.Parameters.AddWithValue("@Yayinevi", kitap.Yayinevi);
                        cmd.Parameters.AddWithValue("@YayinYili", kitap.YayinYili);
                        cmd.Parameters.AddWithValue("@ISBN", kitap.ISBN);
                        cmd.Parameters.AddWithValue("@Kategori", kitap.Kategori);
                        cmd.Parameters.AddWithValue("@Durum", kitap.Durum);
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", kitap.OlusturmaTarihi.ToString("yyyy-MM-dd HH:mm:ss"));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Kitap eklenirken hata: " + ex.Message);
            }
        }

        public void KitapGuncelle(Kitap kitap)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = @"UPDATE Kitaplar SET Ad=@Ad, Yazar=@Yazar, Yayinevi=@Yayinevi, 
                                     YayinYili=@YayinYili, ISBN=@ISBN, Kategori=@Kategori, Durum=@Durum
                                     WHERE KitapId=@KitapId";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@KitapId", kitap.KitapId);
                        cmd.Parameters.AddWithValue("@Ad", kitap.Ad);
                        cmd.Parameters.AddWithValue("@Yazar", kitap.Yazar);
                        cmd.Parameters.AddWithValue("@Yayinevi", kitap.Yayinevi);
                        cmd.Parameters.AddWithValue("@YayinYili", kitap.YayinYili);
                        cmd.Parameters.AddWithValue("@ISBN", kitap.ISBN);
                        cmd.Parameters.AddWithValue("@Kategori", kitap.Kategori);
                        cmd.Parameters.AddWithValue("@Durum", kitap.Durum);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Kitap güncellenirken hata: " + ex.Message);
            }
        }

        public void KitapSil(int kitapId)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = "DELETE FROM Kitaplar WHERE KitapId=@KitapId";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@KitapId", kitapId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Kitap silinirken hata: " + ex.Message);
            }
        }

        public List<Uye> TumUyeleriGetir()
        {
            List<Uye> uyeler = new List<Uye>();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = "SELECT * FROM Uyeler ORDER BY Ad";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Uye uye = new Uye
                                {
                                    UyeId = Convert.ToInt32(reader["UyeId"]),

                                    Ad = reader["Ad"].ToString(),
                                    Soyad = reader["Soyad"].ToString(),
                                    UyeNo = reader["UyeNo"].ToString(),
                                    Telefon = reader["Telefon"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    ToplamCeza = (decimal)(double)reader["ToplamCeza"],
                                    OlusturmaTarihi = DateTime.Parse(reader["OlusturmaTarihi"].ToString())
                                };
                                uyeler.Add(uye);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Üyeler getirilirken hata: " + ex.Message);
            }

            return uyeler;
        }

        public void UyeEkle(Uye uye)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = @"INSERT INTO Uyeler (Ad, Soyad, UyeNo, Telefon, Email, ToplamCeza, OlusturmaTarihi)
                                     VALUES (@Ad, @Soyad, @UyeNo, @Telefon, @Email, @ToplamCeza, @OlusturmaTarihi)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ad", uye.Ad);
                        cmd.Parameters.AddWithValue("@Soyad", uye.Soyad);
                        cmd.Parameters.AddWithValue("@UyeNo", uye.UyeNo);
                        cmd.Parameters.AddWithValue("@Telefon", uye.Telefon);
                        cmd.Parameters.AddWithValue("@Email", uye.Email);
                        cmd.Parameters.AddWithValue("@ToplamCeza", (double)uye.ToplamCeza);
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", uye.OlusturmaTarihi.ToString("yyyy-MM-dd HH:mm:ss"));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Üye eklenirken hata: " + ex.Message);
            }
        }

        public void UyeGuncelle(Uye uye)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = @"UPDATE Uyeler SET Ad=@Ad, Soyad=@Soyad, UyeNo=@UyeNo, 
                                     Telefon=@Telefon, Email=@Email, ToplamCeza=@ToplamCeza
                                     WHERE UyeId=@UyeId";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@UyeId", uye.UyeId);
                        cmd.Parameters.AddWithValue("@Ad", uye.Ad);
                        cmd.Parameters.AddWithValue("@Soyad", uye.Soyad);
                        cmd.Parameters.AddWithValue("@UyeNo", uye.UyeNo);
                        cmd.Parameters.AddWithValue("@Telefon", uye.Telefon);
                        cmd.Parameters.AddWithValue("@Email", uye.Email);
                        cmd.Parameters.AddWithValue("@ToplamCeza", (double)uye.ToplamCeza);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Üye güncellenirken hata: " + ex.Message);
            }
        }

        public void UyeSil(int uyeId)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = "DELETE FROM Uyeler WHERE UyeId=@UyeId";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@UyeId", uyeId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Üye silinirken hata: " + ex.Message);
            }
        }

        public List<OduncIslemi> TumOduncIslemleriniGetir()
        {
            List<OduncIslemi> islemler = new List<OduncIslemi>();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = "SELECT * FROM OduncIslemleri ORDER BY OduncTarihi DESC";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OduncIslemi islem = new OduncIslemi
                                {
                                    IslemId = Convert.ToInt32(reader["IslemId"]),
                                    KitapId = Convert.ToInt32(reader["KitapId"]),
                                    UyeId = Convert.ToInt32(reader["UyeId"]),

                                    OduncTarihi = DateTime.Parse(reader["OduncTarihi"].ToString()),
                                    IadeTarihi = DateTime.Parse(reader["IadeTarihi"].ToString()),
                                    GercekIadeTarihi = string.IsNullOrEmpty(reader["GercekIadeTarihi"].ToString()) ? null : (DateTime?)DateTime.Parse(reader["GercekIadeTarihi"].ToString()),
                                    Ceza = (decimal)(double)reader["Ceza"],
                                    Durum = reader["Durum"].ToString()
                                };
                                islemler.Add(islem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ödünç işlemleri getirilirken hata: " + ex.Message);
            }

            return islemler;
        }

        public void OduncIslemEkle(OduncIslemi islem)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = @"INSERT INTO OduncIslemleri (KitapId, UyeId, OduncTarihi, IadeTarihi, Ceza, Durum)
                                     VALUES (@KitapId, @UyeId, @OduncTarihi, @IadeTarihi, @Ceza, @Durum)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@KitapId", islem.KitapId);
                        cmd.Parameters.AddWithValue("@UyeId", islem.UyeId);
                        cmd.Parameters.AddWithValue("@OduncTarihi", islem.OduncTarihi.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@IadeTarihi", islem.IadeTarihi.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@Ceza", (double)islem.Ceza);
                        cmd.Parameters.AddWithValue("@Durum", islem.Durum);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ödünç işlemi eklenirken hata: " + ex.Message);
            }
        }

        public void OduncIslemGuncelle(OduncIslemi islem)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    string sorgu = @"UPDATE OduncIslemleri SET GercekIadeTarihi=@GercekIadeTarihi, 
                                     Ceza=@Ceza, Durum=@Durum WHERE IslemId=@IslemId";

                    using (SQLiteCommand cmd = new SQLiteCommand(sorgu, conn))
                    {
                        cmd.Parameters.AddWithValue("@IslemId", islem.IslemId);
                        cmd.Parameters.AddWithValue("@GercekIadeTarihi", islem.GercekIadeTarihi?.ToString("yyyy-MM-dd HH:mm:ss") ?? "");
                        cmd.Parameters.AddWithValue("@Ceza", (double)islem.Ceza);
                        cmd.Parameters.AddWithValue("@Durum", islem.Durum);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ödünç işlemi güncellenirken hata: " + ex.Message);
            }
        }
    }
}
