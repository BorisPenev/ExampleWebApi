namespace Application.Core.Swift.MtTags;

public class PatternGetReference : Tag, ITag
{
    public ITag GetTagValues(string resultText)
    {
        // :16X

        base.GetTagName(resultText);

        if (resultText.Contains("::"))
        {
            this.Value = resultText.ToEndOfString($"{this.TagName}::").Trim();
        }
        else
        {
            this.Value = resultText.ToEndOfString($"{this.TagName}:").Trim();
        }

        return this;
    }
}