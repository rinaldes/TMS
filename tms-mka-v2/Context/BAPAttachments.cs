﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class BAPAttachments
    {
        public BAPAttachments()
        {

        }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("BAP")]
        public int IdBAP { get; set; }
        public string Url { get; set; }
        public string Filename { get; set; }
        public string Realfname { get; set; }

        public virtual BAP BAP { get; set; }
    }
}