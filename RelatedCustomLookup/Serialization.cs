using System;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace RelatedCustomLookup
{
    public class Serialization<T> where T : class
    {
        /// <summary>
        /// Serailize object to string
        /// </summary>
        /// <param name="o">Object to be serialized</param>
        /// <returns></returns>
        public static string SerializeObject(T o)
        {
            DataContractSerializer dcs = new DataContractSerializer(o.GetType());

            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateTextWriter(ms))
                {
                    dcs.WriteObject(xdw, o);
                    xdw.Flush();
                    ms.Position = 0;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ms);
                    return xmlDoc.InnerXml;
                }
            }
        }

        /// <summary>
        /// Deserialize string to object
        /// </summary>
        /// <param name="sXmlDoc">XML representation of the object </param>
        /// <returns></returns>
        public static T DeserializeObject(string sXmlDoc)
        {
            if (string.IsNullOrEmpty(sXmlDoc))
                return Activator.CreateInstance(typeof(T)) as T;  // return empty object

            return Deserialize(sXmlDoc);
        }

        /// <summary>
        /// Deserialize string to object
        /// </summary>
        /// <param name="sXmlDoc">XML representation of the object </param>
        /// <returns></returns>
        private static T Deserialize(string sXmlDoc)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(sXmlDoc)))
            {
                object result = dcs.ReadObject(ms);
                return result as T;
            }
        }
    }
}
