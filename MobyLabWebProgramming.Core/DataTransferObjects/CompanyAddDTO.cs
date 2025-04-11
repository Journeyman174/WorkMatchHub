using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class CompanyAddDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Location { get; set; }
    }
}
