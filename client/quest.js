export function qst() {
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
  contQuestResponse.classList.add("form", "login");

  progressBarDiv.style.width = "25%";

  progressDiv.setAttribute("role", "progressbar");
  progressDiv.setAttribute("aria-label", "Success example");
  progressDiv.setAttribute("aria-valuenow", "25");
  progressDiv.setAttribute("aria-valuemin", "0");
  progressDiv.setAttribute("aria-valuemax", "100");

  h3.innerHTML = "Turno 1";
  h1.innerHTML = "Domanda";
  h2.innerHTML = "Per cosa si utilizza Git?";

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

  for (let i = 0; i < 4; i++) {
    const contResp = document.createElement("div");
    const inputResponse = document.createElement("input");

    contResp.classList.add("form__filed");
    inputResponse.classList.add("submit");

    inputResponse.setAttribute("type", "submit");

    inputResponse.value = "quest";

    contResp.append(inputResponse);
    contQuestResponse.append(contResp);
  }
}
