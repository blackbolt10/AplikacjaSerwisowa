﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class SrwUrzRodzPar
    {
        public Int32 ID { get; set; }

        public Int32 SRP_Id { get; set; }

        public Int32 SRP_SURId { get; set; }

        public Int32 SRP_SUDId { get; set; }

        public Int32 SRP_Lp { get; set; }

        public Int32 SRP_ToDo { get; set; }


        public SrwUrzRodzPar(Int32 _SRP_Id, Int32 _SRP_SURId, Int32 _SRP_SUDId, Int32 _SRP_Lp, Int32 _SRP_ToDo)
        {
            SRP_Id = _SRP_Id;
            SRP_SURId = _SRP_SURId;
            SRP_SUDId = _SRP_SUDId;
            SRP_Lp = _SRP_Lp;
            SRP_ToDo = _SRP_ToDo;
        }

        public SrwUrzRodzPar() { }
    }
}