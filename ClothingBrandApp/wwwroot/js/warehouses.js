document.addEventListener("DOMContentLoaded", function () {
    let searchBar = document.getElementById("searchBar");
    let cityFilter = document.getElementById("cityFilter");

    function filterWarehouses() {
        let searchValue = searchBar.value.toLowerCase();
        let cityValue = cityFilter.value.toLowerCase();
        let cinemaCards = document.querySelectorAll(".warehouse-card");

        cinemaCards.forEach(cinema => {
            let cinemaName = cinema.querySelector(".card-title").textContent.toLowerCase();
            let cinemaCity = cinema.getAttribute("data-city").toLowerCase();

            let matchesSearch = searchValue === "" || cinemaName.includes(searchValue);
            let matchesCity = cityValue === "" || cinemaCity === cityValue;

            if (matchesSearch && matchesCity) {
                cinema.style.display = "block";
            } else {
                cinema.style.display = "none";
            }
        });
    }

    searchBar.addEventListener("keyup", filterWarehouses);
    cityFilter.addEventListener("change", filterWarehouses);
});