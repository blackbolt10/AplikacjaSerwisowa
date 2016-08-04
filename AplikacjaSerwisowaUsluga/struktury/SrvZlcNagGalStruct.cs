﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaSerwisowaUsluga
{
    public class SrwZlcNagGalStruct
    {
        public int Id { get; set; }
        public String Dokument { get; set; }
        public int SZN_Id { get; set; }
        public int SZN_KntTyp { get; set; }
        public int SZN_KntNumer { get; set; }
        public int SZN_KnATyp { get; set; }
        public int SZN_KnANumer { get; set; }
        public int SZN_KnDTyp { get; set; }
        public int SZN_KnDNumer { get; set; }
        public int SZN_AdWTyp { get; set; }
        public int SZN_AdWNumer { get; set; }
        public String SZN_DataWystawienia { get; set; }
        public String SZN_DataRozpoczecia { get; set; }
        public String SZN_Stan { get; set; }
        public String SZN_Status { get; set; }
        public String SZN_CechaOpis { get; set; }
        public String SZN_Opis { get; set; }
        public int SZN_Synchronizacja { get; set; }

        public SrwZlcNagGalStruct(String _Dokument, int _SZN_Id, int _SZN_KntTyp, int _SZN_KntNumer, int _SZN_KnATyp, int _SZN_KnANumer, int _SZN_KnDTyp, int _SZN_KnDNumer, int _SZN_AdWTyp, int _SZN_AdWNumer, String _SZN_DataWystawienia, String _SZN_DataRozpoczecia, String _SZN_Stan, String _SZN_Status, String _SZN_CechaOpis, String _SZN_Opis, int _SZN_Synchronizacja)
        {
            Dokument = _Dokument;
            SZN_Id = _SZN_Id;
            SZN_KntTyp = _SZN_KntTyp;
            SZN_KntNumer = _SZN_KntNumer;
            SZN_KnATyp = _SZN_KnATyp;
            SZN_KnANumer = _SZN_KnANumer;
            SZN_KnDTyp = _SZN_KnDTyp;
            SZN_KnDNumer = _SZN_KnDNumer;
            SZN_AdWTyp = _SZN_AdWTyp;
            SZN_AdWNumer = _SZN_AdWNumer;
            SZN_DataWystawienia = _SZN_DataWystawienia;
            SZN_DataRozpoczecia = _SZN_DataRozpoczecia;
            SZN_Stan = _SZN_Stan;
            SZN_Status = _SZN_Status;
            SZN_CechaOpis = _SZN_CechaOpis;
            SZN_Opis = _SZN_Opis;
            SZN_Synchronizacja = _SZN_Synchronizacja;
        }

        public SrwZlcNagGalStruct() { }
    }
}