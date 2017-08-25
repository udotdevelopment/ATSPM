namespace MOE.Common.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MOE.Common.Business.SiteSecurity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<MOE.Common.Models.SPM>
    {
        private readonly bool _pendingMigrations;
        public Configuration()
        {
            
            //AutomaticMigrationsEnabled = true;
            //AutomaticMigrationDataLossAllowed = true;
            //var migrator = new DbMigrator(this);
            //_pendingMigrations = migrator.GetPendingMigrations().Any();

        }

        protected override void Seed(MOE.Common.Models.SPM context)
        {
           
            //  This method will be called after migrating to the latest version.


            context.FAQs.AddOrUpdate(
                f=> f.FAQID,
              new Models.FAQ
              {
                  OrderNumber = 1,
                  Header = @"<b>How do I navigate the UDOT Signal Performance Metrics website?</b>",
                               Body = @"<b>There are two ways to navigate the UDOT signal performance metric website</b><br/><br/><u>MAP</u><ol><li>Zoom in on the map and click on the desired intersection.</li><li>Select the available metric on the map from the list of available metrics for the desired intersection.</li><li>Click a day and/or time range from the calendar.</li><li>Click “Create Metric”.  Wait, and then scroll down to see the data and charts.</li></ol><u>SIGNAL LIST</u><ol><li>Select the metric by clicking the checkbox for the desired metric.</li><li>Click the “Signal List” bar at the top of the map window.</li><li>Click “Select” next to the desired intersection.</li><li>Click a day and/or time range from the calendar.</li><li>Click “Create Metric”.  Wait, and then scroll down to see the data and charts.</li></ol>"
              },
              new Models.FAQ
              {
                  OrderNumber = 2,
                  Header = @"<b>What are Signal Performance Metrics?</b>",
                  Body = @"Signal performance metrics show real-time and a history of performance at signalized intersections.  The various metrics will evaluate the quality of progression of traffic along the corridor, and displays any unused green time that may be available from various movements. This information informs UDOT of vehicle and pedestrian detector malfunctions, measures vehicle delay and lets us know volumes, speeds and travel time of vehicles.   The metrics are used to optimize mobility and manage traffic signal timing and maintenance to reduce congestion, save fuel costs and improve safety.  There are several metrics currently in use with others in development. "
              },
              new Models.FAQ
              {
                  OrderNumber = 3,
                  Header = @"<b>How do Signal Performance Metrics work?</b>",
                  Body = @"The traffic signal controller manufactures (Econolite, Intelight, and some others) wrote a “data-logger” program that runs in the background of the traffic signal controller firmware. The Indiana Traffic Signal Hi Resolution Data Logger Enumerations (http://docs.lib.purdue.edu/cgi/viewcontent.cgi?article=1002&context=jtrpdata) encode events to a resolution to the nearest 100 milliseconds.  The recorded enumerations will have events for “phase begin green”, “phase gap out”, “phase max out”, “phase begin yellow clearance”, “phase end yellow clearance”, “pedestrian begin walk”, “pedestrian begin clearance”, “detector off”, “detector on”, etc.  For each event, a time-stamp is given and the event is stored temporarily in the signal controller.  Over 125 various enumerations are currently in use.  Then, using an FTP connection from a remote server to the traffic signal controller, packets of the hi resolution data logger enumerations (with its 1/10th second resolution time-stamp) are retrieved and stored on a web server at the UDOT Traffic Operations Center about every 10 to 15 minutes (unless the “upload current data” checkbox is enabled, where an FTP connection will be immediately made and the data will be displayed in real-time).  Software was written in-house by UDOT that allows us to graph and display the various data-logger enumerations and to show the results on the UDOT Signal Performance Metric website."
              },
              new Models.FAQ
              {
                  OrderNumber = 4,
                  Header = @"<b>Which central traffic management system is used to get the Signal Performance Metrics?</b>",
                  Body = @"A central traffic management system is not used or needed for the UDOT Traffic Signal Performance Metrics.  It is all being done through FTP connections from a web server through the network directly to the traffic signal controller which currently has the Indiana Traffic Signal Hi Resolution Data Logger Enumerations running in the background of the controller firmware.  The UDOT Traffic Signal Performance Metrics are independent of any central traffic management system."
              },
              new Models.FAQ
              {
                  OrderNumber = 5,
                  Header = @"<b>Why does Utah need Automatic Signal Performance Metrics?</b>",
                  Body = @"In 2011, UDOT’s executive director assigned a Quality Improvement Team (QIT) to make recommendations that will result in UDOT providing “world-class traffic signal maintenance and operations”.  The QIT evaluated existing operations, national best practices, and NCHRP recommendations to better improve UDOT’s signal maintenance and operations practices.  One of the recommendations from the QIT was to “implement real-time monitoring of system health and quality of operations”.  The real-time signal performance metrics allow us to greatly improve the quality of signal operations and to also know when equipment such as pedestrian detection or vehicle detection is not working properly.  We are simply able to do more with less and to manage traffic more effectively 24/7.  In addition, we are able to optimize intersections and corridors when they need to be re-optimized, instead of on a set schedule."
              },
              new Models.FAQ
              {
                  OrderNumber = 6,
                  Header = @"<b>Where did you get the signal metric software?</b>",
                  Body = @"The UDOT Signal Metric software was developed in-house at UDOT by the Department of Technology Services.  Purdue University and the Indiana Department of Transportation (INDOT) assisted in getting us started on this endeavor."
              },
              new Models.FAQ
              {
                  OrderNumber = 7,
                  Header = @"<b>How did the Signal Performance Metrics Begin?</b> ",
                  Body = @"The Purdue coordination diagram concept was introduced in 2009 by Purdue University to visualize the temporal relationship between the coordinated phase indications and vehicle arrivals on a cycle-by-cycle basis.  The Indiana Traffic Signal HI Resolution Data Logger Enumerations (http://docs.lib.purdue.edu/cgi/viewcontent.cgi?article=1002&context=jtrpdata) was a joint transportation research program (updated in November 2012 but started earlier) that included people from Indiana DOT, Purdue University, Econolite, PEEK, and Siemens.  <br/><br/>After discussions with Dr. Darcy Bullock from Purdue University and INDOT’s James Sturdevant, UDOT started development of the UDOT Signal Performance Metrics website April of 2012."
              },
              new Models.FAQ
              {
                  OrderNumber = 8,
                  Header = @"<b>Why are there no passwords or firewalls to access the website and see the Metrics?</b>",
                  Body = @"UDOT’s goal is transparency and unrestricted access to all who have a desire for traffic signal data.  Our goal in optimizing mobility, improving safety, preserving infrastructure and strengthening the economy means that all who have a desire to use the data should have access to the data without restrictions.  This includes all of UDOT (various divisions and groups), consultants, academia, MPO’s, other jurisdictions, FHWA, the public, and others.  It is also UDOT’s goal to be the most transparent Department of Transportation in the country.  Having a website where real-time signal performance metrics can be obtained without special software, passwords or restricted firewalls will help UDOT in achieving the goal of transparency, and allows everyone access to the data without any silos."
              },
              new Models.FAQ
              {
                  OrderNumber = 9,
                  Header = @"<b>How do you use the various Signal Performance Metrics and what do they do?</b> ",
                  Body = @"There are many uses and benefits of the various metrics.  Some of the key uses are:<br/><br/><u>Purdue Coordination Diagrams (PCD’s)</u> – Used to visualize the temporal relationship between the coordinated phase indications and vehicle arrivals on a cycle-by-cycle basis.  The PCD’s show the progression quality along the corridor and answer questions, such as “What percent of vehicles are arriving during the green?” or “What is the platoon ratio during various coordination patterns?” The PCD’s are instrumental in optimizing offsets, identifying oversaturated or under saturated splits for the coordinated phases, the effects of early return of green and coordinated phase actuations, impacts of queuing, adjacent signal synchronization, etc.<br/> <br/>In reading the PCD’s, between the green and yellow lines, the phase is green; between the yellow and red lines, the phase is yellow; and underneath the green line, the phase is red.  The long vertical red lines during the late night reportTimespan is showing the main street sitting in green as the side streets and left turns are being skipped.  The short vertical red lines show skipping of the side street / left turns or a late start of green for the side street or left turn.  AoG is the percent of vehicles arriving during the green phase.  GT is the percent of green split time for the phase and PR is the Platoon Ratio (Equation 15-4 from the 2000 Highway Capacity Manual).<br/><br/><u>Approach Volumes</u> – Counts the approach vehicle volumes as shown arriving upstream of the intersection about 350 ft – 400 ft.  The detection zones are in advance of the turning lanes, so the approach volumes don’t know if the vehicles are going straight through, turning left or right.  The accuracy of the approach volumes tends to undercount under heavy traffic and under multi-lane facilities.  Approach volumes are used in traffic models, as well as identifying directional splits in traffic. In addition, the metric is also used in evaluating the least disruptive time to allow lanes to be taken for maintenance and construction activities.<br/><br/><u/>Approach Speeds</u> – The speeds are obtained from the Wavetronix radar Advance Smartsensor.  As vehicles cross the 10-foot wide detector in advance of the intersection (350 ft – 400 ft upstream of the stop bar), the speed is captured, recorded, and time-stamped.  In graphing the results, a time filter is used that starts 15 seconds (user defined) after the initial green to the start of the yellow.  The time filter allows for free-flow speed conditions to be displayed that are independent of the traffic signal timings.  The approach speed metric is beneficial in knowing the approach speeds to use for modeling purposes – both for normal time-of-day coordination plans and for adverse weather or special event plans.  They are also beneficial in knowing when speed conditions degrade enough to warrant a change in time-of-day coordination plans to adverse weather or special event plans.  In addition, the speed data is used to set yellow and all-red intervals for signal timing, as well as for various speed studies.<br/><br/><u>Purdue Phase Termination Charts</u> – Shows how each phase terminates when it changes from green to red.  The metric will show if the termination occurred by a gapout, a maxout / forceoff, or skip.  A gapout means that not all of the programmed time was used.  A maxout occurs during fully actuated (free) operations, while forceoff’s occur during coordination.  Both a maxout and forceoff shows that all the programmed time was used. A skip means that the phase was not active and did not turn on.  In addition, the termination can be evaluated by consecutive occurrences in a approach.  For example, you can evaluate if three (range is between 1 and 5) gapouts or skips occurred in a approach.  This feature is helpful in areas where traffic volume fluctuations are high.  Also shown are the pedestrian activations for each phase.  What this metric does not show is the amount of gapout time remaining if a gapout occurred.  The Split Monitor metric is used to answer that question.<br/><br/>This metric is used to identify movements where split time may need to be taken from some phases and given to other phases.  Also, this metric is very useful in identifying problems with vehicle and pedestrian detection.  For example, if the phase is showing constant maxouts all night long, it is assumed that there is a detection problem.<br/><br/><u>Split Monitor</u> – This metric shows the amount of split time (green, yellow and all-red) used by the various phases at the intersection.  Greens show gapouts, reds show maxouts, blues show forceoffs and yellows show pedestrian activations.  This metric is useful to know the amount of split time each phase uses.Turning Movement Volume Counts – this metric show the lane-by-lane vehicles per hour (vph) and total volume for each movement.  Three graphs are available for each approach (left, thru, right).  Also shown for each movement are the total volume, peak hour, peak hour factor and lane utilization factor.  The lane-by-lane volume counts are used for traffic models and traffic studies.<br/><br/><u>Approach Delay</u> – This metric shows a simplified approach delay by displaying the time between detector activations during the red phase and when the phase turns green for the coordinated movements.  This metric does not account for start-up delay, deceleration, or queue length that exceeds the detection zone.  This metric is beneficial in evaluating over time the delay per vehicle and delay per hour Values for each coordinated approach.<br/><br/><u>Arrivals on Red</u> – This metric shows the percent of vehicles arriving on red (inverse of % vehicles arriving on green) and the percent red time for each coordination pattern.  The Y axis is graphing the volume (vph) and the secondary Y axis graphs the percent vehicles arriving on red.  This metric is useful in identifying areas where the progression quality is poor.<br/><br/><u>Travel Time</u> – This metric shows the historical INRIX GPS data that UDOT has obtained for corridors statewide.  The data is a month or two old, however, various days and times-of-day can be graphed and compared."
              },
              new Models.FAQ
              {
                  OrderNumber = 10,
                  Header = @"<b>How effective are Signal Performance Metrics?</b>",
                  Body = @"The signal performance metrics are an effective way to reduce congestion, save fuel costs and improve safety.  We are simply able to do more with less and are more effectively able to manage traffic every day of the week and at all times of the day, even when a traffic signal engineer is not available.  We have identified several detection problems, corrected time-of-day coordination errors in the traffic signal controller scheduler, corrected offsets, splits, among other things.  In addition, we have been able to use more accurate data in optimizing models and doing traffic studies, and have been able to more correctly set various signal timing parameters."
              },
              new Models.FAQ
              {
                  OrderNumber = 11,
                  Header = @"<b>Does this mean I never have to stop at a red light?</b>",
                  Body = @"Although the UDOT Signal Performance Metrics cannot guarantee you will only get green lights, the system does help make traveling through Utah more efficient.  UDOT Automatic Signal Performance Measures have already already helped to reduce the number of stops and delay at signalized intersections.  Continued benefits are anticipated."
              },
              new Models.FAQ
              {
                  OrderNumber = 12,
                  Header = @"<b>Will Signal Performance Metrics save me money?  If so, how are cost savings measured?</b>",
                  Body = @"Yes, UDOT Signal Performance Metrics has already saved Utahans time and money.  By increasing corridor speeds while reducing intersection delays, traffic signal stops, and the ability to monitor operations 24/7."
              },
              new Models.FAQ
              {
                  OrderNumber = 13,
                  Header = @"<b>How do Signal Performance Metrics enhance safety?</b>",
                  Body = @"By reducing congestion and reducing the percent of vehicles arriving on a red light, UDOT Signal Performance Metrics helps decrease the number of accidents that occur.  In addition, we are better able to manage detector failures and improve the duration of the change intervals and clearance times at intersections."
              },
              new Models.FAQ
              {
                  OrderNumber = 14,
                  Header = @"<b>Can real-time Signal Performance Metrics be used as a law enforcement tool?</b>",
                  Body = @"UDOT Signal Performance Metrics are designed to increase the safety and efficiency at signalized intersections.  It is not intended to identify speeders or enforce traffic laws.  No personal information is recorded or used in data gathering."
              },
              new Models.FAQ
              {
                  OrderNumber = 15,
                  Header = @"<b>Server and Data Storage Requirements</b>",
                  Body = @"We can estimate that each signal controller requires 11 MB of storage space each day.  For the UDOT system, that means roughly 10 GB of data growth every day."
              },
              new Models.FAQ
              {
                  OrderNumber = 16,
                  Header = @"<b>Who uses the Signal Performance Metrics data?</b>",
                  Body = @"The data has been useful for some of the following users in Utah:<br/><br/><ul><li><u>Signal engineers</u> in optimize and fine-tuning signal timing.</li><li><u>Maintenance signal technicians</u> in identifying broken detector problems and investigating trouble calls.</li><li><u>Traffic engineers</u> in conducting various traffic studies, such as speed studies, turning movement studies, modeling studies, and optimizing the intersection operations.</li><li><u>Consultants</u> in improving traffic signal operations, as UDOT outsources some of the signal operations, design and planning to consultants.</li><li><u>UDOT Traffic & Safety, UDOT Traffic Engineers, UDOT Resident Construction Engineers</u> in conducting various traffic studies and/or in determining the time-of-day where construction or maintenance activities would be least disruptive to the traveling motorists.</li><li><u>Metropolitan Planning Organizations</u> (MPO’s) in calibrating the regional traffic models.</li><li><u>Academia</u> in conducting various research studies, such as evaluating the effectiveness of operations during adverse weather, evaluating the optimum signal timing for innovative intersections such as DDI’s, CFI’s and Thru-Turns, etc.</li><li><u>City and County</u> Government in using the data in similar manner to UDOT.</li></ul>"
              },
              new Models.FAQ
              {
                  OrderNumber = 17,
                  Header = @"<b>What are the detection requirements for each metric?</b> ",
                  Body = @"<table><tr><td><b>METRIC</b></td><td><b>DETECTION NEEDED</b></td></tr><tr><td>Purdue Coordination Diagram &nbsp;&nbsp;&nbsp;&nbsp;</td><td>Setback count (350 ft – 400 ft)</td></tr><tr><td>Approach Volume</td><td>Setback count (350 ft – 400 ft)</td></tr><tr><td>Approach Speed</td><td>Setback count (350 ft – 400 ft) using radar</td></tr><tr><td>Purdue Phase Termination</td><td>No detection needed or used</td></tr><tr><td>Split Monitor</td><td>No detection needed or used</td></tr><tr><td>Turning Movement Counts</td><td>Stop bar (lane-by-lane) count</td></tr><tr><td>Approach Delay</td><td>Setback count (350 ft – 400 ft)</td></tr><tr><td>Arrivals on Red</td><td>Setback count (350 ft – 400 ft)</td></tr><tr><td>Travel Time</td><td>Historical INRIX Data</td></tr></table><br/><br/><b>Signal Performance Metrics will work with any type of detector that is capable of counting vehicles (i.e. loops, video, pucks, radar).  The only exception to this is the speed metric, where UDOT’s Signal Performance Metrics for speeds will only work with the Wavetronix Advance Smartsensor).  Please note that two of the metrics (Purdue Phase Termination and Split Monitor) do not use detection and are extremely useful metrics.</b>"
              },
              new Models.FAQ
              {
                  OrderNumber = 18,
                  Header = @"<b>Why do some intersections only show a few metrics and others have more?</b>",
                  Body = @"Some metrics have different detection requirements than other metrics.  For example, for approach speeds, UDOT uses the Wavetronix Advance Smartsensor radar detector and has been using this detector since 2006 for dilemma zone purposes if the design speed is 40 mph or higher.  This same detector is what we use for our setback counts 350 feet – 400 feet upstream of the intersection.  In addition, we are also able to pull the raw radar speeds from the sensor back to the TOC server for the speed data.  Not all intersections have the Wavetronix Advance Smartsensors, therefore we are not able to display speed data, as well as the PCD’s, approach volume, arrivals on red or approach delay at each intersection.<br/><br/>The turning movement count metric requires lane-by-lane detection capable of counting vehicles in each lane.  Configuring the detection for lane-by-lane counts is time consuming and takes a commitment to financial resources."
              },
              new Models.FAQ
              {
                  OrderNumber = 19,
                  Header = @"<b>What are the System Requirements?</b>",
                  Body = @"<b>System Requirements:</b>

<b>Operating Systems and Software:</b>

The UDOT Signal Performance Metrics system runs on Microsoft Windows Servers.

The web components are hosted by Microsoft Internet Information Server (IIS).

The database server is a Microsoft SQL 2008 server.

<b>Storage and Processing:</b>

Detector data uses about 40% of the storage space of the UDOT system, so the number of detectors attached to a controller will have a huge impact on the amount of storage space required.  Detector data is also the most important information we collect.

We estimate that each signal will generate 11.4 MB of data per day.

The amount of processing power required is highly dependant on how many signals are on the system, how many servers will be part of the system, and how many people will be using the system.  It is possible to host all of the system functions on one powerful server, or split them out into multiple, less expensive servers.  If your agency decided to make the performance metrics available to the public, it might be best to have a web server separate from the database server.  Much of the heavy processing for the charts is done by web services, and it is possible to host these services on a dedicated computer.  

While each agency should consult with their IT department for specific guidelines on how to best deliver a secure, stable and responsive solution, we can estimate that most mid-range to high-end servers will be able to handle the task of hosting and creating metrics for most agencies."
              },
              new Models.FAQ
              {
                  OrderNumber = 20,
                  Header = @"<b>Who do I contact to find out more information about Automatic Signal Performance Metrics?</b> ",
                  Body = @"You can contact UDOT’s Traffic Signal Operations Engineer, Mark Taylor at marktaylor@utah.gov or phone at 801-887-3714 to find out more information about Automatic Signal Performance Metrics."
              },
              new Models.FAQ
              {
                  OrderNumber = 21,
                  Header = @"<b>How do I get the source code for the Signal Performance Metrics Website?</b> ",
                  Body = @"You can download the source code <a href=''>here</a>."
              }
              );

            context.Menus.AddOrUpdate(
              m => m.MenuId,
              new Models.Menu { MenuId = 1, MenuName = "Measures", Controller = "#", Action = "#", ParentId = 0, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
              new Models.Menu { MenuId = 2, MenuName = "Reports", Controller = "#", Action = "#", ParentId = 0, Application = "SignalPerformanceMetrics", DisplayOrder = 2 },
              new Models.Menu { MenuId = 3, MenuName = "Log Action Taken", Controller = "ActionLogs", Action ="Create", ParentId = 0, Application = "SignalPerformanceMetrics", DisplayOrder = 3 },
              new Models.Menu { MenuId = 4, MenuName = "Links", Controller = "#", Action = "#", ParentId = 0, Application = "SignalPerformanceMetrics", DisplayOrder = 4 },
                new Models.Menu { MenuId = 30, MenuName = "ATSPM Documents", Controller = "#", Action = "#", ParentId = 4, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
               new Models.Menu { MenuId = 34, MenuName = "GDOT ATSPM Component Details", Controller = "Images", Action = "ATSPM_Component_Details.pdf", ParentId = 30, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
              new Models.Menu { MenuId = 31, MenuName = "ATSPM Presentations", Controller = "#", Action = "#", ParentId = 4, Application = "SignalPerformanceMetrics", DisplayOrder = 2 },
               new Models.Menu { MenuId = 35, MenuName = "ATSPM UDOT Conference 11-2-16", Controller = "Images", Action = "ATSPM_UDOT_Conference_11-2-16.pdf", ParentId = 31, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
               new Models.Menu { MenuId = 36, MenuName = "ATSPM EDC4 Minnesota 10-25-16", Controller = "Images", Action = "ATSPM_EDC4_Minnesota_10-25-16.pdf", ParentId = 31, Application = "SignalPerformanceMetrics", DisplayOrder = 2 },
               new Models.Menu { MenuId = 37, MenuName = "ATSPM CO WY ITE & Rocky Mtn 10-20-16", Controller = "Images", Action = "ATSPM_CO-WY_ITE___ITS_Rocky_Mtn_10-20-16.pdf", ParentId = 31, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
               new Models.Menu { MenuId = 38, MenuName = "ATSPM ITS California 9-21-16", Controller = "Images", Action = "ATSPM_ITS_CA_9-21-16.pdf", ParentId = 31, Application = "SignalPerformanceMetrics", DisplayOrder = 2 },             
              new Models.Menu { MenuId = 32, MenuName = "UDOT Traffic Signal Documents", Controller = "#", Action = "#", ParentId = 4, Application = "SignalPerformanceMetrics", DisplayOrder = 3 },
               new Models.Menu { MenuId = 39, MenuName = "TSMP UDOT V1-2 2-5-16", Controller = "Images", Action = "TSMP_UDOT_v1-2_2-5-16.pdf", ParentId = 32, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
               new Models.Menu { MenuId = 40, MenuName = "Emergency Traffic Signal Response Plan UDOT 5-6-16", Controller = "Images", Action = "EmergencyTrafficSignalResponsePlanUDOT5-6-16.pdf", ParentId = 32, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
              new Models.Menu { MenuId = 5, MenuName = "FAQ", Controller = "FAQs", Action = "Display", ParentId = 0, Application = "SignalPerformanceMetrics", DisplayOrder = 5 },
              new Models.Menu { MenuId = 8, MenuName = "Chart Usage", Controller = "ActionLogs", Action = "Usage", ParentId = 2, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
              new Models.Menu { MenuId = 9, MenuName = "Signal", Controller = "DefaultCharts", Action = "Index", ParentId = 1, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
              new Models.Menu { MenuId = 10, MenuName = "Purdue Link Pivot", Controller = "LinkPivot", Action="Analysis", ParentId = 1, Application = "SignalPerformanceMetrics", DisplayOrder = 2 },
              new Models.Menu { MenuId = 11, MenuName = "Admin", Controller = "#", Action = "#", ParentId = 0, Application = "SignalPerformanceMetrics", DisplayOrder = 6 },
              new Models.Menu { MenuId = 12, MenuName = "Signal Configuration", Controller = "Signals", Action = "Index", ParentId = 11, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
              new Models.Menu { MenuId = 13, MenuName = "Route Configuration", Controller = "ApproachRoutes", Action = "Index", ParentId = 11, Application = "SignalPerformanceMetrics", DisplayOrder = 2 },
              new Models.Menu { MenuId = 14, MenuName = "Security", Controller = "#", Action = "#", ParentId = 11, Application = "SignalPerformanceMetrics", DisplayOrder = 4 },
              new Models.Menu { MenuId = 15, MenuName = "Roles", Controller = "Account", Action = "RoleAddToUser", ParentId = 14, Application = "SignalPerformanceMetrics", DisplayOrder = 2 },
              new Models.Menu { MenuId = 16, MenuName = "Menu Configuration", Controller = "Menus", Action = "Index", ParentId = 11, Application = "SignalPerformanceMetrics", DisplayOrder = 3 },
              new Models.Menu { MenuId = 27, MenuName = "About", Controller = "Home", Action = "About", ParentId = 0, Application = "SignalPerformanceMetrics", DisplayOrder = 7 },
              new Models.Menu { MenuId = 42, MenuName = "GDOT ATSPM Installation Manual", Controller = "Images", Action = "ATSPM_Installation_Manual.pdf", ParentId = 30, Application = "SignalPerformanceMetrics", DisplayOrder = 2 },
              new Models.Menu { MenuId = 43, MenuName = "GDOT ATSPM Reporting Details", Controller = "Images", Action = "ATSPM_Reporting_Details.pdf", ParentId = 30, Application = "SignalPerformanceMetrics", DisplayOrder = 3},
              new Models.Menu { MenuId = 50, MenuName = "Configuration", Controller = "Signals", Action = "SignalDetail", ParentId = 2, Application = "SignalPerformanceMetrics", DisplayOrder = 2 },
              new Models.Menu { MenuId = 51, MenuName = "Users", Controller = "SPMUsers", Action = "Index", ParentId = 14, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
              new Models.Menu { MenuId = 52, MenuName = "FAQs", Controller = "FAQs", Action = "Index", ParentId = 11, Application = "SignalPerformanceMetrics", DisplayOrder = 6 },
              new Models.Menu { MenuId = 53, MenuName = "Application Settings", Controller = "#", Action = "#", ParentId = 11, Application = "SignalPerformanceMetrics", DisplayOrder = 5 },
              new Models.Menu { MenuId = 54, MenuName = "Watch Dog", Controller = "WatchDogApplicationSettings", Action = "Edit", ParentId = 53, Application = "SignalPerformanceMetrics", DisplayOrder = 1 },
              new Models.Menu { MenuId = 55, MenuName = "Detector Accuracy Information", Controller = "Images", Action = "DetectorAccuracyInformation.pdf", ParentId = 30, Application = "SignalPerformanceMetrics", DisplayOrder = 4 }, 
              new Models.Menu { MenuId = 44, MenuName = "Train the Trainer", Controller = "#", Action = "#", ParentId = 31, Application = "SignalPerformanceMetrics", DisplayOrder = 5 },
              new Models.Menu { MenuId = 45, MenuName = "Mark Taylor", Controller = "Images", Action = "TTTMarkTaylor.pdf", ParentId = 44, Application = "SignalPerformanceMetrics", DisplayOrder = 3 },
              new Models.Menu { MenuId = 46, MenuName = "Jamie Mackey", Controller = "Images", Action = "TTTJamieMackey.pdf", ParentId = 44, Application = "SignalPerformanceMetrics", DisplayOrder = 2 },
              new Models.Menu { MenuId = 47, MenuName = "Derek Lowe & Shane Johnson", Controller = "Images", Action = "TTTDerekLoweShaneJohnson.pdf", ParentId = 44, Application = "SignalPerformanceMetrics", DisplayOrder = 1 }                        
              
            );

            context.ExternalLinks.AddOrUpdate(
                c => c.DisplayOrder,
                new Models.ExternalLink { Name = "Indiana Hi Resolution Data Logger Enumerations", DisplayOrder = 1, Url = " http://docs.lib.purdue.edu/jtrpdata/3/" },
                new Models.ExternalLink { Name = "Seminole County, Florida", DisplayOrder = 2, Url = "http://spm.seminolecountyfl.gov/signalperformancemetrics" },
                new Models.ExternalLink { Name = "FAST (Southern Nevada)", DisplayOrder = 3, Url = "http://challenger.nvfast.org/spm" }
               );

            context.ControllerType.AddOrUpdate(
                c => c.ControllerTypeID,
                new Models.ControllerType { ControllerTypeID = 1, Description = "ASC3", SNMPPort = 161, FTPDirectory = "//Set1", ActiveFTP = true, UserName = "econolite", Password = "ecpi2ecpi" },
                new Models.ControllerType { ControllerTypeID = 2, Description = "Cobalt", SNMPPort = 161, FTPDirectory = "/set1", ActiveFTP = true, UserName = "econolite", Password = "ecpi2ecpi" },
                new Models.ControllerType { ControllerTypeID = 3, Description = "ASC3 - 2070", SNMPPort = 161, FTPDirectory = "/set1", ActiveFTP = true, UserName = "econolite", Password = "ecpi2ecpi" },
                new Models.ControllerType { ControllerTypeID = 4, Description = "MaxTime", SNMPPort = 161, FTPDirectory = "none", ActiveFTP = false, UserName = "none", Password = "none" },
                new Models.ControllerType { ControllerTypeID = 5, Description = "Trafficware", SNMPPort = 161, FTPDirectory = "none", ActiveFTP = true, UserName = "none", Password = "none" },
                new Models.ControllerType { ControllerTypeID = 6, Description = "Siemens SEPAC", SNMPPort = 161, FTPDirectory = "/mnt/sd", ActiveFTP = false, UserName = "admin", Password = "$adm*kon2" },
                new Models.ControllerType { ControllerTypeID = 7, Description = "McCain ATC EX", SNMPPort = 161, FTPDirectory = " /mnt/rd/hiResData", ActiveFTP = false, UserName = "root", Password = "root" },
                new Models.ControllerType { ControllerTypeID = 8, Description = "Peek", SNMPPort = 161, FTPDirectory = "mnt/sram/cuLogging", ActiveFTP = false, UserName = "atc", Password = "PeekAtc" }
                );

            context.DetectionTypes.AddOrUpdate(
                c => c.DetectionTypeID,
                new Models.DetectionType {DetectionTypeID = 1, Description = "Basic" },
                new Models.DetectionType {DetectionTypeID = 2, Description = "Advanced Count"},
                new Models.DetectionType { DetectionTypeID = 3, Description = "Advanced Speed" },
                new Models.DetectionType {DetectionTypeID = 4, Description = "Lane-by-lane Count" },
                new Models.DetectionType { DetectionTypeID = 5, Description = "Lane-by-lane with Speed Restriction" },
                new Models.DetectionType { DetectionTypeID = 6, Description = "Stop Bar Presence" }

             );

            //context.SaveChanges();
            //var detectionTypes = context.DetectionTypes.ToDictionary(d => d.DetectionTypeID, d => d);
            //var dt1 = new Models.DetectionType { DetectionTypeID = 1, Description = "Basic", MetricTypes = new List<Models.MetricType>() };
            //var dt2 = new Models.DetectionType { DetectionTypeID = 2, Description = "Advanced Count", MetricTypes = new List<Models.MetricType>() };
            //var dt3 = new Models.DetectionType { DetectionTypeID = 3, Description = "Advanced Speed", MetricTypes = new List<Models.MetricType>() };
            //var dt4 = new Models.DetectionType { DetectionTypeID = 4, Description = "Lane-by-lane Count", MetricTypes = new List<Models.MetricType>() };
            //var dt5 = new Models.DetectionType { DetectionTypeID = 5, Description = "Lane-by-lane with Speed Restriction", MetricTypes = new List<Models.MetricType>() };
            //var dt6 = new Models.DetectionType { DetectionTypeID = 6, Description = "Stop Bar Presence", MetricTypes = new List<Models.MetricType>() };

            context.MetricTypes.AddOrUpdate(
                c => c.MetricID,
                new Models.MetricType { MetricID = 1, ChartName = "Purdue Phase Termination", Abbreviation = "PPT", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 2, ChartName = "Split Monitor", Abbreviation = "SM", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 3, ChartName = "Pedestrian Delay", Abbreviation = "PedD", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 4, ChartName = "Preemption Details", Abbreviation = "PD", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 5, ChartName = "Turning Movement Counts", Abbreviation = "TMC", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 6, ChartName = "Purdue Coordination Diagram", Abbreviation = "PCD", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 7, ChartName = "Approach Volume", Abbreviation = "AV", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 8, ChartName = "Approach Delay", Abbreviation = "AD", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 9, ChartName = "Arrivals On Red", Abbreviation = "AoR", ShowOnWebsite = true, },
                new Models.MetricType { MetricID = 10, ChartName = "Approach Speed", Abbreviation = "Speed", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 11, ChartName = "Yellow and Red Actuations", Abbreviation = "YRA", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 12, ChartName = "Purdue Split Failure", Abbreviation = "SF", ShowOnWebsite = true },
                new Models.MetricType { MetricID = 13, ChartName = "Purdue Link Pivot", Abbreviation = "LP", ShowOnWebsite = false },
                new Models.MetricType { MetricID = 14, ChartName = "Preempt Service Request", Abbreviation = "PSR", ShowOnWebsite = false },
                new Models.MetricType { MetricID = 15, ChartName = "Preempt Service", Abbreviation = "PS", ShowOnWebsite = false }
                );
            context.SaveChanges();
            //var mt1 = new Models.MetricType { MetricID = 1, ChartName = "Purdue Phase Termination", Abbreviation = "PPT", ShowOnWebsite = true };
            //var mt2 = new Models.MetricType { MetricID = 2, ChartName = "Split Monitor", Abbreviation = "SM", ShowOnWebsite = true };
            //var mt3 = new Models.MetricType { MetricID = 3, ChartName = "Pedestrian Delay", Abbreviation = "PedD", ShowOnWebsite = true };
            //var mt4 = new Models.MetricType { MetricID = 4, ChartName = "Preemption Details", Abbreviation = "PD", ShowOnWebsite = true };
            //var mt5 = new Models.MetricType { MetricID = 5, ChartName = "Turning Movement Counts", Abbreviation = "TMC", ShowOnWebsite = true };
            //var mt6 = new Models.MetricType { MetricID = 6, ChartName = "Purdue Coordination Diagram", Abbreviation = "PCD", ShowOnWebsite = true };
            //var mt7 = new Models.MetricType { MetricID = 7, ChartName = "Approach Volume", Abbreviation = "AV", ShowOnWebsite = true };
            //var mt8 = new Models.MetricType { MetricID = 8, ChartName = "Approach Delay", Abbreviation = "AD", ShowOnWebsite = true };
            //var mt9 = new Models.MetricType { MetricID = 9, ChartName = "Arrivals On Red", Abbreviation = "AoR", ShowOnWebsite = true, };
            //var mt10 = new Models.MetricType { MetricID = 10, ChartName = "Approach Speed", Abbreviation = "Speed", ShowOnWebsite = true };
            //var mt11 = new Models.MetricType { MetricID = 11, ChartName = "Yellow and Red Actuations", Abbreviation = "YRA", ShowOnWebsite = true };
            //var mt12 = new Models.MetricType { MetricID = 12, ChartName = "Purdue Split Failure", Abbreviation = "SF", ShowOnWebsite = true };
            //var mt13 = new Models.MetricType { MetricID = 13, ChartName = "Purdue Link Pivot", Abbreviation = "LP", ShowOnWebsite = false };
            //var mt14 = new Models.MetricType { MetricID = 14, ChartName = "Preempt Service Request", Abbreviation = "PSR", ShowOnWebsite = false };
            //var mt15 = new Models.MetricType { MetricID = 15, ChartName = "Preempt Service", Abbreviation = "PS", ShowOnWebsite = false };

            //dt1.MetricTypes.Add(mt1);
            //dt1.MetricTypes.Add(mt2);
            //dt1.MetricTypes.Add(mt3);
            //dt1.MetricTypes.Add(mt4);
            //dt1.MetricTypes.Add(mt14);
            //dt1.MetricTypes.Add(mt15);

            //dt2.MetricTypes.Add(mt6);
            //dt2.MetricTypes.Add(mt7);
            //dt2.MetricTypes.Add(mt8);
            //dt2.MetricTypes.Add(mt9);
            //dt2.MetricTypes.Add(mt13);

            //dt3.MetricTypes.Add(mt10);

            //dt4.MetricTypes.Add(mt5);
            //dt4.MetricTypes.Add(mt7);
            //dt4.MetricTypes.Add(mt8);
            //dt4.MetricTypes.Add(mt9);

            //dt5.MetricTypes.Add(mt11);

            //dt6.MetricTypes.Add(mt12);         

            //context.DetectionTypes.AddOrUpdate(d => d.DetectionTypeID, dt1, dt2, dt3, dt4, dt5, dt6);
            foreach(var detectionType in context.DetectionTypes)
            {
                switch(detectionType.DetectionTypeID)
                {
                    case 1:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(1));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(2));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(3));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(4));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(14));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(15));
                        break;
                    case 2:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(6));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(7));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(8));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(9));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(13));
                        break;
                    case 3:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(10));
                        break;
                    case 4:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(5));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(7));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(8));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(9));
                        break;
                    case 5:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(11));
                        break;
                    case 6:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(12)); 
                        break;
                }
            }
            context.SaveChanges();

            //context.MetricTypes.AddOrUpdate(
            //    c => c.ChartName,
            //    new Models.MetricType { MetricID = 1, ChartName = "Purdue Phase Termination", Abbreviation = "PPT" ,ShowOnWebsite = true    },
            //    new Models.MetricType { MetricID = 2, ChartName = "Split Monitor", Abbreviation = "SM", ShowOnWebsite = true                },
            //    new Models.MetricType { MetricID = 3, ChartName = "Pedestrian Delay", Abbreviation = "PedD", ShowOnWebsite = true           },
            //    new Models.MetricType { MetricID = 4, ChartName = "Preemption Details", Abbreviation = "PD", ShowOnWebsite = true           },
            //    new Models.MetricType { MetricID = 5, ChartName = "Turning Movement Counts", Abbreviation = "TMC", ShowOnWebsite = true     },
            //    new Models.MetricType { MetricID = 6, ChartName = "Purdue Coordination Diagram", Abbreviation = "PCD", ShowOnWebsite = true },
            //    new Models.MetricType { MetricID = 7, ChartName = "Approach Volume", Abbreviation = "AV", ShowOnWebsite = true              },
            //    new Models.MetricType { MetricID = 8, ChartName = "Approach Delay", Abbreviation = "AD", ShowOnWebsite = true               },
            //    new Models.MetricType { MetricID = 9, ChartName = "Arrivals On Red", Abbreviation = "AoR", ShowOnWebsite = true,             },
            //    new Models.MetricType { MetricID = 10, ChartName = "Approach Speed", Abbreviation = "Speed", ShowOnWebsite = true           },
            //    new Models.MetricType { MetricID = 11, ChartName = "Yellow and Red Actuations", Abbreviation = "YRA", ShowOnWebsite = true  },
            //    new Models.MetricType { MetricID = 12, ChartName = "Purdue Split Failure", Abbreviation = "SF", ShowOnWebsite = true        },
            //    new Models.MetricType { MetricID = 13, ChartName = "Purdue Link Pivot", Abbreviation = "LP", ShowOnWebsite = false          },
            //    new Models.MetricType { MetricID = 14, ChartName = "Preempt Service Request", Abbreviation = "PSR", ShowOnWebsite = false   },
            //    new Models.MetricType { MetricID = 15, ChartName = "Preempt Service", Abbreviation = "PS", ShowOnWebsite = false            }
            //    );

            context.SaveChanges();

            context.MetricsFilterTypes.AddOrUpdate(
                c => c.FilterName,
                new Models.MetricsFilterType { FilterName = "Signal ID" },
                new Models.MetricsFilterType { FilterName = "Primary Name" },
                new Models.MetricsFilterType { FilterName = "Secondary Name" }
                );

            context.Applications.AddOrUpdate(
                c => c.ID,
                new Models.Application { ID = 1, Name = "ATSPM"  },
                new Models.Application { ID = 2, Name = "SPMWatchDog"}
                );

            context.WatchdogApplicationSettings.AddOrUpdate(
                c => c.ApplicationID,
                new Models.WatchDogApplicationSettings {  ApplicationID = 2, ConsecutiveCount = 3, DefaultEmailAddress = "dlowe@utah.gov", EmailServer = "send.state.ut.us", FromEmailAddress= "SPMWatchdog@utah.gov", LowHitThreshold = 50, MaxDegreeOfParallelism = 4, MinimumRecords = 500, MinPhaseTerminations = 50, PercentThreshold = .9, PreviousDayPMPeakEnd = 18, PreviousDayPMPeakStart = 17, ScanDayEndHour = 5, ScanDayStartHour = 1, WeekdayOnly = true, MaximumPedestrianEvents=200   }
                );

            context.LaneTypes.AddOrUpdate(
            new Models.LaneType { LaneTypeID = 1, Description = "Vehicle", Abbreviation = "V" },
            new Models.LaneType { LaneTypeID = 2, Description = "Bike", Abbreviation = "Bike" },
            new Models.LaneType { LaneTypeID = 3, Description = "Pedestrian", Abbreviation = "Ped" },
            new Models.LaneType { LaneTypeID = 4, Description = "Exit", Abbreviation = "E" },
            new Models.LaneType { LaneTypeID = 5, Description = "Light Rail Transit", Abbreviation = "LRT" },
            new Models.LaneType { LaneTypeID = 6, Description = "Bus", Abbreviation = "Bus" },
            new Models.LaneType { LaneTypeID = 7, Description = "High Occupancy Vehicle", Abbreviation = "HOV" }


            );

            context.MovementTypes.AddOrUpdate(
                new Models.MovementType { MovementTypeID = 1, Description = "Thru", Abbreviation = "T", DisplayOrder = 3 },
                new Models.MovementType { MovementTypeID = 2, Description = "Right", Abbreviation = "R", DisplayOrder = 5 },
                new Models.MovementType { MovementTypeID = 3, Description = "Left", Abbreviation = "L", DisplayOrder = 1 },
                new Models.MovementType { MovementTypeID = 4, Description = "Thru-Right", Abbreviation = "TR", DisplayOrder = 4 },
                new Models.MovementType { MovementTypeID = 5, Description = "Thru-Left", Abbreviation = "TL", DisplayOrder = 2 }

                );

            context.DirectionTypes.AddOrUpdate(
                new Models.DirectionType { DirectionTypeID = 1, Description =  "Northbound", Abbreviation = "NB", DisplayOrder = 3},
                new Models.DirectionType { DirectionTypeID = 2, Description = "Southbound", Abbreviation = "SB", DisplayOrder = 4},
                new Models.DirectionType { DirectionTypeID = 3, Description =  "Eastbound", Abbreviation = "EB", DisplayOrder=1},
                new Models.DirectionType { DirectionTypeID = 4, Description = "Westbound", Abbreviation = "WB", DisplayOrder = 2 },
                new Models.DirectionType { DirectionTypeID = 5, Description =  "Northeast", Abbreviation = "NE", DisplayOrder = 5},
                new Models.DirectionType { DirectionTypeID = 6, Description = "Northwest", Abbreviation = "NW", DisplayOrder = 6 },
                new Models.DirectionType { DirectionTypeID = 7, Description = "Southeast", Abbreviation = "SE", DisplayOrder = 7 },
                new Models.DirectionType { DirectionTypeID = 8, Description = "Southwest", Abbreviation = "SW", DisplayOrder = 8 }
   
                );

            context.Regions.AddOrUpdate(
                
                new Models.Region {ID = 1, Description = "Region 1"},
                new Models.Region {ID = 2, Description = "Region 2"},
                new Models.Region {ID = 3, Description = "Region 3"},
                new Models.Region { ID = 4, Description = "Region 4" },
                new Models.Region { ID = 10, Description = "Other" }

        );

            context.Agencies.AddOrUpdate(

                new Models.Agency { AgencyID = 1, Description = "Academics" },
                new Models.Agency { AgencyID = 2, Description = "City Government" },
                new Models.Agency { AgencyID = 3, Description = "Consultant" },
                new Models.Agency { AgencyID = 4, Description = "County Government" },
                new Models.Agency { AgencyID = 5, Description = "Federal Government" },
                new Models.Agency { AgencyID = 6, Description = "MPO" },
                new Models.Agency { AgencyID = 7, Description = "State Government" },
                new Models.Agency { AgencyID = 8, Description = "Other" }

        );
            context.Actions.AddOrUpdate(

                new Models.Action { ActionID = 1, Description = "Actuated Coord." },
                new Models.Action { ActionID = 2, Description = "Coord On/Off" },
                new Models.Action { ActionID = 3, Description = "Cycle Length" },
                new Models.Action { ActionID = 4, Description = "Detector Issue" },
                new Models.Action { ActionID = 5, Description = "Offset" },
                new Models.Action { ActionID = 6, Description = "Sequence" },
                new Models.Action { ActionID = 7, Description = "Time Of Day" },
                new Models.Action { ActionID = 8, Description = "Other" },
                new Models.Action { ActionID = 9, Description = "All-Red Interval" },
                new Models.Action { ActionID = 10, Description = "Modeling" },
                new Models.Action { ActionID = 11, Description = "Traffic Study" },
                new Models.Action { ActionID = 12, Description = "Yellow Interval" },
                new Models.Action { ActionID = 13, Description = "Force Off Type" },
                new Models.Action { ActionID = 14, Description = "Split Adjustment" },
                new Models.Action { ActionID = 15, Description = "Manual Command" }

        );

            context.DetectionHardwares.AddOrUpdate(
                new Models.DetectionHardware { ID = 0, Name = "Unknown" },
                new Models.DetectionHardware { ID = 1, Name = "Wavetronix Matrix"},
                new Models.DetectionHardware { ID = 2, Name = "Wavetronix Advance"},               
                new Models.DetectionHardware { ID = 3, Name = "Inductive Loops"},
                new Models.DetectionHardware { ID = 4, Name = "Sensys"},
                new Models.DetectionHardware { ID = 5, Name = "Video" }
                );

            //These are default values.  They need to be changed before the system goes into production.
            
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<SPMUser>(context);
            var userManager = new UserManager<SPMUser>(userStore);
            if (userManager.FindByName("DefaultAdmin@SPM.Gov") == null)
            {
                var user = new SPMUser { UserName = "DefaultAdmin@SPM.Gov".ToLower(), Email = "DefaultAdmin@SPM.Gov".ToLower() };
                userManager.Create(user, "L3tM3in!");
                roleManager.Create(new IdentityRole("Admin"));
                roleManager.Create(new IdentityRole("User"));
                userManager.AddToRole(user.Id, "Admin");
                userManager.AddToRole(user.Id, "User");
                roleManager.Create(new IdentityRole("Technician"));
                userManager.AddToRole(user.Id, "Technician");
            }
            else
            {
                var user = userManager.FindByName("DefaultAdmin@SPM.Gov");
                roleManager.Create(new IdentityRole("Technician"));
                userManager.AddToRole(user.Id, "Technician");
            }

            context.SaveChanges();
            
        }
    }
}
