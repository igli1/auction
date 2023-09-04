document.addEventListener('DOMContentLoaded', function () {
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
});

function hideSpinner(imgElement) {
    
    const spinner = imgElement.nextElementSibling;
    if (spinner && spinner.classList.contains('loader')) {
        console.log(true);
        spinner.style.display = 'none';
    }
}