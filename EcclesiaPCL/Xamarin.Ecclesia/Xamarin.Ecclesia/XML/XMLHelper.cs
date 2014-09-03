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
        public static IXMLLoader XMLLoader;

        public static XDocument LoadXML(string filename)
        {
            return XMLLoader.LoadXML(filename);
        }
        
    }
}
