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

        [WebMethod(Description = "Pozwala na wygenerowanie SrwZlcNag")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeZlecenSerwisowychNaglowki()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcNag> listaSerwisoweZlecenNaglowki = dataBaseObject.wygenerujListeSerwisowychZlecenNaglowki();

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

        [WebMethod(Description = "Pozwala na wygenerowanie SrwZlcSkladniki")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeZlecenSerwisowychSkladniki()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcSkladniki> listaSerwisoweZlecenCzynnosci = dataBaseObject.wygenerujListeSrwZlcSkladniki();

            return new JavaScriptSerializer().Serialize(listaSerwisoweZlecenCzynnosci);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie TwrKartyTable")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeTwrKarty()
        {
            DataBase dataBaseObject = new DataBase();
            List<TwrKartyTable> twrKartyList = dataBaseObject.wygenerujListeTwrKarty();

            return new JavaScriptSerializer().Serialize(twrKartyList);
        }

        [WebMethod(Description = "Pozwala na synchronizację SrwZlcNag z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string synchronizujSrwZlcNag(String inputJSON)
        {
            DataBase dataBaseObject = new DataBase();
            List<int> output = dataBaseObject.synchronizujSrwZlcNag(inputJSON);

            return new JavaScriptSerializer().Serialize(output);
        }


        
    }
}