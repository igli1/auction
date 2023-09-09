document.addEventListener("DOMContentLoaded", function() {
    let table = document.querySelector("table");
    let row = document.querySelector(".rowDetails");
    if(table) {
        let rows = table.querySelectorAll("tr");
        
        rows.forEach(function(row) {
            processImageLoaders(table);
        });
    }
    else if(row){
        const image = document.querySelector(".loading-image");
        showSpinner(image);
    }});
    
function processImageLoaders(parentElement) {
    const images = parentElement.querySelectorAll('.loading-image');
    images.forEach(img => {
        showSpinner(img);
    });
}

function showSpinner(img){
    const spinner = img.nextElementSibling;;
    if (img.complete) {
        spinner.style.display = 'none';
    } else {
        spinner.style.display = 'block';
    }
}

function hideSpinner(imgElement) {
    const spinner = imgElement.nextElementSibling;
    
    if (spinner && spinner.classList.contains('loader')) {
        spinner.style.display = 'none';
    }
    imgElement.style.visibility = 'visible';
}