//Global Varibles
var AllreadySelectColor = 'wheat';
var selectColor = 'orange';
var unSelectColor = 'white';
var unSelectforDelete = 'red';
var bFirstTime = false;

// Code Region is used by Checkbox Column   
function SelectAllCheckboxes(ChkAll) {
    SelectAllCheckboxes(ChkAll, false)
}
function SelectAllCheckboxes(ChkAll, bCallForNew) {
    //var theBox= (ChkAll.type=="checkbox") ?ChkAll: ChkAll.children.item[0];
    xState = ChkAll.checked;
    var ObjGrid1 = $('#ChkAll.id');
    var ObjGrid = ChkAll.parentNode.parentNode.parentNode.parentNode;
    for (i = 0; i < ObjGrid.rows.length; i++) {
        var CkBox = ObjGrid.rows[i].children[0].firstChild.childNodes[0];
        if (CkBox.type == "checkbox" && CkBox.id != ChkAll.id) {
            if (CkBox.checked != xState)
                CkBox.click();
            if (bCallForNew == 'N') {
                CkBox.parentNode.parentNode.parentNode.children[1].children[0].value = "N"; //When User click on new 
                CkBox.parentNode.parentNode.parentNode.style.backgroundColor = unSelectColor;
            }
        }

    }
}
//Call when a check box is select
//change status of the record

function SelectCheckbox(checkBox, bCallForNew) {
    var txtStatus;
    if (checkBox.checked) {
        if (checkBox.parentNode.parentNode.parentNode.children[1] != null) {
            txtStatus = checkBox.parentNode.parentNode.parentNode.children[1].children[0];
            if (txtStatus.type == "text") {
                var ObjStatus = checkBox.parentNode.parentNode.parentNode.children[1].children[0].value
                if (ObjStatus == "" || ObjStatus == "N") {
                    checkBox.parentNode.parentNode.parentNode.children[1].children[0].value = "N"; //Mark as new Record
                    checkBox.parentNode.parentNode.parentNode.style.backgroundColor = selectColor;
                }
                else if (ObjStatus == "D") {
                    checkBox.parentNode.parentNode.parentNode.children[1].children[0].value = 'E';//Mark as Existing Record
                    checkBox.parentNode.parentNode.parentNode.style.backgroundColor = AllreadySelectColor;
                }
            }
            else {
                if (confirm_delete() == true) {
                    checkBox.parentNode.parentNode.parentNode.style.backgroundColor = selectColor;
                }
                else {
                    checkBox.checked = false;
                }
            }
        }
    }
    else {
        txtStatus = checkBox.parentNode.parentNode.parentNode.children[1].children[0];
        if (txtStatus.type == "text") {
            if (checkBox.parentNode.parentNode.parentNode.children[1].children[0].value == "E") {
                checkBox.parentNode.parentNode.parentNode.children[1].children[0].value = "D";//Mark for delete   
                checkBox.parentNode.parentNode.parentNode.style.backgroundColor = unSelectforDelete;
            }
            else if (checkBox.parentNode.parentNode.parentNode.children[1].children[0].value == "N") {
                checkBox.parentNode.parentNode.parentNode.children[1].children[0].value = ""; //Mark to remain original state      
                checkBox.parentNode.parentNode.parentNode.style.backgroundColor = unSelectColor;
            }
        }
        else {
            checkBox.parentNode.parentNode.parentNode.style.backgroundColor = unSelectColor;
        }

    }
}

function ChangeBackGroundColor(checkBox) {
    //      //changing row style accordingly
    if (checkBox.checked)
        checkBox.parentNode.parentNode.parentNode.style.backgroundColor = selectColor;
    else
        checkBox.parentNode.parentNode.parentNode.style.backgroundColor = unSelectColor;
    return true;
}


