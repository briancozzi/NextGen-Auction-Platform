const connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalr-BidHub")
    .build();


//This method receive the message and Append to our list  
connection.on("BidSaved", (auctionHistoryDto) => {
    console.log(auctionHistoryDto);
});

connection.start().catch(err => console.error(err.toString()));

//Send the message  

document.getElementById("bidNowBtn").addEventListener("click", event => {
    var auctionHistoryDto = {
        bidderName: $("#bidderName").val(),
        auctionItemId: $("#auctionItemId").val(),
        bidAmount: parseFloat($("#biddingAmount").val()),
        auctionBidderId: parseInt($("#auctionBidderId").val())
    };
    connection.invoke("SaveBid", JSON.stringify(auctionHistoryDto)).catch(err => console.log(err.toString()));
    event.preventDefault();
});   