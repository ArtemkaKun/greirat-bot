namespace greirat
{
    public class OrderData
    {
        public string PersonName { get; private set; }
        public string OrderText { get; private set; }

        public OrderData (string personName, string orderText)
        {
            PersonName = personName;
            OrderText = orderText;
        }
    }
}