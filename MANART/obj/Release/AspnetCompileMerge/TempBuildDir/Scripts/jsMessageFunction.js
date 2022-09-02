var ALERT_TITLE = "Oops!";
var ALERT_BUTTON_TEXT = "Ok";

function loadXML(xmlFile) {
    var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
    //'XmlFiles/Test.xml'

    xmlDoc.async = "false";
    //xmlDoc.onreadystatechange=verify; 					
    xmlDoc.load(xmlFile);
    xmlObj = xmlDoc.documentElement;
    ShowMessageFromXml(xmlObj);
}
function ShowMessageFromXml(tree) {
    var intNode, i;
    var intCode, strValue;
    var MessageID = document.getElementById('txtMSGID').value
    if (tree.hasChildNodes()) {
        var nodes = tree.childNodes(1).childNodes.length;
        for (i = 0; i < 2; i++) {
            if (MessageID == tree.childNodes(i).attributes[0].value) {
                strValue = tree.childNodes(0).childNodes(0).text;
                alert(strValue);
                return;
                //strValue is the name of employee and intValue is the employee code i.e Emp_Code		
            }
        }
    }
}
function ShowMessage(MessageId, sDocName, sAddionalMsg) {
    if (MessageId == 1) {
        alert("Please Enter Lowercase value.");
    }
    else if (MessageId == 2) {
        alert("Please Enter Lowercase value.");
    }
    else if (MessageId == 3) {
        alert("Reach to maxlength ");
    }
    else if (MessageId == 4) {
        if (sAddionalMsg != null) {
            alert("" + sDocName + " " + sAddionalMsg + " Saved.");
        }
        else {
            alert("Record Saved.");
        }
    }
    else if (MessageId == 5) {
        if (sAddionalMsg != null) {
            alert("" + sDocName + "" + sAddionalMsg + " Error in Save.");
        }
        else {
            alert("Error in Save.");
        }
    }
    else if (MessageId == 6) {
        alert('Record Does Not Exist.');
    }
    else if (MessageId == 7) {
        if (sAddionalMsg != null) {
            alert("" + sDocName + "" + sAddionalMsg + " Confirmed.");
        }
        else {
            alert("Record Confirmed.");
        }
    }
    else if (MessageId == 8) {
        if (sAddionalMsg != null) {
            alert("" + sDocName + "" + sAddionalMsg + " Canceled.");
        }
        else {
            alert("Record Canceled.");
        }
    }
    else if (MessageId == 9) {
        alert('Record Submitted.');
    }
    else if (MessageId == 10) {
        alert('Record Not Submitted.');
    }
    else if (MessageId == 11) {
        alert('Mail Sent Successfully!');
    }
    else if (MessageId == 12) {
        alert('Mail Sending failed!');
    }
    else if (MessageId == 13) {
        alert('Record Confirmed For Postshipment!');
    }
    else if (MessageId == 14) {
        if (sAddionalMsg != null) {
            alert("" + sDocName + "" + sAddionalMsg + " Created.");
        }
    }
    else if (MessageId == 15) {
        if (sAddionalMsg != null) {
            alert("" + sDocName + "" + sAddionalMsg + " Not Created, Please Contact to Administrator.");
        }
        else {
            alert("PO Not Created, Please Contact to Administrator.");
        }
    }
    else if (MessageId == 16) {
        if (sAddionalMsg != null) {
            alert("" + sDocName + "" + sAddionalMsg + "Shortclosed.");
        }
        else {
            alert("PO Shortclosed.");
        }
    }

}


function createCustomAlert(txt) {
    // shortcut reference to the document object
    d = document;

    // if the modalContainer object already exists in the DOM, bail out.
    if (d.getElementById("modalContainer")) return;

    // create the modalContainer div as a child of the BODY element
    mObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
    mObj.id = "modalContainer";
    // make sure its as tall as it needs to be to overlay all the content on the page
    mObj.style.height = document.documentElement.scrollHeight + "px";

    // create the DIV that will be the alert 
    alertObj = mObj.appendChild(d.createElement("div"));
    alertObj.id = "alertBox";
    // MSIE doesnt treat position:fixed correctly, so this compensates for positioning the alert
    if (d.all && !window.opera) alertObj.style.top = document.documentElement.scrollTop + "px";
    // center the alert box
    alertObj.style.left = (d.documentElement.scrollWidth - alertObj.offsetWidth) / 2 + "px";

    // create an H1 element as the title bar
    h1 = alertObj.appendChild(d.createElement("h1"));
    h1.appendChild(d.createTextNode(ALERT_TITLE));

    // create a paragraph element to contain the txt argument
    msg = alertObj.appendChild(d.createElement("p"));
    msg.innerHTML = txt;

    // create an anchor element to use as the confirmation button.
    btn = alertObj.appendChild(d.createElement("a"));
    btn.id = "closeBtn";
    btn.appendChild(d.createTextNode(ALERT_BUTTON_TEXT));
    btn.href = "#";
    // set up the onclick event to remove the alert when the anchor is clicked
    btn.onclick = function () { removeCustomAlert(); return false; }


}

// removes the custom alert from the DOM
function removeCustomAlert() {
    document.getElementsByTagName("body")[0].removeChild(document.getElementById("modalContainer"));
}
