using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerAttachment
    {
        public CustomerAttachment()
        {

        }

        [Key]
        [Column(Order=0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public string url { get; set; }
        public string filename { get; set; }
        public string realfname { get; set; }
        public virtual Customer Customer { get; set; }
    }
}