namespace Application.Core.Swift
{
    public class BasicHeader
    {
        public string SenderBIC { get; set; }

        public string BranchCode { get; set; }

        public string SessionNumber { get; set; }

        public string SequenceNumber { get; set; }

        public BasicHeader() { }

        public BasicHeader(Dictionary<string, string> parsedSwiftMessage)
        {
            string swiftSection = parsedSwiftMessage["BasicHeader"];
            SenderBIC = swiftSection.Substring(3, 8);
            BranchCode = swiftSection.Substring(12, 3);
        }
    }
}