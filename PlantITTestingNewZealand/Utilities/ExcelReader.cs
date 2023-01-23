using System;
using IronXL;
using Newtonsoft.Json.Linq;
using System.Text;
using AventStack.ExtentReports.Reporter;

namespace PlantITTestingNewZealand.Utilities
{
    public class ExcelReader
    {

            string fileName = Environment.CurrentDirectory.ToString() + "\\Utilities\\SeleniumTraining.xlsx";

        /*  [Test]
          public void testing()
          {
              Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
              TestContext.WriteLine(fileName);
              TestContext.WriteLine("Result is : " + readColumnRowCell("Hello", "Points1"));

          }*/

        public string partPath = "";
        public ExcelReader() {

            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;
            switch (pid)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    partPath = "/Utilities/SeleniumTraining.xlsx";
                    break;
                case PlatformID.MacOSX:
                    partPath = "\\Utilities\\SeleniumTraining.xlsx";
                    break;
                default:
                    partPath = "/Utilities/SeleniumTraining.xlsx";
                    TestContext.WriteLine("No Idea what I'm on!");
                    break;
            }

        }

            /// <summary>
            /// This Reads the data from the Data Table.
            /// </summary>
            /// <returns> None </returns>
            public void readOnExcel(string sheet)
            {
                String fileName = Environment.CurrentDirectory.ToString() + partPath;
                var workbook = WorkBook.Load(fileName);
                var worksheet = workbook.GetWorkSheet("sheet");
                int columnCount = worksheet.ColumnCount;
                int rowCount = worksheet.RowCount;
                TestContext.WriteLine("Column Count " + columnCount);
                TestContext.WriteLine("Row Count " + rowCount);

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        try
                        {
                            var cell = worksheet.GetCellAt(i, j).StringValue;
                            TestContext.Write(worksheet.GetCellAt(i, j).Value + " ");
                        }
                        catch (NullReferenceException e)
                        {
                            TestContext.WriteLine("\nRowNumber: " + i + " ColumnNumber: " + j + " is null");
                        }
                    }
                    TestContext.WriteLine();

                }
                workbook.SaveAs(fileName);
                workbook.Close();
            }

        public int getCountOfProductsFromExcelTable() {
            String fileName = Environment.CurrentDirectory.ToString() + partPath;
            var workbook = WorkBook.Load(fileName);
            var worksheet = workbook.GetWorkSheet("Items");
            int rowCount = worksheet.RowCount;

            return rowCount - 1;

        }
        /// <summary>
        /// This Gets the location of the referenced cell
        /// </summary>
        /// <param name="rowName"> This is the row name </param>
        /// <param name="columnName"> This is the column name </param>
        /// <returns> returnValue - String Value of the Cell Referenced by the ColumnName and RowName </returns>
        public string readColumnRowCell(string sheet, String rowName, String columnName)
            {
                int[] returnValue = new int[2];

                String fileName = Environment.CurrentDirectory.ToString() + partPath;
                var workbook = WorkBook.Load(fileName);
                var worksheet = workbook.GetWorkSheet(sheet);
                int columnCount = worksheet.ColumnCount;
                int rowCount = worksheet.RowCount;
                TestContext.WriteLine("Column Count " + columnCount);
                TestContext.WriteLine("Row Count " + rowCount);

                //Get Row Number of Referenced Data
                for (int i = 0; i < rowCount; i++)
                {
                    if (worksheet.GetCellAt(i, 0).StringValue == rowName)
                    {
                        returnValue[0] = i;
                        break;
                    }

                }
                //Get Column Number of the Referenced Data
                for (int j = 0; j < columnCount; j++)
                {
                    if (worksheet.GetCellAt(0, j).StringValue == columnName)
                    {
                        returnValue[1] = j;
                        break;
                    }

                }

                //---------Debugging Codes---------
                TestContext.WriteLine(returnValue[0]);
                TestContext.WriteLine(returnValue[1]);
                //---------Debugging Codes---------
                string returnCellValue = worksheet.GetCellAt(returnValue[0], returnValue[1]).StringValue;

                //---------Debugging Codes---------
                TestContext.WriteLine(returnCellValue);
                //---------Debugging Codes---------

                workbook.SaveAs(fileName);
                workbook.Close();
                return returnCellValue;
            }


            public void writeOnExcel(string sheet, string rowName, string columnName, string value) {

                int[] returnValue = new int[2];

                String fileName = Environment.CurrentDirectory.ToString() + partPath;
                ExtentTestManager.GetTest().Log(AventStack.ExtentReports.Status.Info, "" +
                    System.Reflection.MethodBase.GetCurrentMethod().Name +
                    "\n FileName: " + fileName);

                var workbook = WorkBook.Load(fileName);
                var worksheet = workbook.GetWorkSheet(sheet);
                int columnCount = worksheet.ColumnCount;
                int rowCount = worksheet.RowCount;
                TestContext.WriteLine("Column Count " + columnCount);
                TestContext.WriteLine("Row Count " + rowCount);

                //Get Row Number of Referenced Data
                for (int i = 0; i < rowCount; i++)
                {
                    if (worksheet.GetCellAt(i, 0).StringValue == rowName)
                    {
                        returnValue[0] = i;
                        break;
                    }

                }
                //Get Column Number of the Referenced Data
                for (int j = 0; j < columnCount; j++)
                {
                    if (worksheet.GetCellAt(0, j).StringValue == columnName)
                    {
                        returnValue[1] = j;
                        break;
                    }

                }

                //---------Debugging Codes---------
                TestContext.WriteLine(returnValue[0]);
                TestContext.WriteLine(returnValue[1]);
                //---------Debugging Codes---------


                workbook.SaveAs(fileName);
                workbook.Close();

                ExtentTestManager.GetTest().Log(AventStack.ExtentReports.Status.Info, "" +
                System.Reflection.MethodBase.GetCurrentMethod().Name +
                "\n Value: " + value);
                writeOnExcelCell(sheet, returnValue[0], returnValue[1], value);


            }
        
            /// <summary>
            /// This Writes on Excel Using IronXL
            /// </summary>
            /// <param name="rowNumber"> Row number of where the object will be writting </param>
            ///  <param name="columnNumber"> Column Number where the object will be written</param>
            /// <returns> None </returns>
            public void writeOnExcelCell(string sheet, int rowNumber, int columnNumber, String Value)
            {
                String fileName = Environment.CurrentDirectory.ToString() + partPath;
                var workbook = WorkBook.Load(fileName);
                var worksheet = workbook.GetWorkSheet(sheet);
                worksheet.SetCellValue(rowNumber, columnNumber, Value);
                TestContext.WriteLine("Row Number : " + rowNumber);
                TestContext.WriteLine("Column Number: " + columnNumber);
                TestContext.WriteLine("Value: " + Value);
                ExtentTestManager.GetTest().Log(AventStack.ExtentReports.Status.Info, "Value: " + Value);
                workbook.SaveAs(fileName);
                workbook.Close();
            }


            /// <summary>
            /// This method calls Json Variable in order to extract Data
            /// </summary>
            /// <param></param>
            /// <returns></returns>
            public static string extractData(String tokenName)
            {
                var myJsonString = File.ReadAllText("Utilities/test.json");
                var jsonObject = JToken.Parse(myJsonString);
                TestContext.WriteLine(jsonObject.SelectToken(tokenName).Value<string>());
                return jsonObject.SelectToken(tokenName).Value<string>();
            }
            /// <summary>
            /// This method calls Json List of Data and returns it to the program
            /// </summary>
            /// <param></param>
            /// <returns></returns>
            public static string[] extractDataArrays(String tokenName)
            {
                var myJsonString = File.ReadAllText("Utilities/test.json");
                var jsonObject = JToken.Parse(myJsonString);
                List<string> productsList = jsonObject.SelectToken(tokenName).Values<string>().ToList();
                return productsList.ToArray();
            }
        }
    }


