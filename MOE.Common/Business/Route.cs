using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business
{
    public class Route
    {
        protected int id;
        public int ID
        {
            get
            {
                return id;
            }
        }

        protected String name;
        public String Name
        {
            get
            {
                return name;
            }
        }

        protected String description;
        public String Description
        {
            get
            {
                return description;
            }
        }

        protected int region;
        public int Region
        {
            get
            {
                return region;
            }
        }



        public List<MOE.Common.Business.Detector> MemberDetectors = new List<Detector>();
        public List<MOE.Common.Business.Detector> NonMemberDetectors = new List<Detector>();

        MOE.Common.Models.SPM db = new Models.SPM();

        



        public Route(int _id, string _name, string _description, int _region)
    {
        id = _id;
        name = _name;
        description = _description;
        region = _region;

    }
        public void Delete()
        {
        }

        public void Save()
        {
        }

        public void FillMembers()
        {
            //TODO: Ef Conversion (Executive Reports.  Pending)
           // this.MemberDetectors.Clear();

           // var routeMembersDT = from d in db.Detectors
           //                      join r in db.Route_Detectors on d.DetectorID equals r.DetectorID
           //                      where r.RouteID == this.ID

           //                      select new
           //                      {
           //                          DetectorID = d.DetectorID, 
           //                          SignalID = d.SignalID,
           //                          Det_Channel = d.Det_Channel,
           //                          Lane = d.Lane,
           //                          Direction = d.Direction,
           //                          Phase = d.Phase,
           //                          RouteOrder = r.RouteOrder };


           //// routeMembersTA.FillMembers(routeMembersDT, this.ID);

           // foreach (var row in routeMembersDT)
           // {
           //     MOE.Common.Business.Detector det = new Detector(row.DetectorID.ToString(), row.SignalID, row.Det_Channel, row.Lane, row.Direction, row.Phase, row.RouteOrder);
           //     MemberDetectors.Add(det);
           // }


        }

        public void FillNonMembers()
        {
            //TODO:EF Conversion (Executive Reports.  Pending)
        //    var routeNonMembersDT = (from e in db.Detectors
        //                              Conversion where e.Has_PCD == true
        //                         select e).Except(from d in db.Detectors
        //                        join r in db.Route_Detectors on d.DetectorID equals r.DetectorID
        //                        where r.RouteID == this.ID
        //                        select d);

        //    this.NonMemberDetectors.Clear();

        //    //routeMembersTA.FillNonMembers(routeMembersDT, this.ID);

        //    foreach (MOE.Common.Models.Detectors row in routeNonMembersDT)
        //    {
        //        MOE.Common.Business.Detector det = new Detector(row.DetectorID.ToString(), row.SignalID, row.Det_Channel, row.Lane, row.Direction, row.Phase, 0);
        //        NonMemberDetectors.Add(det);
        //    }
        }

    }
}
