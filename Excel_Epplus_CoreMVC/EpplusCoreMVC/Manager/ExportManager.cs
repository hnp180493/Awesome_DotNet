using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EpplusCoreMVC.Manager
{
    public class ExportManager
    {
        public static Stream ExportExcelFile<T>(IEnumerable<T> collection, string[] propertyNames, string worksheetName)
        {
            try
            {
                using (var excel = new ExcelPackage())
                {

                    var worksheet = excel.Workbook.Worksheets.Add(worksheetName);
                    MemberInfo[] membersToInclude = typeof(T)
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Where(p => propertyNames.Contains(p.Name))
                        .ToArray();
                    //set header
                    for (int i = 0; i < membersToInclude.Length; i++)
                    {
                        int col = i + 1;
                        var displayName = membersToInclude[i].GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                        worksheet.Cells[1, col].Value = displayName ?? membersToInclude[i].Name;
                    }
                    //style header
                    var lastColumn = ExcelCellAddress.GetColumnLetter(membersToInclude.Length);
                    using (var range = worksheet.Cells[$"A1:{lastColumn}1"])
                    {
                        // Set PatternType
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        // Set Color For Background
                        var color = ColorTranslator.FromHtml("#4AA2D4");
                        range.Style.Fill.BackgroundColor.SetColor(color);
                        // Align center
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //Set Height
                        worksheet.Row(1).Height = 33;
                        // Set Font color
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                    //Load data to body
                    worksheet.Cells[2, 1].LoadFromCollection(collection, false, TableStyles.None, BindingFlags.Instance | BindingFlags.Public, membersToInclude);

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    excel.Save();
                    var stream = excel.Stream;
                    return stream;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
