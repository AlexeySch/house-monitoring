$(function () {
    var sensor = $("#placeholder").attr("href");

    function onDataReceived(series) {
        console.log(series);

        Highcharts.stockChart('placeholder',
        {
            title: {
                text: sensor
            },
            rangeSelector: {
                buttons: [{
                    type: 'hour',
                    count: 1,
                    text: '1h'
                }, {
                    type: 'hour',
                    count: 4,
                    text: '4h'
                }, {
                    type: 'hour',
                    count: 12,
                    text: '12h'
                }, {
                    type: 'day',
                    count: 1,
                    text: '1d'
                }, {
                    type: 'month',
                    count: 1,
                    text: '1m'
                },  {
                    type: 'all',
                    text: 'All'
                }],
                inputEnabled: false, // it supports only days
                selected: 1 // all
            },
            series: [
                {
                    name: sensor,
                    data: series
                }
            ]

        });
    };

    $.ajax({
        url: "/api/Data/sensor/" + sensor,
        type: "GET",
        dataType: "json",
        success: onDataReceived
    });


});
