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
            podaci = Konekcija.Unos("SELECT Usluga.id, Dan, Vreme_dolaska, Vreme_odlaska, Zaposleni.id AS 'Oznaka zaposlenog', Zaposleni.ime + ' ' + Zaposleni.Prezime AS 'Zaposleni', Sto.Naziv AS 'Sto', Placanje, Trajanje FROM Usluga\r\nJOIN Zaposleni ON Zaposleni.id = Usluga.id_zaposlenog\r\nJOIN Sto ON Sto.id = Usluga.id_stola");
            dataGridView1.DataSource = podaci;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int indeks = dataGridView1.CurrentRow.Index;

                textBox1.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["id"].Value);
                textBox2.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Dan"].Value);
                textBox3.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Vreme_dolaska"].Value);
                textBox4.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Vreme_odlaska"].Value);
                textBox9.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Oznaka zaposlenog"].Value);
                comboBox1.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Zaposleni"].Value);
                comboBox2.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Sto"].Value);
                textBox7.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Placanje"].Value);
                textBox8.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Trajanje"].Value);
            }
        }
    }
}
