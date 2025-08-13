document.addEventListener("DOMContentLoaded", function () {
  const form = document.getElementById("checkout-form");

  const firstName = document.getElementById("firstName");
  const lastName = document.getElementById("lastName");
  const phoneNumber = document.getElementById("phoneNumber");
  const email = document.getElementById("email");

  const shippingMethodRadios = document.querySelectorAll('input[name="shippingMethod"]');
  const paymentMethodRadios = document.querySelectorAll('input[name="paymentMethod"]');

  // افتراضياً الكارت عندك مثلاً:
  let cart = [
    // مثال
    { ProductId: 1, Name: "منتج 1", Price: 100, Quantity: 2 },
    { ProductId: 2, Name: "منتج 2", Price: 50, Quantity: 1 },
  ];

  function getSelectedValue(radioNodeList) {
    const selected = Array.from(radioNodeList).find(r => r.checked);
    return selected ? selected.value : null;
  }

  form.addEventListener("submit", async function (e) {
    e.preventDefault();

    // قم بعمل تحقق validation كما عندك
    // ...

    // بعد التحقق، نجهز البيانات للإرسال:
    const orderData = {
      UserId: 1, // ممكن تجيبه من الجلسة أو تخليه ثابت مؤقتاً
      TotalAmount: cart.reduce((sum, item) => sum + item.Price * item.Quantity, 0),
      Items: cart.map(item => ({
        ProductId: item.ProductId,
        Quantity: item.Quantity,
        Price: item.Price
      })),
      ShippingAddress: {
        Address: "عنوان افتراضي", // لو عندك حقل عنوان في الفورم ضيفه هنا
        City: "مدينة",
        Country: "دولة",
        Zipcode: "12345"
      },
      PaymentMethod: getSelectedValue(paymentMethodRadios)
    };

    // إرسال البيانات للسيرفر
    try {
      const response = await fetch("/api/checkout/placeorder", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(orderData),
      });

      const result = await response.json();
      if (response.ok) {
        alert("تم تقديم الطلب بنجاح! رقم الطلب: " + result.OrderId);
        // ممكن تفرغ الكارت أو توجه المستخدم لصفحة شكراً
      } else {
        alert("خطأ: " + result.message || "فشل في تقديم الطلب");
      }
    } catch (error) {
      alert("حدث خطأ في الاتصال بالخادم");
      console.error(error);
    }
  });
});
