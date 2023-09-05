namespace Application.Core.Swift;

public static class StringExtensions
{
    public static string ConvertFromUnix(this string value)
    {
        var s = value.Replace("\n", "\r\n");
        return s;
    }

    public static string Between(this string value, string a, string b)
    {
        int positionA = value.IndexOf(a);
        int positionB = value.IndexOf(b, positionA);

        if (positionA == -1 || positionB == -1)
        {
            return string.Empty;
        }

        int adjustedPosA = positionA + a.Length;
        if (adjustedPosA >= positionB)
        {
            return string.Empty;
        }

        return value.Substring(adjustedPosA, positionB - adjustedPosA);
    }

    public static string ParseFromString(this string value, string a, string b)
    {
        int positionA = value.IndexOf(a);
        string result = value.Substring(positionA + a.Length);
        int displacement = value.Length - result.Length;
        int positionB = result.IndexOf(b);

        if (positionA == -1 || positionB == -1)
        {
            return string.Empty;
        }
       
        return result.Substring(0, positionB);
    }

    public static string ParseWithStringAndIndex(this string value, string a, int index)
    {
        int positionA = value.IndexOf(a);
        int positionB = index;

        if (positionA == -1 || positionB == -1)
        {
            return string.Empty;
        }

        int adjustedPosA = positionA + a.Length;
        return value.Substring(adjustedPosA, index);
    }

    public static string ToEndOfString(this string value, string a)
    {
        int positionA = value.IndexOf(a) + a.Length;
        return value.Substring(positionA);
    }

    public static string TrimAllNewLines(this string value)
    {
        return value.Replace(Environment.NewLine, " ").Trim();
    }

    public static string TrimColon(this string value)
    {
        value = value.Trim(new Char[] { ':' });
        return value;
    }

    public static string TrimEndOfSwift(this string value)
    {
        value = value.Trim(new Char[] { '-', '}' });
        return value;
    }

    public static int GetSwiftTag(this string value, int a)
    {
        int displacement = 6;
        int result;
        if (value.Substring(a, 2) == ":")
        {
            displacement = displacement + 1;
        }
        else if (value.Substring(a, 2) == Environment.NewLine)
        {
            displacement = displacement + Environment.NewLine.Length;
        }

        var startCharacter = value.Substring(a, 1);

        if (startCharacter != ":")
        {
            displacement = displacement + 1;
        }

        int positionA = a + displacement;
        int positionB = value.IndexOf(":", positionA);        

        result = positionB - a;

        return result;
    }
}