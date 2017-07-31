namespace Hmm.Utility.Dal
{
    public class VersionedEntity : Entity
    {
        public virtual byte[] Version { get; set; }
    }
}