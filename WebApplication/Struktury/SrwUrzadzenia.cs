using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class SrwUrzadzenia
    {
        public Int32 ID { get; set; }

        public Int32 SrU_Id { get; set; }

        public Int32 SrU_SURId { get; set; }

        public String SrU_Kod { get; set; }

        public String Sru_Nazwa { get; set; }

        public String SrU_Opis { get; set; }

        public Int32 SrU_Archiwalne { get; set; }
        
        public Int32 SrU_ToDo { get; set; }

        public SrwUrzadzenia(Int32 _SrU_Id, Int32 _SrU_SURId, String _SrU_Kod, String _Sru_Nazwa, String _SrU_Opis, Int32 _SrU_Archiwalne, Int32 _SrU_ToDo)
        {
            SrU_Id = _SrU_Id;
            SrU_SURId = _SrU_SURId;
            SrU_Kod = _SrU_Kod;
            Sru_Nazwa = _Sru_Nazwa;
            SrU_Opis = _SrU_Opis;
            SrU_Archiwalne = _SrU_Archiwalne;
            SrU_ToDo = _SrU_ToDo;
        }

        public SrwUrzadzenia() { }
    }
}