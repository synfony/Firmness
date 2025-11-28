using System;
using System.Collections.Generic;
using Firmness.Core.Models;

namespace Firmness.Core.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public List<SaleDetail> SaleDetails { get; set; } = new();
        public string? ReceiptUrl { get; set; }
    }
}
