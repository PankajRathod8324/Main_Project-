// const togglePassword = document.querySelector('#togglePassword');
// const password = document.querySelector('#password');

// togglePassword.addEventListener('click', function (e) {
//     // toggle the type attribute
//     const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
//     password.setAttribute('type', type);
//     // toggle the eye icon
//     this.classList.toggle('fa-eye-slash');
// });

// document.addEventListener('DOMContentLoaded', function() {
//     const emailInput = document.getElementById('email');
//     const emailMessage = document.getElementById('emailMessage');

//     emailInput.addEventListener('focus', function() {
//         emailMessage.style.display = 'none';
//     });

//     emailInput.addEventListener('blur', function() {
//         if (emailInput.value.trim() === '') {
//             emailMessage.style.display = 'block';
//         }
//     });
// });


// document.addEventListener('DOMContentLoaded', function() {
//     const passwordInput = document.getElementById('password');
//     const passwordMessage = document.getElementById('passwordMessage');

//     passwordInput.addEventListener('focus', function() {
//         passwordMessage.style.display = 'none';
//     });

//     passwordInput.addEventListener('blur', function() {
//         if (passwordInput.value.trim() === '') {
//             passwordMessage.style.display = 'block';
//         }
//     });
// });
document.querySelector("#loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    let isValid = true;

    if (!email) {
        document.getElementById("emailMessage").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("emailMessage").style.display = "none";
    }

    if (!password) {
        document.getElementById("passwordMessage").style.display = "block";
        isValid = false;
    } else {
        document.getElementById("passwordMessage").style.display = "none";
    }

    if (!isValid) {
        return;
    }

    const response = await fetch("/api/User/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ email, password }),
    });

    if (response.ok) {
        const user = await response.json();
        window.location.href = "/User/UserPage";
    } else {
        const error = await response.text();
        document.getElementById("loginErrorMessage").innerText = error;
        document.getElementById("loginErrorMessage").style.display = "block";
    }
});

document.getElementById("togglePassword").addEventListener("click", function () {
    const passwordInput = document.getElementById("password");
    const type = passwordInput.getAttribute("type") === "password" ? "text" : "password";
    passwordInput.setAttribute("type", type);
    this.classList.toggle("fa-eye-slash");
});
