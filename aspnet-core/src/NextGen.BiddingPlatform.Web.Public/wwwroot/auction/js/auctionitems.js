$(function () {
    //GetTemplate();
    GetCategories();
    GetAuctionItems();
    //get auction item by id
    //.golf-event.image,
    $(document).on("click", ".golf-event .desc, #viewItem", function () {
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

    $(document).on("click", "#viewItem", function () {
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


    $(document).on("click", "#setFavoriteItem", function (e) {

        if (UserId === "") {
            alert("Please login first!!");
            return false;
        }

        var itemId = $(this).parents(".golf-event").attr("data-id");
        var isFavorite = $(this).hasClass("liked") ? false : true;
        var currEle = this;
        var input = {
            userId: UserId,
            itemId: itemId,
            isFavorite: isFavorite,
            tenantId: tenantId
        };
        $.ajax({
            url: ApiServerPath + "/api/services/app/UserFavoriteItem/SetItemAsFavoriteOrUnFavorite",
            type: "POST",
            cache: false,
            async: true,
            data: JSON.stringify(input),
            contentType: "application/json",
            dataType: "json",
            success: function (response) {
                if (response != null && response.success) {
                    if (isFavorite) {
                        $(currEle).addClass("liked");
                    }
                    else {
                        $(currEle).removeClass("liked");
                    }
                }
                else {
                    alert("error occured!!");
                }
            },
            error: function (xhr) {
                console.log(xhr.responseText + " " + xhr.status)
            }
        });
    })

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

                if (UserId !== "") {
                    $.ajax({
                        url: ApiServerPath + "/api/services/app/UserFavoriteItem/GetUserFavoriteItems?userId=" + UserId + "&tenantId=" + tenantId,
                        type: "GET",
                        cache: false,
                        async: true,
                        contentType: "application/json",
                        dataType: "json",
                        success: function (responseFromFavorite) {

                            if (responseFromFavorite !== null) {
                                var favItems = responseFromFavorite.result;
                                for (var i = 0; i < favItems.length; i++) {
                                    var result = data.filter(s => s.actualItemId === favItems[i].itemId)[0];
                                    if (result !== null) {
                                        result.isFavorite = "liked";
                                    }
                                }
                                var totalItems = 0;

                                $("#auctionItems").empty();
                                $.each(data, function (i, v) {
                                    if (!v.isAuctionExpired) {
                                        debugger;
                                        var output = Mustache.render($("#auctionItemTemplate").html(), v);
                                        $("#auctionItems").append(output);
                                        totalItems += 1;
                                    }
                                });
                                $("#itemCount").text('(' + totalItems + ')');
                            }

                        },
                        error: function (xhr) {
                            alert("Error occured for favorite items!!");
                        }
                    });
                }

                var totalItems = 0;

                $("#auctionItems").empty();
                $.each(data, function (i, v) {
                    if (!v.isAuctionExpired) {
                        if (v.imageName != null) {
                            v.imageName = ApiServerPath + v.imageName;
                        }
                        else {
                            v.imageName = WebSiteUrl + "/auction/images/no-img.png";
                        }
                        var output = Mustache.render($("#auctionItemTemplate").html(), v);
                        $("#auctionItems").append(output);
                        totalItems += 1;
                    }
                });
                $("#itemCount").text('(' + totalItems + ')');
            }
        },
        error: function (xhr) {
            console.log(xhr.responseText + " " + xhr.status)
        }
    });
}

$(document).on("click", "#categories li a", function () {

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