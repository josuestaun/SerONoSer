using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerONoSer
{
    public partial class FrmNivel : Form
    {
        FrmPreguntas frmPreguntas;
        public FrmNivel(FrmPreguntas frmPreguntas)
        {
            InitializeComponent();
            this.frmPreguntas = frmPreguntas;
        }

        private void btn_Aceptar_Click(object sender, EventArgs e)
        {
            string numString = txt_lvl.Text;
            int number = 0;
            bool canConvert = int.TryParse(numString, out number);
            if (canConvert)
            {
                Program.nivelSeleccionado = number;
                this.Enabled = false;
                this.Hide();
                frmPreguntas.NextNivel(number);
            }
            else
            {
                MessageBox.Show("Escribe un número entero", "Atención");
            }
        }
    }
}
