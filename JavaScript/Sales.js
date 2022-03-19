/*
 *Sales Home JavaScript functions 
 */
function btnTaskCalendarClicked() {
    alert("Calendar View");
}

// Toggle between showing and hiding the sidebar, and add overlay effect
function nav_toggle() {
    // Get the Sidebar
    var mySidebar = document.getElementById("mySidebar");

    // Get the DIV with overlay effect
    var overlayBg = document.getElementById("myOverlay");

    if (mySidebar.style.display === null || mySidebar.style.display === '' ||mySidebar.style.display === 'block') {
        mySidebar.style.display = 'none';
        overlayBg.style.display = "none";
    } else {
        mySidebar.style.display = 'block';
        overlayBg.style.display = "block";
    }
}

// Close the sidebar with the close button
function nav_close() {
    // Get the Sidebar
    var mySidebar = document.getElementById("mySidebar");

    // Get the DIV with overlay effect
    var overlayBg = document.getElementById("myOverlay");
    mySidebar.style.display = "none";
    overlayBg.style.display = "none";
}

function btnNotify_Clicked(btnID) {
    document.getElementById(btnID).click();
}

/*
 *create target page 
 */

function changeFixed(totalTbxID,fixedClass,percentClass) {
    var fixedTbxs = document.getElementsByClassName(fixedClass);
    var percentTbxs = document.getElementsByClassName(percentClass);
    var totalTbx = document.getElementById(totalTbxID);
    for (var i = 0; i < fixedTbxs.length; i++) {
        try {
            if (totalTbx.value != null && totalTbx.value != "") {
                if (percentTbxs[i].value == null || percentTbxs[i].value == '') {
                    percentTbxs[i].value = 100.0 / fixedTbxs.length;
                }
                fixedTbxs[i].value = parseFloat(totalTbx.value) * parseFloat(percentTbxs[i].value) / 100.0;
            }
            else {
                percentTbxs[i].value = "";
                fixedTbxs[i].value = "";
            }
        } catch (ex) {
            console.log(ex);
        }
    }
}
function changeNextTbxs(totalTbxID, myType, myIndx, fixedType, percentType) {
    //var myIndx = parseInt(strMyIndx);
    var fixedTbxs = document.getElementsByClassName(fixedType);
    var percentTbxs = document.getElementsByClassName(percentType);
    var totalTbx = document.getElementById(totalTbxID);
    if ((myType == percentType && percentTbxs[myIndx].value != null && percentTbxs[myIndx].value != "")
        || (myType == fixedType && fixedTbxs[myIndx].value != null && fixedTbxs[myIndx].value != "")) {
        if (myType == percentType) {
            fixedTbxs[myIndx].value = parseFloat(totalTbx.value) * parseFloat(percentTbxs[myIndx].value) / 100.0;
        }
        else {
            percentTbxs[myIndx].value = parseFloat(fixedTbxs[myIndx].value) * 1.0 / parseFloat(totalTbx.value) * 100;
        }
        var totalPercent = 0;
        var totalFixed = 0;
        for (var i = 0; i <= myIndx; i++) {
            totalPercent += parseFloat(percentTbxs[i].value);
            totalFixed += parseFloat(fixedTbxs[i].value);
            //if (totalPercent > 100) {
            //    alert("Invalid Percentages.");
            //    return false;
            //}
            //if (totalFixed > parseFloat(totalTbx.value)) {
            //    alert("Invalid Fixed Values.");
            //    return false;
            //}
        }
        for (var i = myIndx + 1; i < fixedTbxs.length; i++) {
            try {
                percentTbxs[i].value = (100.0 - totalPercent) / (fixedTbxs.length - i);
                totalPercent += parseFloat(percentTbxs[i].value);
                fixedTbxs[i].value = parseFloat(totalTbx.value) * parseFloat(percentTbxs[i].value) / 100.0;
                totalFixed += parseFloat(fixedTbxs[i].value);
                //if (totalPercent > 100) {
                //    alert("Invalid Percentages.");
                //    return false;
                //}
                //if (totalFixed > parseFloat(totalTbx.value)) {
                //    alert("Invalid Fixed Values.");
                //    return false;
                //}
            } catch (ex) {
                console.log(ex);
            }
        }
        //if (totalPercent != 100) {
        //    alert("Sum of Percentages are less than 100%.");
        //    return false;
        //}
        //if (totalFixed != parseFloat(totalTbx.value)) {
        //    alert("Sum of Fixed Values are less than total value.");
        //    return false;
        //}
    }
    else {
        for (var i = 0; i < fixedTbxs.length; i++) {
            percentTbxs[i].value = "";
            fixedTbxs[i].value = "";
        }
    }
    return true;
}