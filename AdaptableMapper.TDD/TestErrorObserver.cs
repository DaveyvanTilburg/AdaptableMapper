using System.Collections.Generic;
using AdaptableMapper.Errors;

namespace AdaptableMapper.TDD
{
    internal class TestErrorObserver : ErrorObserver
    {
        private readonly List<string> _errors = new List<string>();

        public IReadOnlyCollection<string> GetErrors()
        {
            return _errors;
        }

        public void ErrorOccured(Error error)
        {
            _errors.Add(error.Message);
        }
    }
}