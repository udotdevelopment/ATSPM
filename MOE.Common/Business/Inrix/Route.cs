using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.Inrix
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
            set
            {
                id = value;
            }
        }

        protected string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        protected string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public SortedDictionary<DateTime, int> TravelTimes = new SortedDictionary<DateTime,int>();

        public List<Segment> Items = new List<Segment>();
 
        //MOE.Common.Data.InrixTableAdapters.RoutesTableAdapter routesTA = new MOE.Common.Data.InrixTableAdapters.RoutesTableAdapter();
        //MOE.Common.Data.InrixTableAdapters.SegmentsTableAdapter segmentsTA = new Data.InrixTableAdapters.SegmentsTableAdapter();
        //MOE.Common.Data.InrixTableAdapters.GroupsTableAdapter groupsTA = new Data.InrixTableAdapters.GroupsTableAdapter();

       /// <summary>
       /// Default Constructor for the Route Class
       /// </summary>
       /// <param name="id"></param>
       /// <param name="name"></param>
       /// <param name="description"></param>
        public Route(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;

            FillMembers();
         
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="routeCopy"></param>
        public Route(MOE.Common.Business.Inrix.Route routeCopy)
        {
            Models.Inrix.Repositories.RouteRepository rr = new Models.Inrix.Repositories.RouteRepository();

            int tempID = 0;
            Name = "Copy of " + routeCopy.Name;
            Description = routeCopy.Description;
            //routesTA.Insert(this.Name, this.Description, out tempID);

            Models.Inrix.Route route = new Models.Inrix.Route();
            route.Route_Name = this.Name;
            route.Route_Description = this.Description;

            rr.Add(route);


            Models.Inrix.Route copiedRoute = rr.GetRouteByName(this.name);
            ID = Convert.ToInt32(copiedRoute.Route_ID);


            foreach (Segment segment in routeCopy.Items)
            {
                this.Items.Add(segment);
            }

            this.SaveMembers();
            

        }
        

        public void FillMembers()
        {
            this.Items.Clear();
            List<Models.Inrix.Segment> segmentsDT = new List<Models.Inrix.Segment>();
            using (Models.Inrix.Inrix db = new Models.Inrix.Inrix())
            {
                segmentsDT = (from r in db.Segments
                               select r).ToList();

            }
            //MOE.Common.Data.Inrix.SegmentsDataTable segmentsDT = segmentsTA.GetDataByRouteMembers(this.ID);

                       
            //MOE.Common.Data.Inrix.SegmentsRow segRow;
            foreach (Models.Inrix.Segment row in segmentsDT)
            {

                    Segment segment = new Segment(row.Segment_ID, row.Segment_Name, row.Segment_Description);
                    this.Items.Add(segment);

            }

        }

        public void FillNonMembers()
        {
            this.Items.Clear();
            List<Models.Inrix.Segment> segmentsDT = new List<Models.Inrix.Segment>();
            using(Models.Inrix.Inrix db = new Models.Inrix.Inrix())
            {
                segmentsDT = ((from r in db.Segments
                                 select r).Except(from q in db.Segments where q.Segment_ID == this.ID select q)).ToList();
                                
            }
           // MOE.Common.Data.Inrix.SegmentsDataTable segmentsDT = segmentsTA.GetDataByRouteNonMember(this.ID);


            //MOE.Common.Data.Inrix.SegmentsRow segRow;
            foreach (Models.Inrix.Segment row in segmentsDT)
            {

                Segment segment = new Segment(row.Segment_ID, row.Segment_Name, row.Segment_Description);
                this.Items.Add(segment);

            }

        }
        
        public void AddMember(Segment segment)
        {
            this.Items.Add(segment);
        }

        public void RemoveMember(Segment segment)
        {
            this.Items.Remove(segment);
        }


        public void DeleteRoute()
        {
            ////Delete the route from the Groups Members
            //routesTA.DeleteRouteFromRouteMembers(this.ID);
            ////delete the  route from the Route Members table
            //groupsTA.DeleteRouteFromAllGroups(this.ID);
            ////delete the route from the routes table
            //routesTA.Delete(this.ID, this.Name);
            
            
            
            
        }

        public void SaveMembers()
        {
            ////remove the old route members from the DB.
            //routesTA.DeleteRouteFromRouteMembers(this.ID);
            //int x = 0;
            ////add new segmetns to DB in order
            //foreach (Segment segment in this.Items)
            //{     
            //    x++;
            //   routesTA.InsertRouteMembers(this.ID, segment.ID, x);
            //}
        }

        public static void InsertRoute(string RouteName, string RouteDescription)
        {
            //int routeID = 0;
            //MOE.Common.Data.InrixTableAdapters.RoutesTableAdapter routesTA = new MOE.Common.Data.InrixTableAdapters.RoutesTableAdapter();
            //routesTA.Insert(RouteName, RouteDescription, out routeID);

        }

        public void UpdateRoute(string NewName, string NewDescription)
        {
            //routesTA.Update(NewName, NewDescription, this.ID, this.Name);
        }

        public void GetTravelTimes()
        {

        }
    }
}
