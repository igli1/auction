 function load() {
    const images = document.querySelectorAll('.loading-image');
    images.forEach(img => {
        const spinner = img.nextElementSibling;
        if (img.complete) {
            spinner.style.display = 'none';
        } else {
            spinner.style.display = 'block';
            img.onload = function () {
                spinner.style.display = 'none';
            };
        }
    });
}
function hideSpinnerDetails(imgElement) {

    const spinner = imgElement.nextElementSibling;
    if (spinner && spinner.classList.contains('image-spinner')) {
        spinner.style.display = 'none';
    }
}