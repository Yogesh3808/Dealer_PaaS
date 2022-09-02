    <%@ Page Title=" M2 (Quotation Submitted)" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmM2.aspx.cs" 
          EnableEventValidation="false" Inherits="MANART.Forms.VehicleSales.frmM2" MaintainScrollPositionOnPostback="true" %>


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
                            // dateFormat: 'dd/mm/yyyy', minDate: '0d'
                            dateFormat: 'dd/mm/yyyy'
                        });


                    }
                }
            }

            });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            var txtDocDate = document.getElementById("ContentPlaceHolder1_txtM2Date_txtDocDate");
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
                            // dateFormat: 'dd/mm/yyyy', minDate: '0d'
                            dateFormat: 'dd/mm/yyyy', 
                        });


                    }
                }
            }

        });
    </script>

        <script>
            function Finalamoutcalulate() {
                var Rate = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_0').val();
                //var CustDisc = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_1').val();
                var TotalDisc = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_2').val();
                var Dealershare = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').val();
                var MTIShare = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_4').val();
                var sel = document.getElementById('ContentPlaceHolder1_drpStandardDisc');
                var sv = sel.options[sel.selectedIndex].value;
          
               
          
           
                $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_2').blur(function () {
                    
                    var custproposeDisc = Rate - TotalDisc;
                    $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_1').val(custproposeDisc);

                    $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').val(0);

                    if (sv == 1) {
               
                        $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_4').val("Standard Discount");
                    }
                    else
                    {
                
                        $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_4').val(0);
                    }


                });


                var TotalDisc = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_2').val();
             

                if (TotalDisc > 0) {
                    var custproposeDisc = Rate - TotalDisc;
                    $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_1').val(custproposeDisc);





                    $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').blur(function () {

                        var TotalDisc = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_2').val();
                        var Dealershare = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').val();
                        var MTIShare = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_4').val();

                        var sel = document.getElementById('ContentPlaceHolder1_drpStandardDisc');
                        var sv = sel.options[sel.selectedIndex].value;


                        var FinalMTIShare = TotalDisc - Dealershare;
                        if (sv == 1) {
                    
                            $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_4').val("Standard Discount");
                            $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').val(TotalDisc);
                        }
                        else {
                            $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_4').val(FinalMTIShare);
                            $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').val(Dealershare);

                        }
                    });

                    $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_4').blur(function () {
                        var TotalDisc = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_2').val();
                        var Dealershare = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').val();
                        var MTIShare = $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_4').val();

                        var sel = document.getElementById('ContentPlaceHolder1_drpStandardDisc');
                        var sv = sel.options[sel.selectedIndex].value;

                        var FinalDealerShare = TotalDisc - MTIShare;
                        if (sv == 1) {
                          
                            $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_4').val("Standard Discount");
                            $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').val(TotalDisc);
                        }
                        else {
                            $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_3').val(FinalDealerShare);
                            $('#ContentPlaceHolder1_QuotationDtls_txtQuotValue_4').val(MTIShare);
                        }
                    });
                }
            }
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
                    var txtDocDate = document.getElementById("ContentPlaceHolder1_txtM2Date_txtDocDate");
                    var txtqutdate = document.getElementById("ContentPlaceHolder1_txtqutdate_txtDocDate");
                    $('#ContentPlaceHolder1_txtqutdate_txtDocDate').datepick({
                        // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                        onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: '0d'
                    });


                    //var txtM2Date = document.getElementById("ContentPlaceHolder1_txtM2Date_txtDocDate");
                    //$('#ContentPlaceHolder1_txtM2Date_txtDocDate').datepick({
                    //    // dateFormat: 'dd/mm/yyyy', minDate: (txtDocDate.value == '') ? '0d' : txtDocDate.value
                    //    onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: '0d'
                    //});


                  
                

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
                    <asp:Label ID="lblTitle" runat="server" Text=" M2 (Quotation Submitted)"> </asp:Label>
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
                
                     <asp:Panel ID="PnlPendingDocument" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                       <UCPDoc:PendingDocument ID="PDoc" runat="server" />
                       <%--<uc4:SearchGridView ID="PDoc" runat="server" bIsCallForServer="true" OnImage_Click="SearchImage_Click" />--%>
                    </asp:Panel>
              

                    <asp:Panel ID="PSelectionGrid" runat="server" BorderColor="DarkGray" BorderStyle="Double">
                        <uc4:SearchGridView ID="SearchGrid" runat="server" bIsCallForServer="true" OnImage_Click="SearchImage_Click" />
                    </asp:Panel>

                    <asp:Panel ID="M0Details" runat="server" BorderColor="Black" BorderStyle="Double">
                      <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="CntM0"
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
                            ExpandControlID="TtlM1" CollapseControlID="TtlM1" Collapsed="true"
                            ImageControlID="ImgTtlM1Details" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="M1 (Enquiry Generated) Details" ExpandedText="M1 (Enquiry Generated) Details"
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
                                
                            </td>
                        </tr>
                       
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Source Name:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSourceName" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                              
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Source Address:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSourceAdd" Text="" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                               
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Source Mobile No:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSourceMob" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                              
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                Area:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpArea" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                               
                            </td>
                            <td class="tdLabel">
                                Attended By:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpAttendedby" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                              
                            </td>
                            <td class="tdLabel">
                                Allocated To:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpAlloatedTo" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                               
                            </td>
                        </tr>
                      
                        <tr>

                                <td class="tdLabel">
                                PO Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpPOType" runat="server" CssClass="ComboBoxFixedSize">
                                </asp:DropDownList>
                             
                            </td>
                                <td class="tdLabel">
                                Special Packages Required:
                            </td>
                          <td>
                                <asp:DropDownList ID="drpSpecialPackage" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    
                                    EnableViewState="true" >
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
                             
                            
                           
                       
                        
                        
                    </table>
                </asp:Panel>
                     </asp:Panel>
                
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
                                <asp:DropDownList ID="drpModelCat" runat="server" AutoPostBack="True" Enabled="false" OnSelectedIndexChanged="drpModelGroup_SelectedIndexChanged"  CssClass="ComboBoxFixedSize">
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

                    <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double">
                        <table id="Table1" runat="server" class="ContainTable" border="1">
                             <tr class="panel-heading">
                                <td align="center" class="panel-title" style="height: 15px" colspan="6">
                                    M2 (Quotation Submitted)
                                </td>
                            </tr>
                        
                            <tr>
                                 <td class="tdLabel">
                                    M2 No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtM2No" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    &nbsp;
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label4" runat="server" Text="M2 Date:" CssClass="tdLabel"></asp:Label></td><td>
                                    <uc3:CurrentDate ID="txtM2Date" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                                </td>
                                <td>
                                <asp:Label ID="Label5" runat="server" Text="Likely Buy Date:" CssClass="tdLabel"></asp:Label>
                            </td>
                            <td>
                                <uc3:CurrentDate ID="txtLikelydate" runat="server" Mandatory="True" bCheckforCurrentDate="false" />
                                
                            </td>
                            </tr>
                            
                                 <tr>
                                <td class="tdLabel">
                                    Quotation No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQutNo" runat="server" CssClass="TextBoxForString" MaxLength="20"></asp:TextBox>
                                   <asp:Label ID="Label49" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>

                                 <td>
                                
                                    <asp:Label ID="Label24" runat="server" Text="Quotation Date:" CssClass="tdLabel"></asp:Label></td><td>
                                    <uc3:CurrentDate ID="txtqutdate" runat="server" Mandatory="true" bCheckforCurrentDate="false" />
                                </td>

                               <td class="tdLabel">
                               Standard Discount:
                            </td>
                          <td>
                                <asp:DropDownList ID="drpStandardDisc" runat="server" Width="100px" CssClass="ComboBoxFixedSize" AutoPostBack="true"
                                    EnableViewState="true" OnSelectedIndexChanged="drpStandardDisc_SelectedIndexChanged" >
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="2">N</asp:ListItem>
                                </asp:DropDownList>
                               <asp:Label ID="Label11" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                
                        </td>


                            </tr>
                            <tr>
                                 <td class="tdLabel">
                                    Competitor:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpCompetitor" runat="server" CssClass="ComboBoxFixedSize">
                                    </asp:DropDownList>
                                     <asp:Label ID="Label6" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                    
                                </td>
                                <td class="tdLabel">
                                    Competitor Model:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCompModel" runat="server" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:Label ID="Label7" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>
                                <td class="tdLabel">
                                    Proposed Discount:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCompDiscAmt" runat="server" onkeypress=" return CheckForTextBoxValue(event,this,'6');" CssClass="TextBoxForString"></asp:TextBox>
                                    <asp:Label ID="Label8" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                </td>

                            </tr>
                             <tr>

                                 <td>
                                <asp:Label ID="Label50" runat="server" Text="Enquiry No:" Font-Bold="true" CssClass="tdLabel"></asp:Label>
                                     
                            </td>
                            <td>
                                <asp:TextBox ID="txtEnquiryNo" Text="" runat="server" ReadOnly="true" CssClass="TextBoxForString"></asp:TextBox>
                                
                            </td>

                                  <td>
                                <asp:Label ID="Label9" runat="server" Text="Delivery Weeks:" CssClass="tdLabel"></asp:Label>
                                     
                            </td>
                            <td>
                               <asp:TextBox ID="txtDelWeeks" runat="server" onkeypress=" return CheckForTextBoxValue(event,this,'5');" CssClass="TextForAmount"></asp:TextBox>
                            </td>

                                   <td class="tdLabel">
                                TCS Applicable:
                            </td>
                          <td>
                                <asp:DropDownList ID="drpTCSApp" runat="server" Width="100px" CssClass="ComboBoxFixedSize"
                                    
                                    EnableViewState="true" >
                                    <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem Value="1">Y</asp:ListItem>
                                    <asp:ListItem Value="2">N</asp:ListItem>
                                </asp:DropDownList>
                               <asp:Label ID="Label10" Text="*" runat="server" CssClass="Mandatory"></asp:Label>
                                
                        </td>
                           </tr>
                            <tr>
                                <td class="" style="width: 15%"></td>
                                <td>
                                    <asp:Button ID="bConfirm" OnClick="bConvertToM3" Text="Convert To M3" visible="false" runat="server" CssClass="ComboBoxFixedSize"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                
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

                     <asp:Panel ID="PQuotation" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                        class="">
                        <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="Cntquotation"
                            ExpandControlID="TtlQuotation" CollapseControlID="TtlQuotation" Collapsed="false"
                            ImageControlID="ImgTtlQuotation" ExpandedImage="~/Images/Minus.png" CollapsedImage="~/Images/Plus.png"
                            SuppressPostBack="true" CollapsedText="Details for Discount Approval" ExpandedText="Details for Discount Approval"
                            TextLabelID="lblTtlQuotation">
                        </cc1:CollapsiblePanelExtender>
                        <asp:Panel ID="TtlQuotation" runat="server">
                           <table width="100%">
                                  <tr class="panel-heading">
                                    <td align="center" class="panel-title" width="96%">
                               
                                        <asp:Label ID="lblTtlQuotation" runat="server" Text="Details for Discount Approval" Width="96%"
                                            onmouseover="SetCancelStyleonMouseOver(this);" onmouseout="SetCancelStyleOnMouseOut(this);"></asp:Label>
                                    </td>
                                    <td width="1%">
                                        <asp:Image ID="ImgTtlQuotation" runat="server" ImageUrl="~/Images/Plus.png"
                                            Height="15px" Width="100%" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Cntquotation" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                            ScrollBars="None">
                        
                       
                        
                            <asp:GridView ID="QuotationDtls" runat="server" GridLines="Horizontal" SkinID="NormalGrid" CssClass="table table-bordered table-hover"
                                Width="100%" AllowPaging="false" AlternatingRowStyle-Wrap="true" EditRowStyle-Wrap="true"
                                EditRowStyle-BorderColor="Black" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuotationID" runat="server" Text='<%# Eval("ID") %>' Width="3"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                
                                   <asp:TemplateField HeaderText="No." ItemStyle-Width="2%" HeaderStyle-CssClass="HideControl"
                                        ItemStyle-CssClass="HideControl">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuotID" runat="server" CssClass="LabelCenterAlign" 
                                                Text='<%# Eval("QuotId") %>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                        
                                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" CssClass="LabelCenterAlign" 
                                                Text='<%# Eval("Description") %>'> </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="17%" />
                                        </asp:TemplateField>      
                                    
                                         <asp:TemplateField HeaderText="Value" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQuotValue" runat="server"  CssClass="TextForAmount" Text='<%# Eval("Value","{0:#0}") %>' EnableViewState="true"
                                                 onkeypress=" return CheckForTextBoxValue(event,this,'6');" onblur=" return Finalamoutcalulate();"> </asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>        
                                </Columns>
                                <EditRowStyle BorderColor="Black" Wrap="True" />
                                <AlternatingRowStyle Wrap="True" />
                            </asp:GridView>
                        </asp:Panel>
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
                                            <asp:TextBox ID="txtClosureID" runat="server" Text='<%# Eval("ID") %>' Width="1"></asp:TextBox></ItemTemplate>

                                </asp:TemplateField><asp:TemplateField HeaderText="Loss Reason" ItemStyle-Width="10%">
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
                    <asp:TextBox ID="txtUserType" CssClass="HideControl" runat="server" Width="1px" Text=""></asp:TextBox>
                    <asp:TextBox ID="txtControlCount" CssClass="DispalyNon" runat="server" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtFormType" CssClass="DispalyNon" runat="server" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtDealerId" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:TextBox ID="txtVelInDtlID" runat="server" CssClass="DispalyNon" Width="1px"
                        Text=""></asp:TextBox><asp:TextBox ID="txtPreviousDocId" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM0ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtM1ID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtCustID" runat="server" CssClass="DispalyNon" Width="1px"
                         Text=""></asp:TextBox><asp:TextBox ID="txtHDCode" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox><asp:HiddenField ID="hdnMinFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnMaxFPDADate" runat="server" Value="" />
                    <asp:HiddenField ID="hdnConfirm" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnCancle" runat="server" Value="N" />
                    <asp:HiddenField ID="hdnHold" runat="server" Value="N" />
                 <asp:HiddenField ID="hdnLost" runat="server" Value="N" />
                      <asp:TextBox ID="txtAppNo" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                      <asp:TextBox ID="txtRFPID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                     <asp:TextBox ID="txtInqID" runat="server" CssClass="DispalyNon" Width="1px" Text=""></asp:TextBox>
                
                </td>
            </tr>
        </table>
        </div>
    </asp:Content>
