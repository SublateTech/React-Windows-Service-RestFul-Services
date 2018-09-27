using SimpleHttp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using SqlMagic.Monitor.Sql;
using SqlMagic.Monitor.Items.Results;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using OfficeOpenXml;
using System.Globalization;
using System.Net;
using System.Web;
using OfficeOpenXml.Table;
using System.Reflection;
using System.Linq;

namespace ShiolWinSvc
{
    class Plantillas
    {

        public string createPlantilla(string json)
        {
            createFilePlantilla plantilla = new createFilePlantilla();
            try
            {
                plantilla = JsonConvert.DeserializeObject<createFilePlantilla>(json);

            }catch (Exception ex)
            {
                Console.WriteLine("JsonConvert" + ex.Message);
            }

            try
            { 
                // Declare once, keep using
                var nTimeout = 30; // timeout
                var bLog = true;

                var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
                // Create with a timeout of 30 seconds and logging as true
                var oSql = new Sql(connectionString, nTimeout, bLog);


                SqlResultWithDataSet oResult = oSql.Open("Select * from correlativos c Where nombretabla = 'PlantillasProcesadas'",
                    CommandType.Text
                );

                var plantillaProcesadaID = oResult.Results.Tables[0].Rows.Count == 1 ? oResult.Results.Tables[0].Rows[0]["numerogenerado"].ToString().Trim() : "0";

              //  string sql = "INSERT INTO PlantillasProcesadas (plantillaProcesadaID, tipoPlantillaID, rutaArchivo, fechaCarga, usuarioID, estado) VALUES("+ (Convert.ToInt32(plantillaProcesadaID) + 1) + ", '"+ plantilla.tipo + "', '"+ plantilla.tipo + "', @fecha, 1, '0'); "

                oResult = oSql.Open("INSERT INTO PlantillasProcesadas (plantillaProcesadaID, tipoPlantillaID, rutaArchivo, fechaCarga, usuarioID, estado) VALUES(@id, @tipo, @ruta, GETDATE(), 1, '0'); ",
                    CommandType.Text,
                    new SqlParameter("id", Convert.ToInt32(plantillaProcesadaID) + 1),
                    new SqlParameter("tipo", plantilla.tipo),
                    new SqlParameter("ruta", plantilla.files[0].title)
                );

                oResult = oSql.Open("Update correlativos Set  numerogenerado = @corr Where  correlativoid = @id",
                    CommandType.Text,
                    new SqlParameter("id", 3103),
                    new SqlParameter("corr", Convert.ToInt32(plantillaProcesadaID) + 1)
                );

                plantilla.id = plantillaProcesadaID;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return JsonConvert.SerializeObject(plantilla);

            /*SELECT TOP 1000 [plantillaProcesadaID]
      ,[fechaCarga]
      ,[horaCarga]
      ,[tipoPlantillaID]
      ,[usuarioID]
      ,[rutaArchivo]
      ,[fechaProceso]
      ,[horaProceso]
      ,[usuarioIDProcesa]
      ,[estado]
        FROM[SHIOL_DATOS].[dbo].[PlantillasProcesadas]
        */

        }
        

        public bool deletePlantillaExcel(string id)
        {
            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);

            SqlResultWithDataSet oResult = oSql.Open("Delete from PlantillasProcesadas Where plantillaProcesadaID = @id",
                CommandType.Text,
                new SqlParameter("id", id)
            );

            return true;
        }


        
        public void createRecords(string FilePath, List<TableField> fields, string tableName, Sql oSql)
        {

            DateTimeFormatInfo DateTsysFormat = CultureInfo.CurrentCulture.DateTimeFormat;



            FileInfo existingFile = new FileInfo(FilePath);

            Console.WriteLine(FilePath);


            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets["RH"];

                //INSERT INTO table_name (column1, column2, column3, ...)
                //VALUES(value1, value2, value3, ...);

                string strCommand = "INSERT INTO " + tableName;

                string strFields = " (";

                int x = 0;
                foreach (var field in fields)
                {
                    strFields += (x > 0 ? "," : "") + field.name;
                    x++;
                }
                strFields += ")";


                strCommand += strFields + " ";

                Boolean header = false;
                for (int row = 1; row < worksheet.Cells.Rows; row++)
                {
                    if (!header)
                    {
                        header = true;
                        continue;
                    }

                    string strValues = "VALUES(";
                    x = 0;
                    string crc = "";
                    for (int col=1; col <= fields.Count; col++)
                    {
                        //Console.WriteLine(fields[cell.ColumnNumber - 1].Name + " -- " + cell.Value.ToString() + " -- " + cell.GetType());

                        string value = worksheet.Cells[row, col].Value == null ? "" : worksheet.Cells[row, col].Value.ToString().Trim();

                        DateTime result;
                        
                        if (IsDate(value, out result))
                        {
                            // do something with result
                            value = result.ToString("yyyyMMdd hh:mm:ss"); // ("yyyy-MM-dd HH:mm:ss.fff");
                        //    Console.WriteLine(value);
                        }


                        strValues += (x > 0 ? "," : "") + "'" + value + "'"; //.Replace("\"","").Replace("'","")
                        x++;
                        crc += value;
                        
                        if (x == fields.Count)
                            break;
                    }
                    strValues += ")";

                    if (crc == "")
                        return;

                    string strCommandExec = strCommand + strValues;

                    try
                    {
                        SqlResultWithDataSet oResult = oSql.Open(strCommandExec, CommandType.Text);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "----" + strCommandExec);
                    }

                }
                

