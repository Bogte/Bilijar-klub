using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bilijar_klub
{
    public partial class Pocetna_stranica : Form
    {
        public Pocetna_stranica()
        {
            InitializeComponent();
        }

        private void cenovnikToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cenovnik cenovnik = new Cenovnik();
            cenovnik.ShowDialog();
        }

        private void stoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sto sto = new Sto();
            sto.ShowDialog();
        }

        private void plataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Plata plata = new Plata();
            plata.ShowDialog();
        }

        private void zaposleniToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Zaposleni zaposleni = new Zaposleni();
            zaposleni.ShowDialog();
        }
    }
}
