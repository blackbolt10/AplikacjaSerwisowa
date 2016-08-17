using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class SrwZlcPodpisTable
    {
        public int Id { get; set; }

        public Int32 SZN_Id { get; set; }

        public Int32 SZP_Synchronizacja { get; set; }

        public String Podpis { get; set; }

        public String OsobaPodpisujaca { get; set; }


        public SrwZlcPodpisTable(Int32 _SZN_Id, Int32 _SZP_Synchronizacja, String _Podpis, String _OsobaPodpisujaca)
        {
            SZN_Id = _SZN_Id;
            SZP_Synchronizacja = _SZP_Synchronizacja;
            Podpis = _Podpis;
            OsobaPodpisujaca = _OsobaPodpisujaca;
        }

        public SrwZlcPodpisTable() {}
    }
}