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
    public partial class InrixConfigurationTool : Form
    {
        
        //MOE.Common.Data.InrixTableAdapters.SegmentsTableAdapter segmentsTA = new MOE.Common.Data.InrixTableAdapters.SegmentsTableAdapter();
        //MOE.Common.Data.InrixTableAdapters.RoutesTableAdapter routesTA = new MOE.Common.Data.InrixTableAdapters.RoutesTableAdapter();
        //MOE.Common.Data.InrixTableAdapters.GroupsTableAdapter groupsTA = new MOE.Common.Data.InrixTableAdapters.GroupsTableAdapter();
        //MOE.Common.Data.InrixTableAdapters.TMCTableAdapter TMCTA = new MOE.Common.Data.InrixTableAdapters.TMCTableAdapter();       
        //MOE.Common.Data.Inrix.RoutesDataTable routesDT = new MOE.Common.Data.Inrix.RoutesDataTable();
        MOE.Common.Business.Inrix.GroupCollection groups = new MOE.Common.Business.Inrix.GroupCollection();
        MOE.Common.Business.Inrix.Route selectedRoute = null;
        MOE.Common.Business.Inrix.Group selectedGroup= null;
        MOE.Common.Business.Inrix.Segment selectedSegment = null;

        public InrixConfigurationTool()
        {
            InitializeComponent();
            this.AllowDrop = true;
            
        }

  

        private void InrixConfigurationTool_Load(object sender, EventArgs e)
        { 
            //TODO: EF Conversionn (Inrix, We will do it later)
            //routesTA.Fill(routesDT);
            //FillGraphDetectors();
            //FillTMCs();
            //FillSegments();
            //FillRoutes();
            //FillGroups();

        }
        private void uxSegmentsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedItem = this.uxRoutesListView.SelectedItems;
            foreach (ListViewItem item in selectedItem)
            {
                selectedSegment = (item.Tag as MOE.Common.Business.Inrix.Segment);
            }
            if (selectedRoute.Description == "")
            {
                uxDescriptionText.Text = selectedSegment.Name;
            }
            else { uxDescriptionText.Text = selectedSegment.Description; }

            uxDeleteSegmentButton.Enabled = true;
            uxCopySegmentButton.Enabled = true;
            uxEditSegmentButton.Enabled = true;
            FillSegmentMembers();
            FillSegmentNonMembers();

        }

        private void uxRoutesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedItem = this.uxRoutesListView.SelectedItems;
            foreach (ListViewItem item in selectedItem)

            {
            selectedRoute = (item.Tag as MOE.Common.Business.Inrix.Route);
            }
            if (selectedRoute.Description == "")
            {
                uxDescriptionText.Text = selectedRoute.Name;
            }
            else { uxDescriptionText.Text = selectedRoute.Description; }

            uxDeleteRouteButton.Enabled = true;
            uxCopyRouteButton.Enabled = true;
            uxEditRouteButton.Enabled = true;
            FillRouteMembers();
            FillRouteNonMembers();

        }

        private void uxGroupsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedItem = this.uxGroupsListView.SelectedItems;
            foreach (ListViewItem item in selectedItem)
            {
                selectedGroup = (item.Tag as MOE.Common.Business.Inrix.Group);
            }
            if (selectedGroup.Description == "")
            {
                uxGroupDescription.Text = selectedGroup.Name;
            }
            else { uxGroupDescription.Text = selectedGroup.Description; }

            uxDeleteGroupButton.Enabled = true;
            uxEditGroupButton.Enabled = true;
            uxCopyGroupButton.Enabled = true;
            FillGroupMembers();
            FillGroupNonMembers();
        }

        public void FillSegments()
        {

            //TODO: EF Conversion (Inrix, We will do it later)
            //uxSegmentsListView.Clear();
            //MOE.Common.Data.Inrix.SegmentsDataTable segmentsDT = segmentsTA.GetData();
            //foreach (MOE.Common.Data.Inrix.SegmentsRow row in segmentsDT)
            //{
            //    ListViewItem item = new ListViewItem();
            //    MOE.Common.Business.Inrix.Segment segment = new MOE.Common.Business.Inrix.Segment(row.Segment_ID, row.Segment_Name, row.Segment_Description);
            //    item.Text = segment.Name;
            //    item.Tag = segment;
            //    uxSegmentsListView.PhaseCycles.Add(item);
            //}
        }

        public void FillRoutes()
        {

            //TODO: EF Conversion (Inrix, We will do it later)
            //uxRoutesListView.Clear();
            //routesTA.Fill(routesDT);
            //foreach (MOE.Common.Data.Inrix.RoutesRow row in routesDT)
            //{
            //    MOE.Common.Business.Inrix.Route route = new MOE.Common.Business.Inrix.Route(row.Route_ID, row.Route_Name, row.Route_Name);
            //    ListViewItem item = new ListViewItem();
            //    item.Text = route.Name;
            //    item.Tag = route;
            //    uxRoutesListView.PhaseCycles.Add(item);
            //}
        }

        public void FillGroups()
        {
            //TODO: EF Conversion (Inrix, We will do it later)
            //groups.GetGroups();
            
            //uxGroupsListView.Clear();
            //foreach (MOE.Common.Business.Inrix.Group group in groups.PhaseCycles)
            //{
            //    ListViewItem item = new ListViewItem();
            //    item.Text = group.Name;
            //    item.Tag = group;
            //    uxGroupsListView.PhaseCycles.Add(item);
            //}
        }

        public void FillSegmentMembers()
        {
            //TODO: EF Conversion (Inrix, We will do it later)
            //if (selectedSegment != null)
            //{
            //    uxSegmentMembersListView.Clear();

            //    MOE.Common.Data.Inrix.TMCDataTable TMCDT = TMCTA.GetDataBySegment(selectedSegment.ID);

            //    foreach (MOE.Common.Data.Inrix.TMCRow row in TMCDT)
            //    {
            //        MOE.Common.Business.Inrix.TMC tmc = new MOE.Common.Business.Inrix.TMC(row.TMC, row.Direction, row.TMC_Start, row.TMC_Stop, row.Length, row.Street);
            //        ListViewItem item = new ListViewItem();
            //        item.Name = tmc.TMCCode;
            //        item.Text = tmc.Street + " " + tmc.Direction + " " + " From: " + tmc.Start + " To: " + tmc.Stop;
            //        item.Tag = tmc;
            //        uxSegmentMembersListView.PhaseCycles.Add(item);
            //    }
            //}
                
        }

        public void FillSegmentNonMembers()
        {
            //TODO: EF Conversion (Inrix, We will do it later)
            //if (selectedSegment != null)
            //{
            //    uxSegmentNonMembersListView.Clear();

            //    MOE.Common.Data.Inrix.TMCDataTable TMCDT = TMCTA.GetDataBySegmentNonMembers(selectedSegment.ID);
            //    foreach (MOE.Common.Data.Inrix.TMCRow row in TMCDT)
            //    {
            //        MOE.Common.Business.Inrix.TMC tmc = new MOE.Common.Business.Inrix.TMC(row.TMC, row.Direction, row.TMC_Start, row.TMC_Stop, row.Length, row.Street);
            //        ListViewItem item = new ListViewItem();
            //        item.Name = tmc.TMCCode;
            //        item.Text = tmc.Street + " " + tmc.Direction + " " +  " From: " + tmc.Start + " To: " + tmc.Stop;
            //        item.Tag = tmc;
            //        uxSegmentNonMembersListView.PhaseCycles.Add(item);
            //    }
            //}

        }

        private void FillRouteNonMembers()
        {
            //TODO: EF Conversion (Inrix, We will do it later)
            //if (selectedRoute != null)
            //{
            //    uxRouteNonMembersListView.Clear();
            //    MOE.Common.Data.Inrix.SegmentsDataTable routeNonMembersDT = segmentsTA.GetDataByRouteNonMember(selectedRoute.ID);

            //    foreach (MOE.Common.Data.Inrix.SegmentsRow row in routeNonMembersDT)
            //    {
            //        MOE.Common.Business.Inrix.Segment segment = new MOE.Common.Business.Inrix.Segment(row.Segment_ID, row.Segment_Name, row.Segment_Description);
            //        ListViewItem item = new ListViewItem();
            //        string textName = segment.Name;
            //        item.Text = textName;
            //        item.Tag = segment;
            //        uxRouteNonMembersListView.PhaseCycles.Add(item);
            //    }
            //}
        }

        public void FillGroupMembers()
        {
            //TODO: EF Conversion (Inrix, We will do it later)
            //if (selectedGroup != null)
            //{
            //    uxGroupMembersListView.Clear();
                
            //    MOE.Common.Data.Inrix.RoutesDataTable groupMembersDT = routesTA.GetDataByGroup(selectedGroup.ID);

            //    foreach (MOE.Common.Data.Inrix.RoutesRow row in groupMembersDT)
            //    {
            //        MOE.Common.Business.Inrix.Route route = new MOE.Common.Business.Inrix.Route(row.Route_ID, row.Route_Name, row.Route_Description);
            //        ListViewItem item = new ListViewItem();
            //        string textName = route.Name;

            //        item.Text = textName;
            //        item.Tag = route;
            //        uxGroupMembersListView.PhaseCycles.Add(item);
            //    }
            //}
        }

        public void FillRouteMembers()
        {
            //TODO: EF Conversion (Inrix, We will do it later)
            //if (selectedRoute != null)
            //{
            //    uxRouteMembersListView.Clear();
            //    MOE.Common.Data.Inrix.SegmentsDataTable routeMembersDT = segmentsTA.GetDataByRouteMembers(selectedRoute.ID);

            //    foreach (MOE.Common.Data.Inrix.SegmentsRow row in routeMembersDT)
            //    {
            //        MOE.Common.Business.Inrix.Segment segment = new MOE.Common.Business.Inrix.Segment(row.Segment_ID, row.Segment_Name, row.Segment_Description);
            //        ListViewItem item = new ListViewItem();

            //        string textName = segment.Name;

            //        item.Text = textName;
            //        item.Tag = segment;
            //        uxRouteMembersListView.PhaseCycles.Add(item);
            //    }
            //}
        }

        public void FillGroupNonMembers()
        {
            //TODO: EF Conversion (Inrix, We will do it later)
            //if (selectedGroup != null)
            //{
            //    uxGroupNonMembersListView.Clear();
            //    MOE.Common.Data.Inrix.RoutesDataTable groupNonMembersDT = routesTA.GetDataByGroupNonMember(selectedGroup.ID);

            //    foreach (MOE.Common.Data.Inrix.RoutesRow row in groupNonMembersDT)
            //    {
            //        MOE.Common.Business.Inrix.Route route = new MOE.Common.Business.Inrix.Route(row.Route_ID, row.Route_Name, row.Route_Description);
            //        ListViewItem item = new ListViewItem();
            //        string textName = route.Name;

            //        item.Text = textName;
            //        item.Tag = route;
            //        uxGroupNonMembersListView.PhaseCycles.Add(item);
            //    }
            //}
        }

        private void FillTMCs()
        {
            //TODO: EF Conversion (Inrix, We will do it later)
            //MOE.Common.Data.Inrix.TMCDataTable TMCDT = TMCTA.GetData();
            //uxTMCDataGridView.DataSource = TMCDT;
            
        }


        private void FillGraphDetectors()
        {
            //TODO: EF Conversion (Inrix, We will do it later)
            //MOE.Common.Data.SignalsTableAdapters.DetectorsTableAdapter graphDetTA = new MOE.Common.Data.SignalsTableAdapters.DetectorsTableAdapter();
            //MOE.Common.Data.Signals.DetectorsDataTable graphDetDT = graphDetTA.GetData();

            //DGVGraphDetectors.DataSource = graphDetDT;
        }


        private void uxAddNewRouteButton_Click(object sender, EventArgs e)
        {
            NewRoute newRoute = new NewRoute(this);
            newRoute.Show();
        }

        private void uxAddGroupButton_Click(object sender, EventArgs e)
        {
            NewGroup newGroup = new NewGroup(this);
            newGroup.Show();
        }

        private void uxDeleteRouteButton_Click_1(object sender, EventArgs e)
        {
            if (selectedRoute != null)
            {
                selectedRoute.DeleteRoute();
                
                FillRoutes();
            }
        }

        private void uxDeleteGroupButton_Click(object sender, EventArgs e)
        {
            if (selectedGroup != null)
            {
                selectedGroup.DeleteGroup();
                groups.Items.Remove(selectedGroup);
                FillGroups();
            }
        }

        private void uxAddSegmentToRouteButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in uxRouteNonMembersListView.SelectedItems)
            {
                uxRouteNonMembersListView.Items.Remove(item);
                uxRouteMembersListView.Items.Add(item);
                
            }
        }

        private void uxAddRoutetoGroupButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in uxGroupNonMembersListView.SelectedItems)
            {
                uxGroupNonMembersListView.Items.Remove(item);
                uxGroupMembersListView.Items.Add(item);

            }
        }

        private void uxRemoveSegmentFromRoute_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in uxRouteMembersListView.SelectedItems)
            {
                uxRouteMembersListView.Items.Remove(item);
                uxRouteNonMembersListView.Items.Add(item);
                
            }
        }


        private void uxRemoveRouteFromGroupButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in uxGroupMembersListView.SelectedItems)
            {
                uxGroupMembersListView.Items.Remove(item);
                uxGroupNonMembersListView.Items.Add(item);

            }
        }

        private void uxSaveRouteButton_Click(object sender, EventArgs e)
        {
            if (selectedRoute != null)
            {
            selectedRoute.Items.Clear();

            foreach (ListViewItem item in uxRouteMembersListView.Items)
            {
                selectedRoute.Items.Add(item.Tag as MOE.Common.Business.Inrix.Segment);                   
            }
            selectedRoute.SaveMembers();
            FillRouteMembers();
            FillRouteNonMembers();
            }
        }

        private void uxSaveGroupButton_Click(object sender, EventArgs e)
        {
            if (selectedGroup != null)
            {
                selectedGroup.Items.Clear();

                foreach (ListViewItem item in uxGroupMembersListView.Items)
                {
                    selectedGroup.Items.Add(item.Tag as MOE.Common.Business.Inrix.Route);
                }
                selectedGroup.SaveMembers();
                FillGroupMembers();
                FillGroupNonMembers();
            }
        }

        private void uxRouteMembersListBox_ItemDrag(object sender, ItemDragEventArgs e)
        {
            uxRouteMembersListView.DoDragDrop(uxRouteMembersListView.SelectedItems, DragDropEffects.Move);
        }

        private void uxRouteMembersListBox_DragEnter(object sender, DragEventArgs e)
        {
            int len = e.Data.GetFormats().Length - 1;
            int i;
            for (i = 0; i <= len; i++)
            {
                if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    //The data from the drag source is moved to the target.	
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void uxRouteMembersListBox_DragDrop(object sender, DragEventArgs e)
        {
            //Return if the items are not selected in the ListView control.
            if (uxRouteMembersListView.SelectedItems.Count == 0)
            {
                return;
            }
            //Returns the location of the mouse pointer in the ListView control.
            Point cp = uxRouteMembersListView.PointToClient(new Point(e.X, e.Y));
            //Obtain the item that is located at the specified location of the mouse pointer.
            ListViewItem dragToItem = uxRouteMembersListView.GetItemAt(cp.X, cp.Y);
            if (dragToItem == null)
            {
                return;
            }
            //Obtain the index of the item at the mouse pointer.
            int dragIndex = dragToItem.Index;
            ListViewItem[] sel = new ListViewItem[uxRouteMembersListView.SelectedItems.Count];
            for (int i = 0; i <= uxRouteMembersListView.SelectedItems.Count - 1; i++)
            {
                sel[i] = uxRouteMembersListView.SelectedItems[i];
            }
            for (int i = 0; i < sel.GetLength(0); i++)
            {
                //Obtain the ListViewItem to be dragged to the target location.
                ListViewItem dragItem = sel[i];
                int itemIndex = dragIndex;
                if (itemIndex == dragItem.Index)
                {
                    return;
                }
                if (dragItem.Index < itemIndex)
                    itemIndex++;
                else
                    itemIndex = dragIndex + i;
                //Insert the item at the mouse pointer.
                ListViewItem insertItem = (ListViewItem)dragItem.Clone();
                uxRouteMembersListView.Items.Insert(itemIndex, insertItem);
                //Removes the item from the initial location while 
                //the item is moved to the new location.
                uxRouteMembersListView.Items.Remove(dragItem);
            }
        }

        private void uxRouteNonMembersListBox_ItemDrag(object sender, ItemDragEventArgs e)
        {
            uxRouteNonMembersListView.DoDragDrop(uxRouteNonMembersListView.SelectedItems, DragDropEffects.Move);
        }

        private void uxRouteNonMembersListBox_DragEnter(object sender, DragEventArgs e)
        {
            int len = e.Data.GetFormats().Length - 1;
            int i;
            for (i = 0; i <= len; i++)
            {
                if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    //The data from the drag source is moved to the target.	
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void uxRouteNonMembersListBox_DragDrop(object sender, DragEventArgs e)
        {
            //Return if the items are not selected in the ListView control.
            if (uxRouteNonMembersListView.SelectedItems.Count == 0)
            {
                return;
            }
            //Returns the location of the mouse pointer in the ListView control.
            Point cp = uxRouteNonMembersListView.PointToClient(new Point(e.X, e.Y));
            //Obtain the item that is located at the specified location of the mouse pointer.
            ListViewItem dragToItem = uxRouteNonMembersListView.GetItemAt(cp.X, cp.Y);
            if (dragToItem == null)
            {
                return;
            }
            //Obtain the index of the item at the mouse pointer.
            int dragIndex = dragToItem.Index;
            ListViewItem[] sel = new ListViewItem[uxRouteNonMembersListView.SelectedItems.Count];
            for (int i = 0; i <= uxRouteNonMembersListView.SelectedItems.Count - 1; i++)
            {
                sel[i] = uxRouteNonMembersListView.SelectedItems[i];
            }
            for (int i = 0; i < sel.GetLength(0); i++)
            {
                //Obtain the ListViewItem to be dragged to the target location.
                ListViewItem dragItem = sel[i];
                int itemIndex = dragIndex;
                if (itemIndex == dragItem.Index)
                {
                    return;
                }
                if (dragItem.Index < itemIndex)
                    itemIndex++;
                else
                    itemIndex = dragIndex + i;
                //Insert the item at the mouse pointer.
                ListViewItem insertItem = (ListViewItem)dragItem.Clone();
                uxRouteNonMembersListView.Items.Insert(itemIndex, insertItem);
                //Removes the item from the initial location while 
                //the item is moved to the new location.
                uxRouteNonMembersListView.Items.Remove(dragItem);
            }
        }

        private void uxGroupMembersListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            uxGroupMembersListView.DoDragDrop(uxGroupMembersListView.SelectedItems, DragDropEffects.Move);
        }

        private void uxGroupMembersListView_DragEnter(object sender, DragEventArgs e)
        {
            int len = e.Data.GetFormats().Length - 1;
            int i;
            for (i = 0; i <= len; i++)
            {
                if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    //The data from the drag source is moved to the target.	
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void uxGroupMembersListView_DragDrop(object sender, DragEventArgs e)
        {
            //Return if the items are not selected in the ListView control.
            if (uxGroupMembersListView.SelectedItems.Count == 0)
            {
                return;
            }
            //Returns the location of the mouse pointer in the ListView control.
            Point cp = uxGroupMembersListView.PointToClient(new Point(e.X, e.Y));
            //Obtain the item that is located at the specified location of the mouse pointer.
            ListViewItem dragToItem = uxGroupMembersListView.GetItemAt(cp.X, cp.Y);
            if (dragToItem == null)
            {
                return;
            }
            //Obtain the index of the item at the mouse pointer.
            int dragIndex = dragToItem.Index;
            ListViewItem[] sel = new ListViewItem[uxGroupMembersListView.SelectedItems.Count];
            for (int i = 0; i <= uxGroupMembersListView.SelectedItems.Count - 1; i++)
            {
                sel[i] = uxGroupMembersListView.SelectedItems[i];
            }
            for (int i = 0; i < sel.GetLength(0); i++)
            {
                //Obtain the ListViewItem to be dragged to the target location.
                ListViewItem dragItem = sel[i];
                int itemIndex = dragIndex;
                if (itemIndex == dragItem.Index)
                {
                    return;
                }
                if (dragItem.Index < itemIndex)
                    itemIndex++;
                else
                    itemIndex = dragIndex + i;
                //Insert the item at the mouse pointer.
                ListViewItem insertItem = (ListViewItem)dragItem.Clone();
                uxGroupMembersListView.Items.Insert(itemIndex, insertItem);
                //Removes the item from the initial location while 
                //the item is moved to the new location.
                uxGroupMembersListView.Items.Remove(dragItem);
            }
        }

        private void uxGroupNonMembersListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            uxGroupNonMembersListView.DoDragDrop(uxGroupNonMembersListView.SelectedItems, DragDropEffects.Move);
        }

        private void uxGroupNonMembersListView_DragEnter(object sender, DragEventArgs e)
        {
            int len = e.Data.GetFormats().Length - 1;
            int i;
            for (i = 0; i <= len; i++)
            {
                if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    //The data from the drag source is moved to the target.	
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void uxGroupNonMembersListView_DragDrop(object sender, DragEventArgs e)
        {
            //Return if the items are not selected in the ListView control.
            if (uxGroupNonMembersListView.SelectedItems.Count == 0)
            {
                return;
            }
            //Returns the location of the mouse pointer in the ListView control.
            Point cp = uxGroupNonMembersListView.PointToClient(new Point(e.X, e.Y));
            //Obtain the item that is located at the specified location of the mouse pointer.
            ListViewItem dragToItem = uxGroupNonMembersListView.GetItemAt(cp.X, cp.Y);
            if (dragToItem == null)
            {
                return;
            }
            //Obtain the index of the item at the mouse pointer.
            int dragIndex = dragToItem.Index;
            ListViewItem[] sel = new ListViewItem[uxGroupNonMembersListView.SelectedItems.Count];
            for (int i = 0; i <= uxGroupNonMembersListView.SelectedItems.Count - 1; i++)
            {
                sel[i] = uxGroupNonMembersListView.SelectedItems[i];
            }
            for (int i = 0; i < sel.GetLength(0); i++)
            {
                //Obtain the ListViewItem to be dragged to the target location.
                ListViewItem dragItem = sel[i];
                int itemIndex = dragIndex;
                if (itemIndex == dragItem.Index)
                {
                    return;
                }
                if (dragItem.Index < itemIndex)
                    itemIndex++;
                else
                    itemIndex = dragIndex + i;
                //Insert the item at the mouse pointer.
                ListViewItem insertItem = (ListViewItem)dragItem.Clone();
                uxGroupNonMembersListView.Items.Insert(itemIndex, insertItem);
                //Removes the item from the initial location while 
                //the item is moved to the new location.
                uxGroupNonMembersListView.Items.Remove(dragItem);
            }
        }

        private void uxSegmentNonMembersListView_DragEnter(object sender, DragEventArgs e)
        {
            int len = e.Data.GetFormats().Length - 1;
            int i;
            for (i = 0; i <= len; i++)
            {
                if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    //The data from the drag source is moved to the target.	
                    e.Effect = DragDropEffects.Move;
                }
            }
        }



        private void uxSegmentNonMembersListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            uxSegmentNonMembersListView.DoDragDrop(uxSegmentNonMembersListView.SelectedItems, DragDropEffects.Move);
        }



        private void uxSegmentMembersListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            uxSegmentMembersListView.DoDragDrop(uxSegmentMembersListView.SelectedItems, DragDropEffects.Move);
        }

        private void uxSegmentMembersListView_DragEnter(object sender, DragEventArgs e)
        {
            int len = e.Data.GetFormats().Length - 1;
            int i;
            for (i = 0; i <= len; i++)
            {
                if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    //The data from the drag source is moved to the target.	
                    e.Effect = DragDropEffects.Move;
                }
            }
        }



        private void uxSegmentMembersListView_DragDrop(object sender, DragEventArgs e)
        {
            //Return if the items are not selected in the ListView control.
            if (uxSegmentMembersListView.SelectedItems.Count == 0)
            {
                return;
            }
            //Returns the location of the mouse pointer in the ListView control.
            Point cp = uxSegmentMembersListView.PointToClient(new Point(e.X, e.Y));
            //Obtain the item that is located at the specified location of the mouse pointer.
            ListViewItem dragToItem = uxSegmentMembersListView.GetItemAt(cp.X, cp.Y);
            if (dragToItem == null)
            {
                return;
            }
            //Obtain the index of the item at the mouse pointer.
            int dragIndex = dragToItem.Index;
            ListViewItem[] sel = new ListViewItem[uxSegmentMembersListView.SelectedItems.Count];
            for (int i = 0; i <= uxSegmentMembersListView.SelectedItems.Count - 1; i++)
            {
                sel[i] = uxSegmentMembersListView.SelectedItems[i];
            }
            for (int i = 0; i < sel.GetLength(0); i++)
            {
                //Obtain the ListViewItem to be dragged to the target location.
                ListViewItem dragItem = sel[i];
                int itemIndex = dragIndex;
                if (itemIndex == dragItem.Index)
                {
                    return;
                }
                if (dragItem.Index < itemIndex)
                    itemIndex++;
                else
                    itemIndex = dragIndex + i;
                //Insert the item at the mouse pointer.
                ListViewItem insertItem = (ListViewItem)dragItem.Clone();
                uxSegmentMembersListView.Items.Insert(itemIndex, insertItem);
                //Removes the item from the initial location while 
                //the item is moved to the new location.
                uxSegmentMembersListView.Items.Remove(dragItem);
            }
        }

        private void uxCopyRouteButton_Click(object sender, EventArgs e)
        {
            if (selectedRoute != null)
            {
                MOE.Common.Business.Inrix.Route route = new MOE.Common.Business.Inrix.Route(selectedRoute);
                this.FillRoutes();
                selectedRoute = null;
                uxRoutesListView.SelectedItems.Clear();
                FillGroupMembers();
                FillGroupNonMembers();
            }
        }

        private void uxRouteMembersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void uxEditRouteButton_Click(object sender, EventArgs e)
        {
            EditRoute updateRoute = new EditRoute(this, this.selectedRoute);
            updateRoute.Show();

        }

        private void uxEditGroupButton_Click(object sender, EventArgs e)
        {
            EditGroup updateGroup = new EditGroup(this, this.selectedGroup);
            updateGroup.Show();
            FillGroupMembers();
            FillGroupNonMembers();
        }

        private void uxCopyGroupButton_Click(object sender, EventArgs e)
        {
            if (selectedGroup != null)
            {
                MOE.Common.Business.Inrix.Group group = new MOE.Common.Business.Inrix.Group(selectedGroup);
                this.FillGroups();
                selectedGroup = null;
                uxGroupsListView.SelectedItems.Clear();
                FillGroupMembers();
                FillGroupNonMembers();
            }
        }

        private void uxAddSegmentButton_Click(object sender, EventArgs e)
        {
            NewSegment newSegment = new NewSegment(this);
            newSegment.Show();
        }

        private void uxCopySegmentButon_Click(object sender, EventArgs e)
        {
            if (selectedSegment != null)
            {
                MOE.Common.Business.Inrix.Segment segment = new MOE.Common.Business.Inrix.Segment(selectedSegment);
                this.FillSegments();
                selectedGroup = null;
                uxGroupsListView.SelectedItems.Clear();
                FillSegmentMembers();
                FillSegmentNonMembers();
            }
        }

        private void uxSegmentsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedItem = this.uxSegmentsListView.SelectedItems;
            foreach (ListViewItem item in selectedItem)
            {
                selectedSegment = (item.Tag as MOE.Common.Business.Inrix.Segment);
            }
            if (selectedSegment.Description == "")
            {
                uxSegmentDescriptionTextBox.Text = selectedSegment.Name;
            }
            else { uxSegmentDescriptionTextBox.Text = selectedSegment.Description; }

            uxDeleteSegmentButton.Enabled = true;
            uxEditSegmentButton.Enabled = true;
            uxCopySegmentButton.Enabled = true;
            FillSegmentMembers();
            FillSegmentNonMembers();

        }

        private void uxAddTMCtoSegment_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in uxSegmentNonMembersListView.SelectedItems)
            {
                uxSegmentNonMembersListView.Items.Remove(item);
                uxSegmentMembersListView.Items.Add(item);

            }
        }

        private void uxRemoveTMCFromSegment_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in uxSegmentMembersListView.SelectedItems)
            {
                uxSegmentMembersListView.Items.Remove(item);
                uxSegmentNonMembersListView.Items.Add(item);

            }
        }

        private void uxSaveSegment_Click(object sender, EventArgs e)
        {
            if (selectedSegment != null)
            {
                selectedSegment.Items.Clear();

                foreach (ListViewItem item in uxSegmentMembersListView.Items)
                {
                    selectedSegment.Items.Add(item.Tag as MOE.Common.Business.Inrix.TMC);
                }
                selectedSegment.SaveMembers();
                FillSegmentMembers();
                FillSegmentNonMembers();
            }
        }

        private void uxDeleteSegmentButton_Click(object sender, EventArgs e)
        {
            selectedSegment.DeleteSegment();
            
            FillSegments();
        }

        private void uxEditSegmentButton_Click(object sender, EventArgs e)
        {
            if (selectedSegment != null)
            {
                EditSegment es = new EditSegment(this, selectedSegment);
                es.Show();
            }
        }

        
    }
}
