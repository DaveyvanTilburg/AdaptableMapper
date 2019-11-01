using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Errors
{
    public sealed partial class ErrorObservable
    {
        private ErrorObservable() 
        {
            _observers = new List<ErrorObserver>();
            _observersVerbose = new List<ErrorObserver>();
        }

        private List<ErrorObserver> _observers;
        private List<ErrorObserver> _observersVerbose;

        public void Register(ErrorObserver errorObserver)
        {
            _observers.Add(errorObserver);
        }

        public void Unregister(ErrorObserver errorObserver)
        {
            _observers.Remove(errorObserver);
        }

        public void RegisterVerbose(ErrorObserver errorObserver)
        {
            _observersVerbose.Add(errorObserver);
        }

        public void UnregisterVerbose(ErrorObserver errorObserver)
        {
            _observersVerbose.Remove(errorObserver);
        }

        public void Raise(string message, params object[] additionalInfo)
        {
            if (_observers.Any())
            {
                var error = new Error($"{message};");
                _observers.ForEach(o => o?.ErrorOccured(error));
            }
            
            if(_observersVerbose.Any())
            {
                var additionalInfoMessage = Newtonsoft.Json.JsonConvert.SerializeObject(additionalInfo);

                var errorVerbose = new Error($"{message}; objects:{additionalInfoMessage}");
                _observersVerbose.ForEach(o => o?.ErrorOccured(errorVerbose));
            }
        }
    }
}