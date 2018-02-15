using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models
{

    public class GeneralSettings : ApplicationSettings
    {
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }
        [Display(Name = "Raw Data Count Limit")]
        public string RawDataCountLimit { get; set; }
    }
}
