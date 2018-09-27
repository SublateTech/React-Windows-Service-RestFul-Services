using SimpleHttp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SqlMagic.Monitor.Sql;
using SqlMagic.Monitor.Items.Results;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Net;
using OfficeOpenXml.Style;
using System.Drawing;


namespace ShiolWinSvc
{
    class Contabilidad
    {
        public static DataSet currentMayorizadoDataset;
        List<Column> columns;
        List<Column> _columns;

        public enum FileType
        {
            Json = 1,
            Xlsx,
            Pdf
        }

        public string ExportWorkbookToPdf(string workbookPath, string outputPath, bool show)
        {

            //outputPath += @"\Sample.pdf";
            
            // If either required string is null or empty, stop and bail out
            if (string.IsNullOrEmpty(workbookPath) || string.IsNullOrEmpty(outputPath))
            {
                return "";
            }

            // Create COM Objects
            Microsoft.Office.Interop.Excel.Application excelApplication;
            Microsoft.Office.Interop.Excel.Workbook excelWorkbook;

            // Create new instance of Excel
            excelApplication = new Microsoft.Office.Interop.Excel.Application();

            // Make the process invisible to the user
            excelApplication.ScreenUpdating = false;
            

            // Make the process silent
            excelApplication.DisplayAlerts = false;

            // Open the workbook that you wish to export to PDF
            excelWorkbook = excelApplication.Workbooks.Open(workbookPath);

            //'~~> Change as applicable
            var xlsheet = excelWorkbook.Sheets[1];
            //xlsheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
            // A4 papersize
            //xlsheet.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA4;
            // Landscape orientation
            //xlsheet.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
            // Fit Sheet on One Page 
            //xlsheet.FitToPagesWide = 1;
            //xlsheet.FitToPagesTall = 1;
            // Normal Margins
            /*
            xlsheet.LeftMargin = xlsheet.InchesToPoints(0.7);
            xlsheet.RightMargin = xlsheet.InchesToPoints(0.7);
            xlsheet.TopMargin = xlsheet.InchesToPoints(0.75);
            xlsheet.BottomMargin = xlsheet.InchesToPoints(0.75);
            xlsheet.HeaderMargin = xlsheet.InchesToPoints(0.3);
            xlsheet.FooterMargin = xlsheet.InchesToPoints(0.3);

            xlsheet.Zoom = false;
            */

            // If the workbook failed to open, stop, clean up, and bail out
            if (excelWorkbook == null)
            {
                excelApplication.Quit();

                excelApplication = null;
                excelWorkbook = null;

                return "";
            }

            var exportSuccessful = true;
            try
            {
                // Call Excel's native export function (valid in Office 2007 and Office 2010, AFAIK)
                excelWorkbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, outputPath);
            }
            catch (System.Exception ex)
            {
                // Mark the export as failed for the return value...
                exportSuccessful = false;

                // Do something with any exceptions here, if you wish...
                // MessageBox.Show...        
            }
            finally
            {
                // Close the workbook, quit the Excel, and clean up regardless of the results...
                excelWorkbook.Close();
                excelApplication.Quit();

                excelApplication = null;
                excelWorkbook = null;
            }

            // You can use the following method to automatically open the PDF after export if you wish
            // Make sure that the file actually exists first...
            if (System.IO.File.Exists(outputPath) && show)
            {
                System.Diagnostics.Process.Start(outputPath);
            }

