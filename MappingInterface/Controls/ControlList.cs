using System;
using System.Collections;
using System.Collections.Generic;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Controls
{
    public class ControlList
    {
        private readonly IObjectLink _objectLink;

        private readonly EventList _eventList;
        private readonly ListOfTEntries _entries;

        public ControlList(IObjectLink objectLink, EventHandler<ListUpdatedEventArgs> subscriber)
        {
            _objectLink = objectLink;
            
            _eventList = new EventList((IList)_objectLink.Value(), subscriber);
            _entries = new ListOfTEntries(_eventList);
        }

        public IEnumerable<ListOfTEntryControl> Controls()
        {
            if (_eventList.Count() == 0)
                yield break;

            for (int index = 0; index < _eventList.Count(); index++)
            {
                ListOfTEntry tEntry = _entries.Load(index);
                ListOfTEntryControl entryControl = new ListOfTEntryControl(_objectLink, tEntry);
                yield return entryControl;
            }

            _eventList.Trigger();
        }

        public ListOfTEntryControl New()
        {
            ListOfTEntry tEntry = _entries.New();
            ListOfTEntryControl entryControl = new ListOfTEntryControl(_objectLink, tEntry);
            return entryControl;
        }
    }
}