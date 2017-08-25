using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOE.Common.Business.Inrix
{
    public class Segment
    {
        Models.Inrix.Repositories.SegmentRepository SegRep = new Models.Inrix.Repositories.SegmentRepository();

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

        public List<TravelTimeRecord> TravelTimes = new List<TravelTimeRecord>();

        public List<TMC> Items = new List<TMC>();


      

       /// <summary>
       /// Default Constructor for the Route Class
       /// </summary>
       /// <param name="id"></param>
       /// <param name="name"></param>
       /// <param name="description"></param>
        public Segment(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;

            FillMembers();
         
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="routeCopy"></param>
        public Segment(MOE.Common.Business.Inrix.Segment segmentCopy)
        {
            //int tempID = 0;
            Name = "Copy of " + segmentCopy.Name;
            Description = segmentCopy.Description;
            Models.Inrix.Segment copySeg = new Models.Inrix.Segment();

            copySeg.Segment_Name = Name;
            copySeg.Segment_Description = Description;

            SegRep.Add(copySeg);

            copySeg = SegRep.SelectSegmentByName(Name);
            //segmentsTA.Insert(this.Name, this.Description, out tempID);
            ID = Convert.ToInt32(copySeg.Segment_ID);


            foreach (TMC tmc in segmentCopy.Items)
            {
                this.Items.Add(tmc);
            }

            this.SaveMembers();
            

        }
        

        public void FillMembers()
        {
            this.Items.Clear();
            MOE.Common.Models.Inrix.Repositories.TMCRepository tmcr = new Models.Inrix.Repositories.TMCRepository();
            //Models.Inrix.Repositories.TMCRepository tmcr = new Models.Inrix.Repositories.TMCRepository();
            //List<Models.Inrix.Segment_Members> members = tmcr.GetSegmentMembers(this.ID);
            //MOE.Common.Data.Inrix.TMCDataTable TMCDT = TMCTA.GetDataBySegment(this.ID);

            foreach (MOE.Common.Models.Inrix.TMC row in tmcr.GetMembersBySegmentID(this.ID))
            {
                TMC tmc = new TMC(row.TMC1, row.Direction, row.TMC_Start, row.TMC_Stop, Convert.ToDouble(row.Length), row.Street);
                this.Items.Add(tmc);


            }

        }
        
        public void AddMember(TMC segment)
        {
            this.Items.Add(segment);
        }

        public void RemoveMember(TMC segment)
        {
            this.Items.Remove(segment);
        }


        public void DeleteSegment()
        {

            //Delete the segment from any routes
            //routesTA.DeleteSegmentFromAllRoutes(this.ID);

            Models.Inrix.Repositories.RouteMembersRepository rmr = new Models.Inrix.Repositories.RouteMembersRepository();

            rmr.RemoveSegmentFromRoute(this.ID.ToString());
            

            //Delete The segment from the SegmentMembers

            Models.Inrix.Repositories.SegmentMembersRepository sr = new Models.Inrix.Repositories.SegmentMembersRepository();

            sr.DeleteMembersBySegmentID(this.ID);
           

            //Delete The segment from Segments table
            //segmentsTA.Delete(this.ID, this.Name, this.Description);

            SegRep.RemoveByID(this.ID);
            
        }

        public void SaveMembers()
        {
            //remove the old route members form the DB.
            SegRep.RemoveByID(this.ID);

            Models.Inrix.Repositories.SegmentMembersRepository smr = new Models.Inrix.Repositories.SegmentMembersRepository();

            //segmentsTA.DeleteSegmentFromSegmentMembers(this.ID);
            int x = 0;
            foreach (TMC tmc in this.Items)
            {     
                x++;
                smr.InsertSegmentMembers(this.ID, tmc.TMCCode, x);
            }
        }

        public static void InsertSegment(string SegmentName, string SegmentDescription)
        {
            MOE.Common.Models.Inrix.Repositories.SegmentRepository sr = new Models.Inrix.Repositories.SegmentRepository();
            MOE.Common.Models.Inrix.Segment s = new Models.Inrix.Segment();
            s.Segment_Name = SegmentName;
            s.Segment_Description = SegmentDescription;

            sr.Add(s);


        }

        public void UpdateSegment(string NewName, string NewDescription)
        {
            SegRep.Update(this.ID, NewName, NewDescription);
            
        }


    }
}
