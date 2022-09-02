<%@ Page  Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" 
    Title="MTI-System Parameter Value"  Theme="SkinFile" CodeBehind="frmSystemParameterValue.aspx.cs" Inherits="MANART.Forms.Master.frmSystemParameterValue" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%--<%@ Register Src="../../WebParts/MultiselectLocation.ascx" TagName="Location" TagPrefix="uc6" %>--%>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <script src="../../Scripts/jsGridFunction.js"></script>
    <script src="../../Scripts/jsToolbarFunction.js"></script>
    <script src="../../Scripts/jsRFQFunction.js"></script>
    <script src="../../Scripts/jsMessageFunction.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable" border="1">
        <tr class="panel-heading">
            <td class="PageTitle panel-title" align="center">
                <asp:Label ID="lblTitle" runat="server" Text="Parameters Values"> </asp:Label>
            </td>
        </tr>
         <tr id="ToolbarPanel">
            <td style="width: 14%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server" OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       <tr>
            <td >
               <%-- <uc6:Location ID="Location" runat="server" OndrpCountryIndexChanged="Location_drpCountryIndexChanged" />--%>
           <uc2:Location ID="Location" runat="server" />
            </td>
       </tr>
        <tr id="TblControl">
            <td style="width: 14%">
            
                <asp:Panel ID="PDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    
                    <asp:Panel ID="CntModelDetails" runat="server" BorderColor="DarkGray" 
                        BorderStyle="Double" ScrollBars="None">
                        <asp:Panel ID="TtlDocDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                        &nbsp;</td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlDocDetails" runat="server" Height="15px" 
                                            ImageUrl="~/Images/Plus.png" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="CntDocDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <table id="tblRFPDetails" runat="server" class="ContainTable">
                            <tr>                            
                                <td style="width: 15%" class="tdLabel"></td>
                               
                                <td style="width: 15%" class="tdLabel">             
                                    
                                     <asp:Button ID="BtnShow" text="Show" runat="server" Width="76px" 
                                        onclick="BtnShow_Click" visible="false" />
                                </td>
                                
                                <td class="tdLabel" style="width: 15%">
                                    &nbsp;</td>
                                <td style="width: 18%">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                
                 <asp:Panel ID="PSystemParameterValueDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPESystemParameterValueDetails" runat="server" TargetControlID="CntSystemParameterValueDetails"
                        ExpandControlID="TtlSystemParameterValueDetails" CollapseControlID="TtlSystemParameterValueDetails"
                        Collapsed="true" ImageControlID="ImgTtlSystemParameterValueDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show System Parameter Value Details"
                        ExpandedText="Hide System Parameter Value Details" TextLabelID="lblSystemParameterValueDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlSystemParameterValueDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblSystemParameterValueDetails" runat="server" Text="System Parameter Value Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlSystemParameterValueDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntSystemParameterValueDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="Horizontal">
                           <asp:GridView ID="GrdSystemParameterValue" runat="server" AllowPaging="True" 
                        AlternatingRowStyle-Wrap="true" AutoGenerateColumns="False" 
                        EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" 
                        GridLines="Horizontal" SkinID="NormalGrid" 
                        Width="100%" OnRowCommand ="GrdSystemParameterValue_RowCommand"                         
                      >
                        <Columns>    
                          <asp:TemplateField HeaderText="Id" ItemStyle-Width="3%" Visible ="false" >
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" 
                                        Text='<%# Eval("ID") %>'> </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="3%" />
                            </asp:TemplateField>
                                                        
                             <asp:TemplateField HeaderText="Parameter Name" ItemStyle-Width="30%">
                                <ItemTemplate>
                                   <asp:Label ID="lblParameterName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("sys_para_desc") %>'> </asp:Label>
                                     </ItemTemplate>
                                <ItemStyle Width="30%" />
                                           
                               
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Value" ItemStyle-Width="10%"  >
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPara_Value" CssClass="GridTextBoxForAmount" runat="server" 
                                    Text='<%# Eval("Para_Value") %>' Width="100%"  
                                     ></asp:TextBox>    
                                    
                                     <%-- onkeypress="return CheckPercentageAmount(event,this);"   onblur="return CheckPercentageValue(event,this);"  --%>                                
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>  
                            <asp:TemplateField HeaderText="Active" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drpActive" runat ="server"  CssClass="DispalyNon" AutoPostBack="false">
                                    <asp:ListItem Value ="0" Text ="--Select--"></asp:ListItem>  
                                    <asp:ListItem Value ="Y" Text ="Y"></asp:ListItem>  
                                    <asp:ListItem Value ="N" Text ="N"></asp:ListItem>  
                                    </asp:DropDownList>                            
                                    <%--  <asp:Label ID="lblActive" runat="server" CssClass="LabelCenterAlign" 
                                        Text='<%# Eval("Active") %>'> </asp:Label>   --%>                               
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>    
                            <asp:TemplateField HeaderText="Editable" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drpEditable" runat ="server" CssClass="DispalyNon"  AutoPostBack="false">
                                    <asp:ListItem Value ="0" Text ="--Select--"></asp:ListItem>  
                                    <asp:ListItem Value ="Y" Text ="Y"></asp:ListItem>  
                                    <asp:ListItem Value ="N" Text ="N"></asp:ListItem>  
                                    </asp:DropDownList>                         
                                     <%-- <asp:Label ID="lblEditable" runat="server" CssClass="LabelCenterAlign" 
                                        Text='<%# Eval("Editable") %>'> </asp:Label> --%>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>               
                         
                        </Columns>
                        <EditRowStyle BorderColor="Black" Wrap="True" />
                        <AlternatingRowStyle Wrap="True" />
                    </asp:GridView>
                       
                    </asp:Panel>
                   
                </asp:Panel>
                
                <asp:Panel ID="PParameterValueDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntParameterValueDetails"
                        ExpandControlID="TtlParameterValueDetails" CollapseControlID="TtlParameterValueDetails"
                        Collapsed="true" ImageControlID="ImgTtlParameterValueDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Show Parameter Value Details"
                        ExpandedText="Hide Parameter Value Details" TextLabelID="lblParameterValueDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlParameterValueDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="ContaintTableHeader panel-title" width="96%">
                                    <asp:Label ID="lblParameterValueDetails" runat="server" Text="Parameter Value Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlParameterValueDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntParameterValueDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="Horizontal">
                           <asp:GridView ID="GrdParameterValue" runat="server" AllowPaging="True" 
                        AlternatingRowStyle-Wrap="true" AutoGenerateColumns="False" 
                        EditRowStyle-BorderColor="Black" EditRowStyle-Wrap="true" 
                        GridLines="Horizontal" SkinID="NormalGrid" 
                        Width="100%"  OnRowCommand ="GrdParameterValue_RowCommand">                  
                        <Columns>    
                          <asp:TemplateField HeaderText="Id" ItemStyle-Width="3%" Visible ="false" >
                                <ItemTemplate>
                                    <asp:Label ID="lblID" runat="server" CssClass="LabelCenterAlign" 
                                        Text='<%# Eval("ID") %>'> </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="3%" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Parameter Name" ItemStyle-Width="30%">
                                <ItemTemplate>
                                   <asp:Label ID="lblParameterName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("sys_para_desc") %>'> </asp:Label>
                                     </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="Flag Value" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drpFlagValue" runat ="server" CssClass="DispalyNon" AutoPostBack="false">
                                    <asp:ListItem Value ="0" Text ="--Select--"></asp:ListItem>  
                                    <asp:ListItem Value ="Y" Text ="Y"></asp:ListItem>  
                                    <asp:ListItem Value ="N" Text ="N"></asp:ListItem>  
                                    </asp:DropDownList>                            
                                 <%--     <asp:Label ID="lblFlagValue" runat="server" CssClass="LabelCenterAlign" 
                                        Text='<%# Eval("flag_value") %>'> </asp:Label>   --%>                                
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>    
                            <asp:TemplateField HeaderText="Active" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drpActive" runat ="server"  CssClass="DispalyNon" AutoPostBack="false">
                                    <asp:ListItem Value ="0" Text ="--Select--"></asp:ListItem>  
                                    <asp:ListItem Value ="Y" Text ="Y"></asp:ListItem>  
                                    <asp:ListItem Value ="N" Text ="N"></asp:ListItem>  
                                    </asp:DropDownList>                            
                                         <%--  <asp:Label ID="lblActive" runat="server" CssClass="LabelCenterAlign" 
                                        Text='<%# Eval("Active") %>'> </asp:Label> --%>                          
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>    
                            <asp:TemplateField HeaderText="Editable" ItemStyle-Width="10%">
                                <ItemTemplate>
                                      <asp:DropDownList ID="drpEditable" runat ="server" CssClass="DispalyNon" AutoPostBack="false">
                                       <asp:ListItem Value ="0" Text ="--Select--"></asp:ListItem>  
                                    <asp:ListItem Value ="Y" Text ="Y"></asp:ListItem>  
                                    <asp:ListItem Value ="N" Text ="N"></asp:ListItem>  
                                    </asp:DropDownList>                         
                                      <%--  <asp:Label ID="lblEditable" runat="server" CssClass="LabelCenterAlign" 
                                        Text='<%# Eval("Editable") %>'> </asp:Label>    --%>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>               
                        </Columns>
                        <EditRowStyle BorderColor="Black" Wrap="True" />
                        <AlternatingRowStyle Wrap="True" />
                    </asp:GridView>
                       
                    </asp:Panel>
                   
                </asp:Panel>
            </td>
        </tr>
        </table>
</asp:Content>
