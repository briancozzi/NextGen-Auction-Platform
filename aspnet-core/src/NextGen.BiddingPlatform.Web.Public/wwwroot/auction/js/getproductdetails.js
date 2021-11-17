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

                var endDate = new Date(Date.parse(data.auctionEndDateTime));

                if (parseInt(data.remainingDays) > 0) {
                    $('#defaultCountdown').countdown({
                        until: endDate,
                        layout: ' {dn} {dl}  ',
                        onExpiry: disableBidding
                    });
                }
                else {
                    $('#defaultCountdown').countdown({
                        until: endDate,
                        layout: ' {hn} {hl} {mn} {ml} {sn} {sl} ',
                        onExpiry: disableBidding
                    });
                }

                //for testing
                //shortly = new Date();
                //shortly.setSeconds(shortly.getSeconds() + 5.5);
                //$('#defaultCountdown').countdown({
                //    until: shortly,
                //    layout: ' {hn} {hl} {mn} {ml} {sn} {sl} ',
                //    onExpiry: disableBidding
                //});

                if (!data.isBiddingStarted) {
                    $("#bidNowBtn").addClass("disbale-btn");
                    $("#lastBidAmount").parent("li").hide();
                }
                else {
                    //do nothing
                    $("#bidNowBtn").removeClass("disbale-btn");
                    $("#lastBidAmount").parent("li").show();
                }

                if (data.isAuctionExpired || data.isBiddingClosed || data.isHide) {
                    $("#bidNowBtn").addClass("disbale-btn");
                }
                else {
                    $("#bidNowBtn").removeClass("disbale-btn");
                }

                $(".name").text(data.itemName);
                $("#lastBidAmount").text("$" + data.lastBidAmount.toFixed(2));
                $("#lastBidWinner").text(data.lastBidWinnerName);
                $(".price").text(data.lastBidAmount.toFixed(2));
                $("#fmvValue").text("$" + data.fairMarketValue_FMV.toFixed(2));
                $("#totalBids").text(data.totalBidCount);
                $("#itemDescription").text(data.itemDescription);
                var imageFullPath = "";
                if (data.imageName != null) {
                    imageFullPath = ApiServerPath + data.imageName;
                }
                else {
                    imageFullPath = WebSiteUrl + "/auction/images/no-img.png";
                }
                $("#itemImage").attr("src", imageFullPath);
                $("#minimumBidValue").text("$" + data.bidStepIncrementValue);
                $("#bidNowBtn").attr("data-minimumBidAmount", data.bidStepIncrementValue);
                $("#currentUserAuctionHistoryCount").val(data.currentUserAuctionHistoryCount);
                $("#auctionBidderId").val(data.currUserBiddingId);
                $("#auctionBidderName").val(data.currUserBidderName);
                $("#bidderNameFromDb").text(data.currUserBidderName);
                $("#itemNo").text(data.itemNumber);
                $("#auctionBiddingClosed").text(data.isBiddingClosed);
                $("#eventUniqueId").val(data.eventUniqueId);
                var itemHistories = data.auctionItemHistories;
                $.each(itemHistories, function (i, v) {
                    CreateHistoryData(v);
                });

                //add user viewed item entry

                var input = {
                    userId: userId,
                    auctionItemId: id,
                    tenantId: tenantId
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

    function disableBidding() {
        $("#biddingAmount").addClass("disbale-btn");
        $("#bidNowBtn").addClass("disbale-btn");

        SendAuctionWinnerDetails();
    }
}


function CreateHistoryData(data) {
    data.bidAmount = data.bidAmount.toFixed(2);
    var result = Mustache.render($("#auctionHistoryTemplate").html(), data)
    $("#itemHistories").append(result);
}


