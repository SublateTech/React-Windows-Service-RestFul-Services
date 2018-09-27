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
    public class ADCentrales
    {

        private const string PROC_CENTRALES_CREAMODIFICA = "PT_Central_CreaModifica";

        private const string PROC_CENTRALES_SELECT = "PT_Central_Select";

        private const string PROC_TELEFONO_CREA = "USP_TelefonoCrea";

        

         public ADCentrales()
        {
 
        }


         public bool setCentralCreaModifica(String NombreCentral, String ModeloCentral, String TipoFecha, Int32 PI_Fecha, Int32 CD_Fecha, Int32 PI_Discado, Int32 CD_Discado, Int32 PI_Usuario, Int32 CD_Usuario, Int32 PI_Anexo, Int32 CD_Anexo, Int32 PI_Hora, Int32 CD_Hora, Int32 PI_Tiempo, Int32 CD_Tiempo)
         {

             
           
             try
             {
                 SqlParameter[] parametros = {
                                                 new SqlParameter("@NombreCentral",NombreCentral),
                                                 new SqlParameter("@ModeloCentral",ModeloCentral),
                                                 new SqlParameter("@TipoFecha",TipoFecha),
                                                 new SqlParameter("@PI_Fecha",PI_Fecha),
                                                 new SqlParameter("@CD_Fecha",CD_Fecha),
                                                 new SqlParameter("@PI_Discado",PI_Discado),
                                                 new SqlParameter("@CD_Discado",CD_Discado),
                                                 new SqlParameter("@PI_Usuario",PI_Usuario),
                                                 new SqlParameter("@CD_Usuario",CD_Usuario),
                                                 new SqlParameter("@PI_Anexo",PI_Anexo),
                                                 new SqlParameter("@CD_Anexo",CD_Anexo),
                                                 new SqlParameter("@PI_Hora",PI_Hora),
                                                 new SqlParameter("@CD_Hora",CD_Hora),
                                                 new SqlParameter("@PI_Tiempo",PI_Tiempo),
                                                 new SqlParameter("@CD_Tiempo",CD_Tiempo)                 
                                             };

                 SqlHelper.ExecuteNonQuery(Conexion.connectionString(), PROC_CENTRALES_CREAMODIFICA, parametros);

                 return true;

             }
             catch
             {
                 return false;
             }
             
         }

         public DataTable getCentral(String nombreCentral, String modelo)
         {

             try
             {

                 SqlParameter[] parametros = {
                                             new SqlParameter("@NombreCentral",nombreCentral),
                                             new SqlParameter("@Modelo",modelo)
                                         };

                 return SqlHelper.ExecuteDataset(Conexion.connectionString(), PROC_CENTRALES_SELECT, parametros).Tables[0];
             }
             catch
             {
                 return new DataTable();
             }

         }


         public bool saveLlamada(String anexo, string discado,DateTime fecha, DateTime hora,string tiempo,string codigo,decimal precioventa, decimal preciocosto,int productoId, int monedaId, int usuarioID, int minutos, string anexoregistro,decimal descuento,int tipo,int troncalid)
         {
            /*
             * validacion de datos con los parametros
             * 
             */
            
            //SqlParameter dateParameter = new SqlParameter("@FECHA", SqlDbType.Date, 60);
            //dateParameter.Value = fecha;

            SqlParameter[] parametros = {
                                             new SqlParameter("@TelefonoID",""),
                                             new SqlParameter("@ANEXO",anexo),
                                             new SqlParameter("@DISCADO",discado),
                                             new SqlParameter("@FECHA",fecha),  
                                             new SqlParameter("@HORA",hora),
                                             new SqlParameter("@TIEMPO",tiempo),
                                             new SqlParameter("@CODIGO",codigo),
                                             new SqlParameter("@PRECIOVENTA",precioventa),
                                             new SqlParameter("@PRECIOCOSTO",preciocosto),
                                             new SqlParameter("@PRODUCTOID",productoId),
                                             new SqlParameter("@MONEDAID",monedaId),
                                             new SqlParameter("@UsuarioId",usuarioID),
                                             new SqlParameter("@Minutos",minutos),
                                             new SqlParameter("@AnexoRegistro",anexoregistro),
                                             new SqlParameter("@Descuento",descuento),
                                             new SqlParameter("@Tipo",tipo),
                                             new SqlParameter("@TroncalID",troncalid),
                                             new SqlParameter("@HotelID",0),
                                             new SqlParameter("@PuntoVentaID",0),
                                            
                                         };
             
                 SqlHelper.ExecuteNonQuery(Conexion.connectionString(), PROC_TELEFONO_CREA, parametros);
                 return true;

         }

         public DataTable centrales(Boolean onlyLine)
         {

             String query = "";
             if (onlyLine == true)
             {
                 query = "select nombrecentral from Centrales where modeloCentral='@' group by NombreCentral order by NombreCentral";
             }
             else
             {
                 query = "select nombrecentral from Centrales group by NombreCentral order by NombreCentral";
             }
             
             try
             {
                 return SqlHelper.ExecuteDataset(Conexion.connectionString(), CommandType.Text, query).Tables[0];
             }
             catch
             {
                 return new DataTable();
             }
         }

         public DataTable detallecentrales(String nombreCentral)
         {

             StringBuilder q = new StringBuilder();

             q.Append("SELECT NombreCentral, ModeloCentral, TipoFecha, PI_Fecha, CD_Fecha, PI_Discado, CD_Discado, ");

             q.Append("PI_Usuario, CD_Usuario, PI_Anexo, CD_Anexo, PI_Hora, CD_Hora, PI_Tiempo, CD_Tiempo  ");

             q.Append("FROM Centrales WHERE NombreCentral = '" + nombreCentral + "'");

             String query = q.ToString();

             try
             {
                 return SqlHelper.ExecuteDataset(Conexion.connectionString(), CommandType.Text, query).Tables[0];
             }
             catch(Exception ex)
             {
                 Console.WriteLine(ex.Message);
                 return new DataTable();
             }
         }

        public DataTable getPrecioVentaProducto(int productoID)
        {
            DataTable _dataTable = new DataTable();

            using (SqlConnection cnn = new SqlConnection(Conexion.connectionString()))
            {
                string SQL = "select top(1) MonedaId, PrecioVenta from productos where productoid = " + productoID.ToString();
                SqlCommand cmd = new SqlCommand(SQL, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();
                _dataTable.Load(dr);
            }

            return _dataTable;
        }

        public DataTable getproducto(string nro)
        {

            DataTable _dataTable = new DataTable();

            using (SqlConnection cnn = new SqlConnection(Conexion.connectionString()))
            {
                string SQL = "select productoid,preciocosto from productos where codigostelefonicos = '" + nro + "'";
                SqlCommand cmd = new SqlCommand(SQL, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();
                _dataTable.Load(dr);
            }




            return _dataTable;
        }

        public DataTable getProductDetails(string DialedNumber)
        {

            DataTable _dataTable = new DataTable();

            using (SqlConnection cnn = new SqlConnection(Conexion.connectionString()))
            {
                string SQL = $"select productoid,preciocosto  from productos p where codigostelefonicos = '{DialedNumber}'";
                SqlCommand cmd = new SqlCommand(SQL, cnn);
                cmd.CommandType = CommandType.Text;
                cnn.Open();
                SqlDataReader dr = null;
                dr = cmd.ExecuteReader();
                _dataTable.Load(dr);
            }




            return _dataTable;
        }

    }

    /*
    public static class SqlHelper
    {

        static private DataSet ds;
        static private DataTable dt;
        static private SqlDataAdapter da;

        public static DataSet ExecuteDataset(string connectionString, string store_procedure, SqlParameter[] parameteres)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                ds = null;

                // Addcontact is the name of the stored procedure
                using (SqlCommand cmd = new SqlCommand(store_procedure, conn))

                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // here we are passing the parameters that 
                    // Addcontact stored procedure expect.
                    //cmd.Parameters.Add("@CommandType",
                    //SqlDbType.VarChar, 50).Value = "GetAllContactType";

                    // here created the instance of SqlDataAdapter
                    // class and passed cmd object in it
                    da = new SqlDataAdapter(cmd);

                    // created the dataset object
                    ds = new DataSet();

                    // fill the dataset and your result will be
                    //stored in dataset
                    da.Fill(ds);
                }
                return ds;
            }

        }

        public static DataSet ExecuteDataset(string connectionString, CommandType command, string query)
            {
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                ds = null;
                
                // Addcontact is the name of the stored procedure
               using ( SqlCommand cmd =  new SqlCommand(query, conn))

                {
                    cmd.CommandType = command;

                    // here we are passing the parameters that 
                    // Addcontact stored procedure expect.
                    //cmd.Parameters.Add("@CommandType",
                    //SqlDbType.VarChar, 50).Value = "GetAllContactType";

                    // here created the instance of SqlDataAdapter
                    // class and passed cmd object in it
                    da = new SqlDataAdapter(cmd);

                    // created the dataset object
                    ds = new DataSet();

                    // fill the dataset and your result will be
                    //stored in dataset
                    da.Fill(ds);
                }
                return ds;
            }
            
        }

        static public ExecuteScalar(string connString, CommandType command, string query)
        {
            Int32 newProdID = 0;
            string sql =
                "INSERT INTO Production.ProductCategory (Name) VALUES (@Name); "
                + "SELECT CAST(scope_identity() AS int)";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@Name", SqlDbType.VarChar);
                cmd.Parameters["@name"].Value = newName;
                try
                {
                    conn.Open();
                    newProdID = (Int32)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return (int)newProdID;
        }

        public static DataTable ExecuteNonQuery(string connectionString, string store_procedure, SqlParameter[] parameteres)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                ds = null;

                // Addcontact is the name of the stored procedure
                using (SqlCommand cmd = new SqlCommand(store_procedure, conn))

                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    // here created the instance of SqlDataAdapter
                    // class and passed cmd object in it
                    da = new SqlDataAdapter(cmd);

                    // created the dataset object
                    ds = new DataSet();

                    // fill the dataset and your result will be
                    //stored in dataset
                    da.Fill(ds);
                }
                return ds.Tables[0];
            }
        }

    }
    */

}
