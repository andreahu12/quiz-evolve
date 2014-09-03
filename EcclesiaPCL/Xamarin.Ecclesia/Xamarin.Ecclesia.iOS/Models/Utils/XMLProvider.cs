using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Ecclesia.XML;
using System.Xml.Linq;

namespace Xamarin.Ecclesia.Models.Utils
{
    public class XMLLoader : IXMLLoader
    {
        public XDocument LoadXML(string filename)
        {
            return XDocument.Load(filename);
        }
    }
}