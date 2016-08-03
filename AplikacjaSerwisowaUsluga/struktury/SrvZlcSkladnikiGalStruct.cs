using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaSerwisowaUsluga
{
    public class SrwZlcSkladnikiGalStruct
    {
        public int Id { get; set; }

        public Int32 szs_Id { get; set; }

        public Int32 szs_sznId { get; set; }

        public Int32 szs_Pozycja { get; set; }

        public Int32 szs_TwrNumer { get; set; }

        public Int32 szs_TwrTyp { get; set; }

        public Int32 SZS_Synchronizacja { get; set; }

        public double szs_Ilosc { get; set; }

        public String Twr_Jm { get; set; }

        public String szs_TwrNazwa { get; set; }

        public String Twr_Kod { get; set; }
    }
}
