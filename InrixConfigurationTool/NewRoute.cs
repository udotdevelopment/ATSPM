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
    public partial class NewRoute : Form
    {
        //MOE.Common.Data.Inrix.RoutesDataTable RoutesDT;
        InrixConfigurationTool MainForm;
        public NewRoute(InrixConfigurationTool mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
        }

        private void uxSaveNewRouteButton_Click(object sender, EventArgs e)
        {
            int routeID = 0;
            string routeName = uxNewRouteNameText.Text.Trim();
            string routeDescription = uxNewRouteDescriptionText.Text.Trim();
            //MOE.Common.Data.InrixTableAdapters.RoutesTableAdapter RoutesTA = new MOE.Common.Data.InrixTableAdapters.RoutesTableAdapter();
            MOE.Common.Models.Inrix.Repositories.RouteRepository rr = new MOE.Common.Models.Inrix.Repositories.RouteRepository();
            if (routeDescription.Length > 0 && routeName.Length > 0)
            {
                MOE.Common.Models.Inrix.Route r = new MOE.Common.Models.Inrix.Route();
                r.Route_Description = routeDescription;
                r.Route_Name = routeName;

                rr.Add(r);
                
                r = rr.GetRouteByName(routeName);

                if (r != null)
                {
                    MessageBox.Show("Route " + routeName + " added.");
                    
                    
                    MainForm.FillRoutes();
                    uxNewRouteNameText.Clear();
                    uxNewRouteDescriptionText.Clear();
                }


            }
            else
            {
                MessageBox.Show("The Route Name and Route Description can not be empty");
            }
            


        }

        private void uxCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uxNewRouteNameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void uxNewRouteDescriptionText_TextChanged(object sender, EventArgs e)
        {

        }

        private void RouteDescLabel_Click(object sender, EventArgs e)
        {

        }

        private void RouteNameLable_Click(object sender, EventArgs e)
        {

        }


    }
}
