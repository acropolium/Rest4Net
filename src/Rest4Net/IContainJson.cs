using Newtonsoft.Json.Linq;

namespace Rest4Net
{
    public interface IContainJson
    {
        JToken Json { get; }
    }
}
