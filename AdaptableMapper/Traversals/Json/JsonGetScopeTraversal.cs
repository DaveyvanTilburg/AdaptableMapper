using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Traversals.Json
{
    public sealed class JsonGetScopeTraversal : GetScopeTraversal
    {
        public JsonGetScopeTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public MethodResult<IEnumerable<object>> GetScope(object source)
        {
            if (!(source is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#3; Source is not of expected type jToken", "error", Path, source?.GetType().Name);
                return new NullMethodResult<IEnumerable<object>>();
            }

            IEnumerable<JToken> jTokens = jToken.TraverseAll(Path).ToList();
            if (!jTokens.Any())
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#4; Path has no results", "warning", Path);
                return new NullMethodResult<IEnumerable<object>>();
            }

            return new MethodResult<IEnumerable<object>>(jTokens);
        }
    }
}