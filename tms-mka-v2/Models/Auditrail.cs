using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tms_mka_v2.Context;

namespace tms_mka_v2.Models
{
    public class Auditrail
    {
        public int Id { get; set; }
        public string EventDate { get; set; }
        public string Actionnya { get; set; }
        public int? IdUser { get; set; }
        public string NamaUser { get; set; }
        public string RemoteAddress { get; set; }
        public string Modulenya { get; set; }
        public string QueryDetail { get; set; }
        //public bool IsDelete { get; set; }
        
        public Auditrail()
        {

        }
        public Auditrail(Context.Auditrail dbitem)
        {
             Id = dbitem.Id;
             EventDate = dbitem.EventDate.ToString();
             Actionnya = dbitem.Actionnya;
             IdUser = dbitem.IdUser;
             if (dbitem.User != null)
                 NamaUser = dbitem.User.Fristname + " " + dbitem.User.Lastname;
             RemoteAddress = dbitem.RemoteAddress;
             QueryDetail = dbitem.QueryDetail;
             Modulenya = dbitem.Modulenya;
        }
        public void setDb(Context.Auditrail dbitem)
        {
            dbitem.Id = Id;
            dbitem.EventDate = DateTime.ParseExact(EventDate, "dd/MM/yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            dbitem.Actionnya = Actionnya;
            dbitem.IdUser = IdUser;
            dbitem.RemoteAddress = RemoteAddress;
            dbitem.QueryDetail = QueryDetail;
            dbitem.Modulenya = Modulenya;
        }
    }
}