document.addEventListener('DOMContentLoaded', function (e) {
    $("#loginModal").modal("show");
    $(".modal-backdrop").removeClass("modal-backdrop");

    var signInUserNameRadio = document.getElementById("signInWithUserName");
    var signInEmailRadio = document.getElementById("signInWithEmail");
    var homeAnchor = document.getElementById("homeAnchor");
    var registerBtn = document.getElementById("loginRegisterBtn");
    var footer = document.getElementById("footer");

    $(".close").on("click", () => {
        homeAnchor.click();
    });
    signInUserNameRadio.onclick = function (e) {
        $("#emailInput").hide("slow");
        $("#userNameInput").show("slow");
    };
    signInEmailRadio.onclick = function (e) {
        $("#userNameInput").hide("slow");
        $("#emailInput").show("slow");
    };
    signInEmailRadio.click();
    setFooterVisibility(true);
    registerBtn.onclick = function (e) {
        $("#loginModal").modal("hide");
        document.getElementById("registerBtn").click();
    };
});

document.addEventListener("click", function (e) {
    var loginModal = document.getElementById("loginModal");

    if (e.x > 0 && (e.x < loginModal.children[0].offsetWidth || e.x > (loginModal.children[0].offsetWidth * 2))) {
        homeAnchor.click();
    }
    if (e.y > 0 && (e.y < loginModal.children[0].offsetTop || e.y > loginModal.children[0].offsetHeight)) {
        homeAnchor.click();
    }
});

var listOfImages = [
    { count: 0, image: "../images/Deep Cheese.jpg" },
    { count: 1, image: "../images/Granddaddy Purple Plant.jpg" },
    { count: 2, image: "../images/Sugar Leaf.jpg" },
];
var imageCounter = 0;

setInterval(function (e) {
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