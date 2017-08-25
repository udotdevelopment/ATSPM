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
    public partial class EditSegment : Form
    {
        InrixConfigurationTool MainForm;
        MOE.Common.Business.Inrix.Segment SelectedSegment;
        public EditSegment(InrixConfigurationTool mainForm, MOE.Common.Business.Inrix.Segment selectedSegment)
        {
            InitializeComponent();
            MainForm = mainForm;
            SelectedSegment = selectedSegment;
            this.uxNewSegmentNameText.Text = selectedSegment.Name;
            this.uxNewSegmentDescriptionText.Text = selectedSegment.Description;

        }

        private void uxCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uxUpdateNewSegmentButton_Click(object sender, EventArgs e)
        {
            string segmentName = uxNewSegmentNameText.Text.Trim();
            string segmentDescription = uxNewSegmentDescriptionText.Text.Trim();

            if (segmentDescription.Length > 0 && segmentName.Length > 0)
            {
                SelectedSegment.UpdateSegment(segmentName, segmentDescription);

                MainForm.FillSegments();
                uxNewSegmentNameText.Clear();
                uxNewSegmentDescriptionText.Clear();
                MainForm.FillSegmentMembers();
                MainForm.FillSegmentNonMembers();
                
                this.Close();
            }
        }
    }
}
