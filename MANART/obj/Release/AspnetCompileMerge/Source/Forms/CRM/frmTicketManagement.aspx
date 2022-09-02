 <%@ Page Title="Ticket Management" Language="C#"  MasterPageFile="~/Header.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="frmTicketManagement.aspx.cs" Inherits="MANART.Forms.CRM.frmTicketManagement" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    <%--<%@ Register Src="../../WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>--%>
    <%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
    <%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
    <%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
    <%@ Register Src="~/WebParts/MANPendingDoc.ascx" TagName="PendingDocument" TagPrefix="UCPDoc" %>
    <%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script src="../../Scripts/jquery-1.4.2.min.js"></script>
        <script src="../../Scripts/jquery.datepick.js"></script>
        <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
        <link href="../../Content/GridStyle.css" rel="stylesheet" />
        <script src="../../Scripts/jsValidationFunction.js"></script>
        <script src="../../Scripts/jsGridFunction.js"></script>
        <script src="../../Scripts/jsRFQFunction.js"></script>
        <script src="../../Scripts/jsMessageFunction.js"></script>
        <script src="../../Scripts/jsToolbarFunction.js"></script>
        <script src="../../Scripts/jsLCDetailsFunctions.js"></script>
        <script src="../../Scripts/jsVehicleINAndInsttaltionCer.js"></script>      
           <script src="../../Scripts/jsFileAttach.js"></script> 
        <script type="text/javascript">
            function pageLoad() {
                $(document).ready(function () {

                    var txtRoadPermitDate = document.getElementById("ContentPlaceHolder1_txtRoadPermitDate_txtDocDate");
                    $('#ContentPlaceHolder1_txtRoadPermitDate_txtDocDate').datepick({
                        // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                        onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '0d'
                    });





                    function customRange(dates) {
                        if (this.id == 'ContentPlaceHolder1_txtFromDate_txtDocDate') {
                            $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick('option', 'minDate', dates[0] || null);
                        }
                        else {
                            $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick('option', 'maxDate', dates[0] || null);
                        }
                    }
                });
            }
        </script>


    


   

        <script type='text/javascript'>
            function sldeUpDown(id, header) {
                if ($("#" + id).css('display') == 'block') {
                    $("#" + id).slideUp('slow');

                }
                else {
                    $("#" + id).slideDown('slow');
                }
            }
        </script>
        <style type="text/css">
            .auto-style1 {
                height: 22px;
            }
        </style>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="table-responsive">
            <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage">
                <td class="PageTitle" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text="Ticket Management"> </asp:Label>
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
                <td style="width: 14%">
                    <asp:Panel ID="LocationDetails" runat="server">
                        <uc2:Location ID="Location" runat="server" OnDealerSelectedIndexChanged="Location_DealerSelectedIndexChanged" />
                    </asp:Panel>
                
                   
              

                    <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" bIsCallForServer="true" OnImage_Click="SearchImage_Click" />
                    </asp:Panel>


                   

                  <asp:Panel ID="POHeader" runat="server" BorderColor="Black" BorderStyle="Double">

                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="CntDealerPO"
                            ExpandControlID="TtlDealerPO" CollapseControlID="TtlDealerPO" Collapsed="false"
                            ImageControlID="ImgTtlDealerPODetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Ticket Management" ExpandedText="Ticket Management"
                            TextLabelID="lblTtlDealerPO">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlDealerPO" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlDealerPO" runat="server" Text="Ticket Management" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlDealerPODetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="CntDealerPO" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                        <table id="txtDocNoDetails" runat="server" class="ContainTable" border="1">
                             
                            
                      
                                <tr>
                                  

                                     <td class="auto-style1">
                                    Ticket No:
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtPONo" runat="server" CssClass="TextBoxForString" Text="" 
                                        onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    &nbsp;
                                    </td>
                                     <td class="auto-style1">
                                    Ticket Date:
                                    </td>
                                    <td>
                                    <uc3:CurrentDate ID="txtPODate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                                    </td>


                                    </tr>
                          
                           <tr>
                                   <td>
                                    <asp:Label ID="Label1" runat="server" Text="Ticket Type:" CssClass="tdLabel"></asp:Label>
                                </td>

                               <td>
                                   <asp:DropDownList ID="drpTicketType" runat="server" height="25px" Width="90%"  EnableViewState="true" >
                                    <%--<asp:ListItem Value="0" >--Select--</asp:ListItem>--%>
                                    <asp:ListItem Value="1">Spares</asp:ListItem>
                                   <%-- <asp:ListItem Value="2">Service</asp:ListItem>--%>
                                </asp:DropDownList>
                               <asp:Label ID="Label10" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                


                               </td>
                                  <td>
                                    <asp:Label ID="Label12" runat="server" Text="Query:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtComplaint" runat="server" TextMode="MultiLine" MaxLength="250" height="50px" Width="90%" Text="" ></asp:TextBox>
                                   
                                </td>
                          
                           </tr>
                             <tr>
                                 <td>
                                    <asp:Label ID="Label2" runat="server" Text="Remarks by RWH:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtResponseRWH" runat="server"  Height="50px"  TextMode="MultiLine" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                   
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Remarks by MTI Support:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtResponseRSM" runat="server"  Height="50px"  TextMode="MultiLine" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                   
                                </td>

                                


                            </tr>
                            <tr>
                                 <td>
                                    <asp:Label ID="Label4" runat="server" Text="Remarks by Head:" CssClass="tdLabel"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtResponseHead" runat="server"  Height="50px"  TextMode="MultiLine" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                   
                                </td>
                                     
                            </tr>

                        </table>
                    </asp:Panel>
                        </asp:Panel>


                      <asp:Panel ID="PFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEFileAttchDetails" runat="server" TargetControlID="CntFileAttchDetails"
                        ExpandControlID="TtlFileAttchDetails" CollapseControlID="TtlFileAttchDetails"
                        Collapsed="false" ImageControlID="ImgTtlFileAttchDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Attached Documents"
                        ExpandedText="Attached Documents" TextLabelID="lblTtlFileAttchDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlFileAttchDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title" width="82%">
                                    <asp:Label ID="lblTtlFileAttchDetails" runat="server" Text="Attached Documents" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlFileAttchDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntFileAttchDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        Style="display: none;">
                        <table id="Table5" runat="server" class="ContainTable">
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="FileAttchGrid" runat="server" AllowPaging="false" AlternatingRowStyle-Wrap="true"
                                        AutoGenerateColumns="False" EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                                        GridLines="Horizontal" HeaderStyle-Wrap="true" DataKeyNames="File_Names"
                                        SkinID="NormalGrid" Width="100%">
                                        <%--OnRowCommand="DetailsGrid_RowCommand" OnRowDataBound="FileAttchGrid_RowDataBound"--%>
                                        <HeaderStyle CssClass="GridViewHeaderStyle" />
                                        <RowStyle CssClass="GridViewRowStyle" />
                                        <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No." ItemStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtFileAttchID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Description" ItemStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="GridTextBoxForString" Text='<%# Eval("Description") %>'
                                                        Width="96%"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Name" ItemStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFile" runat="server" Text='<%# Eval("File_Names") %>' Width="90%"
                                                        onClick="return ShowAttachDocument(this);"> 
                                                    </asp:Label>
                                                    <%-- onClick="return ShowAttachDoc(this);" ToolTip="Click Here To Open The File" ForeColor="#49A3D3"
                                                        onmouseout="SetCancelStyleOnMouseOut(this);" onmouseover="SetCancelStyleonMouseOver(this);"--%>
                                                    <%--<a id="achFileName" runat="server" title="Click here to download file"><%# Eval("File_Names") %></a>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkForDelete" runat="server" onClick="return SelectDeleteCheckboxCommon(this);" />
                                                    <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Download" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" ToolTip="Click Here To Open/Download File" OnClick="lnkDownload_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                                <HeaderStyle CssClass="HideControl" />
                                                <ItemStyle CssClass="HideControl" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle Wrap="True" />
                                        <EditRowStyle BorderColor="Black" Wrap="True" />
                                        <AlternatingRowStyle Wrap="True" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="width: 50%" align="center">File Description
                                </td>
                                <td class="tdLabel" style="width: 50%" align="center">File Name
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tdLabel">
                                    <div id="upload1" style="display: inline-block; padding-left: 15px">
                                        <input id="Text1" type="text" name="Text1" class="TextBoxForString" placeholder="File Description" style="width: 50%" />
                                        <input id="AttachFile" type="file" runat="server" style="width: 45%" class="TextBoxForString Cntrl1"
                                            onblur="return addFileUploadBox(this);" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>

                 
              
                </td>
            </tr>
            <tr id="TmpControl">
                <td style="width: 14%">
                    <asp:TextBox ID="txtControlCount" CssClass="DispalyNon" runat="server" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtFormType" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtDealerId" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtVelInDtlID" runat="server" CssClass="DispalyNon" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtPreviousDocId" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtCashLoan" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtDocCashLoanType" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM0ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM1ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM2ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM3ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM4ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM5ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM6ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtAppID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtCustID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                 <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                     <asp:Label ID="lblFileName" runat="server" CssClass="HideControl" Text="0"></asp:Label>
                        <asp:TextBox ID="txtUserType" runat="server" CssClass="HideControl"></asp:TextBox>
                <asp:TextBox ID="txtDealerCode" runat="server" CssClass="HideControl"></asp:TextBox>
                     <asp:Label ID="lblFileAttachRecCnt" runat="server" CssClass="HideControl" Text="0"></asp:Label>

                     <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                
                </td>
            </tr>
        </table>
        </div>
    </asp:Content>
