namespace MobyLabWebProgramming.Core.Entities
{
    // SavedJob reprezinta o salvare a unei oferte de job de catre un utilizator
    // Entitate de legatura pentru o relatie Many-to-Many intre User si JobOffer
    public class SavedJob : BaseEntity
    {
        // Id-ul utilizatorului care a salvat jobul - cheie straina
        // Relatie Many-to-One: un utilizator poate salva mai multe joburi
        public Guid UserId { get; set; }

        // Proprietate de navigare catre utilizatorul care a salvat oferta
        public User User { get; set; } = default!;

        // Id-ul ofertei de job salvate - cheie straina
        // Relatie Many-to-One: o oferta poate fi salvata de mai multi utilizatori
        public Guid JobOfferId { get; set; }

        // Proprietate de navigare catre oferta de job salvata
        public JobOffer JobOffer { get; set; } = default!;
    }
}