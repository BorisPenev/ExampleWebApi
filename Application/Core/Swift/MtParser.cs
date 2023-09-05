
namespace Application.Core.Swift;

public class MTParser
{
    private bool isTag;
    private List<string> swiftTags = new List<string>();

    public MTParser()
    {
        LoadSwiftTags();
    }

    public Dictionary<string, string> SeperateSWIFTFile(string message)
    {
        Dictionary<string, string> swiftMessage = new Dictionary<string, string>();

        if (message.Contains("{1:"))
        {
            string Block1 = message.Between("{1:", "}");
            swiftMessage.Add(Constants.BasicHeaderBlock1Key, Block1);
        }
        if (message.Contains("{2:"))
        {
            string Block2 = message.Between("{2:", "}");
            swiftMessage.Add(Constants.ApplicationHeaderBlock2Key, Block2);
        }
        if (message.Contains("{3:"))
        {
            string Block3 = message.Between(":{", "}");
            swiftMessage.Add(Constants.UserHeaderBlock3Key, Block3);

        }
        if (message.Contains("{4:"))
        {
            string Block4 = message.Between("{4:", "}");
            swiftMessage.Add(Constants.TextBlockBlock4Key, Block4);
        }

        if (message.Contains("{5:"))
        {
            string Block5 = message.Between("{5:", "}");
            swiftMessage.Add(Constants.TrailerBlock5Key, Block5);

        }

        return swiftMessage;
    }

    public List<string> Block4ToList(string message)
    {
        List<string> listOfTags = new List<string>();
        this.isTag = false;

        int index = 0;
        var totalStringSize = message.Length;

        while (index < totalStringSize)
        {
            var newIndex = message.GetSwiftTag(index);

            if (newIndex > 0)
            {
                var result = CheckTag(index + newIndex, totalStringSize, message);

                if (this.isTag)
                {
                    var newTag = message.Substring(index, newIndex);
                    listOfTags.Add(newTag.Trim());
                    index = index + newIndex;
                    this.isTag = false;
                }
                else
                {
                    var newTag = message.Substring(index, result);
                    listOfTags.Add(newTag.TrimAllNewLines());
                    index = result;
                    this.isTag = false;
                }
            }
            else
            {
                var newTag = message.Substring(index, (totalStringSize - index));
                listOfTags.Add(newTag.TrimEndOfSwift().Trim());
                index = totalStringSize + 1;
            }
        }

        return listOfTags;

    }

    private int CheckTag(int index, int size, string message)
    {
        int result = 0;

        if (index + 3 >= size || index + 4 >= size)
        {
            result = 0;
        }
        else if (message.Substring(index + 3, 1) == ":" || message.Substring(index + 4, 1) == ":")
        {
            if (CheckValidTag(index, message) == true)
            {
                result = index;
                this.isTag = true;
                return result;
            }
            else
            {
                var displacement = message.GetSwiftTag(index);
                result = index + displacement;
                result = CheckTag(result, size, message);
                this.isTag = false;
            }
        }
        else
        {
            var displacement = message.GetSwiftTag(index);
            result = index + displacement;
            result = CheckTag(result, size, message);
            this.isTag = false;            
        }

        return result;
    }

    private bool CheckValidTag(int index, string message)
    {
        var result = false;

        foreach (var validTag in this.swiftTags)
        {
            if (validTag == message.Substring(index + 1, 2) || validTag == message.Substring(index + 1, 3))
            {
                result = true;
                return result;
            }
        }

        return result;
    }

    private void LoadSwiftTags()
    {
        this.swiftTags.Add("20");
        this.swiftTags.Add("21");
        this.swiftTags.Add("79");
    }
}