using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("SrwZlcCzynnosciTable")]
    public class SrwZlcCzynnosciTable
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }

        [Unique]
        public Int32 szc_Id { get; set; }
        
        public Int32 szc_sznId { get; set; }
        
        public Int32 szc_Pozycja { get; set; }
        
        public Int32 szc_TwrNumer { get; set; }
        public Int32 szc_TwrTyp { get; set; }

        public double szc_Ilosc { get; set; }

        public String Twr_Jm { get; set; }

        public String szc_TwrNazwa { get; set; }
        
        public String Twr_Kod { get; set; }
    }
}