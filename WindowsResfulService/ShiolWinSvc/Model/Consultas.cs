using Newtonsoft.Json;
using SqlMagic.Monitor.Items.Results;
using SqlMagic.Monitor.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using XMPPEngineer.Client;

namespace ShiolWinSvc.Model
{
    class Consultas
    {
        
        public string getEmpresa(string id)
        {
            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);

            var strSql = "SELECT * From Establecimientos Where EstablecimientoID='" + id + "'";
            SqlResultWithDataSet oResult = oSql.Open(strSql, CommandType.Text);

            string json = null;
            List<Item> fields = new List<Item>();
            try
            {
                if (oResult.Results.Tables[0].Rows.Count > 0)
                {
                    DataRow row = oResult.Results.Tables[0].Rows[0];

                    Item field = new Item();
                    field.id = row["EstablecimientoID"].ToString();
                    field.descripcion = row["NombreEstablecimiento"].ToString();
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
        public string getListHoteles()
        {


            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);


            var strSql = "exec UCO_Hoteles_Select @WhereCondition='where EstablecimientoID=112';";
            SqlResultWithDataSet oResult = oSql.Open(strSql, CommandType.Text);

            string json = null;
            List<PlantillaExcel> fields = new List<PlantillaExcel>();
            try
            {
                foreach (DataRow row in oResult.Results.Tables[0].Rows)
                {
                    PlantillaExcel field = new PlantillaExcel(row["HotelID"].ToString(), "");
                    field.descripcion = row["NombreHotel"].ToString();
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
        public string getListEmpresas()
        {


            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);


            var strSql = "Select * from Establecimientos";
            SqlResultWithDataSet oResult = oSql.Open(strSql, CommandType.Text);

            string json = null;
            List<PlantillaExcel> fields = new List<PlantillaExcel>();
            try
            {
                foreach (DataRow row in oResult.Results.Tables[0].Rows)
                {
                    PlantillaExcel field = new PlantillaExcel(row["EstablecimientoID"].ToString(), "");
                    field.descripcion = row["NombreEstablecimiento"].ToString();
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
        public string getListTipoEstadoFinanciero()
        {


            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);


            var strSql = "exec UCO_TipoFormatoDeGGyPP_SelectAll";
            SqlResultWithDataSet oResult = oSql.Open(strSql, CommandType.Text);

            string json = null;
            List<PlantillaExcel> fields = new List<PlantillaExcel>();
            try
            {
                foreach (DataRow row in oResult.Results.Tables[0].Rows)
                {
                    PlantillaExcel field = new PlantillaExcel(row["TipoFormatoGGPPID"].ToString().Trim(), "");
                    field.descripcion = row["NombreTipoFormato"].ToString();
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

        public bool authenticateUser(string user, string password)
        {
            try
            {
                // Declare once, keep using
                var nTimeout = 30; // timeout
                var bLog = true;

                var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
                // Create with a timeout of 30 seconds and logging as true
                var oSql = new Sql(connectionString, nTimeout, bLog);

                SqlResultWithDataSet oResult = oSql.Open("SELECT * FROM Usuarios WHERE NombreUsuario = @user And Clave = @password",
                    CommandType.Text,
                    new SqlParameter("user", user),
                    new SqlParameter("password", password)
                );
                return oResult.Results.Tables[0].Rows.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public class User
        {
            public string username;
            public string password;
        }



        class Item
        {
            public string id;
            public string descripcion;
        }
        /*
        public string sendJsonSqlMessage(XmppClient client, string message)
        {

            return "";
        }
        */

        
        public string getJsonSqlMessage(string sqlMessage)
        {
            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);


            var strSql = sqlMessage;
            SqlResultWithDataSet oResult = oSql.Open(strSql, CommandType.Text);

            string json = null;
            
            try
            {
                json = JsonConvert.SerializeObject(oResult.Results.Tables[0]);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return json;

        }
    

        public string getShiolExcelFiles1()
        {
            string json = null;
            List<Excel> fields = new List<Excel>();
            try
            {
                Excel field = new Excel("1", "Excel 1");
                field.estado = "Procesado";
                fields.Add(field);
                field = new Excel("2", "Excel 2");
                fields.Add(field);
                field = new Excel("3", "Excel 3");
                fields.Add(field);
                json = JsonConvert.SerializeObject(fields);

            }
            catch
            {

            }

            return json;
        }

        class Excel
        {
            public Excel(string id, string descripcion)
            {
                this.id = id;
                this.descripcion = descripcion;
            }

            public string id = null;
            public string descripcion = null;
            public string estado = null;
        }


        private void createHeaderFile(string file)
        {
            string path = Path.Combine(ShiolConfiguration.Instance.Config.LogDirectory, file + ".xml");
            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine("<?xml version=\"1.0\"?>" + "\n"
                             + "<!DOCTYPE logfile [ " + "\n"
                            + "<!ENTITY Events" + "\n"
                            + "SYSTEM \"" + file + ".txt" + "\">" + "\n"
                            + " ] >" + "\n"
                            + "<logfile>" + "\n"
                            + "&Events;" + "\n"
                            + "</logfile>");
                tw.Close();
            }
        }
        private void deleteHeaderFile(string file)
        {
            string path = Path.Combine(ShiolConfiguration.Instance.Config.LogDirectory, file + ".xml");
            File.Delete(path);
        }
        private object getShiolEvents(string strDate)
        {
            List<DataFrameStructure> objs = new List<DataFrameStructure>();
            XmlValidatingReader vr = null;
            try
            {
                DateTime date = DateTime.ParseExact(strDate.Replace("-", ""), "yyyyMMdd", CultureInfo.InvariantCulture);

                //DateTime date = DateTime.Today; // DateTime.ParseExact("20180130", "yyyyMMdd", CultureInfo.InvariantCulture);
                //string fullPathToFile = Path.Combine(dir, fileName);
                string file = "-" + date.ToString("yyyy-MM-dd");

                createHeaderFile("Tramas" + file);

                vr = new XmlValidatingReader(new XmlTextReader(Path.Combine(ShiolConfiguration.Instance.Config.LogDirectory, "Tramas" + file + ".xml")));
                vr.ValidationType = ValidationType.None;
                vr.EntityHandling = EntityHandling.ExpandEntities;

                XmlDocument doc = new XmlDocument();
                doc.Load(vr);

                foreach (XmlElement element in doc.SelectNodes("//Event"))
                {
                    var Processed = element.LastChild;
                    var Received = element.FirstChild;

                    //DataFrameStructure uFrameProvider = XmlConvert.DeserializeObject<DataFrameStructure>(Processed.InnerXml);
                    DataFrameStructure obj = new DataFrameStructure()
                    {
                        Date = Processed["Date"].InnerText,
                        Time = Processed["Time"].InnerText,
                        UserID = Processed["UserID"].InnerText,
                        DialedNumber = Processed["DialedNumber"].InnerText,
                        Duration = Processed["Duration"].InnerText,
                        Anexo = Processed["Anexo"].InnerText,
                        Shiol = Processed["Shiol"] != null ? Processed["Shiol"].InnerText : ""

                    };
                    objs.Add(obj);

                }
                vr.Close();
                deleteHeaderFile("Tramas" + file);
            }
            catch
            {
                if (vr != null)
                    vr.Close();
            }

            return objs;
        }
        public object getShiolMonthFiles()
        {
            List<string> files = new List<string>();

            DirectoryInfo dir = new DirectoryInfo(ShiolConfiguration.Instance.Config.LogDirectory);
            foreach (FileInfo flInfo in dir.GetFiles("Tramas*.txt"))
            {
                try
                {
                    string strDate = flInfo.Name.Mid(7, 10).Replace("-", "");
                    DateTime date = DateTime.ParseExact(strDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                    if (date.Month == DateTime.Today.Month)
                    {
                        files.Add(date.ToString("yyyy-MM-dd"));
                    }
                }
                catch
                {

                }
            }

            return files;
        }
        public object getShiolExcelFiles()
        {
            List<Field> fields = new List<Field>();
            try
            {

                fields.Add(new Field("1", 1));

            }
            catch
            {

            }

            return fields;
        }

        class Field
        {
            public Field(string Name, int Index)
            {
                this.id = Name;
                this.Index = Index;
            }

            public string id = null;
            public int Index = 0;
        }
    }
}
