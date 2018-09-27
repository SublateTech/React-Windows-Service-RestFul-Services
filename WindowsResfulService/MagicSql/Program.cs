using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SqlMagic.Monitor.Sql;
using SqlMagic.Monitor.Extensions;
using SqlMagic.Monitor.Items.Results;
using System.Diagnostics;
using System.Configuration;



namespace TestConsola
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test();
            TranferenciaVentas();
            
        }

        static void Test()
        {
            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;
            var connectionString = "user id=sa;password=server$123$;data source=localhost;initial catalog=Shiol_Datos_MLP;Connection Timeout=30;";

            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);

            // Now let's roll!
            SqlResultWithDataSet oResult = oSql.Open("SELECT * FROM ParametrosGenerales");
            DataSet dataSet = oResult.Results;



            //// I'm going to re-use oSql and oResult from above ^^^

            oResult = oSql.Open("SELECT * FROM ParametrosGenerales WHERE Estado = @id",
                CommandType.Text,
                new SqlParameter("id", 1)
            );


            //SQLMagic makes it incredibly simple to write proper queries:

            /*
            oResult = oSql.Open("SELECT * FROM tbl WHERE tbl_id IN (@id1, @id2, @id3)",
                CommandType.Text,
                new SqlParameter("id1", 1),
                new SqlParameter("id2", 2),
                new SqlParameter("id3", 3)
            );
            */

            // Stored Procedures with parameters?
            oSql.Execute("sp_Productos",
                CommandType.StoredProcedure,
                new SqlParameter("TipoProductoID", "1224")
            );

            var intBack = (SqlResult)oSql.Execute("sp_Productos",
                CommandType.StoredProcedure,
                new SqlParameter("TipoProductoID", "1224")
            );

            // What about return values?
            /* var datetime = oSql.Execute<DateTime>("SELECT GETDATE()");
             var intBack = oSql.Execute<Int32>("SELECT 1");*/

            // Stored Procedure that has a return value?
            var intBackAgain = oSql.Execute<Int32>("sp_Productos", CommandType.StoredProcedure);



            //Asynchronous Support

            //SQLMagic, with compilation flag "NET45" set, supports the async/await model of.NET, and implements it in a very clean, easy to use manner:

            // Converting is VERY easy!
#if NET45
            var oResult = await oSql.OpenAsync("SELECT * FROM aVeryLargeTable");

            var nValue = await oSql.ExecuteAsync<Int32>("SELECT bigNumberComputationThatResultsInInteger");

            var nProcedure = await oSql.ExecuteAsync<Int32>("sp_LongRunningProcedure", CommandType.StoredProcedure);
#endif


            //Transaction Support

            //SQLMagic gives you the ability to Begin, Commit, and RollBack transactions with and without asychronous capability:

            // Converting is VERY easy!
#if NET45
            // Start a Transaction:
            SqlTransaction oTransaction = oSql.BeginTransaction();

            // The asynchronous version:
            SqlTransaction oTransaction = await oSql.BeginTransactionAsync();

            // one of the overloads that SQLMagic has is the ability to specify a transaction!
            // public SqlResultWithDataSet Open(Statement aStatement, SqlConnection aConnection, Boolean aCloseConnection, SqlTransaction aTransaction)
            // Statement is merely a SQLMagic struct that groups up some parameters from earlier:
            Statement oStatement = new Statement
            {
                Sql = "INSERT INTO tbl VALUES(@val1)",
                Type = CommandType.Text,
                Parameters = new List<SqlParameters>() { new SqlParameter("val1", "value") }
            };
            oSql.Open(oStatement, oTransaction.Connection, false, oTransaction);

            // Open a DataSet of the given statement, connection, don't close it, and use that transaction

            // Once you're all done, you end!
            oSql.EndTransaction(oTransaction, true); // true or false indicates COMMIT or ROLLBACK!

            // The asynchronous version:
            await oSql.EndTransactionAsync(oTransaction);
#endif

            //Manual Connection Creation
            //SQLMagic lets you create connections yourself(remember that SQLMagic has overloads to indicate a SqlConnection object!):


            // Synchronous
            SqlConnection oConnection = oSql.CreateConnection(true);

            // Converting is VERY easy!
#if NET45
            // Asynchronous
            SqlConnection oConnection = await oSql.CreateConnectionAsync(true);
