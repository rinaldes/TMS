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
    public class RuteTol
    {
        public int Id { get; set; }
        public string strBerangkat { get; set; }
        public string strPulang { get; set; }
        //public bool IsDelete { get; set; }
        [Display(Name="Kode Rute")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public int? IdRute { get; set; }
        public string KodeRute { get; set; }
        public string NamaRute { get; set; }
        [Display(Name = "Nama Rute Tol")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.MyGlobalErrors))]
        public string NamaRuteTol { get; set; }
        public List<string> TolB { get; set; }
        public List<string> TolP { get; set; }
        public List<TolPP> ListTolBerangkat { get; set; }
        public List<TolPP> ListTolPulang { get; set; }
        public RuteTol()
        {
            ListTolBerangkat = new List<TolPP>();
            ListTolPulang = new List<TolPP>();
        }
        public RuteTol(Context.RuteTol dbitem)
        {
            Id = dbitem.Id;
            IdRute = dbitem.IdRute;
            KodeRute = dbitem.Rute.Kode;
            NamaRute = dbitem.Rute.Nama;
            NamaRuteTol = dbitem.NamaRuteTol;
            ListTolBerangkat = new List<TolPP>();
            ListTolPulang = new List<TolPP>();
            TolB = new List<string>();
            TolP = new List<string>(); 
         
            foreach (Context.TolBerangkat item in dbitem.ListTolBerangkat.ToList())
            {
                ListTolBerangkat.Add(new TolPP(item));
                TolB.Add(item.JnsTol.NamaTol);
            }

            strBerangkat = string.Join("-", TolB);

            foreach (Context.TolPulang item in dbitem.ListTolPulang.ToList())
            {
                ListTolPulang.Add(new TolPP(item));
                TolP.Add(item.JnsTol.NamaTol);
            }

            strPulang = string.Join("-", TolP);

        }
        public void setDb(Context.RuteTol dbitem)
        {
            dbitem.Id = Id;
            dbitem.IdRute = IdRute;
            dbitem.NamaRuteTol = NamaRuteTol;

            dbitem.ListTolBerangkat.Clear();
            dbitem.ListTolPulang.Clear();

            TolPP[] resultBerangkat = JsonConvert.DeserializeObject<TolPP[]>(strBerangkat);
            TolPP[] resultPulang = JsonConvert.DeserializeObject<TolPP[]>(strPulang);

            foreach (TolPP item in resultBerangkat)
            {
                dbitem.ListTolBerangkat.Add(new Context.TolBerangkat()
                {
                    IdTol = item.IdTol,
                    IdRuteTol = item.IdRuteTol
                });
            }

            foreach (TolPP item in resultPulang)
            {
                dbitem.ListTolPulang.Add(new Context.TolPulang()
                {
                    IdTol = item.IdTol,
                    IdRuteTol = item.IdRuteTol
                });
            }
        }
    }
    public class TolPP
    {
        public int Id { get; set; }
        public int? IdTol { get; set; }
        public string NamaTol { get; set; }
        public int IdRuteTol { get; set; }
        
        public TolPP()
        {

        }

        public TolPP(Context.TolBerangkat dbitem)
        {
            Id = dbitem.Id;
            IdTol = dbitem.IdTol;
            NamaTol = dbitem.JnsTol.NamaTol;
            IdRuteTol = dbitem.IdRuteTol;
        }
        public TolPP(Context.TolPulang dbitem)
        {
            Id = dbitem.Id;
            IdTol = dbitem.IdTol;
            NamaTol = dbitem.JnsTol.NamaTol;
            IdRuteTol = dbitem.IdRuteTol;
        }
    }
}