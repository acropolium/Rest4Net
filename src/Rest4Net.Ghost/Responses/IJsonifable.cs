using Newtonsoft.Json.Linq;

namespace Rest4Net.Ghost.Responses
{
    public interface IJsonifable
    {
        JObject ToJson();
    }
}
