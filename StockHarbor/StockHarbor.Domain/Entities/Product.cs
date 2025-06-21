using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHarbor.Domain.Entities;
public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    public ICollection<ProductVariant> Variants { get; set; } = [];
}
