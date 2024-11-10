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
    public partial class UrunListele : Form
    {
        public UrunListele()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\Kirtavsiye.accdb");

        private void UrunListele_Load(object sender, EventArgs e)
        {
            UrunleriYukle();
        }

        private void UrunleriYukle()
        {
            listView1.Items.Clear();

            try
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();

                OleDbCommand komut = new OleDbCommand("Select * From Urunler", baglanti);
                OleDbDataReader oku = komut.ExecuteReader();

                while (oku.Read())
                {
                    ListViewItem urun = new ListViewItem(new string[]
                    {
                        oku["UrunKodu"].ToString(  ),
                        oku["UrunAdi"].ToString(  ),
                        oku["StokSayisi"].ToString(  ),
                        oku["BirimFiyati"].ToString(  )
                    }
                         );
                    listView1.Items.Add(urun);
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
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            ListViewItem item = listView1.SelectedItems[0];
            txtUrunKodu.Text = item.SubItems[0].Text;
            txtUrunAdi.Text = item.SubItems[1].Text;
            nmStokSayisi.Value = Convert.ToInt32(item.SubItems[2].Text);
            txtBirimFiyat.Text = item.SubItems[3].Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();

                OleDbCommand komut = new OleDbCommand("Update Urunler Set UrunAdi='" + txtUrunAdi.Text + "', StokSayisi='" + nmStokSayisi.Value + "', BirimFiyati='" + txtBirimFiyat.Text + "' Where UrunKodu='" + txtUrunKodu.Text + "'", baglanti);
                komut.ExecuteNonQuery();
                MessageBox.Show("Ürün Bilgileri Güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                if (baglanti.State != ConnectionState.Closed)
                    baglanti.Close();

                UrunleriYukle();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();

                OleDbCommand komut = new OleDbCommand("Delete * From Urunler Where UrunKodu='" + txtUrunKodu.Text + "'", baglanti);
                komut.ExecuteNonQuery();
                MessageBox.Show("Ürün Silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                if (baglanti.State != ConnectionState.Closed)
                    baglanti.Close();

                UrunleriYukle();
            }
        }
    }
}

