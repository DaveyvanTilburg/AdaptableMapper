﻿using AdaptableMapper.Model.Language;
using System.Collections.Generic;

namespace AdaptableObjects
{
    public class Root : ModelBase
    {
        public List<Army> Armies { get; set; } = new List<Army>();
    }
}