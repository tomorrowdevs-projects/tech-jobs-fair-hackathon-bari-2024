const SOCKET_URL = "ws://lignux.net:1402";
const DEBUG_SOCKET = "ws://lignux.net:3000";
const LOCAL_DEBUG_SOCKET = "ws://localhost:3000";

const socket = new WebSocket(SOCKET_URL);

function connection() {
  console.log("open");
}

function connectionError() {
  console.log("connection error");
}
const credenzial = {};
function message(msg) {
  const parsed = JSON.parse(msg.data);
  switch (parsed.event) {
    case "nuova_partita":
      if (parsed.status == "success") {
        qst()
        credenzial.id = parsed.id;
        credenzial.gameId = parsed.gameId;
        socket.send(
          JSON.stringify({
            id: parsed.id,
            gameId: parsed.gameId,
            event: "prima_domanda",
          })
        );
      }
      break;
    default:
      break;
  }
}

window.addEventListener("DOMContentLoaded", () => {
  document.querySelector("#submit").addEventListener("click", (e) => {
    e.preventDefault();
    let value = document.getElementById("login__username").value;
    if (value == "") return
      let json = JSON.stringify({ event: "nuova_partita", user: value });
      console.log(json);
    socket.send(json);
  });

  socket.onopen = connection;
  socket.onerror = connectionError;

  // socket.onclose = onClose;
  socket.onmessage = message;
});



function qst() {


  const requestData = {
    type: "multiple",
    difficulty: "easy",
    category: "Entertainment: Music",
    questionText: "In 2006, which band released their debut album \"A Fever You Can't Sweat Out\"?",
    correctAnswer: "Panic! At the Disco",
    incorrectAnswers: [
      "Twenty One Pilots",
      "My Chemical Romance",
      "Fall Out Boy"
    ],
    asked: true
  };
  
    let rispostaData = {}
  
    const cont = document.createElement("div");
    const contTtlQst = document.createElement("div");
    const progressDiv = document.createElement("div");
    const progressBarDiv = document.createElement("div");
    const ttltxt = document.createElement("span");
    const h3 = document.createElement("h3");
    const h1 = document.createElement("h1");
    const contQst = document.createElement("div");
    const h2 = document.createElement("h2");
    const contQuestResponse = document.createElement("div");
  
    cont.classList.add("contPgQst");
    contTtlQst.classList.add("contTtlQst");
    progressDiv.classList.add("progress");
    progressBarDiv.classList.add("progress-bar", "bg-success");
    ttltxt.classList.add("ttltxt");
    contQuestResponse.classList.add("form", "login", "boxQstRs");
  
   
    h2.style.padding = 'var(--boxQst)';
  
    progressDiv.setAttribute("role", "progressbar");
    progressDiv.setAttribute("aria-label", "Success example");
    progressDiv.setAttribute("aria-valuenow", "25");
    progressDiv.setAttribute("aria-valuemin", "0");
    progressDiv.setAttribute("aria-valuemax", "100");
  
    document.body.append(cont);
    cont.append(contTtlQst);
    contTtlQst.append(ttltxt);
    ttltxt.append(h3);
    ttltxt.append(h1);
    contTtlQst.append(progressDiv);
    progressDiv.append(progressBarDiv);
    cont.append(contQst);
    contQst.append(h2);
    cont.append(contQuestResponse);
  
      
   
    fetch('/web/ws/questions', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(requestData)
    })
    .then(response => response.json())
    .then(data => {
      // Handle response data
      console.log('Risposta dal server:', data);
  
      /* progressBarDiv.style.width = timeout + "%"; */
      h3.innerHTML = localStorage.getItem("user") +" " + "0/5";
      h1.innerHTML = data.category;
      h2.innerHTML = data.questionText;

      for (let i = 0; i < 4; i++) {
        const contResp = document.createElement("div");
        const inputResponse = document.createElement("button");
    
        contResp.classList.add("form__filed");
        inputResponse.classList.add("box");
    
        /* inputResponse.setAttribute("type", "submit"); */
        let answers = data.incorrectAnswers[i];
        inputResponse.innerText = answers;
        
    
        inputResponse.addEventListener('click', (e) => {
          let selectedAnswer = e.target.innerText;
          rispostaData = selectedAnswer;
          socket.send(
            JSON.stringify({rispostaData}))
        });
      }
    })
    .catch(error => {
      console.error('Si è verificato un errore durante la richiesta:', error);
    });
  
    
  
      contResp.append(inputResponse);
      contQuestResponse.append(contResp);
  }
  }

  
  
  
  