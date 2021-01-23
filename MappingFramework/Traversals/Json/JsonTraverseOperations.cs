using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using MappingFramework.Configuration;

namespace MappingFramework.Traversals.Json
{
    public static class JsonTraverseOperations
    {
        public static JToken Traverse(this JToken jToken, string path, Context context)
        {
            PathContainer pathContainer = PathContainer.Create(path);
            Queue<string> pathQueue = pathContainer.CreatePathQueue();

            JToken traversedParent = jToken.TraverseToParent(pathQueue, context);
            JToken result = traversedParent.TryTraverse(pathContainer.LastInPath, context);

            if (result == null)
                return CreateNullToken();

            return result;
        }

        private static JToken TryTraverse(this JToken jToken, string path, Context context)
        {
            try
            {
                return jToken.SelectToken(path);
            }
            catch (Exception exception)
            {
                context.NavigationException(path, exception);
                return CreateNullToken();
            }
        }

        public static MethodResult<string> TryTraversalGetValue(this JToken jToken, string path, Context context)
        {
            JToken pathResult = jToken.Traverse(path, context);

            try
            {
                return new MethodResult<string>(pathResult.Value<string>());
            }
            catch
            {
                context.NavigationResultIsEmpty(path);
                return new NullMethodResult<string>();
            }
        }

        private static JToken TraverseToParent(this JToken jToken, Queue<string> path, Context context)
        {
            if (path.Count == 0)
                return jToken;

            string step = path.Dequeue();

            if(!step.Equals(".."))
            {
                context.NavigationInvalid(step, "In JPath, the / operator can only be used to navigate back to parent nodes, expected '..'");
                return CreateNullToken();
            }

            JToken parent = jToken.Parent;
            if(parent == null)
                return CreateNullToken();

            if (path.Count > 0)
                return parent.TraverseToParent(path, context);

            return parent;
        }

        public static IEnumerable<JToken> TraverseAll(this JToken jToken, string path, Context context)
        {
            PathContainer pathContainer = PathContainer.Create(path);
            Queue<string> pathQueue = pathContainer.CreatePathQueue();

            JToken traversedParent = jToken.TraverseToParent(pathQueue, context);
            IEnumerable<JToken> result = traversedParent.TryTraverseAll(pathContainer.LastInPath, context);

            return result;
        }

        private static IEnumerable<JToken> TryTraverseAll(this JToken jToken, string path, Context context)
        {
            try
            {
                return jToken.SelectTokens(path).ToList();
            }
            catch
            {
                context.NavigationResultIsEmpty(path);
                return new List<JToken> { CreateNullToken() };
            }
        }

        private static JToken CreateNullToken()
            => new NullToken();
    }
}