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

        [WebMethod(Description = "Pozwala na wygenerowanie SrwZlcUrz")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ZwrocListeSrwZlcUrz()
        {
            DataBase dataBaseObject = new DataBase();
            List<SrwZlcUrz> SrwZlcUrzList = dataBaseObject.wygenerujListeSrwZlcUrz();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwZlcUrzList);
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
















        [WebMethod(Description = "Zapisuje kontrhanetów z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String KntKarty_ZapiszDaneUrzadzenia(String inputJSON)
        {
            DataBase db = new DataBase();
            db.KntKarty_ZapiszDaneUrzadzenia(inputJSON);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Zwraca nowych kontrahentow")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string KntKarty_ZwrocNowych()
        {
            DataBase db = new DataBase();
            List<KntKarty> output = db.kntKarty_ZwrocNowych();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
        }

        [WebMethod(Description = "Zwraca zmodyfikowanych kontrahentow")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string KntKarty_ZwrocZmodyfikowanych()
        {
            DataBase db = new DataBase();
            List<KntKarty> output = db.kntKarty_ZwrocZmodyfikowanych();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
        }

        [WebMethod(Description = "Zwraca kontrahentow do usuniecia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string KntKarty_ZwrocUsunietych()
        {
            DataBase db = new DataBase();
            List<int> output = db.kntKarty_ZwrocUsunietych();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
        }










        [WebMethod(Description = "Zapisuje kontrhanetów z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String KntAdresy_ZapiszDaneUrzadzenia(String inputJSON)
        {
            DataBase db = new DataBase();
            db.KntAdresy_ZapiszDaneUrzadzenia(inputJSON);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Zwraca nowych kontrahentow")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string KntAdresy_ZwrocNowych()
        {
            DataBase db = new DataBase();
            List<KntAdresy> output = db.KntAdresy_ZwrocNowych();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
        }

        [WebMethod(Description = "Zwraca zmodyfikowanych kontrahentow")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string KntAdresy_ZwrocZmodyfikowanych()
        {
            DataBase db = new DataBase();
            List<KntAdresy> output = db.KntAdresy_ZwrocZmodyfikowanych();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
        }

        [WebMethod(Description = "Zwraca kontrahentow do usuniecia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string KntAdresy_ZwrocUsunietych()
        {
            DataBase db = new DataBase();
            List<int> output = db.KntAdresy_ZwrocUsunietych();

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(output);
        }



        [WebMethod(Description = "Zapis danych z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Aplikacja_Zapisz_Kontrahentow(Int32 idOperatora, String kntKartyString, String kntAdresyString)
        {
            DataBase db = new DataBase();
            db.Zapisz_Dane_Kontrahentow(idOperatora, kntKartyString, kntAdresyString);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Zapis danych z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Aplikacja_Zapisz_Towary(Int32 idOperatora, String TwrKartyString)
        {
            DataBase db = new DataBase();
            db.Zapisz_Dane_Towary(idOperatora, TwrKartyString);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Zapis danych z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Aplikacja_Zapisz_Zlecenia(Int32 idOperatora, String SrwZlcNagString, String srwZlcCzynnosciString, String SrwZlcSkladnikiString, string SrwZlcUrzString)
        {
            DataBase db = new DataBase();
            db.Zapisz_Dane_Zlecenia(idOperatora, SrwZlcNagString, srwZlcCzynnosciString, SrwZlcSkladnikiString, SrwZlcUrzString);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Zapis danych z urządzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Aplikacja_Zapisz_Urzadzenia(Int32 idOperatora, String SrwUrzadzeniaString, String SrwUrzWlascString, String SrwUrzParDefString, string SrwUrzRodzajeString, string SrwUrzRodzParString)
        {
            DataBase db = new DataBase();
            db.Zapisz_Dane_Urzadzenia(idOperatora, SrwUrzadzeniaString, SrwUrzWlascString, SrwUrzParDefString, SrwUrzRodzajeString, SrwUrzRodzParString);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Synchronizacja cz.2 KntKarty")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_KntKartyLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<KntKarty> kntKartyList =  db.WS_KntKartyWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(kntKartyList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 KntKarty potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_KntKartyPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_KntKartyPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Synchronizacja cz.2 KntAdresy")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_KntAdresyLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<KntAdresy> kntAdresyList = db.WS_KntAdresyWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(kntAdresyList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 KntAdresy potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_KntAdresyPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_KntAdresyPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Synchronizacja cz.2 TwrKarty")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_TwrKartyLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<TwrKartyTable> TwrKartyList = db.WS_TwrKartyWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(TwrKartyList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 TwrKarty potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_TwrKartyPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_TwrKartyPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwZlcNag")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwZlcNagLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<SrwZlcNag> SrwZlcNagList = db.WS_SrwZlcNagWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwZlcNagList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwZlcNag potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwZlcNagPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_SrwZlcNagPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }


        [WebMethod(Description = "Synchronizacja cz.2 SrwZlcCzynnosci")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwZlcCzynnosciLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<SrwZlcCzynnosci> SrwZlcCzynnosciList = db.WS_SrwZlcCzynnosciWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwZlcCzynnosciList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwZlcCzynnosci potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwZlcCzynnosciPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_SrwZlcCzynnosciPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }


        [WebMethod(Description = "Synchronizacja cz.2 SrwZlcSkladniki")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwZlcSkladnikiLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<SrwZlcSkladniki> SrwZlcSkladnikiList = db.WS_SrwZlcSkladnikiWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwZlcSkladnikiList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwZlcSkladniki potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwZlcSkladnikiPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_SrwZlcSkladnikiPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }


        [WebMethod(Description = "Synchronizacja cz.2 SrwUrzadzenia")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwUrzadzeniaLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<SrwUrzadzenia> SrwUrzadzeniaList = db.WS_SrwUrzadzeniaWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwUrzadzeniaList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwUrzadzenia potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwUrzadzeniaPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_SrwUrzadzeniaPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwUrzWlasc")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwUrzWlascLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<SrwUrzWlasc> SrwUrzWlascList = db.WS_SrwUrzWlascWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwUrzWlascList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwUrzWlasc potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwUrzWlascPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_SrwUrzWlascPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwUrzParDef")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwUrzParDefLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<SrwUrzParDef> SrwUrzParDefList = db.WS_SrwUrzParDefWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwUrzParDefList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwUrzParDef potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwUrzParDefPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_SrwUrzParDefPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }



        
        [WebMethod(Description = "Synchronizacja cz.2 SrwUrzRodzaje")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwUrzRodzajeLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<SrwUrzRodzaje> SrwUrzRodzajeList = db.WS_SrwUrzRodzajeWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwUrzRodzajeList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwUrzRodzaje potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwUrzRodzajePotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_SrwUrzRodzajePotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }
        
        [WebMethod(Description = "Synchronizacja cz.2 SrwUrzRodzPar")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwUrzRodzParLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<SrwUrzRodzPar> SrwUrzRodzParList = db.WS_SrwUrzRodzParWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwUrzRodzParList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwUrzRodzPar potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwUrzRodzParPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_SrwUrzRodzParPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }
        
        [WebMethod(Description = "Synchronizacja cz.2 SrwZlcUrz")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwZlcUrzLista(Int32 idOperatora)
        {
            DataBase db = new DataBase();
            List<SrwZlcUrz> SrwZlcUrzList = db.WS_SrwZlcUrzWygenerujListe(idOperatora);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            JSS.MaxJsonLength = 50000000;

            return JSS.Serialize(SrwZlcUrzList);
        }

        [WebMethod(Description = "Synchronizacja cz.2 SrwZlcUrz potwierdz liste")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string WS_SrwZlcUrzPotwierdz(Int32 idOperatora, String jsonInput)
        {
            DataBase db = new DataBase();
            db.WS_SrwZlcUrzPotwierdz(idOperatora, jsonInput);

            JavaScriptSerializer JSS = new JavaScriptSerializer();
            return JSS.Serialize("OK");
        }
    }
}