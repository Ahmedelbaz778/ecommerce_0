// cart.js

// امسح الكارت من localStorage كل مرة الصفحة تحمل عشان تبدأ نظيف (لو بتحط cart.js في صفحات مختلفة)
// لو حابب تحكم المسح فقط في product.js ممكن تحذف السطر ده هنا
// localStorage.removeItem("cartItems");

// إرجاع المنتجات المخزنة في localStorage
function getCartItems() {
  return JSON.parse(localStorage.getItem("cartItems")) || [];
}

// حفظ المنتجات
function saveCartItems(items) {
  localStorage.setItem("cartItems", JSON.stringify(items));
}

// إضافة منتج جديد (أو تحديث الكمية إذا موجود)
function addToCart(product) {
  const cart = getCartItems();
  const existing = cart.find(p => p.id === product.id);
  if (existing) {
    existing.quantity += product.quantity;
  } else {
    cart.push(product);
  }
  saveCartItems(cart);
  updateCartUIs();
}

// تحديث جميع الكروت في الصفحة (navbar + offcanvas + أي عنصر تاني)
function updateCartUIs() {
  const cart = getCartItems();

  // تحديث رقم العناصر في الأيقونة
  const countEl = document.querySelector(".cart-count");
  if (countEl) countEl.textContent = cart.length;

  // تحديث الكارت الجانبي
  const container = document.getElementById("cart-items-container");
  const subtotalEl = document.getElementById("cart-subtotal");
  if (container && subtotalEl) {
    if (cart.length === 0) {
      container.innerHTML = "<p class='uk-text-muted'>سلتك فارغة.</p>";
      subtotalEl.textContent = "$0.00";
      return;
    }

    let total = 0;
    container.innerHTML = "";
    cart.forEach(item => {
      total += item.price * item.quantity;
      container.innerHTML += `
        <div class="uk-flex uk-flex-between uk-margin-small-bottom">
          <span>${item.name} × ${item.quantity}</span>
          <span>$${(item.price * item.quantity).toFixed(2)}</span>
        </div>`;
    });

    subtotalEl.textContent = `$${total.toFixed(2)}`;
  }
}

// لما الصفحة تفتح، نحدث الكارت تلقائيًا
document.addEventListener("DOMContentLoaded", updateCartUIs);
