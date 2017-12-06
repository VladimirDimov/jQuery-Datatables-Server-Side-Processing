namespace Examples.Mvc.ViewModels
{
    public class CustomerViewModel
    {
        public string AccountNumber { get; set; }

        public int CustomerID { get; set; }

        public PersonViewModel Person { get; set; }

        public StoreViewModel Store { get; set; }
    }
}