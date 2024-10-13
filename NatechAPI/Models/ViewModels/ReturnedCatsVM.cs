namespace NatechAPI.Models.ViewModels
{
    public class ReturnedCatsVM
    {
        public string CatId { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Image { get; set; }
        public string Tags { get; set; }
    }
}
