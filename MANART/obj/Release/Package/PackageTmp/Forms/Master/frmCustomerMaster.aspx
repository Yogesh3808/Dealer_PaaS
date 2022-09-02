<%@ Page Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"
    Title="MTI-Customer Master" Theme="SkinFile" EnableViewState="true" CodeBehind="frmCustomerMaster.aspx.cs" Inherits="MANART.Forms.Master.frmCustomerMaster" %>

<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
   
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />

    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsProformaFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
       <script src="../../Scripts/jquery-1.4.2.min.js"></script>

     <script type="text/javascript">
         function onTextChange(data) {
             
             PageMethods.GetInvoice(document.getElementById("<%=txtID.ClientID%>").value, ($('#<%=drpActive.ClientID %>').val()), OnSuccess);
                   
        }
        function OnSuccess(response, userContext, methodName) {

            if (response != 0) {
                $("#<%=drpActive.ClientID%>")[0].selectedIndex = 1;
                alert(response);
              
                <%--alert($('#<%=drpActive.ClientID %>').val());--%>

            }
        }
    </script>

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

        function CheckcustType(event, objcontrol) {
            
            if (objcontrol.selectedIndex == 3) {

                alert("Only One customer of Cash Sale Type Allowed");
                // objcontrol.focus();
                objcontrol.value = "0";
                return false;
            }

            else {
                return true;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable table-responsive" border="1">
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
                        Collapsed="False" ImageControlID="ImgTtlDealerHeaderDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Customer Header Details"
                        ExpandedText="Customer Header Details" TextLabelID="lblTtlDealerHeaderDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlDealerHeaderDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlDealerHeaderDetails" runat="server" Text="Customer Header Details"
                                        Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%" class="panel-title">
                                    <asp:Image ID="ImgTtlDealerHeaderDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntDealerHeaderDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <table id="Table1" runat="server" class="ContainTable table table-bordered">
                                    <tr>
                                        <td style="width: 15%" class="tdLabel">Type:
                                        </td>
                                        <td style="width: 15%">
                                            <%--    <asp:DropDownList ID="drpCustType" runat="server"  CssClass="ComboBoxFixedSize" AppendDataBoundItems="true"
                                      onBlur="CheckcustType(this,'DrpCustomerType')" onselectedindexchanged="drpCustType_SelectedIndexChanged"  
                                         
                                         AutoPostBack="true"  >
                                    </asp:DropDownList>--%>
                                            <asp:DropDownList ID="drpCustType" runat="server" AutoPostBack="true" 
                                                CssClass="ComboBoxFixedSize" onBlur="CheckcustType(this,'drpCustType')" OnSelectedIndexChanged="drpCustType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblMandatory" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">Name:
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="TextBoxForString" MaxLength="100" >
                                                </asp:TextBox>
                                              <asp:DropDownList ID="drpDealerName" runat="server"  AutoPostBack="true" 
                                                CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpDealerName_SelectedIndexChanged" >
                                            </asp:DropDownList>
                                            <asp:Label ID="Label1" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                        </td>

                                        <td style="width: 15%" class="tdLabel">Address 1 :
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtAddress1" runat="server" CssClass="TextBoxForString"  MaxLength="200"
                                              
                                                Text=""></asp:TextBox>
                                            <%--  OnTextChanged="txtAddress1_TextChanged" AutoPostBack="true"--%>
                                            <asp:Label ID="Label2" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%" class="tdLabel">Address 2 :
                                        </td>
                                        <td style="width: 15%">

                                            <asp:TextBox ID="txtAddress2" runat="server" CssClass="TextBoxForString"  MaxLength="200"
                                                Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">Address 3 :
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtAddress3" runat="server" CssClass="TextBoxForString"  MaxLength="40"
                                                Text=""></asp:TextBox>


                                        </td>

                                        <td style="width: 15%" class="tdLabel">City:
                                        </td>
                                        <td style="width: 18%">
<%-- OnTextChanged="txtCity_TextChanged"  AutoPostBack="true"--%>
                                            <asp:TextBox ID="txtCity" runat="server" CssClass="TextBoxForString" 
                                              
                                                Text=""></asp:TextBox>
                                            <asp:Label ID="Label5" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="width: 15%" class="tdLabel">Pincode:
                                        </td>
                                        <td style="width: 18%">

                                            <asp:TextBox ID="txtpincode" runat="server" CssClass="TextBoxForString"   
                                                MaxLength="6"
                                        Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"
                                                ></asp:TextBox>
                                         <%--   OnTextChanged="txtpincode_TextChanged"
                                                AutoPostBack="true"--%>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">State:
                                        </td>
                                        <td style="width: 18%">

                                            <asp:DropDownList ID="drpState" runat="server" AutoPostBack="True" 
                                                CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpState_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:Label ID="Label4" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>

                                        </td>
                                        <td style="width: 15%" class="tdLabel">Region:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:UpdatePanel ID="UpdatePanel111" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="drpRegion" runat="server"
                                                        CssClass="ComboBoxFixedSize" 
                                                        OnSelectedIndexChanged="drpRegion_SelectedIndexChanged" Enabled="false">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="drpState" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel" style="width: 15%">Country:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtCountry" runat="server" CssClass="TextBoxForString"  
                                                Text="India" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">Mobile:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtMobile" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                              
                                                Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                              <%--OnTextChanged="txtMobile_TextChanged"
                                                AutoPostBack="true--%>
                                            <asp:Label ID="Label3" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">Phone:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtPhone" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                                Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel" style="width: 15%">Email:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString"  
                                                Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">Contact Person :
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="TextBoxForString"
                                                Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">Contact Person Phone:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtContactPersonPhone" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                                onkeypress="return CheckForTextBoxValue(event,this,'8');"
                                                Text=""></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%" class="tdLabel">PAN No:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtPANNo" runat="server" CssClass="TextBoxForString"  
                                                Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">TIN No:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtTINNo" runat="server" CssClass="TextBoxForString"
                                                Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">C.S.T.:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtcst" runat="server" CssClass="TextBoxForString"
                                                Text=""></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%" class="tdLabel">S.T./VAT:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtST" runat="server" CssClass="TextBoxForString"  
                                                Text=""></asp:TextBox>
                                        </td>
                                         <td style="width: 15%" class="tdLabel">L.B.T:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtLBT" runat="server" CssClass="TextBoxForString"  
                                                Text=""></asp:TextBox>
                                        </td>
                                        <td class="tdLabel" style="width: 15%">Active:
                                        </td>
                                        <td style="width: 15%">
                                            <asp:DropDownList ID="drpActive" runat="server" Width="100px" CssClass="ComboBoxFixedSize" EnableViewState="true"
                                                AutoPostBack="false"  onchange="onTextChange(this)">
                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">Y</asp:ListItem>
                                                <asp:ListItem Value="2">N</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="Label6" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
                                        </td>                                       
                                    </tr>
                                    <tr>
                                         <td style="width: 15%" class="tdLabel">GSTIN No:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtgstno" runat="server" CssClass="TextBoxForString"  
                                                Text=""></asp:TextBox>
                                        </td>

                                        <td style="width: 15%" class="tdLabel">Customer Code:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtCustCode" runat="server" CssClass="TextBoxForString"   ReadOnly="true"
                                                Text=""></asp:TextBox>
                                        </td>
                                        <td style="width: 15%" class="tdLabel"><%--Ledger Name:--%>
                                        </td>
                                        <td style="width: 18%">                                            
                                             <asp:TextBox ID="txtLedgerName" runat="server" CssClass="TextBoxForString"  MaxLength="100" Visible="false"
                                                Text=""></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </asp:Panel>

                </asp:Panel>

                  <asp:Panel ID="Panel2" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="Panel5"
                        ExpandControlID="Panel3" CollapseControlID="Panel3"
                        Collapsed="False" ImageControlID="Image1" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Delivery Details"
                        ExpandedText="Delivery Details" TextLabelID="Label7">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="Panel3" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="Label7" runat="server" Text="Customer Header Details"
                                        Width="96%" onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%" class="panel-title">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="Panel5" runat="server" BorderColor="Black" BorderStyle="Double">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <table id="Table2" runat="server" class="ContainTable table table-bordered">
                 
                                   

                                    <tr>


                                          <td style="width: 15%" class="tdLabel">Delivery Name:
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtDelvName" runat="server" CssClass="TextBoxForString" MaxLength="100" >
                                                </asp:TextBox>
                                            </td>
                                         <td style="width: 15%" class="tdLabel">Delivery Mobile:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtDelvMobile" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                                onkeypress="return CheckForTextBoxValue(event,this,'8');"
                                                Text=""></asp:TextBox>
                                        </td>




                                          <td style="width: 15%" class="tdLabel">Contact Person:
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtDelContactPerson" runat="server" CssClass="TextBoxForString" MaxLength="100" >
                                                </asp:TextBox>
                                            </td>
                                        
                                    </tr>
                                    <tr>

                                         <td style="width: 15%" class="tdLabel">Contact Person Phone:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtDelContactPhNo" runat="server" CssClass="TextBoxForString" MaxLength="12"
                                                onkeypress="return CheckForTextBoxValue(event,this,'8');"
                                                Text=""></asp:TextBox>
                                        </td>

                                        <td style="width: 15%" class="tdLabel">Address 1 :
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtdeliveryadd1" runat="server" CssClass="TextBoxForString" MaxLength="200"
                                              
                                                Text=""></asp:TextBox>
                                            <%--  OnTextChanged="txtAddress1_TextChanged" AutoPostBack="true"--%>
                                        </td>

                                         <td style="width: 15%" class="tdLabel">Address 2 :
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtdeliveryadd2" runat="server" CssClass="TextBoxForString" MaxLength="200"
                                              
                                                Text=""></asp:TextBox>
                                            <%--  OnTextChanged="txtAddress1_TextChanged" AutoPostBack="true"--%>
                                        </td>

                                       
                                       
                                    </tr>
                                    
                               <tr>
                                     <td style="width: 15%" class="tdLabel">Address 3 :
                                        </td>
                                        <td style="width: 15%">
                                            <asp:TextBox ID="txtdeliveryadd3" runat="server" CssClass="TextBoxForString"  MaxLength="200"
                                              
                                                Text=""></asp:TextBox>
                                            <%--  OnTextChanged="txtAddress1_TextChanged" AutoPostBack="true"--%>
                                        </td>
                                        <td style="width: 15%" class="tdLabel">City:
                                        </td>
                                        <td style="width: 18%">
<%-- OnTextChanged="txtCity_TextChanged"  AutoPostBack="true"--%>
                                            <asp:TextBox ID="txtdeliverycity" runat="server" CssClass="TextBoxForString" 
                                              
                                                Text=""></asp:TextBox>

                                        </td>

                                      <td style="width: 15%" class="tdLabel">Pincode:
                                        </td>
                                        <td style="width: 18%">

                                            <asp:TextBox ID="txtDelvPin" runat="server" CssClass="TextBoxForString"   
                                                MaxLength="6"
                                        Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"
                                                ></asp:TextBox>
                                         <%--   OnTextChanged="txtpincode_TextChanged"
                                                AutoPostBack="true"--%>
                                        </td>



                               </tr>
                                    <tr>
                                        
                                        
                                   <td style="width: 15%" class="tdLabel">State:
                                        </td>
                                        <td style="width: 18%">

                                            <asp:DropDownList ID="ddldeliverystate" runat="server" 
                                                CssClass="ComboBoxFixedSize">
                                            </asp:DropDownList>


                                        </td>

                                   

                                        <td class="tdLabel" style="width: 15%">Email:
                                        </td>
                                        <td style="width: 18%">
                                        <asp:TextBox ID="txtDelEmail" runat="server" CssClass="TextBoxForString"  
                                        Text=""></asp:TextBox>
                                        </td>

                                        <td style="width: 15%" class="tdLabel">PAN No:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtDelPan" runat="server" CssClass="TextBoxForString"  
                                                Text=""></asp:TextBox>
                                        </td>




                                     
                                    </tr>
                                    <tr>
                                           <td style="width: 15%" class="tdLabel">GSTIN No:
                                        </td>
                                        <td style="width: 18%">
                                            <asp:TextBox ID="txtDelvGST" runat="server" CssClass="TextBoxForString"  
                                                Text=""></asp:TextBox>
                                        </td>
                                        
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </asp:Panel>

                </asp:Panel>
                <asp:Panel ID="Panel1" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader" >
                    <%--Style="display: none"--%>
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntCategoryDetails"
                        ExpandControlID="PanelCategoryDetails" CollapseControlID="PanelCategoryDetails"
                        Collapsed="false" ImageControlID="ImgTtlCategoryDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Customer Discount Category Details"
                        ExpandedText="Customer Discount Category Details" TextLabelID="lblTtlCategoryDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="PanelCategoryDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblTtlCategoryDetails" runat="server" Text="Customer Discount Category Details"
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
                        <asp:GridView ID="GrdCategoryDetails" runat="server" AllowPaging="True" CssClass="table table-striped table-condensed"
                            AlternatingRowStyle-Wrap="true" AutoGenerateColumns="False"
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true"
                            GridLines="Horizontal" SkinID="NormalGrid"
                            Width="100%">
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            <Columns>
                                <asp:TemplateField HeaderText="Group ID" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGroupID" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("ID") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                    <ItemStyle CssClass="HideControl" />
                                    <HeaderStyle CssClass="HideControl" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Part Category" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartCategory" runat="server" CssClass="LabelCenterAlign"
                                            Text='<%# Eval("Part_Cat") %>'> </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Percentage" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblPercentage" CssClass="GridTextBoxForAmount" runat="server"
                                            Text='<%# Eval("Per","{0:#0.00}") %>' Width="30%" onkeypress="return CheckPercentageAmount(event,this);"
                                            onblur="return CheckPercentageValue(event,this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>

                 <asp:Panel ID="Panel4" runat="server" BorderColor="DarkGray" BorderStyle="Double">
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
                <asp:HiddenField ID="hdnRowCount" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRowCountGlobal" runat="server" Value="0" />
                <asp:HiddenField ID="hdnFlag" runat="server" Value="N" />
            </td>
        </tr>
    </table>
</asp:Content>
