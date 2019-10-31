using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Json
{
    internal static class JsonPathParentOperation
    {
        public static JToken Traverse(this JToken jToken, string path)
        {
            PathContainer pathContainer = PathContainer.Create(path);
            Queue<string> pathQueue = pathContainer.CreatePathQueue();

            JToken traversedParent = jToken.TraverseToParent(pathQueue);
            JToken result = traversedParent.SelectToken(pathContainer.LastInPath);

            if (result == null)
            {
                Errors.ErrorObservable.GetInstance().Raise($"Path {path} resulted in no jTokens");
                return string.Empty;
            }

            return result;
        }

        private static JToken TraverseToParent(this JToken jToken, Queue<string> path)
        {
            if (path.Count == 0)
                return jToken;

            string step = path.Dequeue();

            if(!step.Equals(".."))
                Errors.ErrorObservable.GetInstance().Raise($"In JPath, the / operator can only be used to navigate back to parent nodes, expected '..' but was '{step}'");

            JToken parent = jToken.Parent;
            if(parent == null)
                Errors.ErrorObservable.GetInstance().Raise($"No parent found for jToken {jToken}");

            if (path.Count > 0)
                return parent.TraverseToParent(path);
            else
                return parent;
        }

        public static IEnumerable<JToken> TraverseAll(this JToken jToken, string path)
        {
            PathContainer pathContainer = PathContainer.Create(path);

            Queue<string> pathQueue = pathContainer.CreatePathQueue();

            JToken traversedParent = jToken.TraverseToParent(pathQueue);

            IEnumerable<JToken> result = traversedParent.SelectTokens(pathContainer.LastInPath).ToList();
            return result;
        }
    }
}