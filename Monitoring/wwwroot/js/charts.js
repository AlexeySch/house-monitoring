$(function () {
        var data = null,
            placeholder = $("#placeholder"),
            sensor = $("#placeholder").attr("href"),
            options = {
                series: {
                    lines: {
                        show: true
                    },
                    points: { show: true },
                    shadowSize: 0
                },
                xaxis: {
                    mode: "time",
                    min: (new Date()). getTime() - 24*60*60*1000
    },
            zoom: {
                interactive: true
            },
            pan: {
                interactive: true
            }
        };
    //var plot = $.plot(placeholder, [data], options);

    // show pan/zoom messages to illustrate events 
    placeholder.bind("plotpan", function (event, plot) {
        var axes = plot.getAxes();
        $(".message").html("Panning to x: " + axes.xaxis.min.toFixed(2)
        + " &ndash; " + axes.xaxis.max.toFixed(2)
        + " and y: " + axes.yaxis.min.toFixed(2)
        + " &ndash; " + axes.yaxis.max.toFixed(2));
    });
    placeholder.bind("plotzoom", function (event, plot) {
        var axes = plot.getAxes();
        $(".message").html("Zooming to x: " + axes.xaxis.min.toFixed(2)
        + " &ndash; " + axes.xaxis.max.toFixed(2)
        + " and y: " + axes.yaxis.min.toFixed(2)
        + " &ndash; " + axes.yaxis.max.toFixed(2));
    });
    // add zoom out button 
    //$("<div class='button' style='right:20px;top:20px'>zoom out</div>")
    //    .appendTo(placeholder)
    //    .click(function (event) {
    //        event.preventDefault();
    //        plot.zoomOut();
    //    });
    // and add panning buttons
    // little helper for taking the repetitive work out of placing
    // panning arrows
    function addArrow(dir, right, top, offset) {
        $("<img class='button' src='arrow-" + dir + ".gif' style='right:" + right + "px;top:" + top + "px'>")
            .appendTo(placeholder)
            .click(function (e) {
                e.preventDefault();
                plot.pan(offset);
            });
    }
    //addArrow("left", 55, 60, { left: -100 });
    //addArrow("right", 25, 60, { left: 100 });
    //addArrow("up", 40, 45, { top: -100 });
    //addArrow("down", 40, 75, { top: 100 });
     //Add the Flot version string to the footer


    //Test AJAX Web Api
    //$.plot(placeholder, d, options);

    function onDataReceived(series) {
        console.log(series);

        data = series;
        $.plot(placeholder, [series], options);
    }
    $.ajax({
        url: "/api/Data/sensor/"+sensor,
        type: "GET",
        dataType: "json",
        success: onDataReceived
    });

    console.log(sensor);


});
