using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("TwrKartyTable")]
    public class TwrKartyTable
    {
        [Column("_Id")]
        public int Id { get; set; }

        [PrimaryKey, Unique]
        public Int32 Twr_GIDNumer { get; set; }

        public Int32 Twr_GIDTyp { get; set; }
        
        public String Twr_Kod { get; set; }

        public Int32 Twr_Typ { get; set; }

        public Double Ilosc { get; set; }

        public String Twr_Nazwa { get; set; }
        
        public String Twr_Nazwa1 { get; set; }

        public String Twr_Jm { get; set; }

        public Int32 Twr_ToDo { get; set; }

        public Boolean zaznaczone { get; set; }

    }
}