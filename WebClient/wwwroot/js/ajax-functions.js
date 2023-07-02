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


 $(document).ready(function () {

        $('#citySelect').change(function () {
            var cityCodeName = $(this).val();
            $.ajax({
                url: "/ShippingAddress/GetDistricts",
                type: 'GET',
                data: { cityCodeName: cityCodeName },
                success: function (result) {
                    var districtSelect = $('#districtSelect');
                    districtSelect.empty();
                    $.each(result, function (i, district) {
                        districtSelect.append($('<option></option>').val(district.Codename).text(district.Name));
                    });
                    districtSelect.change(); 
                }
            });
        });

        $('#districtSelect').change(function () {
            var districtCodeName = $(this).val();
            $.ajax({
                url: "/ShippingAddress/GetWards",
                type: 'GET',
                data: { districtCodeName: districtCodeName },
                success: function (result) {
                    var wardSelect = $('#wardSelect');
                    wardSelect.empty();
                    $.each(result, function (i, ward) {
                        wardSelect.append($('<option></option>').val(ward.Codename).text(ward.Name));
                    });
                }
            });
        });
 });

$(document).ready(function () {
    $("#addNewDelivery").click(function () {
        $.ajax({
            url: "/ShippingAddress/_DeliveryModal",
            type: "GET",
            success: function (data) {
                $("#deliveryModal").html(data);
                // Hiển thị modal ở đây nếu cần
            },
            error: function () {
                // Xử lý lỗi nếu cần
            }
        });
    });
});