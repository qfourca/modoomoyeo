/*
"use strict";



document.getElementById("sendButton").disabled = true;



connection.start().then(function () {
    var user = "logrequest"
    var message = "ALL"
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
*/

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
const myid = document.getElementById("myid").value;
const targetid = document.getElementById("targetid").value;
console.log(myid);
connection.start().then(function () {
    var user = myid
    var target = targetid
    var message = "ALL"
    connection.invoke("SendMessage", user*1, target*1, message).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});
connection.on("ReceiveMessage", function (user, message) {
    insertChating(user, message, currentTime())
});

connection.on("logMessage", function (user, message, time) {
    insertChating(user, message, timeParser(time))
});
//----------------이윤성 코드------------------------
$(function () {
    $("input[type='text']").keypress(function (e) {
        if (e.keyCode == 13 && $(this).val().length) {
            var value = $(this).val()
            $(this).val('');
            insertChating(myid, value, currentTime())
            connection.invoke("SendMessage", myid*1, targetid*1, value).catch(function (err) {
                return console.error(err.toString());
            });
        }
    });
});
var currentTime = function () {
    var date = new Date();
    var hh = date.getHours();
    var mm = date.getMinutes();
    var apm = hh > 12 ? "오후" : "오전";
    if(hh > 12)
        hh -= 12;
    if(mm < 10)
        mm = `0${mm}`
    var ct = apm + " " + hh + ":" + mm + "";
    return ct;
}

function insertChating(user, message, time){
    var _class = (user == myid ? "mymsg" : "yourmsg");
    var _tar = $(".chat_wrap .inner").append('<div class="item ' + _class + '"><div class="box"><p class="msg">' + message + '</p><span class="time">' + time + '</span></div></div>');
    var lastItem = $(".chat_wrap .inner").find(".item:last");
    setTimeout(function () {
        lastItem.addClass("on");
    }, 10);
    var position = lastItem.position().top + $(".chat_wrap .inner").scrollTop();
    $(".chat_wrap .inner").stop().animate({ scrollTop: position }, 500);
}

var timeParser = function (inputtime) {
    return inputtime.substr(11, 8);
}