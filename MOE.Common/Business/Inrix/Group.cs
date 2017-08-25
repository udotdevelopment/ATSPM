using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace MOE.Common.Business.Inrix
{
    public class Group
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

        public List<Route> Items = new List<Route>();


        /// <summary>
        /// Default Constructor for the RouteGroupClass
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Group(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;

            FillMembers();

        }

        public Group(Group groupCopy)
        {
            SetProperties(groupCopy);
            InsertGroup(groupCopy.name, groupCopy.Description);
            
            foreach (Route route in groupCopy.Items)
            {
                this.Items.Add(route);
            }
            this.SaveMembers();
        }

        private void SetProperties(Group groupCopy)
        {
            Name = "Copy of " + groupCopy.Name;
            Description = groupCopy.Description;
        }


        public void FillMembers()
        {
            this.Items.Clear();

           
            Models.Inrix.Repositories.IRouteRepository rr = Models.Inrix.Repositories.RouteRepositoryFactory.CreateRepository();


             

            foreach (MOE.Common.Models.Inrix.Route routeRow in rr.GetRoutesByGroupID(this.ID))
            {
                Route route = new Route(routeRow.Route_ID, routeRow.Route_Name, routeRow.Route_Description);
                this.Items.Add(route);
            }

         }

        

        public void AddMember(Route route)
        {
            this.Items.Add(route);
        }

        public void RemoveMember(Route route)
        {
            this.Items.Remove(route);
        }


        public void DeleteGroup()
        {
             Models.Inrix.Repositories.GroupMemberRepository gmr = new Models.Inrix.Repositories.GroupMemberRepository();
             Models.Inrix.Repositories.GroupRepository gr = new Models.Inrix.Repositories.GroupRepository();
             gr.RemoveByID(this.ID);

             gmr.DeleteByGroupID(this.ID);

        }

        public void SaveMembers()
        {
            MOE.Common.Models.Inrix.Repositories.GroupMemberRepository gmr = new Models.Inrix.Repositories.GroupMemberRepository();

            int x = 0;
            //remove the old Group from Group Members
            gmr.DeleteByGroupID(this.ID);

            //Save the  new group members
            foreach (Route route in this.Items)
            {
                x++;
                Models.Inrix.Group_Members gm = new Models.Inrix.Group_Members();
                gm.Group_ID = this.ID;
                gm.Route_ID = route.ID;
                gm.Group_Order = x;

                gmr.Add(gm);

            }
        }

        public static void InsertGroup(string groupName, string groupDescription)
        {
            Models.Inrix.Repositories.GroupRepository gr = new Models.Inrix.Repositories.GroupRepository();
            Models.Inrix.Group newGroup = new Models.Inrix.Group();
            newGroup.Group_Name = groupName;
            newGroup.Group_Description = groupDescription;
            gr.Add(newGroup);
            //ID = newGroup.Group_ID;

        }

        public void UpdateRouteGroup(string NewName, string NewDescription)
        {
            Models.Inrix.Repositories.GroupRepository gr = new Models.Inrix.Repositories.GroupRepository();
            Models.Inrix.Group g = new Models.Inrix.Group();
            g.Group_ID = this.ID;
            g.Group_Description = NewDescription;
            g.Group_Name = NewName;
            gr.Update(g);
            //groupsTA.Update(NewDescription, NewName, this.ID, this.Name);
        }


    }
}
