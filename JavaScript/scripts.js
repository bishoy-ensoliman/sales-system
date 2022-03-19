// $(document).ready(function () {
function openNav() {
  document.getElementById("mySidenav").style.width = "250px";
  document.getElementById("main").style.marginLeft = "220px";
}

function closeNav() {
  document.getElementById("mySidenav").style.width = "0";
  document.getElementById("main").style.marginLeft = "0";
}


$('.tabs .tab-links a').on('click', function (e) {
  var currentAttrValue = $(this).attr('id');

  // Show/Hide Tabs
  $('.tabs ' + currentAttrValue).fadeIn(400).siblings().hide();

  // Change/remove current tab to active
  $(this).parent('li').addClass('active').siblings().removeClass('active');

  e.preventDefault();
});

(function () {
  $('.popOver').hide();
})();


$('.close').on('click', function () {
  $(".popOver").hide();
})

$('.monitorBtn').on('click', function () {


  if ($(this).hasClass('downpopOver')) {
    $('.popOver').css('top', '82%');
    if ($(this).hasClass('downleft')) {
      $('.arrow-up').css('left', '32%');
      $('.popOver').toggle();
    }
    else if ($(this).hasClass('downCenter')) {
      $('.arrow-up').css('left', '56%');
      $('.popOver').toggle();
    }
    else if ($(this).hasClass('downRight')) {
      $('.arrow-up').css('left', '81%');
      $('.popOver').toggle();
    }
  }

  else {
    $('.popOver').css('top', '47%');
    if ($(this).hasClass('topCenter')) {
      $('.arrow-up').css('left', '56%');
      $('.popOver').toggle();
    }
    else if ($(this).hasClass('topRight')) {
      $('.arrow-up').css('left', '81%');
      $('.popOver').toggle();
    }
    else if ($(this).hasClass('topLeft')) {
      $('.arrow-up').css('left', '32%');
      $('.popOver').toggle();
    }
  }
})


$("[data-toggle=popover]").popover({
  html: true,
  content: function () {
    return $('#popover-content').html();
  }
});

function initialize() {
    $('.tabs .tab-links a').on('click', function (e) {
        var currentAttrValue = $(this).attr('id');

        // Show/Hide Tabs
        $('.tabs ' + currentAttrValue).fadeIn(400).siblings().hide();

        // Change/remove current tab to active
        $(this).parent('li').addClass('active').siblings().removeClass('active');

        e.preventDefault();
    });

    (function () {
        $('.popOver').hide();
    })();


    $('.close').on('click', function () {
        $(".popOver").hide();
    })

    $('.monitorBtn').on('click', function () {


        if ($(this).hasClass('downpopOver')) {
            $('.popOver').css('top', '82%');
            if ($(this).hasClass('downleft')) {
                $('.arrow-up').css('left', '32%');
                $('.popOver').toggle();
            }
            else if ($(this).hasClass('downCenter')) {
                $('.arrow-up').css('left', '56%');
                $('.popOver').toggle();
            }
            else if ($(this).hasClass('downRight')) {
                $('.arrow-up').css('left', '81%');
                $('.popOver').toggle();
            }
        }

        else {
            $('.popOver').css('top', '47%');
            if ($(this).hasClass('topCenter')) {
                $('.arrow-up').css('left', '56%');
                $('.popOver').toggle();
            }
            else if ($(this).hasClass('topRight')) {
                $('.arrow-up').css('left', '81%');
                $('.popOver').toggle();
            }
            else if ($(this).hasClass('topLeft')) {
                $('.arrow-up').css('left', '32%');
                $('.popOver').toggle();
            }
        }
    })


    $("[data-toggle=popover]").popover({
        html: true,
        content: function () {
            return $('#popover-content').html();
        }
    });
}

function textCenter(val) {
  Chart.pluginService.register({
    beforeDraw: function (chart) {
      var width = chart.chart.width,
        height = chart.chart.height,
        ctx = chart.chart.ctx;

      ctx.restore();
      var fontSize = (height / 114).toFixed(2);
      ctx.font = fontSize + "em sans-serif";
      ctx.textBaseline = "middle";

      var text = val + "%",
        textX = Math.round((width - ctx.measureText(text).width) / 2),
        textY = height / 2;

      ctx.fillText(text, textX, textY);
      ctx.save();
    }
  });
}



function drawChartsHome() {
    /**********************************************************/

    google.load("visualization", "1.1", { packages: ['controls', 'corechart'] });
    google.setOnLoadCallback(drawCharts);
    


    /*********************DOUNT CHART******************** */
    var value = 75;
    var data = {
        labels: [
            "My val",
            ""
        ],
        datasets: [
            {
                data: [value, 100 - value],
                backgroundColor: [
                    "#48b2c1",
                    "#AAAAAA"
                ],
                hoverBackgroundColor: [
                    "#48b2c1",
                    "#AAAAAA"
                ],
                hoverBorderColor: [
                    "#48b2c1",
                    "#ffffff"
                ]
            }]
    };

    var myChart = new Chart(document.getElementById('myChart'), {
        type: 'doughnut',
        data: data,
        options: {
            responsive: true,
            legend: {
                display: false
            },
            cutoutPercentage: 75,
            tooltips: {
                filter: function (item, data) {
                    var label = data.labels[item.index];
                    if (label) return item;
                }
            }
        }
    });

    textCenter(value);

    /****************************************CHART 2 */
    //google.charts.load('current', {
    //    packages: ['controls', 'corechart'],
    //    callback: drawChart
    //});

    
}

function drawChart() {
    var data = google.visualization.arrayToDataTable([
        ['Pac Man', 'Percentage'],
        ['', 75],
        ['', 25]
    ]);

    var options = {
        legend: 'none',
        pieSliceText: '75',
        pieStartAngle: 90,
        tooltip: { trigger: 'none' },
        slices: {
            0: { color: '#48b2c1' },
            1: { color: 'transparent' }
        }
    };

    var chart = new google.visualization.PieChart(document.getElementById('pacman'));
    chart.draw(data, options);
}