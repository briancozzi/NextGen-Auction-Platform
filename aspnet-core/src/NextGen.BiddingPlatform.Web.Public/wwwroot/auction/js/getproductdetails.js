$(function () {
    GetAuctionItem();
});
function GetAuctionItem() {
    var id = $("#auctionItemId").val();
    var itemStatus = $("#itemStatus").val();
    $.ajax({
        url: ApiServerPath + "/api/services/app/AuctionItem/GetAuctionItemWithHistory?Id=" + id + "&itemStatus=" + itemStatus + "&userId=" + userId,
        type: "GET",
        contentType: "application/json",
        cache: false,
        dataType: "json",
        success: function (response) {
            if (response != null || response != undefined) {
                var data = response.result;
                $(".name").text(data.itemName);
                $("#lastBidAmount").text("$" + data.lastBidAmount.toFixed(2));
                $("#lastBidWinner").text(data.lastBidWinnerName);
                $(".price").text(data.lastBidAmount.toFixed(2));
                $("#fmvValue").text("$" + data.fairMarketValue_FMV.toFixed(2));
                $("#totalBids").text(data.totalBidCount);
                $("#itemDescription").text(data.itemDescription);
                var imageFullPath = ApiServerPath + data.imageName;
                $("#itemImage").attr("src", imageFullPath);
                $("#minimumBidValue").text("$" + data.bidStepIncrementValue);
                $("#bidNowBtn").attr("data-minimumBidAmount", data.bidStepIncrementValue);
                $("#currentUserAuctionHistoryCount").val(data.currentUserAuctionHistoryCount);
                $("#auctionBidderId").val(data.currUserBiddingId);
                $("#auctionBidderName").val(data.currUserBidderName);
                $("#bidderNameFromDb").text(data.currUserBidderName)
                var itemHistories = data.auctionItemHistories;
                $.each(itemHistories, function (i, v) {
                    CreateHistoryData(v);
                });
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText + " " + xhr.status);
        }
    });
}


function CreateHistoryData(data) {
    data.bidAmount = data.bidAmount.toFixed(2);
    var result = Mustache.render($("#auctionHistoryTemplate").html(), data)
    $("#itemHistories").append(result);
}


