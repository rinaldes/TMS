using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class UnitList
    {
        public int Id { get; set; }
        [Display(Name = "Vehicle No")]
        public string VehicleNo { get; set; }
        [Display(Name = "Jenis Truck")]
        public string JenisTruck { get; set; }
        [Display(Name = "Merk Truck")]
        public string MerkTruck { get; set; }
        [Display(Name = "Model Truck")]
        public string ModelTruck { get; set; }
        public string MerkPendingin { get; set; }
        public string ModelPendingin { get; set; }
        public string KaroseriBox { get; set; }
        public string TypeBox { get; set; }
        public string VendorGps { get; set; }
        public UnitList(Context.DataTruck dbitem)
        {
            Id = dbitem.Id;
            VehicleNo = dbitem.VehicleNo;
            JenisTruck = dbitem.IdJenisTruck.HasValue ? dbitem.JenisTrucks.StrJenisTruck : "";
            MerkTruck = dbitem.IdMerk.HasValue ? dbitem.LookupCodeMerk.Nama : "";
            ModelTruck = dbitem.SpecModel;
            MerkPendingin = dbitem.DataPendingin.Count() == 0 ? "" : dbitem.DataPendingin.FirstOrDefault().Merk;
            ModelPendingin = dbitem.DataPendingin.Count() == 0 ? "" : dbitem.DataPendingin.FirstOrDefault().Model;
            KaroseriBox = dbitem.DataBox.Count() == 0 ? "" : dbitem.DataBox.FirstOrDefault().Karoseri;
            TypeBox = dbitem.DataBox.Count() == 0 ? "" : dbitem.DataBox.FirstOrDefault().IdType.HasValue ? dbitem.DataBox.FirstOrDefault().LookupCodeType.Nama : "";
            VendorGps = dbitem.DataGPS.Count() == 0 ? "" : dbitem.DataGPS.FirstOrDefault().IdVendor.HasValue ? dbitem.DataGPS.FirstOrDefault().VendorGps.Nama : "";
        }
    }
}