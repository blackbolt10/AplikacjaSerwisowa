using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace WebApplication
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://91.196.8.98/AplikacjaSerwisowa/WebService.asmx")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld(String test)
        {
            return "Hello World " + test;
        }

        [WebMethod]
        public string lama()
        {
            return "You find secret lame!";
        }

        [WebMethod]
        public string test()
        {
            DataBase dataBaseObject = new DataBase();
            String result = dataBaseObject.pobierzKntKarty();

            return result.ToString();
        }
    }
}
