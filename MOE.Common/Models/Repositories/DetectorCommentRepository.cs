using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace MOE.Common.Models.Repositories
{
    public class DetectorCommentRepository : IDetectorCommentRepository
    {
        private readonly SPM db = new SPM();


        public List<DetectorComment> GetAllDetectorComments()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var detectorComments = (from r in db.DetectorComments
                select r).ToList();

            return detectorComments;
        }

        public DetectorComment GetMostRecentDetectorCommentByDetectorID(int ID)
        {
            var comment = db.DetectorComments.Where(r => r.ID == ID).OrderBy(r => r.TimeStamp).FirstOrDefault();

            if (comment != null)
                return comment;
            return null;
        }

        public List<DetectorComment> GetDetectorsCommentsByDetectorID(int ID)
        {
            var comment = db.DetectorComments.Where(r => r.ID == ID).OrderBy(r => r.TimeStamp).ToList();

            if (comment != null)
                return comment;
            return null;
        }

        public DetectorComment GetDetectorCommentByDetectorCommentID(int detectorCommentID)
        {
            var detectorComment = from r in db.DetectorComments
                where r.CommentID == detectorCommentID
                select r;

            return detectorComment.FirstOrDefault();
        }

        public void AddOrUpdate(DetectorComment detectorComment)
        {
            var g = (from r in db.DetectorComments
                where r.CommentID == detectorComment.CommentID
                select r).FirstOrDefault();
            if (g != null)
                db.Entry(g).CurrentValues.SetValues(detectorComment);
            else
                db.DetectorComments.Add(detectorComment);

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }
        }

        public void Remove(DetectorComment detectorComment)
        {
            var g = (from r in db.DetectorComments
                where r.CommentID == detectorComment.CommentID
                select r).FirstOrDefault();
            if (g != null)
                try
                {
                    db.DetectorComments.Remove(g);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.DetectorCommentRepository";
                    error.Function = "Delete";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
        }

        public void Add(DetectorComment detectorComment)
        {
            var g = (from r in db.DetectorComments
                where r.CommentID == detectorComment.CommentID
                select r).FirstOrDefault();
            if (g == null)
                try
                {
                    db.DetectorComments.Add(detectorComment);
                    db.SaveChanges();
                }

                catch (Exception ex)
                {
                    var repository =
                        ApplicationEventRepositoryFactory.Create();
                    var error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.DetectorCommentRepository";
                    error.Function = "Add";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }
        }
    }
}