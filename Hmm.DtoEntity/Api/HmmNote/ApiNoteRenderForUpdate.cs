namespace Hmm.DtoEntity.Api.HmmNote
{
    public class ApiNoteRenderForUpdate : ApiEntity
    {
        public string Name { get; set; }

        public string Namespace { get; set; }

        public bool IsDefault { get; set; }

        public string Description { get; set; }
    }
}