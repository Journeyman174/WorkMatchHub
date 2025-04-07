using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class JobOfferUpdateDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Salary { get; set; }
    }
}
