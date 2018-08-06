using System;
using System.IO;
using System.Text;

namespace Ical.Net.Serialization
{
    public interface ISerializer 
    {
        SerializationContext SerializationContext { get; }
        Type TargetType { get; }

        void Serialize(object obj, Stream stream, Encoding encoding);
        object Deserialize(Stream stream, Encoding encoding);
    }
}