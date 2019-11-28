using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Configuration.Json
{
    public class JsonChildCreator : ChildCreator
    {
        public object CreateChild(Template template)
        {
            if (!(template.Parent is JArray jArray))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#1; Parent is not of expected type jArray", "error", template.Parent?.GetType().Name);
                return new JObject();
            }

            if (!(template.Child is JToken jTemplate))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#2; Template is not of expected type jToken", "error", template.Child?.GetType().Name);
                return new JObject();
            }

            var jTemplateCopy = jTemplate.DeepClone();
            jArray.Add(jTemplateCopy);

            return jTemplateCopy;
        }
    }
}