using System;
using System.Runtime.Serialization;

namespace SpamBlock
{
    [Serializable]
    public class CheckException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //
        public ErrorReason Reason { get; }

        public CheckException(ErrorReason reason)
        {
            Reason = reason;
        }

        public CheckException(ErrorReason reason, string message) : base(message)
        {
            Reason = reason;
        }

        public CheckException(ErrorReason reason, string message, Exception inner) : base(message, inner)
        {
            Reason = reason;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Reason), (int)Reason);
            base.GetObjectData(info, context);
        }

        protected CheckException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            Reason = (ErrorReason) info.GetInt32(nameof(Reason));
        }
    }
}