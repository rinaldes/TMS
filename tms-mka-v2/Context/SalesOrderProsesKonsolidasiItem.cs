﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class SalesOrderProsesKonsolidasiItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("SalesOrderProsesKonsolidasi")]
        public int SalesOrderProsesKonsolidasiId { get; set; }
        [ForeignKey("SalesOrderKonsolidasi")]
        public int SalesOrderKonsolidasiId { get; set; }

        public virtual SalesOrderProsesKonsolidasi SalesOrderProsesKonsolidasi { get; set; }
        public virtual SalesOrderKonsolidasi SalesOrderKonsolidasi { get; set; }
    }
}