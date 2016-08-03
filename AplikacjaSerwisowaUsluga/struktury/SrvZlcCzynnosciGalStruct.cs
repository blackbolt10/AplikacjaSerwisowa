using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaSerwisowaUsluga
{
    public class SrwZlcCzynnosciGalStruct
    {
        public int Id { get; set; }

        public Int32 szc_Id { get; set; }

        public Int32 szc_sznId { get; set; }

        public Int32 szc_Pozycja { get; set; }

        public Int32 szc_TwrNumer { get; set; }

        public Int32 szc_TwrTyp { get; set; }

        public Int32 SZC_Synchronizacja { get; set; }

        public double szc_Ilosc { get; set; }

        public String Twr_Jm { get; set; }

        public String szc_TwrNazwa { get; set; }

        public String Twr_Kod { get; set; }
    }
}
