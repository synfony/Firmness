using System;
using System.Collections.Generic;

namespace Firmness.ViewModels
{
    public class SaleDto
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int ClientId { get; set; }
        public required string ClientName { get; set; }
        public required List<SaleDetailDto> SaleDetails { get; set; }
        public decimal Total { get; set; }
    }

    public class SaleDetailDto
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
