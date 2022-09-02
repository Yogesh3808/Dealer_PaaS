<%@ Page Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Title="MTI-Part Master" Theme="SkinFile" EnableViewState="true" CodeBehind="frmPartMaster.aspx.cs" Inherits="MANART.Forms.Master.frmPartMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsProformaFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsValidationFunction.js"></script>
           <script src="../../Scripts/jsMessageFunction.js"></script>
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
            //debugger;
            FirstTimeGridDisplay('ContentPlaceHolder1_');
            setTimeout("disableBackButton()", 0);
            disableBackButton();
            return true;
        }

        function refresh() {
            if (event.keyCode == 116) {
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
        function CheckBeforeUploadClick(objbutton, txtFilePath) {

     
  
            var ParentCtrlID;
            var objFileUpload;
            ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));  
            objFileUpload = document.getElementById(ParentCtrlID + "_" + txtFilePath);      
            var filename = objFileUpload.value;
         
            if (filename == "") {
                alert('Please select the file.');
                return false;
            }
            if (filename.search('xls') == -1) {
                alert('File is not in excel format.');
                return false;
            }
            if (filename.search('PartLocation_') == -1) {
                alert('File name is not in given format.');
                return false;
            }
        }
        //function CheckBeforeUploadClick(objbutton, FileUploadID) {
        //    alert('Hi');
        //    if (Validation() == false) {
        //        return false;
        //    }
        //    var ParentCtrlID;
        //    var objFileUpload;
        //    ParentCtrlID = objbutton.id.substring(0, objbutton.id.lastIndexOf("_"));
        //    objFileUpload = document.getElementById(ParentCtrlID + "_" + FileUploadID);
        //    var filename = objFileUpload.value;
        //    var sFileExtension = filename.split('.')[filename.split('.').length - 1].toLowerCase();
        //    var iFileSize = objFileUpload.size;
        //    var iConvert = (objFileUpload.size / 10485760).toFixed(2);
        //    if (filename == "") {
        //        alert('Please select the file.');
        //        return false;
        //    }
        //    if (filename.search('xls') == -1) {
        //        alert('File is not in excel format.');
        //        return false;
        //    }
        //    alert('i');
        //    if (filename.search('_SparesLocation_') == -1) {
        //        alert('File name is not in given format.');
        //        return false;
        //    }

        //}
    </script>
    <style>
        .tdLabel {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            color: Black;
            /*font-weight: bold;*/
            padding-left: 25px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" style="width: 100%" class="table-responsive">
        <tr id="TitleOfPage">
            <td align="center" class="panel-heading">
                <asp:Label ID="lblTitle" runat="server" CssClass="panel-title"></asp:Label>
            </td>
        </tr>
        <tr id="ToolbarPanel">
            <td>
                <table id="ToolbarContainer" runat="server" border="1" width="100%">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="Table3" runat="server" class="table table-responsive table-bordered">
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr id="TblControl">
            <td>
                <asp:Panel ID="PPartHeaderDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <cc1:CollapsiblePanelExtender ID="CPEPartHeaderDetails" runat="server" TargetControlID="CntPartHeaderDetails"
                        ExpandControlID="TtlPartHeaderDetails" CollapseControlID="TtlPartHeaderDetails"
                        Collapsed="false" ImageControlID="ImgTtlPartHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Part Header Details"
                        ExpandedText="Part Header Details" TextLabelID="lblTtlPartHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlPartHeaderDetails" runat="server">
                        <table>
                            <tr class="panel-heading">
                                <td align="center" class="panel-title">
                                    <asp:Label ID="lblTtlPartHeaderDetails" runat="server" Text="Part Header Details"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlPartHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntPartHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table2" runat="server" class="table table-bordered">
                            <tr>
                                <td style="width: 15%" class="tdLabel">Part No:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="NonEditableFields"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Part Name:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtMTIPartName" runat="server" CssClass="NonEditableFields"></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">Base Unit:
                                </td>
                                <td style="width: 18%">
                                    <%--<asp:TextBox ID="txtBaseUnit" runat="server" CssClass="TextBoxForString" ReadOnly="true"
                                        Text="Ltrs."></asp:TextBox>--%>
                                    <%--<asp:DropDownList ID="drpUnit" runat="server" CssClass="NonEditableFields"></asp:DropDownList>--%>
                                    <asp:TextBox ID="txtBaseUnit" runat="server" CssClass="NonEditableFields"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>

                                <td class="tdLabel" style="width: 15%">Part Group:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtGroup" runat="server" CssClass="NonEditableFields"
                                        Text=""></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">Superseded Part: 
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtSupersededPart" runat="server" CssClass="NonEditableFields"
                                        Text=""></asp:TextBox>
                                </td>
                                <td class="tdLabel" style="width: 15%">Block For Purchase: 
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtBlockForPurchase" runat="server" CssClass="NonEditableFields"
                                        Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>

                                <td style="width: 15%" class="tdLabel">Min Order Qty:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtMinOrderQty" runat="server" CssClass="NonEditableFields"
                                        Text=""></asp:TextBox>
                                </td>
                                <td style="width: 15%" class="tdLabel">
                                    <asp:Label ID="lblLocation" runat="server" Text=" Location:"></asp:Label>
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtLocation" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                </td>

                                <td style="width: 15%" class="tdLabel">Active
                                </td>
                                <td style="width: 18%">
                                    <asp:DropDownList ID="drpActive" runat="server" Width="100px" CssClass="NonEditableFields" EnableViewState="true">
                                        <asp:ListItem Value="1" Selected="True">Y</asp:ListItem>
                                        <asp:ListItem Value="2">N</asp:ListItem>
                                    </asp:DropDownList>
                                </td>

                            </tr>
                            <tr style="display: none">
                                <td style="width: 15%" class="tdLabel">Part Category:
                                </td>
                                <td style="width: 18%">
                                    <asp:TextBox ID="txtPartCategory" runat="server" CssClass="NonEditableFields"
                                        Text="" MaxLength="1"></asp:TextBox>
                                </td>
                                <td style="width: 15%"></td>
                                <td style="width: 18%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 18%"></td>
                            </tr>
                        </table>

                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="UploafFile" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <table id="Table1" runat="server" class="table table-bordered">
                        <tr>
                            <td style="width: 15%" class="tdLabel">Select File:
                            </td>
                            <%--  OnClientClick="return CheckBeforeUploadClick(this,'txtFilePath');" --%>
                            <td style="width: 18%">
                                <asp:FileUpload ID="txtFilePath" runat="server" />
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-search"
                                    OnClick="btnUpload_Click"  OnClientClick="return CheckBeforeUploadClick(this,'txtFilePath');" /><br />
                                <asp:Label ID="lblMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009933"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 20px; color: Red" colspan="4">
                                <p>
                                    <a>Please select the file in the excel format. File name should be in format as 'DealerCode_PartLocation_Datestamp'
                                    <br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;e.g. &#39;D002500_PartLocation_11072016&#39;. </a>
                                    <br />
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 10px;">
                                <asp:Label ID="lblListPartNo" runat="server" Text="" Width="100%" ForeColor="Red"
                                    Visible="false"> </asp:Label>
                                <asp:TextBox ID="txtListPartNo" TextMode="MultiLine" runat="server" Text="" Width="100%"
                                    ForeColor="Red" Visible="false"></asp:TextBox>
                                <%--#49A3D3--%>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <div class="">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" OnImage_Click="SearchImage_Click"
                            bIsCallForServer="true" />
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr id="TmpControl">
            <td>
                <div style="display: none">
                    <asp:TextBox ID="txtControlCount" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:TextBox ID="txtFormType" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtID" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtPreviousDocId" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:TextBox ID="txtLastDateNegotiation" runat="server" Width="1px"
                        Text=""></asp:TextBox>
                    <asp:DropDownList ID="drpValidityDays" runat="server">
                    </asp:DropDownList>
                    <asp:HiddenField ID="hidentotalcount" runat="server" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
