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
                $(".name").text(data.itemName);
                $("#lastBidAmount").text("$" + data.lastBidAmount.toFixed(2));
                $("#fmvValue").text("$" + data.fairMarketValue_FMV.toFixed(2));
                $("#totalBids").text(data.totalBidCount);
                $("#itemDescription").text(data.itemDescription);
                var imageFullPath = ApiServerPath + data.imageName;
                $("#itemImage").attr("src", imageFullPath);
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText + " " + xhr.status)
        }
    });
}