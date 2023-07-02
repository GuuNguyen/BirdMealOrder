﻿var lastScrollTop = 0;
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

/*========== delivery model ==========*/
var modal = document.getElementById("deliveryModal");
var btn = document.getElementById("addNewDelivery");
var closeBtn = document.getElementsByClassName("delivery-information-modal-close")[0];

btn.onclick = function () {
    modal.style.display = "block";
}

closeBtn.onclick = function () {
    modal.style.display = "none";
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}