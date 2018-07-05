using System.Collections.Generic;
using MOE.Common.Models.Inrix.Repositories;

namespace MOE.Common.Business.Inrix
{
    public class GroupCollection
    {
        public List<Group> Items = new List<Group>();


        public GroupCollection()
        {
            GetGroups();
        }

        public void GetGroups()
        {
            Items.Clear();

            //MOE.Common.Data.InrixTableAdapters.GroupsTableAdapter groupsTA = new Data.InrixTableAdapters.GroupsTableAdapter();
            //MOE.Common.Data.Inrix.GroupsDataTable groupsDT = new Data.Inrix.GroupsDataTable();

            //groupsTA.Fill(groupsDT);

            var gr = new GroupRepository();
            var groupsDT = gr.GetAll();

            foreach (var row in groupsDT)
            {
                var group = new Group(row.Group_ID, row.Group_Name, row.Group_Description);

                Items.Add(group);
            }
        }
    }
}