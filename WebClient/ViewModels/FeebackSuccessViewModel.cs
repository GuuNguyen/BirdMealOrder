namespace WebClient.ViewModels
{
    public class FeebackSuccessViewModel
    {
        public int OrderId { get; set; }
        public string MealCode { get; set; }
        public string ProductCode { get; set; }
        public int OrderDetailId { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
    }
}
