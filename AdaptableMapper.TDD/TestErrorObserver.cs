using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Errors;

namespace AdaptableMapper.TDD
{
    internal class TestErrorObserver : ProcessObserver
    {
        private readonly List<Information> _information = new List<Information>();

        public IReadOnlyCollection<Information> GetRaisedWarnings()
        {
            return _information.Where(i => i.Type.Equals("warning")).ToList();
        }

        public IReadOnlyCollection<Information> GetRaisedErrors()
        {
            return _information.Where(i => i.Type.Equals("error")).ToList();
        }

        public IReadOnlyCollection<Information> GetRaisedOtherTypes()
        {
            return _information.Where(i => !i.Type.Equals("error") && !i.Type.Equals("warning")).ToList();
        }

        public void InformationRaised(Information information)
        {
            _information.Add(information);
        }
    }
}