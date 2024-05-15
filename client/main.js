

function setUserInLocalStorage() {

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
        
        
        let searching = document.body.appendChild = document.createElement("div");
        searching.id = "searching";
        searching.innerHTML = "<h1>CERCO UN AVVERSARIO .. </h1>";
        document.querySelector(".align").append(searching);
        // for(let i = 0; i< 2; i++){
        //     let wating = document.createElement('span')
        //     wating.style = "font-size:small"
        //     wating.className = "spinner-grow","text-dark";
        //     searching.append(wating);
        // }
    }

    )
}




window.addEventListener("DOMContentLoaded", () => {

    setUserInLocalStorage();

})