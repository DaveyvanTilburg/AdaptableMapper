using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using MappingFramework.Configuration;
using MappingFramework.Languages.Dictionary;
using MappingFramework.Languages.Dictionary.Configuration;
using MappingFramework.Languages.Dictionary.Traversals;
using MappingFramework.Languages.Xml.Configuration;
using MappingFramework.Languages.Xml.Traversals;
using Xunit;

namespace MappingFramework.UnitTests
{
    public class XmlToDictionary
    {
        [Fact]
        public void JsonToDictionaryFullStory()
        {
            var mappingConfiguration = new MappingConfiguration(
                new List<MappingScopeComposite>{
                    new(
                        new List<MappingScopeComposite>{
                            new(
                                new List<MappingScopeComposite>{
                                    new(
                                        new List<MappingScopeComposite>(),
                                        new List<Mapping>{
                                            new(
                                                new XmlGetThisValueTraversal(),
                                                new DictionarySetValueTraversal(
                                                    new XmlGetValueTraversal("./@dow")
                                                )
                                            )
                                        },
                                        new NullObject(),
                                        new XmlGetListValueTraversal("./day"),
                                        new DictionaryGetTemplateTraversal(),
                                        new DictionaryChildToParent()
                                    )
                                },
                                new List<Mapping>(),
                                new NullObject(),
                                new XmlGetListValueTraversal("./week"),
                                new DictionaryGetTemplateTraversal(),
                                new DictionaryChildCreator(
                                    new XmlGetValueTraversal("./@weekNr")
                                )
                            )
                        },
                        new List<Mapping>(),
                        new NullObject(),
                        new XmlGetListValueTraversal("//month"),
                        new DictionaryGetTemplateTraversal(),
                        new DictionaryChildCreator(
                            new XmlGetValueTraversal("./@name")
                        )
                    )
                },
                new ContextFactory(
                    new XmlSourceCreator(),
                    new DictionaryTargetCreator()
                ),
                new NullObject()
            );

            string source = File.ReadAllText("./Resources/XMLSource_Calendar.xml");
            MapResult mapResult = mappingConfiguration.Map(source, null);
            var result = mapResult.Result as EasyAccessDictionary;
            
            result.Keys.Count.Should().Be(3);
            ((Dictionary<string, object>)result["Januari"]).Keys.Count.Should().Be(2);
            ((Dictionary<string, object>)result["Februari"]).Keys.Count.Should().Be(2);
            ((Dictionary<string, object>)result["March"]).Keys.Count.Should().Be(1);

            ((Dictionary<string, object>)((Dictionary<string, object>)result["Januari"])["1"]).Keys.Count.Should().Be(7);
            ((Dictionary<string, object>)((Dictionary<string, object>)result["Januari"])["2"]).Keys.Count.Should().Be(7);
            ((Dictionary<string, object>)((Dictionary<string, object>)result["Februari"])["5"]).Keys.Count.Should().Be(5);
            ((Dictionary<string, object>)((Dictionary<string, object>)result["Februari"])["6"]).Keys.Count.Should().Be(5);
            ((Dictionary<string, object>)((Dictionary<string, object>)result["March"])["7"]).Keys.Count.Should().Be(5);

            ((Dictionary<string, object>)((Dictionary<string, object>)result["Januari"])["1"])["monday"].Should().Be("Available");
            ((Dictionary<string, object>)((Dictionary<string, object>)result["Januari"])["1"])["saturday"].Should().Be("Not Available");

            ((Dictionary<string, object>)((Dictionary<string, object>)result["March"])["7"]).ContainsKey("saturday").Should().BeFalse();
        }
    }
}