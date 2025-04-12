namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class JobOfferAddDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Salary { get; set; }
        public Guid CompanyId { get; set; } 
    }
}
