using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Ecclesia.XML;
using System.Xml.Linq;

namespace Xamarin.Ecclesia.Models.Utils
{
    public static class XMLProvider
    {
        public static void Init()
        {
            XMLLoader.RequestXML += XMLLoader_RequestXML;
        }

        static void XMLLoader_RequestXML(string filename)
        {
            XMLHelper.SetXML(XDocument.Load(filename));
        }
    }
}