using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace AD
{
    public class ADConfiguracion
    {

        private const string PROC_PUERTOSERIE = "PT_PuertoSerie";

        private const string PROC_PUERTOSERIE_SELECT = "PT_PuertoSerie_Select";

        public ADConfiguracion()
        {
 
        }

        public bool setConfiguracion(String puertoCOM, Int32 velocidadPuerto, Int32 bitsDatos,Parity paridad, Handshake controlFlujoSoftware, string rutaArchivoRegistro, Int32 tiempoMinimoLlamada)
        {
            try
            {
                string query = PROC_PUERTOSERIE;

                SqlParameter[] parametros = {
                                                new SqlParameter("@NombrePuerto",puertoCOM),
                                                new SqlParameter("@velocidadPuerto",velocidadPuerto),
                                                new SqlParameter("@bitsDatos",bitsDatos),
                                                new SqlParameter("@paridad",paridad),
                                                new SqlParameter("@controlFlujoSW",controlFlujoSoftware),
                                                new SqlParameter("@rutaArchivoRegistro",rutaArchivoRegistro),
                                                new SqlParameter("@tiempoMinimoLlamada",tiempoMinimoLlamada)
                                            };


                SqlHelper.ExecuteNonQuery(Conexion.connectionString(), query, parametros);



                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataTable getConfiguracion()
        {
            try
            {
                return SqlHelper.ExecuteDataset(Conexion.connectionString(), CommandType.StoredProcedure, PROC_PUERTOSERIE_SELECT).Tables[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DataTable();
            }
        }

        public String getRutaRegistro()
        {
            String query = "SELECT RTRIM(Valor) FROM ParametrosGenerales WHERE ParametroGeneralId = 3505";
            try
            {
                return SqlHelper.ExecuteScalar(Conexion.connectionString(), CommandType.Text, query).ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public Int32 getTiempoMinimo()
        {
            String query = "SELECT RTRIM(Valor) FROM ParametrosGenerales WHERE ParametroGeneralId = 3506";

            object valor;

            try
            {

                valor = SqlHelper.ExecuteScalar(Conexion.connectionString(), CommandType.Text, query);

                return Convert.ToInt32(valor);

            }
            catch
            {
                return 0;
            }
        }

        public Boolean setTiempoMinimo(Int32 tiempo)
        {
            String query = "UPDATE ParametrosGenerales SET Valor = " + tiempo.ToString() + " WHERE ParametroGeneralId = 3506";
            try
            {
                SqlHelper.ExecuteNonQuery(Conexion.connectionString(), CommandType.Text, query);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string getDelimitadores()
        {
            String query = "SELECT Valor FROM ParametrosGenerales WHERE ParametroGeneralId = 3507";
            try
            {
                return SqlHelper.ExecuteScalar(Conexion.connectionString(), CommandType.Text, query).ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

		public bool verificaServidor()

        {
            SqlConnection cnxSql = new SqlConnection(Conexion.connectionString());

            //prueba si se conecta al servidor;
            try
            {
                
                cnxSql.Open();
                cnxSql.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}
