document.addEventListener('DOMContentLoaded', function (e) {
    $("#registerModal").modal("show");
    $(".modal-backdrop").removeClass("modal-backdrop");
    $("body").css("background-image", 'url("../images/Deep Cheese.jpg")');

    var homeAnchor = document.getElementById("homeAnchor");
    var loginBtn = document.getElementById("registerLoginBtn");
    var footer = document.getElementById("footer");

    $(".close").on("click", () => {
        homeAnchor.click();
    });
    document.getElementById("footer").classList.add("fixed-bottom");
    loginBtn.onclick = function (e) {
        $("#registerModal").modal("hide");
        document.getElementById("loginBtn").click();
    };
    setFooterVisibility(true);
});

document.addEventListener("click", function (e) {
    var registerModal = document.getElementById("registerModal");
    var x = e.x;
    var y = e.y;

    if (x > 0 && (x < registerModal.children[0].offsetLeft || x > (registerModal.children[0].offsetWidth * 2))) {
        homeAnchor.click();
    }
    if (y > 0 && (y < registerModal.children[0].offsetTop || y > registerModal.children[0].offsetHeight)) {
        homeAnchor.click();
    }
});

var listOfImages = [
    { count: 0, image: "../images/Deep Cheese.jpg" },
    { count: 1, image: "../images/Granddaddy Purple Plant.jpg" },
    { count: 2, image: "../images/Sugar Leaf.jpg" },
];
var imageCounter = 1;

setInterval(function () {
    var imageToUse = listOfImages.find(x => x.count === imageCounter)?.image;
    $("body").css("background-image", 'url("' + imageToUse + '")');
    $("body").css("background-size", "cover");
    imageCounter++
    if (imageCounter >= listOfImages.length)
        imageCounter = 0;
}, 3000);

setFooterVisibility = function (value) {
    footer.setAttribute("hidden", `${value}`);
};