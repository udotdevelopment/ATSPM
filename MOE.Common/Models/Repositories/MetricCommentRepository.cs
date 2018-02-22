using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class MetricCommentRepository : IMetricCommentRepository
    {
        private readonly SPM db = new SPM();


        public MetricComment GetLatestCommentForReport(string signalID, int metricID)
        {
            var comments = (from r in db.MetricComments
                where r.Signal.SignalID == signalID
                orderby r.TimeStamp descending
                select r).ToList();
            var commentsForMetricType = new List<MetricComment>();
            if (comments != null)
                foreach (var mc in comments)
                foreach (var mt in mc.MetricTypes)
                    if (mt.MetricID == metricID)
                    {
                        commentsForMetricType.Add(mc);
                        break;
                    }

            return commentsForMetricType.FirstOrDefault();
            //group r by r.CommentID into a
            //select a.OrderByDescending(g => g.TimeStamp).FirstOrDefault();
        }

        public List<MetricComment> GetAllMetricComments()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var metricComments = (from r in db.MetricComments
                select r).ToList();

            return metricComments;
        }

        public MetricComment GetMetricCommentByMetricCommentID(int metricCommentID)
        {
            var metricComment = from r in db.MetricComments
                where r.CommentID == metricCommentID
                select r;

            return metricComment.FirstOrDefault();
        }

        public List<MetricType> GetMetricTypesByMetricComment(MetricComment metricComment)
        {
            var metricTypes = (from r in db.MetricTypes
                where metricComment.MetricTypeIDs.Contains(r.MetricID)
                select r).ToList();

            return metricTypes;
        }

        public void AddOrUpdate(MetricComment metricComment)
        {
            var g = (from r in db.MetricComments
                where r.CommentID == metricComment.CommentID
                select r).FirstOrDefault();
            if (g != null)
                try
                {
                    db.Entry(g).CurrentValues.SetValues(metricComment);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.MetricCommentRepository";
                    error.Function = "AddOrUpdate";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
            else
                db.MetricComments.Add(metricComment);
        }

        public void Remove(MetricComment metricComment)
        {
            var g = (from r in db.MetricComments
                where r.CommentID == metricComment.CommentID
                select r).FirstOrDefault();
            if (g != null)
            {
                db.MetricComments.Remove(g);
                db.SaveChanges();
            }
        }

        public void Add(MetricComment metricComment)
        {
            var g = (from r in db.MetricComments
                where r.CommentID == metricComment.CommentID
                select r).FirstOrDefault();
            if (g == null)
            {
                if (metricComment.MetricTypes == null)
                    metricComment.MetricTypes = db.MetricTypes
                        .Where(x => metricComment.MetricTypeIDs.Contains(x.MetricID)).ToList();
                try
                {
                    db.MetricComments.Add(metricComment);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var errorMessage = string.Empty;
                    foreach (var eve in e.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        errorMessage += " Property:" + ve.PropertyName + " Error:" + ve.ErrorMessage;
                    repository.QuickAdd("Moe.Common", "MetricCommentRepository", "Add",
                        ApplicationEvent.SeverityLevels.Medium, errorMessage);
                    throw new Exception(errorMessage);
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    repository.QuickAdd("Moe.Common", "MetricCommentRepository", "Add",
                        ApplicationEvent.SeverityLevels.Medium, ex.Message);
                    throw;
                }
            }
            else
            {
                AddOrUpdate(metricComment);
            }
        }
    }
}