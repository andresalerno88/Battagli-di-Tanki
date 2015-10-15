using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombateMultiplayer
{
    public partial class TelaConvite : Form
    {
        public TelaConvite(string str)
        {
            InitializeComponent();
            label1.Text = String.Format("{0} mijou no seu quintal,\n te chamou de mariquinha\n e peidou na cara da sua avó.\n Vai deixar barato?",str);
        }
    }
}
