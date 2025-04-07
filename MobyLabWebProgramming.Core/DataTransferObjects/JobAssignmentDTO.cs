using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class JobAssignmentDTO
    {
        public Guid Id { get; set; }
        public JobRequestDTO JobRequest { get; set; } = null!;
        public JobOfferDTO JobOffer { get; set; } = null!;
        public DateTime AssignedAt { get; set; }
        public UserDTO User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
