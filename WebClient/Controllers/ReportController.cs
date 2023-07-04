using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using WebClient.ViewModels;

namespace WebClient.Controllers
{
    public class ReportController : Controller
    {
        private readonly HttpClient client;
        private string OrderDetailApiUrl = "";
        public ReportController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderDetailApiUrl = "https://localhost:7022/api/OrderDetail";
        }
        public async Task<IActionResult> ReportFilter(DateTime startDate, DateTime endDate)
        {
            HttpResponseMessage response = await client.GetAsync(OrderDetailApiUrl);

            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<SalesReportViewModel> salesReportViewModels = JsonSerializer.Deserialize<List<SalesReportViewModel>>(strData, options);
            var report = salesReportViewModels.Where(s => s.Order.Status != BusinessObject.Enums.OrderStatus.Pending && s.Order.Status != BusinessObject.Enums.OrderStatus.Processing).ToList();
            if (startDate == default(DateTime) && endDate == default(DateTime))
            {
                return RedirectToAction("Report_Index", "Staff");
            }
            var onlyDateStart = startDate.Date.ToString("dd/MM/yyyy");
            var onlyDateEnd = endDate.Date.ToString("dd/MM/yyyy");
            TempData["period"] = $"Report from {onlyDateStart} to {onlyDateEnd}";
            decimal totalRevenue = 0;
            var reportFiltered = report.Where(o => o.Order.ShipDate.Value.Date >= startDate && o.Order.ShipDate.Value.Date <= endDate).ToList();
            foreach (var re in reportFiltered)
            {

                if (re.Order.Status == BusinessObject.Enums.OrderStatus.Completed)
                {
                    totalRevenue += re.OrderDetail.Quantity * re.OrderDetail.UnitPrice;
                }

            };
            TempData["tottalRevenue"] = $"Total revenue: ${totalRevenue.ToString("0.00")}";
            return View(reportFiltered);
        }
    }
}
