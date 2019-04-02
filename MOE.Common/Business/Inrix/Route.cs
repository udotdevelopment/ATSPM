using System;
using System.Collections.Generic;
using System.Linq;
using MOE.Common.Models.Inrix.Repositories;

namespace MOE.Common.Business.Inrix
{
    public class Route
    {
        protected string description;
        protected int id;

        public List<Segment> Items = new List<Segment>();

        protected string name;

        public SortedDictionary<DateTime, int> TravelTimes = new SortedDictionary<DateTime, int>();

        //MOE.Common.Data.InrixTableAdapters.RoutesTableAdapter routesTA = new MOE.Common.Data.InrixTableAdapters.RoutesTableAdapter();
        //MOE.Common.Data.InrixTableAdapters.SegmentsTableAdapter segmentsTA = new Data.InrixTableAdapters.SegmentsTableAdapter();
        //MOE.Common.Data.InrixTableAdapters.GroupsTableAdapter groupsTA = new Data.InrixTableAdapters.GroupsTableAdapter();

        /// <summary>
        ///     Default Constructor for the Route Class
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
        ///     Copy Constructor
        /// </summary>
        /// <param name="routeCopy"></param>
        public Route(Route routeCopy)
        {
            var rr = new RouteRepository();

            Name = "Copy of " + routeCopy.Name;
            Description = routeCopy.Description;
            //routesTA.Insert(this.Name, this.Description, out tempID);

            var route = new Models.Inrix.Route();
            route.Route_Name = Name;
            route.Route_Description = Description;

            rr.Add(route);


            var copiedRoute = rr.GetRouteByName(name);
            ID = Convert.ToInt32(copiedRoute.Route_ID);


            foreach (var segment in routeCopy.Items)
                Items.Add(segment);

            SaveMembers();
        }

        public int ID
        {
            get => id;
            set => id = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }


        public void FillMembers()
        {
            Items.Clear();
            var segmentsDT = new List<Models.Inrix.Segment>();
            using (var db = new Models.Inrix.Inrix())
            {
                segmentsDT = (from r in db.Segments
                    select r).ToList();
            }
            //MOE.Common.Data.Inrix.SegmentsDataTable segmentsDT = segmentsTA.GetDataByRouteMembers(this.ID);


            //MOE.Common.Data.Inrix.SegmentsRow segRow;
            foreach (var row in segmentsDT)
            {
                var segment = new Segment(row.Segment_ID, row.Segment_Name, row.Segment_Description);
                Items.Add(segment);
            }
        }

        public void FillNonMembers()
        {
            Items.Clear();
            var segmentsDT = new List<Models.Inrix.Segment>();
            using (var db = new Models.Inrix.Inrix())
            {
                segmentsDT = (from r in db.Segments
                    select r).Except(from q in db.Segments where q.Segment_ID == ID select q).ToList();
            }
            // MOE.Common.Data.Inrix.SegmentsDataTable segmentsDT = segmentsTA.GetDataByRouteNonMember(this.ID);


            //MOE.Common.Data.Inrix.SegmentsRow segRow;
            foreach (var row in segmentsDT)
            {
                var segment = new Segment(row.Segment_ID, row.Segment_Name, row.Segment_Description);
                Items.Add(segment);
            }
        }

        public void AddMember(Segment segment)
        {
            Items.Add(segment);
        }

        public void RemoveMember(Segment segment)
        {
            Items.Remove(segment);
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
            //foreach (Segment segment in this.PhaseCycles)
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