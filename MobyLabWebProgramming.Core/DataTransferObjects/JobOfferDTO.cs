using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class JobOfferDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Salary { get; set; }

        public CompanyDTO Company { get; set; } = null!;
        public UserDTO Recruiter { get; set; } = null!;
        public UserDTO User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
