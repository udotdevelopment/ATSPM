function AddData() {var pinColor =new Microsoft.Maps.Color(255, 238, 118, 35);
            var iconURL = './images/orangePin.png';
            var regionDdl = $('#Regions')[0];
            var pins = [];
            var regionFilter = 0; if (regionDdl.options[regionDdl.selectedIndex].value != '') 
                {regionFilter = regionDdl.options[regionDdl.selectedIndex].value;}
            var reportType = $('#MetricTypes')[0]; 
            var reportTypeFilter = 0; if (reportType.options[reportType.selectedIndex].value != '')
    { reportTypeFilter = ',' + reportType.options[reportType.selectedIndex].value; }
    if ((regionFilter == 0 && reportTypeFilter == 0)
        || (reportTypeFilter == 0 && regionFilter == 2)
        || (regionFilter == 0 && '1,2,3,4,14,15,6,7,8,9,13,10,12,5'.indexOf(reportTypeFilter) > -1)
        || ('1,2,3,4,14,15,6,7,8,9,13,10,12,5'.indexOf(reportTypeFilter) > -1 && regionFilter == 2)) 

return pins;}