using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Models
{
    public class ResponeModel
    {
        public string Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public ResponeModel() { }
        public ResponeModel(bool success)
        {
            Success = success;
        }
        public void SetFail(string message)
        {
            Success = false;
            Message = message;
        }
    }
}