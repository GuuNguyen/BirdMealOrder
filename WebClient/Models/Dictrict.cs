namespace WebClient.Models
{
    public class District
    {
        public string Name { get; set; }
        public string Codename { get; set; }
        public List<Ward> Wards { get; set; }
    }

}