// function will call when form is load.    
function FirstTimeGridDisplay(containerName) {
    //ContentPlaceHolder1_     
    if (containerName == null) {
        containerName = 'ContentPlaceHolder1_';
    }
    SetContainerName(containerName);
}
function onGridViewRowSelected(ObjSelectedImage) {

    if (PcontainerName == null) {
        PcontainerName = '';
    }
    if (PcontainerName == null || PcontainerName == "") PcontainerName = "ContentPlaceHolder1_";
    //var gridView = document.getElementById(PcontainerName +'SearchGrid_SelectionGrid');
    var ObjGrid1 = $('#' + PcontainerName + 'SearchGrid_SelectionGrid');
    if (ObjSelectedImage == null) {
        if (ObjGrid1 == null) return true;
        ObjSelectedImage = ObjGrid1[0].rows[1].cells[0].children[0];
    }
    //Set Value from grid to form controls
    var i, j;
    var controlid;
    var ObjControl;
    var objFormType
    //ObjControl = document.getElementById(PcontainerName + 'txtControlCount')
    ObjControl = $('#' + PcontainerName + 'txtControlCount')
    //objFormType = document.getElementById(PcontainerName + 'txtFormTypeID')
    objFormType = $('#' + PcontainerName + 'txtFormTypeID')

    if (ObjControl == null) {
        return;
    }
    var colcount = (ObjControl[0].value == "") ? "0" : ObjControl[0].value;


    var GridCell;
    var iGridCellNo;
    var bSetEnable = true;

    for (i = 0; i <= eval(colcount) ; i++) {
        if (i == 0) {
            controlid = PcontainerName + 'txtID';
            ObjControl = $('#' + controlid)
            iGridCellNo = i + 1;
        }
        else {
            iGridCellNo = i + 2;
            controlid = PcontainerName + (i);
            ObjControl = ($('#' + controlid) == null) ? $('#' + controlid + '_txtDocDate') : $('#' + controlid);
        }

        if (ObjControl != null) {
            GridCell = ObjSelectedImage.parentNode.parentNode.cells[iGridCellNo]; //ObjSelectedImage.parentNode.parentNode.childNodes[iGridCellNo];                
            if (GridCell != null) {

                if (ObjControl[0].type == "text") {
                    //ObjControl[0].value = (GridCell.innerText == "") ? ObjSelectedImage.parentNode.parentNode.childNodes[iGridCellNo].childNodes[0].childNodes[0].checked : GridCell.innerText;
                    ObjControl[0].value = GridCell.innerHTML.replace("&nbsp;", "");
                }
                else if (ObjControl[0].type == "select-one") {
                    //document.getElementById(controlid).options[index].value=ObjSelectedImage.parentNode.parentNode.childNodes[i].innerText;
                    //document.getElementById(controlid)[0].innerText=ObjSelectedImage.parentNode.parentNode.childNodes[i].innerText;
                    ObjControl[0].selectedIndex = 0;
                    // var Optioncount = ObjControl.length;
                    var Optioncount = ObjControl[0].length;
                    for (j = 0; j < Optioncount; j++) {
                        if (GridCell.innerText.trim() == "Y") {
                            ObjControl[0].selectedIndex = 1;
                        }
                        else if (GridCell.innerText.trim() == "N") {
                            ObjControl[0].selectedIndex = 2;
                        }

                        else if (ObjControl[0][j] != null) {
                            if (ObjControl[0][j].innerText.trim() == GridCell.innerText.trim()) {
                                // ObjControl[0].value = ObjControl[0][j].value;
                                ObjControl[0].selectedIndex = ObjControl[0][j].value;

                            }

                        }
                    }
                }
                if (iGridCellNo != 1 && ObjControl.disabled == true)
                    ObjControl[0].disabled = true;
                if (bSetEnable == false) {
                    ObjControl[0].disabled = true;
                }
                //Megha 31052012 
                if (bSetEnable == true) {
                    ObjControl[0].disabled = false;
                }
                //Megha 31052012 
                if (iGridCellNo == 1) {
                    GridCell = ObjSelectedImage.parentNode.parentNode.childNodes[2];

                    if (GridCell.innerText == "Y") {
                        if (objFormType != null) {
                            var FormType = objFormType[0].value;
                            if (FormType != "1") {
                                alert("This Record is Used,You Can Not Change The Data!.");
                                bSetEnable = false;
                            }
                        }
                    }
                        //Megha 31052012  changes for Not Used Record
                    else if (GridCell.innerText == "N") {
                        bSetEnable = true;
                    }
                    //Megha 31052012 

                }
            }
        }
        else {
            controlid = PcontainerName + i + '_ctl01_ChkAll';
            ObjControl = $('#' + controlid)
            if (ObjControl != null) {
                ObjControl[0].checked = false;
                SelectAllCheckboxes(ObjControl, 'N');
            }
        }
    }
    return true;
}
function SetStatusOfDealer(ObjtxtBox, RowNo, ColNO) {

    if (ObjtxtBox.parentNode.parentNode.parentNode.children[RowNo].children[ColNO].childNodes[0].value == "E")//If Exist Mark is Exist
    {
        ObjtxtBox.parentNode.parentNode.parentNode.children[RowNo].children[ColNO].childNodes[0].value = "N";
    }
    else if (ObjtxtBox.parentNode.parentNode.parentNode.children[RowNo].children[ColNO].childNodes[0].value == "N")//If Exist Mark is New
    {
        ObjtxtBox.parentNode.parentNode.parentNode.children[RowNo].children[ColNO].childNodes[0].value = "N";
    }

}
//check before Delete
function confirm_delete() {
    if (confirm("Are you sure you want to delete?") == true)
        return true;
    else
        return false;
}


