using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHarbor.Domain.Exceptions;
public class StockHarborException(string message) : Exception(message)
{
}
