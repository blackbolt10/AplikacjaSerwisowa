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

        [MaxLength(20), Unique]
        public String Knt_Akronim { get; set; }

        [MaxLength(50)]
        public String Knt_nazwa1 { get; set; }

        [MaxLength(50)]
        public String Knt_nazwa2 { get; set; }

        [MaxLength(250)]
        public String Knt_nazwa3 { get; set; }

        [MaxLength(10)]
        public String Knt_KodP { get; set; }

        [MaxLength(30)]
        public String Knt_miasto { get; set; }

        [MaxLength(30)]
        public String Knt_ulica { get; set; }

        [MaxLength(30)]
        public String Knt_Adres { get; set; }

        [MaxLength(13)]
        public String Knt_nip { get; set; }

        [MaxLength(30)]
        public String Knt_telefon1 { get; set; }

        [MaxLength(30)]
        public String Knt_telefon2 { get; set; }
        
        [MaxLength(30)]
        public String Knt_telex { get; set; }

        [MaxLength(30)]
        public String Knt_fax { get; set; }

        [MaxLength(255)]
        public String Knt_email { get; set; }

        [MaxLength(255)]
        public String Knt_url { get; set; }
    }
}