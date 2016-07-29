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

        public String Podpis { get; set; }
    }
}