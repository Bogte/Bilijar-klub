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
    public partial class Zaposleni : Form
    {
        DataTable podaci;
        SqlCommand menjanja;

        public Zaposleni()
        {
            InitializeComponent();
        }

        private void Zaposleni_Load(object sender, EventArgs e)
        {
            podaci = new DataTable();//Dodavanje zaposlenih
            podaci = Konekcija.Unos("SELECT DISTINCT Uloga AS 'Z' FROM Plata");
            string[] pomocna = new string[podaci.Rows.Count];
            for (int i = 0; i < podaci.Rows.Count; i++)
            {
                pomocna[i] = Convert.ToString(podaci.Rows[i]["Z"]);
                comboBox1.Items.Add(pomocna[i]);
            }

            Osvezi();
        }

        private void Osvezi()
        {
            podaci = new DataTable();
            podaci = Konekcija.Unos("SELECT Zaposleni.id, ime, prezime , convert(varchar(10), datum_zaposlenja) AS 'Datum zaposlenja', Telefon, JMBG, email, Plata.Plata, Plata.Uloga, Lozinka FROM Zaposleni JOIN Plata ON Plata.id = Zaposleni.Plata_id");
            dataGridView1.DataSource = podaci;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int indeks = dataGridView1.CurrentRow.Index;

                textBox1.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["id"].Value);
                textBox2.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["ime"].Value);
                textBox4.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Datum zaposlenja"].Value);
                textBox5.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Telefon"].Value);
                textBox6.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["JMBG"].Value);
                textBox7.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["email"].Value);
                textBox8.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["plata"].Value);
                comboBox1.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Uloga"].Value);
                textBox3.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["prezime"].Value);
                textBox9.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Lozinka"].Value);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            podaci = new DataTable();
            podaci = Konekcija.Unos("SELECT Plata FROM Plata WHERE Uloga = '" + comboBox1.Text + "'");
            textBox8.Text = Convert.ToString(podaci.Rows[0][0]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da obrisete ove podatake?", "Bilijar klub", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("DELETE FROM Zaposleni WHERE id = " + textBox1.Text);

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
                    if (textBox2.Text == "" || comboBox1.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "")
                    {
                        MessageBox.Show("Sva polja moraju biti popunjena - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    if (textBox6.Text.Length != 13)
                    {
                        MessageBox.Show("JMBG mora imati 13 cifara! - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    podaci = new DataTable();
                    podaci = Konekcija.Unos("SELECT JMBG FROM Zaposleni WHERE id <> " + textBox1.Text);
                    for (int i = 0; i < podaci.Rows.Count; i++)
                    {
                        if (textBox6.Text == Convert.ToString(podaci.Rows[i]["JMBG"]))
                        {
                            MessageBox.Show("Dva zaposlena ne smeju imati isti JMBG! - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Osvezi();
                            return;
                        }
                    }

                    podaci = new DataTable();
                    podaci = Konekcija.Unos("SELECT Telefon FROM Zaposleni WHERE id <> " + textBox1.Text);
                    for (int i = 0; i < podaci.Rows.Count; i++)
                    {
                        if (textBox5.Text == Convert.ToString(podaci.Rows[i]["Telefon"]))
                        {
                            MessageBox.Show("Dva zaposlena ne smeju imati isti broj telefona! - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Osvezi();
                            return;
                        }
                    }

                    podaci = new DataTable(); //Trazenje id-a za platu
                    podaci = Konekcija.Unos("SELECT id FROM Plata WHERE Uloga = '" + comboBox1.Text + "' AND Plata = " + textBox8.Text);
                    int id_plate = (int)podaci.Rows[0][0];

                    podaci = new DataTable();
                    podaci = Konekcija.Unos("SELECT * FROM Zaposleni WHERE ime = '" + textBox2.Text + "' AND prezime = '" + textBox3.Text + "' AND Datum_zaposlenja = '" + textBox4.Text + "' AND Telefon = '" + textBox5.Text + "' AND JMBG = '" + textBox6.Text + "' AND Email = '" + textBox7.Text + "' AND Plata_id = " + id_plate + " AND Lozinka = '" + textBox9.Text + "'");
                    if (podaci.Rows.Count >= 1) throw new Exception();

                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("UPDATE Zaposleni SET ime = '" + textBox2.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Zaposleni SET prezime = '" + textBox3.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Zaposleni SET Datum_zaposlenja = '" + textBox4.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Zaposleni SET Telefon = '" + textBox5.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Zaposleni SET JMBG = '" + textBox6.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Zaposleni SET Email = '" + textBox7.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Zaposleni SET Lozinka = '" + textBox9.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Zaposleni SET Plata_id = " + id_plate + " WHERE id = " + textBox1.Text);

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
                    if (textBox2.Text == "" || comboBox1.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "")
                    {
                        MessageBox.Show("Sva polja moraju biti popunjena - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    if (textBox6.Text.Length != 13)
                    {
                        MessageBox.Show("JMBG mora imati 13 cifara! - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    podaci = Konekcija.Unos("SELECT * FROM Zaposleni WHERE JMBG = '" + textBox6.Text + "'");
                    if (podaci.Rows.Count >= 1)
                    {
                        MessageBox.Show("Dva zaposlena ne smeju imati isti JMBG! - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    podaci = Konekcija.Unos("SELECT * FROM Zaposleni WHERE Telefon = '" + textBox5.Text + "'");
                    if (podaci.Rows.Count >= 1)
                    {
                        MessageBox.Show("Dva zaposlena ne smeju imati isti broj telefona! - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    podaci = new DataTable(); //Trazenje id-a za platu
                    podaci = Konekcija.Unos("SELECT id FROM Plata WHERE Uloga = '" + comboBox1.Text + "' AND Plata = " + textBox8.Text);
                    int id_plate = (int)podaci.Rows[0][0];

                    podaci = new DataTable();
                    podaci = Konekcija.Unos("SELECT * FROM Zaposleni WHERE ime = '" + textBox2.Text + "' AND prezime = '" + textBox3.Text + "' AND Datum_zaposlenja = '" + textBox4.Text + "' AND Telefon = '" + textBox5.Text + "' AND JMBG = '" + textBox6.Text + "' AND Email = '" + textBox7.Text + "' AND Plata_id = " + id_plate + " AND Lozinka = '" + textBox9.Text + "'");
                    if (podaci.Rows.Count >= 1) throw new Exception();


                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("INSERT INTO Zaposleni VALUES ('" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + textBox6.Text + "', '" + textBox7.Text + "', " + id_plate + ", '" + textBox9.Text + "')");
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
