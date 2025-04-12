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
        public DateTime CreatedAt { get; set; }
    }
}
