using SCI_PreAssessment_FindChips.Classes;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var scraper = new FindChipsScraper();
        var offers = await scraper.ScrapeAsync("2N222");

        if (offers.Count == 0)
        {
            Console.WriteLine("No offers found.");
            return;
        }

        string filePath = "./2N222_Offers.xlsx";
        ExcelExporter.Export(offers, filePath);

        Console.WriteLine($"Data exported to {filePath}");
    }
}