            return outputPath;
        }
        public DataTable getContabilidadEstadoResultadosDataTable(Request req, Response res)
        {
            // Declare once, keep using
            var nTimeout = 30; // timeout
            var bLog = true;

            var connectionString = ShiolConfiguration.Instance.Config.SqlServerConnection.ConnectionString;
            // Create with a timeout of 30 seconds and logging as true
            var oSql = new Sql(connectionString, nTimeout, bLog);

            /*
            exec UCO_Mayorizados_Select_PorMes_ComparativoPorHotel @TipoFormatoGGPPID=2,@TipoCuentaID=0,@Ejercicio=2017,
	                @NivelID=0,@HotelID=0,@EstablecimientoID=112,@AreaID=0,@UnidadNegocioID=0,
	                @Periodo='PeriodoID<=12',@OrderByExpression='',@FormatoReporte=2
             */

            //Where Statement
            int HotelID = req.PathVariables["HotelID"] != null ? Convert.ToInt32(req.PathVariables["HotelID"]) : 0;
            columns = new List<Column>();
            _columns = new List<Column>();
            columns.Add(new Column("Descripcion", "Descripcion"));

            if (HotelID > 0)
            {
                var strSqlC = @"Select * from Hoteles  Where HotelID=@HotelId";
                SqlResultWithDataSet oResult1 = oSql.Open(strSqlC, CommandType.Text,
                    new SqlParameter("HotelId", HotelID)
                    );

                columns.Add(new Column((oResult1.Results.Tables[0].Rows.Count == 1 ? oResult1.Results.Tables[0].Rows[0]["NombreHotel"].ToString().Trim() : ""), "H1"));
            }

            else
            {
                columns.Add(new Column("Todos", "Todos"));
                columns.Add(new Column("SALKANTAY", "H1"));
                columns.Add(new Column("LARES", "H2"));
                columns.Add(new Column("EL MERCADO", "H3"));
                columns.Add(new Column("VIÑAK", "H4"));
                columns.Add(new Column("CORPORATIVO", "H5"));
                columns.Add(new Column("HUACAHUASI", "H6"));
                columns.Add(new Column("OFICINA CUSCO", "H7"));
            }
            foreach (Column col in columns)
            {   
                _columns.Add(new Column("", col.accessor));
            }

            if (req.PathVariables["mes"] == null || req.PathVariables["tipo"] == null || req.PathVariables["moneda"] == null)
                return new DataTable();

            int Ejercicio = req.PathVariables["periodo"] != null ? Convert.ToInt32(req.PathVariables["periodo"]) : 2017;
            int EstablecimientoID = req.PathVariables["empresa"] != null ? Convert.ToInt32(req.PathVariables["empresa"]) : 112;
            int Periodo = req.PathVariables["mes"] != null ? Convert.ToInt32(req.PathVariables["mes"]) : 13;
            int TipoFormato = req.PathVariables["TipoFormatoGGPPID"] != null ? Convert.ToInt32(req.PathVariables["TipoFormatoGGPPID"]) : 2;
            string Moneda = req.PathVariables["moneda"] != null ? req.PathVariables["moneda"] : "USD"; //Soles
            string sTipo = req.PathVariables["tipo"] != null ? req.PathVariables["tipo"] : "mensual"; //0
            int Tipo = sTipo == "mensual" ? 1 : 0;

            bool isSoles = (Moneda == "USD" ? false : true);
            /*
             * exec UCO_Mayorizados_Select_PorMes_ComparativoPorHotel @TipoFormatoGGPPID=2,@TipoCuentaID=0,@Ejercicio=2018,@NivelID=0,@HotelID=0,@EstablecimientoID=112,@AreaID=0,@UnidadNegocioID=0,@Periodo='PeriodoID<=5',@OrderByExpression='',@FormatoReporte=2
             */
            var strSql = @"exec UCO_Mayorizados_Select_PorMes_ComparativoPorHotel @TipoFormatoGGPPID="+ TipoFormato + @",@TipoCuentaID= 0,@Ejercicio="+Ejercicio+ @",
                    @NivelID = 0,@HotelID = "+ HotelID + @",@EstablecimientoID = "+EstablecimientoID+ @",@AreaID = 0,@UnidadNegocioID = 0,
	                @Periodo = 'PeriodoID "+ (Tipo==1?"=":"<=") + Periodo + "',@OrderByExpression = '',@FormatoReporte = 2";
            SqlResultWithDataSet oResult = oSql.Open(strSql, CommandType.Text);

            return oResult.Results.Tables[0];

        }

