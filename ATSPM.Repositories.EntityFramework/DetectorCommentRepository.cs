using ATSPM.IRepositories;
using ATSPM.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATSPM.Infrastructure.Repositories.EntityFramework
{
    public class DetectorCommentRepository : IDetectorCommentRepository
    {
        private readonly MOEContext db;

        public DetectorCommentRepository(MOEContext db)
        {
            this.db = db;
        }

        public List<DetectorComment> GetAllDetectorComments()
        {
            //db.Configuration.LazyLoadingEnabled = false;
            var detectorComments = (from r in db.DetectorComments
                                    select r).ToList();

            return detectorComments;
        }

        public DetectorComment GetMostRecentDetectorCommentByDetectorID(int Id)
        {
            var comment = db.DetectorComments.Where(r => r.Id == Id).OrderBy(r => r.TimeStamp).FirstOrDefault();

            if (comment != null)
                return comment;
            return null;
        }

        public DetectorComment GetDetectorCommentByDetectorCommentID(int detectorCommentID)
        {
            var detectorComment = from r in db.DetectorComments
                                  where r.CommentId == detectorCommentID
                                  select r;

            return detectorComment.FirstOrDefault();
        }

        public void AddOrUpdate(DetectorComment detectorComment)
        {
            var g = (from r in db.DetectorComments
                     where r.CommentId == detectorComment.CommentId
                     select r).FirstOrDefault();
            if (g != null)
                db.Entry(g).CurrentValues.SetValues(detectorComment);
            else
                db.DetectorComments.Add(detectorComment);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //            ve.PropertyName, ve.ErrorMessage);
                //}
                throw;
            }
        }

        public void Remove(DetectorComment detectorComment)
        {
            var g = (from r in db.DetectorComments
                     where r.CommentId == detectorComment.CommentId
                     select r).FirstOrDefault();
            if (g != null)
                try
                {
                    db.DetectorComments.Remove(g);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository = new ApplicationEventRepository(db);
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.DetectorCommentRepository";
                    error.Function = "Delete";
                    error.Description = ex.Message;
                    error.SeverityLevel =(int)ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
        }

        public void Add(DetectorComment detectorComment)
        {
            var g = (from r in db.DetectorComments
                     where r.CommentId == detectorComment.CommentId
                     select r).FirstOrDefault();
            if (g == null)
                try
                {
                    db.DetectorComments.Add(detectorComment);
                    db.SaveChanges();
                }

                catch (Exception ex)
                {

                    var logRepository = new ApplicationEventRepository(db);
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.DetectorCommentRepository";
                    error.Function = "Add";
                    error.Description = ex.Message;
                    error.SeverityLevel = (int)ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    logRepository.Add(error);
                    throw;
                }
        }
    }
}