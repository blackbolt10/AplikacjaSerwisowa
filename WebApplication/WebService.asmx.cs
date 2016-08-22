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
    [WebService(Namespace = "galsoftsrv")]
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

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(listaKntAdresy);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie KntKarty")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeKntKarty()
        {
            DataBase dataBaseObject = new DataBase();
            List<KntKarty> listaKntKarty = dataBaseObject.wygenerujListeKntKarty();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(listaKntKarty);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie SrwZlcNag")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeZlecenSerwisowychNaglowki()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcNag> listaSerwisoweZlecenNaglowki = dataBaseObject.wygenerujListeSerwisowychZlecenNaglowki();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(listaSerwisoweZlecenNaglowki);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie SrwZlcCzynnosci")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeZlecenSerwisowychCzynnosci()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcCzynnosci> listaSerwisoweZlecenCzynnosci = dataBaseObject.wygenerujListeSrwZlcCzynnosci();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(listaSerwisoweZlecenCzynnosci);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie SrwZlcSkladniki")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeZlecenSerwisowychSkladniki()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcSkladniki> listaSerwisoweZlecenCzynnosci = dataBaseObject.wygenerujListeSrwZlcSkladniki();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(listaSerwisoweZlecenCzynnosci);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie TwrKartyTable")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeTwrKarty()
        {
            DataBase dataBaseObject = new DataBase();
            List<TwrKartyTable> twrKartyList = dataBaseObject.wygenerujListeTwrKarty();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(twrKartyList);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie SrwUrzRodzaje")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeSrwUrzRodzaje()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwUrzRodzaje> SrwUrzRodzajeList = dataBaseObject.wygenerujListeSrwUrzRodzaje();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwUrzRodzajeList);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie SrwUrzRodzaje")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeSrwUrzadzenia()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwUrzadzenia> SrwUrzadzeniaList = dataBaseObject.wygenerujListeSrwUrzadzenia();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwUrzadzeniaList);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie SrwUrzRodzPar")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeSrwUrzRodzPar()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwUrzRodzPar> SrwUrzRodzParList = dataBaseObject.wygenerujListeSrwUrzRodzPar();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwUrzRodzParList);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie SrwUrzParDef")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeSrwUrzParDef()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwUrzParDef> SrwUrzParDefList = dataBaseObject.wygenerujListeSrwUrzParDef();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwUrzParDefList);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie SrwZlcPodpisTable")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListePodpisow()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcPodpisTable> podpisyList = dataBaseObject.wygenerujListeSrwZlcPodpis();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(podpisyList);
        }

        [WebMethod(Description = "Pozwala na synchronizację SrwZlcNag z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string synchronizujSrwZlcNag(String inputJSON)
        {
            DataBase dataBaseObject = new DataBase();
            List<int> output = dataBaseObject.synchronizujSrwZlcNag(inputJSON);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
        }

        [WebMethod(Description = "Pozwala na synchronizację SrwZlcCzynnosci z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string synchronizujSrwZlcCzynnosci(String inputJSON)
        {
            DataBase dataBaseObject = new DataBase();
            List<int> output = dataBaseObject.synchronizujSrwZlcCzynnosci(inputJSON);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
        }

        [WebMethod(Description = "Pozwala na synchronizację SrwZlcSkladniki z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string synchronizujSrwZlcSkladniki(String inputJSON)
        {
            DataBase dataBaseObject = new DataBase();
            List<int> output = dataBaseObject.synchronizujSrwZlcSkladniki(inputJSON);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
        }

        [WebMethod(Description = "Pozwala na wygenerowanie listy Operatorów")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeOperatorow()
        {
            DataBase dataBaseObject = new DataBase();
            List<Operatorzy> output = dataBaseObject.wygenerujListeOperatorow();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
        }

        [WebMethod(Description = "... do GalSrv SrwZlcNag")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GalSrv_SrwZlcNag()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcNag> output = dataBaseObject.GalSrv_Generuj_SrwZlcNag();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
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

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
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

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
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