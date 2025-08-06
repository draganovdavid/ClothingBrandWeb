document.addEventListener("DOMContentLoaded", function () {
    let searchBar = document.getElementById("searchBar");
    let cityFilter = document.getElementById("cityFilter");

    function filterWarehouses() {
        let searchValue = searchBar.value.toLowerCase();
        let cityValue = cityFilter.value.toLowerCase();
        let warehouseCards = document.querySelectorAll(".warehouse-card");

        warehouseCards.forEach(warehouse => {
            let warehouseName = warehouse.querySelector(".card-title").textContent.toLowerCase();
            let warehouseCity = warehouse.getAttribute("data-city").toLowerCase();

            let matchesSearch = searchValue === "" || warehouseName.includes(searchValue);
            let matchesCity = cityValue === "" || warehouseCity === cityValue;

            if (matchesSearch && matchesCity) {
                warehouse.style.display = "block";
            } else {
                warehouse.style.display = "none";
            }
        });
    }

    searchBar.addEventListener("keyup", filterWarehouses);
    cityFilter.addEventListener("change", filterWarehouses);
});