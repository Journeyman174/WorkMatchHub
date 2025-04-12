namespace MobyLabWebProgramming.Core.DataTransferObjects
{
    public class SavedJobDTO
    {
        public JobOfferDTO JobOffer { get; set; } = null!;
        public UserDTO User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
