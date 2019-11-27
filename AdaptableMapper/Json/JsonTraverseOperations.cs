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
                return CreateNullToken();

            return result;
        }

        private static JToken TryTraverse(this JToken jToken, string path)
        {
            try
            {
                return jToken.SelectToken(path);
            }
            catch (Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#28; Path resulted in no items", "error", path, exception.GetType().Name, exception.Message);
                return CreateNullToken();
            }
        }

        public static MethodResult<string> TryTraversalGetValue(this JToken jToken, string path)
        {
            JToken pathResult = jToken.Traverse(path);

            try
            {
                return new MethodResult<string>(pathResult.Value<string>());
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#6; Path resulted in no items", "error", path, exception.GetType().Name, exception.Message);
                return new NullMethodResult<string>();
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
                return CreateNullToken();
            }

            JToken parent = jToken.Parent;
            if(parent == null)
                return CreateNullToken();

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
                Process.ProcessObservable.GetInstance().Raise("JSON#29; Path resulted in no items", "error", path, exception.GetType().Name, exception.Message);
                return new List<JToken> { CreateNullToken() };
            }
        }

        private static JToken CreateNullToken()
        {
            return new NullToken();
        }
    }
}