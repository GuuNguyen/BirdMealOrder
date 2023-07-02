namespace WebClient.Models
{
    public class City
    {
        public string? Name { get; set; }
        public string? Codename { get; set; }
        public List<District>? Districts { get; set; }
    }
}
