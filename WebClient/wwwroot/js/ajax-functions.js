// ajax-functions.js

$(document).ready(function () {
    $(".product-detail-atc-btn").click(function () {
        var itemCode = $(this).data("code");
        var quantity = $('#quantityInput').val();
        if (quantity == null) {
            quantity = 1;
        }
        addToCart(itemCode, quantity);
    });

    //$(".product-detail-atc-btn").click(function () {
    //    var itemCode = $(this).data("code");
    //    var quantity = $('#quantityInput').val();
    //    if (quantity == null) {
    //        quantity = 1;
    //    }
    //    addToCart(itemCode, quantity);
    //});
});

function addToCart(itemCode, quantity) {
    $.ajax({
        url: '/Checkout/AddToCart',
        type: 'GET',
        data: { code: itemCode, quantity: quantity },
        success: function (response) {
            if (response.redirectToLogin) {
                window.location.href = '/Login/Login';
            } else {
                $('#successMessage').text("Added to cart successfully!").show().delay(3000).fadeOut();
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}
