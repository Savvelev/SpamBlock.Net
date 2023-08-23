using System;

namespace SpamBlock.HttpBL
{
    [Flags]
    public enum SpammerType
    {
        Suspicious = 1,
        Harvester = 2,
        CommentSpammer = 4
    }
}