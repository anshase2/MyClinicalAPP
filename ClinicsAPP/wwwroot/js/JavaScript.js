document.addEventListener("DOMContentLoaded", function () {

    const ee = document.getElementById("ee");
    const tt = document.getElementById("tt");
    const loginForm = document.getElementById("loginForm");

    if (ee && tt && loginForm) {
        ee.addEventListener("click", function () {
            tt.style.display = "none";
            loginForm.style.display = "block";
        });
    }

    const form = document.getElementById("rr");
    if (form) {
        form.addEventListener("submit", function (e) {
            let email = document.getElementById("email").value.trim();
            let password = document.getElementById("password").value.trim();

            if (email === "" || password === "") {
                e.preventDefault();
                alert("All fields must be filled out");
                return;
            }

            if (!email.includes("@")) {
                e.preventDefault();
                alert("Enter a valid email");
            }
        });
    }
});
