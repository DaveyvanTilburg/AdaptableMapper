using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonGetScope : GetScopeTraversal
    {
        public JsonGetScope(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public IEnumerable<object> GetScope(object source)
        {
            if (!(source is JToken jToken))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type JToken");
                return new JObject();
            }

            IEnumerable<JToken> jTokens = jToken.SelectTokens(Path).ToList();

            if(!jTokens.Any())
                Errors.ErrorObservable.GetInstance().Raise("Json GetScope resulted in no tokens");

            return jTokens;
        }
    }
}