        public bool getContabilidadEstadoResultados(Request req, Response res, FileType type)
        {
            ReporteEstadoResultado reporte = new ReporteEstadoResultado();
            

            DataTable dt = getContabilidadEstadoResultadosDataTable(req, res);

            reporte._columnsGroups = _columns;
            reporte._columnsSubGroups = _columns;
            reporte._columns = _columns;
            reporte.columns = columns;
            reporte.Descripcion = "Total General";

            


            string Moneda = req.PathVariables["moneda"] != null ? req.PathVariables["moneda"] : "USD"; //Soles
            bool isSoles = (Moneda == "USD" ? false : true);

            string json = null;

            try
            {
                string NombreTipoCuenta = "";
                string NombreFormatoBalance = "";


                List<ReporteEstadoResultadoGroup> listGroups = new List<ReporteEstadoResultadoGroup>();
                ReporteEstadoResultadoSubGroup subGroup = new ReporteEstadoResultadoSubGroup();
                ReporteEstadoResultadoGroup group = new ReporteEstadoResultadoGroup();




                Int32 idGroup = -1;
                Int32 idSubGroup = -1;
                Int32 idDetail = -1;

                foreach (DataRow row in dt.Rows)
                {
                    if (NombreTipoCuenta != row["NombreTipoCuenta"].ToString())
                    {
                        idGroup++;
                        idSubGroup = -1;
                        group = new ReporteEstadoResultadoGroup();
                        NombreFormatoBalance = "";

                        NombreTipoCuenta = row["NombreTipoCuenta"].ToString();
                        group.Descripcion = "Cuenta: " + row["NombreTipoCuenta"].ToString().Trim();
                        group.id = idGroup.ToString();
                        listGroups.Add(group);
                    }


                    if (NombreFormatoBalance != row["NombreFormatoBalance"].ToString())
                    {

                        idSubGroup++;
                        idDetail = -1;
                        subGroup = new ReporteEstadoResultadoSubGroup();

                        NombreFormatoBalance = row["NombreFormatoBalance"].ToString();
                        subGroup.Descripcion = "    Tipo: " + row["NombreFormatoBalance"].ToString().Trim();
                        subGroup.id = idSubGroup.ToString();

                        listGroups[idGroup].subGroups.Add(subGroup);
                    }

                    idDetail++;
                    ReporteEstadoResultadoSubGroupChild field = new ReporteEstadoResultadoSubGroupChild();
                    field.Descripcion = row["NombreFormatoGanaciasPerdidas"].ToString().Trim();
                    field.H1.value = isSoles ? Convert.ToDouble(row["H1Soles"].ToString()) : Convert.ToDouble(row["H1Dolares"].ToString());
                    field.H2.value = isSoles ? Convert.ToDouble(row["H2Soles"].ToString()) : Convert.ToDouble(row["H2Dolares"].ToString());
                    field.H3.value = isSoles ? Convert.ToDouble(row["H3Soles"].ToString()) : Convert.ToDouble(row["H3Dolares"].ToString());
                    field.H4.value = isSoles ? Convert.ToDouble(row["H4Soles"].ToString()) : Convert.ToDouble(row["H4Dolares"].ToString());
                    field.H5.value = isSoles ? Convert.ToDouble(row["H5Soles"].ToString()) : Convert.ToDouble(row["H5Dolares"].ToString());
                    field.H6.value = isSoles ? Convert.ToDouble(row["H6Soles"].ToString()) : Convert.ToDouble(row["H6Dolares"].ToString());
                    field.H7.value = isSoles ? Convert.ToDouble(row["H7Soles"].ToString()) : Convert.ToDouble(row["H7Dolares"].ToString());
                    field.H8.value = isSoles ? Convert.ToDouble(row["H8Soles"].ToString()) : Convert.ToDouble(row["H8Dolares"].ToString());
                    field.Todos.value = field.H1.value + field.H2.value + field.H3.value + field.H4.value + field.H5.value + field.H6.value + field.H7.value;

                    listGroups[idGroup].subGroups[idSubGroup].subGroupChilds.Add(field);

                    listGroups[idGroup].subGroups[idSubGroup].H1 += field.H1.value;
                    listGroups[idGroup].subGroups[idSubGroup].H2 += field.H2.value;
                    listGroups[idGroup].subGroups[idSubGroup].H3 += field.H3.value;
                    listGroups[idGroup].subGroups[idSubGroup].H4 += field.H4.value;
                    listGroups[idGroup].subGroups[idSubGroup].H5 += field.H5.value;
                    listGroups[idGroup].subGroups[idSubGroup].H6 += field.H6.value;
                    listGroups[idGroup].subGroups[idSubGroup].H7 += field.H7.value;
                    listGroups[idGroup].subGroups[idSubGroup].Todos += field.Todos.value;

                    listGroups[idGroup].H1 += field.H1.value;
                    listGroups[idGroup].H2 += field.H2.value;
                    listGroups[idGroup].H3 += field.H3.value;
                    listGroups[idGroup].H4 += field.H4.value;
                    listGroups[idGroup].H5 += field.H5.value;
                    listGroups[idGroup].H6 += field.H6.value;
                    listGroups[idGroup].H7 += field.H7.value;
                    listGroups[idGroup].Todos += field.Todos.value;

                    
                    reporte.H1.value += field.H1.value;
                    reporte.H2.value += field.H2.value;
                    reporte.H3.value += field.H3.value;
                    reporte.H4.value += field.H4.value;
                    reporte.H5.value += field.H5.value;
                    reporte.H6.value += field.H6.value;
                    reporte.H7.value += field.H7.value;
                    reporte.Todos.value += field.Todos.value;
                    

                }

                reporte.groups = listGroups;
                

                List<ReporteEstadoResultado> report = new List<ReporteEstadoResultado>();
                report.Add(reporte);


                res.httpListenerResponse.Headers.Add("Access-Control-Expose-Headers", "X-Total-Count");
                res.httpListenerResponse.Headers.Add("X-Total-Count", dt.Rows.Count.ToString());

                if (type == FileType.Json)
                {
                    json = JsonConvert.SerializeObject(report);
                    
                    res.Content = json;
                    res.ContentType = "application/json";
                    res.httpListenerResponse.StatusCode = 200; //401 for error;

                }
                if (type == FileType.Xlsx | type == FileType.Pdf)
                {
                    string excelPath = getMayorizadoExcelFile(reporte);

                    if (type == FileType.Pdf)
                    {
                        string pdfFile = ExportWorkbookToPdf(excelPath, ShiolConfiguration.Instance.Config.DirPlantillas + @"\Mayorizado.pdf", false);
                        WriteFile(res, pdfFile, "Mayorizado.pdf");
                    }
                    else
                        WriteFile(res, excelPath, "Mayorizado.xlsx");

                    //int converted = ExcelConverter.Convert(excelPath, ShiolConfiguration.Instance.Config.DirPlantillas + "\\Test.pdf", null);
                }

                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public string getMayorizadoExcelFile(ReporteEstadoResultado reporte)
        {

            string tmpFile = "Mayorizado_" + Utils.Global.GenerateName(10) + ".xlsx";
            //@"d:\ShiolFramework\tmp"
            DirectoryInfo outputDir = new DirectoryInfo(ShiolConfiguration.Instance.Config.DirPlantillas);

            FileInfo newFile = new FileInfo(Path.Combine(outputDir.FullName , tmpFile));
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(Path.Combine(outputDir.FullName, tmpFile));
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Mayorizado");
                //Add the headers

                int col = 1;
                int init_row = 4, row = 4;
                foreach (Column column in reporte.columns)
                {
                    if (column.Header.ToUpper() != "TODOS")
                        worksheet.Cells[row, col++].Value = column.Header;
                }

                worksheet.Cells[row, col++].Value = "Adjustments \n (+) &(-)";
                worksheet.Cells[row, col++].Value = "Consolidado Total";
                worksheet.Cells[row, col++].Value = "Bdg 2017 TTD";
                worksheet.Cells[row, col++].Value = "Exe 2016 TTD";
                worksheet.Cells[row, col++].Value = "Var % Exe vs Bgd 2017";
                worksheet.Cells[row, col++].Value = "Var % Exe 2017 vs 2016";
                worksheet.Cells[row, col++].Value = "Unit Analysis";
                worksheet.Cells[row, col++].Value = "% of Sales";
                worksheet.Cells[row, col++].Value = "Unit Analysis";
                worksheet.Cells[row, col++].Value = "% of Sales";
                /*
                worksheet.Cells[1, col++].Value = "";
                worksheet.Cells[1, col++].Value = "";
                worksheet.Cells[1, col++].Value = "";
                worksheet.Cells[1, col++].Value = "";
                */


                //worksheet.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.LightGray;
                worksheet.Cells["A" + row + ":" + "R" + row].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                //ws.Cells["B24"].Style.Font.Bold = true;
                //ws.Cells["B24:X24"].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                //worksheet.Cells["A4:G4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells["A4:G4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                //worksheet.Cells[startRow, 1, row - 1, 1].StyleName = "HyperLink";
                //worksheet.Cells[startRow, 4, row - 1, 6].Style.Numberformat.Format = "[$$-409]#,##0";
                //worksheet.Cells[startRow, 7, row - 1, 7].Style.Numberformat.Format = "0%";

                //worksheet.Cells[startRow, 7, row - 1, 7].FormulaR1C1 = "=IF(RC[-2]=0,0,RC[-1]/RC[-2])";


                int numberWidth = 15;
                worksheet.Column(1).Width = 45;
                worksheet.Column(2).Width = numberWidth;
                worksheet.Column(3).Width = numberWidth;
                worksheet.Column(4).Width = numberWidth;
                worksheet.Column(5).Width = numberWidth;
                worksheet.Column(6).Width = numberWidth;
                worksheet.Column(7).Width = numberWidth;
                worksheet.Column(8).Width = numberWidth;
                worksheet.Column(9).Width = numberWidth;

                row++;
                foreach (ReporteEstadoResultadoGroup group in reporte.groups)
                {
                    worksheet.Cells["A"+ row].Value = group.Descripcion;
                    
                   // worksheet.Cells[row, 2].Value = group.Todos;
                    worksheet.Cells[row, 2].Value = group.H1;
                    worksheet.Cells[row, 3].Value = group.H2;
                    worksheet.Cells[row, 4].Value = group.H3;
                    worksheet.Cells[row, 5].Value = group.H4;
                    worksheet.Cells[row, 6].Value = group.H5;
                    worksheet.Cells[row, 7].Value = group.H6;
                    worksheet.Cells[row, 8].Value = group.H7;
                    worksheet.Cells["B"+row+":" +"J"+row] .Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells["A" + row + ":" + "J" + row].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells["B" + row + ":" + "J" + row].Style.Font.Bold = true ;

                    //Consolidado
                    worksheet.Cells["J" + row].Formula = "=SUM(B" + row + ":" + "H" + row + ")";
                    row++;
                    foreach (ReporteEstadoResultadoSubGroup sgroup in group.subGroups)
                    {
                        worksheet.Cells[row, 1].Value = "         " + sgroup.Descripcion;
                        
                        //worksheet.Cells[row, 2].Value = sgroup.Todos;
                        worksheet.Cells[row, 2].Value = sgroup.H1;
                        worksheet.Cells[row, 3].Value = sgroup.H2;
                        worksheet.Cells[row, 4].Value = sgroup.H3;
                        worksheet.Cells[row, 5].Value = sgroup.H4;
                        worksheet.Cells[row, 6].Value = sgroup.H5;
                        worksheet.Cells[row, 7].Value = sgroup.H6;
                        worksheet.Cells[row, 8].Value = sgroup.H7;
                        worksheet.Cells["B" + row + ":" + "J" + row].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells["B" + row + ":" + "J" + row].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                        worksheet.Cells["B" + row + ":" + "J" + row].Style.Font.Bold = true;
                        //Consolidado
                        worksheet.Cells["J" + row].Formula = "=SUM(B" + row + ":" + "H" + row + ")";
                        row++;
                        int rowsg = row;
                        foreach (ReporteEstadoResultadoSubGroupChild sgroupc in sgroup.subGroupChilds)
                        {
                            worksheet.Cells[row, 1].Value = "         " + "         " + sgroupc.Descripcion;
                            
                            //worksheet.Cells[row, 2].Value = sgroupc.Todos;
                            worksheet.Cells[row, 2].Value = sgroupc.H1;
                            worksheet.Cells[row, 3].Value = sgroupc.H2;
                            worksheet.Cells[row, 4].Value = sgroupc.H3;
                            worksheet.Cells[row, 5].Value = sgroupc.H4;
                            worksheet.Cells[row, 6].Value = sgroupc.H5;
                            worksheet.Cells[row, 7].Value = sgroupc.H6;
                            worksheet.Cells[row, 8].Value = sgroupc.H7;
                            worksheet.Cells["B" + row + ":" + "J" + row].Style.Numberformat.Format = "#,##0.00";
                            //Consolidado
                            worksheet.Cells["J" + row].Formula = "=SUM(B" + row + ":" + "H" + row + ")";
                            row++;
                        }
                        worksheet.Cells["B" + rowsg + ":" + "J" + (row -1)].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }

                }
                worksheet.Cells.AutoFitColumns(0);  //Autofit columns for all cells
                worksheet.Cells["A"+ init_row +":" + "I" + (row - 1)].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                //Add a formula for the value-column
                //  worksheet.Cells["E2:E4"].Formula = "C2*D2";

                /*
                //Ok now format the values;
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                worksheet.Cells["A5:E5"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A5:E5"].Style.Font.Bold = true;

                worksheet.Cells[5, 3, 5, 5].Formula = string.Format("SUBTOTAL(9,{0})", new ExcelAddress(2, 3, 4, 3).Address);
                worksheet.Cells["C2:C5"].Style.Numberformat.Format = "#,##0";
                worksheet.Cells["D2:E5"].Style.Numberformat.Format = "#,##0.00";

                //Create an autofilter for the range
                worksheet.Cells["A1:E4"].AutoFilter = true;

                worksheet.Cells["A2:A4"].Style.Numberformat.Format = "@";   //Format as text

                //There is actually no need to calculate, Excel will do it for you, but in some cases it might be useful. 
                //For example if you link to this workbook from another workbook or you will open the workbook in a program that hasn't a calculation engine or 
                //you want to use the result of a formula in your program.
                worksheet.Calculate();

                worksheet.Cells.AutoFitColumns(0);  //Autofit columns for all cells

                // lets set the header text 
                worksheet.HeaderFooter.OddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\" Inventory";
                // add the page number to the footer plus the total number of pages
                worksheet.HeaderFooter.OddFooter.RightAlignedText =
                    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                // add the sheet name to the footer
                worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                // add the file path to the footer
                worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;

                worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:2"];
                worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:G"];

                // Change the sheet view to show it in page layout mode
                worksheet.View.PageLayoutView = true;
                */

                // set some document properties
                package.Workbook.Properties.Title = "Mayorizado";
                package.Workbook.Properties.Author = "Alvaro Medina";
                package.Workbook.Properties.Comments = "Mayorización...";

                // set some extended property values
                package.Workbook.Properties.Company = "Shiol Sistemas";

                // set some custom property values
                package.Workbook.Properties.SetCustomPropertyValue("Checked by", "Alvaro Medina");
                package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "SHG Sistemas");
                // save our new workbook and we are done!
                package.Save();

            }

            return newFile.FullName;

        }
        public string getMayorizadoExcelFile1(DataTable dt, ReporteEstadoResultado reporte)
        {
            //@"d:\ShiolFramework\tmp"
            DirectoryInfo outputDir = new DirectoryInfo(ShiolConfiguration.Instance.Config.DirPlantillas);

            ExcelPackage pck = new ExcelPackage();

            //Create a datatable with the directories and files from the root directory...
            // DataTable dt = GetDataTable(outputDir.Root);

            var wsDt = pck.Workbook.Worksheets.Add("FromDataTable");

            //Load the datatable and set the number formats...
            wsDt.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.Medium9);
            wsDt.Cells[2, 2, dt.Rows.Count + 1, 2].Style.Numberformat.Format = "#,##0";
            wsDt.Cells[2, 3, dt.Rows.Count + 1, 4].Style.Numberformat.Format = "mm-dd-yy";
            wsDt.Cells[wsDt.Dimension.Address].AutoFitColumns();
           /*
            
            //Select Name and Created-time...
            var collection = (from row in dt.Select() select new { Name = row["Name"], Created_time = (DateTime)row["Created"] });

            
           
            var wsEnum = pck.Workbook.Worksheets.Add("FromAnonymous");
            
            //Load the collection starting from cell A1...
            wsEnum.Cells["A1"].LoadFromCollection(collection, true, TableStyles.Medium9);

            //Add some formating...
            wsEnum.Cells[2, 2, dt.Rows.Count - 1, 2].Style.Numberformat.Format = "mm-dd-yy";
            wsEnum.Cells[wsEnum.Dimension.Address].AutoFitColumns();

            //Load a list of FileDTO objects from the datatable...
            var wsList = pck.Workbook.Worksheets.Add("FromList");
            List<FileDTO> list = (from row in dt.Select()
                                  select new FileDTO
                                  {
                                      Name = row["Name"].ToString(),
                                      Size = row["Size"].GetType() == typeof(long) ? (long)row["Size"] : 0,
                                      Created = (DateTime)row["Created"],
                                      LastModified = (DateTime)row["Modified"],
                                      IsDirectory = (row["Size"] == DBNull.Value)
                                  }).ToList<FileDTO>();

            //Load files ordered by size...
            wsList.Cells["A1"].LoadFromCollection(from file in list
                                                  orderby file.Size descending
                                                  where file.IsDirectory == false
                                                  select file, true, TableStyles.Medium9);

            wsList.Cells[2, 2, dt.Rows.Count + 1, 2].Style.Numberformat.Format = "#,##0";
            wsList.Cells[2, 3, dt.Rows.Count + 1, 4].Style.Numberformat.Format = "mm-dd-yy";


            //Load directories ordered by Name...
            wsList.Cells["F1"].LoadFromCollection(from file in list
                                                  orderby file.Name ascending
                                                  where file.IsDirectory == true
                                                  select new
                                                  {
                                                      Name = file.Name,
                                                      Created = file.Created,
                                                      Last_modified = file.LastModified
                                                  }, //Use an underscore in the property name to get a space in the title.
                                                  true, TableStyles.Medium11);

            wsList.Cells[2, 7, dt.Rows.Count + 1, 8].Style.Numberformat.Format = "mm-dd-yy";

            //Load the list using a specified array of MemberInfo objects. Properties, fields and methods are supported.
            var rng = wsList.Cells["J1"].LoadFromCollection(list,
                                                  true,
                                                  TableStyles.Medium10,
                                                  BindingFlags.Instance | BindingFlags.Public,
                                                  new MemberInfo[] {
                                                      typeof(FileDTO).GetProperty("Name"),
                                                      typeof(FileDTO).GetField("IsDirectory"),
                                                      typeof(FileDTO).GetMethod("ToString")}
                                                  );

            wsList.Tables.GetFromRange(rng).Columns[2].Name = "Description";

            wsList.Cells[wsList.Dimension.Address].AutoFitColumns();
            */

