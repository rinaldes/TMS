using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Context
{
    public class MasterArea
    {
        public MasterArea()
        {
            this.ListLocationArea = new HashSet<ListLocationArea>();
        }
        [Key]
        public int Id { get; set; }
        public string Kode { get; set; }
        public string Nama { get; set; }
        //public bool IsDelete { get; set; }
        public int Urutan { get; set; }
        public virtual ICollection<ListLocationArea> ListLocationArea { get; set; }
    }
}