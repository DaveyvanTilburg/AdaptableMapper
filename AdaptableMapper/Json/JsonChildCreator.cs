using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public class JsonChildCreator : ChildCreator
    {
        public object CreateChildOn(object parent, object template)
        {
            if (!(parent is JToken jToken))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type jToken");
                return new JObject();
            }

            if (!(template is JToken jTemplate))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type jToken");
                return new JObject();
            }

            var jTemplateCopy = new JObject(jTemplate);
            jToken.AddAfterSelf(jTemplateCopy);

            return jTemplateCopy;
        }
    }
}