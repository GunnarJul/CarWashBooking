
var socket;
var currenUserName = "";
var bookingUsers = "";
$(window).unload(function () {
    socket.close(1000, "Closing from client");
});



function connect(userName) {

    currenUserName = userName;
    var scheme = document.location.protocol === "https:" ? "wss" : "ws";
    var port = document.location.port ? ":" + document.location.port : "";
    var connectUrl = scheme + "://" + document.location.hostname + port + "/ws";  
    socket = new WebSocket(connectUrl);
    socket.open = function (event) {
        if (currenUserName)
        socket.send(currenUserName);
    };
    socket.onopen = function (event) {
        socket.send(currenUserName);
    };

    socket.onmessage = function (event) {
        bookingUsers = event.data;
    };

}
function displayCurrentUsers() {
  
    setInterval(function () {
        socket.send('');
        var d = new Date();
        var info = "Antal brugere på booking siden lige nu:" + bookingUsers + " " + d.toLocaleTimeString();
        $('#currentUser').html(info);
    }, 3000);


}


