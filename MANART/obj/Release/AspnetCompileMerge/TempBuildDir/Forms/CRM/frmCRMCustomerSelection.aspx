
<%@ Page Title="MTI-Customer Selection" Language="C#" AutoEventWireup="true" CodeBehind="frmCRMCustomerSelection.aspx.cs" Inherits="MANART.Forms.CRM.frmCRMCustomerSelection" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
     <link href="../../Content/PaggerStyle.css" rel="stylesheet" />
 <%-- //  <script src="../../Scripts/jsShowForm.js"></script>--%>
    <script type="text/javascript">
        function ReturnCRMCustomerValue(objImgControl) {
            //debugger;
            var objRow = objImgControl.parentNode.parentNode.childNodes;
            var sValue;
            var ArrOfCustomer = new Array();
            for (var cnt = 1; cnt < objRow.length - 1; cnt++) {
                // sValue = objRow[cnt].innerHTML.trim();
                sValue = objRow[cnt].innerText.trim();
                ArrOfCustomer.push(sValue);
            }
            //alert(ArrOfChassis);
            window.returnValue = ArrOfCustomer;
            window.close();
        }
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

        }

    </script>
    <base target="_self" />
</head>
<body>
    <form id="form2" runat="server">
    <div class="table-responsive">
        <table class="PageTable" border="1" width="100%">
            <tr id="TitleOfPage" class="panel-heading">
                <td class="panel-title" align="center" style="width: 14%">
                     <asp:Label ID="lblTitle" runat="server" Text="">
                         </asp:Label>
                </td>
            </tr>
          
       
            <tr id="TblControl">
                <td style="width: 14%">
                    <div align="center" class="ContainTable">
                        <table style="background-color: #efefef;" width="100%">
                           
                        <tr align="center">
                            <td class="tdLabel" style="width: 7%;">
                                Search:
                            </td>
                            <td class="tdLabel" style="width: 15%;">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="TextBoxForString"></asp:TextBox>                                
                            </td>
                            
                            <td class="tdLabel" style="width: 15%;">
                                <asp:DropDownList ID="DdlSelctionCriteria" runat="server" 
                                    CssClass="ComboBoxFixedSize">
                                    <asp:ListItem Selected="True" Value ="Customer_name">Customer Name</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td  style="width: 15%;">                                
                                <asp:Button ID="btnSearch" runat="server" Text="Search"  
                                    CssClass="CommandButton" onclick="btnSearch_Click" />
                            </td>                           
                        </tr>
                          
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="ContaintTableHeader" ScrollBars="Vertical">
                         <div class="rounded_corners">
                        <asp:GridView ID="ChassisGrid" runat="server" AlternatingRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" GridLines="Horizontal"
                            HeaderStyle-Wrap="true" AllowPaging="true" Width="100%"
                            AutoGenerateColumns="false"
                            OnPageIndexChanging="ChassisGrid_PageIndexChanging">
                            <FooterStyle CssClass="GridViewFooterStyle" />
                            <RowStyle CssClass="GridViewRowStyle" />
                            <PagerStyle CssClass="GridViewPagerStyle" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                            <HeaderStyle CssClass="GridViewHeaderStyle" />
                            
                        <Columns>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="2%">
                                <ItemTemplate>
                                 <%--  <asp:Button ID="ImgSelect"  runat="server" ImageUrl="~/Images/arrowRight.png"  OnClientClick="return ReturnCRMChassisValue(this);"/> --%>     
                                <asp:Image ID="ImgSelect"  runat="server" ImageUrl="~/Images/arrowRight.png"  onClick="return ReturnCRMCustomerValue(this);"/>  
                                     
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cust_Type" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblCust_Type" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Cust_Type") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CRM_Cust_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblCRM_Cust_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CRM_Cust_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Global_Cust_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblGlobal_Cust_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Global_Cust_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Customer_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Existing_MTI_Cust" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblExisting_MTI_Cust" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Existing_MTI_Cust") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="add1" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lbladd1" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("add1") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="add2" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lbladd2" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("add2") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Pincode" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblpincode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("pincode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="state_id" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblstate_id" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("state_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="State" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblState" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("State") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="district_id" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lbldistrict_id" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("district_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Distict_Name" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblDistict_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Distict_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Region_id" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegion_id" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Region_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Region_Name" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegion_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Region_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="city" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblcity" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("city") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Country_Id" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblCountry_Id" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Country_Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Country_Name" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblCountry_Name" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Country_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhone" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Phone") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="E_mail" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl" >
                                <ItemTemplate>
                                    <asp:Label ID="lblE_mail" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("E_mail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Primary_Application_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl" >
                                <ItemTemplate>
                                    <asp:Label ID="lblPrimary_Application_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Primary_Application_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Secondary_Application_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl" >
                                <ItemTemplate>
                                    <asp:Label ID="lblSecondary_Application_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Secondary_Application_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prefix" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl" >
                                <ItemTemplate>
                                    <asp:Label ID="lblPrefix" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Prefix") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              </Columns> 
                            <HeaderStyle Wrap="True" />
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </div> 
                             </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>






