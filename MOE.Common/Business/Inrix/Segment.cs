using System;
using System.Collections.Generic;
using MOE.Common.Models.Inrix.Repositories;

namespace MOE.Common.Business.Inrix
{
    public class Segment
    {
        protected string description;

        protected int id;

        public List<TMC> Items = new List<TMC>();

        protected string name;
        private readonly SegmentRepository SegRep = new SegmentRepository();

        public List<TravelTimeRecord> TravelTimes = new List<TravelTimeRecord>();


        /// <summary>
        ///     Default Constructor for the Route Class
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
        ///     Copy Constructor
        /// </summary>
        /// <param name="routeCopy"></param>
        public Segment(Segment segmentCopy)
        {
            //int tempID = 0;
            Name = "Copy of " + segmentCopy.Name;
            Description = segmentCopy.Description;
            var copySeg = new Models.Inrix.Segment();

            copySeg.Segment_Name = Name;
            copySeg.Segment_Description = Description;

            SegRep.Add(copySeg);

            copySeg = SegRep.SelectSegmentByName(Name);
            //segmentsTA.Insert(this.Name, this.Description, out tempID);
            ID = Convert.ToInt32(copySeg.Segment_ID);


            foreach (var tmc in segmentCopy.Items)
                Items.Add(tmc);

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


        public void FillMembers()
        {
            Items.Clear();
            var tmcr = new TMCRepository();
            //Models.Inrix.Repositories.TMCRepository tmcr = new Models.Inrix.Repositories.TMCRepository();
            //List<Models.Inrix.Segment_Members> members = tmcr.GetSegmentMembers(this.ID);
            //MOE.Common.Data.Inrix.TMCDataTable TMCDT = TMCTA.GetDataBySegment(this.ID);

            foreach (var row in tmcr.GetMembersBySegmentID(ID))
            {
                var tmc = new TMC(row.TMC1, row.Direction, row.TMC_Start, row.TMC_Stop, Convert.ToDouble(row.Length),
                    row.Street);
                Items.Add(tmc);
            }
        }

        public void AddMember(TMC segment)
        {
            Items.Add(segment);
        }

        public void RemoveMember(TMC segment)
        {
            Items.Remove(segment);
        }


        public void DeleteSegment()
        {
            //Delete the segment from any routes
            //routesTA.DeleteSegmentFromAllRoutes(this.ID);

            var rmr = new RouteMembersRepository();

            rmr.RemoveSegmentFromRoute(ID.ToString());


            //Delete The segment from the SegmentMembers

            var sr = new SegmentMembersRepository();

            sr.DeleteMembersBySegmentID(ID);


            //Delete The segment from Segments table
            //segmentsTA.Delete(this.ID, this.Name, this.Description);

            SegRep.RemoveByID(ID);
        }

        public void SaveMembers()
        {
            //remove the old route members form the DB.
            SegRep.RemoveByID(ID);

            var smr = new SegmentMembersRepository();

            //segmentsTA.DeleteSegmentFromSegmentMembers(this.ID);
            var x = 0;
            foreach (var tmc in Items)
            {
                x++;
                smr.InsertSegmentMembers(ID, tmc.TMCCode, x);
            }
        }

        public static void InsertSegment(string SegmentName, string SegmentDescription)
        {
            var sr = new SegmentRepository();
            var s = new Models.Inrix.Segment();
            s.Segment_Name = SegmentName;
            s.Segment_Description = SegmentDescription;

            sr.Add(s);
        }

        public void UpdateSegment(string NewName, string NewDescription)
        {
            SegRep.Update(ID, NewName, NewDescription);
        }
    }
}