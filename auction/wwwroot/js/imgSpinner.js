document.addEventListener('DOMContentLoaded', function () {
    const img = document.getElementById('loading-image');
    const spinner = document.getElementById('create-link-spinner');

    if (img.complete) {
        spinner.style.display = 'none'; // If the image is already loaded (cached), hide the spinner.
    } else {
        spinner.style.display = 'block'; // Show the spinner if the image is not yet loaded.
        img.onload = function () {
            spinner.style.display = 'none'; // Hide spinner once image is loaded.
        };
    }
});