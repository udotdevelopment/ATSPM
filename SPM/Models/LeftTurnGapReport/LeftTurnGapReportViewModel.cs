using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOE.Common.Models.ViewModel.Chart;

namespace SPM.Models
{
    public class LeftTurnGapReportViewModel
    {

        public LeftTurnGapReportViewModel()
        {
            SignalSearch = new SignalSearchViewModel();
            SetDefaultDates();
            Directions = new List<CheckModel>
            {
                new CheckModel{Id=1, Name = "All Left Turns", Checked = true},
                new CheckModel{Id=1, Name = "NBL", Checked = true},
                new CheckModel{Id=1, Name = "SBL", Checked = true},
                new CheckModel{Id=1, Name = "EBL", Checked = true},
                new CheckModel{Id=1, Name = "WBL", Checked = true},
            };

            ReportInfo = new List<CheckModel>
            {
                new CheckModel{Id=1, Name = "Signal Data Check", Checked = true},
                new CheckModel{Id=1, Name = "Final Gap Analysis Report", Checked = true},
                new CheckModel{Id=1, Name = "Split Fail Analysis", Checked = false},
                new CheckModel{Id=1, Name = "Pedestrian Call Analysis", Checked = false},
                new CheckModel{Id=1, Name = "Conflicting Volumes Analysis", Checked = false},
            };
            CyclesWithPedCalls = 30;
            CyclesWithGapOuts = 50;
            LeftTurnVolume = 60;
            AcceptableGaps = 70;
            CyclesWithSplitFail = 50;
            LtCyclesWithPedCalls = 30;
            DateSelectorViewModel = new DateSelectorViewModel();
        }
        //public List<Models.Signal> Signals { get; set; }      
        public DateSelectorViewModel DateSelectorViewModel { get; set; }

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

        public List<CheckModel> Directions { get; set; }

        public List<CheckModel> ReportInfo { get; set; }
        [Display(Name = "% Cycles with Ped Calls")]
        public int CyclesWithPedCalls { get; set; }
        [Display(Name = "% Cycles with Gap Outs")]
        public int CyclesWithGapOuts { get; set; }
        [Display(Name = "Left-turn Volume (vph)")]
        public int LeftTurnVolume { get; set; }

        [Display(Name = "LT Vehicles/% Acceptable Gaps")]
        public int AcceptableGaps { get; set; }

        [Display(Name = "% Cycles with Split Failure")]
        public int CyclesWithSplitFail { get; set; }


        [Display(Name = "% Cycles with Ped Calls")]
        public int LtCyclesWithPedCalls { get; set; }

        protected void SetDefaultDates()
        {
            StartDateDay = Convert.ToDateTime("1/1/2020");// DateTime.Today;
            EndDateDay = Convert.ToDateTime("1/1/2020");// DateTime.Today;
            StartTime = "12:00";
            EndTime = "11:59";
            StartAMPMList = new List<SelectListItem>();
            StartAMPMList.Add(new SelectListItem { Value = "AM", Text = "AM", Selected = true });
            StartAMPMList.Add(new SelectListItem { Value = "PM", Text = "PM" });
            EndAMPMList = new List<SelectListItem>();
            EndAMPMList.Add(new SelectListItem { Value = "AM", Text = "AM" });
            EndAMPMList.Add(new SelectListItem { Value = "PM", Text = "PM", Selected = true });
        }
    }

    public class CheckModel
    {
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public bool Checked
        {
            get;
            set;
        }
    }
}