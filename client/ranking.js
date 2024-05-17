
let server = "";
let players = [];

//soluzione in caso di salvataggio dati su local storage
// players = JSON.parse(localStorage.getItem("players"));
// if (players === null) {
//     players = [];
// }

//prendiamo i dati dal server
if (server !== "") {
    fetch(server + "/players")
        .then(response => response.json())
        .then(data => {
            players = data;
        })
        .catch(err => {
            console.log("Error Server ", err);
        });
}
else {
    //dati di esempio in caso di variabile server vuoto"
    players = [{ "id": 0, "name": "Torepa8", "score": 300 },
    { "id": 1, "name": "Giuseppe", "score": 400 },
    { "id": 2, "name": "Guido", "score": 550 },];
}

//creiamo la tabella con i giocatori e i punteggi
var table = document.createElement("table");
var tr = document.createElement("tr");
var th = document.createElement("th");
tr.style.padding = "10px";
th.style.padding = "10px";
th.innerHTML = "Name";
tr.appendChild(th);
th = document.createElement("th");
table.appendChild(tr);
th.innerHTML = "Score";
tr.appendChild(th);

//ordiniamo la lista dei giocatori in base al punteggio
players.sort((a, b) => b.score - a.score);

for (var i = 0; i < players.length; i++) {
    tr = document.createElement("tr");
    var td = document.createElement("td");
    td.style.padding = "10px";
    td.innerHTML = players[i].name;
    tr.appendChild(td);
    td = document.createElement("td");
    td.style.padding = "10px";
    td.innerHTML = players[i].score;
    tr.appendChild(td);
    table.appendChild(tr);
    tr.style.padding = "10px";
    th.style.padding = "10px";
}
document.getElementById("ranking").appendChild(table);

//se non ci sono giocatori, mostriamo un messaggio
if (players.length === 0) {
    var p = document.createElement("p");
    p.innerHTML = "No players found";
    document.getElementById("ranking").appendChild(p);
}