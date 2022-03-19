function drawBranchCharts(canvasID, givenData) {
    if (givenData != "") {
        var ctx = document.getElementById(canvasID).getContext('2d');
        var chart = new Chart(ctx, {
            // The type of chart we want to create
            type: 'bar',

            // The data for our dataset
            data: JSON.parse(givenData),

            // Configuration options go here
            options: {}
        });
    }
}