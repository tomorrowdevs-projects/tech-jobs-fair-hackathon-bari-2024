


function connection() {
    console.log("open");
}


function connectionError() {
    console.log("error")
}

function message(msg){
    const parsed = JSON.parse(msg.data);
    switch(parsed.event){
        case "nuova_partita":
            if(parsed.status == "success"){
                document.getElementById("searching").style.display = "none";
                let start = document.createElement("h1");
                start.innerHTML = "comincia il quiz";
                document.body.append(start);
            }
        default:
            break
    }
}



const SOCKET_URL = "ws://lignux.net:1402";
const DEBUG_SOCKET = "ws://localhost:3000";

window.addEventListener("DOMContentLoaded", () => {

    const socket = new WebSocket(DEBUG_SOCKET);

    document.querySelector('#submit').addEventListener('click', (e) => {
        e.preventDefault()
        let value = document.getElementById('login__username').value;
        if (value == '') return
        
        socket.send(JSON.stringify({event:"nuova_partita",user:value}));

    });




    socket.onopen = connection;
    socket.onerror = connectionError;

    // socket.onclose = onClose;
    socket.onmessage = message;


});








