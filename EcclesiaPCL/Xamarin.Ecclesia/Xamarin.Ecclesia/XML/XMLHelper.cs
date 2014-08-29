using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Xamarin.Ecclesia.XML
{
    public static class XMLHelper
    {
        public static event Action<XDocument> XMLLoaded;
        public static void SetXML(XDocument xmlDocument)
        {
            if (XMLLoaded != null)
            {
                XMLLoaded(xmlDocument);
            }
        }
    }
}
