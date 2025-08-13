document.addEventListener("DOMContentLoaded", async () => {
  // امسح الكارت من localStorage كل مرة الصفحة تحمل عشان تبدأ نظيف
  localStorage.removeItem("cartItems");

  let currentProduct = null;

  try {
    const errorContainer = document.getElementById('error-container');
    const loadingElement = document.querySelector('.loading-state');

    const params = new URLSearchParams(window.location.search);
    const productId = params.get('id');

    if (!productId) throw new Error("لم يتم العثور على معرف المنتج في الرابط");

    if (loadingElement) loadingElement.style.display = 'block';

    const response = await fetch(`http://localhost:5204/api/Products/${productId}`);

    if (!response.ok) {
      const errorData = await response.json().catch(() => null);
      throw new Error(errorData?.message || "فشل في جلب بيانات المنتج");
    }

    const product = await response.json();

    if (!product || !product.id) throw new Error("بيانات المنتج غير كاملة");

    currentProduct = product;
    updateProductUI(product);

    // ربط زر الإضافة للعربة
    const addToCartBtn = document.querySelector('#add-to-cart-section button');
    if (addToCartBtn) {
      addToCartBtn.addEventListener('click', () => {
        handleAddToCart(product);
        UIkit.offcanvas('#cart-offcanvas').show(); // فتح السلة الجانبية
      });
    }

  } catch (error) {
    console.error("حدث خطأ:", error);
    showError(error.message);
  } finally {
    const loadingElement = document.querySelector('.loading-state');
    if (loadingElement) loadingElement.style.display = 'none';
  }

  // عند فتح السلة الجانبية، أعرض العناصر
  UIkit.util.on('#cart-offcanvas', 'beforeshow', () => {
    renderCartItems();
  });
});

function updateProductUI(product) {
  document.title = product.name || "تفاصيل المنتج";
  const titleElement = document.querySelector('h1');
  if (titleElement) titleElement.textContent = product.name;

  const brandImg = document.querySelector('#product-brand-image');
  if (brandImg && product.brands) {
    brandImg.src = product.brands.logoUrl || '/images/default-brand.png';
    brandImg.alt = product.brands.name || 'العلامة التجارية';
  }

  const mainImage = document.getElementById('primary-product-image');
  if (mainImage && product.imageUrl) {
    mainImage.src = product.imageUrl;
    mainImage.alt = product.name || 'صورة المنتج';
  }

  updatePriceAndAvailability(product);
  updateDeliveryInfo(product);
}

function updatePriceAndAvailability(product) {
  const priceElement = document.getElementById('product-price');
  const availabilityElement = document.getElementById('product-availability-message');

  if (priceElement) {
    priceElement.textContent = product.price
      ? `$${product.price.toFixed(2)}`
      : 'السعر غير متوفر';
  }

  if (availabilityElement) {
    availabilityElement.textContent = product.inStock
      ? 'متوفر في المخزن'
      : 'غير متوفر حالياً';
    availabilityElement.style.color = product.inStock ? 'green' : 'red';
  }
}

function updateDeliveryInfo(product) {
  const deliveryElement = document.getElementById('delivery-info-text');
  const pickupElement = document.getElementById('pickup-info-text');

  if (deliveryElement) {
    deliveryElement.textContent = product.delivery || 'التوصيل خلال 2-3 أيام عمل';
  }

  if (pickupElement && product.store) {
    pickupElement.innerHTML = `
      <div>${product.store.address}</div>
      <div>مواعيد العمل: ${product.store.hours}</div>
    `;
  }
}

function showError(message) {
  const errorContainer = document.getElementById('error-container');
  if (errorContainer) {
    errorContainer.innerHTML = `
      <div class="alert-error">
        <h3>فشل تحميل المنتج</h3>
        <p>${message}</p>
        <button onclick="window.location.reload()">إعادة المحاولة</button>
      </div>
    `;
    errorContainer.style.display = 'block';
  }
}

// تعديل هنا: استخدم addToCart من cart.js بدون تكرار التعامل مع localStorage هنا
function handleAddToCart(productData) {
  const quantityInput = document.getElementById('product-quantity-input');
  const quantity = parseInt(quantityInput.value) || 1;

  const productWithQty = {
    id: productData.id,
    name: productData.name,
    price: productData.price,
    image: productData.imageUrl,
    quantity: quantity
  };

  addToCart(productWithQty);
}

function renderCartItems() {
  const container = document.getElementById('cart-items-container');
  const cart = getCartItems();

  if (!container) return;

  if (cart.length === 0) {
    container.innerHTML = `<p class="uk-text-muted">سلتك فارغة.</p>`;
    return;
  }

  container.innerHTML = cart.map(item => `
    <div class="uk-margin-small">
      <div class="uk-flex uk-flex-middle">
        <img src="${item.image}" width="40" height="40" class="uk-margin-small-right" alt="${item.name}">
        <div>
          <div>${item.name}</div>
          <div class="uk-text-meta">$${item.price.toFixed(2)} × ${item.quantity}</div>
        </div>
      </div>
    </div>
  `).join('');
}
