
window.addEventListener("DOMContentLoaded",()=>{
    const user = localStorage.getItem('user');
    if(user != '' && user){
        document.getElementById('login__username').value = user;
    }
    
    document.querySelector('#submit').addEventListener('click' , (e) =>{
        e.preventDefault()
        let value = document.getElementById('login__username').value;
        if(value == '') return alert('Username is required !')
        localStorage.setItem('user', value);
        // loginInputBackgroundColor: #3b4148; remove
        //  background-color: var(--loginInputBackgroundColor);
        // background-color: var();
    })
})