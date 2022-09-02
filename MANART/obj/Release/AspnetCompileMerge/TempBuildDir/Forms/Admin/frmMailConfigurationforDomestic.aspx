<%@ Page Title="MTI-Mail Configuration for Domestic " Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmMailConfigurationforDomestic.aspx.cs" Inherits="MANART.Forms.Admin.frmMailConfigurationforDomestic" %>

<%@ Register Src="../../WebParts/MultiselectLocation.ascx" TagName="Location" TagPrefix="uc6" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%--<%@ Register Src="../../WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>--%>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../../WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsAdminFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsWarrantyFunction.js"></script>

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
    <div class="table-responsive table-hover">
        <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="PageTitle panel-title" align="center" style="width: 15%">
                    <asp:Label ID="lblTitle" runat="server" Text="Mail Configuration"> </asp:Label>
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
                <td style="width: 15%">
                    <asp:Panel ID="PPartHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader">
                        <cc1:CollapsiblePanelExtender ID="CPEMailConfigurationHeaderDetails" runat="server" TargetControlID="CntMailConfigurationHeaderDetails"
                            ExpandControlID="TtlMailConfigurationHeaderDetails" CollapseControlID="TtlMailConfigurationHeaderDetails"
                            Collapsed="false" ImageControlID="ImgTtlMailConfigurationHeaderDetails" ExpandedImage="~/Images/Minus.png"
                            CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Mail Configuration Header Details"
                            ExpandedText="Hide Mail Configuration Header Details" TextLabelID="lblTtlMailConfigurationHeaderDetails">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlMailConfigurationHeaderDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                        <asp:Label ID="lblTtlMailConfigurationHeaderDetails" runat="server" Text="Mail Configuration Header Details"
                                            Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlMailConfigurationHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="CntMailConfigurationHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                            <table id="txtDocNoDetails" runat="server" class="ContainTable">
                                <tr>

                                    <td class="tdLabel" style="width: 15%; height: 10px;"></td>
                                </tr>
                                <tr>

                                    <td class="tdLabel" style="width: 3%; height: 30px;">
                                        <asp:Label ID="txtUserSelection" Font-Bold="true" CssClass="LabelLeftAlign" runat="server" Text="User Selection :"></asp:Label>
                                    </td>
                                    <td style="width: 20%; height: 30px;">

                                        <asp:RadioButtonList ID="OptionUserType" runat="server" RepeatDirection="Horizontal"
                                            BackColor="#efefef" Height="16px" AutoPostBack="True" OnSelectedIndexChanged="OptionUserType_SelectedIndexChanged">
                                            <asp:ListItem Value="D" Selected="True">Domestic</asp:ListItem>
                                            <asp:ListItem Value="E">Export</asp:ListItem>

                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 5%; height: 30px;">
                                        <asp:Label ID="lblDealerregion" Font-Bold="true" runat="server" CssClass="LabelLeftAlign" Text="Region:"></asp:Label>
                                    </td>
                                    <td style="width: 5%; height: 30px;">
                                        <asp:DropDownList ID="drpregion" runat="server" CssClass="ComboBoxFixedSize" EnableViewState="true"
                                            AutoPostBack="True"
                                            OnSelectedIndexChanged="drpregion_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tdLabel" style="width: 2%; height: 30px;">
                                        <%--  <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="State:" ></asp:Label>--%>
                                    </td>
                                    <td class="tdLabel" style="width: 9%; height: 30px;">
                                        <asp:Label ID="lblState" Font-Bold="true" CssClass="LabelLeftAlign" runat="server" Text="State:"></asp:Label>
                                    </td>
                                    <td style="width: 15%; height: 30px;">&nbsp;&nbsp;
                                        <asp:DropDownList ID="drpState" runat="server" CssClass="ComboBoxFixedSize" EnableViewState="true"
                                            AutoPostBack="True"
                                            OnSelectedIndexChanged="drpState_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="width: 5%; height: 26px;">

                                        <asp:Label ID="lblDistributorName" runat="server" CssClass="LabelLeftAlign" Text="Dealer Name:"></asp:Label>
                                    </td>
                                    <td class="tdLabel" style="width: 30%; height: 26px;" colspan="4">

                                        <div id="dvTopDpo" class="dt" style="width: 60%;">
                                            <asp:CheckBox ID="ChkAll" runat="server" onClick="SelectAll1();" />
                                            <%--oncheckedchanged="ChkAll_CheckedChanged" --%>
                                            <asp:TextBox ID="txtDealerName" ReadOnly="True" runat="server" class="mstbm"
                                                Width="87%" Height="16px"></asp:TextBox>

                                        </div>
                                        <div id="divMain" runat="server" class="dvmain" style="width: 60%;">
                                            <img class="nicheimage" src="../Images/niche.gif" style="display: none" />
                                            <asp:CheckBoxList ID="ChkDealer" runat="server" Width="87%">
                                            </asp:CheckBoxList>
                                            <asp:LinkButton ID="lnkMain" runat="server" Text="Close Me"></asp:LinkButton>
                                        </div>

                                    </td>

                                    <td class="tdLabel" style="width: 25%; height: 26px;">
                                        <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Dealer ID :" Visible="false"></asp:Label>
                                    </td>
                                    <td style="width: 20%; height: 26px;">
                                        <asp:TextBox ID="txtDealerID" CssClass="TextBoxForString" ReadOnly="true" Enabled="false" Visible="false" runat="server"
                                            Height="15px"></asp:TextBox>
                                    </td>

                                </tr>
                                <%-- <tr>
                            <td align="center"  style="height: 5px" colspan="8">
                           </td>
                        </tr>--%>
                                <tr>
                                    <td class="tdLabel" style="width: 10%; height: 22px;">
                                        <asp:Label ID="lblProcessName" Font-Bold="true" runat="server" CssClass="LabelLeftAlign" Text="Process Name :"></asp:Label>
                                    </td>
                                    <td style="width: 2%; height: 25px;">
                                        <asp:TextBox ID="txtProcessName" CssClass="TextBoxForString" runat="server"
                                            Width="150px" Height="15px" Visible="false"></asp:TextBox>
                                        <asp:DropDownList ID="drpProcessName" runat="server" Width="200px"
                                            CssClass="ComboBoxFixedSize" EnableViewState="true"
                                            AutoPostBack="True"
                                            OnSelectedIndexChanged="drpProcessName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 1%; height: 25px;">
                                        <%--<asp:DropDownList ID="drpProcessName" runat="server"  Width="200px" 
                                    CssClass="ComboBoxFixedSize" EnableViewState="true"
                                     AutoPostBack="True" 
                                         onselectedindexchanged="drpProcessName_SelectedIndexChanged" >
                                     </asp:DropDownList>--%>
                                    </td>

                                    <td class="tdLabel" style="width: 5%; height: 25px;">
                                        <asp:Label ID="lblDesc" Font-Bold="true" CssClass="LabelLeftAlign" runat="server" Text="Process Description :"></asp:Label>
                                    </td>
                                    <td colspan="4" style="width: 30%; height: 25px;">&nbsp;&nbsp;
                                        <asp:TextBox ID="txtDesc" CssClass="MultilineTextbox" runat="server"
                                            Width="300px" Height="15px"></asp:TextBox></td>
                                    <td style="height: 22px"></td>
                                    <td style="height: 22px"></td>
                                    <td style="height: 22px"></td>
                                </tr>


                                <tr>
                                    <td class="tdLabel" style="width: 13%; height: 25px;">
                                        <asp:Label ID="lblUserType" runat="server" CssClass="LabelLeftAlign" Text="User Type:" Font-Bold="true"></asp:Label>
                                    </td>

                                    <td style="width: 5%; height: 25px;">
                                        <asp:DropDownList ID="drpUserType" runat="server" CssClass="ComboBoxFixedSize" Width="150px">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Dealer</asp:ListItem>
                                            <asp:ListItem Value="2">VECV</asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                    <td></td>
                                    <td class="tdLabel" style="width: 10%; height: 22px;">
                                        <asp:Label ID="Label2" runat="server" Text="Module Type:" CssClass="LabelLeftAlign" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td class="tdLabel" style="width: 5%; height: 24px;">&nbsp;&nbsp;<asp:DropDownList ID="drpModuleType" runat="server" CssClass="ComboBoxFixedSize" Width="100px">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">P</asp:ListItem>
                                        <asp:ListItem Value="2">M</asp:ListItem>
                                    </asp:DropDownList>
                                    </td>

                                </tr>


                                <tr>
                                    <td class="tdLabel" style="width: 5%; height: 70px;">
                                        <asp:Label ID="Label3" runat="server" Text="To Mail:" CssClass="LabelLeftAlign" Font-Bold="true"></asp:Label>

                                    </td>
                                    <td colspan="4" style="width: 20%; height: 70px;">

                                        <asp:TextBox ID="txtTomail" runat="server" CssClass="MultilineTextbox"
                                            Height="60px" Text="" Width="98%" OnTextChanged="txtTomail_TextChanged"></asp:TextBox>
                                    </td>
                                    <td colspan="3" style="height: 30px;">

                                        <b class="Mandatory">&nbsp;&nbsp;&nbsp;&nbsp;Difference between Two Mail Separated By "-;" characters.</b> </td>


                                </tr>

                                <tr>
                                    <td class="tdLabel" style="width: 5%; height: 70px;">
                                        <asp:Label ID="Label4" runat="server" Text="Cc Mail:" CssClass="LabelLeftAlign" Font-Bold="true"></asp:Label>

                                    </td>
                                    <td colspan="4" style="width: 20%; height: 70px;">
                                        <asp:TextBox ID="txtCcMail" runat="server" CssClass="MultilineTextbox"
                                            Height="60px" Text="" Width="98%"></asp:TextBox>
                                    </td>
                                    <td style="height: 70px"></td>
                                    <td style="height: 70px"></td>
                                    <td style="height: 70px"></td>

                                </tr>

                                <tr>
                                    <td class="tdLabel" style="width: 5%; height: 32px;">
                                        <asp:Label ID="Label5" runat="server" Text="Subject:" CssClass="LabelLeftAlign" Font-Bold="true"></asp:Label>

                                    </td>
                                    <td colspan="4" style="width: 20%; height: 32px;">
                                        <asp:TextBox ID="txtsubject" runat="server" CssClass="MultilineTextbox"
                                            Height="22px" Text="" Width="98%"></asp:TextBox>
                                    </td>
                                    <td colspan="3" style="height: 32px;">

                                        <b class="Mandatory">&nbsp;&nbsp;&nbsp;&nbsp;Mail Subject Type Format like this "Warranty Claim no.
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp; "no" date "date"  is pending for Processing". </b></td>

                                </tr>

                                <tr>

                                    <td class="tdLabel" style="width: 5%; height: 32px;">
                                        <asp:Label ID="Label6" runat="server" Text="Mail Body:" CssClass="LabelLeftAlign" Font-Bold="true"></asp:Label>

                                    </td>
                                    <td colspan="4" style="width: 20%; height: 32px;">
                                        <asp:TextBox ID="txtMailBody" runat="server" CssClass="MultilineTextbox"
                                            Height="22px" Text="" Width="98%"></asp:TextBox>
                                    </td>
                                    <td colspan="3" style="height: 32px;">

                                        <b class="Mandatory">&nbsp;&nbsp;&nbsp;&nbsp;Mail Body Type Format like this "Warranty Claim no.
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp; "no" date "date" is pending for Processing".  </b></td>


                                </tr>

                                <tr>
                                    <td class="tdLabel" style="width: 5%; height: 22px;">

                                        <asp:Label ID="Label7" runat="server" Text="Signature:" CssClass="LabelLeftAlign" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td colspan="2" style="width: 20%; height: 22px;">
                                        <asp:DropDownList ID="drpSign" runat="server" CssClass="ComboBoxFixedSize">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Dealer</asp:ListItem>
                                            <asp:ListItem Value="2">VECV</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 15%" class="tdLabel">Active
                                    </td>
                                    <td style="width: 18%">
                                        <asp:DropDownList ID="drpActive" runat="server" Width="100px" CssClass="ComboBoxFixedSize" EnableViewState="true"
                                            AutoPostBack="True">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Y</asp:ListItem>
                                            <asp:ListItem Value="2">N</asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                    <td></td>
                                    <td></td>
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
            <tr id="TmpControl">
                <td style="width: 15%">
                    <asp:TextBox ID="txtControlCount" CssClass="HideControl" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:TextBox ID="txtFormType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtPreviousDocId" CssClass="HideControl" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDummyLC" CssClass="HideControl" runat="server" Width="1px" Text="N"></asp:TextBox>

                </td>
            </tr>
        </table>
        <input id="hapb" type="hidden" name="tempHiddenField" runat="server" />
        <input id="hsiv" type="hidden" name="hsiv" runat="server" />
        <input id="__ET1" type="hidden" name="__ET1" runat="server" />
        <input id="__EA1" type="hidden" name="__EA1" runat="server" />
        <input id="txtControl_ID" type="hidden" name="__EA1" runat="server" />
    </div>
</asp:Content>
