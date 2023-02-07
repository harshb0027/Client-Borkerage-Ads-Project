namespace Lab04.Models.AdsViewModels
{
    public class AdsViewModel
    {
        public Brokerage Brokerage { get; set; }

        public IEnumerable<Advertisement> Advertisements { get; set; }
    }
}
