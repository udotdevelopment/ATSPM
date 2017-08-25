using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MOE.Common;

namespace InrixConfigurationTool
{
    public partial class EditGroup : Form
    {
        InrixConfigurationTool MainForm;
        MOE.Common.Business.Inrix.Group SelectedGroup;
        public EditGroup(InrixConfigurationTool mainForm, MOE.Common.Business.Inrix.Group selectedGroup)
        {
            InitializeComponent();
            MainForm = mainForm;
            SelectedGroup = selectedGroup;
            this.uxNewGroupNameText.Text = SelectedGroup.Name;
            this.uxNewGroupDescriptionText.Text = SelectedGroup.Description;
        }

        private void uxCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uxUpdateGroupButton_Click(object sender, EventArgs e)
        {
            string groupName = uxNewGroupNameText.Text.Trim();
            string groupDescription = uxNewGroupDescriptionText.Text.Trim();

            if (groupDescription.Length > 0 && groupName.Length > 0)
            {
                SelectedGroup.UpdateRouteGroup(groupName, groupDescription);

                MainForm.FillRoutes();
                uxNewGroupNameText.Clear();
                uxNewGroupDescriptionText.Clear();

                this.Close();


            }
            else
            {
                MessageBox.Show("The Route Name and Route Description can not be empty");
            }
            
        }
    }
}
