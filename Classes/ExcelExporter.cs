using ClosedXML.Excel;

namespace SCI_PreAssessment_FindChips.Classes
{
    public static class ExcelExporter
    {
        public static void Export(List<DistributorOffer> offers, string filePath)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Offers");

            worksheet.Cell(1, 1).InsertTable(offers);

            workbook.SaveAs(filePath);
        }
    }
}
