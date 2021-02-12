using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using CapaDatos;
using CapaNegocio;
using Entidades;

namespace SerONoSer
{
    public partial class FrmPreguntas : Form
    {
        int contador;
        int nivelActual = 1;
        int numValidas;
        int numNoValidas;
        public static CapaNegocio.GestionNegocio gestNegocio;
        PreguntaDTO pregunta;
        public FrmPreguntas()
        {
            InitializeComponent();
        }
        private void FrmPreguntas_Load(object sender, EventArgs e)
        {
            ColorBotones();
            //txt_Nivel.Text = nivelActual.ToString();
            gestNegocio = new CapaNegocio.GestionNegocio(out string msg);
            if (!(msg.Equals("")))
            {
                MessageBox.Show(msg, "Atencion!");
                FinalizarPrograma();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (contador > 1)
            {
                contador--;
                txtTimer.Text = contador.ToString();
            }
            else
            {
                timer1.Stop();
                MessageBox.Show("Te has quedado sin tiempo", "Atención");
                //Preguntar si quiere seguir con la siguiente pregunta
                DialogResult dialogResult = MessageBox.Show("Te has quedado sin tiempo, ¿Quieres seguir con otra pregunta?", "Atención", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //siguiente Pregunta
                    NextPregunta();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //Finalizar programa
                    FinalizarPrograma();
                }
            }
        }
        private void checkEnd()
        {
            if (numValidas >= 8)
            {
                timer1.Stop();
                DialogResult dialogResult = MessageBox.Show("Has acertado las 8 respuestas validas, ¿Quieres seguir con otra pregunta?", "Atención", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //siguiente Pregunta
                    NextPregunta();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //Finalizar programa
                    FinalizarPrograma();
                }
            }
            else if (numNoValidas >= 4)
            {
                timer1.Stop();
                DialogResult dialogResult = MessageBox.Show("Has fallado las 4 respuestas erroneas, ¿Quieres seguir con otra pregunta?", "Atención", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //siguiente Pregunta
                    NextPregunta();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //Finalizar programa
                    FinalizarPrograma();
                }
            }
        }

        private void btn_comenzar_Click(object sender, EventArgs e)
        {
            FrmNivel frmNivel = new FrmNivel(this);
            frmNivel.Show();
        }
        public void NextNivel(int lvl)
        {
            gestNegocio.RecibeNivel(lvl, out string msg);
            if (msg != "")
            {
                MessageBox.Show(msg, "Atención");
                FinalizarPrograma();
                return;
            }
            NextPregunta();
        }
        public void NextPregunta()
        {
            this.nivelActual = Program.nivelSeleccionado;
            txt_Nivel.Text = nivelActual.ToString();
            timer1.Enabled = true;
            contador = 61;
            HabilitarBotones();
            ColorBotones();
            numValidas = 0;
            numNoValidas = 0;
            pregunta = gestNegocio.GetPregunta(nivelActual, out String msj);
            if (msj != "")
            {
                DialogResult dialogResult = MessageBox.Show(msj + ", Quieres ir al siguiente nivel", "Atención", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //siguiente nivel
                    Program.nivelSeleccionado++;
                    NextNivel(Program.nivelSeleccionado);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //Finalizar programa
                    FinalizarPrograma();
                }
                return;
            }
            //Enunciado
            txt_Enunciado.Text = pregunta.Enunciado;
            //Respuestas
            btn_1.Text = (pregunta.RespuestasDTO[0].PosibleRespuesta);
            btn_2.Text = (pregunta.RespuestasDTO[1].PosibleRespuesta);
            btn_3.Text = (pregunta.RespuestasDTO[2].PosibleRespuesta);
            btn_4.Text = (pregunta.RespuestasDTO[3].PosibleRespuesta);
            btn_5.Text = (pregunta.RespuestasDTO[4].PosibleRespuesta);
            btn_6.Text = (pregunta.RespuestasDTO[5].PosibleRespuesta);
            btn_7.Text = (pregunta.RespuestasDTO[6].PosibleRespuesta);
            btn_8.Text = (pregunta.RespuestasDTO[7].PosibleRespuesta);
            btn_9.Text = (pregunta.RespuestasDTO[8].PosibleRespuesta);
            btn_10.Text = (pregunta.RespuestasDTO[9].PosibleRespuesta);
            btn_11.Text = (pregunta.RespuestasDTO[10].PosibleRespuesta);
            btn_12.Text = (pregunta.RespuestasDTO[11].PosibleRespuesta);
        }

        private void ColorBotones()
        {
            btn_1.BackColor = Color.Gray;
            btn_2.BackColor = Color.Gray;
            btn_3.BackColor = Color.Gray;
            btn_4.BackColor = Color.Gray;
            btn_5.BackColor = Color.Gray;
            btn_6.BackColor = Color.Gray;
            btn_7.BackColor = Color.Gray;
            btn_8.BackColor = Color.Gray;
            btn_9.BackColor = Color.Gray;
            btn_10.BackColor = Color.Gray;
            btn_11.BackColor = Color.Gray;
            btn_12.BackColor = Color.Gray;
        }

        private void HabilitarBotones()
        {
            btn_1.Enabled = true;
            btn_2.Enabled = true;
            btn_3.Enabled = true;
            btn_4.Enabled = true;
            btn_5.Enabled = true;
            btn_6.Enabled = true;
            btn_7.Enabled = true;
            btn_8.Enabled = true;
            btn_9.Enabled = true;
            btn_10.Enabled = true;
            btn_11.Enabled = true;
            btn_12.Enabled = true;
        }

        private void FinalizarPrograma()
        {
            Application.Exit();
        }

        private void btn_finalizar_Click(object sender, EventArgs e)
        {
            FinalizarPrograma();
        }

        private void btn_1_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_1.Enabled = false;
            if (pregunta.RespuestasDTO[0].Valida)
            {
                numValidas++;
                btn_1.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_1.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[0].Explicacion;
            }
            checkEnd();
        }

