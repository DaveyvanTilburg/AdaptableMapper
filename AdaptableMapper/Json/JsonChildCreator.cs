using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public class JsonChildCreator : ChildCreator
    {
        public object CreateChildOn(object parent, object template)
        {
            if (!(parent is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#1; Parent is not of expected type jToken", "error", parent?.GetType().Name);
                return new JObject();
            }

            if (!(template is JToken jTemplate))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#2; Template is not of expected type jToken", "error", template?.GetType().Name);
                return new JObject();
            }

            var jTemplateCopy = new JObject(jTemplate);
            jToken.AddAfterSelf(jTemplateCopy);

            return jTemplateCopy;
        }
    }
}