using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using Entidades;

namespace CapaNegocio
{
    public class GestionNegocio
    {
        public static CapaDatos.GestionDataSet gestDatos;
        private List<PreguntaDTO> listaPreguntas;
        public GestionNegocio(out string message)
        {
            message = "";
            gestDatos = new CapaDatos.GestionDataSet(out string msg);
            message = msg;
        }
        public void RecibeNivel(int nivel, out string msg)
        {
            msg = "";
           listaPreguntas = gestDatos.PreguntasDeNivel(nivel, out string mssg);
            msg = mssg;
            //this.GetPregunta(nivel, out string msj);
        }
        
        public PreguntaDTO GetPregunta(int nivel, out string msj)
        {
            msj = "";
            Random rnd = new Random();
            if (listaPreguntas.Count() < 1)
            {
                msj = "Se han acabado las preguntas de este nivel";
                return null;
            }
            int aleatorio = rnd.Next(0, listaPreguntas.Count());
            PreguntaDTO pregunta = listaPreguntas[aleatorio];
            listaPreguntas.RemoveAt(aleatorio);
            return pregunta;

            //Random rnd = new Random();
            //List<PreguntaDTO> listaABorrar = gestDatos.PreguntasDeNivel(nivel, out string msg);
            //msj = msg;
            //if (msj != "")
            //{
            //    return null;
            //}
            //int aleatorio = rnd.Next(1, listaABorrar.Count);
            //PreguntaDTO pregunta = listaABorrar[aleatorio];
            //listaABorrar.RemoveAt(aleatorio);
            //return pregunta;
        }
    }
}
