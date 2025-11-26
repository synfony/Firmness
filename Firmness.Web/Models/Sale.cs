using System;
using System.Collections.Generic;

namespace Firmness.Web.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; } // Changed to nullable
        public List<SaleDetail> SaleDetails { get; set; } = new();
        public string? ReceiptUrl { get; set; }
    }
}
