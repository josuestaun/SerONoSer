using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos.dsCulturaTableAdapters;
using Entidades;

namespace CapaDatos
{
    public class GestionDataSet
    {
        dsCultura dsCultura = new dsCultura();

        public GestionDataSet(out String msj)
        {
            msj = GestionarTablas();
        }

        public String GestionarTablas()
        {
            //Importamos los adaptadores para las tablas del DataSet
            PreguntasTableAdapter preguntasTableAdapter = new PreguntasTableAdapter();

            RespNoValidasTableAdapter respNoValidasTableAdapter = new RespNoValidasTableAdapter();

            RespuestasTableAdapter respuestasTableAdapter = new RespuestasTableAdapter();

            String errormsj = "";
            //Llenamos las tablas
            try
            {
                //llenar tablas (si se conecta a la base de datos)
                preguntasTableAdapter.Fill(dsCultura.Preguntas);
                respNoValidasTableAdapter.Fill(dsCultura.RespNoValidas);
                respuestasTableAdapter.Fill(dsCultura.Respuestas);

                //control de errores
                int nivelMinimo = 1;
                int nivelMaximo = (from preg in dsCultura.Preguntas
                                   select preg.Nivel).Max();

                //comprobar si tiene preguntas
                if (dsCultura.Preguntas.Count < 1)
                {
                    errormsj+= "No hay preguntas en la base de datos";
                }
              
                foreach (var drPpreg in dsCultura.Preguntas)
                {
                    //comprobar que cada pregunta tenga 12 respuestas
                    int numPreg = drPpreg.NumPregunta;
                    if (drPpreg.GetRespuestasRows().Count() != 12)
                    {
                        errormsj += "\nNo hay 12 respuestas para la pregunta" + numPreg;
                    }
                    else
                    {
                        //Comprobar que cada pregunta tiene 8 respuestas validas y 4 no validas
                        int numValidas = 0;
                        int numNovalidas = 0;
                        foreach (var drResp in drPpreg.GetRespuestasRows())
                        {
                            if (drResp.Valida)
                            {
                                numValidas++;
                            }
                            else
                            {
                                numNovalidas++;
                            }
                        }
                        if (numNovalidas != 4 && numValidas != 8)
                        {
                            errormsj += "\nLa pregunta: " + numPreg + " no tiene 8 respuestas validas y 4 no validas";
                        }
                    }
                }

                //comprobar que al menos hay 1 pregunta en cada nivel
                for (int i = nivelMinimo; i <= nivelMaximo; i++)
                {
                    var numPreguntas = (from preg in dsCultura.Preguntas
                                           where preg.Nivel == i
                                           select preg.NumPregunta);
                    if (numPreguntas.Count()  == 0)
                    {
                        errormsj += "\nEl nivel máximo es " + nivelMaximo + " pero no hay preguntas en el nivel: " + i;
                    }
                }

            }
            catch (Exception exc)
            {
                if (errormsj != "") errormsj = "\n";
                errormsj += exc.Message;
            }
            return errormsj;
        }

        public List<PreguntaDTO> PreguntasDeNivel(int nivel, out String msg)
        {
            msg = "";
            List<PreguntaDTO> listaPreguntas = new List<PreguntaDTO>();
            int nivelMaximo = (from preg in dsCultura.Preguntas
                               select preg.Nivel).Max();
            if (nivel > nivelMaximo)
            {
                msg = "El nivel no puede ser superior a Máximo";
                //return listaPreguntas;
                return null;
            }

            listaPreguntas = (from drPreg in dsCultura.Preguntas
                                 where drPreg.Nivel == nivel
                                 select new PreguntaDTO(drPreg.NumPregunta,
                                 drPreg.Enunciado,
                                 drPreg.Nivel,
                                 RespuestasPregunta(drPreg.GetRespuestasRows()))).ToList();
            if (listaPreguntas.Count() < 1)
            {
                msg = "No hay preguntas en el nivel " + nivel;
            }
            return listaPreguntas;
        }
        private List<RespuestaDTO> RespuestasPregunta(dsCultura.RespuestasRow[] respuestasRows)
        {
            List<RespuestaDTO> respuestaDTOs = new List<RespuestaDTO>();
            foreach (dsCultura.RespuestasRow respuesta in respuestasRows)
            {
                if (respuesta.GetRespNoValidasRows().Count() >= 1)
                {
                    respuestaDTOs.Add(new RespuestaDTO(respuesta.NumPregunta, respuesta.NumRespuesta, respuesta.PosibleRespuesta, respuesta.Valida,
                      respuesta.GetRespNoValidasRows()[0].Explicacion));
                }
                else
                {
                    if (respuesta.Valida)
                    {
                        respuestaDTOs.Add(new RespuestaDTO(respuesta.NumPregunta, respuesta.NumRespuesta, respuesta.PosibleRespuesta, respuesta.Valida));
                    }
                    else
                    {
                        respuestaDTOs.Add(new RespuestaDTO(respuesta.NumPregunta, respuesta.NumRespuesta, respuesta.PosibleRespuesta, respuesta.Valida, "No tiene explicacion, búscala tu mismo"));
                    }
                }
            }
            return respuestaDTOs;
        }

    }
}
