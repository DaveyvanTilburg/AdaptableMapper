﻿using FluentAssertions;
using Xunit;

namespace AdaptableMapper.TDD
{
    public class ConfigurationSaveAndLoad
    {
        [Fact]
        public void CheckIfSaveAndLoadMementoWorks()
        {
            MappingConfiguration source = XmlToMemory.GetFakedSerializationConfiguration();

            string serialized = Mapper.GetMemento(source);
            MappingConfiguration target = Mapper.LoadMemento(serialized);

            source.Should().BeEquivalentTo(target);
        }
    }
}