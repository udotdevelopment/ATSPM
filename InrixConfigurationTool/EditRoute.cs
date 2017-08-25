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
    public partial class EditRoute : Form
    {
        InrixConfigurationTool MainForm;
        MOE.Common.Business.Inrix.Route SelectedRoute;
        public EditRoute(InrixConfigurationTool mainForm, MOE.Common.Business.Inrix.Route selectedRoute)
        {
            InitializeComponent();
            MainForm = mainForm;
            SelectedRoute = selectedRoute;
            this.uxNewRouteNameText.Text = selectedRoute.Name;
            this.uxNewRouteDescriptionText.Text = selectedRoute.Description;

        }

        private void uxCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uxUpdateNewRouteButton_Click(object sender, EventArgs e)
        {
           
            string routeName = uxNewRouteNameText.Text.Trim();
            string routeDescription = uxNewRouteDescriptionText.Text.Trim();

             if (routeDescription.Length > 0 && routeName.Length > 0)
             {
                 SelectedRoute.UpdateRoute(routeName, routeDescription); 
                 
                     MainForm.FillRoutes();
                     uxNewRouteNameText.Clear();
                     uxNewRouteDescriptionText.Clear();
                     MainForm.FillGroupMembers();
                     MainForm.FillGroupNonMembers();   
                     this.Close();


             }
            else
            {
                MessageBox.Show("The Route Name and Route Description can not be empty");
            }
            

        }
    }
}
