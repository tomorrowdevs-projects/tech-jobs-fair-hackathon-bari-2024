//carichiamo sulla pagina ranking i dati relativi ai punteggi dei giocatori
//prendendoli da localStorage

//prendiamo i dati dal localStorage
var players = JSON.parse(localStorage.getItem("players"));
if (players === null) {
    players = [];
}

//creiamo la tabella
var table = document.createElement("table");
var tr = document.createElement("tr");
var th = document.createElement("th");
th.innerHTML = "Name";
tr.appendChild(th);
th = document.createElement("th");
th.innerHTML = "Score";
tr.appendChild(th);
table.appendChild(tr);
for (var i = 0; i < players.length; i++) {
    tr = document.createElement("tr");
    var td = document.createElement("td");
    td.innerHTML = players[i].name;
    tr.appendChild(td);
    td = document.createElement("td");
    td.innerHTML = players[i].score;
    tr.appendChild(td);
    table.appendChild(tr);
}
document.getElementById("ranking").appendChild(table);

//se non ci sono giocatori, mostriamo un messaggio
if (players.length === 0) {
    var p = document.createElement("p");
    p.innerHTML = "No players found";
    document.getElementById("ranking").appendChild(p);
}
