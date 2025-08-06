document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.view-details-btn').forEach(button => {
        button.addEventListener('click', function () {
            const id = this.getAttribute('data-warehouse-id');
            const name = this.getAttribute('data-warehouse-name');
            const location = this.getAttribute('data-warehouse-location');
            const manager = this.getAttribute('data-warehouse-manager');

            const mapSrc = `https://www.google.com/maps?q=${encodeURIComponent(location)}&output=embed`;

            const content = `
                <div class="mb-3">
                    <h4 class="fw-bold">${name}</h4>
                    <p><strong>Warehouse Id:</strong> ${id}</p>
                    <p><strong>Manager Id:</strong> ${manager}</p>
                    <p><strong>Location:</strong> ${location}</p>
                    <div class="ratio ratio-16x9 mt-3">
                        <iframe src="${mapSrc}" style="border:0;" allowfullscreen="" loading="lazy"></iframe>
                    </div>
                </div>
            `;

            document.getElementById('warehouseDetailsContent').innerHTML = content;
            new bootstrap.Modal(document.getElementById('warehouseDetailsModal')).show();
        });
    });
});
