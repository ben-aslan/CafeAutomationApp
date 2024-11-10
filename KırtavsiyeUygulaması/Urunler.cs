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
    public partial class Urunler : Form
    {
        public Urunler()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\Kirtavsiye.accdb");

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();

                OleDbCommand komut = new OleDbCommand("Insert Into Urunler ( UrunKodu, UrunAdi, StokSayisi, BirimFiyati ) Values( '" + txtUrunKodu.Text + "', '" + txtUrunAdi.Text + "', '" + nmStokSayisi.Value + "', '" + txtBirimFiyat.Text + "' )", baglanti);
                komut.ExecuteNonQuery();
                MessageBox.Show("Ürün Eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
    }
}
