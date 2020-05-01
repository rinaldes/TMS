using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class CustomerPic
    {
        public CustomerPic()
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
        public string Code { get; set; }
        public string Name { get; set; }
        [ForeignKey("LookUpCodesDept")]
        public int? DepartemenId { get; set; }
        [ForeignKey("LookUpCodesJabatan")]
        public int? JabatanId { get; set; }
        public string EmailAdd { get; set; }
        public string Mobile { get; set; }
        public int Urutan { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual LookupCode LookUpCodesDept { get; set; }
        public virtual LookupCode LookUpCodesJabatan { get; set; }
    }
}