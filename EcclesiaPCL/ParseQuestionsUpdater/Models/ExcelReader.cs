using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseQuestionsUpdater.Models
{
    class ExcelReader
    {
        public async void ReadExcel(string fileName, string tableName)
        {
            Application excelApp;
            Workbook excelWorkbook;

            //first check for valid file - if no file no sense to do anything
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            {
                Console.WriteLine("Can't find file " + fileName);
                return ;
            }

            if (Path.GetExtension(fileName) != ".xlsx")
            {
                Console.WriteLine("Not supported file format " + fileName);
                return ;
            }

            
            //Check if MS office is installed
            if (!IsInstalled())
            {
                Console.WriteLine("Excel is not installed");
                return;
            }
            

            // Now try to launch excel
            try
            {
                excelApp = new Application();
            }
            catch (Exception )
            {
                Console.WriteLine("An error occurred launching Excel");
                return;
            }

            try
            {
                excelWorkbook = excelApp.Workbooks.Open(fileName);

            }
            catch (Exception)
            {
                Console.WriteLine("Can't open file "+fileName);
                excelApp.Quit();
                excelApp=null;
                return;
            }

            Worksheet workSheet = excelWorkbook.Worksheets[1];

            var rv = new List<ParseObject>();
            int row = 3;
            var cellValue =((Range)workSheet.Cells[row,1]).Value;
                        
            while(cellValue!=null)
            {
                int number = Convert.ToInt32(cellValue);
                var parseQuestion = new ParseObject(tableName);
                var query = ParseObject.GetQuery(tableName).WhereEqualTo("Number", number);
                var oldProgress = await query.FirstOrDefaultAsync();
                if (oldProgress != null)
                    parseQuestion.ObjectId = oldProgress.ObjectId;

                parseQuestion["Number"] = number;
                parseQuestion["Question"] = ((Range)workSheet.Cells[row, 2]).Value.ToString();
                parseQuestion["A"] = ((Range)workSheet.Cells[row, 3]).Value.ToString();
                parseQuestion["B"] = ((Range)workSheet.Cells[row, 4]).Value.ToString();
                parseQuestion["C"] = ((Range)workSheet.Cells[row, 5]).Value.ToString();
                parseQuestion["D"] = ((Range)workSheet.Cells[row, 6]).Value.ToString();
                parseQuestion["SolutionNum"] = Convert.ToInt32(((Range)workSheet.Cells[row, 7]).Value);

                await parseQuestion.SaveAsync();

                Console.WriteLine(string.Format("Question {0} is saved",number));
                row++;
                cellValue = ((Range)workSheet.Cells[row, 1]).Value;
            };
            excelWorkbook.Close(false);
            excelApp.Quit();
            excelApp = null;

        }

        private bool IsInstalled()
        {

            string strSubKey = null;

            RegistryKey objKey = Registry.ClassesRoot;
            try
            {
                strSubKey = "Excel.Application";
                
                RegistryKey objSubKey = objKey.OpenSubKey(strSubKey);
                return objSubKey != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                objKey.Close();
            }
            return false;
        }

     }
}