            //...and save
            var fi = new FileInfo(outputDir.FullName + @"\Mayorizado.xlsx");
            if (fi.Exists)
            {
                fi.Delete();
            }
            pck.SaveAs(fi);

            return fi.FullName;

        }

        private void WriteFile(Response res, string path, string asFileName )
        {
            // path = @"D:\SGH_Sistemas\ShiolRESTService\SHIOLRH02.xlsx";

            string aspath = Path.Combine(ShiolConfiguration.Instance.Config.DirPlantillas, asFileName);

            var response = res.httpListenerResponse;
            using (FileStream fs = File.OpenRead(path))
            {
                string filename = Path.GetFileName(path);


                //response is HttpListenerContext.Response...
                response.ContentLength64 = fs.Length;
                response.SendChunked = false;
                response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                response.AddHeader("Content-disposition", "attachment; filename=" + asFileName);

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


        public class FileDTO
        {
            public string Name { get; set; }
            public long Size { get; set; }
            public DateTime Created { get; set; }
            public DateTime LastModified { get; set; }

            public bool IsDirectory = false;                  //This is a field variable

            public override string ToString()
            {
                if (IsDirectory)
                {
                    return Name + "\t<Dir>";
                }
                else
                {
                    return Name + "\t" + Size.ToString("#,##0");
                }
            }
        }
        public static string getContabilidadEstadoResultadosGroups()
        {

            //if (currentMayorizadoDataset == null)
            //  getContabilidadEstadoResultados();

            DataTable table = currentMayorizadoDataset.Tables[0];
            var groupedData = from b in table.AsEnumerable()
                              group b by b.Field<string>("NombreTipoCuenta") into g
                              select new
                              {
                                  NombreTipoCuenta = g.Key
                              };

            string json = null;
            List<ReporteEstadoResultado> fields = new List<ReporteEstadoResultado>();
            try
            {
                Int32 id = 1;
                foreach (var row in groupedData)
                {
                    ReporteEstadoResultado field = new ReporteEstadoResultado();
                    field.id = id.ToString();
                    // field.Descripcion = row.NombreTipoCuenta;
                    fields.Add(field);
                    id++;
                }
                json = JsonConvert.SerializeObject(fields);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(json);
            return json;

        }

        public static string getContabilidadEstadoResultadosSubGroups(string sGroup)
        {

            //  if (currentMayorizadoDataset == null)
            //      getContabilidadEstadoResultados();

            DataTable table = currentMayorizadoDataset.Tables[0];
            var groupedData = from b in table.AsEnumerable()
                              where b.Field<string>("NombreFormatoBalance") == sGroup
                              group b by b.Field<string>("NombreFormatoBalance") into g
                              select new
                              {
                                  NombreFormatoBalance = g.Key
                              };

            string json = null;
            List<ReporteEstadoResultado> fields = new List<ReporteEstadoResultado>();
            try
            {
                Int32 id = 1;
                foreach (var row in groupedData)
                {
                    ReporteEstadoResultado field = new ReporteEstadoResultado();
                    field.id = id.ToString();
                    //field.NombreTipoCuenta = row.NombreFormatoBalance;
                    fields.Add(field);
                    id++;
                }
                json = JsonConvert.SerializeObject(fields);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return json;

        }

        public class ReporteEstadoResultado
        {
            public string id;
            public string Descripcion;
            public List<ReporteEstadoResultadoGroup> groups = new List<ReporteEstadoResultadoGroup>();
            //Totales
            public Params Todos = new Params();
            public Params H1 = new Params();
            public Params H2 = new Params();
            public Params H3 = new Params();
            public Params H4 = new Params();
            public Params H5 = new Params();
            public Params H6 = new Params();
            public Params H7 = new Params();
            public Params H8 = new Params();

            //Columns
            public List<Column> columns;
            public List<Column> _columnsGroups;
            public List<Column> _columnsSubGroups;
            public List<Column> _columns;
        }

        public class ReporteEstadoResultadoGroup
        {
            public string id;
            //public string Descripcion;
            public string Descripcion;
            public List<ReporteEstadoResultadoSubGroup> subGroups = new List<ReporteEstadoResultadoSubGroup>();
            //Totales
            public double Todos;
            public double H1;
            public double H2;
            public double H3;
            public double H4;
            public double H5;
            public double H6;
            public double H7;
            public double H8;            
        }

        public class Params
        {
            public double value = 0;
            public string param1 = "par1";
            public string param2 = "par2";
        }
        public class Column
        {
            public string Header;
            public string accessor;
            
            public Column(string name, string field)
            {
                Header = name;
                accessor = field;
            }
        }

        public class ReporteEstadoResultadoSubGroup
        {
            public string id;
            public string Descripcion;
            public List<ReporteEstadoResultadoSubGroupChild> subGroupChilds = new List<ReporteEstadoResultadoSubGroupChild>();
            //Totales
            public double Todos;
            public double H1;
            public double H2;
            public double H3;
            public double H4;
            public double H5;
            public double H6;
            public double H7;
            public double H8;
          }

        public class ReporteEstadoResultadoSubGroupChild
        {
            public string id;
            public string Descripcion;
            public Params Todos = new Params();
            public Params H1 = new Params();
            public Params H2 = new Params();
            public Params H3 = new Params();
            public Params H4 = new Params();
            public Params H5 = new Params();
            public Params H6 = new Params();
            public Params H7 = new Params();
            public Params H8 = new Params();
        }
    }
}
