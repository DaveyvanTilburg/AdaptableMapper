using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public class JsonChildCreator : ChildCreator
    {
        public object CreateChildOn(object parent, object template)
        {
            if (!(parent is JArray jArray))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#1; Parent is not of expected type jArray", "error", parent?.GetType().Name);
                return new JObject();
            }

            if (!(template is JToken jTemplate))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#2; Template is not of expected type jToken", "error", template?.GetType().Name);
                return new JObject();
            }

            var jTemplateCopy = jTemplate.DeepClone();
            jArray.Add(jTemplateCopy);

            return jTemplateCopy;
        }
    }
}