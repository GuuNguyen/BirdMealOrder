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



$(document).ready(function () {
    $(".detail-btn-plus").click(function () {
        var quantityInput = $("#quantityInput");
        var currentValue = parseInt(quantityInput.val());
        if (currentValue < 1000) {
            quantityInput.val(currentValue + 1);
        }
    });

    $(".detail-btn-minus").click(function () {
        var quantityInput = $("#quantityInput");
        var currentValue = parseInt(quantityInput.val());
        if (currentValue > 1) {
            quantityInput.val(currentValue - 1);
        }
    });

    $("#quantityInput").on("input", function () {
        var quantityInput = $(this);
        var currentValue = parseInt(quantityInput.val());
        if (currentValue < 1 || isNaN(currentValue)) {
            quantityInput.val(1);
        } else if (currentValue > 1000) {
            quantityInput.val(1000);
        }
    });
});
