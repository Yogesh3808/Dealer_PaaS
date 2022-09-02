<%@ Page Title="M1 (Enquiry Generated)" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true"   MaintainScrollPositionOnPostback="true"
     CodeBehind="frmM1MTI.aspx.cs" Inherits="MANART.Forms.VehicleSales.frmM1MTI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="../../WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>--%>
  <%@ Register Src="~/WebParts/MTILocation.ascx" TagPrefix="uc2" TagName="Location" %>
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
    <script type="text/javascript">
        $(document).ready(function () {
            var DetailsGrid = document.getElementById("<%=DetailsGrid.ClientID%>");
            if (DetailsGrid != null) {
                var Elements = DetailsGrid.getElementsByTagName("input");
            }
            objDate = new Date();

            if (Elements != null) {
                var splDate = '';
                var dt = '';
                var Today = new Date(objDate.getFullYear(), objDate.getMonth + 1, objDate.getDay)
                for (var i = 0; i < Elements.length; i++) {


                    if (Elements[i].type == 'text' && Elements[i].id.indexOf("txtNextObjDate") != -1) {
                        splDate = Elements[i].value.split("/")
                        dt = new Date(splDate[2], splDate[1] - 1, splDate[0]);
                        $('#' + Elements[i].id).datepick({
                            //                    dateFormat: 'dd/mm/yyyy', minDate: (Today > dt) ? '0d' : Elements[i].value
                            //   dateFormat: 'dd/mm/yyyy', minDate: '0d'
                            dateFormat: 'dd/mm/yyyy'
                        });


                    }
                }
            }

        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            var txtDocDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
            var DetailsGrid = document.getElementById("<%=DetailsGrid.ClientID%>");
            if (DetailsGrid != null) {
                var Elements = DetailsGrid.getElementsByTagName("input");
            }
            objDate = new Date();

            if (Elements != null) {
                var splDate = '';
                var dt = '';
                var Today = new Date(objDate.getFullYear(), objDate.getMonth + 1, objDate.getDay)
                for (var i = 0; i < Elements.length; i++) {
                    if (Elements[i].type == 'text' && Elements[i].id.indexOf("txtObjDate") != -1) {
                        splDate = Elements[i].value.split("/")
                        dt = new Date(splDate[2], splDate[1] - 1, splDate[0]);
                        $('#' + Elements[i].id).datepick({
                            //                    dateFormat: 'dd/mm/yyyy', minDate: (Today > dt) ? '0d' : Elements[i].value
                            //     dateFormat: 'dd/mm/yyyy', minDate: '0d'
                            dateFormat: 'dd/mm/yyyy', 
                        });


                    }
                }
            }

        });
    </script>

   

    <script type="text/javascript">
        function pageLoad() {
            $(document).ready(function () {



                var txtLikelydate = document.getElementById("ContentPlaceHolder1_txtLikelydate_txtDocDate");


                //var txtDocDate = document.getElementById("ContentPlaceHolder1_txtDocDate_txtDocDate");
                //$('#ContentPlaceHolder1_txtDocDate_txtDocDate').datepick({
                //    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                //    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value, maxDate: '0d'
                //});

                var txtLikelydate = document.getElementById("ContentPlaceHolder1_txtLikelydate_txtDocDate");
                $('#ContentPlaceHolder1_txtLikelydate_txtDocDate').datepick({
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="table-responsive">
        <table id="PageTbl" class="PageTable" border="1">
        <tr id="TitleOfPage">
            <td class="PageTitle" align="center" style="width: 14%">
                <asp:Label ID="lblTitle" runat="server" Text="M1 (Enquiry Generated)"> </asp:Label>
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
                    <asp:Panel ID="LocationDetails" runat="server" BorderColor="Black" BorderStyle="Double">
                    <uc2:Location ID="Location" runat="server" />
                </asp:Panel>
                
                 <asp:Panel ID="PnlPendingDocument" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                   <UCPDoc:PendingDocument ID="PDoc" runat="server" />
                   <%--<uc4:SearchGridView ID="PDoc" runat="server" bIsCallForServer="true" OnImage_Click="SearchImage_Click" />--%>
                </asp:Panel>
              
                <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                    <uc4:SearchGridView ID="SearchGrid" runat="server" bIsCallForServer="true" OnImage_Click="SearchImage_Click" />
                </asp:Panel>
                <asp:Panel ID="M0Details" runat="server" BorderColor="Black" BorderStyle="Double">
                      <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="CntM0"
                            ExpandControlID="TtlM0" CollapseControlID="TtlM0" Collapsed="true"
                            ImageControlID="ImgTtlM0Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M0 (General Discussion)" ExpandedText="M0 (General Discussion)"
                            TextLabelID="lblTtlM0">
                        </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlM0" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM0" runat="server" Text="M0 (General Discussion) Details" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlM0Details" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                     <asp:Panel ID="CntM0" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                        <table id="Table4" runat="server" class="ContainTable" border="1">
                       <%-- <tr class="panel-heading">
                            <td align="center" class="panel-title" style="height: 15px" colspan="6">
                                M0 Details
                            </td>
                        </tr>--%>
                            <tr>

                                 <td>
                                        <asp:Label ID="Label32" runat="server" Text="M0 No:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                    <asp:TextBox ID="txtM0" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                               
                                    </td>
                                    <td>
                                    <asp:Label ID="Label33" runat="server" Text="M0 Date:" CssClass="tdLabel"></asp:Label>
                                    </td>
                                    <td>
                                    <uc3:CurrentDate ID="txtM0Date" runat="server" Mandatory="false" bCheckforCurrentDate="false" />
                                    </td>
                                  



                                <td style="width: 15%" class="tdLabel">
                                   Type:
                                </td>
                                <td style="width: 15%">
                                 <%--    <asp:DropDownList ID="drpCustType" runat="server"  CssClass="ComboBoxFixedSize" AppendDataBoundItems="true"
                                      onBlur="CheckcustType(this,'DrpCustomerType')" onselectedindexchanged="drpCustType_SelectedIndexChanged"  
                                         
                                         AutoPostBack="true"  >
                                    </asp:DropDownList>--%>
                                    <asp:DropDownList ID="drpM0CustType" runat="server"  AutoPostBack="True" 
                                        CssClass="ComboBoxFixedSize" ></asp:DropDownList>
                                     <asp:Label ID="lblMandatory" runat="server" CssClass="Mandatory" Font-Bold="true"></asp:Label>
                                </td>
                                                    
                               
                            </tr>
                            <tr>
                                <td style="width: 15%" class="tdLabel">
                                   Title:
                                </td>
                                <td style="width: 15%">
                                 
                                    <asp:DropDownList ID="drpTitle" runat="server"  AutoPostBack="True" 
                                        CssClass="ComboBoxFixedSize"></asp:DropDownList>
                                     <asp:Label ID="Label25" runat="server" CssClass="Mandatory"  Font-Bold="true"></asp:Label>
                                </td>
                                
                                <td style="width: 15%" class="tdLabel">
                                   Name:
                                </td>
                                <td style="width: 15%">
                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                         <asp:Label ID="Label26" runat="server" CssClass="Mandatory"  Font-Bold="true"></asp:Label>
                                </td>

                                 <td>
                                <asp:Label ID="Label20" runat="server" Text="Existing MTI Customer:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpIsMTICust" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true" AutoPostBack="True">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="2">N</asp:ListItem>
                                </asp:DropDownList>
                                
                            </td>
                                
                            </tr>
                            <tr>
                                
                                
                                 <td style="width: 15%" class="tdLabel"  >
                                    Address 1 :
                                </td>
                                <td style="width: 15%" >                               
                                  
                                       <asp:TextBox ID="txtAddress1" runat="server" CssClass="TextBoxForString"  
                                        Text=""  ></asp:TextBox>
                                         <asp:Label ID="Label27" runat="server" CssClass="Mandatory"  Font-Bold="true"></asp:Label>
                                 
                                </td>   
                                 <td style="width: 15%" class="tdLabel"  >
                                    Address 2 :
                                </td>
                                <td style="width: 15%" >                               
                                  
                                       <asp:TextBox ID="txtAddress2" runat="server" CssClass="TextBoxForString"  
                                        Text=""  ></asp:TextBox>
                                        
                                 
                                </td>
                                 <td style="width: 15%" class="tdLabel">
                                    Pincode:
                                </td>
                                <td style="width: 18%">
                                    
                                    <asp:TextBox ID="txtpincode" runat="server" CssClass="TextBoxForString" 
                                        Text=""></asp:TextBox>
                                </td>  
                            </tr>
                            <tr>
                           
                             
                           
                              
                                <td style="width: 15%" class="tdLabel">
                                    State:
                                </td>
                                <td style="width: 18%">
                                    
                                       <asp:DropDownList ID="drpState" runat="server"  AutoPostBack="True" 
                                        CssClass="ComboBoxFixedSize" >
                                    </asp:DropDownList>
                                    <asp:Label ID="Label28" runat="server" CssClass="Mandatory" Font-Bold="true"></asp:Label>
                                   
                                </td>
                                
                                <td style="width: 15%" class="tdLabel">
                                    District:
                                </td>
                                <td style="width: 18%">
                                    
                                       <asp:DropDownList ID="drpDistrict" runat="server"  AutoPostBack="True" 
                                        CssClass="ComboBoxFixedSize" 
                                           >
                                    </asp:DropDownList>
                                    <asp:Label ID="Label29" runat="server" CssClass="Mandatory"  Font-Bold="true"></asp:Label>
                                   
                                </td>
                                
                                
                                
                                
                            <td style="width: 15%" class="tdLabel">
                                    Region:
                                </td>
                                <td style="width: 18%">
                                   
                                        <asp:DropDownList ID="drpRegion" runat="server" 
                                        CssClass="ComboBoxFixedSize"   
                                            Enabled="false">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label30" runat="server" CssClass="Mandatory" Font-Bold="true"></asp:Label>
                                  
                                </td>
                                           
                                
                            </tr>
                            <tr>
                                 <td style="width: 15%" class="tdLabel">
                                    City:
                                </td>
                                <td style="width: 18%">
                                   
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="TextBoxForString" 
                                        Text="" ></asp:TextBox>
                                         <asp:Label ID="Label31" runat="server" CssClass="Mandatory" Font-Bold="true"></asp:Label>
                                   
                                </td>  
                                <td class="tdLabel" style="width: 15%">
                                    Country:
                                </td>
                                <td style="width: 18%">
                                   
                                        <asp:TextBox ID="txtCountry" runat="server" CssClass="TextBoxForString" 
                                        Text="India" ReadOnly ="true" ></asp:TextBox>
                                   
                                </td>
                                 <td style="width: 15%" class="tdLabel">
                                    Mobile:
                                </td>
                                <td style="width: 18%">
                                  
                                     <asp:TextBox ID="txtMobile" runat="server" CssClass="TextBoxForString" MaxLength =12
                                        Text="" onkeypress="return CheckForTextBoxValue(event,this,'8');"></asp:TextBox>
                                    
                                </td>
                             
                                </tr>
                                <tr>
                                 
                                 
                               <td class="tdLabel" style="width: 15%">
                                        Email:
                                    </td>
                                    <td style="width: 18%">
                                       
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="TextBoxForString" 
                                             Text=""></asp:TextBox>
                                      
                                    </td>
                                    <td class="auto-style5">Primary Application:
                                </td>                              

                                <td class="auto-style6">
                                <asp:DropDownList ID="drpM0PriApp" runat="server"
                                CssClass="ComboBoxFixedSize" AutoPostBack="True" OnSelectedIndexChanged="drpM0PriApp_SelectedIndexChanged">
                                </asp:DropDownList>
                                </td>
                            
                                <td class="auto-style5">Secondary Application:
                                </td>
                                <td class="auto-style6">
                                <asp:DropDownList ID="drpM0SecApp" runat="server"
                                CssClass="ComboBoxFixedSize" >
                                </asp:DropDownList>
                                </td>

                                    
                                
                               
                                </tr>
                            <tr>
                                                            
                              
                                    <td class="tdLabel">
                                Financier of the Customer:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpM0Financier" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                
                            </td>
                                   <td>
                                    <asp:Label ID="Label22" runat="server" Text="Body Builder Details:" CssClass=""></asp:Label>
                                </td>

                                <td style="width: 15%">
                                    <asp:TextBox ID="txtBodyBuilder" runat="server" CssClass="TextBoxForString"
                                        Text=""></asp:TextBox>
                                </td>
                              </tr>
                              
                              
                            
                          
                                 
                        </table>
                         </asp:Panel>
                    </asp:Panel>
                
                <asp:Panel ID="DocNoDetails" runat="server" BorderColor="Black" BorderStyle="Double">

                      <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="CntM1"
                            ExpandControlID="TtlM1" CollapseControlID="TtlM1" Collapsed="false"
                            ImageControlID="ImgTtlM1Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M1  (Enquiry Generated) Details" ExpandedText="M1  (Enquiry Generated) Details"
                            TextLabelID="lblTtlM1">
                        </cc1:CollapsiblePanelExtender>

                     <asp:Panel ID="TtlM1" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlM1" runat="server" Text="M1 (Enquiry Generated)" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlM1Details" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                         <asp:Panel ID="CntM1" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                    <table id="txtDocNoDetails" runat="server" class="ContainTable" border="1">
                        <%-- <tr class="panel-heading">
                            <td align="center" class="panel-title" style="height: 15px" colspan="6">
                                M1 (Enquiry Generated)
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <asp:Label ID="lblLeadNo" runat="server" Text="M1 No:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLeadNo" runat="server" CssClass="TextBoxForString" Text="" onkeydown="return ToSetKeyPressValueFalse(event,this);"></asp:TextBox>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblLeadDate" runat="server" Text="M1 Date:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <uc3:CurrentDate ID="txtDocDate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />

                            </td>
                            <td class="tdLabel">
                                Source:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpInqSource" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label16" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                            </td>
                        </tr>
                       
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Source Name:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSourceName" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                <%--<asp:Label ID="Label17" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Source Address:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSourceAdd" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                               <%--<asp:Label ID="Label18" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Source Mobile No:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSourceMob" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                 <%--<asp:Label ID="Label19" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                Area:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpArea" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label21" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                               
                            </td>
                            <td class="tdLabel">
                                Attended By:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpAttendedby" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label23" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                               
                            </td>
                            <td class="tdLabel">
                                Allocated To:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpAlloatedTo" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label24" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                              
                            </td>
                        </tr>
                      <tr>

                                <td class="tdLabel">
                                PO Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpPOType" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label34" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                             
                            </td>
                                <td class="tdLabel">
                                Special Packages Required:
                            </td>
                          <td>
                                <asp:DropDownList ID="drpSpecialPackage" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    OnSelectedIndexChanged="drpSpecialPackage_SelectedIndexChanged"
                                    EnableViewState="true" AutoPostBack="True">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="2">N</asp:ListItem>
                                </asp:DropDownList>
                                
                        </td>
                               <%-- <td>
                                    <asp:Button ID="bConfirm" OnClick="bConvertToM2" Text="Convert To M2" runat="server" Visible="false" CssClass="ComboBoxFixedSize"></asp:Button>
                                </td>--%>
                          <td>
                                <asp:Label ID="Label12" runat="server" Text="AMC Details:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAMCDet" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                               
                            </td>

                            </tr>
                        <tr>
                             

                              <td>
                                <asp:Label ID="Label13" runat="server" Text="Special Warranty Details:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSpWarrDet" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                
                            </td>
                             <td>
                                <asp:Label ID="Label14" runat="server" Text="Extended Warranty Details:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExWarrDet" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                
                            </td>
                             <td>
                                <asp:Label ID="Label15" runat="server" Text="Other Details:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOthersDet" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                
                            </td>

                        </tr>
                          <tr>

                                 <td>
                                <asp:Label ID="Label50" runat="server" Text="Enquiry No:" Font-Bold="true" CssClass="tdLabel"></asp:Label>
                                     
                            </td>
                            <td>
                                <asp:TextBox ID="txtEnquiryNo" Text="" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>
                                
                            </td>
                           </tr>
                           
                    </table>
                </asp:Panel>
                     </asp:Panel>

<%--                  <asp:UpdatePanel UpdateMode="Conditional" ID="UPObj" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>--%>
                    <asp:Panel ID="PObjectiveDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CPEObjectiveDetails" runat="server" TargetControlID="CntObjectiveDetails"
                        ExpandControlID="TtlObjectiveDetails" CollapseControlID="TtlObjectiveDetails"
                        Collapsed="false" ImageControlID="ImgTtlObjectiveDetails" ExpandedImage="~/Images/Minus.png"
                        CollapsedImage="~/Images/Plus.png" SuppressPostBack="true" CollapsedText="Visit Details"
                        ExpandedText="Visit Details" TextLabelID="lblTtlObjectiveDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlObjectiveDetails" runat="server">
                        <table width="100%">
                            <tr class="panel-heading">
                                <td align="center" class="panel-title">
                                    <asp:Label ID="lblTtlObjectiveDetails" runat="server" Text="Visit Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                </td>
                                <td width="1%">
                                    <asp:Image ID="ImgTtlObjectiveDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntObjectiveDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="DetailsGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered table-hover"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="DetailsGrid_RowCommand">
                            <Columns>
                                <%--<asp:TemplateField HeaderText="No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNo" runat="server" CssClass="LabelCenterAlign" Text="<%# Container.DataItemIndex + 1  %>">
                                        </asp:Label>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                    ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtObjID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Visit Details" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpVisitObj" runat="server" CssClass="GridComboBoxFixedSize">
                                            <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <uc3:CurrentDate ID="txtObjDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Discussion/Result" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDiscussion" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Time Spent" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTimeSpent" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Next Visit Details" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpNextObj" runat="server" CssClass="GridComboBoxFixedSize">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <uc3:CurrentDate ID="txtNextObjDate" runat="server" Mandatory="false" bCheckforCurrentDate="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Commitment To Customer" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCommitment" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="New/Cancel" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkForDelete" runat="server" />
                                        <asp:LinkButton ID="lnkNew" OnClientClick="return CheckRowValueLead(event,this);"
                                            runat="server">New</asp:LinkButton>
                                        <asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" 
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
<%--</ContentTemplate>
         </asp:UpdatePanel>  --%>                              
         <%--    <asp:UpdateProgress ID="UPRObject" runat="server" AssociatedUpdatePanelID="UPObj">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>  --%>



                <asp:Panel ID="Panel3" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="Table2" runat="server" class="ContainTable" border="1">
                         <tr class="panel-heading">
                            <td align="center" class="panel-title" style="height: 15px" colspan="6">
                                Model
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                Model Category:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelCat" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="drpModelGroup_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label35" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                            </td>
                             
                             <td class="tdLabel">
                                Model Code:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelCode" runat="server"  AutoPostBack="True"  OnSelectedIndexChanged="drpModelCode_SelectedIndexChanged" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label37" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                              
                            </td>

                            
                              <td >
                                 <asp:Label ID="Modelgrp" Text="Model Group:" runat="server" Visible="false" ></asp:Label>
                                
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModelGroup" runat="server" Visible="false"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label36" Text="*" Visible="false" runat="server" CssClass="Mandatory"></asp:Label>
                               
                            </td>
                        </tr>
                        <tr>
                            
                           
                            
                            <td class="tdLabel">
                                Model:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpModel" runat="server"   AutoPostBack="True"  OnSelectedIndexChanged="drpModel_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label38" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                              
                            </td>
                            <td class="tdLabel">
                                Quantity:
                            </td>
                            <td>
                                 <asp:TextBox ID="txtQty" runat="server"     onkeypress=" return CheckForTextBoxValue(event,this,'5');" CssClass="TextForAmount"></asp:TextBox>
                                <asp:Label ID="Label39" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
                  <asp:UpdatePanel UpdateMode="Conditional" ID="UpFleet" runat="server" ChildrenAsTriggers="true">
                         <ContentTemplate>
                    <asp:Panel ID="PFleetDet" runat="server" BorderColor="Black" BorderStyle="Double"
                    class="">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntFleetDetails"
                        ExpandControlID="TtlFleetDetails" CollapseControlID="TtlFleetDetails" Collapsed="false"
                        ImageControlID="ImgTtlFleetDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Fleet Details" ExpandedText="Fleet Details"
                        TextLabelID="lblTtlFleetDetails">
                    </cc1:CollapsiblePanelExtender>
                    

                         <asp:Panel ID="TtlFleetDetails" runat="server">
                            <table width="100%">
                                <tr class="panel-heading">
                                    <td align="center" class="panel-title">
                                        <asp:Label ID="lblTtlFleetDetails" runat="server" Text="Fleet Details" Width="96%" onmouseover="SetCancelStyleonMouseOver(this);"
                                            onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlFleetDetails" runat="server" ImageUrl="~/Images/Plus.png" Height="15px"
                                            Width="123%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                    <asp:Panel ID="CntFleetDetails" runat="server" BorderColor="Black" BorderStyle="Double"
                        ScrollBars="None">
                        <asp:GridView ID="FleetDtls" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-condensed table-bordered"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" > 
                            <Columns>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                    ItemStyle-CssClass="HideControl">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFleetID" runat="server" Text='<%# Eval("ID") %>' Width="3"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Competitor No" ItemStyle-Width="15%" HeaderStyle-CssClass="HideControl"
                                    ItemStyle-CssClass="HideControl" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompNo" runat="server"                                             
                                           
                                            CssClass="LabelCenterAlign" Text='<%# Eval("CompID")  %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>      
                                    
                                                                 
                                    <asp:TemplateField HeaderText="Manufacturer" ItemStyle-Width="17%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompName" runat="server" CssClass="LabelCenterAlign" Text='<%# Eval("Competitor_Name") %>'> </asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="17%" />
                                    </asp:TemplateField>      
                                    
                                           
                                 <%--<asp:TemplateField HeaderText="Bus" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtHDTQty" runat="server" EnableViewState="false" CssClass="TextForAmount" 
                                                
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');"> </asp:TextBox>
                                            <%--<asp:Label ID="lblHDTQty" runat="server" CssClass="LabelCenterAlign" EnableViewState ="false" > </asp:Label>--%>
                                            <%--Text='<%# Eval("HDTQty","{0:#0}") %>'--%>
                                       <%-- </ItemTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Model" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpFleetModel1" runat="server" CssClass="GridComboBoxFixedSize">
                                            <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Qty" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty1" runat="server" CssClass="TextForAmount"
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');" EnableViewState ="false"> </asp:TextBox>
                                        </ItemTemplate>  <%--Text='<%# Eval("LDTQty","{0:#0}") %>'--%>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>     
                                     <asp:TemplateField HeaderText="Model" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpFleetModel2" runat="server" CssClass="GridComboBoxFixedSize">
                                            <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>  

                                <asp:TemplateField HeaderText="Qty" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty2" runat="server" CssClass="TextForAmount"
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');" EnableViewState ="false"> </asp:TextBox>
                                        </ItemTemplate>  <%--Text='<%# Eval("LDTQty","{0:#0}") %>'--%>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>   

                                 <asp:TemplateField HeaderText="Model" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="drpFleetModel3" runat="server" CssClass="GridComboBoxFixedSize">
                                            <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>  

                                <asp:TemplateField HeaderText="Qty" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQty3" runat="server" CssClass="TextForAmount"
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');" EnableViewState ="false"> </asp:TextBox>
                                        </ItemTemplate>  <%--Text='<%# Eval("LDTQty","{0:#0}") %>'--%>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>   
                                
                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
                             </ContentTemplate>
                         </asp:UpdatePanel>
                    
                     <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpFleet">
                        <ProgressTemplate>
                            Inserting Record ......
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                
                <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="tblProformaDetails2" runat="server" class="ContainTable" border="1">
                        <tr class="panel-heading">
                            <td align="center" class="panel-title" style="height: 15px" colspan="8">
                                Profile Details
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                Customer Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpCustType" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                <%--<b class="Mandatory">*</b>--%>
                                 <%--<asp:Label ID="Label40" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                            </td>
                            <td class="tdLabel">
                                Industry Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpIndustryType" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                <%--<b class="Mandatory">*</b>--%>
                                 <%--<asp:Label ID="Label41" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                            </td>
                            <td class="tdLabel">
                                Drive Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpDriveType" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <%--<asp:Label ID="Label42" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                                <%--<b class="Mandatory">*</b>--%>
                            </td>
                            <td class="tdLabel">
                                Load Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpLoadType" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <%--<asp:Label ID="Label43" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                              
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                Primary Application:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpPrimaryApplication" runat="server" CssClass="ComboBoxFixedSize"
                                    AutoPostBack="True" OnSelectedIndexChanged="drpPrimaryApplication_SelectedIndexChanged">
                                </asp:DropDownList>
                                 <%--<asp:Label ID="Label44" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                               
                            </td>
                            <td class="tdLabel">
                                Secondary Application:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpSeconadryApplication" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <%--<asp:Label ID="Label45" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                              
                            </td>
                            <td class="tdLabel">
                                Road Type
                            </td>
                            <td>
                                <asp:DropDownList ID="drpRoadType" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <%--<asp:Label ID="Label46" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                             
                            </td>
                            <td class="tdLabel">
                                Route Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpRouteType" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                <%--<b class="Mandatory">*</b>--%>
                                 <%--<asp:Label ID="Label47" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                Financier:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpFinancierType" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                <%--<b class="Mandatory">*</b>--%>
                                 <%--<asp:Label ID="Label48" Text="*" runat="server" CssClass="Mandatory"></asp:Label>--%>
                            </td>
                            <td class="tdLabel">
                            </td>
                            <td>
                            </td>
                            <td class="tdLabel">
                            </td>
                            <td>
                            </td>
                            <td class="tdLabel">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel4" runat="server" BorderColor="Black" BorderStyle="Double">
                    <table id="Table3" runat="server" class="ContainTable" border="1">
                        <tr class="panel-heading">
                            <td align="center" class="panel-title" style="height: 15px" colspan="4">
                                Additional Information
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Likely Buy Date:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <uc3:CurrentDate ID="txtLikelydate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />
                                
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Likely Buy Brand:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpLikelyBuyBrand" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                                 <asp:Label ID="Label49" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                              
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Load/Permit Availability:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpLoadPermit" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true" AutoPostBack="True">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="2">N</asp:ListItem>
                                </asp:DropDownList>
                                
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Tie Up with Body Builder:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpBodyBuilder" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    EnableViewState="true" AutoPostBack="True">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="2">N</asp:ListItem>
                                </asp:DropDownList>
                            
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Tie Up with Local Mechanic:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpLocalMech" runat="server" Width="100px" OnSelectedIndexChanged="drpLocalMech_SelectedIndexChanged"
                                    CssClass="ComboBoxFixedSize" EnableViewState="true" AutoPostBack="True">
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="2">N</asp:ListItem>
                                </asp:DropDownList>
                               
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Mechanic Name:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMechName" runat="server" CssClass="TextBoxForString" Text=""
                                    ></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Mechanic Address:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMechAdd" runat="server" CssClass="TextBoxForString" Text="" ></asp:TextBox>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Mechanic Phone:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMechPhone" runat="server" CssClass="TextBoxForString" Text=""
                                   ></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
               
                
                
                <asp:Panel ID="PClosure" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                    class="ContaintTableHeader">
                    <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="CntClosureDetails"
                        ExpandControlID="TtlClosureDetails" CollapseControlID="TtlClosureDetails" Collapsed="false"
                        ImageControlID="ImgTtlClosureDetails" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                        SuppressPostBack="true" CollapsedText="Loss Details" ExpandedText="Loss Details"
                        TextLabelID="lblTtlClosureDetails">
                    </cc1:CollapsiblePanelExtender>
                    <asp:Panel ID="TtlClosureDetails" runat="server">
                        <table width="100%">
                             <tr class="panel-heading">
                                <td align="center" class="panel-title" width="96%">
                                    <asp:Label ID="lblTtlClosureDetails" runat="server" Text="Loss Details" Width="96%"
                                        onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></td><td width="1%">
                                    <asp:Image ID="ImgTtlClosureDetails" runat="server" ImageUrl="~/Images/Plus.png"
                                        Height="15px" Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="CntClosureDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        ScrollBars="None">
                       
                        
                        <asp:GridView ID="ClosureGrid" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered table-hover"
                            Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                            EditRowStyle-BorderColor="Black" AutoGenerateColumns="False" OnRowCommand="ClosureGrid_RowCommand" >
                            <Columns>
                                <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtClosureID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox></ItemTemplate></asp:TemplateField>
                                <asp:TemplateField HeaderText="Loss Reason" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpCloseRsn"  AutoPostBack="true" OnSelectedIndexChanged="drpCloseRsn_SelectedIndexChanged" runat="server" CssClass="GridComboBoxFixedSize">
                                                <%--onselectedindexchanged="drpBodyType_SelectedIndexChanged" AutoPostBack ="True"--%></asp:DropDownList>

                                        </ItemTemplate></asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Competitor" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="drpCloseCompetitor" runat="server" CssClass="GridComboBoxFixedSize">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Competitor Make" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCompetitor" runat="server" CssClass="GridTextBoxForString" Width="60%"></asp:TextBox></ItemTemplate><ItemStyle Width="10%" />
                                    </asp:TemplateField>

                                <asp:TemplateField HeaderText="Qty" ItemStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCompQty" runat="server" CssClass="TextForAmount"
                                             onkeypress=" return CheckForTextBoxValue(event,this,'6');" EnableViewState ="true"> </asp:TextBox>
                                        </ItemTemplate>  <%--Text='<%# Eval("LDTQty","{0:#0}") %>'--%>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>   


                                    <asp:TemplateField HeaderText="New/Cancel" ItemStyle-Width="10%" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkForDelete" runat="server" />
                                            <asp:LinkButton ID="lnkNew" OnClientClick="return CheckRowValueLead(event,this);"
                                                runat="server">New</asp:LinkButton><asp:Label ID="lblCancel" runat="server" ForeColor="#49A3D3" Text="Cancel" 
                                                onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label></ItemTemplate><ItemStyle Width="10%" />
                                    </asp:TemplateField>

                            </Columns>
                            <EditRowStyle BorderColor="Black" Wrap="True" />
                            <AlternatingRowStyle Wrap="True" />
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
            </td>
        </tr>
        <tr id="TmpControl">
            <td style="width: 14%">
                <asp:TextBox ID="txtControlCount" CssClass="DispalyNon" runat="server" Width="1px"
                    Text=""></asp:TextBox><asp:TextBox ID="txtFormType" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtDealerId" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtVelInDtlID" runat="server" CssClass="DispalyNon" Width="1px"
                    Text=""></asp:TextBox><asp:TextBox ID="txtPreviousDocId" runat="server" CssClass="DispalyNon" Width="1px"
                     Text=""></asp:TextBox><asp:TextBox ID="txtDoneBy" runat="server" CssClass="DispalyNon" Width="1px"
                     Text=""></asp:TextBox>
                 <asp:TextBox ID="txtAppNo" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>

                <asp:TextBox ID="txtM0ID" runat="server" CssClass="DispalyNon" Width="1px"
                     Text=""></asp:TextBox><asp:TextBox ID="txtCustID" runat="server" CssClass="DispalyNon" Width="1px"
                     Text=""></asp:TextBox><asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
             <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                 <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                     <asp:HiddenField ID="hdnGSTDoc" runat="server" Value="" />
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
