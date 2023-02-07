namespace Lab04.Models.ViewModels
{
    public class ClientSubscriptionsViewModel
    {
        public Client Client { get; set; }

        public IEnumerable<BrokerageSubscriptionsViewModel> Subscriptions { get; set; }
    }
}
