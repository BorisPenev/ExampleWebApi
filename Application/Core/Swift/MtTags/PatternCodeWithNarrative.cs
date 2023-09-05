namespace Application.Core.Swift.MtTags;

public class PatternCodeWithNarrative : Tag, ITag
{
    //35*50x:4c/8c/34x (Qualifier) (Issuer Code) (Proprietor Code)

    public ITag GetTagValues(string resultText)
    {
        base.GetTagName(resultText);

        this.Qualifier = resultText.Between("::", "/");
        this.Code = resultText.ParseFromString(this.Qualifier + "/", "/");
        this.Value = resultText.ToEndOfString(this.Code + "/");

        return this;
    }

}