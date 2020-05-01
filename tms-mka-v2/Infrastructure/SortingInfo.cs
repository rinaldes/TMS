using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Infrastructure
{
    public class SortingInfo
    {
        public string SortOrder { get; set; }
        public string SortOn { get; set; }

        /**
         * mengubah sentence case menjadi underscore
         * mis: ContractorId -> contractor_id
         */
        public void FormatSortOnToUnderscore()
        {
            this.SortOn = System.Text.RegularExpressions.Regex.Replace(this.SortOn, @"(\p{Ll})(\p{Lu})", "$1_$2");
        }

        /**
         * mengubah kata original menjadi modification
         */
        public void ReplaceSortOn(string original, string modification)
        {
            if ((original != null) && (modification != null))
            {
                if (this.SortOn == original)
                    this.SortOn = modification;
            }
        }
    }
}