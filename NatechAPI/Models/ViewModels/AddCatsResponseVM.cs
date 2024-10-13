namespace NatechAPI.Models.ViewModels
{
    public class AddCatsResponseVM
    {
        public string status { get; set; }
        public int addedCatsNum { get; set; }
        public HashSet<string> addedCats { get; set; }
    }
}
