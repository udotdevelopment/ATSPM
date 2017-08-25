using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Models.Repositories
{
    public class ApproachRouteDetailRepository : IApproachRouteDetailRepository
    {
        MOE.Common.Models.SPM db = new SPM();

        public List<Models.ApproachRouteDetail> GetAllRoutesDetails()
        {
            List<Models.ApproachRouteDetail> routes = (from r in db.ApproachRouteDetails
                                                        select r).ToList();
            return routes;
        }

        public List<Models.ApproachRouteDetail> GetByRouteID(int routeID)
        {
            List<Models.ApproachRouteDetail> routes = (from r in db.ApproachRouteDetails
                                                 where r.ApproachRouteId == routeID
                                                 select r).ToList();

            if (routes.Count > 0)
            {
                return routes;
            }
            else
            {

                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                    error.Function = "GetByRouteID";
                    error.Description = "No ApproachRoute for ID.  Attempted ID# = " + routeID.ToString();
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw (new Exception("There is no ApproachRouteDetail for this ID"));
                }
            }
        }


        public void DeleteByRouteID(int routeID)
        {
            List<Models.ApproachRouteDetail> routes = (from r in db.ApproachRouteDetails
                                                       where r.ApproachRouteId == routeID
                                                       select r).ToList();

            try
            {
                db.ApproachRouteDetails.RemoveRange(routes);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                error.Function = "DeleteByRouteID";
                error.Description = ex.Message;
                error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw;
            }
        }

        public void UpdateByRouteAndApproachID(int routeID, int approachID, int newOrderNumber)
        {
            Models.ApproachRouteDetail RouteDetail = (from r in db.ApproachRouteDetails
                                                 where r.ApproachRouteId == routeID 
                                                 && r.ApproachID == approachID
                                                 select r).FirstOrDefault();
            if (RouteDetail != null)
            {
                Models.ApproachRouteDetail newRouteDetail = new Models.ApproachRouteDetail();
                newRouteDetail.ApproachOrder = newOrderNumber;

                try
                {
                    db.Entry(RouteDetail).CurrentValues.SetValues(newRouteDetail);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                            MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                    MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                    error.ApplicationName = "MOE.Common";
                    error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                    error.Function = "UpdateByRouteAndApproachID";
                    error.Description = ex.Message;
                    error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    error.Timestamp = DateTime.Now;
                    repository.Add(error);
                    throw;
                }

            }
        }
        public void Add(Models.ApproachRouteDetail newRouteDetail)
        {
            try
            {
                db.ApproachRouteDetails.Add(newRouteDetail);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MOE.Common.Models.Repositories.IApplicationEventRepository repository =
                        MOE.Common.Models.Repositories.ApplicationEventRepositoryFactory.Create();
                MOE.Common.Models.ApplicationEvent error = new ApplicationEvent();
                error.ApplicationName = "MOE.Common";
                error.Class = "Models.Repository.ApproachRouteDetailsRepository";
                error.Function = "UpdateByRouteAndApproachID";
                error.Description = ex.Message;
                error.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                error.Timestamp = DateTime.Now;
                repository.Add(error);
                throw;
            }
        }
    }
}

