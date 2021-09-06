$(function () {
    GetAuctionItem();
});
function GetAuctionItem() {
    var id = $("#auctionItemId").val();
    $.ajax({
        url: ApiServerPath + "/api/services/app/AuctionItem/GetAuctionItem?Id=" + id,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json",
        dataType: "json",
        success: function (response) {
            if (response != null || response != undefined) {
                var data = response.result;
                var endDate = new Date(Date.parse(data.auctionEndDateTime));

                //if (parseInt(data.remainingDays) > 0) {
                //    $('#defaultCountdown').countdown({
                //        until: endDate,
                //        layout: ' {dn} {dl}  '
                //    });
                //}
                //else {
                $('#defaultCountdown').countdown({
                    until: endDate,
                    layout: ' {hn} {hl} {mn} {ml} {sn} {sl} '
                });
                //}



                $(".name").text(data.itemName);
                $("#lastBidAmount").text("$" + data.lastBidAmount.toFixed(2));
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
                $("#itemNo").text(data.itemNumber);
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText + " " + xhr.status)
        }
    });
}