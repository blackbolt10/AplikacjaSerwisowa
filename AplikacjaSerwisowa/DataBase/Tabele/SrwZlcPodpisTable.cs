using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("SrwZlcPodpisTable")]
    public class SrwZlcPodpisTable
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }

        [Unique]
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

        public SrwZlcPodpisTable() { }
    }
}