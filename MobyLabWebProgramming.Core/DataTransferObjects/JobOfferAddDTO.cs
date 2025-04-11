using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class JobOfferCreateDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Salary { get; set; }
    }
}
