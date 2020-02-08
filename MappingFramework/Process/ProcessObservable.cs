using System.Collections.Generic;
using System.Linq;

namespace MappingFramework.Process
{
    public sealed partial class ProcessObservable
    {
        private ProcessObservable() 
        {
            _observers = new List<ProcessObserver>();
        }

        private readonly List<ProcessObserver> _observers;

        public void Register(ProcessObserver errorObserver)
        {
            _observers.Add(errorObserver);
        }

        public void Unregister(ProcessObserver errorObserver)
        {
            _observers.Remove(errorObserver);
        }

        public void Raise(string message, string type, params object[] additionalInfo)
        {
            if (_observers.Any())
            {
                var informationMessage = message;

                var additionalInfoMessage = Newtonsoft.Json.JsonConvert.SerializeObject(additionalInfo);
                if (additionalInfoMessage != "[]")
                    informationMessage += $"; objects:{additionalInfoMessage}";

                var information = new Information(informationMessage, type);
                _observers.ForEach(o => o?.InformationRaised(information));
            }
        }
    }
}