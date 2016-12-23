$(function () {
    var sensors = null;
    function onStatusReceived(status) {
        console.log(status);
        sensors = status;
        $("#temp-value").text(sensors.temperature_Inside);
        $("#hum-value").text(sensors.humidity_Inside);
        $("#power-value").text(sensors.power_Voltage);
        $("#power-status-value").text(sensors.power_Status);
    }
    $.ajax({
        url: "/api/Data/latest",
        type: "GET",
        dataType: "json",
        success: onStatusReceived
    });

});
