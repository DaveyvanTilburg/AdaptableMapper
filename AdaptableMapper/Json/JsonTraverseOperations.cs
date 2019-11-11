using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Json
{
    internal static class JsonTraverseOperations
    {
        public static JToken Traverse(this JToken jToken, string path)
        {
            PathContainer pathContainer = PathContainer.Create(path);
            Queue<string> pathQueue = pathContainer.CreatePathQueue();

            JToken traversedParent = jToken.TraverseToParent(pathQueue);
            JToken result = traversedParent.TryTraverse(pathContainer.LastInPath);

            if (result == null)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#14; Path resulted in no items", "warning", path);
                return new JObject();
            }

            return result;
        }

        private static JToken TryTraverse(this JToken jToken, string path)
        {
            try
            {
                return jToken.SelectToken(path);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#28; Path resulted in no items", "warning", path, exception.GetType().Name, exception.Message);
                return new JObject();
            }
        }

        private static JToken TraverseToParent(this JToken jToken, Queue<string> path)
        {
            if (path.Count == 0)
                return jToken;

            string step = path.Dequeue();

            if(!step.Equals(".."))
            {
                Process.ProcessObservable.GetInstance().Raise($"JSON#15; In JPath, the / operator can only be used to navigate back to parent nodes, expected '..' but was '{step}'", "error");
                return new JObject();
            }

            JToken parent = jToken.Parent;
            if(parent == null)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#16; No parent found for jToken", "warning", jToken);
                return new JObject();
            }

            if (path.Count > 0)
                return parent.TraverseToParent(path);

            return parent;
        }

        public static IEnumerable<JToken> TraverseAll(this JToken jToken, string path)
        {
            PathContainer pathContainer = PathContainer.Create(path);
            Queue<string> pathQueue = pathContainer.CreatePathQueue();

            JToken traversedParent = jToken.TraverseToParent(pathQueue);
            IEnumerable<JToken> result = traversedParent.TryTraverseAll(pathContainer.LastInPath);

            return result;
        }

        private static IEnumerable<JToken> TryTraverseAll(this JToken jToken, string path)
        {
            try
            {
                return jToken.SelectTokens(path).ToList();
            }
            catch (Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#29; Path resulted in no items", "warning", path, exception.GetType().Name, exception.Message);
                return new JObject();
            }
        }
    }
}