// To Search Text in Grid
function SearchTextInGrid(sGridName) {
    //ContentPlaceHolder1_
    var gridView = null;
    var ObjTextControl = null;
    var ObjSelctionCriteria = null;


    gridView = document.getElementById(sGridName);
    ObjTextControl = document.getElementById('txtSearch');
    ObjSelctionCriteria = document.getElementById('DdlSelctionCriteria');

    var sSearchValue = ObjTextControl.value;
    if (sSearchValue == "") return;

    if (gridView == null) return;
    if (ObjTextControl == null) return;
    if (ObjSelctionCriteria == null) return;


    var iGridColIndex = 2;

    var iSelectionCritera = ObjSelctionCriteria.selectedIndex;

    if (iSelectionCritera == 0) {
        iGridColIndex = 2
    }
    else if (iSelectionCritera == 1) {
        iGridColIndex = 3
    }

    var i;
    var rows = gridView.rows;
    var gridCell;
    var sCellValue = "";
    var bFind = false;

    sSearchValue = sSearchValue.toUpperCase();
    for (i = 1; i < rows.length; i++) {
        gridCell = gridView.children[0].rows[i].cells[iGridColIndex];
        sCellValue = gridCell.children[0].innerText.toUpperCase();

        if (sCellValue.search(sSearchValue) == -1) { // not find
            gridView.children[0].rows[i].style.backgroundColor = 'white';

        }
        else {
            // if find
            gridView.children[0].rows[i].style.backgroundColor = '#D6EEFB';
            gridView.children[0].rows[i].focus();
            bFind = true;
        }
    }
    if (bFind == false) {
        alert("Record Not Found !");
    }
    return false;
}

