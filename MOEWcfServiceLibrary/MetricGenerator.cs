using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MOEWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChartGenerator" in both code and config file together.
    public class MetricGenerator : IMetricGenerator
    {
        public List<String> CreateMetric(MOE.Common.Business.WCFServiceLibrary.MetricOptions options)
        {
            List<string> result = new List<string>();
            try
            {
                MOE.Common.Models.Repositories.IMetricTypeRepository metricTypeRepository =
                    MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
                options.MetricType = metricTypeRepository.GetMetricsByID(options.MetricTypeID);
                result = options.CreateMetric();
            }
            catch(Exception ex)
           {
                MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                e.ApplicationName = "MOEWCFServicLibrary";
                e.Class = this.GetType().ToString();
                e.Function = "CreateMetric";
                e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                e.Description = ex.Message + ex.InnerException;
                e.Timestamp = DateTime.Now;
                logRepository.Add(e);
                throw;
            }
            return result;
        }

        //public List<MOE.Common.Business.ApproachVolume.MetricInfo> CreateMetricWithDataTable(MOE.Common.Business.WCFServiceLibrary.ApproachVolumeOptions options)
        //{
        //    List<string> result = new List<string>();


        //    try
        //    {
        //        options.CreateMetric();
        //        return options.TmcInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
        //            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
        //        MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
        //        e.ApplicationName = "MOEWCFServicLibrary";
        //        e.Class = this.GetType().ToString();
        //        e.Function = "CreateMetric";
        //        e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
        //        e.Description = ex.Message;
        //        e.Timestamp = DateTime.Now;
        //        logRepository.Add(e);
        //        throw;
        //    }
        //    return options.MetricInfoList;
        //}

        public List<MOE.Common.Business.ApproachVolume.MetricInfo> CreateMetricWithDataTable(MOE.Common.Business.WCFServiceLibrary.ApproachVolumeOptions options)
        {
            List<string> result = new List<string>();


            try
            {
                MOE.Common.Models.Repositories.IMetricTypeRepository metricTypeRepository =
                    MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
                options.MetricType = metricTypeRepository.GetMetricsByID(options.MetricTypeID);
                result = options.CreateMetric();
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                e.ApplicationName = "MOEWCFServicLibrary";
                e.Class = this.GetType().ToString();
                e.Function = "CreateMetric";
                e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                e.Description = ex.Message;
                e.Timestamp = DateTime.Now;
                logRepository.Add(e);
                throw;
            }
            return options.MetricInfoList;
        }

        public MOE.Common.Business.TMC.TMCInfo CreateTMCChart(MOE.Common.Business.WCFServiceLibrary.TMCOptions options)
        {
            try
            {
                MOE.Common.Models.Repositories.IMetricTypeRepository metricTypeRepository =
                    MOE.Common.Models.Repositories.MetricTypeRepositoryFactory.Create();
                options.MetricType = metricTypeRepository.GetMetricsByID(options.MetricTypeID);
                options.CreateMetric();
                return options.TmcInfo;
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository logRepository =
                    MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent e = new MOE.Common.Models.ApplicationEvent();
                e.ApplicationName = "MOEWCFServicLibrary";
                e.Class = this.GetType().ToString();
                e.Function = "CreateMetric";
                e.SeverityLevel = MOE.Common.Models.ApplicationEvent.SeverityLevels.High;
                e.Description = ex.Message;
                e.Timestamp = DateTime.Now;
                logRepository.Add(e);
                throw;
            }
            //return options.MetricInfoList;
        }
        
    }
}
