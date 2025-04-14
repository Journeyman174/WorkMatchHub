namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class SavedJobDTO
    {
        public Guid Id { get; set; }
        public JobOfferDTO JobOffer { get; set; } = default!;
        public UserDTO User { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }

}
