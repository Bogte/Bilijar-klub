using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Bilijar_klub
{
    public partial class Usluga : Form
    {
        DataTable podaci;
        SqlCommand menjanja;

        public Usluga()
        {
            InitializeComponent();
        }

        private void Usluga_Load(object sender, EventArgs e)
        {
            podaci = new DataTable();//Dodavanje zaposlenih
            podaci = Konekcija.Unos("SELECT DISTINCT ime + ' ' + prezime AS 'Zaposleni' FROM Zaposleni");
            string[] pomocna = new string[podaci.Rows.Count];
            for (int i = 0; i < podaci.Rows.Count; i++)
            {
                pomocna[i] = Convert.ToString(podaci.Rows[i]["Zaposleni"]);
                comboBox1.Items.Add(pomocna[i]);
            }

            podaci = new DataTable();//Dodavanje stolova
            podaci = Konekcija.Unos("SELECT DISTINCT Naziv AS 'Sto' FROM Sto");
            pomocna = new string[podaci.Rows.Count];
            for (int i = 0; i < podaci.Rows.Count; i++)
            {
                pomocna[i] = Convert.ToString(podaci.Rows[i]["Sto"]);
                comboBox2.Items.Add(pomocna[i]);
            }

            Osvezi();
        }

        private void Osvezi()
        {
            podaci = new DataTable();
            podaci = Konekcija.Unos("SELECT Usluga.id, convert(varchar(10), Dan) AS 'Datum', Vreme_dolaska, Vreme_odlaska, Zaposleni.id AS 'Oznaka zaposlenog', Zaposleni.ime + ' ' + Zaposleni.Prezime AS 'Zaposleni', Sto.Naziv AS 'Sto', Placanje, Trajanje FROM Usluga\r\nJOIN Zaposleni ON Zaposleni.id = Usluga.id_zaposlenog\r\nJOIN Sto ON Sto.id = Usluga.id_stola");
            dataGridView1.DataSource = podaci;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int indeks = dataGridView1.CurrentRow.Index;

                textBox1.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["id"].Value);
                textBox2.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Datum"].Value);
                textBox3.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Vreme_dolaska"].Value);
                textBox4.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Vreme_odlaska"].Value);
                textBox9.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Oznaka zaposlenog"].Value);
                comboBox1.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Zaposleni"].Value);
                comboBox2.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Sto"].Value);
                textBox7.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Placanje"].Value);
                textBox8.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Trajanje"].Value);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da obrisete ove podatake?", "Bilijar klub", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("DELETE FROM Usluga WHERE id = " + textBox1.Text);

                    SqlConnection con = new SqlConnection(Konekcija.Veza());
                    con.Open();
                    menjanja.Connection = con;
                    menjanja.ExecuteNonQuery();
                    con.Close();

                    Osvezi();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ne mozete da obrisete ove podatake, druge tabele zahtevaju ove podatake! - " + ex.Source, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SqlConnection con = new SqlConnection(Konekcija.Veza());
                con.Close();
                Osvezi();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da izmenite ove podatke?", "Bilijar klub", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (textBox2.Text == "" || comboBox1.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                    {
                        MessageBox.Show("Datum, vreme dolaska, vreme odlaska i zaposleni moraju biti popunjeni - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("UPDATE Usluga SET Dan = '" + textBox2.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Usluga SET Vreme_dolaska = '" + textBox3.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Usluga SET Vreme_odlaska = '" + textBox4.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Usluga SET id_zaposlenog = " + textBox9.Text + " WHERE id = " + textBox1.Text);

                    SqlConnection con = new SqlConnection(Konekcija.Veza());
                    con.Open();
                    menjanja.Connection = con;
                    menjanja.ExecuteNonQuery();
                    con.Close();

                    Osvezi();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SqlConnection con = new SqlConnection(Konekcija.Veza());
                con.Close();
                Osvezi();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            podaci = new DataTable();
            podaci = Konekcija.Unos("SELECT id FROM Zaposleni WHERE ime + ' ' + prezime = '" + comboBox1.Text + "'");
            textBox9.Text = Convert.ToString(podaci.Rows[0][0]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da dodate ove podatke?", "Bilijar klub", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (comboBox1.Text == "" || textBox3.Text == "" || comboBox2.Text == "")
                    {
                        MessageBox.Show("Vreme dolaska, zaposleni i sto moraju biti popunjena - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    podaci = new DataTable(); //Trazenje id-a za sto
                    podaci = Konekcija.Unos("SELECT id FROM Sto WHERE Naziv = '" + comboBox2.Text + "'");
                    int id_stola = (int)podaci.Rows[0][0];

                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("INSERT INTO Usluga (Vreme_dolaska, id_zaposlenog, id_stola) VALUES ('" + textBox3.Text + "', " + textBox9.Text + ", '" + id_stola + "')");
                    SqlConnection con = new SqlConnection(Konekcija.Veza());
                    con.Open();
                    menjanja.Connection = con;
                    menjanja.ExecuteNonQuery();
                    con.Close();

                    Osvezi();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SqlConnection con = new SqlConnection(Konekcija.Veza());
                con.Close();
                Osvezi();
            }
        }
    }
    
}
