var oldRecord = 0
var radioValue = true
function ViewOfferData(offerId) {
    if (offerId != null && offerId != undefined) {
        $.ajax({
            type: "Get",
            url: "/Pricing/OfferCardData",
            data: {
                offerId: offerId
            },
            success: function (result) {
                $(".body-content").html(result);
                window.location.href = window.location.host + '/pricing/OfferCardData/' + offerId;
            }
        })
    }
}


function GetFilterData() {
    $.ajax({
        url: "/Pricing/GetFilterData",
        type: "get",
        data: {
            searchData: $("#tbxFilter").val()
        },
        success: function (result) {
            $("#form1").children().remove();
            $("#form1").append(result);
        }
    });
}
$("#AllUsersDropDown").on("change", function () {
    if ($("#AllUsersDropDown").val != 0) {
        $.ajax({
            url: "/Pricing/SetPricingPerson",
            type: "post",
            data: {
                userID: $("#AllUsersDropDown").val(),
                offerID: $("#offerIDHidden").val()
            },
            success: function (result) {
                if (result == "True") {
                    alert("User Assigned Successfully")
                }
                else {
                    alert("User not Assigned choose one more time")
                }
            }
        })
    }

})


var BomIDSelectedGlobal = 0;
function GetBomData(self, productID) {
    BomIDSelectedGlobal = $(self).val()
    $.ajax({
        url: "/Pricing/BomLibraryView",
        type: "get",
        data: {
            productID: productID,
            bomIDChoosen: BomIDSelectedGlobal
        },
        success: function (result) {
            $("#bomPopUp").children().remove();
            $("#bomPopUp").append(result)
            $("#myModal").modal('show')
        }
    })
}
$("#radioValue").on("click", function () {
    $('.percentageLabel').css('display', 'none');
    $('.valueLabel').css('display', "block");
    $("#radioPercentage").prop('checked', false);
    radioValue = true;
})
$("#radioPercentage").on("click", function () {
    $('.valueLabel').css('display', 'none');
    $('.percentageLabel').css('display', 'block');
    $("#radioValue").prop('checked', false);
    radioValue = false;
})
function getOldRecordData(self, NewOrDefaultExtraPrice) {
    var isChecked = $('#radioValue').prop('checked');
    if (isChecked || isChecked == undefined) {
        oldRecord = $(self).val()
    }
}

function SetNewTotalRecord(self, kindOfPrice) {
    var isChecked = $('#radioValue').prop('checked');
    if (isChecked || isChecked == undefined) {

        var result = SetNewTotalCostData(self, kindOfPrice);
    }
}
function OpenTermPragraph(self) {
    var dataIdValue = $(self).attr("data-id")
    var dataNameValue = $(self).attr("data-name")
    if ($(self).prop("checked") == true) {
        $(".termPragraph[data-id=" + dataIdValue + "][data-name=" + dataNameValue + "]").removeAttr("disabled")
    }
    else if ($(self).prop("checked") == false) {
        $(".termPragraph[data-id=" + dataIdValue + "][data-name=" + dataNameValue + "]").prop("disabled", true);
    }
}

function AddNewPricingRow() {
    var i = parseInt($(".rowIndex").last().text());
    if (isNaN(i)) {
        i=0
    }
    $.ajax({
        url: "/Pricing/AddNewRow",
        type: "get",
        data: {
            rowIndex: i + 1
        },
        success: function (result) {
            $("#pricingRow").append(result)
        }
    })
}
function setBomLibraryForProduct(self) {
    var bomID = $(self).attr("id");
    $("#myModal").modal('hide')
    var oldOne = $("#" + BomIDSelectedGlobal + ".bomSelectedLabel").val()

    $("#" + BomIDSelectedGlobal + ".bomSelectedLabel").text(bomID)
    $("#" + BomIDSelectedGlobal + ".bomSelectedLabel").val(bomID)

    BomIDSelectedGlobal = 0;
}
function SetNewTotalCostData(self, kindOfPrice) {
    var newValue = $(self).val()
    if (oldRecord != newValue) {
        if (kindOfPrice == 'Quantity') {
            var value = parseFloat($("#totalQuantity").val()) - oldRecord + parseFloat(newValue)
            $("#totalQuantity").val(value)
        }
        else if (kindOfPrice == 'Bom') {
            var valueBom = parseFloat($("#totalPrice").val()) - oldRecord + parseFloat(newValue)
            var valueFinal = parseFloat($("#finalTotal").val()) - oldRecord + parseFloat(newValue)

            $("#totalPrice").val(valueBom)
            $("#finalTotal").val(valueFinal)

        }
        else if (kindOfPrice == 'ExtraCost') {
            var valueExtraCost = parseFloat($("#newTotalPrice").val()) - oldRecord + parseFloat(newValue)
            var valueFinal = parseFloat($("#finalTotal").val()) - oldRecord + parseFloat(newValue)
            $("#newTotalPrice").val(valueExtraCost)
            $("#finalTotal").val(valueFinal)

        }
        else if (kindOfPrice == 'FinalTotal') {
            var valueFinal = parseFloat($("#finalTotal").val()) - oldRecord + parseFloat(newValue)
            $("#finalTotal").val(valueFinal)
        }
        oldRecord = 0;
        return true;
    }
}
function SearchInBomLibraries() {
    var searchText = $("#searchBomTextField").val()
    var productID = $("#ProductIDForBomSearch").val();
    $.ajax({
        url: "/Pricing/BomLibraryView",
        type: "get",
        data: {
            productID: productID,
            bomIDChoosen: BomIDSelectedGlobal,
            searchBomText: searchText
        },
        success: function (res) {
            $("#bomPopUp").children().remove();
            $("#bomPopUp").append(res);
            $("#myModal").modal('show')
            $("#searchBomTextField").val(searchText)
        }
    })
}
function CollectAddedDataBeforeSubmit() {
    var newPricingProductList = []
    var pricingID = $("#pricingID").val()
    $(".addedProducts").each(function (i, obj) {
        var newPricingProduct = {
            ProductGroupID: obj.cells[1].childNodes[1].value,
            ProductID: obj.cells[2].childNodes[1].value,
            Description: obj.cells[3].childNodes[1].value,
            Quantity: obj.cells[4].childNodes[1].value,
            Attachment: obj.cells[5].childNodes[1].value,
            BomLibraryID: obj.cells[6].childNodes[1].value,
            Price: obj.cells[7].childNodes[1].value,
            PriceAdded: obj.cells[8].childNodes[1].value,
            Comments: obj.cells[9].childNodes[1].value,
        }
        newPricingProductList.push(newPricingProduct);
    })
    $.ajax({
        url: "/Pricing/AddNewPricingProducts",
        type: "post",
        data: {
            newPricingProduct: newPricingProductList,
            pricingID: pricingID,
            isValue: radioValue
        }
    })
}
function ReturnPricingToUnderPricing(pricingID) {
    $.ajax({
        url: "/Pricing/ReturnToUnderPricing",
        type: "get",
        data: {
            pricingID: pricingID
        }
    })
}