// ajax-functions.js
$(document).ready(function () {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
});
$(document).ready(function () {
    $('#login-form').on('submit', function (event) {
        event.preventDefault();

        // Xử lý đăng nhập
        var formData = $(this).serialize();
        $.ajax({
            url: 'Login/Login',
            type: 'POST',
            data: formData,
            success: function (response) {
                toastr.success('Login Success!');
                window.location.href = response.redirectUrl;
            },
            error: function (xhr, status, error) {
                if (xhr.status == 400) {
                    toastr.error('Login Fail!');
                }
                console.error(error);
            }
        });
    });
});




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
                toastr.success('Added to cart successfully!')
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

    function updateShippingAddresses() {
        $.ajax({
            url: "/ShippingAddress/GetDeliveryAddresses",
            type: "GET",
            success: function (data) {
                var html = '';
                for (var i = 0; i < data.length; i++) {
                    var item = data[i];
                    html += '<div class="hero-shipping-address-item">';
                    html += '<input type="radio" class="delivery-radio" name="selectedAddress" value="' + item.shippingAddressId + '" />';
                    html += '<div class="shipping-address-item">';
                    html += '<p>' + item.fullName + '</p>';
                    html += '<p>' + item.city + '</p>';
                    html += '<p>+84 ' + item.phoneNumber + '</p>';
                    html += '</div></div>';
                }
                $(".hero-delivery-table").html(html);
                $(".delivery-information-modal").css("display", "none");
            },
            error: function () {
            }
        });
    }

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
        $.ajax({
            url: "/ShippingAddress/Create",
            type: "POST",
            data: deliveryData,
            success: function (response) {
                toastr.success(response.value.successMessage);
                updateShippingAddresses();
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



    $('.checkout-btn').on('click', function (event) {
        event.preventDefault();
        var selectedAddress = $('input[name="selectedAddress"]:checked').val();
        if (selectedAddress) {
            createOrder(selectedAddress);
        } else {
            toastr.warning('Please select a shipping address!!');
        }
    });
 
    
    function createOrder(shippingAddressId) {
        $('.container-checkout-body').append('<div id="loaderOverlay" class="loader-overlay"><div class="loader"></div></div>');

        $.ajax({
            url: '/Checkout/Order',
            type: 'POST',
            data: { shippingAddressId: shippingAddressId },
            success: function (response) {
                var responseMessage = response.value;
                if (responseMessage.errorMessage != null) {
                    toastr.error('Cart is empty.');
                } else {
                    toastr.success(responseMessage.successMessage);
                    $('.cart-item').remove();
                    updateCartDisplay();
                }
            },
            error: function (xhr) {
                if (xhr.status === 400) {
                    var response = xhr.responseJSON.value;
                    toastr.error(response.errorMessage);
                }
                else {
                    alert('An error occurred while creating the order.');
                }
            },
            complete: function () {
                $('#loaderOverlay').remove();
            }
        });
    }
});





$(document).ready(function () {
    var filterTable = $("#filterTable");

    $("#filterButton").click(function (event) {
        filterTable.toggle();
        event.stopPropagation();
    });

    $("#applyFilterButton").click(function () {
        $("#errorImage").css("display", "none");
        applyFilter();
    });

    $(document).click(function (event) {
        var target = $(event.target);
        if (!target.closest("#filterButton").length && !target.closest("#filterTable").length) {
            filterTable.hide();
        }
    });

    var pageType = $(".hero-filter").data("page-type");
    console.log("Page type:", pageType);

    function applyFilter() {
        var selectedPrice = $("input[name='price']:checked").map(function () {
            var priceRange = this.value.split('-');
            var minPrice = 0;
            var maxPrice = 0;
            var onlyPrice = 0; 

            if (priceRange.length === 2) {
                minPrice = parseInt(priceRange[0]);
                maxPrice = parseInt(priceRange[1]);
            } else {
                onlyPrice = parseInt(this.value);
            }          
            return {
                Min: minPrice,
                Max: maxPrice,
                Only: onlyPrice
            };
        }).get()[0];


        var selectedBirds = $("input[name='birdId']:checked").map(function () {
            return parseInt(this.value);
        }).get();

        var sortValue = {
            PageType: pageType,
            SelectedPrice: selectedPrice,
            SelectedBirds: selectedBirds
        };
        

        $.ajax({
            url: "/Home/ApplyFilter",
            type: "POST",
            data: sortValue,
            success: function (response) {
                if (sortValue.PageType === "Meal") {
                    var mealContainer = $("#mealContainer");
                    mealContainer.empty(); 

                    for (var i = 0; i < response.length; i++) {
                        var mealItem = response[i];

                        var gridItemMeal = $('<div class="grid-item-meal"></div>');

                        var aLink = $('<a class="a-text-link"></a>')
                            .attr('href', '/Home/Detail?code=' + mealItem.mealCode);

                        var itemImage = $('<div class="item-image"></div>')
                            .append($('<img />').attr('src', mealItem.mealImage).attr('alt', mealItem.mealName));

                        var itemDetails = $('<div class="item-details"></div>')
                            .append($('<h5></h5>').text(mealItem.mealName))
                            .append($('<p></p>').html('<i class="fa-solid fa-dollar-sign"></i>' + mealItem.price.toFixed(2)));

                        var buttonATC = $('<button class="button-atc"></button>')
                            .attr('data-code', mealItem.mealCode)
                            .append($('<span>Add to cart</span>'))
                            .append($('<div class="cart"></div>')
                                .append($('<svg viewBox="0 0 36 26"><polyline points="1 2.5 6 2.5 10 18.5 25.5 18.5 28.5 7.5 7.5 7.5"></polyline><polyline points="15 13.5 17 15.5 22 10.5"></polyline></svg>')));

                        aLink.append(itemImage);
                        aLink.append(itemDetails);
                        gridItemMeal.append(aLink);
                        gridItemMeal.append(buttonATC);

                        mealContainer.append(gridItemMeal);
                    }
                }
                else {
                    var productContainer = $("#foodContainer");
                    productContainer.empty(); 

                    for (var i = 0; i < response.length; i++) {
                        var productItem = response[i];

                        var gridItemProduct = $('<div class="grid-item-meal"></div>');

                        var aLink = $('<a class="a-text-link"></a>')
                            .attr('href', '/Home/Detail?code=' + productItem.productCode);

                        var itemImage = $('<div class="item-image"></div>')
                            .append($('<img />').attr('src', productItem.productImage).attr('alt', productItem.productName));

                        var itemDetails = $('<div class="item-details"></div>')
                            .append($('<h5></h5>').text(productItem.productName))
                            .append($('<p></p>').html('<i class="fa-solid fa-dollar-sign"></i>' + productItem.price.toFixed(2)));

                        var buttonATC = $('<button class="button-atc"></button>')
                            .attr('data-code', productItem.productCode)
                            .append($('<span>Add to cart</span>'))
                            .append($('<div class="cart"></div>')
                                .append($('<svg viewBox="0 0 36 26"><polyline points="1 2.5 6 2.5 10 18.5 25.5 18.5 28.5 7.5 7.5 7.5"></polyline><polyline points="15 13.5 17 15.5 22 10.5"></polyline></svg>')));

                        aLink.append(itemImage);
                        aLink.append(itemDetails);
                        gridItemProduct.append(aLink);
                        gridItemProduct.append(buttonATC);

                        productContainer.append(gridItemProduct);
                    }
                }
            },
            error: function (xhr, status, error) {
                if (xhr.status === 404) {
                    if (sortValue.PageType === "Meal") {
                        var mealContainer = $("#mealContainer");
                        mealContainer.empty();
                    } else {
                        var productContainer = $("#foodContainer");
                        productContainer.empty(); 
                    }
                    $("#errorImage").css("display", "block");
                } 
            }
        });
        filterTable.hide();
    }
});