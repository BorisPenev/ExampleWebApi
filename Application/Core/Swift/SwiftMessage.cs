﻿using Application.Core.Swift.MtTags;

namespace Application.Core.Swift
{
    public class SwiftMessage
    {
        public SwiftMessage() { }

        public BasicHeader Block1 { get; set; }
        public ApplicationHeader Block2 { get; set; }
        public UserHeader Block3 { get; set; }
        public List<ITag> Block4 { get; set; }
        public Trailer Block5 { get; set; }

        public void ParseSwiftMessage(string swiftFormattedFile)
        {
            MTParser message = new MTParser();
            TagFactory tagFactory = new TagFactory();
            List<string> listOfTags = new List<string>();
            List<ITag> listOfITags = new List<ITag>();

            Dictionary<string, string> swiftBlocks = message.SeperateSWIFTFile(swiftFormattedFile);
            listOfTags = message.Block4ToList(swiftBlocks["TextBlock"]);

            this.Block1 = new BasicHeader(swiftBlocks);
            this.Block2 = new ApplicationHeader(swiftBlocks);

            foreach (var tag in listOfTags)
            {
                listOfITags = tagFactory.CreateInstance(tag, listOfITags);
            }

            this.Block4 = listOfITags;
        }
    }
}
