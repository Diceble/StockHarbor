using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHarbor.Domain.Exceptions;
public class ProductNotFoundException : StockHarborException
{
    public ProductNotFoundException(string message) : base(message) { }
    public ProductNotFoundException(int productId) : base($"Product with Id {productId} was not found.")
    {
    }
}
