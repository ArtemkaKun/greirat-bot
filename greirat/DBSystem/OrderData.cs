namespace greirat
{
    public class OrderData
    {
        public int OrderID { get; private set; }
        public string PersonName { get; private set; }
        public string OrderText { get; private set; }

        public OrderData (int orderId, string personName, string orderText)
        {
            OrderID = orderId;
            PersonName = personName;
            OrderText = orderText;
        }
    }
}