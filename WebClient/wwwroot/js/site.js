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
        menu.classList.remove("active");
    }
});


document.querySelectorAll(".button").forEach((button) =>
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

