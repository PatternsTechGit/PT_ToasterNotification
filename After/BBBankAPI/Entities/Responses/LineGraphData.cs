using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Responses
{
    public class LineGraphData
    {
        public LineGraphData()
        {
            this.Labels = new List<string>();   
            this.Figures = new List<decimal>();  
        }
        public decimal TotalBalance { get; set; }
        public ICollection<string> Labels { get; set; }
        public ICollection<decimal> Figures { get; set; }
    }
}
