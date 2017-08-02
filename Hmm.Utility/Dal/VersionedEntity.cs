namespace Hmm.Utility.Dal
{
    public class VersionedEntity : Entity
    {
        public byte[] Version { get; set; }
    }
}