using System.Reflection;

namespace Application.Core.Swift.MtTags;

public class TagFactory
{
    Dictionary<string, Type> mappings;
    Dictionary<string, string> swiftTagToITagMapping;

    public TagFactory()
    {
        LoadITagDataTypes();
        LoadTagToClassMappings();
    }

    public List<ITag> CreateInstance(string parsedSwiftTag, List<ITag> listOfITags)
    {
        string tagID = parsedSwiftTag.Substring(1, 3);
        Type t = GetITagToCreate(tagID.TrimColon());

        if (t != null)
        {
            ITag t1 = Activator.CreateInstance(t) as ITag;
            t1.GetTagValues(parsedSwiftTag);
            listOfITags.Add(t1);
        }

        return listOfITags;
    }

    private Type GetITagToCreate(string iTagToInstatiate)
    {
        foreach (var tagMapping in this.swiftTagToITagMapping.OrderBy(tm => tm.Key))
        {
            if (tagMapping.Key == iTagToInstatiate)
            {
                iTagToInstatiate = this.swiftTagToITagMapping[tagMapping.Key];
            }
        }

        foreach (var mapping in this.mappings.OrderBy(map => map.Key))
        {
            if (mapping.Key.Contains(iTagToInstatiate.ToUpper()))
            {
                return this.mappings[mapping.Key];
            }
        }

        return null;
    }

    private void LoadITagDataTypes()
    {
        this.mappings = new Dictionary<string, Type>();

        Type[] mappingTypes = Assembly.GetExecutingAssembly().GetTypes();

        foreach (Type type in mappingTypes)
        {
            if (type.GetInterface(typeof(ITag).ToString()) != null)
            {
                this.mappings.Add(type.Name.ToUpper(), type);
            }
        }
    }

    private void LoadTagToClassMappings()
    {
        this.swiftTagToITagMapping = new Dictionary<string, string>();

        //:16x
        this.swiftTagToITagMapping.Add("20", "PatternGetReference");
        this.swiftTagToITagMapping.Add("21", "PatternGetReference");
                   
        // 35*50x
        this.swiftTagToITagMapping.Add("79", "PatternGetAllLines");
    }
}