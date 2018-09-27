using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;


using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using Newtonsoft.Json;
using SimpleHttp;
using System.Configuration;
using SqlMagic.Monitor.Sql;
using SqlMagic.Monitor.Items.Results;
using System.Data;
using System.Data.SqlClient;
using ShiolWinSvc.Model;

namespace ShiolWinSvc
{
    class WebServerServiceProvider : AbstractServiceProvider
    {
        private App app;

        public override void StartService()
        {
            app = new App();
            Plantillas plantillas = new Plantillas();
            Contabilidad conta = new Contabilidad();
            Consultas consultas = new Consultas();

            LoggingProvider.Use(LoggingProvider.FileLoggingProvider);

            /*
            app.Get("/", async (req, res) =>
            {
                res.Content = "<p>You did a GET.</p>";
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            */
            app.Post("/", async (req, res) =>
            {
                res.Content = "<p>You did a POST: " + await req.GetBodyAsync() + "</p>";
                res.ContentType = "text/html";
                await res.SendAsync();
            });

            app.Get("/", (req, res) =>
            {
                _rootDirectory = "build";
                Process(req, res);
            });

            app.Get("/[id]", (req, res) =>
            {
                _rootDirectory = "build";
                Process(req, res);
            });

            app.Get("/static/js/[id]", (req, res) =>
            {
                _rootDirectory = "build\\static\\js";
                Process(req, res);
            });

            app.Get("/static/css/[id]", (req, res) =>
            {
                _rootDirectory = "build\\static\\css";
                Process(req, res);
            });

            app.Get("/version", async (req, res) =>
            {
                res.Content = "{ \"version\": \"0.1\" }";
                res.ContentType = "application/json";
                await res.SendAsync();
            });

            app.Get("/hoteles", async (req, res) =>
            {
                res.httpListenerResponse.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
                res.httpListenerResponse.Headers.Add("X-Total-Count", "2");

                res.Content = consultas.getListHoteles(); //getShiolExcelFiles(); //"[{ \"version\": \"0.1\" }]";
                res.ContentType = "application/json";
                res.httpListenerResponse.StatusCode = 200; //401 for error;
                await res.SendAsync();
            });

            app.Get("/empresas", async (req, res) =>
            {
                res.httpListenerResponse.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
                res.httpListenerResponse.Headers.Add("X-Total-Count", "2");

                res.Content = consultas.getListEmpresas(); //getShiolExcelFiles(); //"[{ \"version\": \"0.1\" }]";
                res.ContentType = "application/json";
                res.httpListenerResponse.StatusCode = 200; //401 for error;
                await res.SendAsync();
            });

            app.Get("/empresas/[id]", async (req, res) =>
            {
                string id = req.Parameters["id"];
                if (id == "undefined")
                {
                    res.Content = "{ \"message\": \"The item does not exist\" }";
                    res.httpListenerResponse.StatusCode = 404; //401 for error;
                }
                else
                {

                    res.Content = consultas.getEmpresa(id);
                    res.ContentType = "application/json";
                    if (res.Content == null)
                    {
                        res.Content = "{ \"message\": \"The item does not exist\" }";
                        res.httpListenerResponse.StatusCode = 404; //401 for error;
                    }
                    else
                        res.httpListenerResponse.StatusCode = 200; //401 for error;
                }
                await res.SendAsync();
            });

            app.Get("/TipoEstadoFinanciero", async (req, res) =>
            {
                res.httpListenerResponse.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
                res.httpListenerResponse.Headers.Add("X-Total-Count", "2");

                res.Content = consultas.getListTipoEstadoFinanciero(); //getShiolExcelFiles(); //"[{ \"version\": \"0.1\" }]";
                res.ContentType = "application/json";
                res.httpListenerResponse.StatusCode = 200; //401 for error;
                await res.SendAsync();
            });

            app.Get("/contabilidad", async (req, res) =>
            {
                conta.getContabilidadEstadoResultados(req, res, Contabilidad.FileType.Json);
                await res.SendAsync();
            });

            app.Get("/contabilidad/export/excel/[id]", (req, res) =>
            {
                // plantillas.getExcelExportedFile(req, res);
                conta.getContabilidadEstadoResultados(req, res, Contabilidad.FileType.Xlsx);

            });

            app.Get("/contabilidad/export/pdf/[id]", (req, res) =>
            {
                // plantillas.getExcelExportedFile(req, res);
                conta.getContabilidadEstadoResultados(req, res, Contabilidad.FileType.Pdf);

            });


            #region Plantillas
            app.Get("/plantillas", async (req, res) =>
            {
                res.Content = plantillas.getListExcelFiles(req, res); //getShiolExcelFiles(); //"[{ \"version\": \"0.1\" }]";
                res.ContentType = "application/json";
                res.httpListenerResponse.StatusCode = 200; //401 for error;
                await res.SendAsync();
            });
            app.Get("/tipo_plantillas", async (req, res) =>
            {
                res.httpListenerResponse.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
                res.httpListenerResponse.Headers.Add("X-Total-Count", "2");

                res.Content = plantillas.getListTipoPlantilla(); //getShiolExcelFiles(); //"[{ \"version\": \"0.1\" }]";
                res.ContentType = "application/json";
                res.httpListenerResponse.StatusCode = 200; //401 for error;
                await res.SendAsync();
            });
            app.Get("/plantillas/[id]", async (req, res) =>
            {
                string id = req.Parameters["id"];

                res.Content = plantillas.getPlantilla(id); //"[{ \"plantilla\": \"0.1\", \"id\": \""+username +"\" }]";
                res.ContentType = "application/json";
                if (res.Content == null)
                {
                    res.Content = "{ \"message\": \"The item does not exist\" }";
                    res.httpListenerResponse.StatusCode = 404; //401 for error;
                }
                else
                    res.httpListenerResponse.StatusCode = 200; //401 for error;
                await res.SendAsync();
            });

            app.Put("/plantillas/[id]", async (req, res) =>
            {
                string id = req.Parameters["id"];
                res.Content = "{ \"data\": {\"id\": \"" + id + "\" }}";
                res.ContentType = "application/json";
                res.httpListenerResponse.StatusCode = 200; //401 for error;
                await res.SendAsync();

            });
            app.Put("/process/[id]", async (req, res) =>
            {
                res.Content = "[{ \"data\": {\"id\": \"" + req.Parameters["id"] + "\" }]"; // data: { id: 123, title: "hello, world!" }
                res.ContentType = "application/json";

                if (plantillas.processPlantillaExcel(req.Parameters["id"]))
                    res.httpListenerResponse.StatusCode = 200; //401 for error;
                else
                    res.httpListenerResponse.StatusCode = 401;
                await res.SendAsync();

            });

            app.Delete("/plantillas/[id]", async (req, res) =>
            {
                string id = req.Parameters["id"];
                res.Content = "{ \"data\": {\"id\": \"" + id + "\" }}";
                res.ContentType = "application/json";
                res.httpListenerResponse.StatusCode = 200; //401 for error;

                if (plantillas.deletePlantillaExcel(req.Parameters["id"]))
                    res.httpListenerResponse.StatusCode = 200; //401 for error;
                else
                    res.httpListenerResponse.StatusCode = 401;
                await res.SendAsync();

                //res.httpListenerResponse.Close();

            });
            app.Post("/plantillas", async (req, res) =>
            {
                string id = req.Parameters["id"];

                res.ContentType = "application/json";

                res.httpListenerResponse.StatusCode = 200; //401 for error;
                string json = await req.GetBodyAsync();
                //   Console.WriteLine("POST: " + json);
                json = plantillas.createPlantilla(json);
                res.Content = json;

                await res.SendAsync();


            });
            #endregion 

            app.Post("/authenticate", async (req, res) =>
            {
                res.Content = "{ \"token\": 10001" + "}";
                res.ContentType = "application/json";

                string json = await req.GetBodyAsync();

                Consultas.User user = JsonConvert.DeserializeObject<Consultas.User>(json);

                if (consultas.authenticateUser(user.username.Trim(), user.password.Trim()))
                    res.httpListenerResponse.StatusCode = 200;
                else
                {
                    res.httpListenerResponse.StatusCode = 403;
                    Console.WriteLine("POST: " + json);
                }
                await res.SendAsync();

            });

            app.Start("3500");

            /*
            Task.Factory.StartNew(
            () =>
            {
                while (true)
                {

                    app.Start("3500");
                }
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            */
            return;
         }
        
