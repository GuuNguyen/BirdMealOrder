var lastScrollTop = 0;
var navbar = document.getElementById('navbar');

window.addEventListener('scroll', function () {
    var scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    if (scrollTop > lastScrollTop) {
        navbar.style.transform = 'translateY(-100%)';
    } else {
        navbar.style.transform = 'translateY(0)';
    }

    lastScrollTop = scrollTop;
});
function menuToggle() {
    const toggleMenu = document.querySelector(".menu");
    const isActive = toggleMenu.classList.contains("active");

    if (isActive) {
        toggleMenu.classList.remove("active");
    } else {
        toggleMenu.classList.add("active");
    }
}

document.addEventListener("click", function (event) {
    const menu = document.querySelector(".menu");
    const targetElement = event.target;

    if (!targetElement.closest(".menu") && !targetElement.closest(".action")) {
        menu.setAttribute("class", menu.getAttribute("class").replace("active", "").trim());
    }
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

$(document).ready(function() {
  var filterTable = $("#filterTable");

  $("#filterButton").click(function() {
    filterTable.toggle();
    event.stopPropagation();
  });

  $("#applyFilterButton").click(function() {
    applyFilter();
  });

  $(document).click(function(event) {
    var target = $(event.target);
    if (!target.closest("#filterButton").length && !target.closest("#filterTable").length) {
      filterTable.hide();
    }
  });

  function applyFilter() {
    var selectedPrices = $("input[name='price']:checked").map(function() {
      return this.value;
    }).get();

    console.log(selectedPrices);
    filterTable.hide();
  }
});

$(document).ready(function () {
    $(".detail-btn-plus").click(function () {
        var quantityInput = $("#quantityInput");
        var currentValue = parseInt(quantityInput.val());
        quantityInput.val(currentValue + 1);
    });

    $(".detail-btn-minus").click(function () {
        var quantityInput = $("#quantityInput");
        var currentValue = parseInt(quantityInput.val());
        if (currentValue > 1) {
            quantityInput.val(currentValue - 1);
        }
    });
});


$(document).ready(function () {
    calculateTotal();
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
    function showDeletePopup(cartItem) {
        var cartItem = $(this).closest(".cart-item");
        var id = cartItem.data("id");
        $('.overlay').hide();
        $('#deletePopup').show();
        $('#confirmDelete').click(function () {
            updateQuantity(id, 0); 
            deleteCartItem(cartItem);
            $('.overlay').hide();
        });
        $('#cancelDelete, #deletePopup').click(function () {
            $('.overlay').hide();
        });
    }

    function deleteCartItem(cartItem) {
        cartItem.remove();
        calculateTotal();
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
        calculateSubtotal($(this).closest('.cart-item'));
        calculateTotal();
        var cartItem = $(this).closest(".cart-item");
        var id = cartItem.data("id");
        var quantity = parseInt($(this).val());
        console.log(id, quantity);
        updateQuantity(id, quantity);
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
