﻿using AdaptableMapper.Traversals;

namespace AdaptableMapper.Xml
{
    public sealed class XmlTraversalThis : Traversal
    {
        public object Traverse(object target)
        {
            return target;
        }
    }
}