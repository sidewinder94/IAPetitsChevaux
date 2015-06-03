function connectToHosts() {
    var host0 = "ws://" + $("#player0").val();
    var host1 = "ws://" + $("#player1").val();

    $("#content").append("<li>" + host0 + "</li>");
    $("#content").append("<li>" + host1 + "</li>");

    var c0 = new WebSocket(host0);
    var c1 = new WebSocket(host1);
    var data;

    c1.onopen = function () {
        //alert("Connected");
        $("#startButton").hide();
    };

    c0.onopen = function () {
        //alert("Connected");
        $("#startButton").hide();
        c0.send("init");
    };

    c0.onmessage = function (message) {
        data = $.parseJSON(message.data);
        updateGraphics(data);
        $("<li/>").html(message.data).appendTo($("#content"));
        data.PlayerIdIs = 1;
        if (!Ended(data.Players)) {
            c1.send(JSON.stringify(data));
        }
    };

    c0.onclose = function () {
        alert("disconnected");
        $("#startButton").show();
    }

    c1.onmessage = function (message) {
        data = $.parseJSON(message.data);
        updateGraphics(data);
        $("<li/>").html(message.data).appendTo($("#content"));
        data.PlayerIdIs = 0;
        if (!Ended(data.Players)) {
            c0.send(JSON.stringify(data));
        }
    };

    c1.onclose = function () {
        alert("disconnected");
        $("#startButton").show();
    }
    
}


function Ended(players) {

    var win = false;
    var playerId = -1;

    players.forEach(function(element, index, array) {
        if (element.Won && !win) {
            win = true;
            playerId = element.Id + 1;
        }
    });

    if (win) {
        displayWin(playerId);
    }

    return win;
}

function displayWin(playerId) {
    alert("Player " + playerId + " won");
}

function resetGrid() {
    
    //For normal positions
    for (var i = 0; i < 56; i++) {
        $("#c-" + i).html("");
    }

    //For Endgame & starting positions
    for (i = 1; i < 5; i++) {
        for (var j = 1; j < 7; j++) {
            var content = '<div class="number">' + j + '</div>';
            $("#e-" + i + "-" + j).html(content);
        }
    }


}

function updateGraphics(data) {
    resetGrid();

    data.Players.forEach(function(element, index, array) {
        var id = element.Id + 1;

        var squareCount = 0;

        element.Pawns.forEach(function (pawn, i, a) {
            switch (pawn.TypeConverter) {
                case "c-{0}":
                    $("#c-" + pawn.Position).html('<div class="horse player' + id + '"></div>');
                    break;
                case "e-{0}-{1}":
                    $("#e-" + id + "-" + pawn.Position).html('<div class="number">' + pawn.Position + '</div>' + 
                    '<div class="horse player' + id + '"></div>');
                    break;
                case "sq-{0}":
                    squareCount++;
                    break;
                default:
                    console.log("Unknown case type");
            }


        });

        $("#sq-" + id).html(squareCount);

    });
}