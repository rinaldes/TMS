using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tms_mka_v2.Infrastructure
{
    public class FilterInfo
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public string Logic { get; set; }
        public List<FilterInfo> Filters { get; set; }

        public FilterInfo()
        {

        }

        public FilterInfo(string field, string operator_, string val)
        {
            this.Field = field;
            this.Operator = operator_;
            this.Value = val;
        }

        public FilterInfo(List<FilterInfo> list)
        {
            Filters = list;
        }

        public static FilterOperations ParseOperator(string theOperator)
        {
            switch (theOperator)
            {
                //equal ==
                case "eq":
                case "==":
                case "isequalto":
                case "equals":
                case "equalto":
                case "equal":
                    return FilterOperations.Equals;
                //not equal !=
                case "neq":
                case "!=":
                case "isnotequalto":
                case "notequals":
                case "notequalto":
                case "notequal":
                case "ne":
                    return FilterOperations.NotEquals;
                // Greater
                case "gt":
                case ">":
                case "isgreaterthan":
                case "greaterthan":
                case "greater":
                    return FilterOperations.Greater;
                // Greater or equal
                case "gte":
                case ">=":
                case "isgreaterthanorequalto":
                case "greaterthanequal":
                case "ge":
                    return FilterOperations.GreaterOrEquals;
                // Less
                case "lt":
                case "<":
                case "islessthan":
                case "lessthan":
                case "less":
                    return FilterOperations.LessThan;
                // Less or equal
                case "lte":
                case "<=":
                case "islessthanorequalto":
                case "lessthanequal":
                case "le":
                    return FilterOperations.LessThanOrEquals;
                case "startswith":
                    return FilterOperations.StartsWith;

                case "endswith":
                    return FilterOperations.EndsWith;
                //string.Contains()
                case "contains":
                    return FilterOperations.Contains;
                case "doesnotcontain":
                    return FilterOperations.NotContains;
                default:
                    return FilterOperations.Contains;
            }
        }

        public enum FilterOperations
        {
            Equals,
            NotEquals,
            Greater,
            GreaterOrEquals,
            LessThan,
            LessThanOrEquals,
            StartsWith,
            EndsWith,
            Contains,
            NotContains,
        }

        /**
         * mengubah sentence case menjadi underscore
         * mis: ContractorId -> contractor_id
         */
        //public void FormatFieldToUnderscore()
        //{
        //    if (this.Field != null)
        //        this.Field = System.Text.RegularExpressions.Regex.Replace(this.Field, @"(\p{Ll})(\p{Lu})", "$1_$2");

        //    if (this.Filters != null)
        //    {
        //        int i = 0;
        //        while (i < this.Filters.Count())
        //        {
        //            this.Filters[i].FormatFieldToUnderscore();
        //            ++i;
        //        }
        //    }
        //}

        /**
         * mengubah atribut field di dalam FilterInfo
         * field yang tadinya bernilai == original, diubah menjadi modification
         */
        public void ReplaceField(string original, string modification)
        {
            if (this.Field == original)
                this.Field = modification;

            if (this.Filters != null)
            {
                int i = 0;
                while (i < this.Filters.Count())
                {
                    this.Filters[i].ReplaceField(original, modification);
                    ++i;
                }
            }
        }

        /**
         * menghapus filter dengan field tertentu
         * batasan: belum rekursif
         * @return null kalau tidak ditemukan, atau nilai dari field tsb
         */
        public string RemoveFilterField(string field)
        {
            int initCount = Filters.Count;
            string val = Filters.Where(m => m.Field.ToLower() == field.ToLower()).Select(m => m.Value).FirstOrDefault();

            if (val != null)
            {
                Filters.RemoveAll(m => m.Field.ToLower() == field.ToLower());
            }

            return val;
        }

        public FilterInfo Clone()
        {
            FilterInfo clone = new FilterInfo { Field = this.Field, Logic = this.Logic, Operator = this.Operator, Value = this.Value };

            if (Filters != null)
            {
                clone.Filters = new List<FilterInfo>();
                foreach (var f in Filters)
                {
                    clone.Filters.Add(f.Clone());
                }
            }

            return clone;
        }
    }
}