<%@ Page Title="EGP-Dealer Master" Language="C#" MasterPageFile="~/Header.Master"
    Theme="SkinFile" EnableViewState="true" AutoEventWireup="true" CodeBehind="frmEGPDealer.aspx.cs" Inherits="MANART.Forms.EGP1.frmEGPDealer" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsProformaFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>

    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
                // event.returnValue = false;
                //alert(message);
                // return false;
            }
        }
        window.onload
        {
            AtPageLoad();
        }
        function AtPageLoad() {
            FirstTimeGridDisplay('ContentPlaceHolder1_');
            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }

        //        function refresh() {
        //            if (event.keyCode == 116 || event.keyCode == 8) {
        //                event.keyCode = 0;
        //                event.returnValue = false
        //                return false;
        //            }
        //        }

        function refresh() {
            if (116 == event.keyCode) {
                event.keyCode = 0;
                event.returnValue = false
                return false;
            }
        }
        document.onkeydown = function () {
            refresh();
        }

    </script>

    <script type="text/javascript">


        function SHMulSelmail123() {
            var textBoxMain = window.document.getElementById('ContentPlaceHolder1_' + 'txtDealerName');
            var divisionMain = window.document.getElementById('ContentPlaceHolder1_' + 'divMain');

            var displayStatus = divisionMain.style.display;
            if (displayStatus == 'block') {
                divisionMain.style.display = 'none';
                if (window.document.getElementById('ContentPlaceHolder1_' + 'hapb').value == 'True') {
                    document.getElementById('ContentPlaceHolder1_' + '__EVENTTARGET1').value = 'MultiSelectDropDown';
                    document.getElementById('ContentPlaceHolder1_' + '__EVENTARGUMENT1').value = textBoxMain.value;
                    __doPostBack('MultiSelectDropDown', window.document.getElementById(ControlClientID + '_' + 'txtDealerName').value);
                }
            }
            else {
                divisionMain.style.display = 'block';
                divisionMain.className = 'dvmain';
            }
            var evt = (window.event == null) ? e : window.event;
            evt.cancelBubble = true;
        }
        function OnComboValueChange1(ObjCombo, txtboxId) {

            var sSelecedValue = ObjCombo.options[ObjCombo.selectedIndex].text;
            //var ParentCtrlID;
            var objtxtControl;
            if (sSelecedValue == "NEW") {
                ObjCombo.style.display = 'none';
                // ParentCtrlID = ObjCombo.id.substring(0, ObjCombo.id.lastIndexOf("_"));
                objtxtControl = document.getElementById('ContentPlaceHolder1_' + 'txtProcessName');
                objtxtControl.style.display = '';
                objtxtControl.focus();
            }
            else {
                ObjCombo.style.display = '';
                //ParentCtrlID = ObjCombo.id.substring(0, ObjCombo.id.lastIndexOf("_"));
                objtxtControl = document.getElementById('ContentPlaceHolder1_' + 'txtProcessName');
                objtxtControl.style.display = 'none';
                //objtxtControl.value='';
                //        if (CheckComboValueAlreadyUsedInGrid(ObjCombo) == false)
                //            return false;

            }
            //return true;
        }
        function SCIT1123(chkbox) {
            var labelCollection = window.document.getElementsByTagName('label');
            var hSelectedItemsValueList = document.getElementById('ContentPlaceHolder1_' + 'hsiv');
            var textBoxCurrentValue = new String();
            // var textBoxCurrentValue1 = new String();
            var textBoxMain = window.document.getElementById('ContentPlaceHolder1_' + 'txtDealerName');
            // var textBoxMain1 = window.document.getElementById('ContentPlaceHolder1_' + 'txtDealerID');
            var selectedText;
            var selectedValue;

            textBoxCurrentValue = textBoxMain.value;
            // textBoxCurrentValue1 =textBoxMain1.value;
            if (chkbox.nextSibling != null) {
                selectedText = chkbox.nextSibling.innerText;

            }
            var pElement = chkbox.parentElement == null ? chkbox.parentNode : chkbox.parentElement;
            // selectedValue = pElement.attributes["alt"].value


            if (chkbox.checked) {
                textBoxCurrentValue = selectedText + ', ';
                // textBoxCurrentValue1 = selectedValue + ',';


                if (textBoxMain.value == '--Select--')
                    textBoxMain.value = textBoxCurrentValue;
                    //   textBoxMain1.value = '';
                else
                    textBoxMain.value += textBoxCurrentValue;
                // textBoxMain1.value +=textBoxCurrentValue1;

                hSelectedItemsValueList.value = hSelectedItemsValueList.value + selectedValue + ', ';
            }
            else {
                textBoxCurrentValue = textBoxCurrentValue.replace(selectedText + ', ', "");
                // textBoxCurrentValue1=textBoxCurrentValue1.replace(selectedValue + ', ', ""); 
                if (textBoxCurrentValue == '')
                    textBoxMain.value = '--Select--';
                    //textBoxMain1.value = '';
                else
                    textBoxMain.value = textBoxCurrentValue;
                // textBoxMain1.value +=textBoxCurrentValue1;

                hSelectedItemsValueList.value = hSelectedItemsValueList.value.replace(selectedValue + ', ', "");
            }
        }
        function SelectAll1() {
            //<%=this.ClientID%>
                // var ObjControl_ID = document.getElementById('ContentPlaceHolder1_');
                //ControlClientID = ObjControl_ID.value;
                var tblCbl = document.getElementById('ContentPlaceHolder1_' + 'ChkDealer');
                if (tblCbl == null) return;
                var tblBody = tblCbl.childNodes[0];
                var counter = tblBody.childNodes.length;
                if (bCheckAll == null) bCheckAll = true;
                else if (bCheckAll == true) bCheckAll = false;
                else if (bCheckAll == false) bCheckAll = true;
                for (index = 0; index < counter; index++) {
                    var tr = tblBody.childNodes[index];
                    if (tr.childNodes[0].childNodes[0].childNodes[0] == null) {
                        var checkbox = tr.childNodes[0].childNodes[0];
                    }
                    else {
                        var checkbox = tr.childNodes[0].childNodes[0].childNodes[0];
                    }
                    if (checkbox == null) return;
                    if (bCheckAll == true) {
                        checkbox.checked = true;
                    }
                    else {
                        checkbox.checked = false;
                    }
                    //        if (checkbox.checked)
                    //            checkbox.checked = false;
                    //        else
                    //            checkbox.checked = true;

                    //  SCIT(checkbox, ControlClientID);


                }
                var textBoxMain = window.document.getElementById('ContentPlaceHolder1' + '_' + 'txtDealerName');

                if (bCheckAll == true) {
                    if (textBoxMain != null) {
                        textBoxMain.value = 'All Selected';
                    }
                }
                else if (bCheckAll == false) {

                    textBoxMain.value = '--Select--';
                }

            }
            function SHMulSel11(ControlClientID, divMainWidth, divMainHeight, e) {
                var textBoxMain = window.document.getElementById('ContentPlaceHolder1' + '_' + 'txtDealerName');
                var divisionMain = window.document.getElementById('ContentPlaceHolder1' + '_' + 'divMain');

                var displayStatus = divisionMain.style.display;
                if (displayStatus == 'block') {
                    divisionMain.style.display = 'none';
                    return false;
                }
                else {
                    var ChkDealer = document.getElementById(ControlClientID + "_" + "ChkDealer");
                    //        divisionMain.style.width = divMainWidth + 'px';
                    //        divisionMain.style.height = divMainHeight + 'px';
                    if (ChkDealer == null) {
                        divisionMain.style.display = 'none';
                        return false;
                    }
                    else {
                        divisionMain.style.display = 'block';
                        return true;
                    }
                }
            }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="PageTitle panel-title" align="center" style="width: 15%">
                    <asp:Label ID="lblTitle" runat="server" Text="">
                    </asp:Label>
                </td>
            </tr>
            <tr id="ToolbarPanel">
                <td style="width: 15%">
                    <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                        <tr>
                            <td>
                                <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 15%; height: 92px;">
                    <asp:Panel ID="PDealerHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEDealerHeaderDetails" runat="server" TargetControlID="CntDealerHeaderDetails"
                            ExpandControlID="TtlDealerHeaderDetails" CollapseControlID="TtlDealerHeaderDetails"
                            Collapsed="false" ImageControlID="ImgTtlDealerHeaderDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show EGP Dealer Header Details"
                            ExpandedText="Hide EGP Dealer Header Details" TextLabelID="lblTtlDealerHeaderDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlDealerHeaderDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                        <asp:Label ID="lblTtlDealerHeaderDetails" runat="server" Text="EGP Dealer Header Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlDealerHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                            <table id="Table1" runat="server" class="ContainTable">
                                <tr>
                                    <td style="width: 15%" class="tdLabel">EGP Dealer Name:
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtEGPDealerName" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="lblMandatory" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">EGP Dealer Short Name:
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtEGPDealerShortName" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label1" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>

                                    <td style="width: 15%" class="tdLabel">Address 1 :
                                    </td>
                                    <td style="width: 15%">

                                        <asp:TextBox ID="txtAddress1" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label2" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                    </td>


                                </tr>
                                <tr>
                                    <td style="width: 15%" class="tdLabel">Address 2 :
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtAddress2" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label3" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                    </td>

                                    <td style="width: 15%" class="tdLabel">City:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:TextBox ID="txtCity" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label4" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                    </td>
                                    <td style="width: 15%" class="tdLabel">Pincode:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:TextBox ID="txtpincode" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label18" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="width: 15%" class="tdLabel">State:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:DropDownList ID="drpState" runat="server"
                                            CssClass="ComboBoxFixedSize" AutoPostBack="True"
                                            OnSelectedIndexChanged="drpState_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label6" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                    </td>
                                    <td style="width: 15%" class="tdLabel">Region:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:DropDownList ID="drpRegion" runat="server"
                                            CssClass="ComboBoxFixedSize" AutoPostBack="True"
                                            OnSelectedIndexChanged="drpRegion_SelectedIndexChanged" Enabled="false">
                                        </asp:DropDownList>

                                        <asp:Label ID="Label5" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                    </td>


                                    <td class="tdLabel" style="width: 15%">Country:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:TextBox ID="txtCountry" runat="server" CssClass="TextBoxForString"
                                            Text="India"></asp:TextBox>
                                        <asp:Label ID="Label7" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                    </td>

                                </tr>
                                <tr>
                                    <td style="width: 15%" class="tdLabel">Mobile:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:TextBox ID="txtDealerMobile" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                            Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                        <asp:Label ID="Label8" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                    </td>
                                    <td style="width: 15%" class="tdLabel">Phone:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:TextBox ID="txtLandLinePhone" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                            Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                        <asp:Label ID="Label9" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">Fax:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:TextBox ID="txtfax" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 15%">Email:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label10" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                    </td>


                                    <td class="tdLabel" style="width: 15%">MD Email:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:TextBox ID="txtMDEmail" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>


                                    </td>

                                    <td class="tdLabel" style="width: 5%; height: 26px;">

                                        <asp:Label ID="lblDistributorName" runat="server" CssClass="LabelLeftAlign" Text="Select Distributor :"></asp:Label>
                                    </td>
                                    <%--<td class="tdLabel" style="width: 30%; height: 26px;" colspan="3">--%>
                                    <td class="tdLabel" style="width: 30%; height: 26px;">

                                        <div id="dvTopDpo" class="dt" style="width: 95%;">
                                            <asp:CheckBox ID="ChkAll" runat="server" onClick="SelectAll1();" />
                                            <%--oncheckedchanged="ChkAll_CheckedChanged" --%>
                                            <asp:TextBox ID="txtDealerName" ReadOnly="True" runat="server" class="mstbm"
                                                Width="95%" Height="16px"></asp:TextBox>

                                        </div>
                                        <div id="divMain" runat="server" class="dvmain" style="width: 95%;">
                                            <img class="nicheimage" src="../Images/niche.gif" style="display: none" />
                                            <asp:CheckBoxList ID="ChkDealer" runat="server" Width="95%">
                                            </asp:CheckBoxList>
                                            <asp:LinkButton ID="lnkMain" runat="server" Text="Close Me"></asp:LinkButton>
                                        </div>
                                        <asp:Label ID="Label19" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>



                                </tr>
                                <tr>
                                    <td style="width: 15%" class="tdLabel">Start Year :
                                    </td>
                                    <td style="width: 15%">
                                        <asp:DropDownList ID="drpStartYear" runat="server"
                                            CssClass="ComboBoxFixedSize"
                                            OnSelectedIndexChanged="drpStartYear_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label12" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                    </td>
                                    <td style="width: 15%" class="tdLabel">Starting Finacial Year Date :
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtstartYear" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">Ending Finacial Year Date :
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtEndYear" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                    </td>

                                </tr>

                                <tr>
                                    <td style="width: 15%" class="tdLabel">Select HO/Branch :
                                    </td>
                                    <td style="width: 18%">
                                        <asp:DropDownList ID="drpHOBranch" runat="server" CssClass="ComboBoxFixedSize"
                                            Width="150px" OnSelectedIndexChanged="drpHOBranch_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1" Selected>HO</asp:ListItem>
                                            <asp:ListItem Value="2">Branch</asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                    <td style="width: 15%" class="tdLabel">

                                        <asp:Label ID="lblHO" runat="server" CssClass="LabelLeftAlign" Visible="false" Text="Select HO :"></asp:Label>
                                    </td>
                                    <td style="width: 18%">
                                        <asp:DropDownList ID="drpHO" runat="server"
                                            CssClass="ComboBoxFixedSize" Visible="false">
                                        </asp:DropDownList>

                                    </td>


                                    <td style="width: 15%" class="tdLabel">PAN No:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:TextBox ID="txtPANNo" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label13" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="width: 15%" class="tdLabel">TIN No:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtTINNo" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label14" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">C.S.T.:
                                    </td>
                                    <td style="width: 18%">

                                        <asp:TextBox ID="txtcst" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label15" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>

                                    <td style="width: 15%" class="tdLabel">S.T./VAT:
                                    </td>
                                    <td style="width: 18%">
                                        <asp:TextBox ID="txtST" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                        <asp:Label ID="Label16" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>





                                </tr>
                                <tr>
                                    <td style="width: 15%" class="tdLabel">Service Tax :
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtServiceTax" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">Contact Person :
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtContactPerson" runat="server" CssClass="TextBoxForString"
                                            Text=""></asp:TextBox>
                                    </td>


                                    <td class="tdLabel" style="width: 15%">Active:
                                    </td>
                                    <td style="width: 15%">
                                        <asp:DropDownList ID="drpActive" runat="server" AutoPostBack="True"
                                            CssClass="ComboBoxFixedSize" EnableViewState="true" Width="100px">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1" Selected>Y</asp:ListItem>
                                            <asp:ListItem Value="2">N</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="Label17" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                    </td>

                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="Panel1" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntCategoryDetails"
                            ExpandControlID="PanelCategoryDetails" CollapseControlID="PanelCategoryDetails"
                            Collapsed="false" ImageControlID="ImgTtlCategoryDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show EGP Dealer Part Category Details"
                            ExpandedText="Hide EGP Dealer Part Category Details" TextLabelID="lblTtlCategoryDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="PanelCategoryDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                        <asp:Label ID="lblTtlCategoryDetails" runat="server" Text="EGP Dealer Part Category Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlCategoryDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntCategoryDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                            <table id="tblCategoryDetails" runat="server" class="ContainTable">
                                <tr>
                                    <td style="width: 15%" class="tdLabel">Part Category A:
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtPartcatA" runat="server" CssClass="TextForAmount" Text="0.00"></asp:TextBox>

                                    </td>
                                    <td style="width: 15%" class="tdLabel">Part Category B:
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtPartcatB" runat="server" CssClass="TextForAmount"
                                            Text="0.00"></asp:TextBox>
                                    </td>


                                    <td style="width: 15%" class="tdLabel">Part Category C:
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtPartcatC" runat="server" CssClass="TextForAmount"
                                            Text="0.00"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="width: 15%" class="tdLabel">Part Category D:
                                    </td>
                                    <td style="width: 15%">
                                        <asp:TextBox ID="txtPartcatD" runat="server" CssClass="TextForAmount"
                                            Text="0.00"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%" class="tdLabel"></td>
                                    <td style="width: 15%"></td>


                                    <td style="width: 15%" class="tdLabel"></td>
                                    <td style="width: 15%"></td>

                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                            bIsCallForServer="true" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 15%"></td>
            </tr>
            <tr id="TmpControl">
                <td style="width: 15%">
                    <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:TextBox ID="txtLastDateNegotiation" runat="server" CssClass="HideControl" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:DropDownList ID="drpValidityDays" runat="server" CssClass="HideControl">
                    </asp:DropDownList>
                    <input id="hapb" type="hidden" name="tempHiddenField" runat="server" />
                    <input id="hsiv" type="hidden" name="hsiv" runat="server" />
                    <input id="__ET1" type="hidden" name="__ET1" runat="server" />
                    <input id="__EA1" type="hidden" name="__EA1" runat="server" />
                    <input id="txtControl_ID" type="hidden" name="__EA1" runat="server" />

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
