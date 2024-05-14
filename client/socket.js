const SOCKET_URL = "ws://lignux.net:1402";
const DEBUG_SOCKET = "ws://localhost:3000";

const socket = new WebSocket(DEBUG_SOCKET);


function connection() {
    console.log("open");
}


function connectionError(){
console.log("error")
}


function message(msg){
    console.log(msg)
}




socket.onopen = connection;
socket.onerror = connectionError;

// socket.onclose = onClose;
socket.onmessage = message;