document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("login-form");
  
    if (!form) return;
  
    form.addEventListener("submit", async function (e) {
      e.preventDefault();
  
      const email = document.getElementById("email").value.trim();
      const password = document.getElementById("password").value;
  
      if (!email || !password) {
        alert("Please enter both email and password.");
        return;
      }
  
      try {
        const response = await fetch("http://localhost:5204/api/UserAuth/login", {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify({ email, password })
        });
  
        if (response.ok) {
          const data = await response.json();
          localStorage.setItem("token", data.token);
  
          Swal.fire({
            icon: 'success',
            title: 'Login Successful',
            text: 'Redirecting to homepage...',
            timer: 1500,
            showConfirmButton: false
          }).then(() => {
            window.location.href = "index.html";
          });
  
        } else {
          const error = await response.text();
          Swal.fire({
            icon: 'error',
            title: 'Login Failed',
            text: error || 'Invalid email or password'
          });
        }
      } catch (err) {
        Swal.fire({
          icon: 'error',
          title: 'Connection Error',
          text: 'Could not connect to server.'
        });
        console.error(err);
      }
    });
  });
  