using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuestLibraryApplication.Models.OrderModel
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public long OrderRefID { get; set; }
        public int BookID { get; set; }
        public string BookName { get; set; }
        public int TotalBookCount { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public double TotalPrice { get; set; }
    }
}