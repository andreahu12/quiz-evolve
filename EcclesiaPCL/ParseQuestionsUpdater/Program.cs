using Parse;
using ParseQuestionsUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Settings;

namespace ParseQuestionsUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                ParseClient.Initialize(Constants.ParseID, Constants.ParseNETKey);
                var excelUri = @"C:\Users\Anton\Documents\GitHub\ecclesia-evolve\Docs\Questions\" + args[0] + ".xlsx";
                var reader = new ExcelReader();
                reader.ReadExcel(excelUri, args[0]);
            }
            else
            {
                Console.WriteLine(string.Format("Wrong number of arguments ({0}}, expected - 1", args.Length));
            }
            Console.ReadKey();
        }
    }
}
