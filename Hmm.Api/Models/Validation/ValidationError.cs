using Newtonsoft.Json;

namespace Hmm.Api.Models.Validation
{
    public class ValidationError
    {
        public ValidationError(string field, string message)
        {
            Field = string.IsNullOrEmpty(field) ? null : field;
            Message = message;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }
    }
}