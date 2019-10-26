using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptableMapper.Traversals.AdaptableTraversals
{
    public class AdaptableCreateNewChild : CreateNewChild
    {
        private IList _parent;

        protected override object DuplicateTemplate(object template)
        {
            if (!(template is Type listType))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Adaptable");
                return string.Empty;
            }

            Adaptable newEntry = listType.CreateAdaptable();
            newEntry.SetParent()
        }

        protected override object GetTemplate(object target)
        {
            if (!(target is IList parent))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Adaptable");
                return string.Empty;
            }

            _parent = parent;

            return _parent.GetType();
        }
    }
}