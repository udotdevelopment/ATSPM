using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InrixConfigurationTool
{
    public partial class NewGroup : Form
    {
        //MOE.Common.Data.Inrix.RoutesDataTable RoutesDT;
        InrixConfigurationTool MainForm;
        public NewGroup(InrixConfigurationTool mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
        }

        private void uxCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uxSaveNewGroupButton_Click(object sender, EventArgs e)
        {
            
            string groupName = uxGroupNameText.Text.Trim();
            string groupDescription = uxGroupDescriptionText.Text.Trim();
            //MOE.Common.Data.InrixTableAdapters.GroupsTableAdapter groupsTA = new MOE.Common.Data.InrixTableAdapters.GroupsTableAdapter();
            MOE.Common.Models.Inrix.Repositories.GroupRepository gr = new MOE.Common.Models.Inrix.Repositories.GroupRepository();
            MOE.Common.Models.Inrix.Group g = new MOE.Common.Models.Inrix.Group();

            if (groupDescription.Length > 0 && groupName.Length > 0)
            {

                g.Group_Name = groupName;
                g.Group_Description = groupDescription;

                gr.Add(g);
                    MessageBox.Show("Group " + groupName + " added.");


                    MainForm.FillGroups();
                    uxGroupNameText.Clear();
                    uxGroupDescriptionText.Clear();
                


            }
            else
            {
                MessageBox.Show("The Group Name and Group Description can not be empty");
            }
            
        }
    }
}
