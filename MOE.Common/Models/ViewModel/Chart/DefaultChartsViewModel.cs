using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MOE.Common.Models.ViewModel.Chart
{
    public class DefaultChartsViewModel
    {
        public DefaultChartsViewModel()
        {
            SignalSearch = new SignalSearchViewModel();
            SetDefaultDates();
        }
        //public List<Models.Signal> Signals { get; set; }       

        [Display(Name = "Y-axis Max")]
        public double? YAxisMax { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDateDay { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [Required]
        [Display(Name = "Start AM/PM")]
        public string SelectedStartAMPM { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime EndDateDay { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public string EndTime { get; set; }

        [Required]
        [Display(Name = "End AM/PM")]
        public string SelectedEndAMPM { get; set; }

        public List<SelectListItem> StartAMPMList { get; set; }
        public List<SelectListItem> EndAMPMList { get; set; }
        public List<string> ImageLocation { get; set; }
        public SignalSearchViewModel SignalSearch { get; set; }

        [AllowHtml]
        public string RunMetricJavascript { get; set; } = string.Empty;

        protected void SetDefaultDates()
        {
            StartDateDay = DateTime.Today;
            EndDateDay = DateTime.Today;
            StartTime = "12:00";
            EndTime = "11:59";
            //StartDateDay = new DateTime(2018, 11, 29);
            //EndDateDay = new DateTime(2018, 11, 29);
            //StartTime = "11:30";
            //EndTime = "1:00";
            StartAMPMList = new List<SelectListItem>();
            StartAMPMList.Add(new SelectListItem {Value = "AM", Text = "AM", Selected = true});
            StartAMPMList.Add(new SelectListItem {Value = "PM", Text = "PM"});
            EndAMPMList = new List<SelectListItem>();
            EndAMPMList.Add(new SelectListItem {Value = "AM", Text = "AM"});
            EndAMPMList.Add(new SelectListItem {Value = "PM", Text = "PM", Selected = true});
        }
    }
}