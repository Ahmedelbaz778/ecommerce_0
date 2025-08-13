document.addEventListener("DOMContentLoaded", function () {
  const form = document.getElementById("register-form");

  form.addEventListener("submit", async function (e) {
    e.preventDefault();

    const name = document.getElementById("username").value.trim();
    const email = document.getElementById("email").value.trim();
    const password = document.getElementById("password").value;
    const phone = document.getElementById("Phone").value;
    const role = "User"; // أو سيبه فاضي لو مش محتاج

    try {
      const response = await fetch("http://localhost:5204/api/UserAuth/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          name,
          email,
          password,
          phone,
          role
        })
      });

      if (response.ok) {
        Swal.fire({
          icon: 'success',
          title: 'Registration Successful',
          text: 'Redirecting to login page...',
          timer: 2000,
          showConfirmButton: false
        }).then(() => {
          window.location.href = "login.html";
        });
      } else {
        const error = await response.text();
        Swal.fire({
          icon: 'error',
          title: 'Registration Failed',
          text: error || 'Please check your inputs'
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
