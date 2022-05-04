using System;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MOE.Common.Business.SiteSecurity;
using MOE.Common.Models;
using Action = MOE.Common.Models.Action;

namespace MOE.Common.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<SPM>
    {
        private readonly bool _pendingMigrations;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            var migrator = new DbMigrator(this);
            //_pendingMigrations = migrator.GetPendingMigrations().Any();
            CommandTimeout = int.MaxValue;
        }

        protected override void Seed(SPM context)
        {
            //  This method will be called after migrating to the latest version.
            context.Jurisdictions.AddOrUpdate(
                j => j.JurisdictionName,
                new Models.Jurisdiction
                {
                    JurisdictionName = "Default Name",
                    MPO = "",
                    OtherPartners = "",
                    CountyParish = ""
                }
                );
            context.FAQs.AddOrUpdate(
                f => f.Header,
                new FAQ
                {
                    OrderNumber = 1,
                    Header = @"<b>How do I navigate the UDOT Automated Traffic Signal Performance Measures website?</b>",
                    Body = @"<b>There are two ways to navigate the UDOT Automated Traffic Signal Performance Measures website</b><br/><br/><u>MAP</u><ol><li>Zoom in on the map and click on the desired intersection (note: the map can be filtered by selecting “metric type” ).</li><li>Select the available chart on the map from the list of available measures for the desired intersection.</li><li>Click a day and/or time range from the calendar.</li><li>Click “Create Chart”.  Wait, and then scroll down to see the data and charts.</li></ol><u>SIGNAL LIST</u><ol><li>Select the chart by clicking the checkbox for the desired chart.</li><li>Click the “Signal List” bar at the top of the map window.</li><li>Click “Select” next to the desired intersection.</li><li>Click a day and/or time range from the calendar.</li><li>Click “Create Chart”.  Wait, and then scroll down to see the data and charts.</li></ol>"
                },
                new FAQ
                {
                    OrderNumber = 2,
                    Header = @"<b>What are Automated Traffic Signal Performance Measures</b>",
                    Body = @"Automated Traffic Signal Performance Measures show real-time and a history of performance at signalized intersections.  The various measures will evaluate the quality of progression of traffic along the corridor, and displays any unused green time that may be available from various movements. This information informs UDOT of vehicle and pedestrian detector malfunctions, measures vehicle delay and lets us know volumes, speeds and travel time of vehicles.   The measures are used to optimize mobility and manage traffic signal timing and maintenance to reduce congestion, save fuel costs and improve safety.  There are several measures currently in use with others in development. "
                },
                new FAQ
                {
                    OrderNumber = 3,
                    Header = @"<b>How do Automated Traffic Signal Performance Measures work?</b>",
                    Body = @"The traffic signal controller manufactures (Econolite, Intelight, Siemens, McCain, TrafficWare and some others) wrote a “data-logger” program that runs in the background of the traffic signal controller firmware. The Indiana Traffic Signal Hi Resolution Data Logger Enumerations (http://docs.lib.purdue.edu/cgi/viewcontent.cgi?article=1002&context=jtrpdata) encode events to a resolution to the nearest 100 milliseconds.  The recorded enumerations will have events for “phase begin green”, “phase gap out”, “phase max out”, “phase begin yellow clearance”, “phase end yellow clearance”, “pedestrian begin walk”, “pedestrian begin clearance”, “detector off”, “detector on”, etc.  For each event, a time-stamp is given and the event is stored temporarily in the signal controller.  Over 125 various enumerations are currently in use.  Then, using an FTP connection from a remote server to the traffic signal controller, packets of the hi resolution data logger enumerations (with its 1/10th second resolution time-stamp) are retrieved and stored on a web server at the UDOT Traffic Operations Center about every 10 to 15 minutes (unless the “upload current data” checkbox is enabled, where an FTP connection will be immediately made and the data will be displayed in real-time).  Software was written in-house by UDOT that allows us to graph and display the various data-logger enumerations and to show the results on the UDOT Automated Traffic Signal Performance Measures website."
                },
                new FAQ
                {
                    OrderNumber = 4,
                    Header = @"<b>Which central traffic management system is used to get the Automated Traffic Signal Performance Measures</b>",
                    Body = @"A central traffic management system is not used or needed for the UDOT Automated Traffic Signal Performance Measures.  It is all being done through FTP connections from a web server through the network directly to the traffic signal controller which currently has the Indiana Traffic Signal Hi Resolution Data Logger Enumerations running in the background of the controller firmware.  The UDOT Automated Traffic Signal Performance Measures are independent of any central traffic management system.
"
                },
                new FAQ
                {
                    OrderNumber = 5,
                    Header = @"<b>Why does Utah need Automated Traffic Signal Performance Measures?</b>",
                    Body = @"In 2011, UDOT’s executive director assigned a Quality Improvement Team (QIT) to make recommendations that will result in UDOT providing “world-class traffic signal maintenance and operations”.  The QIT evaluated existing operations, national best practices, and NCHRP recommendations to better improve UDOT’s signal maintenance and operations practices.  One of the recommendations from the QIT was to “implement real-time monitoring of system health and quality of operations”.  The real-time Automated Signal Performance Measures allow us to greatly improve the quality of signal operations and to also know when equipment such as pedestrian detection or vehicle detection is not working properly.  We are simply able to do more with less and to manage traffic more effectively 24/7.  In addition, we are able to optimize intersections and corridors when they need to be re-optimized, instead of on a set schedule."
                },
                new FAQ
                {
                    OrderNumber = 6,
                    Header = @"<b>Where did you get the Automated Traffic Signal Performance Measure software?</b>",
                    Body = @"The UDOT Automated Traffic Signal Measures software was developed in-house at UDOT by the Department of Technology Services.  Purdue University and the Indiana Department of Transportation (INDOT) assisted in getting us started on this endeavor."
                },
                new FAQ
                {
                    OrderNumber = 7,
                    Header = @"<b>How did the Automated Traffic Signal Performance Measures Begin?</b> ",
                    Body = @"The Purdue coordination diagram concept was introduced in 2009 by Purdue University to visualize the temporal relationship between the coordinated phase indications and vehicle arrivals on a cycle-by-cycle basis.  The Indiana Traffic Signal HI Resolution Data Logger Enumerations (http://docs.lib.purdue.edu/cgi/viewcontent.cgi?article=1002&context=jtrpdata) was a joint transportation research program (updated in November 2012 but started earlier) that included people from Indiana DOT, Purdue University, Econolite, PEEK, and Siemens.  <br/><br/>After discussions with Dr. Darcy Bullock from Purdue University and INDOT’s James Sturdevant, UDOT started development of the UDOT Automated Signal Performance Measures website April of 2012."
                },
                new FAQ
                {
                    OrderNumber = 8,
                    Header = @"<b>Why are there no passwords or firewalls to access the website and see the measures?</b>",
                    Body = @"UDOT’s goal is transparency and unrestricted access to all who have a desire for traffic signal data.  Our goal in optimizing mobility, improving safety, preserving infrastructure and strengthening the economy means that all who have a desire to use the data should have access to the data without restrictions.  This includes all of UDOT (various divisions and groups), consultants, academia, MPO’s, other jurisdictions, FHWA, the public, and others.  It is also UDOT’s goal to be the most transparent Department of Transportation in the country.  Having a website where real-time Automated Signal Performance Measures can be obtained without special software, passwords or restricted firewalls will help UDOT in achieving the goal of transparency, and allows everyone access to the data without any silos."
                },
                new FAQ
                {
                    OrderNumber = 9,
                    Header = @"<b>How do you use the various Signal Performance Measures and what do they do?</b> ",
                    Body = @"There are many uses and benefits of the various measures.  Some of the key uses are:<br/><br/><u>Purdue Coordination Diagrams (PCD’s)</u> – Used to visualize the temporal relationship between the coordinated phase indications and vehicle arrivals on a cycle-by-cycle basis.  The PCD’s show the progression quality along the corridor and answer questions, such as “What percent of vehicles are arriving during the green?” or “What is the platoon ratio during various coordination patterns?” The PCD’s are instrumental in optimizing offsets, identifying oversaturated or under saturated splits for the coordinated phases, the effects of early return of green and coordinated phase actuations, impacts of queuing, adjacent signal synchronization, etc.<br/> <br/>In reading the PCD’s, between the green and yellow lines, the phase is green; between the yellow and red lines, the phase is yellow; and underneath the green line, the phase is red.  The long vertical red lines during the late night hours is showing the main street sitting in green as the side streets and left turns are being skipped.  The short vertical red lines show skipping of the side street / left turns or a late start of green for the side street or left turn.  AoG is the percent of vehicles arriving during the green phase.  GT is the percent of green split time for the phase and PR is the Platoon Ratio (Equation 15-4 from the 2000 Highway Capacity Manual).<br/><br/><u>Approach Volumes</u> – Counts the approach vehicle volumes as shown arriving upstream of the intersection about 350 ft – 400 ft.  The detection zones are in advance of the turning lanes, so the approach volumes don’t know if the vehicles are going straight through, turning left or right.  The accuracy of the approach volumes tends to undercount under heavy traffic and under multi-lane facilities.  Approach volumes are used in traffic models, as well as identifying directional splits in traffic. In addition, the measure is also used in evaluating the least disruptive time to allow lanes to be taken for maintenance and construction activities.<br/><br/><u/>Approach Speeds</u> – The speeds are obtained from the Wavetronix radar Advance Smartsensor.  As vehicles cross the 10-foot wide detector in advance of the intersection (350 ft – 400 ft upstream of the stop bar), the speed is captured, recorded, and time-stamped.  In graphing the results, a time filter is used that starts 15 seconds (user defined) after the initial green to the start of the yellow.  The time filter allows for free-flow speed conditions to be displayed that are independent of the traffic signal timings.  The approach speed measure is beneficial in knowing the approach speeds to use for modeling purposes – both for normal time-of-day coordination plans and for adverse weather or special event plans.  They are also beneficial in knowing when speed conditions degrade enough to warrant a change in time-of-day coordination plans to adverse weather or special event plans.  In addition, the speed data is used to set yellow and all-red intervals for signal timing, as well as for various speed studies.<br/><br/><u>Purdue Phase Termination Charts</u> – Shows how each phase terminates when it changes from green to red.  The measure will show if the termination occurred by a gapout, a maxout / forceoff, or skip.  A gapout means that not all of the programmed time was used.  A maxout occurs during fully actuated (free) operations, while forceoff’s occur during coordination.  Both a maxout and forceoff shows that all the programmed time was used. A skip means that the phase was not active and did not turn on.  In addition, the termination can be evaluated by consecutive occurrences in a approach.  For example, you can evaluate if three (range is between 1 and 5) gapouts or skips occurred in a approach.  This feature is helpful in areas where traffic volume fluctuations are high.  Also shown are the pedestrian activations for each phase.  What this measure does not show is the amount of gapout time remaining if a gapout occurred.  The Split Monitor measure is used to answer that question.<br/><br/>This measure is used to identify movements where split time may need to be taken from some phases and given to other phases.  Also, this measure is very useful in identifying problems with vehicle and pedestrian detection.  For example, if the phase is showing constant maxouts all night long, it is assumed that there is a detection problem.<br/><br/><u>Split Monitor</u> – This measure shows the amount of split time (green, yellow and all-red) used by the various phases at the intersection.  Greens show gapouts, reds show maxouts, blues show forceoffs and yellows show pedestrian activations.  This measure is useful to know the amount of split time each phase uses.Turning Movement Volume Counts – this measure shows the lane-by-lane vehicles per hour (vph) and total volume for each movement.  Three graphs are available for each approach (left, thru, right).  Also shown for each movement are the total volume, peak hour, peak hour factor and lane utilization factor.  The lane-by-lane volume counts are used for traffic models and traffic studies.<br/><br/><u>Approach Delay</u> – This measure shows a simplified approach delay by displaying the time between detector activations during the red phase and when the phase turns green for the coordinated movements.  This measure does not account for start-up delay, deceleration, or queue length that exceeds the detection zone.  This measure is beneficial in evaluating over time the delay per vehicle and delay per hour values for each coordinated approach.<br/><br/><u>Arrivals on Red</u> – This measure shows the percent of vehicles arriving on red (inverse of % vehicles arriving on green) and the percent red time for each coordination pattern.  The Y axis is graphing the volume (vph) and the secondary Y axis graphs the percent vehicles arriving on red.  This measure is useful in identifying areas where the progression quality is poor.<br/><br/><u>Yellow and Red Actuations</u> – This measure plots vehicle arrivals during the yellow and red portions of an intersection's movements where the speed of the vehicle is interpreted to be too fast to stop before entering the intersection. It provides users with a visual indication of occurrences, violations, and several related statistics. The purpose of this chart is to identify engineering countermeasures to deal with red light running.<br/><br/><u>Purdue Split Failure</u> – This measure calculates the percent of time that stop bar detectors are occupied during the green phase and then during the first five seconds of red. This measure is a good indication that at least one vehicle did not clear during the green."
                },
                new FAQ
                {
                    OrderNumber = 10,
                    Header = @"<b>How effective are Automated Traffic Signal Performance Measures</b>",
                    Body = @"The Automated Signal Performance Measures are an effective way to reduce congestion, save fuel costs and improve safety.  We are simply able to do more with less and are more effectively able to manage traffic every day of the week and at all times of the day, even when a traffic signal engineer is not available.  We have identified several detection problems, corrected time-of-day coordination errors in the traffic signal controller scheduler, corrected offsets, splits, among other things.  In addition, we have been able to use more accurate data in optimizing models and doing traffic studies, and have been able to more correctly set various signal timing parameters."
                },
                new FAQ
                {
                    OrderNumber = 11,
                    Header = @"<b>Does this mean I never have to stop at a red light?</b>",
                    Body = @"Although the UDOTAutomated Traffic Signal Performance Measures cannot guarantee you will only get green lights, the system does help make traveling through Utah more efficient.  UDOT Automatic Signal Performance Measures have already already helped to reduce the number of stops and delay at signalized intersections.  Continued benefits are anticipated."
                },
                new FAQ
                {
                    OrderNumber = 12,
                    Header = @"<b>Will Automated Traffic Signal Performance Measures save me money?  If so, how are cost savings measured?</b>",
                    Body = @"Yes, UDOT Automated Traffic Signal Performance Measures has already saved Utahans time and money.  By increasing corridor speeds while reducing intersection delays, traffic signal stops, and the ability to monitor operations 24/7."
                },
                new FAQ
                {
                    OrderNumber = 13,
                    Header = @"<b>How do Automated Traffic Signal Performance Measures enhance safety?</b>",
                    Body = @"By reducing congestion and reducing the percent of vehicles arriving on a red light, UDOT Automated Traffic Signal Performance Measures helps decrease the number of accidents that occur.  In addition, we are better able to manage detector failures and improve the duration of the change intervals and clearance times at intersections."
                },
                new FAQ
                {
                    OrderNumber = 14,
                    Header = @"<b>Can real-time Automated Traffic Signal Performance Measures be used as a law enforcement tool?</b>",
                    Body = @"UDOT Automated Traffic Signal Performance Measures are designed to increase the safety and efficiency at signalized intersections.  It is not intended to identify speeders or enforce traffic laws.  No personal information is recorded or used in data gathering."
                },
                new FAQ
                {
                    OrderNumber = 15,
                    Header = @"<b>Server and Data Storage Requirements</b>",
                    Body = @"We can estimate that each signal controller high resolution data requires approximately 19 MB of storage space each day.  For the UDOT system, we have approximately 2040 traffic signals bringing back about 1 TB of data per month. In addition to the high resolution data, version 4.2.0 and above also allows for the data to be rolled up into aggregated tables in 15-minute bins. UDOT averages approximately 6 GB of aggregated tables per month. UDOT uses a SAN server that holds approximately 40 TB that runs SQL 2016. Our goal is to keep between 24 months and 36 months of high resolution data files and then to cold storage the old high resolution  data files for up to five years after that. The cold storage flat files (comma deliminated file with no indexing) will require about 2 TB of storage per year. UDOT plans on keeping the aggregated tables in 15-minute bins indefinitely. "
                },
                new FAQ
                {
                    OrderNumber = 16,
                    Header = @"<b>Who uses the Automated Traffic Signal Performance Measures data?</b>",
                    Body = @"The data has been useful for some of the following users in Utah:<br/><br/><ul><li><u>Signal engineers</u> in optimize and fine-tuning signal timing.</li><li><u>Maintenance signal technicians</u> in identifying broken detector problems and investigating trouble calls.</li><li><u>Traffic engineers</u> in conducting various traffic studies, such as speed studies, turning movement studies, modeling studies, and optimizing the intersection operations.</li><li><u>Consultants</u> in improving traffic signal operations, as UDOT outsources some of the signal operations, design and planning to consultants.</li><li><u>UDOT Traffic & Safety, UDOT Traffic Engineers, UDOT Resident Construction Engineers</u> in conducting various traffic studies and/or in determining the time-of-day where construction or maintenance activities would be least disruptive to the traveling motorists.</li><li><u>Metropolitan Planning Organizations</u> (MPO’s) in calibrating the regional traffic models.</li><li><u>Academia</u> in conducting various research studies, such as evaluating the effectiveness of operations during adverse weather, evaluating the optimum signal timing for innovative intersections such as DDI’s, CFI’s and Thru-Turns, etc.</li><li><u>City and County</u> Government in using the data in similar manner to UDOT.</li></ul>"
                },
                new FAQ
                {
                    OrderNumber = 17,
                    Header = @"<b>What are the detection requirements for each metric?</b> ",
                    Body = @"<table class='table table-bordered'>
 	                            <tr>
                                    <th> MEASURE </th>
                                    <th> DETECTION NEEDED </th>
                                </tr>
                                <tr>
                                    <td> Purdue Coordination Diagram </td>
                                    <td> Setback count (350 ft – 400 ft) </td>
                                </tr>
                                <tr>
                                    <td> Approach Volume </td>
                                    <td> Setback count (350 ft – 400 ft) </td>
                                </tr>
                                <tr>
                                    <td> Approach Speed </td>
                                    <td> Setback count (350 ft – 400 ft) using radar </td>
                                </tr>
                                <tr>
                                    <td> Purdue Phase Termination </td>
                                    <td> No detection needed or used </td>
                                </tr>
                                <tr>
                                    <td> Split Monitor </td>
                                    <td> No detection needed or used </td>
                                </tr>
                                <tr>
                                    <td> Turning Movement Counts </td>
                                    <td> Stop bar (lane-by-lane) count </td>
                                </tr>
                                <tr>
                                    <td> Approach Delay </td>
                                    <td> Setback count (350 ft – 400 ft) </td>
                                </tr>
                                <tr>
                                    <td> Arrivals on Red </td>
                                    <td> Setback count (350 ft – 400 ft) </td>
                                </tr>
                                <tr>
                                    <td> Yellow and Red Actuations </td>
                                    <td> Stop bar (lane-by-lane) count that is either in front of the stop bar or has a speed filter enabled </td>
                                </tr>
                                <tr>
                                    <td> Purdue Split Failure </td>
                                    <td> Stop bar presence detection, either by lane group or individual lane </td>
                                </tr>
                        </table>
                        <b> Automated Traffic Signal Performance Measures will work with any type of detector that is capable of counting vehicles, e.g., loops, video, pucks, radar. (The only exception to this is the speed measure, where UDOT’s Automated Signal Performance Measures for speeds will only work with the Wavetronix Advance SmartSensor.) Please note that two of the measures (Purdue Phase Termination and Split Monitor) do not use detection and are extremely useful measures.</b>"
                },
                new FAQ
                {
                    OrderNumber = 18,
                    Header = @"<b>Why do some intersections only show a few metrics and others have more?</b>",
                    Body = @"Some measures have different detection requirements than other measures. For example, for approach speeds, UDOT uses the Wavetronix Advance Smartsensor radar detector and has been using this detector since 2006 for dilemma zone purposes if the design speed is 40 mph or higher.  This same detector is what we use for our setback counts 350 feet – 400 feet upstream of the intersection.  In addition, we are also able to pull the raw radar speeds from the sensor back to the TOC server for the speed data.  Not all intersections have the Wavetronix Advance Smartsensors, therefore we are not able to display speed data, as well as the PCD’s, approach volume, arrivals on red or approach delay at each intersection.<br/><br/>The turning movement count measure requires lane-by-lane detection capable of counting vehicles in each lane.  Configuring the detection for lane-by-lane counts is time consuming and takes a commitment to financial resources."
                },
                new FAQ
                {
                    OrderNumber = 19,
                    Header = @"<b>What are the System Requirements?</b>",
                    Body = @" <b> System Requirements:</b>
                        <b> Operating Systems and Software:</b>
                        The UDOT Automated Signal Performance Measures system runs on Microsoft Windows Servers.
                        The web components are hosted by Microsoft Internet Information Server(IIS).
                        The database server is a Microsoft SQL 2016 server.
                        <b> Storage and Processing:</b>
                        Detector data uses about 40 % of the storage space of the UDOT system,
                        so the number of detectors attached to a controller will have a huge impact on the amount of storage space required.Detector data is also the most important information we collect.
                        We estimate that each signal will generate 19 MB of data per day.
                        The amount of processing power required is highly dependant on how many signals are on the system,
                        how many servers will be part of the system,
                        and how many people will be using the system.  It is possible to host all of the system functions on one powerful server, or split them out into multiple, less expensive servers.  If your agency decided to make the Automated Signal Performance Measures available to the public, it might be best to have a web server separate from the database server.Much of the heavy processing for the charts is done by web services, and it is possible to host these services on a dedicated computer.
                        While each agency should consult with their IT department for specific guidelines on how to best deliver a secure, stable and responsive solution, we can estimate that most mid-range to high-end servers will be able to handle the task of hosting and creating measures for most agencies.<ul>
                        <li>Windows Server 2008 or newer installed</li>
                        <li>.NET 4.5.2 Framework installed</li>
                        <li>IIS 7 or better installed, along with ASP.NET 4.0 or later</li>
                        <li>SQL Server Express, SQL Server 2008 R2, or newer installed</li>
                        <li>Firewall exceptions for connections to the controllers</li>
                        <li>If Watchdog features are desired, installation requires access to an SMTP (email) server. It will accept email from the Automated Signal Performance Measures (ATSPM) server. The SMTP server can reside on the same machine.</li>
                        <li>Microsoft Visual Studio 2013 or later is recommended</li></ul>"
                },
                new FAQ
                {
                    OrderNumber = 20,
                    Header = @"<b>Who do I contact to find out more information about Automated Traffic Signal Performance Measures</b> ",
                    Body = @"You can contact UDOT’s Traffic Signal Operations Engineer, Mark Taylor at marktaylor@utah.gov or phone at 801-887-3714 to find out more information about Automated Signal Performance Measures."
                },
                new FAQ
                {
                    OrderNumber = 21,
                    Header = @"<b>How do I get the source code for the Automated Traffic Signal Performance Measures Website?</b> ",
                    Body = @"You can download the source code at GitHub at: https://github.com/udotdevelopment/ATSPM. GitHub is more for development and those interested in further developing and modifying the code. We encourage developers to contribute the enhancements back to GitHub so others can benefit as well.  For those interested in the executable ATSPM files, those are found on the FHWA's open source portal at: https://www.itsforge.net/index.php/community/explore-applications#/30."
                }
            );

            context.Menus.AddOrUpdate(
                m => m.MenuId,
                new Menu
                {
                    MenuId = 1,
                    MenuName = "Measures",
                    Controller = "#",
                    Action = "#",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 10
                },
                new Menu
                {
                    MenuId = 2,
                    MenuName = "Reports",
                    Controller = "#",
                    Action = "#",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 20
                },
                new Menu
                {
                    MenuId = 3,
                    MenuName = "Log Action Taken",
                    Controller = "ActionLogs",
                    Action = "Create",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 30
                },
                new Menu
                {
                    MenuId = 4,
                    MenuName = "Links",
                    Controller = "#",
                    Action = "#",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 40
                },
                new Menu
                {
                    MenuId = 5,
                    MenuName = "FAQ",
                    Controller = "FAQs",
                    Action = "Display",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 50
                },
                //new Menu
                //{
                //    MenuId = 32,
                //    MenuName = "UDOT Traffic Signal Documents",
                //    Controller = "#",
                //    Action = "#",
                //    ParentId = 0,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 60
                //},
                //new Menu
                //{
                //    MenuId = 6,
                //    MenuName = "ATSPM Manuals",
                //    Controller = "#",
                //    Action = "#",
                //    ParentId = 0,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 70
                //},
                //new Menu
                //{
                //    MenuId = 7,
                //    MenuName = "ATSPM Presentations",
                //    Controller = "#",
                //    Action = "#",
                //    ParentId = 0,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 80
                //},
                new Menu
                {
                    MenuId = 17,
                    MenuName = "Agency Configuration",
                    Controller = "Jurisdictions",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 80
                },
                new Menu
                {
                    MenuId = 27,
                    MenuName = "About",
                    Controller = "Home",
                    Action = "About",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 90
                },
                new Menu
                {
                    MenuId = 11,
                    MenuName = "Admin",
                    Controller = "#",
                    Action = "#",
                    ParentId = 0,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 100
                },
                new Menu
                {
                    MenuId = 9,
                    MenuName = "Signal",
                    Controller = "DefaultCharts",
                    Action = "Index",
                    ParentId = 1,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 10
                },
                new Menu
                {
                    MenuId = 10,
                    MenuName = "Purdue Link Pivot",
                    Controller = "LinkPivot",
                    Action = "Analysis",
                    ParentId = 1,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 20
                },
                new Menu
                {
                    MenuId = 8,
                    MenuName = "Chart Usage",
                    Controller = "ActionLogs",
                    Action = "Usage",
                    ParentId = 2,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 10
                },
                new Menu
                {
                    MenuId = 71,
                    MenuName = "Configuration",
                    Controller = "Signals",
                    Action = "SignalDetail",
                    ParentId = 2,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 15
                },
                new Menu
                {
                    MenuId = 48,
                    MenuName = "Aggregate Data",
                    Controller = "AggregateDataExport",
                    Action = "Index",
                    ParentId = 2,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 20
                },
                new Menu
                {
                    MenuId = 58,
                    MenuName = "Left Turn Gap Analysis",
                    Controller = "LeftTurnGapReport",
                    Action = "Index",
                    ParentId = 2,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 25
                },
                //new Menu
                //{
                //    MenuId =42,
                //    MenuName = "GDOT ATSPM Installation Manual",
                //    Controller = "Images",
                //    Action = "ATSPM_Installation_Manual_2020-01-28.pdf",
                //    ParentId = 6,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 10
                //},
                //new Menu
                //{
                //    MenuId = 34,
                //    MenuName = "GDOT ATSPM Component Details",
                //    Controller = "Images",
                //    Action = "ATSPM_Component_Details_20200120.pdf",
                //    ParentId = 6,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 20
                //},
                //new Menu
                //{
                //    MenuId = 43,
                //    MenuName = "GDOT ATSPM Reporting Details",
                //    Controller = "Images",
                //    Action = "ATSPM_Reporting_Details_20200121.pdf",
                //    ParentId = 6,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 30
                //},
                //new Menu
                //{
                //    MenuId = 70,
                //    MenuName = "ATSPM_User Case Examples_Manual",
                //    Controller = "Images",
                //    Action = "ATSPM_User Case Examples_Manual_20200128.pdf",
                //    ParentId = 6,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 40
                //},
                //new Menu
                //{
                //    MenuId = 38,
                //    MenuName = "ATSPM ITS California 9-21-16",
                //    Controller = "Images",
                //    Action = "ATSPM_ITS_CA_9-21-16.pdf",
                //    ParentId = 7,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 10
                //},
                //new Menu
                //{
                //    MenuId = 37,
                //    MenuName = "ATSPM CO WY ITE & Rocky Mtn 10-20-16",
                //    Controller = "Images",
                //    Action = "ATSPM_CO-WY_ITE___ITS_Rocky_Mtn_10-20-16.pdf",
                //    ParentId = 7,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 20
                //},
                //new Menu
                //{
                //    MenuId = 36,
                //    MenuName = "ATSPM EDC4 Minnesota 10-25-16",
                //    Controller = "Images",
                //    Action = "ATSPM_EDC4_Minnesota_10-25-16.pdf",
                //    ParentId = 7,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 30
                //},
                //new Menu
                //{
                //    MenuId = 35,
                //    MenuName = "ATSPM UDOT Conference 11-2-16",
                //    Controller = "Images",
                //    Action = "ATSPM_UDOT_Conference_11-2-16.pdf",
                //    ParentId = 7,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 40
                //},
                //new Menu
                //{
                //    MenuId = 45,
                //    MenuName = "Mark Taylor",
                //    Controller = "Images",
                //    Action = "TTTMarkTaylor.pdf",
                //    ParentId = 7,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 50
                //},
                //new Menu
                //{
                //    MenuId = 62,
                //    MenuName = "ATSPM UDOT Conference 11-6-18",
                //    Controller = "Images",
                //    Action = "Session 27_ATSPMs_UDOT Conference_20181106.pdf",
                //    ParentId = 7,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 60
                //},
                //new Menu
                //{
                //    MenuId = 46,
                //    MenuName = "Jamie Mackey",
                //    Controller = "Images",
                //    Action = "TTTJamieMackey.pdf",
                //    ParentId = 7,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 70
                //},
                //new Menu
                //{
                //    MenuId = 47,
                //    MenuName = "Derek Lowe & Shane Johnson",
                //    Controller = "Images",
                //    Action = "TTTDerekLoweShaneJohnson.pdf",
                //    ParentId = 7,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 80
                //},
                new Menu
                {
                    MenuId = 12,
                    MenuName = "Signal Configuration",
                    Controller = "Signals",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 10
                },
                new Menu
                {
                    MenuId = 16,
                    MenuName = "Menu Configuration",
                    Controller = "Menus",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 20
                },
                new Menu
                {
                    MenuId = 13,
                    MenuName = "Route Configuration",
                    Controller = "Routes",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 30
                },
                new Menu
                {
                    MenuId = 57,
                    MenuName = "General Settings",
                    Controller = "GeneralSettings",
                    Action = "Edit",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 40
                },
                new Menu
                {
                    MenuId = 49,
                    MenuName = "Raw Data Export",
                    Controller = "DataExport",
                    Action = "RawDataExport",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 50
                },
                new Menu
                {
                    MenuId = 54,
                    MenuName = "Watch Dog",
                    Controller = "WatchDogApplicationSettings",
                    Action = "Edit",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 60
                },
                new Menu
                {
                    MenuId = 56,
                    MenuName = "Database Archive Settings",
                    Controller = "DatabaseArchiveSettings",
                    Action = "edit",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 70
                },
                new Menu
                {
                    MenuId = 52,
                    MenuName = "FAQs",
                    Controller = "FAQs",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 70
                },
                new Menu
                {
                    MenuId = 51,
                    MenuName = "Users",
                    Controller = "SPMUsers",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 90
                },
                new Menu
                {
                    MenuId = 15,
                    MenuName = "Roles",
                    Controller = "Account",
                    Action = "RoleAddToUser",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 100
                },
                new Menu
                {
                    MenuId = 100,
                    MenuName = "Measure Defaults Settings",
                    Controller = "MeasuresDefaults",
                    Action = "Index",
                    ParentId = 11,
                    Application = "SignalPerformanceMetrics",
                    DisplayOrder = 45
                }
                //new Menu
                //{
                //    MenuId = 61,
                //    MenuName = "NEMA Phase # Convention at UDOT",
                //    Controller = "Images",
                //    Action = "NEMA Phase # Convention UDOT.pdf",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 10
                //},
                //new Menu
                //{
                //    MenuId = 39,
                //    MenuName = "TSMP UDOT V1-2 2-5-16",
                //    Controller = "Images",
                //    Action = "TSMP_UDOT_v1-2_2-5-16.pdf",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 20
                //},
                //new Menu
                //{
                //    MenuId = 40,
                //    MenuName = "Emergency Traffic Signal Response Plan UDOT 5-6-16",
                //    Controller = "Images",
                //    Action = "EmergencyTrafficSignalResponsePlanUDOT5-6-16.pdf",
                //    ParentId = 4,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 1
                //},
                //new Menu
                //{
                //    MenuId = 41,
                //    MenuName = "Signal Ops QIT Final Report",
                //    Controller = "Images",
                //    Action = "Signal Ops QIT Final Report Released.pdf",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 40
                //},
                //new Menu
                //{
                //    MenuId = 55,
                //    MenuName = "Detector Accuracy Information",
                //    Controller = "Images",
                //    Action = "DetectorAccuracyInformation.pdf",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 50
                //},
                //new Menu
                //{
                //    MenuId = 60,
                //    MenuName = "Wavetronix Matrix Latency Information",
                //    Controller = "Images",
                //    Action = "WavetronixMatrixLatencyInformation.pdf",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 60
                //},
                //new Menu
                //{
                //    MenuId = 64,
                //    MenuName = "UDOT Detection Form 2019-04-09",
                //    Controller = "Images",
                //    Action = "UDOT Detection Form 2019-04-09.xlsm",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 70 
                //},
                //new Menu
                //{
                //    MenuId = 65,
                //    MenuName = "UDOT Detection Form Printable Tables 2019-04-09",
                //    Controller = "Images",
                //    Action = "UDOT Detection Form Printable Tables 20190409.pdf",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 80 
                //},
                //new Menu
                //{
                //    MenuId = 68,
                //    MenuName = "Examples of Detector Setup DZ",
                //    Controller = "Images",
                //    Action = "Examples of Detector Setup 2017-05-02.pdf",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 90
                //},
                //new Menu
                //{
                //    MenuId = 69,
                //    MenuName = "Configuration - Detection Type - Log Action Taken",
                //    Controller = "Images",
                //    Action = "Configuration-DetectionType-LogActionTaken.pdf",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 100
                //},
                //new Menu
                //{
                //    MenuId = 66,
                //    MenuName = "AWS LFT and Detection Worksheets 2019-04-10",
                //    Controller = "Images",
                //    Action = "AWS LFT and Detection Worksheets 2019-04-10.xlsm",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder =110
                //},
                //new Menu
                //{
                //    MenuId = 67,
                //    MenuName = "AWS LFT and Detection Worksheets Printable ",
                //    Controller = "Images",
                //    Action = "AWS LFT and Detection Worksheets Printable.pdf",
                //    ParentId = 32,
                //    Application = "SignalPerformanceMetrics",
                //    DisplayOrder = 120
                //}
            );

            context.ExternalLinks.AddOrUpdate(
                c => c.DisplayOrder,
                new ExternalLink
                {
                    Name = "Indiana Hi Resolution Data Logger Enumerations",
                    DisplayOrder = 1,
                    Url = " https://docs.lib.purdue.edu/jtrpdata/3/"
                },
                new ExternalLink
                {
                    Name = "Florida ATSPM",
                    DisplayOrder = 2,
                    Url = "https://atspm.cflsmartroads.com/ATSPM"
                },
                new ExternalLink
                {
                    Name = "FAST (Southern Nevada)",
                    DisplayOrder = 3,
                    Url = "http://challenger.nvfast.org/spm"
                },
                new ExternalLink
                {
                    Name = "Georgia ATSPM",
                    DisplayOrder = 4,
                    Url = "https://traffic.dot.ga.gov/atspm"
                },
                new ExternalLink
                {
                    Name = "Arizona ATSPM",
                    DisplayOrder = 5,
                    Url = "http://spmapp01.mcdot-its.com/ATSPM"
                },
                new ExternalLink
                {
                    Name = "Alabama ATSPM",
                    DisplayOrder = 6,
                    Url = "http://signalmetrics.ua.edu"
                },
                new ExternalLink
                {
                    Name = "ATSPM Workshop 2016 SLC",
                    DisplayOrder = 7,
                    Url = "http://docs.lib.purdue.edu/atspmw/2016"
                },
                new ExternalLink
                {
                    Name = "Train The Trainer Webinar Day 1 - Morning",
                    DisplayOrder = 8,
                    Url = "https://connectdot.connectsolutions.com/p75dwqefphk   "
                },
                new ExternalLink
                {
                    Name = "Train The Trainer Webinar Day 1 - Afternoon",
                    DisplayOrder = 9,
                    Url = "https://connectdot.connectsolutions.com/p6l6jaoy3gj"
                },
                new ExternalLink
                {
                    Name = "Train The Trainer Webinar Day 2 - Morning",
                    DisplayOrder = 10,
                    Url = "https://connectdot.connectsolutions.com/p6mlkvekogo/"
                },
                new ExternalLink
                {
                    Name = "Train The Trainer Webinar Day 2 - Mid Morning",
                    DisplayOrder = 11,
                    Url = "https://connectdot.connectsolutions.com/p3ua8gtj09r/"
                }
            );
            context.ControllerType.AddOrUpdate(
                c => c.ControllerTypeID,
                new ControllerType
                {
                    ControllerTypeID = 1,
                    Description = "ASC3",
                    SNMPPort = 161,
                    FTPDirectory = "//Set1",
                    ActiveFTP = true,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                },
                new ControllerType
                {
                    ControllerTypeID = 2,
                    Description = "Cobalt",
                    SNMPPort = 161,
                    FTPDirectory = "/set1",
                    ActiveFTP = true,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                },
                new ControllerType
                {
                    ControllerTypeID = 3,
                    Description = "ASC3 - 2070",
                    SNMPPort = 161,
                    FTPDirectory = "/set1",
                    ActiveFTP = true,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                },
                new ControllerType
                {
                    ControllerTypeID = 4,
                    Description = "MaxTime",
                    SNMPPort = 161,
                    FTPDirectory = "none",
                    ActiveFTP = false,
                    UserName = "none",
                    Password = "none"
                },
                new ControllerType
                {
                    ControllerTypeID = 5,
                    Description = "Trafficware",
                    SNMPPort = 161,
                    FTPDirectory = "none",
                    ActiveFTP = true,
                    UserName = "none",
                    Password = "none"
                },
                new ControllerType
                {
                    ControllerTypeID = 6,
                    Description = "Siemens SEPAC",
                    SNMPPort = 161,
                    FTPDirectory = "/mnt/sd",
                    ActiveFTP = false,
                    UserName = "admin",
                    Password = "$adm*kon2"
                },
                new ControllerType
                {
                    ControllerTypeID = 7,
                    Description = "McCain ATC EX",
                    SNMPPort = 161,
                    FTPDirectory = " /mnt/rd/hiResData",
                    ActiveFTP = false,
                    UserName = "root",
                    Password = "root"
                },
                new ControllerType
                {
                    ControllerTypeID = 8,
                    Description = "Peek",
                    SNMPPort = 161,
                    FTPDirectory = "mnt/sram/cuLogging",
                    ActiveFTP = false,
                    UserName = "atc",
                    Password = "PeekAtc"
                },
                new ControllerType
                {
                    ControllerTypeID = 9,
                    Description = "EOS",
                    SNMPPort = 161,
                    FTPDirectory = "/set1",
                    ActiveFTP = true,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                }
            );

            context.DetectionTypes.AddOrUpdate(
                c => c.DetectionTypeID,
                new DetectionType { DetectionTypeID = 1, Description = "Basic" },
                new DetectionType { DetectionTypeID = 2, Description = "Advanced Count" },
                new DetectionType { DetectionTypeID = 3, Description = "Advanced Speed" },
                new DetectionType { DetectionTypeID = 4, Description = "Lane-by-lane Count" },
                new DetectionType { DetectionTypeID = 5, Description = "Lane-by-lane with Speed Restriction" },
                new DetectionType { DetectionTypeID = 6, Description = "Stop Bar Presence" },
                new DetectionType { DetectionTypeID = 7, Description = "Advanced Presence" }
            );


            context.MetricTypes.AddOrUpdate(
                c => c.MetricID,
                new MetricType
                {
                    MetricID = 1,
                    ChartName = "Purdue Phase Termination",
                    Abbreviation = "PPT",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 1
                },
                new MetricType
                {
                    MetricID = 2,
                    ChartName = "Split Monitor",
                    Abbreviation = "SM",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 5
                },
                new MetricType
                {
                    MetricID = 3,
                    ChartName = "Pedestrian Delay",
                    Abbreviation = "PedD",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 10
                },
                new MetricType
                {
                    MetricID = 4,
                    ChartName = "Preemption Details",
                    Abbreviation = "PD",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 15
                },
                new MetricType
                {
                    MetricID = 17,
                    ChartName = "Timing And Actuation",
                    Abbreviation = "TAA",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 20
                },
                new MetricType
                {
                    MetricID = 12,
                    ChartName = "Purdue Split Failure",
                    Abbreviation = "SF",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 30
                },
                new MetricType
                {
                    MetricID = 11,
                    ChartName = "Yellow and Red Actuations",
                    Abbreviation = "YRA",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 35
                },
                new MetricType
                {
                    MetricID = 5,
                    ChartName = "Turning Movement Counts",
                    Abbreviation = "TMC",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 40
                },
                new MetricType
                {
                    MetricID = 7,
                    ChartName = "Approach Volume",
                    Abbreviation = "AV",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 45
                },
                new MetricType
                {
                    MetricID = 8,
                    ChartName = "Approach Delay",
                    Abbreviation = "AD",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 50
                },
                new MetricType
                {
                    MetricID = 9,
                    ChartName = "Arrivals On Red",
                    Abbreviation = "AoR",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 55
                },
                new MetricType
                {
                    MetricID = 6,
                    ChartName = "Purdue Coordination Diagram",
                    Abbreviation = "PCD",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 60
                },
                new MetricType
                {
                    MetricID = 10,
                    ChartName = "Approach Speed",
                    Abbreviation = "Speed",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 65
                },
                new MetricType
                {
                    MetricID = 13,
                    ChartName = "Purdue Link Pivot",
                    Abbreviation = "LP",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 70
                },
                new MetricType
                {
                    MetricID = 15,
                    ChartName = "Preempt Service",
                    Abbreviation = "PS",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 75
                },
                new MetricType
                {
                    MetricID = 14,
                    ChartName = "Preempt Service Request",
                    Abbreviation = "PSR",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 80
                },
                new MetricType
                {
                    MetricID = 16,
                    ChartName = "Detector Activation Count",
                    Abbreviation = "DVA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 85
                },

                new MetricType
                {
                    MetricID = 18,
                    ChartName = "Approach Pcd", //"Purdue Coodination",
                    Abbreviation = "APCD", // "PCDA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 102
                },
                new MetricType
                {
                    MetricID = 19,
                    ChartName = "Approach Cycle", // "Cycle"
                    Abbreviation = "CA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 103
                },
                new MetricType
                {
                    MetricID = 20,
                    ChartName = "Approach Split Fail", //"Purdue Split Failure",
                    Abbreviation = "SFA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 104
                },
                new MetricType
                {
                    MetricID = 22,
                    ChartName = "Signal Preemption", //"Preemption",
                    Abbreviation = "PreemptA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 105
                },
                new MetricType
                {
                    MetricID = 24,
                    ChartName = "Signal Priority", // "Transit Signal Priority",
                    Abbreviation = "TSPA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 106
                },
                new MetricType
                {
                    MetricID = 25,
                    ChartName = "Approach Speed",
                    Abbreviation = "ASA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 107
                },
                new MetricType
                {
                    MetricID = 26,
                    ChartName = "Approach Yellow Red Activations", //"Yellow Red Activations",
                    Abbreviation = "YRAA",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 108
                },
                new MetricType
                {
                    MetricID = 27,
                    ChartName = "Signal Event Count",
                    Abbreviation = "SEC",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 109
                },
                new MetricType
                {
                    MetricID = 28,
                    ChartName = "Approach Event Count",
                    Abbreviation = "AEC",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 110
                },
                new MetricType
                {
                    MetricID = 29,
                    ChartName = "Phase Termination",
                    Abbreviation = "AEC",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 111
                },
                new MetricType
                {
                    MetricID = 30,
                    ChartName = "Phase Pedestrian Delay",
                    Abbreviation = "APD",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 112
                },
                new MetricType
                {
                    MetricID = 31,
                    ChartName = "Left Turn Gap Analysis",
                    Abbreviation = "LTGA",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 112
                },
                new MetricType
                {
                    MetricID = 32,
                    ChartName = "Wait Time",
                    Abbreviation = "WT",
                    ShowOnWebsite = true,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 113
                },
                new MetricType
                {
                    MetricID = 33,
                    ChartName = "Gap Vs Demand",
                    Abbreviation = "GVD",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = false,
                    DisplayOrder = 115
                },
                new MetricType
                {
                    MetricID = 34,
                    ChartName = "Left Turn Gap",
                    Abbreviation = "LTG",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 114
                },
                new MetricType
                {
                    MetricID = 35,
                    ChartName = "Split Monitor",
                    Abbreviation = "SM",
                    ShowOnWebsite = false,
                    ShowOnAggregationSite = true,
                    DisplayOrder = 120
                }

            );
            context.SaveChanges();

            foreach (var detectionType in context.DetectionTypes)
                switch (detectionType.DetectionTypeID)
                {
                    case 1:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(1));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(2));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(3));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(4));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(14));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(15));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(17));
                        break;
                    case 2:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(6));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(7));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(8));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(9));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(13));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(32));
                        break;
                    case 3:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(10));
                        break;
                    case 4:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(5));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(7));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(31));
                        break;
                    case 5:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(11));
                        break;
                    case 6:
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(12));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(31));
                        detectionType.MetricTypes.Add(context.MetricTypes.Find(32));
                        break;
                }
            context.SaveChanges();

            context.VersionActions.AddOrUpdate(
                new VersionAction { ID = 1, Description = "New" },
                new VersionAction { ID = 2, Description = "Edit" },
                new VersionAction { ID = 3, Description = "Delete" },
                new VersionAction { ID = 4, Description = "New Version" },
                new VersionAction { ID = 10, Description = "Initial" }
            );

            context.MetricsFilterTypes.AddOrUpdate(
                c => c.FilterName,
                new MetricsFilterType { FilterName = "Signal ID" },
                new MetricsFilterType { FilterName = "Primary Name" },
                new MetricsFilterType { FilterName = "Secondary Name" },
                new MetricsFilterType { FilterName = "Agency" }
            );

            context.Applications.AddOrUpdate(
                c => c.ID,
                new Application { ID = 1, Name = "ATSPM" },
                new Application { ID = 2, Name = "SPMWatchDog" },
                new Application { ID = 3, Name = "DatabaseArchive" },
                new Application { ID = 4, Name = "GeneralSetting" }
            );

            context.WatchdogApplicationSettings.AddOrUpdate(
                c => c.ApplicationID,
                new WatchDogApplicationSettings
                {
                    ApplicationID = 2,
                    ConsecutiveCount = 3,
                    DefaultEmailAddress = "SomeOne@AnEmail.address",
                    EmailServer = "send.EmailServer",
                    FromEmailAddress = "SPMWatchdog@default.com",
                    LowHitThreshold = 50,
                    MaxDegreeOfParallelism = 4,
                    MinimumRecords = 500,
                    MinPhaseTerminations = 50,
                    PercentThreshold = .9,
                    PreviousDayPMPeakEnd = 18,
                    PreviousDayPMPeakStart = 17,
                    ScanDayEndHour = 5,
                    ScanDayStartHour = 1,
                    WeekdayOnly = true,
                    MaximumPedestrianEvents = 200,
                    EmailAllErrors = false
                }
            );

            context.DatabaseArchiveSettings.AddOrUpdate(m => m.ApplicationID,
                new DatabaseArchiveSettings
                {
                    ApplicationID = 3,
                    ArchivePath = @"\\ATSPM_Backup_DataTables\tcshare2\MOEFlatFiles\",
                }
            );

            //context.GeneralSettings.AddOrUpdate(
            //    c => c.ApplicationID,
            //    new Models.GeneralSettings
            //    {
            //        ApplicationID = 4,
            //        RawDataCountLimit = 1048576,
            //        ImageUrl = "http://defaultWebServer/spmimages/",
            //        ImagePath = @"\\defaultWebserver\SPMImages\"
            //    }
            //);

            context.LaneTypes.AddOrUpdate(
                new LaneType { LaneTypeID = 1, Description = "Vehicle", Abbreviation = "V" },
                new LaneType { LaneTypeID = 2, Description = "Bike", Abbreviation = "Bike" },
                new LaneType { LaneTypeID = 3, Description = "Pedestrian", Abbreviation = "Ped" },
                new LaneType { LaneTypeID = 4, Description = "Exit", Abbreviation = "E" },
                new LaneType { LaneTypeID = 5, Description = "Light Rail Transit", Abbreviation = "LRT" },
                new LaneType { LaneTypeID = 6, Description = "Bus", Abbreviation = "Bus" },
                new LaneType { LaneTypeID = 7, Description = "High Occupancy Vehicle", Abbreviation = "HOV" }
            );

            context.SaveChanges();

            context.MovementTypes.AddOrUpdate(
                new MovementType { MovementTypeID = 1, Description = "Thru", Abbreviation = "T", DisplayOrder = 3 },
                new MovementType { MovementTypeID = 2, Description = "Right", Abbreviation = "R", DisplayOrder = 5 },
                new MovementType { MovementTypeID = 3, Description = "Left", Abbreviation = "L", DisplayOrder = 1 },
                new MovementType
                {
                    MovementTypeID = 4,
                    Description = "Thru-Right",
                    Abbreviation = "TR",
                    DisplayOrder = 4
                },
                new MovementType { MovementTypeID = 5, Description = "Thru-Left", Abbreviation = "TL", DisplayOrder = 2 },

                new MovementType { MovementTypeID = 6, Description = "None", Abbreviation = "na", DisplayOrder = 6 }
            );
            context.SaveChanges();

            context.DirectionTypes.AddOrUpdate(
                new DirectionType
                {
                    DirectionTypeID = 1,
                    Description = "Northbound",
                    Abbreviation = "NB",
                    DisplayOrder = 3
                },
                new DirectionType
                {
                    DirectionTypeID = 2,
                    Description = "Southbound",
                    Abbreviation = "SB",
                    DisplayOrder = 4
                },
                new DirectionType
                {
                    DirectionTypeID = 3,
                    Description = "Eastbound",
                    Abbreviation = "EB",
                    DisplayOrder = 1
                },
                new DirectionType
                {
                    DirectionTypeID = 4,
                    Description = "Westbound",
                    Abbreviation = "WB",
                    DisplayOrder = 2
                },
                new DirectionType
                {
                    DirectionTypeID = 5,
                    Description = "Northeast",
                    Abbreviation = "NE",
                    DisplayOrder = 5
                },
                new DirectionType
                {
                    DirectionTypeID = 6,
                    Description = "Northwest",
                    Abbreviation = "NW",
                    DisplayOrder = 6
                },
                new DirectionType
                {
                    DirectionTypeID = 7,
                    Description = "Southeast",
                    Abbreviation = "SE",
                    DisplayOrder = 7
                },
                new DirectionType
                {
                    DirectionTypeID = 8,
                    Description = "Southwest",
                    Abbreviation = "SW",
                    DisplayOrder = 8
                }
            //new DirectionType
            //    {
            //        DirectionTypeID = 9,
            //        Description = "None",
            //        Abbreviation = " ",
            //        DisplayOrder = 9
            //    }
            );

            //context.ToBeProcessededTables.AddOrUpdate(t => t.PartitionedTableName,
            //    new ToBeProcessededTable()
            //    {
            //        PartitionedTableName = "Controller_Event_Log",
            //        UpdatedTime = Convert.ToDateTime("2018-05-13 00:20:00.000"),
            //        PreserveDataSelect = "SELECT [SignalID], [Timestamp], [EventCode], [EventParam]",
            //        TableId = 1,
            //        PreserveDataWhere =
            //            "WHERE SignalID in (select SignalID from  [dbo].[DatabaseArchiveExcludedSignals] )",
            //        InsertValues = "INSERT INTO [SignalID], [Timestamp], [EventCode], [EventParam]",
            //        DataBaseName = "Moe",
            //        Verbose = true,

            //                    //CreateColumns4Table = @"[SignalID] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL, 
            //                    //                        [Timestamp] [datetime2](7) NOT NULL, [EventCode] [int] NOT NULL, [EventParam] [int] NOT NULL"
            //                    CreateColumns4Table = @"[SignalID] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS, 
            //                                            [Timestamp] [datetime2](7), [EventCode] [int], [EventParam] [int]"
            //    },
            //    new ToBeProcessededTable()
            //    {
            //        PartitionedTableName = "Speed_Events",
            //        UpdatedTime = Convert.ToDateTime("2018-05-13 00:20:00.000"),
            //        PreserveDataSelect = "SELECT [DetectorID], [MPH], [KPH], [Timestamp]",
            //        TableId = 2,
            //        PreserveDataWhere = @" WHERE  DetectorID  in 
            //                                            (SELECT [DetectorID]   
            //                                            FROM [dbo].[Detectors]   
            //                                             WHERE  [ApproachID]  in  
            //                                                (SELECT [ApproachID]  
            //                                                 FROM  [dbo].[Approaches]   
            //                                                 WHERE [SignalID]  in  
            //                                                    (Select [SignalId]  
            //                                                     FROM  [dbo].[DatabaseArchiveExcludedSignals] )))",
            //        InsertValues = "Insert into [DetectorID], [MPH], [KPH], [Timestamp]",
            //        DataBaseName = "MoePartition",
            //        Verbose = true,
            //        CreateColumns4Table =
            //            "[DetectorID] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL, [MPH] [int] NOT NULL, [KPH] [int] NOT NULL, [Timestamp] [datetime2](7) NOT NULL"
            //    }
            //);

            //context.ToBeProcessededIndexes.AddOrUpdate(t => t.IndexName,
            //    new ToBeProcessedTableIndex()
            //    {
            //        TableId = 1,
            //        IndexId = 1,
            //        ClusteredText = "Clustered",
            //        TextForIndex =
            //            "([Timestamp] ASC) WITH (PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)",
            //        IndexName = "IX_Clustered_Controller_Event_Log_Temp"
            //    },
            //    new ToBeProcessedTableIndex()
            //    {
            //        TableId = 1,
            //        IndexId = 2,
            //        ClusteredText = "NonClustered",
            //        TextForIndex =
            //            "([SignalID] ASC, [Timestamp] ASC, [EventCode] ASC, [EventParam] ASC) WITH (PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)",
            //        IndexName = "IX_Controller_Event_Log"
            //    },
            //    new ToBeProcessedTableIndex()
            //    {
            //        TableId = 2,
            //        IndexId = 1,
            //        ClusteredText = "Clustered",
            //        TextForIndex =
            //            "([Timestamp] ASC) WITH (PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)",
            //        IndexName = "IX_Clustered_Speed_Events"
            //    },
            //    new ToBeProcessedTableIndex()
            //    {
            //        TableId = 2,
            //        IndexId = 2,
            //        ClusteredText = "NonClustered",
            //        TextForIndex =
            //            "([DetectorID] ASC, [Timestamp] ASC) WITH (PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)",
            //        IndexName = "IX_ByDetID"
            //    },
            //    new ToBeProcessedTableIndex()
            //    {
            //        TableId = 2,
            //        IndexId = 3,
            //        ClusteredText = "NonClustered",
            //        TextForIndex =
            //            "([Timestamp] ASC, [DetectorID] ASC) WITH (PAD_INDEX = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)",
            //        IndexName = "ByTimestampByDetID"
            //    }
            //);

            context.Regions.AddOrUpdate(
                new Region { ID = 1, Description = "Region 1" },
                new Region { ID = 2, Description = "Region 2" },
                new Region { ID = 3, Description = "Region 3" },
                new Region { ID = 4, Description = "Region 4" },
                new Region { ID = 10, Description = "Other" }
            );

            context.Agencies.AddOrUpdate(
                new Agency { AgencyID = 1, Description = "Academics" },
                new Agency { AgencyID = 2, Description = "City Government" },
                new Agency { AgencyID = 3, Description = "Consultant" },
                new Agency { AgencyID = 4, Description = "County Government" },
                new Agency { AgencyID = 5, Description = "Federal Government" },
                new Agency { AgencyID = 6, Description = "MPO" },
                new Agency { AgencyID = 7, Description = "State Government" },
                new Agency { AgencyID = 8, Description = "Other" }
            );
            context.Actions.AddOrUpdate(
                new Action { ActionID = 1, Description = "Actuated Coord." },
                new Action { ActionID = 2, Description = "Coord On/Off" },
                new Action { ActionID = 3, Description = "Cycle Length" },
                new Action { ActionID = 4, Description = "Detector Issue" },
                new Action { ActionID = 5, Description = "Offset" },
                new Action { ActionID = 6, Description = "Sequence" },
                new Action { ActionID = 7, Description = "Time Of Day" },
                new Action { ActionID = 8, Description = "Other" },
                new Action { ActionID = 9, Description = "All-Red Interval" },
                new Action { ActionID = 10, Description = "Modeling" },
                new Action { ActionID = 11, Description = "Traffic Study" },
                new Action { ActionID = 12, Description = "Yellow Interval" },
                new Action { ActionID = 13, Description = "Force Off Type" },
                new Action { ActionID = 14, Description = "Split Adjustment" },
                new Action { ActionID = 15, Description = "Manual Command" }
            );

            context.DetectionHardwares.AddOrUpdate(
                new DetectionHardware { ID = 0, Name = "Unknown" },
                new DetectionHardware { ID = 1, Name = "Wavetronix Matrix" },
                new DetectionHardware { ID = 2, Name = "Wavetronix Advance" },
                new DetectionHardware { ID = 3, Name = "Inductive Loops" },
                new DetectionHardware { ID = 4, Name = "Sensys" },
                new DetectionHardware { ID = 5, Name = "Video" },
                new DetectionHardware { ID = 6, Name = "FLIR: Thermal Camera" }
            );

            //These are default values.  They need to be changed before the system goes into production.

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<SPMUser>(context);
            var userManager = new UserManager<SPMUser>(userStore);
            if (userManager.FindByName("DefaultAdmin@SPM.Gov") == null)
            {
                var user = new SPMUser
                {
                    UserName = "DefaultAdmin@SPM.Gov".ToLower(),
                    Email = "DefaultAdmin@SPM.Gov".ToLower()
                };
                userManager.Create(user, "L3tM3in!");
                roleManager.Create(new IdentityRole("Admin"));
                roleManager.Create(new IdentityRole("User"));
                userManager.AddToRole(user.Id, "Admin");
                userManager.AddToRole(user.Id, "User");
                roleManager.Create(new IdentityRole("Technician"));
                userManager.AddToRole(user.Id, "Technician");
                roleManager.Create(new IdentityRole("Data"));
                userManager.AddToRole(user.Id, "Data");
                roleManager.Create(new IdentityRole("Configuration"));
                userManager.AddToRole(user.Id, "Configuration");
                roleManager.Create(new IdentityRole("Restricted Configuration"));
                userManager.AddToRole(user.Id, "Restricted Configuration");
            }
            else
            {
                var user = userManager.FindByName("DefaultAdmin@SPM.Gov");
                roleManager.Create(new IdentityRole("Technician"));
                userManager.AddToRole(user.Id, "Technician");
                roleManager.Create(new IdentityRole("Data"));
                userManager.AddToRole(user.Id, "Data");
                roleManager.Create(new IdentityRole("Configuration"));
                userManager.AddToRole(user.Id, "Configuration");
                roleManager.Create(new IdentityRole("Restricted Configuration"));
                userManager.AddToRole(user.Id, "Restricted Configuration");
            }

            context.SaveChanges();
        }
    }
}
