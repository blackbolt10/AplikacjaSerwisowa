using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Xml;

using Newtonsoft.Json;


namespace WebApplication
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://kwronski.hostingasp.pl/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public testClass[] GettestClassXML()
        {
            testClass[] empsw = new testClass[]
            {
                new testClass()
                {
                    ID = 1,
                    Name = "Test",
                    Salary = 10000
                },
                new testClass()
                {
                    ID = 2,
                    Name = "Test1",
                    Salary = 100000
                },
                new testClass()
                {
                    ID = 3,
                    Name = "Test2",
                    Salary = 1000000
                }
            };
            return empsw;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GettestClassJSON()
        {
            testClass[] empsw = new testClass[]
            {
                new testClass()
                {
                    ID = 1,
                    Name = "Test",
                    Salary = 10000
                },
                new testClass()
                {
                    ID = 2,
                    Name = "Test1",
                    Salary = 100000
                },
                new testClass()
                {
                    ID = 3,
                    Name = "Test2",
                    Salary = 1000000
                }
            };
            return new JavaScriptSerializer().Serialize(empsw);
        }

        [WebMethod(Description = "HelloWorld")]
        public string HelloWorld(String test)
        {
            return "Hello World " + test + "!";
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


        [WebMethod]
        public string SetData(String jsonString)
        {
            //Decodowanie JSON
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var records = ser.Deserialize<List<Person>>(jsonString);

            String result = "";

            for(int i = 0; i < records.Count; i++)
            {
                result += i + ": " + records[i].Name + "\n";
            }

            return result;
        }

        [ScriptMethod(UseHttpGet = true)]
        public string ZwrocCoDostales(String input)
        {
            return input;
        }


















        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeZlecenSerwisowychNaglowki()
        {
            /*
            List<SerwisoweZleceniaNaglownki> listaSerwisoweZlecenNaglowki = new List<SerwisoweZleceniaNaglownki>();

            DataBase dataBaseObject = new DataBase();
            listaSerwisoweZlecenNaglowki = dataBaseObject.wygenerujListeSerwisowychZlecenNaglowki();

            return new JavaScriptSerializer().Serialize(listaSerwisoweZlecenNaglowki);
            */
            return "[{\"ID\":3228,\"Dokument\":\"ZSR - 1 / 02 / 2013\",\"SZN_Id\":25,\"SZN_KntTyp\":32,\"SZN_KntNumer\":25,\"SZN_KnATyp\":864,\"SZN_KnANumer\":37,\"SZN_KnDTyp\":32,\"SZN_KnDNumer\":25,\"SZN_AdWTyp\":896,\"SZN_AdWNumer\":190,\"SZN_DataWystawienia\":\"2013 - 02 - 04 00:00:00\",\"SZN_DataRozpoczecia\":\"2013 - 02 - 04 00:00:00\",\"SZN_Stan\":\"Zatwierdzone\",\"SZN_Status\":\"\",\"SZN_CechaOpis\":\"\",\"SZN_Opis\":\"Przyczyna problemu:\r\nUszkodzona roleta automatyczna zerwała przewody elektryczne\r\n\r\nUwagi:\r\nRegały chłodnicze działają prawidłowo po odłączeniu silniczka rolety, który powodował zwarcie w instalacji.\"},{\"ID\":3229,\"Dokument\":\"ZSR - 2 / 02 / 2013\",\"SZN_Id\":26,\"SZN_KntTyp\":32,\"SZN_KntNumer\":25,\"SZN_KnATyp\":864,\"SZN_KnANumer\":37,\"SZN_KnDTyp\":32,\"SZN_KnDNumer\":25,\"SZN_AdWTyp\":896,\"SZN_AdWNumer\":156,\"SZN_DataWystawienia\":\"2013 - 02 - 01 00:00:00\",\"SZN_DataRozpoczecia\":\"2013 - 02 - 01 00:00:00\",\"SZN_Stan\":\"Zamknięte\",\"SZN_Status\":\"\",\"SZN_CechaOpis\":\"\",\"SZN_Opis\":\"\"},{\"ID\":3230,\"Dokument\":\"ZSR - 1 / 06 / 2013\",\"SZN_Id\":31,\"SZN_KntTyp\":32,\"SZN_KntNumer\":242,\"SZN_KnATyp\":864,\"SZN_KnANumer\":341,\"SZN_KnDTyp\":32,\"SZN_KnDNumer\":242,\"SZN_AdWTyp\":896,\"SZN_AdWNumer\":396,\"SZN_DataWystawienia\":\"2013 - 06 - 27 00:00:00\",\"SZN_DataRozpoczecia\":\"2013 - 06 - 27 00:00:00\",\"SZN_Stan\":\"Zamknięte\",\"SZN_Status\":\"\",\"SZN_CechaOpis\":\"\",\"SZN_Opis\":\"Wymiana wentylatora skraplacza w witrynie cukierniczej\"}]";
        }
    }

    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }

        public Person(String _Name, Int32 _ID, Int32 _Salary)
        {
            ID = _ID;
            Name = _Name;
            Salary = _Salary;
        }
        public Person() { }
    }
}