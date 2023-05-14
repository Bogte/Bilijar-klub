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
    public partial class Cenovnik : Form
    {
        DataTable podaci;
        SqlCommand menjanja;

        public Cenovnik()
        {
            InitializeComponent();
        }

        private void Cenovnik_Load(object sender, EventArgs e)
        {
            Osvezi();
        }

        private void Osvezi()
        {
            podaci = new DataTable();
            podaci = Konekcija.Unos("SELECT id, Cena_po_minutu AS 'Cena po minutu (Din)', LEFT(Otvaranje, 5) AS 'Otvaranje', LEFT(Zatvaranje, 5) AS 'Zatvaranje' FROM Cenovnik");
            dataGridView1.DataSource = podaci;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int indeks = dataGridView1.CurrentRow.Index;

                textBox1.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["id"].Value);
                textBox2.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Cena po minutu (Din)"].Value);
                textBox3.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Otvaranje"].Value);
                textBox4.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Zatvaranje"].Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da izmenite ove podatke?", "Bilijar klub", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                    {
                        MessageBox.Show("Sva polja moraju biti popunjena - Bilijar klub", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Osvezi();
                        return;
                    }

                    podaci = new DataTable();
                    podaci = Konekcija.Unos("SELECT * FROM Cenovnik WHERE Cena_po_minutu = " + textBox2.Text + " AND Otvaranje = '" + textBox3.Text + "' AND Zatvaranje = '" + textBox4.Text + "'");
                    if (podaci.Rows.Count >= 1) throw new Exception();

                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("UPDATE Cenovnik SET Cena_po_minutu = " + textBox2.Text + " WHERE id = " + textBox1.Text +
                        " UPDATE Cenovnik SET Otvaranje = '" + textBox3.Text + "' WHERE id = " + textBox1.Text +
                        " UPDATE Cenovnik SET Zatvaranje = '" + textBox4.Text + "' WHERE id = " + textBox1.Text);

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
