namespace Hmm.DtoEntity.Api.HmmNote
{
    public class ApiAuthor : ApiEntity
    {
        public string AccountName { get; set; }

        public string Role { get; set; }

        public bool IsActivated { get; set; }

        public string Description { get; set; }
    }
}