using HtmlAgilityPack;
using System.Net.Http;

namespace SCI_PreAssessment_FindChips.Classes
{
    public class FindChipsScraper
    {
        private readonly HttpClient _httpClient;

        public FindChipsScraper()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
        }

        public async Task<List<DistributorOffer>> ScrapeAsync(string partNumber)
        {
            var offers = new List<DistributorOffer>();
            string url = $"https://www.findchips.com/search?q={partNumber}";
            var html = await _httpClient.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // TODO: Adjust based on actual HTML structure
            //var distNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'distributor-container')]")?.Take(5);
            var distNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'distributor-box')]")?.Take(5);

            if (distNodes == null) return offers;

            foreach (var dist in distNodes)
            {
                var distributorName = dist.SelectSingleNode(".//img")?.GetAttributeValue("alt", "")?.Replace(" logo", "").Trim() ?? "Unknown";
                var offerNodes = dist.SelectNodes(".//tr[contains(@class, 'offer-row')]")?.Take(5);

                if (offerNodes == null) continue;

                foreach (var offer in offerNodes)
                {
                    var moq = offer.SelectSingleNode(".//td[contains(@class, 'moq')]")?.InnerText.Trim() ?? "";
                    var spq = offer.SelectSingleNode(".//td[contains(@class, 'spq')]")?.InnerText.Trim() ?? "";
                    var unitPrice = offer.SelectSingleNode(".//td[contains(@class, 'price')]")?.InnerText.Trim() ?? "";
                    var currency = "$"; // Assume USD, or parse from price string if needed
                    var sellerName = offer.SelectSingleNode(".//td[contains(@class, 'seller')]")?.InnerText.Trim() ?? "";
                    var offerUrl = "https://www.findchips.com" + (offer.SelectSingleNode(".//a")?.GetAttributeValue("href", "") ?? "");

                    offers.Add(new DistributorOffer
                    {
                        DistributorName = distributorName,
                        SellerName = sellerName,
                        MOQ = moq,
                        SPQ = spq,
                        UnitPrice = unitPrice,
                        Currency = currency,
                        OfferURL = offerUrl,
                        Timestamp = DateTime.Now
                    });
                }
            }

            return offers;
        }
    }
}
