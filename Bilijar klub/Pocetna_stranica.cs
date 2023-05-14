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
    }
}
