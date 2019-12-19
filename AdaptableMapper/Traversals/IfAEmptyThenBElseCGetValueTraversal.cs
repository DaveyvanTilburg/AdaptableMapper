using AdaptableMapper.Configuration;

namespace AdaptableMapper.Traversals
{
    public class IfAEmptyThenBElseCGetValueTraversal : GetValueTraversal
    {
        public IfAEmptyThenBElseCGetValueTraversal(GetValueTraversal getValueTraversalA, GetValueTraversal getValueTraversalB, GetValueTraversal getValueTraversalC)
        {
            GetValueTraversalA = getValueTraversalA;
            GetValueTraversalB = getValueTraversalB;
            GetValueTraversalC = getValueTraversalC;
        }

        public GetValueTraversal GetValueTraversalA { get; set; }
        public GetValueTraversal GetValueTraversalB { get; set; }
        public GetValueTraversal GetValueTraversalC { get; set; }

        public string GetValue(Context context)
        {
            if (!Validate())
                return string.Empty;

            var valueA = GetValueTraversalA.GetValue(context);

            if (string.IsNullOrWhiteSpace(valueA))
                return GetValueTraversalB.GetValue(context);

            return GetValueTraversalC.GetValue(context);
        }

        private bool Validate()
        {
            bool result = true;

            if (GetValueTraversalA == null)
            {
                Process.ProcessObservable.GetInstance().Raise("IfAEmptyThenBElseCGetValueTraversal#1; GetValueTraversalA cannot be null", "error");
                result = false;
            }

            if (GetValueTraversalB == null)
            {
                Process.ProcessObservable.GetInstance().Raise("IfAEmptyThenBElseCGetValueTraversal#2; GetValueTraversalB cannot be null", "error");
                result = false;
            }

            if (GetValueTraversalC == null)
            {
                Process.ProcessObservable.GetInstance().Raise("IfAEmptyThenBElseCGetValueTraversal#3; GetValueTraversalC cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}