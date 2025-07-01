using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHarbor.Domain.Exceptions;
public class NotFoundException : StockHarborException
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string name,int id) : base($"{name} with Id {id} was not found.") { }
    public NotFoundException(string message, Exception innerException)
    : base(message, innerException) { }
}
