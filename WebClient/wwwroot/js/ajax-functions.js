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
    $("#addNewDelivery").click(function (e) {
        e.preventDefault();
        $.ajax({
            url: "/ShippingAddress/_DeliveryModal",
            type: "GET",
            success: function (data) {
                $("#deliveryModalContainer").html(data);
                $(".delivery-information-modal").css("display", "block");
            },
            error: function () {
            }
        });
    });

    $(window).click(function (event) {
        var modal = $(".delivery-information-modal");
        if (event.target === modal[0]) {
            modal.css("display", "none");
        }
    });
    $(document).on('click', '.delivery-submit-btn button:first-child', function () {
        var modal = $(".delivery-information-modal");
        modal.css("display", "none");
    });
});



$(document).ready(function () {
    $(document).on('change', '#citySelect', function () {
        var cityCodeName = $(this).val();
        var selectedCity = $(this).val();
        if (selectedCity) {
            $('#districtSelect').prop('disabled', false);
            $('#wardSelect').prop('disabled', false);
        } else {
            $('#districtSelect').prop('disabled', true);
        }
        $.ajax({
            url: "/ShippingAddress/GetDistricts",
            type: 'GET',
            data: { cityCodeName: cityCodeName },
            success: function (result) {
                var districtSelect = $('#districtSelect');
                districtSelect.empty(); 
                districtSelect.append($('<option></option>').val('').text('Select District')); 
                $.each(result, function (i, district) {
                    districtSelect.append($('<option></option>').val(district.codename).text(district.name));
                });
                districtSelect.change();
            }
        });
    });

    $(document).on('change', '#districtSelect', function () {
        var districtCodeName = $(this).val();
        var selectedDistrict = $(this).val();
        if (selectedDistrict) {
            $('#wardSelect').prop('disabled', false);
        } else {
            $('#wardSelect').prop('disabled', true);
        }
        $.ajax({
            url: "/ShippingAddress/GetWards",
            type: 'GET',
            data: { districtCodeName: districtCodeName },
            success: function (result) {
                var wardSelect = $('#wardSelect');
                wardSelect.empty();
                wardSelect.append($('<option></option>').val('').text('Select Ward')); 
                $.each(result, function (i, ward) {
                    wardSelect.append($('<option></option>').val(ward.codename).text(ward.name));
                });
            }
        });
    });

    $(document).on('submit', '.delivery-information-modal-content', function (event) {
        event.preventDefault();
        var fullName = $('.delivery-username input').val();
        var phoneNumber = $('.delivery-phonenumber input').val();
        var city = $('#citySelect option:selected').text();
        var district = $('#districtSelect option:selected').text();
        var ward = $('#wardSelect option:selected').text();
        var streetAddress = $('.delivery-streetaddress textarea').val();


        var deliveryData = {
            UserId: 0,
            FullName: fullName,
            PhoneNumber: phoneNumber,
            City: city,
            District: district,
            Ward: ward,
            StreetAddress: streetAddress
        };
        console.log(deliveryData)
        $.ajax({
            url: "/ShippingAddress/Create",
            type: "POST",
            data: deliveryData,
            success: function (response) {
                console.log(response)
                window.location.reload();
            },
            error: function () {
            }
        });
    });
});



$(document).ready(function () {
    $('.checkout-btn').on('click', function () {
        var selectedAddress = $('input[name="selectedAddress"]:checked').val();
        if (selectedAddress) {
            createOrder(selectedAddress);
        } else {
            alert('Please select a shipping address.');
        }
    });

    function createOrder(shippingAddressId) {
        $.ajax({
            url: '/Checkout/Order',
            type: 'POST',
            data: { shippingAddressId: shippingAddressId },
            success: function (response) {
                if (response.cartIsEmpty) {
                    alert('Cart is empty.');
                }else if (response.successMessage) {
                    alert(response.successMessage);
                }
            },
            error: function (xhr) {
                var response = xhr.responseJSON;
                if (response && response.errorMessage) {
                    alert(response.errorMessage);
                } else {
                    alert('An error occurred while creating the order.');
                }
            }
        });
    }

});