                // Read the rows using the worksheet index
                // Worksheet indexes are start at 1 not 0
                // This method is slightly faster to find the underlying file (so slight you probably wouldn't notice)
                //worksheet = fastExcel.Read(1);
            }

        }
        
        bool IsDate(string strDate,  out DateTime result)
        {
            result = DateTime.Now;
            if (strDate.IndexOf('/') > -1 || strDate.IndexOf('-') > -1)
            {
                //strDate = strDate.Substring(0, strDate.IndexOf(' ') + 1);

                DateTime date = DateTime.Now;

                string culture = ""; // ConfigurationManager.AppSettings["Culture"];

                CultureInfo info = culture == "" ? CultureInfo.CurrentCulture: CultureInfo.CreateSpecificCulture(culture);

                try
                {
                    date = DateTime.Parse(strDate, info);//CultureInfo.CreateSpecificCulture("en-US"));
                    result = date;
                    return true;
                }
                catch (Exception ex)
                {   
                    return false;
                }
            }
            else

                return false;
        }

        public static DateTime FromDateInteger(int mjd)
        {
            long a = mjd + 2468570;
            long b = (long)((4 * a) / 146097);
            a = a - ((long)((146097 * b + 3) / 4));
            long c = (long)((4000 * (a + 1) / 1461001));
            a = a - (long)((1461 * c) / 4) + 31;
            long d = (long)((80 * a) / 2447);
            int Day = (int)(a - (long)((2447 * d) / 80));
            a = (long)(d / 11);
            int Month = (int)(d + 2 - 12 * a);
            int Year = (int)(100 * (b - 49) + c + a);
            return new DateTime(Year, Month, Day);
        }

        public bool processPlantillaExcel(string id)
        {
            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            PlantillaExcel plantilla = getPlantillaField(id);
            if (plantilla == null)
                return false;
            string path = ShiolConfiguration.Instance.Config.DirPlantillas; //ConfigurationManager.AppSettings["dirPlantillas"]; //@"D:\SGH_Sistemas\";

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);

            string file = Path.Combine(path, plantilla.archivo); // @"D:\SGH_Sistemas\ShiolExcelService\ShiolWinSvc\FastExcel\FastExcelDemo\bin\Debug\SHIOL 1-31 FINAL.XLSX";
            if (!File.Exists(file))
            {
                Console.WriteLine("Archivo : " + file + " no existe");
                return false;
            }


            List<TableField> fields = getExcelStructure(file);

            if (fields.Count > 0)
            {
                string TableName = createTable(fields, oSql);

                Console.WriteLine("Generated : " + TableName);

                createRecords(file, fields, TableName, oSql);

                SqlResultWithDataSet oResult = oSql.Open("USP_Plantilla_Excel_CargaCuentasPorPagar_RH @nombreTabla = '" + TableName + "';",
                    CommandType.Text
                );
                /*
                oResult = oSql.Open("IF OBJECT_ID('dbo."+TableName+"', 'U') IS NOT NULL DROP TABLE dbo."+TableName+"; ",
                    CommandType.Text
                );
                */
                oResult = oSql.Open("Update PlantillasProcesadas Set  estado = 2 Where plantillaProcesadaID = @id",
                   CommandType.Text,
                   new SqlParameter("id", id)
               );

                Console.WriteLine("Processed : " + TableName);


                return true;
            }
            else
            {
                Console.WriteLine("No Processed...");
                return false;
            }
        }

        public string  createTable(List<TableField> fields, Sql oSql)
        {

            //var commandStr = "If not exists (select name from sysobjects where name = 'Customer') CREATE TABLE Customer(First_Name char(50),Last_Name char(50),Address char(50),City char(50),Country char(25),Birth_Date datetime)";
            //using (SqlCommand command = new SqlCommand(commandStr, con))
            //    command.ExecuteNonQuery();

            string tableName = "table_" + GenerateName(10);

            var commandStr = "IF NOT EXISTS (select name from sysobjects where name = '" + tableName + "')  CREATE TABLE " + tableName + "(";


            int x = 0;
            foreach (var field in fields)
            {
                commandStr += (x > 0 ? "," : "") + field.ToString();
                x++;
            }
            commandStr += ")";

            //Console.WriteLine(commandStr);

            SqlResultWithDataSet oResult = oSql.Open(commandStr,
                CommandType.Text
            );

            return tableName;
        }

        public static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }


       
        public List<TableField> getExcelStructure(string FilePath)
        {
            List<TableField> fields = new List<TableField>();


            try
            {
                FileInfo existingFile = new FileInfo(FilePath);
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["RH"];
                    //int col = 2; //The item description

                    //var value = worksheet.Cells.Value;

                    int columns = 0;
                    for (int col = 1; col < worksheet.Cells.Columns; col++)
                    {
                        if (worksheet.Cells[1, col].Value != null)
                        {
                            columns++;
                            //Console.WriteLine(worksheet.Cells[1, col].Value);
                            fields.Add(new TableField(worksheet.Cells[1, col].Value.ToString(), 50, "char"));
                        }
                        else
                            break;
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return fields;
        }


        public class TableField
        {
            public TableField(string Name, int Len, string Type)
            {
                this.name = Name;
                this.lenght = Len;
                this.type = Type;
            }
            public string name;
            public string type;
            public int lenght;
            public override string ToString()  {   //First_Name char(50)
                return name + " " + type + "("+lenght+")"; }
        }


        public static string getShiolExcelFiles()
        {
            string json = null;
            List<PlantillaExcel> fields = new List<PlantillaExcel>();
            try
            {
                PlantillaExcel field = new PlantillaExcel("1", "Excel 1");
                field.estado = "Procesado";
                field.tipo = "RH";
                fields.Add(field);
                field = new PlantillaExcel("2", "Excel 2");
                field.estado = "Procesado";
                field.tipo = "CT";
                fields.Add(field);
                field = new PlantillaExcel("3", "Excel 3");
                field.estado = "Procesado";
                field.tipo = "ST";
                fields.Add(field);
                json = JsonConvert.SerializeObject(fields);

            }
            catch
            {

            }

            return json;
        }

        public string getPlantilla1(string id)
        {
            string json = null;
            List<PlantillaExcel> fields = new List<PlantillaExcel>();
            try
            {
                PlantillaExcel field = new PlantillaExcel("1", "Excel 1");
                field.estado = "Procesado1";
                field.tipo = "1";
                fields.Add(field);
                field = new PlantillaExcel("2", "Excel 2");
                field.estado = "Procesado2";
                field.tipo = "2";
                fields.Add(field);
                field = new PlantillaExcel("3", "Excel 3");
                field.estado = "Procesado3";
                field.descripcion = "aklklaka";
                field.id = "777";
                field.tipo = "2";
                fields.Add(field);
                json = JsonConvert.SerializeObject(field);

            }
            catch
            {

            }

            return json;
        }
        public string getPlantilla(string id)
        {
            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);

            var strSql = "SELECT tp.nombre, p.* FROM PlantillasProcesadas p Left Join tipoPlantillas tp On tp.tipoPlantillaID = p.tipoPlantillaID Where plantillaProcesadaID='" + id + "'";
            SqlResultWithDataSet oResult = oSql.Open(strSql, CommandType.Text);

            string json = null;
            List<PlantillaExcel> fields = new List<PlantillaExcel>();
            try
            {
                if (oResult.Results.Tables[0].Rows.Count > 0)
                {
                    DataRow row = oResult.Results.Tables[0].Rows[0];

                    PlantillaExcel field = new PlantillaExcel(row["plantillaProcesadaID"].ToString().Trim(), row["usuarioID"].ToString());
                    field.estado = row["estado"].ToString() == "2" ? "Procesado" : (row["estado"].ToString() == "1" ? "Procesando" : "No Procesado");
                    field.tipo_nombre = row["nombre"].ToString();
                    field.tipo = row["tipoPlantillaID"].ToString().Trim();
                    field.archivo = row["rutaArchivo"].ToString();
                    field.fecha_creacion = row["fechaCarga"].ToString();
                    field.fecha_proceso = row["fechaProceso"].ToString();
                    //field.descripcion = "xxxx";
                    fields.Add(field);

                    json = JsonConvert.SerializeObject(field);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return json;

        }

        public PlantillaExcel getPlantillaField(string id)
        {
            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);

            var strSql = "SELECT tp.nombre, p.* FROM PlantillasProcesadas p Left Join tipoPlantillas tp On tp.tipoPlantillaID = p.tipoPlantillaID Where plantillaProcesadaID='" + id + "'";
            SqlResultWithDataSet oResult = oSql.Open(strSql, CommandType.Text);
            
            PlantillaExcel field = null;
            
            try
            {
                if (oResult.Results.Tables[0].Rows.Count > 0)
                {
                    DataRow row = oResult.Results.Tables[0].Rows[0];

                    field = new PlantillaExcel(row["plantillaProcesadaID"].ToString().Trim(), row["usuarioID"].ToString());
                    field.estado = row["estado"].ToString() == "2" ? "Procesado" : (row["estado"].ToString() == "1" ? "Procesando" : "No Procesado");
                    field.tipo_nombre = row["nombre"].ToString();
                    field.tipo = row["tipoPlantillaID"].ToString();
                    field.archivo = row["rutaArchivo"].ToString();
                    field.fecha_creacion = row["fechaCarga"].ToString();
                    field.fecha_proceso = row["fechaProceso"].ToString();
                    field.descripcion = "xxxx";
                    
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return field;

        }

        public string getListTipoPlantillas()
        {
            string json = null;
            List<PlantillaExcel> fields = new List<PlantillaExcel>();
            try
            {
                PlantillaExcel field = new PlantillaExcel("1", "Excel 1");
                field.estado = "Procesado1";
                field.tipo = "1";
                fields.Add(field);
                field = new PlantillaExcel("2", "Excel 2");
                field.estado = "Procesado2";
                field.tipo = "2";
                fields.Add(field);
                field = new PlantillaExcel("3", "Excel 3");
                field.estado = "Procesado3";
                field.tipo = "2";
                fields.Add(field);
                json = JsonConvert.SerializeObject(fields);

            }
            catch
            {

            }

            return json;
        }

        public string getListTipoPlantilla()
        {
            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);


            var strSql = "SELECT * FROM tipoPlantillas";
            SqlResultWithDataSet oResult = oSql.Open(strSql, CommandType.Text);

            string json = null;
            List<PlantillaExcel> fields = new List<PlantillaExcel>();
            try
            {
                foreach (DataRow row in oResult.Results.Tables[0].Rows)
                {
                    PlantillaExcel field = new PlantillaExcel(row["tipoPlantillaID"].ToString().Trim(), "");
                    field.tipo_nombre = row["nombre"].ToString();
                    field.tipo = row["tipoPlantillaID"].ToString().Trim();
                    fields.Add(field);
                }
                json = JsonConvert.SerializeObject(fields);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return json;

        }

        public string getListExcelFiles(Request req, Response res)
        {
            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;
            
            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);




            /*SELECT TOP 1000 [plantillaProcesadaID]
        ,[fechaCarga]
        ,[horaCarga]
        ,[tipoPlantillaID]
        ,[usuarioID]
        ,[rutaArchivo]
        ,[fechaProceso]
        ,[horaProceso]
        ,[usuarioIDProcesa]
        ,[estado]
          FROM[SHIOL_DATOS].[dbo].[PlantillasProcesadas]
          */


            int Estado = req.PathVariables["estado"] != null ? Convert.ToInt32(req.PathVariables["estado"]) : -1;
            int Tipo = req.PathVariables["tipo"] != null ? Convert.ToInt32(req.PathVariables["tipo"]) : -1;
            string FechaCreacion  = req.PathVariables["fecha_creacion"] != null ? req.PathVariables["fecha_creacion"].Substring(0,10) : "";

            string strWhere = "";

            if (Tipo  > -1)
            {
                strWhere = "Where " + "p.tipoPlantillaID" + "=" + Tipo;
            }
            if (Estado > -1)
            {
                strWhere += strWhere == "" ? "Where p.Estado ='" + Estado + "' " : " And  p.Estado ='" + Estado + "' ";
            }

            if (FechaCreacion !="")
            {
                strWhere += strWhere == "" ? "Where FORMAT(fechaCarga, 'yyyy-MM-dd') ='" + FechaCreacion + "' " : " And  FORMAT(fechaCarga, 'yyyy-MM-dd') ='" + FechaCreacion + "' ";
            }

            //OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY;

            string strCount = "0";

            SqlResultWithDataSet oResult = null;
            try
            {
                var strSql = "SELECT count(*) as _total FROM PlantillasProcesadas p Left Join tipoPlantillas tp On tp.tipoPlantillaID = p.tipoPlantillaID  " + strWhere;
                oResult = oSql.Open(strSql, CommandType.Text);
                strCount = oResult.Results.Tables[0].Rows[0]["_total"].ToString();

                strSql = "SELECT tp.nombre, p.* FROM PlantillasProcesadas p Left Join tipoPlantillas tp On tp.tipoPlantillaID = p.tipoPlantillaID  " + strWhere + " ORDER by p.fechaCarga DESC OFFSET "+ req.PathVariables["_start"] + " ROWS FETCH NEXT " + (Convert.ToInt32(req.PathVariables["_end"])- Convert.ToInt32(req.PathVariables["_start"])) + " ROWS ONLY;";
                    oResult = oSql.Open(strSql, CommandType.Text);

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            

            string json = null;
            List<PlantillaExcel> fields = new List<PlantillaExcel>();
            try
            {

                foreach (DataRow row in oResult.Results.Tables[0].Rows)
                {
                    PlantillaExcel field = new PlantillaExcel(row["plantillaProcesadaID"].ToString().Trim(), row["usuarioID"].ToString());
                    field.estado = row["estado"].ToString() == "2" ? "Procesado" : (row["estado"].ToString() == "1" ? "En Proceso" : "No Procesado");
                    field.tipo_nombre = row["nombre"].ToString();
                    field.tipo = row["tipoPlantillaID"].ToString();
                    field.archivo = row["rutaArchivo"].ToString();
                    field.fecha_creacion = row["fechaCarga"].ToString();
                    field.fecha_proceso = row["fechaProceso"].ToString();
                    fields.Add(field);


                }
                
                json = JsonConvert.SerializeObject(fields);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            res.httpListenerResponse.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
            res.httpListenerResponse.Headers.Add("X-Total-Count", strCount);

            return json;

        }


        void WriteFile(Response res, string path)
        {
            path = @"D:\SGH_Sistemas\ShiolRESTService\SHIOLRH02.xlsx";
            var response = res.httpListenerResponse;
            using (FileStream fs = File.OpenRead(path))
            {
                string filename = Path.GetFileName(path);

                
                //response is HttpListenerContext.Response...
                response.ContentLength64 = fs.Length;
                response.SendChunked = false;
                response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                response.AddHeader("Content-disposition", "attachment; filename=" + filename);

                byte[] buffer = new byte[64 * 1024];
                int read;
                using (BinaryWriter bw = new BinaryWriter(response.OutputStream))
                {
                    while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        bw.Write(buffer, 0, read);
                        bw.Flush(); //seems to have no effect
                    }

                    bw.Close();
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "OK";
                response.OutputStream.Close();
            }
        }

        



        public bool getExcelExportedFile(Request req, Response res)
        {
            WriteFile(res, "");
            return false;
            string _rootDirectory = @"D:\SGH_Sistemas\ShiolRESTService";
            string filename = req.httpRequest.Url.AbsolutePath;

            if (req.Parameters["id"] != null)
                filename = req.Parameters["id"].Substring(0);
            else
                filename = filename.Substring(1);
            filename += ".xlsx";

            filename = "SHIOLRH02.xlsx";
            string strFile = "SHIOLRH02.xlsx";

            filename = Path.Combine(_rootDirectory, filename);

            if (File.Exists(filename))
            {
                try
                {
                    Stream input = new FileStream(filename, FileMode.Open);

                    //Adding permanent http response headers
                    string mime;
                    res.httpListenerResponse.ContentType = WebServerServiceProvider._mimeTypeMappings.TryGetValue(Path.GetExtension(filename), out mime) ? mime : "application/octet-stream";
                    res.httpListenerResponse.ContentLength64 = input.Length;
                    res.httpListenerResponse.AddHeader("Content-Disposition", "attachment; filename=" + strFile);

                    byte[] buffer = new byte[1024 * 16];
                    int nbytes;
                    while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                        res.httpListenerResponse.OutputStream.Write(buffer, 0, nbytes);
                    input.Close();

                    res.httpListenerResponse.StatusCode = (int)HttpStatusCode.OK;
                    res.httpListenerResponse.OutputStream.Flush();
                }
                catch (Exception ex)
                {
                    res.httpListenerResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return false;
                }

            }
            else
            {
                res.httpListenerResponse.StatusCode = (int)HttpStatusCode.NotFound;
                return false;
            }

            res.httpListenerResponse.OutputStream.Close();

           // await res.SendAsync();
            return true;
        }

    }

    class createFilePlantilla
    {
        public string id { get; set; }
       // public string descripcion { get; set; }
        public string tipo { get; set; }
        public IList<rawFile> files { get; set; }

    }

    class rawFile
    {
       // public string preview = null;
       // public string src = null;
        public string title = null;
    }

    class Field
    {
        public Field(string Name, int Index)
        {
            this.Name = Name;
            this.Index = Index;
        }

        public string Name = null;
        public int Index = 0;
    }

    class PlantillaExcel
    {
        public PlantillaExcel(string id, string descripcion)
        {
            this.id = id;
            this.descripcion = descripcion;
        }

        public string id = null;
        public string descripcion = null;
        public string estado = null;
        public string tipo = null;
        public string tipo_nombre = null;
        public string archivo = null;
        public string fecha_creacion = null;
        public string fecha_proceso = null;
    }


}
