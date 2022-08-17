using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class MetricCommentRepository : IMetricCommentRepository
    {
        private readonly MOEContext _db;

        public MetricCommentRepository(MOEContext db)
        {
            this._db = db;
        }

        public MetricComment GetLatestCommentForReport(string signalID, int metricID)
        {
            var comments = (from r in _db.MetricComments
                            where r.Signal.SignalId == signalID
                            orderby r.TimeStamp descending
                            select r).ToList();
            var commentsForMetricType = new List<MetricComment>();
            if (comments != null)
                foreach (var mc in comments)
                    foreach (var mt in mc.MetricCommentMetricTypes)
                        if (mt.MetricTypeMetricId == metricID)
                        {
                            commentsForMetricType.Add(mc);
                            break;
                        }

            return commentsForMetricType.FirstOrDefault();
            //group r by r.CommentId into a
            //select a.OrderByDescending(g => g.TimeStamp).FirstOrDefault();
        }

        public List<MetricComment> GetAllMetricComments()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            var metricComments = (from r in _db.MetricComments
                                  select r).ToList();

            return metricComments;
        }

        public MetricComment GetMetricCommentByMetricCommentId(int metricCommentId)
        {
            var metricComment = from r in _db.MetricComments
                                where r.CommentId == metricCommentId
                                select r;

            return metricComment.FirstOrDefault();
        }

        public List<MetricType> GetMetricTypesByMetricComment(MetricComment metricComment)
        {

            throw new NotImplementedException();
            //var metricTypes = (from r in _db.MetricTypes
            //                   where metricComment.MetricTypeIDs.Contains(r.MetricID)
            //                   select r).ToList();

            //return metricTypes;
        }

        public void AddOrUpdate(MetricComment metricComment)
        {
            var g = (from r in _db.MetricComments
                     where r.CommentId == metricComment.CommentId
                     select r).FirstOrDefault();
            if (g != null)
                try
                {
                    _db.Entry(g).CurrentValues.SetValues(metricComment);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository = new ApplicationEventRepository(_db);
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.MetricCommentRepository";
                    error.Function = "AddOrUpdate";
                    error.Description = ex.Message;
                    error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
            else
            {
                _db.MetricComments.Add(metricComment);
                _db.SaveChanges();
            }
        }

        public void Remove(MetricComment metricComment)
        {
            var g = (from r in _db.MetricComments
                     where r.CommentId == metricComment.CommentId
                     select r).FirstOrDefault();
            if (g != null)
            {
                _db.MetricComments.Remove(g);
                _db.SaveChanges();
            }
        }

        public void Add(MetricComment metricComment)
        {
            throw new NotImplementedException();
            //var g = (from r in _db.MetricComments
            //         where r.CommentId == metricComment.CommentId
            //         select r).FirstOrDefault();
            //if (g == null)
            //{
            //    if (metricComment.MetricTypes == null)
            //        metricComment.MetricTypes = _db.MetricTypes
            //            .Where(x => metricComment.MetricTypeIDs.Contains(x.MetricID)).ToList();
            //    try
            //    {
            //        _db.MetricComments.Add(metricComment);
            //        _db.SaveChanges();
            //    }
            //    //catch (DbEntityValidationException e)
            //    //{
            //    //    var repository =
            //    //        ApplicationEventRepositoryFactory.Create();
            //    //    var errorMessage = string.Empty;
            //    //    foreach (var eve in e.EntityValidationErrors)
            //    //        foreach (var ve in eve.ValidationErrors)
            //    //            errorMessage += " Property:" + ve.PropertyName + " Error:" + ve.ErrorMessage;
            //    //    repository.QuickAdd("Moe.Common", "MetricCommentRepository", "Add",
            //    //        ApplicationEvent.SeverityLevels.Medium, errorMessage);
            //    //    throw new Exception(errorMessage);
            //    //}
            //    catch (Exception ex)
            //    {
            //        var repository = new ApplicationEventRepository(_db);
            //        repository.QuickAdd("Moe.Common", "MetricCommentRepository", "Add",
            //            ApplicationEvent.SeverityLevels.Medium, ex.Message);
            //        throw;
            //    }
            //}
            //else
            //{
            //    AddOrUpdate(metricComment);
            //}
        }

        public MetricComment GetMetricCommentByMetricCommentID(int metricCommentID)
        {
            throw new NotImplementedException();
        }
    }
}