//
function SetDateFields(SearchGrid) {
    var SearchList = (document.getElementById("ContentPlaceHolder1_" + SearchGrid + "_DdlSelctionCriteria") != null) ? document.getElementById("ContentPlaceHolder1_" + SearchGrid + "_DdlSelctionCriteria") : document.getElementById(SearchGrid + "_DdlSelctionCriteria");
    var DivSearchDiv = (document.getElementById("ContentPlaceHolder1_" + SearchGrid + "_divDateSearch") != null) ? document.getElementById("ContentPlaceHolder1_" + SearchGrid + "_divDateSearch") : document.getElementById(SearchGrid + "_divDateSearch");
    var txtSearch = (document.getElementById("ContentPlaceHolder1_" + SearchGrid + "_txtSearch") != null) ? document.getElementById("ContentPlaceHolder1_" + SearchGrid + "_txtSearch") : document.getElementById(SearchGrid + "_txtSearch");
    var selectedStatus = SearchList.options[SearchList.selectedIndex].value;
    var txtUseDate = (document.getElementById("ContentPlaceHolder1_" + SearchGrid + "_txtUseDate") != null) ? document.getElementById("ContentPlaceHolder1_" + SearchGrid + "_txtUseDate") : document.getElementById(SearchGrid + "_txtUseDate");

    if (selectedStatus.indexOf("Date") > 0) {
        DivSearchDiv.style.display = '';
        txtSearch.style.display = "none";
        txtUseDate.value = "Yes";
    }
    else {
        DivSearchDiv.style.display = "none";
        txtSearch.style.display = '';
        txtUseDate.value = "No";
    }
}

// Scrollable Gridview with fixed headers Vikram K on 01/02/2018
// gridId -- GridView ID
// DivHeaderRow -- New div for Header Creation
// DivMainContent -- Main Gridview
// height,width -- height of div
// headerHeight -- new created header div height
// isFooter -- if wants to footer then  set to true in CS and in Grid set to Showfooter= "true"
function MakeStaticHeader(gridId, DivHeaderRow, DivMainContent, height, width, headerHeight, isFooter) {
    //debugger;
    var tbl = document.getElementById(gridId);
    if (tbl) {
        //var DivHR = document.getElementById('DivHeaderRow');
        //var DivMC = document.getElementById('DivMainContent');
        //var DivFR = document.getElementById('DivFooterRow');
        var DivHR = DivHeaderRow;
        var DivMC = DivMainContent;
        width = 100;
    

        //*** Set divheaderRow Properties ****
        DivHR.style.height = headerHeight + 'px';
        //DivHR.style.width = (parseInt(width) - 16) + 'px';
        DivHR.style.width = (parseInt(width) - 1) + '%';
        DivHR.style.position = 'relative';
        //DivHR.style.top = '0px';
        DivHR.style.top = '40px';
        DivHR.style.zIndex = '10';
        DivHR.style.verticalAlign = 'top';

        //*** Set divMainContent Properties ****
        //DivMC.style.width = width + 'px';
        DivMC.style.width = width + '%';
        DivMC.style.height = height + 'px';
        DivMC.style.position = 'relative';
        //DivMC.style.top = -headerHeight + 'px';
        DivMC.style.zIndex = '1';

        //*** Set divFooterRow Properties ****
        //DivFR.style.width = (parseInt(width) - 16) + 'px';
        //DivFR.style.width = (parseInt(width) - 1) + '%';
        //DivFR.style.position = 'relative';
        //DivFR.style.top = -headerHeight + 'px';
        //DivFR.style.verticalAlign = 'top';
        //DivFR.style.paddingtop = '2px';

        //if (isFooter) {
        //    var tblfr = tbl.cloneNode(true);
        //    tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
        //    var tblBody = document.createElement('tbody');
        //    tblfr.style.width = '100%';
        //    tblfr.cellSpacing = "0";
        //    tblfr.border = "0px";
        //    tblfr.rules = "none";
        //    //*****In the case of Footer Row *******
        //    tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
        //    tblfr.appendChild(tblBody);
        //    DivFR.appendChild(tblfr);
        //}
        //****Copy Header in divHeaderRow****
        DivHR.appendChild(tbl.cloneNode(true));
    }
}

function OnScrollDiv(Scrollablediv, DivHeaderRow) {
   
    //document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;
    DivHeaderRow.scrollLeft = Scrollablediv.scrollLeft;
    //document.getElementById('DivFooterRow').scrollLeft = Scrollablediv.scrollLeft;
}

//END 02022018