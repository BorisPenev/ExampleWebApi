namespace Application.Core.Swift.MtTags;

public class PatternGetAllLines : Tag, ITag
{
    public ITag GetTagValues(string resultText)
    {
        base.GetTagName(resultText);
        this.Value = resultText.ToEndOfString(this.TagName + ":").TrimAllNewLines();
        return this;
    }
}
