using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("SrwZlcSkladnikiTable")]
    public class SrwZlcSkladnikiTable
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }

        [Unique]
        public Int32 szs_Id { get; set; }
        
        public Int32 szs_sznId { get; set; }
        
        public Int32 szs_Pozycja { get; set; }
        
        public Int32 szs_TwrNumer { get; set; }

        public Int32 szs_TwrTyp { get; set; }

        public double szs_Ilosc { get; set; }

        public String Twr_Jm { get; set; }

        public String szs_TwrNazwa { get; set; }
        
        public String Twr_Kod { get; set; }
    }
}