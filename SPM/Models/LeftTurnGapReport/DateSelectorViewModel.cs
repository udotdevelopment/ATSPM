using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MOE.Common.Models.ViewModel;

namespace SPM.Models
{
    public class DateSelectorViewModel
    {
        public DateSelectorViewModel()
        {
            AvailableDays = new List<Day>();
            AvailableDays.Add(new Day(0, "Sunday"));
            AvailableDays.Add(new Day(1, "Monday"));
            AvailableDays.Add(new Day(2, "Tuesday"));
            AvailableDays.Add(new Day(3, "Wednesday"));
            AvailableDays.Add(new Day(4, "Thursday"));
            AvailableDays.Add(new Day(5, "Friday"));
            AvailableDays.Add(new Day(6, "Saturday"));

            SelectedDays = new List<Day>();
            SelectedDays.Add(new Day(1, "Monday"));
            SelectedDays.Add(new Day(2, "Tuesday"));
            SelectedDays.Add(new Day(3, "Wednesday"));
            SelectedDays.Add(new Day(4, "Thursday"));
            SelectedDays.Add(new Day(5, "Friday"));
            StartDate = Convert.ToDateTime("1/1/2020");// DateTime.Today.AddDays(-1);
            EndDate = Convert.ToDateTime("1/1/2020");// DateTime.Today;
        }
        public string StartAMPM;
        public string EndAMPM;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }
        public int StartTimeHour { get; set; }
        public int StartTimeMinute { get; set; }
        public int EndTimeHour { get; set; }
        public int EndTimeMinute { get; set; }
        public List<Day> AvailableDays { get; set; }
        public List<Day> SelectedDays { get; set; }
        public PostedDays PostedDays { get; set; }
    }
}