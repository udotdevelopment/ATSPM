using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            Models.Inrix.Repositories.GroupRepository gr = new Models.Inrix.Repositories.GroupRepository();
            List<Models.Inrix.Group> groupsDT = gr.GetAll();

            foreach (Models.Inrix.Group row in groupsDT)
            {
                MOE.Common.Business.Inrix.Group group = new Group(row.Group_ID, row.Group_Name, row.Group_Description);

                this.Items.Add(group);

            }
        }
    }
}
