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
    public partial class NewSegment : Form
    {
        //MOE.Common.Data.Inrix.SegmentsDataTable SegmentsDT;
        //MOE.Common.Business.Inrix.Segment Segment;
        InrixConfigurationTool MainForm;
        public NewSegment(InrixConfigurationTool mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
            //Segment = segment;
        }

        private void uxCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uxSaveNewSegmentButton_Click(object sender, EventArgs e)
        {
            int segmentID = 0;
            string segmentName = uxNewSegmentNameText.Text.Trim();
            string segmentDescription = uxNewSegmentDescriptionText.Text.Trim();
            //MOE.Common.Data.InrixTableAdapters.SegmentsTableAdapter segmentsTA = new MOE.Common.Data.InrixTableAdapters.SegmentsTableAdapter();
            if (segmentDescription.Length > 0 && segmentName.Length > 0)
            {
                MOE.Common.Models.Inrix.Repositories.SegmentRepository sr = new MOE.Common.Models.Inrix.Repositories.SegmentRepository();

                //if (segmentsTA.Insert(segmentName, segmentDescription, out segmentID) > 0)
                MOE.Common.Models.Inrix.Segment s = new MOE.Common.Models.Inrix.Segment();
                s.Segment_Description = segmentDescription;
                s.Segment_Name = segmentName;

                sr.Add(s);
                
                    MessageBox.Show("Segment " + segmentName + " added.");


                    MainForm.FillSegments();
                    uxNewSegmentNameText.Clear();
                    uxNewSegmentDescriptionText.Clear();
                


            }
            else
            {
                MessageBox.Show("The Segment Name and Segment Description can not be empty");
            }
            
        }
    }
}
