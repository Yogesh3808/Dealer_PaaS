//################# File Upload DownLoad Working/////////////
function NotDoAnything() {
    return true;
}
function addFileUploadBox(objControl) {
    // The new box needs a name and an ID
    //debugger;
    var Objtxt = objControl.value;
    if (Objtxt == "") return;
    objControl.onblur = null; //function() { NotDoAnything() };
    if (!addFileUploadBox.lastAssignedId)

        addFileUploadBox.lastAssignedId = 2;

    if (!document.getElementById || !document.createElement)

        return false;
    var uploadArea = document.getElementById("upload1");
    if (!uploadArea)
        return;
    var container = document.createElement("div");
    container.setAttribute("id", "Div" + addFileUploadBox.lastAssignedId);

    var newTextBox = document.createElement("input");
    var newUploadBox = document.createElement("input");
    var newDelBtn = document.createElement("img");

    // Set the Properties of the Controls



    newTextBox.setAttribute("id", "Text" + addFileUploadBox.lastAssignedId);
    newTextBox.setAttribute("name", "Text" + addFileUploadBox.lastAssignedId);
    newTextBox.setAttribute("Class", "TextBoxForString");
    //newTextBox.style.marginLeft = "8px";
    newTextBox.type = "text";
    newTextBox.style.width = "50%";


    newUploadBox.type = "file";
    newUploadBox.style.width = "45%";
    newUploadBox.setAttribute("id", "File" + addFileUploadBox.lastAssignedId);
    newUploadBox.setAttribute("name", "File" + addFileUploadBox.lastAssignedId);
    newUploadBox.setAttribute("Class", "TextBoxForString");
    newUploadBox.onblur = function() { addFileUploadBox(this) };

    newDelBtn.setAttribute("id", "img" + addFileUploadBox.lastAssignedId);
    newDelBtn.src = "../Images/cal_close.gif";


    newDelBtn.style.marginLeft = "4px";
    newDelBtn.onclick = function() { RemoveUploadBox(this.id) };


    // Add Control To Container
    container.appendChild(newTextBox);
    container.appendChild(newUploadBox);
    container.appendChild(newDelBtn);
    uploadArea.appendChild(container);

    addFileUploadBox.lastAssignedId++;
}
// To Remove uploaded Box
function RemoveUploadBox(id) {

    var str;

    str = id.substring(3);
    var Container = document.getElementById("Div" + str);

    Container.removeChild(document.getElementById(id));
    Container.removeChild(document.getElementById("Text" + str));
    Container.removeChild(document.getElementById("File" + str));

    var uploadArea = document.getElementById("upload1");

    uploadArea.removeChild(Container);

}
//To Show attach Document
function ShowAttachDocument(objFileControl) {
    var objRow = objFileControl.parentNode.parentNode.childNodes;
    var sFileName = '';
    var sUserType = '';
    popht = 3; // popup height
    popwth = 3; // popup width
    var sDealerCode = document.getElementById('ContentPlaceHolder1_txtDealerCode').value;
    var txtUserType = document.getElementById('ContentPlaceHolder1_txtUserType');
    if (txtUserType != null) {
        sUserType = txtUserType.value;
    }  
    sFileName = (objRow[3].children[0].innerText);
    var scrleft = (screen.width / 2) - (popwth / 2) - 80; //centres horizontal
    var scrtop = ((screen.height / 2) - (popht / 2)) - 40; //centres vertical
    //window.open("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId ,"List", "scrollbars=no,resizable=no,width=1500,height=100");
    window.open("../Warranty/frmOpenAttachDocument.aspx?FileName=" + sFileName + "&DealerCode=" + sDealerCode + "&UserType=" + sUserType, "List", "top=" + scrtop + ",left=" + scrleft + ",width=1px,height=3px");
}
//Sujata 12012011
//To Show attach Document on Processing
function ShowAttachDocumentProcessing(objFileControl) {
    var objRow = objFileControl.parentNode.parentNode.childNodes;
    var sFileName = '';
    var sUserType = '';
    popht = 3; // popup height
    popwth = 3; // popup width
    var sDealerCode = document.getElementById('txtDealerCode').value;
    var txtUserType = document.getElementById('txtUserType');
    if (txtUserType != null) {
        sUserType = txtUserType.value;
    }  
    sFileName = (objRow[3].children[0].innerText);
    var scrleft = (screen.width / 2) - (popwth / 2) - 80; //centres horizontal
    var scrtop = ((screen.height / 2) - (popht / 2)) - 40; //centres vertical
    //window.open("/AUTODMS/Forms/Common/frmSelectModel.aspx?DealerID=" + sDealerId ,"List", "scrollbars=no,resizable=no,width=1500,height=100");
    window.open("/AUTODMS/Forms/Warranty/frmOpenAttachDocument.aspx?FileName=" + escape(sFileName) + "&DealerCode=" + sDealerCode + "&UserType=" + sUserType, "List", "top=" + scrtop + ",left=" + scrleft + ",width=1px,height=3px");
}
//Sujata 12012011