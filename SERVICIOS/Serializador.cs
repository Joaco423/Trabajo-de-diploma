using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SERVICIOS
{
    // Serializa un objeto a texto en XML o JSON y lo vuelve a armar a partir de ese texto.
    // El formato se indica con las palabras "XML" o "JSON" (las que ofrece el combo de la pantalla).
    public class Serializador
    {
        public string Serializar_235JB<T>(T objeto_235JB, string formato_235JB)
        {
            if (objeto_235JB == null) throw new ArgumentNullException("objeto");

            using (MemoryStream memoria_235JB = new MemoryStream())
            {
                switch (NormalizarFormato_235JB(formato_235JB))
                {
                    case "XML":
                        XmlWriterSettings ajustes_235JB = new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(false) };
                        using (XmlWriter escritorXml_235JB = XmlWriter.Create(memoria_235JB, ajustes_235JB))
                            new XmlSerializer(typeof(T)).Serialize(escritorXml_235JB, objeto_235JB);
                        break;

                    default:
                        using (XmlDictionaryWriter escritorJson_235JB = JsonReaderWriterFactory.CreateJsonWriter(memoria_235JB, Encoding.UTF8, false, true, "  "))
                            new DataContractJsonSerializer(typeof(T)).WriteObject(escritorJson_235JB, objeto_235JB);
                        break;
                }
                return Encoding.UTF8.GetString(memoria_235JB.ToArray());
            }
        }

        public T Deserializar_235JB<T>(string texto_235JB, string formato_235JB)
        {
            if (string.IsNullOrWhiteSpace(texto_235JB)) throw new ArgumentException("El texto a deserializar esta vacio.");

            switch (NormalizarFormato_235JB(formato_235JB))
            {
                case "XML":
                    using (StringReader lector_235JB = new StringReader(texto_235JB))
                        return (T)new XmlSerializer(typeof(T)).Deserialize(lector_235JB);

                default:
                    using (MemoryStream memoria_235JB = new MemoryStream(Encoding.UTF8.GetBytes(texto_235JB)))
                        return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(memoria_235JB);
            }
        }

        // Devuelve "XML" o "JSON"; cualquier otra cosa no se soporta.
        public string NormalizarFormato_235JB(string formato_235JB)
        {
            string f_235JB = (formato_235JB ?? string.Empty).Trim().ToUpperInvariant();
            if (f_235JB != "XML" && f_235JB != "JSON")
                throw new ArgumentException("Formato de serializacion no soportado: " + formato_235JB);
            return f_235JB;
        }
    }
}
