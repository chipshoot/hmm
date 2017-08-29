namespace Hmm.Contract
{
    public interface INoteSerializer
    {
        string GetSerializationString<T>(T entity);
    }
}