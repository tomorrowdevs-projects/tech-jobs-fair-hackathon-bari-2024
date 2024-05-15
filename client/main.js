// import { qst } from "./quest";

function setUserInLocalStorage() {

   qst = ciao()

    const user = localStorage.getItem('user');
    if (user != '' && user) {
        document.getElementById('login__username').value = user;
    }

    document.querySelector('#submit').addEventListener('click', (e) => {
        e.preventDefault()
        let value = document.getElementById('login__username').value;
        if (value == '') return alert('Username is required !')
        localStorage.setItem('user', value);
        document.querySelector(".grid").style.display = 'none';
        
        
       /*  let searching = document.body.appendChild = document.createElement("div");
        searching.id = "searching";
        searching.innerHTML = "<h1>CERCO UN AVVERSARIO .. </h1>";
        document.querySelector(".align").append(searching); */
        
//    qst()
    }

    )
}




