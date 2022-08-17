using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace MOE.Common.Models
{
    public class GeneralSettings : ApplicationSettings
    {
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Display(Name = "ReCaptcha Public Key")]
        public string ReCaptchaPublicKey { get; set; }

        [Display(Name = "ReCaptcha Secret Key")]
        public string ReCaptchaSecretKey { get; set; }

        [Display(Name = "Raw Data Count Limit")]
        public int? RawDataCountLimit { get; set; }

        [Display(Name = "Seconds After Chart End Date to Complete Cycle")]
        public int? CycleCompletionSeconds { get; set; }
    }
}