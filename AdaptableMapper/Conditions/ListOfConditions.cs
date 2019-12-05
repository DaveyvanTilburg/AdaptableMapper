using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Conditions
{
    public class ListOfConditions : Condition
    {
        public List<Condition> Conditions { get; set; }

        public ListOfConditions()
            => Conditions = new List<Condition>();

        public bool Validate(object source)
        {
            if (!Validate())
                return false;

            bool result = false;
            foreach (Condition condition in Conditions)
                result = condition.Validate(result);

            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if ((Conditions?.Any() ?? false) == false)
            {
                Process.ProcessObservable.GetInstance().Raise($"ListOfConditions#1; {nameof(Conditions)} is empty", "error");
                result = false;
            }

            return result;
        }
    }
}