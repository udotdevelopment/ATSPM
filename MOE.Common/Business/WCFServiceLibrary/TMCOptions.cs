using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using MOE.Common.Business;
using MOE.Common.Models;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class TMCOptions: MetricOptions
    {
        [Required]
        [DataMember]
        [Display(Name = "Volume Bin Size")]
        public int SelectedBinSize { get; set; }
        [DataMember]
        public List<int> BinSizeList { get; set; }

        [DataMember]
        [Display(Name = "Show MovementType Volume")]
        public bool ShowLaneVolumes { get; set; }
        [DataMember]
        [Display(Name = "Show Total Volume")]
        public bool ShowTotalVolumes { get; set; }
        [DataMember]
        [Display(Name = "Show Data Table")]
        public bool ShowDataTable { get; set; }

        private int MetricTypeID = 5;

        public TMC.TMCInfo TmcInfo;

        public TMCOptions(string signalID, DateTime startDate, DateTime endDate, double yAxisMax, double y2AxisMax,
            int binSize, bool showPlanStatistics, bool showVolumes, int metricTypeID, bool showLaneVolumes, bool showTotalVolumes)
        {
            SignalID = signalID;
            //StartDate = startDate;
            //EndDate = endDate;
            YAxisMax = yAxisMax;
            Y2AxisMax = y2AxisMax;
            SelectedBinSize = binSize;
            MetricTypeID = metricTypeID;
            ShowLaneVolumes = showLaneVolumes;
            ShowTotalVolumes = showTotalVolumes;
        }
        public TMCOptions()
        {
            BinSizeList = new List<int>();
            BinSizeList.Add(60);
            BinSizeList.Add(15);
            BinSizeList.Add(5);
            SelectedBinSize = 15;
            MetricTypeID = 5;
            SetDefaults();
        }
        public void SetDefaults()
        {
            Y2AxisMax = 300;
            YAxisMax = 1000;
            ShowLaneVolumes = true;
            ShowTotalVolumes = true;
            ShowDataTable = false;
        }

        public TMC.TMCInfo CreateMetric()
        {

            LogMetricRun();

            Models.Repositories.ISignalsRepository repository =
            Models.Repositories.SignalsRepositoryFactory.Create();
            Models.Signal signal = repository.GetVersionOfSignalByDate(SignalID, StartDate);
            TmcInfo = new TMC.TMCInfo();
            List<Plan> plans = PlanFactory.GetBasicPlans(StartDate, EndDate, SignalID);


            Models.Repositories.ILaneTypeRepository ltr = Models.Repositories.LaneTypeRepositoryFactory.Create();
            List<LaneType> laneTypes = ltr.GetAllLaneTypes();

            Models.Repositories.IMovementTypeRepository mtr = Models.Repositories.MovementTypeRepositoryFactory.Create();
            List<MovementType> movementTypes = mtr.GetAllMovementTypes();

            Models.Repositories.IDirectionTypeRepository dtr = Models.Repositories.DirectionTypeRepositoryFactory.Create();
            List<DirectionType> directions = dtr.GetAllDirections();
            

            CreateLaneTypeCharts(signal, "Vehicle", laneTypes, movementTypes, directions, plans, TmcInfo);
            CreateLaneTypeCharts(signal, "Exit", laneTypes, movementTypes, directions, plans, TmcInfo);
            CreateLaneTypeCharts(signal, "Bike", laneTypes, movementTypes, directions, plans, TmcInfo);


            return TmcInfo;
            
        }

        private void CreateLaneTypeCharts(Models.Signal signal, string laneTypeDescription, 
            List<LaneType> laneTypes, List<MovementType> movementTypes,
            List<DirectionType> directions, List<Plan> plans, TMC.TMCInfo tmcInfo)
        {
           

            foreach (DirectionType direction in directions)
            {
                 List<Approach> approaches = (from r in signal.Approaches
                              where r.DirectionType.DirectionTypeID == direction.DirectionTypeID
                              select r).ToList();

                 List<Models.Detector> DetectorsByDirection = new List<Models.Detector>();

                 foreach (Approach a in approaches)
                 {
                     foreach(Models.Detector d in a.Detectors)
                     {
                         if(d.DetectorSupportsThisMetric(5))
                         {
                             DetectorsByDirection.Add(d);
                         }
                     }
                 }


                //Loop through the major movement types
                 List<int> movementTypeIdsSorted = new List<int> { 3, 1, 2 };
                foreach(int x in movementTypeIdsSorted)
                {
                LaneType lanetype = (from r in laneTypes
                                                   where r.Description == laneTypeDescription
                                                  select r).FirstOrDefault();

                MovementType movementType = (from r in movementTypes
                                                           where r.MovementTypeID == x
                                                           select r).FirstOrDefault();

                List<Models.Detector> DetectorsForChart = (from r in DetectorsByDirection
                                                          where r.MovementType.MovementTypeID == movementType.MovementTypeID
                                                          && r.LaneType.LaneTypeID == lanetype.LaneTypeID
                                                          select r).ToList();

                    //movement type 1 is the thru movement.  We have to add the thru/turn lanes to the thru movment count.

                    if(x == 1)
                    {
                        List<Models.Detector> turnthrudetectors = (from r in DetectorsByDirection
                                                                   where (r.MovementType.MovementTypeID == 4 || r.MovementType.MovementTypeID == 5)
                                                                   && r.LaneType.LaneTypeID == lanetype.LaneTypeID
                                                                   select r).ToList();

                        if (turnthrudetectors != null && turnthrudetectors.Count > 0)
                        {
                            DetectorsForChart.AddRange(turnthrudetectors);
                        }
                    }

                if (DetectorsForChart.Count > 0)
                {


                    TMC.TMCMetric TMCchart = 
                        new TMC.TMCMetric(StartDate, EndDate, signal, direction, 
                            DetectorsForChart, lanetype, movementType, this, tmcInfo);
                    Chart chart = TMCchart.chart;
                    SetSimplePlanStrips(plans, chart, StartDate);
                    //Create the File Name

                    string chartName = CreateFileName();

                    //Save an image of the chart
                    chart.SaveImage(MetricFileLocation + chartName, ChartImageFormat.Jpeg);

                    //ReturnList.Add(MetricWebPath + chartName);
                   tmcInfo.ImageLocations.Add(MetricWebPath + chartName);
                    
                }
                        
                }
                
            }
        }




        private void SetSimplePlanStrips(List<Plan> plans, Chart chart, DateTime StartDate)
        {
            int backGroundColor = 1;
            foreach (MOE.Common.Business.Plan plan in plans)
            {
                StripLine stripline = new StripLine();
                //Creates alternating backcolor to distinguish the plans
                if (backGroundColor % 2 == 0)
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightGray);
                }
                else
                {
                    stripline.BackColor = Color.FromArgb(120, Color.LightBlue);
                }

                //Set the stripline properties
                stripline.IntervalOffsetType = DateTimeIntervalType.Hours;
                stripline.Interval = 1;
                stripline.IntervalOffset = (plan.StartTime - StartDate).TotalHours;
                stripline.StripWidth = (plan.EndTime - plan.StartTime).TotalHours;
                stripline.StripWidthType = DateTimeIntervalType.Hours;

                chart.ChartAreas["ChartArea1"].AxisX.StripLines.Add(stripline);

                //Add a corrisponding custom label for each strip
                CustomLabel Plannumberlabel = new CustomLabel();
                Plannumberlabel.FromPosition = plan.StartTime.ToOADate();
                Plannumberlabel.ToPosition = plan.EndTime.ToOADate();
                switch (plan.PlanNumber)
                {
                    case 254:
                        Plannumberlabel.Text = "Free";
                        break;
                    case 255:
                        Plannumberlabel.Text = "Flash";
                        break;
                    case 0:
                        Plannumberlabel.Text = "Unknown";
                        break;
                    default:
                        Plannumberlabel.Text = "Plan " + plan.PlanNumber.ToString();

                        break;
                }
                Plannumberlabel.LabelMark = LabelMarkStyle.LineSideMark;
                Plannumberlabel.ForeColor = Color.Black;
                Plannumberlabel.RowIndex = 6;


                chart.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(Plannumberlabel);


                backGroundColor++;

            }


        }
    }
}

