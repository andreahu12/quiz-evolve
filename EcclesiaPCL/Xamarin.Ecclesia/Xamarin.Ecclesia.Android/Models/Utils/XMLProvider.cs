using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Ecclesia.XML;
using System.Xml.Linq;
using Android.App;

namespace Xamarin.Ecclesia.Models.Utils
{
    
    public class XMLLoader : IXMLLoader
    {
        static Activity _context;

        public XMLLoader(Activity activity)
        {
            _context = activity;
        }

        public XDocument LoadXML(string filename)
        {
            return XDocument.Load(_context.Assets.Open(filename));
        }
    }
}