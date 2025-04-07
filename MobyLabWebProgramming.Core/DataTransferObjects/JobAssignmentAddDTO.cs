using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class JobAssignmentAddDTO
    {
        public Guid JobRequestId { get; set; }
        public Guid JobOfferId { get; set; }
        public DateTime AssignedAt { get; set; }
        public Guid UserId { get; set; }
    }
}
