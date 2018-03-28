using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MOE.Common.Models.ViewModel
{
    public class DateTimePickerViewModel
    {
        public DateTimePickerViewModel()
        {
            
        }


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

        public DateTime GetStartDateTime()
        {
            if (!String.IsNullOrEmpty(StartTime) && StartDateDay != null && !String.IsNullOrEmpty(StartTime) &&
                !String.IsNullOrEmpty(SelectedStartAMPM))
            {
                return Convert.ToDateTime(StartDateDay.ToShortDateString() + " " + StartTime + " " +
                                          SelectedStartAMPM);
            }
            else
            {
                throw new Exception("Missing Start Time Info");
            }
        }

        public DateTime GetEndDateTime()
        {
            if (!String.IsNullOrEmpty(EndTime) && EndDateDay != null && !String.IsNullOrEmpty(EndTime) &&
                !String.IsNullOrEmpty(SelectedEndAMPM))
            {
                return Convert.ToDateTime(EndDateDay.ToShortDateString() + " " + EndTime + " " +
                                          SelectedEndAMPM);
            }
            else
            {
                throw new Exception("Missing End Time Info");
            }
        }

        public DateTime GetEndDateTimePlusOneMinute()
        {
            return GetEndDateTime().AddMinutes(1);
        }

        public void SetDates()
        {
            StartDateDay = DateTime.Today.AddDays(-1);
            EndDateDay = DateTime.Today;
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

}