using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kalitte.Sensors.Utilities
{
    public sealed class SerializationHelper
    {
        private SerializationHelper()
        {
        }

        public static T DeserializeFromXmlDataContract<T>(string xml)
        {
            return (T)DeserializeFromXmlDataContract(xml, typeof(T));
        }

        internal static object DeserializeFromXmlDataContract(Stream stream, Type typeToDeserializeInto)
        {
            XmlTextReader reader = new XmlTextReader(stream);
            reader.WhitespaceHandling = WhitespaceHandling.Significant;
            reader.Normalization = true;
            return DeserializeFromXmlDataContract(reader, typeToDeserializeInto);
        }

        public static object DeserializeFromXmlDataContract(string xml, Type typeToDeserializeInto)
        {
            using (StringReader reader = new StringReader(xml))
            {
                return DeserializeFromXmlDataContract(XmlReader.Create(reader, GetXmlReaderSettingsForDeserializeFromXmlDataContruct()), typeToDeserializeInto);
            }
        }

        internal static object DeserializeFromXmlDataContract(XmlReader reader, Type typeToDeserializeInto)
        {
            return GetDataContractSerializer(typeToDeserializeInto).ReadObject(reader);
        }

        private static DataContractSerializer GetDataContractSerializer(Type type)
        {
            return new DataContractSerializer(type, TypesHelper.KnownTypes);
        }

        private static XmlReaderSettings GetXmlReaderSettingsForDeserializeFromXmlDataContruct()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CheckCharacters = false;
            return settings;
        }

        public static string SerializeToXmlDataContract(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            return SerializeToXmlDataContract(obj,false);
        }

        public static string SerializeToXmlDataContract(object obj, bool omitHeader)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            return SerializeToXmlDataContract(obj, obj.GetType(), omitHeader, false);
        }



        public static string SerializeToXmlDataContract(object obj, Type type, bool omitHeader)
        {
            return SerializeToXmlDataContract(obj, type, omitHeader, false);
        }

        public static string SerializeToXmlDataContract(object obj, Type type, bool omitHeader, bool makeXmlReadable)
        {
            StringBuilder output = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CheckCharacters = false;
            settings.OmitXmlDeclaration = omitHeader;
            if (makeXmlReadable)
            {
                settings.Indent = true;
                settings.IndentChars = "\t";
            }
            XmlWriter writer = XmlWriter.Create(output, settings);
            GetDataContractSerializer(type).WriteObject(writer, obj);
            writer.Close();
            return output.ToString();
        }

        public static object CopiedObject(object source)
        {
            string xml = SerializeToXmlDataContract(source, false);

            return DeserializeFromXmlDataContract(xml, source.GetType());

        }

        public static byte [] BinarySerializeToByteArray(object obj)
        {
            BinaryFormatter fmt = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                fmt.Serialize(stream, obj);
                byte[] b = stream.ToArray();
                stream.Close();
                return b;
            }
        }

        public static object BinaryDeSerializeFromByteArray(byte [] bytes)
        {
            BinaryFormatter fmt = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return fmt.Deserialize(stream);
            }
        }

        public static string BinarySerialize(object obj)
        {
            BinaryFormatter fmt = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                fmt.Serialize(stream, obj);
                byte[] b = stream.ToArray();
                stream.Close();
                return Convert.ToBase64String(b);
            }
        }

        public static object BinaryDeSerialize(string data)
        {
            BinaryFormatter fmt = new BinaryFormatter();
            byte[] bytes = Convert.FromBase64String(data);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return fmt.Deserialize(stream);
            }
        }
    }
}
