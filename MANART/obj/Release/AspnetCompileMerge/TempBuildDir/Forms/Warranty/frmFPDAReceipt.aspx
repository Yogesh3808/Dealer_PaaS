<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFPDAReceipt.aspx.cs" Inherits="MANART.Forms.Warranty.frmFPDAReceipt" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Details</title>
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
      <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsShowForm.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jquery-1.11.1.js"></script>
    <script src="../../Scripts/jsWCFileAttach.js"></script>
    <script src="../../Scripts/jsWCServiceHistory.js"></script>
    <%--<script src="../../Scripts/jsFileAttach.js"></script>--%>
    <script type="text/javascript">
        //function MaxLength1(maxLength) {
        //    debugger;
        //    text = document.getElementById('txtDtlDesc');
        //    if (text.value.length > maxLength) {
        //        alert("only max " + maxLength + " characters are allowed");
        //        //this limits the textbox with only 5 characters as lenght is given as 5.
        //        text.value = text.value.substring(0, maxLength);
        //    }
        //}
        function checkTextAreaMaxLength(textBox, e, length) {
            //debugger;
            text = document.getElementById('txtDtlDesc');
            var mLen = textBox["MaxLength"];
            if (null == mLen)
                mLen = length;

            var maxLength = parseInt(mLen);
            if (textBox.value.length > maxLength - 1) {
                alert("only max " + maxLength + " characters are allowed");
                //this limits the textbox with only 250 characters as lenght is given as 250.
                textBox.value = text.value.substring(0, length - 1);
            }
        }

    </script>
    <script type="text/javascript">
        document.onmousedown = disableclick;
        function disableclick(e) {
            var message = "Sorry, Right Click is disabled.";
            if (event.button == 2) {
                event.returnValue = false;
                alert(message);
                return false;
            }
        }
        window.onload = function () {
            //FirstTimeGridDisplay('');
            debugger;
            ObjGrid = window.document.getElementById("DetailsGrid");
            if (ObjGrid == null) return;
            var objtxtControl;            
            for (var MLoopCnt = 1; MLoopCnt <= ObjGrid.rows.length - 1; MLoopCnt++) {
                if (ObjGrid.rows[MLoopCnt].cells[8].childNodes[1].checked == false) {
                    ObjGrid.rows[MLoopCnt].cells[10].children[8].childNodes[1].value = "";
                    ObjGrid.rows[MLoopCnt].cells[10].children[8].childNodes[1].disabled = true;
                }
                else {
                    ObjGrid.rows[MLoopCnt].cells[10].children[8].childNodes[1].disabled = false;
                }
            }
        }        
        $(document).ready(function () {
            debugger;
            var txtDocDate = document.getElementById("txtDocDate_txtDocDate");           
            var txtLRDate = document.getElementById("txtLRDate_txtDocDate");
            var txtRecpDate = document.getElementById("txtRecpDate_txtDocDate");
            
           
            $('#txtLRDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
            });
            $('#txtRecpDate_txtDocDate').datepick({
                dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value, maxDate: '0d'
            });

            var DetailsGrid = document.getElementById("DetailsGrid");
            if (DetailsGrid != null) {
                var Elements = DetailsGrid.getElementsByTagName("input");
            }           

            if (Elements != null) {
                var splDate = '';
                var dt = '';
             
                for (var i = 0; i < Elements.length; i++) {
                    if (Elements[i].type == 'text' && Elements[i].id.indexOf("txtReceiptDate") != -1) {                        
                        $('#' + Elements[i].id).datepick({
                            //dateFormat: 'dd/mm/yyyy', minDate: txtDocDate.value
                            dateFormat: 'dd/mm/yyyy', minDate: (txtLRDate.value == '') ? '0d' : txtLRDate.value, maxDate: '0d' // '0d'//(txtDocDate.value == '') ? txtDocDate.value : txtDocDate.value
                        });
                    }
                }
            }
            if (DetailsGrid != null) {
                for (var MLoopCnt = 1; MLoopCnt <= DetailsGrid.rows.length - 1; MLoopCnt++) {
                    if (DetailsGrid.rows[MLoopCnt].cells[8].childNodes[1].checked == false) {
                        DetailsGrid.rows[MLoopCnt].cells[10].children[8].childNodes[1].value = "";
                        DetailsGrid.rows[MLoopCnt].cells[10].children[8].childNodes[1].disabled = true;
                    }
                    else {
                        DetailsGrid.rows[MLoopCnt].cells[10].children[8].childNodes[1].disabled = false;
                    }
                }
            }
        });
        function CloseMe() {
            window.close();
        }

        function ReturnPCRValue() {
            //debugger;
            var ObjControl = window.document.getElementById("txtID");
            var sValue;
            sValue = ObjControl.value;
            window.returnValue = sValue;
            window.close();

        }

    </script>
    <script type="text/javascript">
       
    
        function SelectAllFPDA(id) {
            var frm = document.forms[0];
            if (document.getElementById(id).checked == false) {
                if (confirm("Are you sure you want to deselect all the record?") == true) {
                }
                else {
                    return false;
                }
            }
            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "checkbox") {
                    frm.elements[i].checked = document.getElementById(id).checked;
                }
            }
        }

        function SetCurrentAndPastDate(obj, Msg) {

            var objDateValue = "";
            var ObjDate = obj;
            var x = new Date();
            var y = x.getYear();
            var m = x.getMonth() + 1; // added +1 because javascript counts month from 0
            var d = x.getDate();
            var dtCur = d + '/' + m + '/' + y;
            var dtCurDate = new Date(x.getYear(), x.getMonth(), x.getDate(), 00, 00, 00, 000)

            objDateValue = ObjDate.value;
            var sTmpValue = objDateValue;
            var day = dGetValue(sTmpValue.split("/")[0]);
            var month = dGetValue(sTmpValue.split("/")[1]) - 1;
            var year = dGetValue(sTmpValue.split("/")[2]);
            var sTmpDate = new Date(year, month, day);
            var TmpDay = 0;

            if (objDateValue == '') {
                return false;
            }
            if (dtCurDate < sTmpDate) {
                alert(Msg)
                ObjDate.value = "";
                ObjDate.focus();
                return false;
            }
        }
        
        function AcceptValidation(Obj) {
            var ObjGrid = null;
            ObjGrid = window.document.getElementById("DetailsGrid");            
            if (ObjGrid == null) return;
            var objtxtControl;
            debugger;
            if (Obj != null)
                for (var MLoopCnt = 1; MLoopCnt <= ObjGrid.rows.length - 1; MLoopCnt++) {
                    if (ObjGrid.rows[MLoopCnt].cells[8].childNodes[1].checked == false) {                                                    
                        ObjGrid.rows[MLoopCnt].cells[10].children[8].childNodes[1].value = "";                        
                        ObjGrid.rows[MLoopCnt].cells[10].children[8].childNodes[1].disabled = true;
                    }
                    else {                        
                        ObjGrid.rows[MLoopCnt].cells[10].children[8].childNodes[1].disabled = false;
                    }       
                }
            return true;
        }        
    </script>
   
    <style type="text/css">
        .auto-style1 {
            font-size: smaller;
        }

        .auto-style2 {
            width: 9%;
            height: 35px;
        }

        .auto-style3 {
            width: 14%;
            height: 35px;
        }
    </style>
    <base target="_self" />
