namespace BotCommands
{
    public class SimpleOrderInfo
    {
        public int OrderID { get; }
        public string OrderMessage { get; }
        public string OrderOwner { get; private set; }

        public SimpleOrderInfo (string orderMessage)
        {
            OrderMessage = orderMessage;
        }

        public SimpleOrderInfo (int orderId)
        {
            OrderID = orderId;
        }

        public SimpleOrderInfo (int orderId, string orderMessage)
        {
            OrderID = orderId;
            OrderMessage = orderMessage;
        }

        public void SetOrderOwner (string orderOwner)
        {
            OrderOwner = orderOwner;
        }
    }
}