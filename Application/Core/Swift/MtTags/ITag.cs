namespace Application.Core.Swift.MtTags;

public interface ITag
{
    string TagName { get; set; }

    string Qualifier { get; set; }

    string Type { get; set; }

    string Code { get; set; }

    string Value { get; set; }

    string Description { get; set; }

    ITag GetTagValues(string resultText);
}