</head>

<body runat="server">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release">
        </asp:ScriptManager>
        <table class="PageTable" border="1" width="100%">
            <tr id="TitleOfPage">
                <td class="PageTitle" align="center" style="width: 14%" colspan="4">
                    <asp:Label ID="lblTitle" runat="server" Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 9%" colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Save Receipt" CssClass="btn btn-search btn-sm"
                        Width="100px" OnClick="btnSave_Click" />                    
                    <asp:Button ID="btnApprove" runat="server" Text="Confirm Receipt" CssClass="btn btn-search btn-sm"
                        Width="100px" OnClick="btnApprove_Click" />                   
                </td>
                <td class="tdLabel" style="width: 14%;" colspan="2">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-search btn-sm"
                        OnClientClick="ReturnPCRValue();"
                        OnClick="btnBack_Click"></asp:Button> 
                </td>
            </tr>
            <tr id="TblControl">
                <td style="width: 9%; font-weight: 700;">FPDA No :
                </td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtDocNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>

                <td style="width: 9%">FPDA Date :
                </td>
                <td class="tdLabel" style="width: 14%;">
                    <uc3:CurrentDate ID="txtDocDate" runat="server" Mandatory="true" bCheckforCurrentDate="true" />
                </td>

            </tr>
            <tr id="TblControl">
                <td style="width: 9%; font-weight: 700;">Receipt No :
                </td>
                <td class="tdLabel" style="width: 14%;">
                    <asp:TextBox ID="txtRecptNo" CssClass="TextBoxForString" runat="server" Width="96%"
                        onkeydown="return ToSetKeyPressValueFalse(event,this);" Font-Bold="False" BackColor="#F7F7F7"></asp:TextBox>
                </td>

                <td style="width: 9%">Receipt Date :
                </td>
                <td class="tdLabel" style="width: 14%;">
                    <uc3:CurrentDate ID="txtRecpDate" runat="server" Mandatory="true" bCheckforCurrentDate="true" />
                </td>

            </tr>

            <tr>

                <td class="auto-style2">Transporter:</td>
                <td class="auto-style3">
                     <asp:TextBox ID="txtTransporter" runat="server" CssClass="TextBoxForString" MaxLength="50"
                                        ReadOnly="false" Text=""></asp:TextBox>
                </td>
                <td class="auto-style2">No Of Boxes :</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtNoOfCases" runat="server" CssClass="TextForAmount" ReadOnly="false"
                                        Text="" MaxLength="5" onkeypress="JavaScript:return CheckForNumericForKeyPress(event,0);" onblur="return BoxHdrValidation(this);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 9%">LR No:</td>
                <td class="tdLabel" style="width: 14%;">
                      <asp:TextBox ID="txtLRNo" runat="server" CssClass="TextBoxForString" ReadOnly="false"
                                        Text="" MaxLength="15"></asp:TextBox>
                </td>
                <td style="width: 9%">LR Date:</td>
                <td class="tdLabel" style="width: 14%;">
                   <uc3:CurrentDate ID="txtLRDate" runat="server" Mandatory="true" />                    
                </td>
            </tr>
            <tr>
                <td style="width: 9%">Remarks:</td>
                <td class="tdLabel" style="width: 14%;">
                   <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                       Text="" style="Height:34px;width :462px"></asp:TextBox>
                </td>
                <td style="width: 9%">Receipt Remark:</td>
                <td class="tdLabel" style="width: 14%;">
                  <asp:TextBox ID="txtReceiptRemark" runat="server" TextMode="MultiLine"
                                       Text="" style="Height:34px;width :462px"></asp:TextBox>
                </td>
            </tr>            
            <tr>
                <td style="width: 9%" colspan="4"></td>
            </tr>          
            <tr>
                <td style="width: 9%" colspan="4">
                    <asp:UpdatePanel UpdateMode="Conditional" ID="UPCompl" runat="server" ChildrenAsTriggers="true">                        
                        <ContentTemplate>
                             <asp:Panel ID="PModelDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEModelDetails" runat="server" TargetControlID="CntModelDetails"
                        ExpandControlID="TtlModelDetails" CollapseControlID="TtlModelDetails" Collapsed="false"
                        ImageControlID="ImgTtlModel Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Failed Part Details" ExpandedText="Failed Part Details"
                        TextLabelID="lblTtlModelDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlModelDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlModelDetails" runat="server" Text="Failed Part Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlModelDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>                    
                    <asp:Panel ID="CntModelDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <asp:GridView ID="DetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid"
                            Width="96%" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true" EditRowStyle-BorderColor="Black"
                            AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand"
                            Height="16px">                            
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <Columns>                                
                                <asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="17%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomer" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Customer_Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="17%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Claim  No" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCliam_No" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Cliam_No") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Claim Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCliam_Date" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Cliam_Date","{0:dd-MM-yyyy}") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Code" ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPart_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Name") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Claimed Qty" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPart_Qty" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Part_Qty","{0:#0.00}") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity Sent" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccPartQty" runat="server" CssClass="LabelCenterAlign" Text='<%#  Eval("AccPart_Qty","{0:#0.00}") %>'> </asp:Label>                                        
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Box No" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBoxNo" runat="server" CssClass="LabelCenterAlign" Text='<%#  Eval("Box_no") %>'> </asp:Label>                                                                                
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Received (Y/N)" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForAccept" runat="server" Checked='<%# Eval("RecvdYN") %>' onclick="return AcceptValidation(this);" />                                   
                                    </ItemTemplate>                                    
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="15%" HeaderText="Remark">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtRecvdRemark" runat="server"  CssClass="TextBoxForString" Text='<%# Eval("RecvdRemark") %>'> </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="15%" HeaderText="Received Date">
                                    <ItemTemplate>
                                       <uc3:CurrentDate ID="txtReceiptDate" runat="server" Mandatory="true" bCheckforCurrentDate="true" Text='<%# Eval("RecvdDate") %>' />                                     
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>                               
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                    <%-- </div>--%>
                </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UPRCompl" runat="server" AssociatedUpdatePanelID="UPCompl">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>            
            <tr>
                <td colspan="4">
                    <asp:TextBox ID="txtID" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDealerID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtDealerBrID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>                    
                    <asp:Label ID="lblComplaintsRecCnt" CssClass="HideControl" runat="server" Text="0"></asp:Label>        
                    <asp:TextBox ID="txtReportChassisID" CssClass="HideControl" runat="server" Width="1%" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtUserType" runat="server" CssClass="HideControl"></asp:TextBox>
                    <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"></asp:TextBox>
                    <asp:HiddenField ID="hdnReceiptStatus" runat="server" Value="" />
                </td>
            </tr>

        </table>
    </form>
</body>


</html>
