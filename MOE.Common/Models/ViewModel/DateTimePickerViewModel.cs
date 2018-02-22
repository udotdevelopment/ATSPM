using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MOE.Common.Models.ViewModel
{
    public class DateTimePickerViewModel
    {
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


        public DateTime StartDateTime
        {
            get
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
        }

        public DateTime EndDateTime
        {
            get
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
        }
    }

}