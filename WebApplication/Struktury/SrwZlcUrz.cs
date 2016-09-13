﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class SrwZlcUrz
    {
        public Int32 ID { get; set; }
        public Int32 SZU_Id { get; set; }
        public Int32 SZU_SZNId { get; set; }
        public Int32 SZU_SrUId { get; set; }
        public Int32 SZU_Pozycja { get; set; }
        public Int32 SZU_ToDo { get; set; }


        public SrwZlcUrz(Int32 _SZU_Id, Int32 _SZU_SZNId, Int32 _SZU_SrUId, Int32 _SZU_Pozycja, Int32 _SZU_ToDo)
        {
            SZU_Id = _SZU_Id;
            SZU_SZNId = _SZU_SZNId;
            SZU_SrUId = _SZU_SrUId;
            SZU_Pozycja = _SZU_Pozycja;
            SZU_ToDo = _SZU_ToDo;
        }

        public SrwZlcUrz() { }
    }
}