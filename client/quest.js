

export function qst(){
  let cont = document.createElement('div').className.add("progress");
  let progressDiv = document.createElement('div');
  progressDiv.setAttribute('role','progressbar');
  progressDiv.setAttribute('aria-label','Success example');
  progressDiv.setAttribute('aria-valuenow', '25');
  progressDiv.setAttribute('aria-valuemin', '0');
  progressDiv.setAttribute('aria-valuemax', '100');
  
  let progressBarDiv = document.createElement('div').className.add('progress-bar', 'bg-success')
      progressBarDiv.style.width = "25%";

    progressDiv.append("ttlQst");

    // document.body.append(progressDiv);
}