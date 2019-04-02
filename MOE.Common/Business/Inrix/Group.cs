using System.Collections.Generic;
using MOE.Common.Models.Inrix;
using MOE.Common.Models.Inrix.Repositories;

namespace MOE.Common.Business.Inrix
{
    public class Group
    {
        protected string description;
        protected int id;

        public List<Route> Items = new List<Route>();

        protected string name;


        /// <summary>
        ///     Default Constructor for the RouteGroupClass
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

            foreach (var route in groupCopy.Items)
                Items.Add(route);
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

        private void SetProperties(Group groupCopy)
        {
            Name = "Copy of " + groupCopy.Name;
            Description = groupCopy.Description;
        }


        public void FillMembers()
        {
            Items.Clear();


            var rr = RouteRepositoryFactory.CreateRepository();


            foreach (var routeRow in rr.GetRoutesByGroupID(ID))
            {
                var route = new Route(routeRow.Route_ID, routeRow.Route_Name, routeRow.Route_Description);
                Items.Add(route);
            }
        }


        public void AddMember(Route route)
        {
            Items.Add(route);
        }

        public void RemoveMember(Route route)
        {
            Items.Remove(route);
        }


        public void DeleteGroup()
        {
            var gmr = new GroupMemberRepository();
            var gr = new GroupRepository();
            gr.RemoveByID(ID);

            gmr.DeleteByGroupID(ID);
        }

        public void SaveMembers()
        {
            var gmr = new GroupMemberRepository();

            var x = 0;
            //remove the old Group from Group Members
            gmr.DeleteByGroupID(ID);

            //Save the  new group members
            foreach (var route in Items)
            {
                x++;
                var gm = new Group_Members();
                gm.Group_ID = ID;
                gm.Route_ID = route.ID;
                gm.Group_Order = x;

                gmr.Add(gm);
            }
        }

        public static void InsertGroup(string groupName, string groupDescription)
        {
            var gr = new GroupRepository();
            var newGroup = new Models.Inrix.Group();
            newGroup.Group_Name = groupName;
            newGroup.Group_Description = groupDescription;
            gr.Add(newGroup);
            //ID = newGroup.Group_ID;
        }

        public void UpdateRouteGroup(string NewName, string NewDescription)
        {
            var gr = new GroupRepository();
            var g = new Models.Inrix.Group();
            g.Group_ID = ID;
            g.Group_Description = NewDescription;
            g.Group_Name = NewName;
            gr.Update(g);
            //groupsTA.Update(NewDescription, NewName, this.ID, this.Name);
        }
    }
}