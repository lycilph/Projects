using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClosedXML.Excel;

namespace ClosedXMLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var workbook = new XLWorkbook("Input.xlsx");
            var ws = workbook.Worksheet("Posts");

            var cell = ws.Column(3).FirstCellUsed().CellBelow();
            while (!cell.IsEmpty())
            {
                int value = cell.GetValue<int>();
                Console.WriteLine("Value: " + value);
                cell.CellRight().Style.Fill.BackgroundColor = (value < 0 ? XLColor.Red : XLColor.Green);
                cell = cell.CellBelow();
            }

            workbook.SaveAs("Output.xlsx");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
