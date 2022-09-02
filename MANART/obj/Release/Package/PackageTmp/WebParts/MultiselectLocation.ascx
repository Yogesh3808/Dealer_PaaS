<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiselectLocation.ascx.cs" Inherits="MANART.WebParts.MultiselectLocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<link href="../Content/style.css" rel="stylesheet" />
<link href="../Content/bootstrap.css" rel="stylesheet" />
<script src="../Scripts/jsValidationFunction.js"></script>
       
<style type="text/css">
    .style1
    {
        width: 100%;
    }
</style>
<style type="text/css">
        .checkbox {
            padding-left: 20px;
        }

            .checkbox label {
                display: inline-block;
                vertical-align: middle;
                text-align: left;
                position: relative;
                padding-left: 5px;
                width: auto;
            }

                .checkbox label::before {
                    content: "";
                    display: inline-block;
                    position: absolute;
                    width: 17px;
                    height: 17px;
                    left: 0;
                    margin-left: -20px;
                    border: 1px solid #cccccc;
                    border-radius: 3px;
                    background-color: #fff;
                    -webkit-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                    -o-transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                    transition: border 0.15s ease-in-out, color 0.15s ease-in-out;
                }

                .checkbox label::after {
                    display: inline-block;
                    position: absolute;
                    width: 16px;
                    height: 16px;
                    left: 0;
                    top: 0;
                    margin-left: -20px;
                    padding-left: 3px;
                    padding-top: 1px;
                    font-size: 11px;
                    color: #555555;
                }

            .checkbox input[type="checkbox"] {
                opacity: 0;
                z-index: 1;
            }

                .checkbox input[type="checkbox"]:checked + label::after {
                    font-family: "FontAwesome";
                    content: "\f00c";
                }

        .checkbox-primary input[type="checkbox"]:checked + label::before {
            background-color: #337ab7;
            border-color: #337ab7;
        }

        .checkbox-primary input[type="checkbox"]:checked + label::after {
            color: #fff;
        }
    </style>
<table id="tblLocationDetails" runat="server" class="ContainTable table-responsive">
    <tr class="panel-heading">
        <td id="LocationID" align="center" class="ContaintTableHeader panel-title" style="height: 15px">
            Location Details
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="PLocationDetails" runat="server" BorderColor="DarkGray" BorderStyle="Double"
                ScrollBars="None">
                <table id="LocationDetails" runat="server" class="ContainTable tab ">
                    <tr>
                        <td class="tdLabel" style="width: 15%">
                            Region:
                        </td>
                        <td style="width: 18%">
                            <asp:DropDownList ID="drpRegion" runat="server" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpRegion_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="tdLabel" style="width: 15%">
                            <asp:Label ID="lblCountry" runat="server" Text=" Country.:"></asp:Label>
                        </td>
                        <td style="width: 18%">
                            <asp:DropDownList ID="drpCountry" runat="server" CssClass="ComboBoxFixedSize" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="tdLabel" style="width: 15%">
                            <%--Currency:--%>
                            <asp:Label ID="lblCurrency" runat="server" Text=" Currency:"></asp:Label>
                        </td>
                        <td style="width: 18%">
                            <asp:TextBox ID="txtCurrency" Text="" runat="server" CssClass="TextBoxForString"
                                Width="30%" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="width: 15%">
                            <%--Distributor Name:--%>
                            <asp:Label ID="lblDistributorName" runat="server" Text=" Supplier Name:"></asp:Label>
                        </td>
                        <td class="tdLabel" style="width: 25%" colspan="3">
                            
                            <div id="dvTop" class="dt" style="width: 53%;">
                                <asp:CheckBox ID="ChkAll" runat="server" onClick="SelectAll();"  /> <%-- oncheckedchanged="ChkAll_CheckedChanged" onClick="return SelectAll();" --%>
                                    
                                <asp:TextBox ID="txtDealerName" ReadOnly="True" runat="server" class="mstbm" onmouseover="DisplayTitle(this);"
                                    Width="90%"  >--Select--</asp:TextBox>                                
                            </div>
                            <div id="divMain" runat="server" class="dvmain checkbox checkbox-primary" style="width: 53%;">
                                <img class="nicheimage" src="../Images/niche.gif" style="display: none" />
                                <asp:CheckBoxList ID="ChkDealer" runat="server" Width="100%"  >
                                </asp:CheckBoxList>
                                <asp:LinkButton ID="lnkMain" runat ="server" CssClass="btn-link" Text ="Close Me" ></asp:LinkButton>
                            </div>
                        </td>
                        <td class="tdLabel" style="width: 15%">
                            &nbsp;
                        </td>
                        <td style="width: 18%">
                            &nbsp;
                        </td>
                    </tr>                  
                </table>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="PLocationDetails_CollapsiblePanelExtender" runat="server"
                Enabled="True" TargetControlID="PLocationDetails" CollapseControlID="LocationID"
                ExpandControlID="LocationID">
            </asp:CollapsiblePanelExtender>
        </td>
    </tr>
</table>
<input id="hapb" type="hidden" name="tempHiddenField" runat="server" />
<input id="hsiv" type="hidden" name="hsiv" runat="server" />
<input id="__ET1" type="hidden" name="__ET1" runat="server" />
<input id="__EA1" type="hidden" name="__EA1" runat="server" />
<input id="txtControl_ID" type="hidden" name="__EA1" runat="server" />

