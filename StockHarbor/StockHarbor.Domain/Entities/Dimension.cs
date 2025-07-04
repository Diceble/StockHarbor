using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockHarbor.Domain.Entities;
public class Dimension
{
    public int Height { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }
    public string Unit { get; set; } = string.Empty;
}
