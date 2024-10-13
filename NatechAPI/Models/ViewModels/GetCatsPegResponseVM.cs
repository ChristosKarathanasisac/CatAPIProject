namespace NatechAPI.Models.ViewModels
{
    public class GetCatsPegResponseVM
    {
        public int TotalCats { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<ReturnedCatsVM> Cats { get; set; }
    }
}
