document.addEventListener("DOMContentLoaded", async () => {
  const token = localStorage.getItem("token");
  if (!token) {
    // لو مش مسجل دخول، ممكن توجهه لصفحة تسجيل الدخول
    window.location.href = "/login.html";
    return;
  }

  try {
    const response = await fetch("http://localhost:5204/api/UserAuth/me", {
      method: "GET",
      headers: {
        "Authorization": `Bearer ${token}`,
        "Content-Type": "application/json"
      }
    });

    if (!response.ok) {
      if (response.status === 401) {
        // توجيه لصفحة تسجيل الدخول لو التوكن غير صالح
        alert("Session expired, please login again.");
        localStorage.removeItem("token");
        window.location.href = "/login.html";
      } else {
        throw new Error("Failed to fetch user data.");
      }
      return;
    }

    const userData = await response.json();

    // مثال: عرض البيانات في صفحة الـ account
    // لازم يكون في HTML عناصر بعرفها هنا عشان اعرض البيانات فيها
    document.getElementById("user-name").textContent = `${userData.firstName} ${userData.lastName}`;
    document.getElementById("user-email").textContent = userData.email;
    document.getElementById("user-phone").textContent = userData.phone;
    document.getElementById("user-role").textContent = userData.role;
    document.getElementById("user-joined").textContent = userData.registrationDate;

    // لو في صورة بروفايل
    document.getElementById("user-image").src = userData.image;

  } catch (error) {
    console.error(error);
    alert("An error occurred while loading your account.");
  }
});
