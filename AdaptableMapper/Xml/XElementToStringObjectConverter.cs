﻿using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XElementToStringObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#9; source is not of expected type XElement", "error", source?.GetType().Name);
                return string.Empty;
            }

            return xElement.ToString();
        }
    }
}