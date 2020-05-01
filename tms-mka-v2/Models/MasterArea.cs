using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class MasterArea
    {
        public int Id { get; set; }
        public string Kode { get; set; }
        [Display(Name = "Nama")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        [RegularExpression(@"^[a-zA-Z0-9''-'\s]{1,40}$", ErrorMessage = "Input hanya alfanumeric.")]
        public string Nama { get; set; }
        //public bool IsDelete { get; set; }
        public string strArea { get; set; }
        public List<LocationArea> listArea { get; set; }

        public MasterArea()
        {
            listArea = new List<LocationArea>();
        }
        public MasterArea(Context.MasterArea dbitem)
        {
            Id = dbitem.Id;
            Kode = dbitem.Kode;
            Nama = dbitem.Nama;
            listArea = new List<LocationArea>();
            foreach (Context.ListLocationArea item in dbitem.ListLocationArea.ToList())
            {
                listArea.Add(new LocationArea(item));
            }
        }
        public void setDb(Context.MasterArea dbitem)
        {
            dbitem.Kode = Kode;
            dbitem.Nama = Nama;

            dbitem.ListLocationArea.Clear();
            
            LocationArea[] result = JsonConvert.DeserializeObject<LocationArea[]>(strArea);

            foreach (LocationArea item in result)
            {
                dbitem.ListLocationArea.Add(new Context.ListLocationArea(){
                    LocationId = item.idLoc
                });
            }

        }
    }

    public class LocationArea
    {
        public int idLoc { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public string parentLoc { get; set; }
        public string name { get; set; }
        public LocationArea()
        {

        }
        public LocationArea(Context.ListLocationArea dbitem)
        {
            idLoc = dbitem.LocationId.Value;
            code = dbitem.Location.Code;
            type = dbitem.Location.Type;
            name = dbitem.Location.Nama;
            if(dbitem.Location.LocationParent != null)
                parentLoc = dbitem.Location.LocationParent.Nama;

        }
    }
}