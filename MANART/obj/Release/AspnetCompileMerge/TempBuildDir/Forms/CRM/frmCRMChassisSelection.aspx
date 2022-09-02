<%@ Page Title="MTI-Chassis Selection" Language="C#" AutoEventWireup="true" CodeBehind="frmCRMChassisSelection.aspx.cs" Inherits="MANART.Forms.CRM.frmCRMChassisSelection" %>
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
 <%-- //  <script src="../../Scripts/jsShowForm.js"></script>--%>
    <script type="text/javascript">
        function ReturnCRMChassisValue(objImgControl) {
            //debugger;
            var objRow = objImgControl.parentNode.parentNode.childNodes;
            var sValue;
            var ArrOfChassis = new Array();
            for (var cnt = 1; cnt < objRow.length - 1; cnt++) {
               // sValue = objRow[cnt].innerHTML.trim();
                sValue = objRow[cnt].innerText.trim();
                ArrOfChassis.push(sValue);
            }
            //alert(ArrOfChassis);
            window.returnValue = ArrOfChassis;
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
                                    <asp:ListItem Selected="True" Value="Chassis_no">Chassis No</asp:ListItem>                                                                       
                                    <asp:ListItem Value ="Reg_No">Vehicle Reg No</asp:ListItem>
                                    <asp:ListItem Value ="Customer_name">Customer Name</asp:ListItem>
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
                                <asp:Image ID="ImgSelect"  runat="server" ImageUrl="~/Images/arrowRight.png"  onClick="return ReturnCRMChassisValue(this);"/>  
                                     
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Chassis_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Chassis No." ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblChassisNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Chassis_no") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Engine No" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblEngineNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Engine_no") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Vehicle No" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblVehicleNo" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Reg_No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Model_ID" ItemStyle-Width="1%" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Model Code" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblModelCode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                              
                            <asp:TemplateField HeaderText="Model Name" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblModelName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Model_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                              <asp:TemplateField HeaderText="CRM_Cust_ID" HeaderStyle-CssClass="HideControl"
                                                ItemStyle-CssClass="HideControl">
                                <ItemTemplate>
                                    <asp:Label ID="lblCRM_Cust_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("CRM_Cust_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Customer_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                             
                            <asp:TemplateField HeaderText="Phone" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhone" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Phone") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField HeaderText="pincode " ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblpincode" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("pincode ") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                                                    
                            <asp:TemplateField HeaderText="city" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblcity" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("city") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="State" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblState" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("State") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Global_Cust_ID" ItemStyle-CssClass="HideControl"
                                HeaderStyle-CssClass="HideControl" >
                                <ItemTemplate>
                                    <asp:Label ID="lblGlobal_Cust_ID" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Global_Cust_ID") %>'></asp:Label>
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




