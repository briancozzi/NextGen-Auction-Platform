$(function () {
    //GetTemplate();
    GetCategories();
    GetAuctionItems();
    //get auction item by id
    $(document).on("click", ".golf-event .image, .golf-event .desc", function () {
        var isClosedAuction = $(this).parents(".golf-event").attr("data-is-closed-auction");
        var itemStatus = $(this).parents(".golf-event").attr("data-itemstatus");
        if (isClosedAuction == "true") {
            if (IsLoggedInUser == "true") {
                location.href = "/Home/ProductDetailClosedWithLogin?id=" + $(this).parents(".golf-event").attr("data-id") + "&itemStatus=" + itemStatus;
            }
            else {
                location.href = "/Home/ProductDetailClosed?id=" + $(this).parents(".golf-event").attr("data-id") + "&itemStatus=" + itemStatus;
            }
        }
        else {
            if (IsLoggedInUser == "true") {
                location.href = "/Home/ProductDetailWithLogin?id=" + $(this).parents(".golf-event").attr("data-id") + "&itemStatus=" + itemStatus;
            }
            else {
                location.href = "/Home/ProductDetail?id=" + $(this).parents(".golf-event").attr("data-id");
            }
        }
    });
});

function GetCategories() {
    $.ajax({
        url: ApiServerPath + "/api/services/app/Category/GetAllCategory",
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json",
        dataType: "json",
        success: function (response) {
            if (response != null || response != undefined) {
                var data = response.result.items;
                $.each(data, function (i, v) {
                    $("#categories").append('<li><a href="#" data-value="' + v.id + '">' + v.categoryName + '</a></li>');
                });
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText + " " + xhr.status)
        }
    });
}
function GetAuctionItems(categoryId, search) {
    if (categoryId === undefined)
        categoryId = 0;
    if (search === undefined || search === "undefined")
        search = "";
    $.ajax({
        url: ApiServerPath + "/api/services/app/AuctionItem/GetAllAuctionItems?categoryId=" + categoryId + "&search=" + search,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json",
        dataType: "json",
        success: function (response) {
            if (response != null || response != undefined) {
                var data = response.result.items;
                $("#itemCount").text('(' + data.length + ')');
                $("#auctionItems").empty();
                $.each(data, function (i, v) {
                    v.imageName = ApiServerPath + v.imageName;
                    var output = Mustache.render($("#auctionItemTemplate").html(), v);
                    $("#auctionItems").append(output);
                });
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText + " " + xhr.status)
        }
    });
}

$(document).on("click", "#categories li a", function () {
    debugger;
    var items = $(this).parents("ul").children("li");
    $.each(items, function (i, v) {
        $(this).find("a").removeClass("active");
    })
    var selectedVal = $(this).attr("data-value");
    $(this).addClass("active");
    var selectedText = $(this).text();
    $("#selectedCategory").text(selectedText);
    GetAuctionItems(selectedVal);
});

$(document).on("keyup", "#txtSearch", function () {
    var searchTxt = $(this).val();
    var categoryId = $("#categories li a.active").attr("data-value");
    setTimeout(GetAuctionItems(categoryId, searchTxt), 3000);

});
//function GetTemplate() {
//    var url = WebSiteUrl + "Home/GetAuctionItemTemplate";
//    $.ajax({
//        url: url,
//        type: "GET",
//        //async: true,
//        cache: false,
//        dataType: "html",
//        success: function (response) {
//            if (response != null || response != undefined) {
//                $(response).appendTo("#pageTemplates");
//            }
//        },
//        error: function (xhr) {
//            console.log(xhr.responseText + " " + xhr.status)
//        }
//    });
//}