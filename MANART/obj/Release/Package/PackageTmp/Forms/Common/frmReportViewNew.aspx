<%@ Page Title="" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmReportViewNew.aspx.cs" Inherits="MANART.Forms.Common.frmReportViewNew" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>


<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/style.css" rel="stylesheet" />
    <link href="../../Content/cssDatePicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/jquery.datepick.js"></script>
    <link href="../../Content/jquery.datepick.css" rel="stylesheet" />
    <link href="../../Content/GridStyle.css" rel="stylesheet" />
    <script src="../../Scripts/jsValidationFunction.js"></script>
    <style type="text/css">
        input[type="radio"], input[type="checkbox"] {
            margin: 4px 0 0;
            margin-top: 1px \9;
            line-height: normal;
            margin-left: -190px;
        }

        label {
            color: #303c49;
            float: left;
            font-size: 12px;
            font-weight: normal;
            width: 203px;
            padding-left: 30px;
        }
            /*.myradioButton {
            margin-right: 2px;
            margin-left: 2px;
            margin: 2px 2px 2px 2px;
            padding: 1px 1px 1px 1px;
            padding-left: 1px;
            text-indent: 1px;
            text-align: left;
            width: 10px;
        }*/
            /*.radio input[type="radio"],
        .radio-inline input[type="radio"],
        .checkbox input[type="checkbox"],
        .checkbox-inline input[type="checkbox"] {
            position: absolute;
            margin-top: 4px \9;
            margin-left: -145px;
        }*/
            /*input[type="radio"] {
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
            padding: 0;
        }

        input[type="radio"],
        input[type="checkbox"] {
            margin: 4px 0 0;
            margin-top: 1px \9;
            line-height: normal;
        }

            input[type="file"]:focus,
            input[type="radio"]:focus,
            input[type="checkbox"]:focus {
                outline: thin dotted;
                outline: 5px auto -webkit-focus-ring-color;
                outline-offset: -2px;
            }

        .radio,
        .checkbox {
            position: relative;
            display: block;
            margin-bottom: 10px;
        }

            .radio label,
            .checkbox label {
                min-height: 20px;
                margin-right:20px;
                margin-bottom: 0;
                font-weight: normal;
                cursor: pointer;
            }

            .radio input[type="radio"],
            .radio-inline input[type="radio"],
            .checkbox input[type="checkbox"],
            .checkbox-inline input[type="checkbox"] {
                position: absolute;
                margin-top: 4px \9;
                margin-left: -20px;
            }

            .radio + .radio,
            .checkbox + .checkbox {
                margin-top: -5px;
                
            }

        .radio-inline,
        .checkbox-inline {
            position: relative;
            display: inline-block;
            padding-left: 20px;
            margin-bottom: 0;
            font-weight: normal;
            vertical-align: middle;
            cursor: pointer;
        }

            .radio-inline + .radio-inline,
            .checkbox-inline + .checkbox-inline {
                margin-top: 0;
                margin-left: 10px;
            }

        input[type="radio"][disabled],
        input[type="checkbox"][disabled],
        input[type="radio"].disabled,
        input[type="checkbox"].disabled,
        fieldset[disabled] input[type="radio"],
        fieldset[disabled] input[type="checkbox"] {
            cursor: not-allowed;
        }

        .radio-inline.disabled,
        .checkbox-inline.disabled,
        fieldset[disabled] .radio-inline,
        fieldset[disabled] .checkbox-inline {
            cursor: not-allowed;
        }

        .radio.disabled label,
        .checkbox.disabled label,
        fieldset[disabled] .radio label,
        fieldset[disabled] .checkbox label {
            cursor: not-allowed;
        }*/
            /*.radiobutton label::before {
            content: "";
            display: inline-block;
            position: relative;
            width: 15px;
            height: 15px;
            left: 0;
            margin-left: -20px;
            border: 1px solid #cccccc;
            border-radius: 3px;
            background-color: #fff;
            -webkit-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
            -o-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
            transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
        }

        .radiobutton label::after {
            display: inline-block;
            position: absolute;
            width: 25px;
            height: 16px;
            left: 0;
            top: 0;
            padding-left: 3px;
            padding-top: 1px;
            font-size: 11px;
            color: #555555;
        }



        .radiobutton input[type="radio"] {
            opacity: 0;
            z-index: 1;
        }

            .radiobutton input[type="radio"]:checked + label::after {
                font-family: "FontAwesome";
                content: "\f00c";
            }

        .radiobutton-primary input[type="radio"]:checked + label::before {
            background-color: #337ab7;
            border-color: #337ab7;
        }

        .radiobutton-primary input[type="radio"]:checked + label::after {
            color: #fff;
        }*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var txtFromDate = document.getElementById("ContentPlaceHolder1_txtFromDate_txtDocDate");
            var txtToDate = document.getElementById("ContentPlaceHolder1_txtToDate_txtDocDate");
            $('#ContentPlaceHolder1_txtFromDate_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', maxDate: txtToDate.value
            });

            $('#ContentPlaceHolder1_txtToDate_txtDocDate').datepick({
                onSelect: customRange, dateFormat: 'dd/mm/yyyy', minDate: txtFromDate.value
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
    </script>

    <script type="text/javascript">
        ///Report Display
        function ShowReports(Rurl) {
            // window.showModalDialog(Rurl ,"Report", "dialogHeight: 500px; dialogWidth: 1000px; dialogTop: 150px; dialogLeft: 150px; edge: Raised; center: Yes; help: No; scroll: Yes; status: Yes;");
            //window.open(Rurl, null, "titlebar=no, status=no,menubar=no,resizable=yes, scrollbars=no,toolbar=no,location=no,directories=no,left=0,top=0,height=' + document.documentElement.offsetHeight + ',width=' + document.documentElement.offsetWidth;");
            var windowFeatures;
            window.opener = self;
            //window.close()  
            //windowFeatures = "top=0,left=0,resizable=yes,width=" + (screen.width) + ",height=" + (screen.height);
            windowFeatures = "top=0,left=0,resizable=yes,width=" + (window.outerWidth) + ",height=" + (window.outerHeight-60);
            newWindow = window.open(Rurl, "", windowFeatures)
            window.moveTo(0, 0);
            //window.resizeTo(screen.width, screen.height - 100);
            //window.resizeTo(window.outerWidth, window.outerHeight - 100);
            //newWindow.focus();

        }

        //sujata 16032011
        function client_OnTreeNodeChecked() {
            var obj = window.event.srcElement;
            var treeNodeFound = false;
            var checkedState;
            if (obj.tagName == "INPUT" && obj.type == "checkbox") {
                var treeNode = obj;
                checkedState = treeNode.checked;
                do {
                    obj = obj.parentElement;
                } while (obj.tagName != "TABLE")
                var parentTreeLevel = obj.rows[0].cells.length;
                var parentTreeNode = obj.rows[0].cells[0];
                var tables = obj.parentElement.getElementsByTagName("TABLE");
                var numTables = tables.length
                if (numTables >= 1) {
                    for (i = 0; i < numTables; i++) {
                        if (tables[i] == obj) {
                            treeNodeFound = true;
                            i++;
                            if (i == numTables) {
                                return;
                            }
                        }
                        if (treeNodeFound == true) {
                            var childTreeLevel = tables[i].rows[0].cells.length;
                            if (childTreeLevel > parentTreeLevel) {
                                var cell = tables[i].rows[0].cells[childTreeLevel - 1];
                                var inputs = cell.getElementsByTagName("INPUT");
                                inputs[0].checked = checkedState;
                            }
                            else {
                                return;
                            }
                        }
                    }
                }
            }
        }
        //Sujata 16032011               
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="PageTbl" class="PageTable" border="1">
        <tr id="TitleOfPage" class="panel-heading">
            <td class="PageTitle panel-title" align="center" style="width: 14%" colspan="2">
                <asp:Label ID="lblTitle" runat="server" Text="Report List">
                </asp:Label>
            </td>

        </tr>
        <tr>
            <td style="width: 5%" rowspan="2">
                <div style="border-right-style: solid; border-right-width: 2%; border-right-color: #99CCFF;">
                    <asp:TreeView ID="TreeReportGroup" runat="server" AutoGenerateDataBindings="False"
                        ShowLines="True" Font-Names="Verdana" Font-Size="12px" ForeColor="#156FC4"
                        Height="10%"
                        Style="direction: ltr; border-top-style: none; border-right-style: none; border-left-style: none; text-align: left; border-bottom-style: none; font-family: verdana, sans-serif;"
                        Width="96%"
                        onclick="client_OnTreeNodeChecked();"
                        OnSelectedNodeChanged="TreeReportGroup_SelectedNodeChanged">
                        <ParentNodeStyle CssClass="normal" />
                        <NodeStyle BackColor="White" VerticalPadding="10px" />
                        <LeafNodeStyle BackColor="White" VerticalPadding="9px" />
                        <HoverNodeStyle BackColor="White" />
                        <RootNodeStyle CssClass="normal" />
                        <SelectedNodeStyle CssClass="hover" />
                    </asp:TreeView>
                </div>
            </td>
            <td style="width: 20%">
                <div style="border: medium solid #33CCFF; background-color: #EFEFEF; height: 230px;">
                    <%-- <div class="controls radio">--%>
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" CssClass="myradioButton"
                        CellPadding="2" CellSpacing="20" RepeatColumns="2" TabIndex="2"
                        Font-Names="Verdana" Font-Size="X-Small" Height="10%" Width="100%"
                        BackColor="#E9ECF0" TextAlign="Right">
                    </asp:RadioButtonList>
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 14%">
                <asp:Panel ID="RptSelection" runat="server" BorderColor="#6EBADC"
                    BorderStyle="Double" CssClass="DispalyNon">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                                <table id="Table2" runat="server" class="ContainTable" border="1">
                                    <tr>
                                        <td align="center" class="ContaintTableHeader" style="height: 15px" colspan="8">
                                            <asp:Panel ID="Panel2" runat="server">
                                                Selection
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%; height: 21px;" align="right">
                                            <asp:Label ID="LblRegion" runat="server" Text="Region" CssClass="LabelCenterAlign">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 15%; height: 21px;">
                                            <asp:DropDownList ID="drpRegionSelection" runat="server" CssClass="ComboBoxFixedSize"
                                                AutoPostBack="True"
                                                OnSelectedIndexChanged="drpRegionSelection_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 15%; height: 21px;" align="right">
                                            <asp:Label ID="LblCountry" runat="server" Text="State" CssClass="LabelCenterAlign">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 15%; height: 21px;">
                                            <asp:DropDownList ID="drpCountrySelection0" runat="server" CssClass="ComboBoxFixedSize"
                                                AutoPostBack="True"
                                                OnSelectedIndexChanged="drpCountrySelection_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 15%; height: 21px;" align="right">
                                            <asp:Label ID="LblCSM" runat="server" Text="CSM" CssClass="LabelCenterAlign">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 15%; height: 21px;">
                                            <asp:DropDownList ID="drpCSMSelection" runat="server" CssClass="ComboBoxFixedSize"
                                                AutoPostBack="True"
                                                OnSelectedIndexChanged="drpCSMSelection_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 15%; height: 21px;" align="right">
                                            <asp:Label ID="lblDealer" runat="server" Text="Dealer" CssClass="LabelCenterAlign">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 15%; height: 21px;">
                                            <asp:DropDownList ID="drpDealer" runat="server" CssClass="ComboBoxFixedSize"
                                                AutoPostBack="True"
                                                OnSelectedIndexChanged="drpDealer_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%; height: 24px;" align="right">
                                            <asp:Label ID="lblFrmDt" runat="server" CssClass="LabelCenterAlign"
                                                Text="From Date"> </asp:Label>
                                        </td>
                                        <td style="width: 15%; height: 24px">
                                            <uc3:CurrentDate ID="txtFromDate" runat="server" bCheckforCurrentDate="false" Mandatory="false" />
                                        </td>
                                        <td style="width: 15%; height: 24px" align="right">
                                            <asp:Label ID="lblToDt" runat="server" CssClass="LabelCenterAlign"
                                                Text="To Date"> </asp:Label>
                                        </td>
                                        <td style="width: 15%; height: 24px">
                                            <uc3:CurrentDate ID="txtToDate" runat="server" bCheckforCurrentDate="false" Mandatory="false" />
                                        </td>
                                        <td style="width: 15%; height: 24px;" align="right">
                                            <asp:Label ID="lblMonth" runat="server" Text="Month" CssClass="LabelCenterAlign">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 16%; height: 24px;" align="right">
                                            <asp:DropDownList ID="drpMonthSelection" runat="server" CssClass="ComboBoxFixedSize"
                                                AutoPostBack="True"
                                                OnSelectedIndexChanged="drpMonthSelection_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 16%; height: 24px;"></td>
                                        <td style="width: 16%; height: 24px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%; height: 24px;" align="right">
                                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="LabelCenterAlign">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 15%; height: 24px" align="right">
                                            <asp:DropDownList ID="DrpStatus" runat="server" CssClass="ComboBoxFixedSize"
                                                OnSelectedIndexChanged="DrpStatus_SelectedIndexChanged">
                                                <asp:ListItem Text="All" Value="A">                                 
                                                </asp:ListItem>
                                                <asp:ListItem Text="Done" Value="Y">                                 
                                                </asp:ListItem>
                                                <asp:ListItem Text="Pending" Value="N">                                 
                                                </asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 15%; height: 24px" align="right">
                                            <asp:Label ID="lblUsrEnter" runat="server" Text="Enter N " CssClass="LabelCenterAlign">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 15%; height: 24px">
                                            <asp:TextBox ID="txtUserVal" runat="server" CssClass="TextForAmount" Width="90%"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%; height: 24px"></td>
                                        <td style="width: 15%; height: 24px" align="right"></td>
                                        <td style="width: 15%; height: 24px"></td>
                                        <td style="width: 15%; height: 24px"></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%; height: 24px;" align="right">
                                            <asp:Label ID="lblModCat" runat="server" Text="Model Category" CssClass="LabelCenterAlign">
                                            </asp:Label>
                                        </td>
                                        <td style="height: 24px">
                                            <asp:DropDownList ID="drpCategorySelection" runat="server" CssClass="ComboBoxFixedSize"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 18%; height: 24px;" align="right">
                                            <asp:Label ID="lblcusttypeid" runat="server" Text="Cust Type" CssClass="LabelCenterAlign">
                                            </asp:Label>
                                        </td>
                                        <td style="height: 24px">
                                            <asp:DropDownList ID="drpcusttype" runat="server" CssClass="ComboBoxFixedSize"
                                                AutoPostBack="True"
                                                OnSelectedIndexChanged="drpCSMSelection_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="height: 24px" align="right"></td>
                                        <td style="height: 24px"></td>
                                        <td style="height: 24px" align="right">
                                            <asp:Label ID="lblRptOptn" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblVehSpr" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblDelOrg" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblRegionID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblCountryID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblCSMID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblDealerID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblAreaID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblMonthID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblYearid" runat="server" Text="3" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblRptTitle" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblFromDate" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblToDate" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblReportOptNo" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblUserEnter" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblcusttype" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblStatusID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblLogUserID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblStateID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblDeptID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <%--'Sujata23052012_Begin--%>
                                            <asp:Label ID="lblUserDeptID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <%--'Sujata23052012_End--%>
                                            <asp:Label ID="lblUserRole" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblLoginUserName" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblLoginServiceUserName" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <%--'vrushali26032011_Begin--%>
                                            <asp:Label ID="lblWarrClaimType" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblCustomerType" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblPOType" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblJobType" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblDealerType" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <%--'vrushali26032011_End--%>
                                            <asp:Label ID="lblHierarchy" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblRportHG" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <%--Sujata 21052011--%>
                                            <asp:Label ID="lblPartClaimType" runat="server" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblDlrDealerID" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblMonthNo" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <asp:Label ID="lblYearNo" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <%--Sujata 02082011--%>
                                            <asp:Label ID="lblDrlFrLocalData" runat="server" Text="" CssClass="DispalyNon"></asp:Label>
                                            <%--Sujata 02082011--%>
                                            <%--<asp:Label ID="lblFrmDate" runat="server" Text="" CssClass="DispalyNon"></asp:Label> 
                                 <asp:Label ID="lblTDate" runat="server" Text="" CssClass="DispalyNon"></asp:Label> --%>
                                            <%--Sujata 21052011--%>
                                        </td>
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <div>
                    <table runat="server" class="ContainTable" border="1">
                        <tr>
                            <td style="width: 7%; height: 24px">
                                <asp:Label ID="lblRptOptionLbl" runat="server" Text="Report Option-:" CssClass="LabelRightAlign"></asp:Label>
                            </td>
                            <td style="width: 45%; height: 24px">
                                <asp:DropDownList ID="DrpReportOption" runat="server" CssClass="ComboBoxFixedSize"
                                    AutoPostBack="True"
                                    OnSelectedIndexChanged="DrpReportOption_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 7%; height: 24px">
                                <asp:Label ID="Label3" runat="server" Text="Basic Model Category-:" CssClass="LabelRightAlign"></asp:Label>
                            </td>
                            <td style="width: 45%; height: 24px">
                                <asp:DropDownList ID="DrpBasModCatID" runat="server" CssClass="ComboBoxFixedSize" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 15%; height: 24px">
                                <asp:Button ID="btnShow" runat="server" Text="Preview" CssClass="CommandButton" align="Center"
                                    OnClick="btnShow_Click" />
                            </td>
                        </tr>

                    </table>
                </div>
            </td>
        </tr>
        <tr height="55%">
            <td style="width: 14%" colspan="2" height="25%">
                <div style="height: 45%">
                    <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double" CssClass="DispalyNon">
                        <div>
                          <%--  <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%">
                            </rsweb:ReportViewer>--%>
                        </div>
                    </asp:Panel>
                </div>

            </td>
        </tr>
    </table>
</asp:Content>
