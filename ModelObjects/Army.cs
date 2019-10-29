﻿using AdaptableMapper.Model.Language;
using System.Collections.Generic;

namespace AdaptableObjects
{
    public class Army : ModelBase
    {
        public List<Platoon> Platoons { get; set; } = new List<Platoon>();
        public string Code { get; set; } = string.Empty;
    }
}