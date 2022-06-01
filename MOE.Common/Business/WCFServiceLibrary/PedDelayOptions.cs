﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MOE.Common.Business.PEDDelay;
using System.ComponentModel.DataAnnotations;

namespace MOE.Common.Business.WCFServiceLibrary
{
    [DataContract]
    public class PedDelayOptions : MetricOptions
    {
        public PedDelayOptions(string signalId, DateTime startDate, DateTime endDate, int timeBuffer, double? yAxisMax)
        {
            SignalID = signalId;
            StartDate = startDate;
            EndDate = endDate;
            TimeBuffer = timeBuffer;
            YAxisMax = yAxisMax;
        }

        public PedDelayOptions()
        {
            Y2AxisMax = 10;
            SetDefaults();
        }

        [DataMember]
        [Display(Name = "Time Buffer Between Unique Ped Detections")]
        public int TimeBuffer { get; set; }

        public override List<string> CreateMetric()
        {
            base.CreateMetric();
            var signalRepository = Models.Repositories.SignalsRepositoryFactory.Create();
            Models.Signal signal= signalRepository.GetVersionOfSignalByDate(SignalID, StartDate);

            var pds = new PedDelaySignal(signal, TimeBuffer, StartDate, EndDate);
            foreach (var p in pds.PedPhases)
                if (p.Cycles.Count > 0)
                {
                    var pdc = new PEDDelayChart(this, p);
                    var chart = pdc.Chart;
                    var chartName = CreateFileName();
                    chart.SaveImage(MetricFileLocation + chartName);
                    ReturnList.Add(MetricWebPath + chartName);
                }
            return ReturnList;
        }
    }
}