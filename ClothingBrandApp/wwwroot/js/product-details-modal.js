document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.view-details-btn').forEach(button => {
        button.addEventListener('click', function () {
            const productId = this.getAttribute('data-product-id');
            const name = this.getAttribute('data-product-name');
            const price = this.getAttribute('data-product-price');
            const stock = this.getAttribute('data-product-stock');
            const image = this.getAttribute('data-product-image');
            const gender = this.getAttribute('data-product-gender');
            const warehouseId = this.getAttribute('data-warehouse-id');

            const content = `
                <div class="row">
                    <div class="col-md-5 text-center">
                        <img src="${image}" class="img-fluid rounded mb-3" alt="${name}" />
                    </div>
                    <div class="col-md-7">
                        <h4>${name}</h4>
                        <p><strong>Product ID:</strong> ${productId}</p>
                        <p><strong>Price:</strong> $${price}</p>
                        <p><strong>Status:</strong> ${stock}</p>
                        <p><strong>Gender:</strong> ${gender}</p>
                    </div>
                </div>
            `;

            document.getElementById('productDetailsContent').innerHTML = content;
            document.getElementById('editProductBtn').setAttribute('href', `/Admin/ProductManagement/Edit/${productId}?warehouseId=${warehouseId}`);

            new bootstrap.Modal(document.getElementById('productDetailsModal')).show();
        });
    });
});
