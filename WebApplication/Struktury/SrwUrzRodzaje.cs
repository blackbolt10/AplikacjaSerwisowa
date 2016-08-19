using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class SrwUrzRodzaje
    {
        public Int32 ID { get; set; }

        public Int32 SUR_Id { get; set; }

        public String SUR_Kod { get; set; }

        public String SUR_Nazwa { get; set; }
        

        public SrwUrzRodzaje(Int32 _SUR_Id, String _SUR_Kod, String _SUR_Nazwa)
        {
            SUR_Id = _SUR_Id;
            SUR_Kod = _SUR_Kod;
            SUR_Nazwa = _SUR_Nazwa;
        }

        public SrwUrzRodzaje() { }
    }
}