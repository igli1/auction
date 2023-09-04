"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/auctionsHub").build();

connection.start().catch(error => console.error(error));

connection.on("ReceiveUpdatedAuctions", function () {
    getAllAuctions();
});
function getAllAuctions(){
    fetch('Home/GetAuctions/')
        .then(response => {
            if (!response.ok) {
                throw new Error("HTTP error " + response.status);
            }
            return response.json();  // This returns a promise
        })
        .then(updatedAuctions => {
            updateTable(updatedAuctions);
        })
        .catch(function() {
            console.log("An error occurred while fetching the auctions.");
        });
}
function updateTable(updatedAuctions){
    let tbody = document.querySelector('.table tbody');

    tbody.innerHTML = '';

    updatedAuctions.forEach(function(auction) {
        let tr = document.createElement('tr');

        let imgTd = document.createElement('td');
        imgTd.className = "image-container-home";
        let image = document.createElement('img');
        image.src = `/Home/GetImage?imageName=${auction.productPhoto}`;
        image.alt = "Product Image";
        image.className = "loading-image";
        image.width = "150";
        image.height = "150";
        image.onload = function() {
            hideSpinner(this);
        };
        let spinnerDiv = document.createElement('div');
        spinnerDiv.className = "spinner-border text-primary image-spinner";
        imgTd.appendChild(image);
        imgTd.appendChild(spinnerDiv);

        let productTd = document.createElement('td');
        let productLink = document.createElement('a');
        productLink.href = `/Home/ProductDetails/${auction.productId}`;
        productLink.textContent = auction.productName;
        productTd.appendChild(productLink);

        let sellerTd = document.createElement('td');
        sellerTd.textContent = auction.sellerName;
        if (auction.isCurrentUserProductOwner) {
            let sellerSpan = document.createElement('span');
            sellerSpan.textContent = "(You)";
            sellerTd.appendChild(sellerSpan);
        }

        let topBidTd = document.createElement('td');
        topBidTd.textContent = auction.topBid;

        let timeRemainingTd = document.createElement('td');
        timeRemainingTd.textContent = `${auction.timeRemaining} Days`;

        let actionTd = document.createElement('td');
        if (auction.isCurrentUserProductOwner) {
            let deleteLink = document.createElement('a');
            deleteLink.href = `/Home/DeleteProduct/${auction.productId}`;
            deleteLink.textContent = "Delete";
            deleteLink.classList.add("btn", "btn-danger");
            actionTd.appendChild(deleteLink);
        } else {
            let emptySpan = document.createElement('span');
            actionTd.appendChild(emptySpan);
        }

        tr.appendChild(imgTd);
        tr.appendChild(productTd);
        tr.appendChild(sellerTd);
        tr.appendChild(topBidTd);
        tr.appendChild(timeRemainingTd);
        tr.appendChild(actionTd);

        tbody.appendChild(tr);
    });
}