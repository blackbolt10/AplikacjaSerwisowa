using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Xml;


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

        [WebMethod(Description = "Hello lame .......!")]
        public string SayHelloToLame(String test)
        {
            return "Hello lame " + test + "!";
        }

        [WebMethod(Description = "Zwraca tajny kod!")]
        public string lama()
        {
            return "You find secret lame!";
        }

        [WebMethod(Description = "Wstępnie do usunięcia")]
        public string XMLZwrocListeKntKarty()
        {
            DataBase dataBaseObject = new DataBase();
            String result = dataBaseObject.pobierzKntKarty();

            return result.ToString();
        }













        [WebMethod(Description = "Pozwala na wygenerowanie KntAdresy")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeKntAdresy()
        {
            DataBase dataBaseObject = new DataBase();
            List<KntAdresy> listaKntAdresy = dataBaseObject.wygenerujListeKntAdresy();

            return new JavaScriptSerializer().Serialize(listaKntAdresy);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie KntKarty")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeKntKarty()
        {
            DataBase dataBaseObject = new DataBase();
            List<KntKarty> listaKntKarty = dataBaseObject.wygenerujListeKntKarty();

            return new JavaScriptSerializer().Serialize(listaKntKarty);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie SerwisoweZleceniaNaglownki")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeZlecenSerwisowychNaglowki()
        {
            DataBase dataBaseObject = new DataBase();
            List<SerwisoweZleceniaNaglownki> listaSerwisoweZlecenNaglowki = dataBaseObject.wygenerujListeSerwisowychZlecenNaglowki();

            return new JavaScriptSerializer().Serialize(listaSerwisoweZlecenNaglowki);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie SrwZlcCzynnosci")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeZlecenSerwisowychCzynnosci()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcCzynnoci> listaSerwisoweZlecenCzynnosci = dataBaseObject.wygenerujListeSrwZlcCzynnoci();

            return new JavaScriptSerializer().Serialize(listaSerwisoweZlecenCzynnosci);
        }
    }
}