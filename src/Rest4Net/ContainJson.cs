using Newtonsoft.Json.Linq;

namespace Rest4Net
{
    public class ContainJson : IContainJson
    {
        public JToken Json { get; internal set; }
    }
}
