using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;


namespace DataSurceInitialize
{
    public static class XMLTools
    {

        const string s_dir = @"..\xml\";
        static XMLTools()
        {
            if (!Directory.Exists(s_dir))
                Directory.CreateDirectory(s_dir);
        }

        //static readonly bool s_writing = false;
        public static void SaveListToXMLSerializer<T>(List<T?> list, string entity) where T : struct
        {
            string filePath = $"{s_dir + entity}.xml";
            try
            {
                using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                //using XmlWriter writer = XmlWriter.Create(file, new XmlWriterSettings() { Indent = true });

                XmlSerializer serializer = new(typeof(List<T?>));
                //if (s_writing)
                //    serializer.Serialize(writer, list);
                //else
                serializer.Serialize(file, list);
            }
            catch (Exception ex)
            {
                // DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex); 
                throw new Exception($"fail to create xml file: {filePath}", ex);
            }
        }

        public static List<T?> LoadListFromXMLSerializer<T>(string entity) where T : struct
        {
            string filePath = $"{s_dir + entity}.xml";
            try
            {
                if (!File.Exists(filePath)) return new();
                using FileStream file = new(filePath, FileMode.Open);
                XmlSerializer x = new(typeof(List<T?>));
                return x.Deserialize(file) as List<T?> ?? new();
            }
            catch (Exception ex)
            {
                // DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
                throw new Exception($"fail to load xml file: {filePath}", ex);
            }
        }

        public static void SaveListToXMLElement(XElement rootElem, string entity)
        {
            string filePath = $"{s_dir + entity}.xml";
            try
            {
                rootElem.Save(filePath);
            }
            catch (Exception ex)
            {
                // DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
                throw new Exception($"fail to create xml file: {filePath}", ex);
            }
        }

        public static XElement LoadListFromXMLElement(string entity)
        {
            string filePath = $"{s_dir + entity}.xml";
            try
            {
                if (File.Exists(filePath))
                    return XElement.Load(filePath);
                XElement rootElem = new(entity);
                rootElem.Save(filePath);
                return rootElem;
            }
            catch (Exception ex)
            {
                //new DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
                throw new Exception($"fail to load xml file: {filePath}", ex);
            }
        }
    }


}
