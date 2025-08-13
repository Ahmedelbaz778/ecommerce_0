document.addEventListener("DOMContentLoaded", function () {
  const input = document.getElementById("search-input");
  const resultsContainer = document.getElementById("product-list");

  let currentPage = 1;
  let currentQuery = "";
  const pageSize = 6;

  input.addEventListener("keydown", async function (e) {
    const query = input.value.trim();

    if (e.key === "Escape") {
      resultsContainer.style.display = "none";
      return;
    }

    if (query === "") {
      resultsContainer.style.display = "none";
      return;
    }

    if (e.key === "Enter") {
      e.preventDefault();
      if (!query) return;

      currentPage = 1;
      currentQuery = query;

      try {
        const response = await fetch(`http://localhost:5204/api/Search/products?query=${encodeURIComponent(query)}&pageNumber=${currentPage}&pageSize=${pageSize}`);
        const data = await response.json();
        displayResults(data, true);
      } catch (error) {
        console.error(error);
        resultsContainer.innerHTML = "<p class='uk-text-danger'>Something went wrong...</p>";
        resultsContainer.style.display = "block";
      }
    }
  });

  function displayResults(products, reset = false) {
    if (reset) resultsContainer.innerHTML = "";

    resultsContainer.style.display = "block";

    if (!products || products.length === 0) {
      if (reset) {
        resultsContainer.innerHTML = "<p class='uk-text-muted'>No products found</p>";
      }
      return;
    }

    let grid = resultsContainer.querySelector(".uk-grid");

    if (!grid) {
      grid = document.createElement("div");
      grid.className = "uk-grid-small uk-child-width-1-2@s uk-child-width-1-3@m uk-grid-match";
      grid.setAttribute("uk-grid", "");
      resultsContainer.appendChild(grid);
    }

    products.forEach(p => {
      const cardWrapper = document.createElement("div");

      cardWrapper.innerHTML = `
        <a href="product.html?id=${p.id}" class="uk-link-reset">
          <div class="uk-card uk-card-default uk-card-small uk-card-hover uk-card-body">
            <div class="uk-text-center uk-margin-small-bottom">
              <img src="${p.imageUrl}" alt="${p.name}" style="max-height: 150px; object-fit: contain;">
            </div>
            <h4 class="uk-card-title uk-margin-small-top">${p.name}</h4>
            <p><strong>$${p.price}</strong></p>
          </div>
        </a>
      `;

      grid.appendChild(cardWrapper);
    });

    addLoadMoreButton(products.length);
    UIkit.update(resultsContainer);
  }

  function addLoadMoreButton(lastFetchedCount) {
    const oldBtn = document.getElementById("load-more-btn");
    if (oldBtn) oldBtn.remove();

    if (lastFetchedCount === pageSize) {
      const btn = document.createElement("button");
      btn.id = "load-more-btn";
      btn.textContent = "Load More";
      btn.className = "uk-button uk-button-default uk-width-1-1 uk-margin-top";

      btn.addEventListener("click", async () => {
        currentPage++;
        try {
          const response = await fetch(`http://localhost:5204/api/Search/products?query=${encodeURIComponent(currentQuery)}&pageNumber=${currentPage}&pageSize=${pageSize}`);
          const data = await response.json();
          displayResults(data);
        } catch (error) {
          console.error(error);
        }
      });

      resultsContainer.appendChild(btn);
    }
  }
});
