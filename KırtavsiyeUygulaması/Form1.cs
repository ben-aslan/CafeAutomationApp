using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KırtavsiyeUygulaması
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dateTimePickerTarih.Value = DateTime.Now;
        }
        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\Kirtavsiye.accdb");


        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();

                OleDbCommand sorgu = new OleDbCommand("Select * From Urunler Where UrunKodu='" + txtUrunKodu.Text + "'", baglanti);
                OleDbDataReader oku = sorgu.ExecuteReader();
                oku.Read();

                OleDbCommand komut = new OleDbCommand("Insert Into Satis ( UrunKodu, UrunAdi, Adet, SatisTarihi, Fiyati ) Values( '" + txtUrunKodu.Text + "', '" + oku["UrunAdi"].ToString() + "', '" + numericAdet.Value + "', '" + dateTimePickerTarih.Value.ToString() + "', '" + oku["BirimFiyati"].ToString() + "' )", baglanti);
                komut.ExecuteNonQuery();
                MessageBox.Show("Ürün Satıldı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OleDbCommand sorguu = new OleDbCommand("Update Urunler Set StokSayisi=StokSayisi-" + numericAdet.Value + " Where UrunKodu='" + txtUrunKodu.Text + "'", baglanti);
                sorguu.ExecuteNonQuery();

                ListViewItem urun = new ListViewItem(new string[]
                {
                    txtUrunKodu.Text,
                    oku["UrunAdi"].ToString(),
                    numericAdet.Value.ToString(),
                    dateTimePickerTarih.Value.ToString(),
                    oku["BirimFiyati"].ToString()
                }
                     );
                listView1.Items.Add(urun);
                double hesap = 0;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    hesap += (Convert.ToDouble(listView1.Items[i].SubItems[4].Text) * Convert.ToInt32(listView1.Items[i].SubItems[2].Text));
                    fiyatToplam.Text = "Toplam Tutar= " + hesap.ToString() + " TL";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                if (baglanti.State != ConnectionState.Closed)
                    baglanti.Close();
                SatisListesiniiYukle();

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Urunler frm = new Urunler();
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UrunListele frm1 = new UrunListele();
            frm1.ShowDialog();
        }

        private void SatisListesiniiYukle()
        {
            listView2.Items.Clear();

            try
            {
                double toplamFiyat = 0;
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();

                OleDbCommand komut = new OleDbCommand("Select * From Satis", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();

                while (oku.Read())
                {
                    ListViewItem urun = new ListViewItem(new string[]
                        {
                            oku["UrunKodu"].ToString(  ),
                            oku["UrunAdi"].ToString(  ),
                            oku["Adet"].ToString(  ),
                            oku["SatisTarihi"].ToString(  ),
                            oku["Fiyati"].ToString(  )
                        }
                    );
                    listView2.Items.Add(urun);
                    toplamFiyat += Convert.ToDouble(oku["Fiyati"].ToString()) * Convert.ToInt32(oku["Adet"].ToString());
                }
                satisSayisi.Text = "Yapılan Satış Sayısı: " + listView2.Items.Count.ToString();
                satisToplami.Text = "Satışların Toplamı: " + (toplamFiyat).ToString() + " TL";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                if (baglanti.State != ConnectionState.Closed)
                    baglanti.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SatisListesiniiYukle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(fiyatToplam.Text, "Toplam Ücret");
            listView1.Items.Clear();
            fiyatToplam.Text = "";
        }


        //private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}
    }
}
