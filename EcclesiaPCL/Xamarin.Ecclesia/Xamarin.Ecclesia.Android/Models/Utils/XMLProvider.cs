using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Ecclesia.XML;
using System.Xml.Linq;
using Android.App;

namespace Xamarin.Ecclesia.Models.Utils
{
    public static class XMLProvider
    {
        static Activity _context;

        public static void Init(Activity activity)
        {
            _context = activity;
            XMLLoader.RequestXML += XMLLoader_RequestXML;
        }

        static void XMLLoader_RequestXML(string filename)
        {
            XMLHelper.SetXML(XDocument.Load(_context.Assets.Open(filename)));
        }
    }
}