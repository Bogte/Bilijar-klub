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
    public partial class Sto : Form
    {
        DataTable podaci;
        SqlCommand menjanja;

        public Sto()
        {
            InitializeComponent();
        }

        private void Sto_Load(object sender, EventArgs e)
        {
            Osvezi();   
        }

        private void Osvezi()
        {
            podaci = new DataTable();
            podaci = Konekcija.Unos("SELECT * FROM Sto");
            dataGridView1.DataSource = podaci;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int indeks = dataGridView1.CurrentRow.Index;

                textBox1.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["id"].Value);
                textBox2.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Naziv"].Value);
                comboBox1.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Slobodan"].Value);

                if (Convert.ToString(dataGridView1.Rows[indeks].Cells["Slobodan"].Value) == "True")
                {
                    comboBox1.Text = "Slobodan";
                }
                else
                {
                    comboBox1.Text = "Zauzet";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da obrisete ove podatake?", "Bilijar klub", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("DELETE FROM Sto WHERE id = " + textBox1.Text);

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
                    if (textBox2.Text == "" || comboBox1.Text == "")
                    {
                        MessageBox.Show("Sva polja moraju biti popunjena - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    int slobodan;
                    if (comboBox1.Text == "Slobodan")
                    {
                        slobodan = 1;
                    }
                    else
                    {
                        slobodan = 0;
                    }

                    podaci = new DataTable();
                    podaci = Konekcija.Unos("SELECT * FROM Sto WHERE Naziv = '" + textBox2.Text + "' AND Slobodan = " + slobodan);
                    if (podaci.Rows.Count >= 1) throw new Exception();

                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("UPDATE Sto SET Naziv = '" + textBox2.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Sto SET Slobodan = " + slobodan + " WHERE id = " + textBox1.Text);

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
                MessageBox.Show("Podatak vec postoji u tabeli - " + ex.Source, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SqlConnection con = new SqlConnection(Konekcija.Veza());
                con.Close();
                Osvezi();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da dodate ove podatke?", "Bilijar klub", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (textBox2.Text == "" || comboBox1.Text == "")
                    {
                        MessageBox.Show("Sva polja moraju biti popunjena - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    if (comboBox1.Text != "Slobodan")
                    {
                        MessageBox.Show("Sto prilikom unosa mora biti slobodan - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    int slobodan;
                    if (comboBox1.Text == "Slobodan")
                    {
                        slobodan = 1;
                    }
                    else
                    {
                        slobodan = 0;
                    }

                    podaci = new DataTable();
                    podaci = Konekcija.Unos("SELECT * FROM Sto WHERE Naziv = '" + textBox2.Text + "' AND Slobodan = " + slobodan);
                    if (podaci.Rows.Count >= 1) throw new Exception();

                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("INSERT INTO Sto VALUES ('" + textBox2.Text + "', " + slobodan + ")");
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
                MessageBox.Show("Podatak vec postoji u tabeli - " + ex.Source, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SqlConnection con = new SqlConnection(Konekcija.Veza());
                con.Close();
                Osvezi();
            }
        }
    }
}
