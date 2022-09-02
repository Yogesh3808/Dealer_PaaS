<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrentDate.ascx.cs"
    Inherits="MANART.WebParts.CurrentDate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="../Content/bootstrap.css" rel="stylesheet" />
<link href="../Content/style.css" rel="stylesheet" />
<script src="../Scripts/jsValidationFunction.js"></script>

<%--For Jquery DatePicker--%>
<link href="../../Content/cssDatePicker.css" rel="stylesheet" />
<script src="../../Scripts/jquery-1.4.2.min.js"></script>
<script src="../../Scripts/jquery.datepick.js"></script>
<script type="text/javascript">
    window.onload = function () {
        //DisplayCurrentDate('ContentPlaceHolder1_txtDocDate_txtDocDate');        
    }
</script>
<style type="text/css">
    .style1
    {
        margin-left: 10px;
        background-color: #efefef;
    }
    .StyleForDate
    {
        font-family: Arial, Helvetica, sans-serif;
        font-size: 10px;
        color: Black;        
        height: 15px;
        border: 1px solid #8DC5E3;
    }   
        .CalenderCss
    {
        background-color:White;   
        border-color:Black;
        border-width:1px;
    }
    .CalenderCss td
    {
        padding:2px 0px;        
        border-right:none 1px #d9d9d9;    
    }
        
</style>
<div class="">
        <asp:TextBox ID="txtDocDate" runat="server" CssClass="TextForDate"
            OnTextChanged="txtDocDate_TextChanged"></asp:TextBox>
        <%--onkeydown="return ToSetKeyPressValueFalse(event,this);" 
                 onClick="return SetControlReadOnly(this);"  onkeydown="return ToSetKeyPressValueFalse(event,this);"--%>
        <asp:Label ID="lblMandatory" runat="server" Text=" *" CssClass="Mandatory"></asp:Label>
    </div>


