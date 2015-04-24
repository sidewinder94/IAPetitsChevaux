function connectToHost() {
    var host = "ws://" + $("#server").val();
    $("#content").append("<li>" + host + "</li>");
    var connection = new WebSocket(host);

    connection.onopen = function () {
        alert("Connected");
        $("#startButton").hide();
    };

    connection.onmessage = function (message) {
        var data = $.parseJSON(message.data);
        updateGraphics(data);
        $("<li/>").html(message.data).appendTo($("#content"));
    };

    connection.onclose = function () {
        alert("disconnected");
        $("#startButton").show();
    }
}

function updateGraphics(data) {
    console.log(data);
    //For normal positions
    for (var i = 0; i < 56; i++) {
        if (data["c-" + i]) {
            if (data["c-" + i] > 0 && data["c-" + i] < 5) {
                $("#c-" + i).html('<div class="horse player' + data["c-" + i] + '"></div>');
            }
            else {
                console.log("Unknown player : " + data["c-" + i]);
            }
        } else {
            $("#c-" + i).html("");
        }
    }

    for (i = 1; i < 5; i++) {
        for (var j = 1; j < 7; j++) {
            var content = '<div class="number">' + j + '</div>';
            if (data["e-" + i + "-" + j] != undefined) {
                content += '<div class="horse player' + i + '"></div>';
            }
            $("#e-" + i + "-" + j).html(content);
        }
    }

}