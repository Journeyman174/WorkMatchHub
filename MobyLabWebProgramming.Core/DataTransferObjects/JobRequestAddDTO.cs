using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class JobRequestAddDTO
    {
        public Guid JobOfferId { get; set; }
        public Guid JobSeekerId { get; set; }
        public string CoverLetter { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