        /// <SUMMARY>
        /// Stop Service
        /// </SUMMARY>
        public override void StopService()
        {
            
           // if (app != null)
            //    app.Shutdown();
            app = null;
        }

        public static void Main(string[] args)
        {
            WebServerServiceProvider server = new WebServerServiceProvider();
            server.StartService();
            Console.ReadKey();
            server.StopService();
        }
        
        public static IDictionary<string, string> _mimeTypeMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
        #region extension to MIME type list
        {".asf", "video/x-ms-asf"},
        {".asx", "video/x-ms-asf"},
        {".avi", "video/x-msvideo"},
        {".bin", "application/octet-stream"},
        {".cco", "application/x-cocoa"},
        {".crt", "application/x-x509-ca-cert"},
        {".css", "text/css"},
        {".deb", "application/octet-stream"},
        {".der", "application/x-x509-ca-cert"},
        {".dll", "application/octet-stream"},
        {".dmg", "application/octet-stream"},
        {".ear", "application/java-archive"},
        {".eot", "application/octet-stream"},
        {".exe", "application/octet-stream"},
        {".flv", "video/x-flv"},
        {".gif", "image/gif"},
        {".hqx", "application/mac-binhex40"},
        {".htc", "text/x-component"},
        {".htm", "text/html"},
        {".html", "text/html"},
        {".ico", "image/x-icon"},
        {".img", "application/octet-stream"},
        {".iso", "application/octet-stream"},
        {".jar", "application/java-archive"},
        {".jardiff", "application/x-java-archive-diff"},
        {".jng", "image/x-jng"},
        {".jnlp", "application/x-java-jnlp-file"},
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpeg"},
        {".js", "application/x-javascript"},
        {".mml", "text/mathml"},
        {".mng", "video/x-mng"},
        {".mov", "video/quicktime"},
        {".mp3", "audio/mpeg"},
        {".mpeg", "video/mpeg"},
        {".mpg", "video/mpeg"},
        {".msi", "application/octet-stream"},
        {".msm", "application/octet-stream"},
        {".msp", "application/octet-stream"},
        {".pdb", "application/x-pilot"},
        {".pdf", "application/pdf"},
        {".pem", "application/x-x509-ca-cert"},
        {".pl", "application/x-perl"},
        {".pm", "application/x-perl"},
        {".png", "image/png"},
        {".prc", "application/x-pilot"},
        {".ra", "audio/x-realaudio"},
        {".rar", "application/x-rar-compressed"},
        {".rpm", "application/x-redhat-package-manager"},
        {".rss", "text/xml"},
        {".run", "application/x-makeself"},
        {".sea", "application/x-sea"},
        {".shtml", "text/html"},
        {".sit", "application/x-stuffit"},
        {".swf", "application/x-shockwave-flash"},
        {".tcl", "application/x-tcl"},
        {".tk", "application/x-tcl"},
        {".txt", "text/plain"},
        {".war", "application/java-archive"},
        {".wbmp", "image/vnd.wap.wbmp"},
        {".wmv", "video/x-ms-wmv"},
        {".xml", "text/xml"},
        {".xlsx", "application/octet-stream"},
        {".xpi", "application/x-xpinstall"},
        {".zip", "application/zip"},
        #endregion
         };
        private static readonly string[] _indexFiles = {
        "index.html",
        "index.htm",
        "default.html",
        "default.htm"
        };
        private static string _rootDirectory = "";
        private static async void Process(Request req, Response res)
        {
            string exactPath = System.AppDomain.CurrentDomain.BaseDirectory; //Path.GetFullPath(req.httpRequest.Url.AbsolutePath);

            string filename = req.httpRequest.Url.AbsolutePath;

            if (req.Parameters["id"] != null)
                filename = req.Parameters["id"].Substring(0);
            else
                filename = filename.Substring(1);

            _rootDirectory = Path.Combine(exactPath, _rootDirectory);

            //    filename = filename.Substring(1);

            ///     Console.WriteLine(filename);
            if (string.IsNullOrEmpty(filename))
            {
                foreach (string indexFile in _indexFiles)
                {
                    if (File.Exists(Path.Combine(_rootDirectory, indexFile)))
                    {
                        filename = indexFile;
                        break;
                    }
                }
            }
            
            filename = Path.Combine(_rootDirectory, filename);

            if (File.Exists(filename))
            {
                try
                {
                    Stream input = new FileStream(filename, FileMode.Open);

                    //Adding permanent http response headers
                    string mime;
                    res.httpListenerResponse.ContentType = _mimeTypeMappings.TryGetValue(Path.GetExtension(filename), out mime) ? mime : "application/octet-stream";
                    res.httpListenerResponse.ContentLength64 = input.Length;
                    res.httpListenerResponse.AddHeader("Date", DateTime.Now.ToString("r"));
                    res.httpListenerResponse.AddHeader("Last-Modified", System.IO.File.GetLastWriteTime(filename).ToString("r"));

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
                }
                res.httpListenerResponse.OutputStream.Close();
            }
            else
            {
                // res.httpListenerResponse.StatusCode = (int)HttpStatusCode.NotFound;
                res.Content = "File not found: <p>" + filename  + "</p>";
                res.ContentType = "text/html";
                await res.SendAsync();
            }

            
        }

    }
}
