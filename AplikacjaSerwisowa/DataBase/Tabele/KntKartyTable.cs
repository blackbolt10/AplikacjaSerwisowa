using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("KntKartyTable")]
    public class KntKartyTable
    {
        [Column("Id")]
        public int Id { get; set; }

        [PrimaryKey, Unique]
        public Int32 Knt_GIDNumer { get; set; }
        
        public String Knt_Akronim { get; set; }
        
        public String Knt_nazwa1 { get; set; }
        
        public String Knt_nazwa2 { get; set; }
        
        public String Knt_nazwa3 { get; set; }
        
        public String Knt_KodP { get; set; }
        
        public String Knt_miasto { get; set; }
        
        public String Knt_ulica { get; set; }
        
        public String Knt_Adres { get; set; }
        
        public String Knt_nip { get; set; }
        
        public String Knt_telefon1 { get; set; }
        
        public String Knt_telefon2 { get; set; }
        
        public String Knt_telex { get; set; }
        
        public String Knt_fax { get; set; }
        
        public String Knt_email { get; set; }
        
        public String Knt_url { get; set; }
    }
}