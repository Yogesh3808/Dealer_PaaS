<%@ Page Title="Request For Price" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="frmReqforProforma.aspx.cs" Inherits="MANART.Forms.VehiclePurchase.Export.frmReqforProforma" MaintainScrollPositionOnPostback="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
    <%--<%@ Register Src="../../WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>--%>
    <%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
    <%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
    <%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
    <%@ Register Src="~/WebParts/MANPendingDoc.ascx" TagName="PendingDocument" TagPrefix="UCPDoc" %>
    <%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script src="../../../Scripts/jsValidationFunction.js"></script>
        <script src="../../../Scripts/jsMessageFunction.js"></script>
        <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
  
    <script src="../../Scripts/jsToolbarFunction.js"></script>
  
  
      
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="table-responsive">
            <table id="PageTbl" class="PageTable" border="1">
            <tr id="TitleOfPage">
                <td class="PageTitle" align="center" style="width: 14%">
                    <asp:Label ID="lblTitle" runat="server" Text="Request For Price (RFP)"> </asp:Label>
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
                            SuppressPostBack="true" CollapsedText="Request For Price (RFP)" ExpandedText="Request For Price (RFP)"
                            TextLabelID="lblTtlDealerPO">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlDealerPO" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlDealerPO" runat="server" Text="Request For Price (RFP)" Width="96%"
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
                                    RFP No:
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtPONo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                    &nbsp;
                                    </td>
                                     <td class="auto-style1">
                                    RFP Date:
                                    </td>
                                    <td>
                                    <uc3:CurrentDate ID="txtPODate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                                    </td>


                                    </tr>
                            <tr>
                                     
                                     

                                  <td class="auto-style1">
                                    Dealer Remarks:
                                    </td>
                                  <td>
                                    <asp:TextBox ID="txtPaymentDetailsPO" runat="server" TextMode="MultiLine"  MaxLength="250" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                    </td>
                              
                              
                                    
                                </tr>
                             
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHORemarks" runat="server" Text="HO Remarks:" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAppRemarks" runat="server" CssClass="TextBoxForString" MaxLength="250" Text="" TextMode="MultiLine" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                            
                            
                              
                        </table>
                    </asp:Panel>
                        </asp:Panel>

                    <asp:Panel ID="Panel3" runat="server" BorderColor="Black" BorderStyle="Double">
                          <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntModel"
                            ExpandControlID="TtlModel" CollapseControlID="TtlModel" Collapsed="false"
                            ImageControlID="ImgTtlModelDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Model" ExpandedText="Model"
                            TextLabelID="lblTtlModel">
                        </cc1:CollapsiblePanelExtender>
                         <asp:Panel ID="TtlModel" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlModel" runat="server" Text="Model" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlModelDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="CntModel" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">

                         <table id="Table2" runat="server" class="ContainTable" border="1">
                            <%-- <tr class="panel-heading">
                                <td align="center" class="panel-title" style="height: 15px" colspan="6">
                                    Model
                                </td>
                            </tr>--%>
                            <tr>
                                
                               
                               <td class="tdLabel">
                                Model Category:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelCat" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="drpModelGroup_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label35" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                            </td>
                                  <td class="auto-style1">
                                    Model Code:
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpModelCode" runat="server"  AutoPostBack="True"  OnSelectedIndexChanged="drpModelCode_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    
                                </td>
                            
                                  <td class="auto-style1">
                                    Model:
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpModel" runat="server"   AutoPostBack="True"  OnSelectedIndexChanged="drpModel_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                   
                                </td>
                            </tr>
                            <tr>
                            
                            
                                <td class="auto-style1">
                                    Model Price:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtModelRate" ReadOnly="true" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    &nbsp;
                                </td>

                                 <td class="auto-style1">
                                    Quantity:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtQty" runat="server" AutoPostBack="true" OnTextChanged="txtQty_TextChanged" onkeypress=" return CheckForTextBoxValue(event,this,'5');" CssClass="TextForAmount" ></asp:TextBox>
                                    
                                </td>

                                <td class="auto-style1">
                                    Total:
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtTotal" runat="server" ReadOnly="true" CssClass="TextBoxForString" ></asp:TextBox>
                                   
                                </td>


                              
                            </tr>
                              <tr>
                                  <td class="auto-style1">
                                 <asp:Label ID="Modelgrp" Text="Model Group:" runat="server" Visible="false" ></asp:Label>
                                    </td>
                                <td>
                                    <asp:DropDownList ID="drpModelGroup" runat="server" Visible="false"  CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                    <%--<b class="Mandatory">*</b>--%>
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
                    <asp:HiddenField ID="hdntax" runat="server" Value="" />
                    <asp:HiddenField ID="hdGST" runat="server" value=""/>
                      <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                     <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                
                </td>
            </tr>
        </table>
        </div>
    </asp:Content>
