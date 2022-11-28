using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GSID.Admin.Models
{
    public class Language
    {
        public enum LanguagueCountry : int
        {
            None = -1,
            Vietmamese = 1,
            English = 2
        }

        public string Name { get; set; }
        public string Parent { get; set;}
        public string Url{ get; set; }
        public bool Current { get; set; }
        public bool Default { get; set; }
        public LanguagueCountry Country { get; set; }
    }
    public class LanguagueCountry
    { 
    }
}