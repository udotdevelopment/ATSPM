using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MOE.Common.Business;

namespace MOE.Common.Models.ViewModel
{
    public class LinkPivotViewModel
    {
        private SPM db = new SPM();

        public LinkPivotViewModel()
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
        }

        public int LinkPivotViewModelId { get; set; }
        public List<Route> Routes { get; set; }

        [Required(ErrorMessage = "Starting Point is required")]
        [Display(Name = "Starting Point")]
        public string StartingPoint { get; set; }

        [Required(ErrorMessage = "Route is required")]
        public int SelectedRouteId { get; set; }

        public int Bias { get; set; }

        [Required(ErrorMessage = "Bias Direction is required")]
        public string BiasUpDownStream { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public string EndTime { get; set; }

        [Required]
        [Display(Name = "Start AM/PM")]
        public string StartAMPM { get; set; }

        [Required]
        [Display(Name = "Start AM/PM")]
        public string EndAMPM { get; set; }

        //These parameters are used when a user selects a row on the table
        public string SelectedSignalId { get; set; }

        public string SelectedDownSignalId { get; set; }
        public string SelectedUpstreamDirection { get; set; }
        public string SelectedDownstreamDirection { get; set; }
        public int SelectedDelta { get; set; }

        public List<Day> AvailableDays { get; set; }

        public List<Day> SelectedDays { get; set; }

        [Required]
        public PostedDays PostedDays { get; set; }

        [Required(ErrorMessage = "Cycle Length is required")]
        [Display(Name = "Cycle Length")]
        public int CycleLength { get; set; }

        public List<DateTime> GetDays()
        {
            var daysList = new List<DayOfWeek>();

            if (PostedDays.DayIDs.Contains("0"))
                daysList.Add(DayOfWeek.Sunday);
            if (PostedDays.DayIDs.Contains("1"))
                daysList.Add(DayOfWeek.Monday);
            if (PostedDays.DayIDs.Contains("2"))
                daysList.Add(DayOfWeek.Tuesday);
            if (PostedDays.DayIDs.Contains("3"))
                daysList.Add(DayOfWeek.Wednesday);
            if (PostedDays.DayIDs.Contains("4"))
                daysList.Add(DayOfWeek.Thursday);
            if (PostedDays.DayIDs.Contains("5"))
                daysList.Add(DayOfWeek.Friday);
            if (PostedDays.DayIDs.Contains("6"))
                daysList.Add(DayOfWeek.Saturday);

            var dates = LinkPivot.GetDates(StartDate.Value, EndDate.Value, daysList);
            return dates;
        }
    }

    public class PostedDays
    {
        public PostedDays()
        {
        }

        public PostedDays(string[] dayIds)
        {
            DayIDs = dayIds;
        }

        public string[] DayIDs { get; set; }
    }
}