        private void btn_2_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_2.Enabled = false;
            if (pregunta.RespuestasDTO[1].Valida)
            {
                numValidas++;
                btn_2.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_2.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[1].Explicacion;
            }
            checkEnd();
        }

        private void btn_3_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_3.Enabled = false;
            if (pregunta.RespuestasDTO[2].Valida)
            {
                numValidas++;
                btn_3.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_3.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[2].Explicacion;
            }
            checkEnd();
        }

        private void btn_4_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_4.Enabled = false;
            if (pregunta.RespuestasDTO[3].Valida)
            {
                numValidas++;
                btn_4.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_4.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[3].Explicacion;
            }
            checkEnd();
        }

        private void btn_5_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_5.Enabled = false;
            if (pregunta.RespuestasDTO[4].Valida)
            {
                numValidas++;
                btn_5.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_5.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[4].Explicacion;
            }
            checkEnd();
        }

        private void btn_6_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_6.Enabled = false;
            if (pregunta.RespuestasDTO[5].Valida)
            {
                numValidas++;
                btn_6.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_6.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[5].Explicacion;
            }
            checkEnd();
        }

        private void btn_7_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_7.Enabled = false;
            if (pregunta.RespuestasDTO[6].Valida)
            {
                numValidas++;
                btn_7.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_7.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[6].Explicacion;
            }
            checkEnd();
        }

        private void btn_8_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_8.Enabled = false;
            if (pregunta.RespuestasDTO[7].Valida)
            {
                numValidas++;
                btn_8.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_8.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[7].Explicacion;
            }
            checkEnd();
        }

        private void btn_9_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_9.Enabled = false;
            if (pregunta.RespuestasDTO[8].Valida)
            {
                numValidas++;
                btn_9.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_9.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[8].Explicacion;
            }
            checkEnd();
        }

        private void btn_10_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_10.Enabled = false;
            if (pregunta.RespuestasDTO[9].Valida)
            {
                numValidas++;
                btn_10.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_10.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[9].Explicacion;
            }
            checkEnd();
        }

        private void btn_11_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_11.Enabled = false;
            if (pregunta.RespuestasDTO[10].Valida)
            {
                numValidas++;
                btn_11.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_11.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[10].Explicacion;
            }
            checkEnd();
        }

        private void btn_12_Click(object sender, EventArgs e)
        {
            //la desactico para que no se pueda clicar mas
            btn_12.Enabled = false;
            if (pregunta.RespuestasDTO[11].Valida)
            {
                numValidas++;
                btn_12.BackColor = Color.Green;
            }
            else
            {
                numNoValidas++;
                btn_12.BackColor = Color.Red;
                txt_Correccion.Text = pregunta.RespuestasDTO[12].Explicacion;
            }
            checkEnd();
        }
    }
}
