using System;

namespace SpamBlock
{
    [Flags]
    public enum SpammerType
    {
        Suspicious = 1,
        Harvester = 2,
        CommentSpammer = 4
    }
}