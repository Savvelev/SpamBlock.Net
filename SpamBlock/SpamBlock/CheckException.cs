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

        public CheckException()
        {
        }

        public CheckException(string message) : base(message)
        {
        }

        public CheckException(string message, Exception inner) : base(message, inner)
        {
        }

       

        protected CheckException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}