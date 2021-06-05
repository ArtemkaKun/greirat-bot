using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace greirat
{
    [Table("Orders")]
    public class OrderData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int OrderID { get; private set; }
        public string OrderDate { get; private set; }
        public string PersonName { get; private set; }
        public string OrderText { get; set; }

        public OrderData (string orderDate, string personName, string orderText)
        {
            OrderDate = orderDate;
            PersonName = personName;
            OrderText = orderText;
        }
    }
}