#endif
            // The boolean value indicates whether or not the connection is automatically opened when created

            //TryOpen/TryExecute/TryExecute<T>

            //SQLMagic can attempt to execute a query for you and, instead of throwing an exception, will simply return a Boolean value that indicates success, and use an out parameter to store your result:

            // Variables
            //SqlResultWithDataSet oResult;
            if (oSql.TryOpen("SELECT * FROM tbl WHER tbl.id = 1", out oResult))
            {
                // This fails because of syntax   ^
            }
            else
            {
                // Because it failed, you can handle the exception here!
                //MessageBox.Show(oResult.Exception.ToString());
            }
        }
        static void TranferenciaVentas()
        {
            DateTime dateDesde = new DateTime(2016, 01, 01);
            DateTime dateHasta = new DateTime(2016, 12, 30);
            String Usuario = "5093";


            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;
            //var connectionString1 = "user id=sa;password=server$123$;data source=localhost;initial catalog=SHIOL_DATOS_MLP;Connection Timeout=30;";
            var connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);

            Console.WriteLine("Start : " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"));
            var watch = Stopwatch.StartNew();

            var strSql = "Select valor from ParametrosGenerales where parametrogeneralID = '2010'";
            SqlResultWithDataSet oResult = oSql.Open(strSql, CommandType.Text);

            var EstablecimientoID = oResult.Results.Tables[0].Rows.Count == 1 ? oResult.Results.Tables[0].Rows[0]["valor"].ToString().Trim() : "";
            var Periodo = dateDesde.Year.ToString();
            var NombreTablaCabecera = "ACC_" + EstablecimientoID + "_" + Periodo;
            var NombreTablaDetalle = "ACD_" + EstablecimientoID + "_" + Periodo;

            //Deleting Header and Detail Tables
            oSql.Open("Delete from " + NombreTablaCabecera + ";"
                + "Delete from " + NombreTablaDetalle,
               CommandType.Text
           );

            // Creating Local Virtual Tables Header and Detail
            oResult = oSql.Open(@"SET FMTONLY ON; 
                                 Select TOP 1 * from " + NombreTablaCabecera + ";" +   
                                  "Select TOP 1 * from " + NombreTablaDetalle + "; SET FMTONLY OFF;",
               CommandType.Text
           );

            DataTable tablaHeader = new DataTable();
            DataTable tablaDetail = new DataTable();
            if (oResult.Results.Tables.Count < 2)
                Console.WriteLine("No se encuantran las tablas:" +
                    NombreTablaCabecera + " o " + NombreTablaDetalle +
                    " de este periodo en la base de datos");
            else
            {
                tablaHeader = oResult.Results.Tables[0];
                tablaHeader.TableName = "ACC";
                tablaDetail = oResult.Results.Tables[1];
                tablaDetail.TableName = "ACD";
            }
            
            SqlResultWithDataSet oResult1 = oSql.Open("UCOTRA_VENTAS_SQL",
               CommandType.StoredProcedure,
               new SqlParameter("Desde", dateDesde.ToString("dd-MM-yyyy")),
               new SqlParameter("Hasta", dateHasta.ToString("dd-MM-yyyy")),
               new SqlParameter("UsuarioID", "0"),
               new SqlParameter("Movimiento", "0"),
               new SqlParameter("IPEquipo", "0"),
               new SqlParameter("UsuarioactualEquipo", "0"),
               new SqlParameter("NombreEquipo", "0")
           );
            DataSet dsTables = oResult1.Results;


            //DataSet dataSet = oResult.Results;
            dsTables.Tables[0].TableName = "Documentos";
            DataTable tablaDocumentos = dsTables.Tables[0];
            dsTables.Tables[1].TableName = "PlanDeCuentas";
            DataTable tablaPlanDeCuentas = dsTables.Tables[1];
            dsTables.Tables[2].TableName = "DetalleEventos";
            DataTable tablaDetalleEventos = dsTables.Tables[2];



            /*
            //Establecer Relaciones
            DataRelation planDeCuentasRel = dsTables.Relations.Add("PlanDeCuenta",
               new DataColumn[] { dsTables.Tables["Documentos"].Columns["EstablecimientoID1"] , dsTables.Tables["Documentos"].Columns["PlanCuentaID"] },
               new DataColumn[] { dsTables.Tables["PlanDeCuentas"].Columns["EstablecimientoID"],  dsTables.Tables["PlanDeCuentas"].Columns["PlanCuentaID"]}, false);
            */

            //      Console.WriteLine("Records : " + tablaDocumentos.Rows.Count.ToString());




            /******************************************
             * 
             *  Aquí empieza propiamente el proceso
             *               
             ******************************************/

            //Correlativos Para Formato de Comprobante Cabecera
            Int32 RegistroID = 0; // CorrelativoIDMensual = 0;
            Int32 CorrelativoIDMensualComprobanteAsiento = 0;

            
            Double TotalDebeCuadre = 0.00;
            Double TotalHaberCuadre = 0.00;


            Double TotalDebeSolesCuadre = 0.00;
            Double TotalDebeDolaresCuadre = 0.00;
            Double TotalHaberSolesCuadre = 0.00;
            Double TotalHaberDolaresCuadre = 0.00;

            Int32 AsientoContableID = 0;
            Int32 AsientoContableDetalleID = 0;

            String PlanCuentaID = "";

            foreach (DataRow rowDocumento in dsTables.Tables["Documentos"].Rows)
            {

                String DocumentoId = rowDocumento["DocumentoId"].ToString();
                //    Console.WriteLine(rowDocumento["TransaccionDocumentoId"] + " - " + rowDocumento["TipoDocumentoID"]);

                Int32 EmpresaID = 0;
                if (Convert.ToDouble(rowDocumento["IGV"].ToString()) == 0 && Convert.ToInt32(rowDocumento["Turista"].ToString()) == 1)
                    EmpresaID = Convert.ToInt32(rowDocumento["EmpresaIDTuristas"].ToString());
                else
                {
                    if (DocumentoId.Substring(0, 2) == "03" ||
                        DocumentoId.ToString().Substring(0, 2) == "123" ||
                        DocumentoId.ToString().Substring(0, 2) == "12B")
                        EmpresaID = Convert.ToInt32(rowDocumento["EmpresaIDParticular"].ToString());
                }

               

                /*
                var intBack = (SqlResult)oSql.Execute("sp_ACC_ExisteRegistro",
                CommandType.StoredProcedure, [
                    new SqlParameter("NombreTabla", NombreTabla),
                    new SqlParameter("ReferenciaSHIOL", rowDocumento["TransaccionDocumentoID"].ToString().Trim()),
                    new SqlParameter("TablaMovimientoID", rowDocumento["TablaMovimientoID"].ToString().Trim())
                    ] 
                );
                */

                var TablaMovimientoID = "137"; //Indica que tipo de provision de venta se Hace

                /*
                strSql = "Select Count(*) FROM " + NombreTabla
                       + " Where ReferenciaSHIOL = '" + rowDocumento["TransaccionDocumentoID"].ToString().Trim() + "' "
                       + " AND TablaMovimientoID = '" + TablaMovimientoID + "'";

                oResult = oSql.Open(strSql, CommandType.Text);
                var LPRegistrosExiste = oResult.Results.Tables[0].Rows.Count == 1 ? true : false;


                Console.WriteLine(LPRegistrosExiste.ToString());
                */
                //Console.WriteLine(rowDocumento["TransaccionDocumentoId"] + " - " + rowDocumento["TipoDocumentoID"]);

                // Console.WriteLine(EmpresaID.ToString());

                //  Console.WriteLine(rowDocumento["TransaccionDocumentoId"].ToString());

                if (AsientoContableID == 0)
                {
                    var intBack = oSql.Execute<Int32>("USP_GeneraID",
                    CommandType.StoredProcedure,
                         new SqlParameter("lpTabla", "ASIENTOSCONTABLES_" + Periodo),
                         new SqlParameter("Codigo", "0"));
                    AsientoContableID = intBack.Value;
                }
                else
                {
                    AsientoContableID++;
                }

                if (AsientoContableDetalleID == 0)
                {
                    var intBack = oSql.Execute<Int32>("USP_GeneraID",
                    CommandType.StoredProcedure,
                         new SqlParameter("lpTabla", "AsientosContablesDetalle_" + Periodo),
                         new SqlParameter("Codigo", "0"));
                    AsientoContableDetalleID = intBack.Value;
                }
                else
                {
                    AsientoContableDetalleID++;
                }

                String CodigoComprobante = "";
               if (rowDocumento["FormatoComprobante"].ToString().Trim() == "1")
                { 
                    if ( RegistroID == 0) {

                        //Console.WriteLine(rowDocumento["SubDiarioID"].ToString());
                        //Console.WriteLine(rowDocumento["FechaEmision"].ToString());
                        var intBack = oSql.Execute<Int32>("USP_GeneraIDMensual",
                           CommandType.StoredProcedure,  
                                new SqlParameter("MemoID", "0"),
                                new SqlParameter("UsuarioID", "0"),
                                new SqlParameter("Formato", rowDocumento["SubDiarioID"].ToString()),
                                new SqlParameter("Dia", rowDocumento["FechaEmision"].ToString()),
                                new SqlParameter("Codigo", "0")
                        
                       );
                        RegistroID = intBack.Value;
                    } else { RegistroID ++ ; }

                    
                    DateTime FechaEmision = DateTime.Parse(rowDocumento["FechaEmision"].ToString());

                    CodigoComprobante = (FechaEmision.Month < 10 ? "0" : "")
                        + FechaEmision.Month.ToString().Trim()
                        + rowDocumento["HotelIDformato"].ToString()
                        + rowDocumento["Localidad_CODIGO"].ToString()
                        + "0000"
                        + RegistroID.ToString().PadLeft(5,'0').Substring(RegistroID.ToString().PadLeft(5, '0').Length-5, 5);
                    
                   // AsientoContableID = RegistroID;
                 } else { 
                    if (CorrelativoIDMensualComprobanteAsiento == 0)
                    {
                        
                        var intBack = oSql.Execute<Int32>("UCO_GeneraIDMensualComprobanteAsiento",
                           CommandType.StoredProcedure,
                                new SqlParameter("Periodo", rowDocumento["Periodo"].ToString()),
                                new SqlParameter("Mensual", rowDocumento["Mes"].ToString()),
                                new SqlParameter("EstablecimientoID", rowDocumento["EstablecimientoID"].ToString()),
                                new SqlParameter("HotelID", rowDocumento["HotelID"].ToString()),
                                new SqlParameter("SubDiarioID", rowDocumento["SubDiarioID"].ToString()),
                                new SqlParameter("Codigo","0")
                       );
                       CorrelativoIDMensualComprobanteAsiento = intBack.Value;

                    } else { CorrelativoIDMensualComprobanteAsiento++; }
                    AsientoContableID = CorrelativoIDMensualComprobanteAsiento;
                }

                // Insertar en Tabla de Cabecera 
                AddHeaderTable(AsientoContableID, CodigoComprobante, TablaMovimientoID, tablaHeader, rowDocumento);

                //Insertar Detalle Step 001
                Double Total = Convert.ToDouble(rowDocumento["total"]);
                Double MontoDebe = Total > 0 ? Math.Round(Total, 2) : 0.00;
                Double MontoHaber = Total > 0 ? 0.00 : Math.Round(Total, 2);
                String Referencia = rowDocumento["Cliente"].ToString();
                Double ImporteSoles = rowDocumento["Cliente"].ToString() == "1" ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round((MontoDebe + MontoHaber) * Convert.ToDouble(rowDocumento["TipoCambio"]), 2);
                Double ImporteDolares = rowDocumento["Cliente"].ToString() == "2" ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round(MontoDebe + MontoHaber,2);

                String AsientoContableDetallIDTransferencia = "0";

                
                TotalDebeCuadre += MontoDebe;
                TotalHaberCuadre += MontoHaber;
                                
                if (MontoDebe > 0)
                {
                    TotalDebeSolesCuadre += ImporteSoles;
                    TotalDebeDolaresCuadre += ImporteDolares;
                } else
                {
                    TotalHaberSolesCuadre += ImporteSoles;
                    TotalHaberDolaresCuadre += ImporteDolares;
                }

                PlanCuentaID = rowDocumento["PlanCuentaID"].ToString();
                MontoHaber = Convert.ToDouble(rowDocumento["Total"].ToString()) > 0 ? 0.00 : Convert.ToDouble(rowDocumento["Total"].ToString());
                MontoDebe = Convert.ToDouble(rowDocumento["Total"].ToString()) > 0 ? Convert.ToDouble(rowDocumento["Total"].ToString()) : 0.00;
                ImporteSoles = rowDocumento["Cliente"].ToString() == "1" ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round((MontoDebe + MontoHaber) * Convert.ToDouble(rowDocumento["TipoCambio"]), 2);
                ImporteDolares = rowDocumento["Cliente"].ToString() == "2" ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round(MontoDebe + MontoHaber, 2);
                AddDetailTable(rowDocumento["Cliente"].ToString(), PlanCuentaID, AsientoContableID, AsientoContableDetalleID, MontoDebe, MontoHaber, ImporteSoles, ImporteDolares, TablaMovimientoID, tablaDetail, rowDocumento);

                if (Convert.ToInt32(rowDocumento["Estado"].ToString()) != 3)
                {
                    if (Convert.ToDouble(rowDocumento["IGV"].ToString()) != 0.00)
                    {
                        PlanCuentaID = rowDocumento["CuentaIGV"].ToString();
                        Referencia = rowDocumento["Cliente"].ToString();
                        MontoDebe = Convert.ToDouble(rowDocumento["Total"].ToString()) > 0 ? 0.00 : Convert.ToDouble(rowDocumento["IGV"].ToString());
                        MontoHaber = Convert.ToDouble(rowDocumento["Total"].ToString()) > 0 ? Convert.ToDouble(rowDocumento["IGV"].ToString()) : 0.00;
                        ImporteSoles = rowDocumento["Cliente"].ToString() == "1" ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round((MontoDebe + MontoHaber) * Convert.ToDouble(rowDocumento["TipoCambio"]), 2);
                        ImporteDolares = rowDocumento["Cliente"].ToString() == "2" ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round(MontoDebe + MontoHaber, 2);
                        AddDetailTable(Referencia, PlanCuentaID, AsientoContableID, AsientoContableDetalleID, MontoDebe, MontoHaber, ImporteSoles, ImporteDolares, TablaMovimientoID, tablaDetail, rowDocumento);
                    }

                    if (Convert.ToDouble(rowDocumento["Servicio"].ToString()) != 0.00)
                    {
                        PlanCuentaID = rowDocumento["CuentaServicio"].ToString(); ;
                        Referencia = rowDocumento["Cliente"].ToString();
                        MontoHaber = Convert.ToDouble(rowDocumento["Total"].ToString()) >0 ? 0.00 : Convert.ToDouble(rowDocumento["Servicio"].ToString());
                        MontoDebe  = Convert.ToDouble(rowDocumento["Total"].ToString()) > 0 ? Convert.ToDouble(rowDocumento["Servicio"].ToString()):0.00;
                        ImporteSoles = rowDocumento["Cliente"].ToString() == "1" ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round((MontoDebe + MontoHaber) * Convert.ToDouble(rowDocumento["TipoCambio"]), 2);
                        ImporteDolares = rowDocumento["Cliente"].ToString() == "2" ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round(MontoDebe + MontoHaber, 2);
                        AddDetailTable(Referencia, PlanCuentaID, AsientoContableID, AsientoContableDetalleID, MontoDebe, MontoHaber, ImporteSoles, ImporteDolares, TablaMovimientoID, tablaDetail, rowDocumento);

                    }

                    if (Convert.ToDouble(rowDocumento["Propina"].ToString()) != 0.00)   
                    {
                        PlanCuentaID = rowDocumento["CuentaPropina"].ToString();
                        Referencia = rowDocumento["Cliente"].ToString();
                        MontoHaber = Convert.ToDouble(rowDocumento["Total"].ToString()) > 0 ? 0.00 : Convert.ToDouble(rowDocumento["Propina"].ToString());
                        MontoDebe = Convert.ToDouble(rowDocumento["Total"].ToString()) > 0 ? Convert.ToDouble(rowDocumento["Propina"].ToString()) : 0.00;
                        ImporteSoles = rowDocumento["Cliente"].ToString() == "1" ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round((MontoDebe + MontoHaber) * Convert.ToDouble(rowDocumento["TipoCambio"]), 2);
                        ImporteDolares = rowDocumento["Cliente"].ToString() == "2" ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round(MontoDebe + MontoHaber, 2);
                        AddDetailTable(Referencia, PlanCuentaID, AsientoContableID, AsientoContableDetalleID, MontoDebe, MontoHaber, ImporteSoles, ImporteDolares, TablaMovimientoID, tablaDetail, rowDocumento);
                    }

                    
                    if (Convert.ToInt32(rowDocumento["Prepago"].ToString()) == 1 && Convert.ToInt32(rowDocumento["PrepagosNoConsiderarVentas"].ToString()) == 1)
                    {
                        Int32 MonedaId = Convert.ToInt32(rowDocumento["MonedaId"].ToString());

                        if (Convert.ToInt32(rowDocumento["Vinculada"].ToString()) == 1 
                            && (DocumentoId.Substring(0,2) =="01" || DocumentoId.Substring(0, 2) == "08") 
                            && Convert.ToInt32(rowDocumento["Turista"].ToString()) == 0)
                        {
                            if ( MonedaId == 1)
                                PlanCuentaID = Convert.ToString(rowDocumento["PlanCuentaIDVinculadaAnticipoSoles"]);
                            else
                                PlanCuentaID = Convert.ToString(rowDocumento["PlanCuentaIDVinculadaAnticipoDolares"]);
                        }
                        else
                        {
                            if ( MonedaId == 1)
                            {
                                PlanCuentaID = Convert.ToString(rowDocumento["FacturaCuentaSolesPrepago"]);
                            }
                            else
                            {
                                PlanCuentaID = Convert.ToString(rowDocumento["FacturaCuentaDolaresPrepago"]);
                            }
                            
                            Referencia = "ANTICIPO";
                            //Console.WriteLine("ANTICIPO");
                            ImporteDolares = MonedaId == 1 ? Math.Round(MontoDebe + MontoHaber,2) : Math.Round((MontoDebe + MontoHaber) * Convert.ToDouble(rowDocumento["TipoCambio"].ToString()), 2);
                            ImporteSoles = MonedaId == 2 ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round((MontoDebe + MontoHaber) * Convert.ToDouble(rowDocumento["TipoCambio"].ToString()), 2);
                            if (MontoDebe > 0 )
                            {
                                ; // Poner cuadre completar
                            } else
                            {
                                
                            }

                            Double Diferencia = Math.Round(TotalDebeCuadre - TotalHaberCuadre, 2);
                            Double DiferenciaSoles = Math.Round(TotalDebeSolesCuadre - TotalHaberSolesCuadre, 2);
                            Double DiferenciaDolares = Math.Round(TotalDebeDolaresCuadre - TotalHaberDolaresCuadre, 2);
                            if (Math.Abs(Diferencia) < 0.05 || Math.Abs(DiferenciaSoles) < 0.05 || Math.Abs(DiferenciaDolares) < 0.05)
                            {

                            }
                            
                            //PlanCuentaID = Pla
                            // Añadir Aquí registro
                           // Console.WriteLine("Adding Record");
                            AddDetailTable(Referencia, PlanCuentaID, AsientoContableID, AsientoContableDetalleID, MontoDebe, MontoHaber, ImporteSoles, ImporteDolares, TablaMovimientoID, tablaDetail, rowDocumento);
                        }
                    }
                    else
                    {
                        //Console.WriteLine(" DETALLE DE EVENTOS");
                        var query = tablaDetalleEventos.AsEnumerable().Where(d => d.Field<String>("DocumentoId") == DocumentoId);
                        foreach(var row in query)
                        {
                            Int32 MonedaID = Convert.ToInt32(row["MonedaIDPaidOut"].ToString());
                            if (Convert.ToDouble(row["TotalPaidOut"].ToString()) != 0.00)
                            {
                                //Console.WriteLine(row["DocumentoId"].ToString());
                                
                                PlanCuentaID = row["PlanCuentaID"].ToString();
                                Referencia = rowDocumento["Cliente"].ToString();
                                MontoHaber = Convert.ToDouble(rowDocumento["TotalPaidOut"].ToString()) > 0 ? 0.00 : Convert.ToDouble(rowDocumento["TotalPaidOut"].ToString());
                                MontoDebe = Convert.ToDouble(rowDocumento["TotalPaidOut"].ToString()) > 0 ? 0.00 :Math.Round(Convert.ToDouble(row["TotalPaidOut"].ToString()),2);
                                ImporteSoles = MonedaID == 1 ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round((MontoDebe + MontoHaber) * Convert.ToDouble(rowDocumento["TipoCambio"]), 2);
                                ImporteDolares = MonedaID == 2 ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round(MontoDebe + MontoHaber, 2);
                                AddDetailTable(Referencia, PlanCuentaID, AsientoContableID, AsientoContableDetalleID, MontoDebe, MontoHaber, ImporteSoles, ImporteDolares, TablaMovimientoID, tablaDetail, rowDocumento);
                            }


                            MontoDebe = Convert.ToDouble(row["TotalTipoProducto"].ToString()) > 0 ? 0.00 : Math.Round(Convert.ToDouble(row["TotalTipoProducto"].ToString()), 2);
                            MontoHaber = Convert.ToDouble(row["TotalTipoProducto"].ToString()) < 0 ? 0.00 : Math.Round(Convert.ToDouble(row["TotalTipoProducto"].ToString()),2);
                            
                            
                            Referencia = row["nombre"].ToString();
                            ImporteSoles = MonedaID == 1 ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round((MontoDebe + MontoHaber) * Convert.ToDouble(rowDocumento["TipoCambio"]), 2);
                            ImporteDolares = MonedaID == 2 ? Math.Round(MontoDebe + MontoHaber, 2) : Math.Round((MontoDebe + MontoHaber)/ Convert.ToDouble(rowDocumento["TipoCambio"]), 2);

                            Double Diferencia = Math.Round(TotalDebeCuadre - TotalHaberCuadre, 2);
                            Double DiferenciaSoles = Math.Round(TotalDebeSolesCuadre - TotalHaberSolesCuadre, 2);
                            Double DiferenciaDolares = Math.Round(TotalDebeDolaresCuadre - TotalHaberDolaresCuadre, 2);
                            if (Math.Abs(Diferencia) < 0.05 || Math.Abs(DiferenciaSoles) < 0.05 || Math.Abs(DiferenciaDolares) < 0.05)
                            {
                                if (MontoHaber > 0.00)
                                {

                                    MontoHaber += Diferencia;
                                    ImporteSoles += DiferenciaSoles;
                                    ImporteDolares += DiferenciaDolares;
                                }

                                if (MontoDebe >0.00)
                                {
                                    if (Diferencia > 0)
                                        Diferencia *= -1;
                                    else
                                        Diferencia = Math.Abs(Diferencia);

                                    if (DiferenciaSoles > 0)
                                        DiferenciaSoles *= -1;
                                    else
                                        DiferenciaSoles = Math.Abs(DiferenciaSoles);
                                    if (DiferenciaDolares > 0.00)
                                        DiferenciaDolares *= -1;
                                    else
                                        DiferenciaDolares = Math.Abs(DiferenciaDolares);
                                    MontoDebe += Diferencia;
                                    ImporteSoles += DiferenciaSoles;
                                    ImporteDolares += DiferenciaDolares;
                                }
                            }
                            PlanCuentaID = row["PlanCuentaID"].ToString();
                            Referencia = row["nombre"].ToString();
                            AddDetailTable(Referencia, PlanCuentaID, AsientoContableID, AsientoContableDetalleID, MontoDebe, MontoHaber, ImporteSoles, ImporteDolares, TablaMovimientoID, tablaDetail, rowDocumento);
                        }
                    }
                }
            }
            
            bulkCopy(connectionString, NombreTablaCabecera, tablaHeader);
            bulkCopy(connectionString, NombreTablaDetalle, tablaDetail);
            

            watch.Stop();
            Console.WriteLine("Total Time:" + watch.ElapsedMilliseconds / 1000);

            Console.WriteLine("End   : " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"));
            Console.ReadKey();

        }
        
        static void bulkCopy(String connectionString, String targetTable,  DataTable table)
        {
            Console.WriteLine("Aplicando BulCopy a " + table.TableName);

            using (var bulkCopy = new SqlBulkCopy(connectionString))
            {
                bulkCopy.DestinationTableName = targetTable;

                bulkCopy.BulkCopyTimeout = 0;

                bulkCopy.WriteToServer(table);
            }
        }


        static void AddHeaderTable(int AsientoContableID, String  CodigoComprobante, String TablaMovimientoID,  DataTable tablaHeader, DataRow rowDocumento)
        {

            DataRow row = tablaHeader.NewRow();
            row["AsientoContableID"] = AsientoContableID;
            row["SubDiarioID"] = rowDocumento["SubDiarioID"];
            row["HotelID"] = rowDocumento["hotelId"];
            row["Glosa"] = rowDocumento["Glosa"];
            row["Comprobante"] = CodigoComprobante; ;
            row["Dia"] = rowDocumento["FechaEmision"];
            row["Ejercicio"] = rowDocumento["Periodo"];
            row["Bloqueado"] = "2";
            row["ReferenciaSHIOL"] = rowDocumento["TransaccionDocumentoId"];
            row["FechaRegistro"] = rowDocumento["FechaEmision"];
            row["UsuarioId"] = "0";
            row["TablaMovimientoID"] = TablaMovimientoID;
            tablaHeader.Rows.Add(row);
        }

        static void AddDetailTable(String referencia, String PlanCuentaID, int AsientoContableID, int AsientoContableDetalleID,Double MontoDebe, Double MontoHaber, Double ImporteSoles, Double ImporteDolares, String TablaMovimientoID,  DataTable tablaDetail, DataRow rowDocumento)
        {
            
            Int32 PeriodoID = DateTime.Parse(rowDocumento["FechaEmision"].ToString()).Month;
            //String TablaMovimientoID = "137";
            String TipoConversionMonedaID = "113";
            String PlanCuentaIDCaja = "0";

            String Doc = rowDocumento["DocumentoId"].ToString();


         //   String NumeroDocumentoID = rowDocumento["DocumentoId"].ToString().Substring(4, 8);
         //   String SerieDocumentoID =  rowDocumento["DocumentoId"].ToString().Substring(2, 3);


            /* Viene del Principal
                   UCO_AsientosContablesDetalle_CreaoModifica
                                           @EstablecimientoID,@Periodo,@AsientoContableDetalleID,@AsientoContableId,@MontoDebe,
                                           @MontoHaber,@CanalContableID,@EmpresaID,@TipoDocumentoID,@ProyectoID,@FechaEmision,
                                           @Referencia,@FechaEmision,@FechaVencimiento,0,@NumeroDocumentoID,@TransaccionDocumentoID,
                                           @FlujoCajaID,@PlanCuentaID,@ImporteSoles,@ImporteDolares,@AsientoContableDetalleIDTranferencia, 
                                           @PeriodoID,@TablaMovimientoID,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,
                                           NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL, 
                                           @TipoConversionMonedaID, @MonedaID, @TipoCambio,0,0,0,0,'0',0,'',@SerieDocumentoID, @HotelID,
                                           @IdentidadID,@NombreEmpresa,@Ruc

                   UCO_AsientoCreaDetalle @EstablecimientoID, @Periodo, @AsientoContableID, @MontoDebeUN,     
                                           @MontoHaberUN, @CanalContableID, @EmpresaID, @TipoDocumentoID,     
                                           @ProyectoID,@Dia, @Referencia, @FechaEmision, @FechaVencimiento, @AreaID,     
                                           @NumeroDocumento,@TransaccionDocumentoID, @FlujoCajaID, @PlanCuentaActual,          
                                           @ImporteSoles_1, @ImporteDolares_1,@AsientoContableDetalleIDTransferencia, @PeriodoID,@TablaMovimientoIDAsientos, @AsientoContableDetalleID_Temp OUTPUT,     
                                           @UnidadNegocioID_1,@MonedaID,@TipoCambio,@TipoConversionMonedaID,@Discrepancia,
                                           --saul - 28082013
                                           @SegmentoID,
                                           @PaisId,
                                           @PaqueteTOID,
                                           @PeriodoGPA , @PlanCuentaIDCaja, @MedioPagoID, @NroOperacion,@SerieDocumento, @HotelID,
                                           @IdentidadID,@NombreEmpresa,@NumeroRuc


                Viene del Detalle en-si

             * 
             */
            DataRow row = tablaDetail.NewRow();
            row["AsientoContableDetalleID"] = AsientoContableDetalleID;
            row["AsientoContableID"] = AsientoContableID;
            row["MontoDebe"] = MontoDebe;
            row["MontoHaber"] = MontoHaber;
            row["CanalContableID"] = rowDocumento["CanalContableID"];
            row["EmpresaID"] = rowDocumento["EmpresaID"];
            row["TipoDocumentoID"] = rowDocumento["TipoDocumentoID"];
            row["ProyectoID"] = rowDocumento["ProyectoID"];
            row["Dia"] = rowDocumento["FechaEmision"]; ;
            row["Referencia"] = referencia;
            row["FechaEmision"] = rowDocumento["FechaEmision"];
            row["FechaVencimiento"] = rowDocumento["FechaEmision"]; ;
            row["AreaID"] = "0";
            row["NumeroDocumento"] = rowDocumento["NumeroDocumentoID"];
            row["TransaccionDocumentoID"] = rowDocumento["TransaccionDocumentoID"];
            row["FlujoCajaID"] = 0;                             //variable creada pero no asignada.
            row["PlanCuentaID"] = getFieldStringValue(PlanCuentaID,20); //getFieldStringValue(rowDocumento["PlanCuentaID"].ToString(), 20);
            row["ImporteSoles"] = ImporteSoles;
            row["ImporteDolares"] = ImporteDolares;
            row["AsientoContableDetalleIDTransferencia"] = "0"; //Asignado pero no usado
            row["PeriodoID"] = PeriodoID;
            row["TablaMovimientoID"] = TablaMovimientoID;
            row["UnidadNegocioID"] = 0; // DBNull.Value;
            row["MonedaId"] = rowDocumento["MonedaId"];
            row["TipoCambio"] = rowDocumento["TipoCambio"];
            row["TipoConversionMonedaID"] = TipoConversionMonedaID;
            row["Discrepancia"] = 0;
            row["SegmentoID"] = 0;       //Creada pero no asignada
            row["PaisID"] = 0;           //Creada pero no asignada
            row["PaqueteTOID"] = "";    //Creada pero no asignada
            row["PeriodoGPA"] = "";    //Creada pero no asignada
            row["PlanCuentaIDCaja"] = PlanCuentaIDCaja;
            row["MedioPagoID"] = 0;      //Creada pero no asignada
            row["NroOperacion"] = ""; //No Usada
            row["Serie"] = rowDocumento["SerieDocumentoID"]; ;
            row["HotelID"] = rowDocumento["hotelId"];
            row["IdentidadId"] = rowDocumento["IdentidadId"];
            row["NombreEmpresa"] = getFieldStringValue(rowDocumento["NombreEmpresa"].ToString(), 150);
            row["NumeroRuc"] = getFieldStringValue(rowDocumento["ruc"].ToString(), 20);

            tablaDetail.Rows.Add(row);

        }

        static String getFieldStringValue(String value, int maxLength)
        {
            if (maxLength < value.Length)
                return value.Substring(0, maxLength);
            else
                return value;
        }

        public DataSet GetDataSet(String storeProcedure, string paramValue)
        {
            SqlCommand sqlcomm = new SqlCommand();
            SqlConnection  sqlConn = new SqlConnection() ;
            sqlcomm.Connection = sqlConn;
            using (sqlConn)
            {
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        // This will be your input parameter and its value
                        sqlcomm.Parameters.AddWithValue("@ParameterName", paramValue);

                        // You can retrieve values of `output` variables
                        var returnParam = new SqlParameter
                        {
                            ParameterName = "@Error",
                            Direction = ParameterDirection.Output,
                            Size = 1000
                        };
                        sqlcomm.Parameters.Add(returnParam);
                        // Name of stored procedure
                        sqlcomm.CommandText = "StoredProcedureName";
                        da.SelectCommand = sqlcomm;
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        DataSet ds = new DataSet();
                        da.Fill(ds);
                    }
                }
                /*
                catch (SQLException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                }
                */
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            return new DataSet();
        }
        public static DataTable GetDataTableSchemaFromTable(string tableName, SqlConnection sqlConn, SqlTransaction transaction)
        {
            DataTable dtResult = new DataTable();

            using (SqlCommand command = sqlConn.CreateCommand())
            {
                command.CommandText = String.Format("SELECT TOP 1 * FROM {0}", tableName);
                command.CommandType = CommandType.Text;
                if (transaction != null)
                {
                    command.Transaction = transaction;
                }

                SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly);

                dtResult.Load(reader);

            }

            return dtResult;
        }


    }
}
