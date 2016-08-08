using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;


namespace WebApplication
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://91.196.9.105/")]
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
            List<SrwZlcCzynnosci> listaSerwisoweZlecenCzynnosci = dataBaseObject.wygenerujListeSrwZlcCzynnosci();

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

        [WebMethod(Description = "Pozwala na synchronizację SrwZlcCzynnosci z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string synchronizujSrwZlcCzynnosci(String inputJSON)
        {
            DataBase dataBaseObject = new DataBase();
            List<int> output = dataBaseObject.synchronizujSrwZlcCzynnosci(inputJSON);

            return new JavaScriptSerializer().Serialize(output);
        }

        [WebMethod(Description = "Pozwala na synchronizację SrwZlcSkladniki z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string synchronizujSrwZlcSkladniki(String inputJSON)
        {
            DataBase dataBaseObject = new DataBase();
            List<int> output = dataBaseObject.synchronizujSrwZlcSkladniki(inputJSON);

            return new JavaScriptSerializer().Serialize(output);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie listy Operatorów")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeOperatorow()
        {
            DataBase dataBaseObject = new DataBase();
            List<Operatorzy> output = dataBaseObject.wygenerujListeOperatorow();

            return new JavaScriptSerializer().Serialize(output);
        }

        [WebMethod(Description = "... do GalSrv SrwZlcNag")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GalSrv_SrwZlcNag()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcNag> output = dataBaseObject.GalSrv_Generuj_SrwZlcNag();

            return new JavaScriptSerializer().Serialize(output);
        }

        [WebMethod(Description = "GalSrv SrwZlcNagPotwierdzenie")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GalSrv_SrwZlcNagPotwierdzenie(String listaPotwierdzonych)
        {
            DataBase dataBaseObject = new DataBase();
            string result =  dataBaseObject.GalSrv_Potwierdz_SrwZlcNag(listaPotwierdzonych);
            return result;
        }

        [WebMethod(Description = "... do GalSrv SrwZlcCzynnosci")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GalSrv_SrwZlcCzynnosci()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcCzynnosci> output = dataBaseObject.GalSrv_Generuj_SrwZlcCzynnosci();

            return new JavaScriptSerializer().Serialize(output);
        }

        [WebMethod(Description = "GalSrv SrwZlcCzynnosciPotwierdzenie")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GalSrv_SrwZlcCzynnosciPotwierdzenie(String listaPotwierdzonych)
        {
            DataBase dataBaseObject = new DataBase();
            string result =  dataBaseObject.GalSrv_Potwierdz_SrwZlcCzynnosci(listaPotwierdzonych);
            return result;
        }

        [WebMethod(Description = "... do GalSrv SrwZlcSkladniki")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GalSrv_SrwZlcSkladniki()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcSkladniki> output = dataBaseObject.GalSrv_Generuj_SrwZlcSkladniki();

            return new JavaScriptSerializer().Serialize(output);
        }

        [WebMethod(Description = "GalSrv SrwZlcSkladnikiPotwierdzenie")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GalSrv_SrwZlcSkladnikiPotwierdzenie(String listaPotwierdzonych)
        {
            DataBase dataBaseObject = new DataBase();
            dataBaseObject.GalSrv_Potwierdz_SrwZlcSkladniki(listaPotwierdzonych);
        }
    }
}