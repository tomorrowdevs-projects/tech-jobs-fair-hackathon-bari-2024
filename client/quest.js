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

  progressBarDiv.style.width = "%";
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

    h3.innerHTML = localStorage.getItem("user") +" " + "0/5";
    h1.innerHTML = requestData.category;
    h2.innerHTML = requestData.questionText;
 
 /*  fetch('/web/ws/questions', {
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

    h3.innerHTML = localStorage.getItem("user") + "0/5";
    h1.innerHTML = data.category;
    h2.innerHTML = data.questionText;

  })
  .catch(error => {
    console.error('Si è verificato un errore durante la richiesta:', error);
  }); */

  for (let i = 0; i < 4; i++) {
    const contResp = document.createElement("div");
    const inputResponse = document.createElement("button");

    contResp.classList.add("form__filed");
    inputResponse.classList.add("box");

    /* inputResponse.setAttribute("type", "submit"); */

    inputResponse.innerText = requestData.incorrectAnswers[i];

    contResp.append(inputResponse);
    contQuestResponse.append(contResp);
}
}
qst()



