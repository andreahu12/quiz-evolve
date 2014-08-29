using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Xamarin.Ecclesia.XML
{
    public static class XMLLoader
    {
        public static event Action<string> RequestXML;

        public async static Task<XDocument> LoadXML(string fileName)
        {
            
            var tcs = new TaskCompletionSource<XDocument>();

            if (RequestXML != null)
            {
                Action<XDocument> handler = null;
                handler = (result) =>
                {
                    //we got result and can return it 
                    XMLHelper.XMLLoaded -= handler;
                    tcs.SetResult(result);
                };
                XMLHelper.XMLLoaded += handler;
                RequestXML(fileName);
            }
            else
                return null;
            return await tcs.Task;
        }
    }
}
