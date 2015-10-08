using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombateMultiplayer
{
    public partial class TelaInicial : Form
    {

        Thread ThreadEnviadoraDeCodenome;

        private Socket socketUDP;


        public TelaInicial()
        {
            InitializeComponent();
        }

        private void TelaInicial_Load(object sender, EventArgs e)
        {
            ThreadEnviadoraDeCodenome = new Thread(EnviaCodenome);
        }

        void EnviaCodenome(Object o) { 
        
        
        
        
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        


      
    }
}
