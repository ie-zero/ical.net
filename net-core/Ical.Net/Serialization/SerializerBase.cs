using System;
using System.IO;
using System.Text;

namespace Ical.Net.Serialization
{
    public abstract class SerializerBase : IStringSerializer
    {
        protected SerializerBase()
        {
            SerializationContext = SerializationContext.Default;
        }

        protected SerializerBase(SerializationContext ctx)
        {
            SerializationContext = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        protected SerializationContext SerializationContext { get; }

        public abstract Type TargetType { get; }

        public abstract string SerializeToString(object obj);

        public abstract object Deserialize(TextReader tr);

        // TODO: 'SerializerBase' class is the only place the Push() and Pop() methods of the EncodingStack are used. 
        //      Consider removing the Deserialize(Stream, ...) and Serialize(..., Stream, ...) methods all together or using UTF8 enciding.
        // 
        // RFC5545   - 8.1. iCalendar Media Type Registration
        // The "charset" parameter is defined in [RFC2046] for subtypes of
        // the "text" media type.It is used to indicate the charset used in
        // the body part.The charset supported by this revision of
        // iCalendar is UTF-8.  The use of any other charset is deprecated by
        // this revision of iCalendar; however, note that this revision
        // requires that compliant applications MUST accept iCalendar streams
        // using either the UTF-8 or US-ASCII charset.
        // 

        public object Deserialize(Stream stream, Encoding encoding)
        {
            object obj;
            using (var reader = new StreamReader(stream, encoding))
            {
                var encodingStack = SerializationContext.GetService<EncodingStack>();
                encodingStack.Push(encoding);
                obj = Deserialize(reader);
                encodingStack.Pop();
            }
            return obj;
        }

        public void Serialize(object obj, Stream stream, Encoding encoding)
        {
            // TODO: Check 'stream' for null. 
            // TODO: Check 'encoding' for null.

            // NOTE: we don't use a 'using' statement here because
            // we don't want the stream to be closed by this serialization.
            // Fixes bug #3177278 - Serialize closes stream

            const int defaultBuffer = 1024;     //This is StreamWriter's built-in default buffer size
            using (var writer = new StreamWriter(stream, encoding, defaultBuffer, leaveOpen: true))
            {
                // Push the current object onto the serialization stack
                SerializationContext.Push(obj);

                // Push the current encoding on the stack
                var encodingStack = SerializationContext.GetService<EncodingStack>();
                encodingStack.Push(encoding);

                writer.Write(SerializeToString(obj));

                // Pop the current encoding off the serialization stack
                encodingStack.Pop();

                // Pop the current object off the serialization stack
                SerializationContext.Pop();
            }
        }
    }
}