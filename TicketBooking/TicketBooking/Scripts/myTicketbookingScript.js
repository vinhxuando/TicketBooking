function loadPage(link) {
    $("#showSelected").load(link);
}

function loadDestination() {
    var e = document.getElementById("departures");
    var v = e.options[e.selectedIndex].value;
    var url = '/Ticket/GetDestination?departure=' + encodeURI(v);
    $("#divDest").load(url);
}

function loadDateDeparture() {
    var e = document.getElementById("departures");
    var v = e.options[e.selectedIndex].value;
    var x = document.getElementById("destinations");
    var y = x.options[x.selectedIndex].value;
    var url = '/Ticket/GetDateDeparture?departure=' + encodeURI(v) + '&destination=' + encodeURI(y);
    $("#divDateDeparture").load(url);
}

function loadDateDestination() {
    var e = document.getElementById("departures");
    var v = e.options[e.selectedIndex].value;
    var x = document.getElementById("destinations");
    var y = x.options[x.selectedIndex].value;
    var a = document.getElementById("dateDeparture");
    var b = a.options[a.selectedIndex].value;
    var url = '/Ticket/GetDateDestination?departure=' + encodeURI(v) + '&destination=' + encodeURI(y) + '&dateDepart=' + encodeURI(b);
    $("#divDateDest").load(url);
}

function loadFlightID() {
    var e = document.getElementById("departures");
    var v = e.options[e.selectedIndex].value;
    var x = document.getElementById("destinations");
    var y = x.options[x.selectedIndex].value;
    var a = document.getElementById("dateDeparture");
    var b = a.options[a.selectedIndex].value;
    var c = document.getElementById("dateDestination");
    var d = c.options[c.selectedIndex].value;
    var url = '/Ticket/BookTicket?departure=' + encodeURI(v) + '&destination=' + encodeURI(y) + '&dateDepart=' + encodeURI(b) + '&dateDest=' + encodeURI(d);
    $('#divBookTicket').load(url);
}

function calTotalPrice() {
    var x = document.getElementById("numOfSeat");
    var y = document.getElementById("grades");
    var z = document.getElementById("totalPrice");
    var numOfSeat = x.value;
    var price = y.value;
    z.value = numOfSeat * price;

    var a = document.getElementById("remainingSeat");
    var b = document.getElementById("txtFlightID");

    $.get('/Ticket/GetRemainingSeat?flightID=' + b.value + '&grade=' + encodeURI(y.options[y.selectedIndex].text), function (data) {
        a.value = data;
    });
}