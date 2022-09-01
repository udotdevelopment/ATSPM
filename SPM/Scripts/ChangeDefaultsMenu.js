function rotate(element) {
    var element = $(element).children()[0];
    if (element.classList.contains("fa-rotate-90")) {
        element.classList.remove("fa-rotate-90")
    } else {
        element.classList.add("fa-rotate-90");
    }
}