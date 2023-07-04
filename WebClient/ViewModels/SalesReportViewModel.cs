using BusinessObject.Enums;
using BusinessObject.Models;

namespace WebClient.ViewModels
{
    public class SalesReportViewModel
    {
        public OrderDetail OrderDetail { get; set; }
        public string UserName { get; set; }
        public Order Order { get; set; }
    }
}
