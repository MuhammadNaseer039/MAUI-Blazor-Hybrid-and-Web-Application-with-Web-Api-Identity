$(document).ready(function () {
    console.log("File is loaded");

    // Use event delegation for Blazor/MAUI Web compatibility
    $(document).on("click", "#menu-toggle", function (e) {
        console.log("Button Clicked");
        e.preventDefault();
        $("#sidebar-wrapper").toggleClass("toggled");
    });
});