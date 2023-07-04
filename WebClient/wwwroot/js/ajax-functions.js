// ajax-functions.js

document.querySelectorAll(".button-atc").forEach((button) =>
    button.addEventListener("click", (e) => {
        if (!button.classList.contains("loading")) {
            button.classList.add("loading");
            setTimeout(() => button.classList.remove("loading"), 3700);
        }
        e.preventDefault();
    })
);

$(document).ready(function () {
    $(".product-detail-atc-btn").click(function () {
        var itemCode = $(this).data("code");
        var quantity = $('#quantityInput').val();
        if (quantity == null) {
            quantity = 1;
        }
        addToCart(itemCode, quantity);
    });

    $(".button-atc").click(function () {
        var itemCode = $(this).data("code");
        var quantity = 1;
        addToCart(itemCode, quantity);
    });
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
    calculateTotal();
    updateCartDisplay();

    function updateCartDisplay() {
        var cartItemsCount = $('.cart-item').length;

        if (cartItemsCount > 0) {
            $('.cart-have-item').show();
            $('.cart-img-empty').hide();
        } else {
            $('.cart-have-item').hide();
            $('.cart-img-empty').show();
        }
    }

    $('.button-minus-checkout').click(function () {
        var input = $(this).siblings('.btn-plus-minus-count-checkout');
        var currentValue = parseFloat(input.val());
        var newValue = currentValue - 1;
        var cartItem = $(this).closest(".cart-item");
        var id = cartItem.data("id");
        if (currentValue > 1) {
            input.val(newValue);
            calculateSubtotal($(this).closest('.cart-item'));
            calculateTotal();
            updateQuantity(id, newValue);
        } else {
            showDeletePopup($(this).closest('.cart-item'));
        }

    });

    $('.cart-item-remove-text').click(function () {
        var cartItem = $(this).closest('.cart-item');
        var id = cartItem.data('id');
        showDeletePopup(cartItem, id);
    });

    function showDeletePopup(cartItem) {
        $('#modalDeleteItemCart').show();
        var id = cartItem.data("id");
        $('#confirmDelete').click(function () {
            updateQuantity(id, 0);
            deleteCartItem(cartItem);
            hideDeletePopup();
        });

        $('#cancelDelete').click(function () {
            hideDeletePopup();
        });

        $('.modal-confirmation-delete-cart').click(function () {
            hideDeletePopup();
        });

        function hideDeletePopup() {
            $('#modalDeleteItemCart').hide();
        }
    }

    function deleteCartItem(cartItem) {
        cartItem.remove();
        calculateTotal();
        updateCartDisplay();
    }

    $('.button-plus-checkout').click(function () {
        var input = $(this).siblings('.btn-plus-minus-count-checkout');
        var currentValue = parseFloat(input.val());
        var maxValue = parseFloat(input.attr('max'));
        var newValue = currentValue + 1;
        var cartItem = $(this).closest(".cart-item");
        var id = cartItem.data("id");
        if (currentValue < maxValue) {
            input.val(newValue);
            calculateSubtotal($(this).closest('.cart-item'));
            calculateTotal();
            updateQuantity(id, newValue);
        }
    });

    $('.btn-plus-minus-count-checkout').on('change', function () {
        var inputValue = parseInt($(this).val());
        if (inputValue === 0) {
            showDeletePopup($(this).closest('.cart-item'));
        } else if (inputValue < 1 || inputValue > 1000) {
            $(this).val(1);
        } else {
            calculateSubtotal($(this).closest('.cart-item'));
            calculateTotal();
            var cartItem = $(this).closest(".cart-item");
            var id = cartItem.data("id");
            updateQuantity(id, inputValue);
        }
    });


    function calculateTotal() {
        var total = 0;
        $('.cart-item').each(function () {
            var subtotal = parseFloat($(this).find('.cart-item-content-money p#itemSubPrice').text());
            total += subtotal;
        });
        $('#totalAmount').text('$' + total.toFixed(2));
    }
    function updateQuantity(id, quantity) {
        $.ajax({
            url: "/Checkout/UpdateQuantity",
            type: "POST",
            data: {
                id: id,
                quantity: quantity
            },
            success: function (response) {
                console.log("Quantity updated successfully");
                updateCartDisplay();
            },
            error: function (xhr, status, error) {
                console.error("Error updating quantity: " + error);
            }
        });
    }
    function calculateSubtotal(cartItem) {
        var price = parseFloat(cartItem.find('.cart-item-content-money p#itemPrice').text());
        var quantity = parseFloat(cartItem.find('.btn-plus-minus-count-checkout').val());
        var subtotal = price * quantity;
        cartItem.find('.cart-item-content-money p#itemSubPrice').text(subtotal.toFixed(2));
    }
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


