﻿$(function () {
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
            debugger;
            if (response != null || response != undefined) {
                var data = response.result;


                var endDate = new Date(Date.parse(data.auctionEndDateTime));

                $('#defaultCountdown').countdown({
                    until: endDate,
                    layout: ' {dn} {dl} {hn} {hl} {sn} {sl} '
                });

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
                $("#bidderNameFromDb").text(data.currUserBidderName);
                $("#itemNo").text(data.itemNumber);
                var itemHistories = data.auctionItemHistories;
                $.each(itemHistories, function (i, v) {
                    CreateHistoryData(v);
                });

                //add user viewed item entry

                var input = {
                    userId: userId,
                    auctionItemId: id
                };
                $.ajax({
                    url: ApiServerPath + "/api/services/app/UserViewedItem/AddViewedItem",
                    type: "POST",
                    cache: false,
                    async: true,
                    data: JSON.stringify(input),
                    contentType: "application/json",
                    dataType: "json",
                    success: function (response) {
                        debugger;
                        if (response != null && response.success) {
                            console.log("added item to viewed");
                        }
                        else {
                            alert("error occured!!");
                        }
                    },
                    error: function (xhr) {
                        console.log(xhr.responseText + " " + xhr.status)
                    }
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


