!! I don't know if this should be categorized as mapping or serialization... any thoughts?

# XPathSerialization

A package for serializing string objects to memory objects via configuration

## Plans

Completed plans:
 - Xml to memory objects
 - Soft error handling (through observer)
 - Write documentation

Future plans in relative priority order:
 - Json to memory objects
 - Xml directly to xml
 - Json directly to json
 - Xml directly to json and vice versa
 
Maybe plans:
  - Maybe change the way XPathConfigurations are constructed
    - I am unhappy about the fact that XPathConfiguration has a string property type that says what it does, and based on the type it does not use parts of itself. Like the type Map only uses XPath and AdaptablePath. SearchPath uses ontop of what Map uses a SearchPath. Scope is the only one that uses xPathConfigurations
    - But the upside of this approach is that is leaves both XPathTransformation and XPathConfiguration open to extension. So its a maybe because i have to find an alternative that does the same but better expresses its design

## How to use

### Create your own memory objects to map to
To do this, simply create a object hierarchy that all inherit the adaptable abstraction from the package.

Currently, since there is no implementation (nor plans so far) for conversion of data during the serialization all the expected types are IList<adaptable> for traversion and strings for values.

Example:
```
public class Hardware : Adaptable
{
    public string Title { get; set; } = string.Empty;
    public List<Component> Components { get; set; } = New List<Component>();
}

public class Component : Adaptable
{
    public string Title { get; set; } = string.Empty;
    public string Width { get; set; } = string.Empty;
    public string Height { get; set; } = string.Empty;
}
```

The other part of the setup (for which this package is really meant) is the hierarchie of configuration on how to travers and map the values of the Xml.

Currently there are 3 ways to configure:
  - Scope
  - Map
  - Search

### Scope 
As a way to traverse both the Xml (or future different formats like Json and maybe yamll) and the in memory object at the same time.
So for each result of XPath result, an Adaptable path result will be created. (so lets say like the example above where hardware has multiple components, if the xpath result hits 3 XElements in the Xml, it will create 3 components.

The flow of this will be:
  - Find XPath
  - Iterate through results
    - Create an Adaptable object (It is expected at this point that the adaptable path leads to a IList<Adaptable>)
    - Iterate through my children XPathConfigurations (which can also be a scope, and then the story starts at the top)
    
### Map
A simple, get result from XPath and place it in result of AdaptablePath (it is expected that this is a string)

### Search
A way to navigate outside of the scope while being able to modify the XPath with a value from within the scope.
So, the SearchPath will be done first, the result of which will be replaced in the XPath, expecting a {{searchResult}}
Then it pretty much does the same thing as Map

### Search on Object (so when you are deserialization)
Since Adaptable path is not using XPath, i had to create something of my own to facilitate searching over in memory objects.

The pattern that can be used in the XPath is:
```
  ./Components{'PropertyName':'Title','Value':'{{searchResult}}'}/Width
```

Given that the scope has already been set to /Hardware on both XPath and AdaptablePath.
This XPath will navigate to IList<Adaptable> components and search it for an entry with Title that has the value {{searchResult}} (assuming you have something in the searchPath that results in a value, {{searchResult}} will be replaced with that value)  

## Versioning

I use [SemVer](http://semver.org/) for versioning. (But forgive me if i mess that up sometimes, just correct me on it!)

## Authors

* **Davey van Tilburg**

## License

This project is licensed under the WTFPL License - see the [License.txt](XPathSerialization/License.txt) file for details

## Acknowledgments

* Newtonsoft for json serialization
