"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/auctionsHub").build();

connection.start().catch(error => console.error(error));

connection.on("ReceiveUpdatedAuctions", function (updatedAuctions) {
    console.log(Date.now());
    $("#auctionTable tbody").empty();

    // Loop through the updated auctions and add them to the table
    updatedAuctions.forEach(function (auction) {
        var row = "<tr>" +
            "<td><a href='/Home/ProductDetails/" + auction.productId + "'>" + auction.productName + "</a></td>" +
            "<td>" + auction.sellerName;

        if (auction.isCurrentUserProductOwner) {
            row += "<span>(You)</span>";
        }

        row += "</td>" +
            "<td>" + (auction.topBid || "") + "</td>" +
            "<td>" + auction.timeRemaining + "</td>" +
            "<td>";

        if (auction.isCurrentUserProductOwner) {
            row += "<a class='btn btn-danger' href='/Home/DeleteProduct/" + auction.productId + "'>Delete</a>";
        }

        row += "</td>" +
            "</tr>";

        $("#auctionTable tbody").append(row);
    });
});