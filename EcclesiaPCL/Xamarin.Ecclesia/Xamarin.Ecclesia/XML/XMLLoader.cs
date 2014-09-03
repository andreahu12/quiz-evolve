using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Xamarin.Ecclesia.XML
{
    public interface IXMLLoader
    {
        XDocument LoadXML(string fileName);
        
    }
}
