<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrentDateTime.ascx.cs" Inherits="MANART.WebParts.CurrentDateTime" %>
<link href="../Content/style.css" rel="stylesheet" />
<link href="../Content/bootstrap.css" rel="stylesheet" />
<%--For Jquery DatePicker--%>
<link href="../../Content/cssDatePicker.css" rel="stylesheet" />
<script src="../../Scripts/jquery-1.4.2.min.js"></script>
<script src="../../Scripts/jquery.datepick.js"></script>


<script src="../Scripts/js_datetimepicker.js"></script>
<%--<script src="../Scripts/jquery.datepick.js"></script>--%>
<%--<script src="../Scripts/bootstrap.min.js"></script>--%>
<div class="form-inline">
    <div class="form-group">
<asp:TextBox ID="txtDocDate" runat="server" CssClass=""  Width ="80px" Height="25px"></asp:TextBox><asp:DropDownList ID="DrpHrs" runat="server" Width="40px" Height="25px" >
    <asp:ListItem Value ="00" Text="00" ></asp:ListItem>
    <asp:ListItem Value ="01" Text="01"></asp:ListItem>
    <asp:ListItem Value ="02" Text="02"></asp:ListItem>
    <asp:ListItem Value ="03" Text="03"></asp:ListItem>
    <asp:ListItem Value ="04" Text="04" ></asp:ListItem>
    <asp:ListItem Value ="05" Text="05"></asp:ListItem>
    <asp:ListItem Value ="06" Text="06"></asp:ListItem>
    <asp:ListItem Value ="07" Text="07"></asp:ListItem>
    
    <asp:ListItem Value ="08" Text="08" ></asp:ListItem>
    <asp:ListItem Value ="09" Text="09" ></asp:ListItem>
    <asp:ListItem Value ="10" Text="10" ></asp:ListItem>
    <asp:ListItem Value ="11" Text="11" ></asp:ListItem>
    <asp:ListItem Value ="12" Text="12" ></asp:ListItem>
    <asp:ListItem Value ="13" Text="13" ></asp:ListItem>
    <asp:ListItem Value ="14" Text="14" ></asp:ListItem>
    <asp:ListItem Value ="15" Text="15" ></asp:ListItem>
    
    <asp:ListItem Value ="16" Text="16" ></asp:ListItem>
    <asp:ListItem Value ="17" Text="17"></asp:ListItem>
    <asp:ListItem Value ="18" Text="18"></asp:ListItem>
    <asp:ListItem Value ="19" Text="19"></asp:ListItem>
    <asp:ListItem Value ="20" Text="20" ></asp:ListItem>
    <asp:ListItem Value ="21" Text="21"></asp:ListItem>
    <asp:ListItem Value ="22" Text="22"></asp:ListItem>
    <asp:ListItem Value ="23" Text="23"></asp:ListItem>    
    
</asp:DropDownList><asp:DropDownList ID="DrpMin" runat="server" Width="40px" Height="25px" >
    <asp:ListItem Value ="00" Text="00" ></asp:ListItem>
    <asp:ListItem Value ="01" Text="01"></asp:ListItem>
    <asp:ListItem Value ="02" Text="02"></asp:ListItem>
    <asp:ListItem Value ="03" Text="03"></asp:ListItem>
    <asp:ListItem Value ="04" Text="04" ></asp:ListItem>
    <asp:ListItem Value ="05" Text="05"></asp:ListItem>
    <asp:ListItem Value ="06" Text="06"></asp:ListItem>
    <asp:ListItem Value ="07" Text="07"></asp:ListItem>
    
    <asp:ListItem Value ="08" Text="08" ></asp:ListItem>
    <asp:ListItem Value ="09" Text="09" ></asp:ListItem>
    <asp:ListItem Value ="10" Text="10" ></asp:ListItem>
    <asp:ListItem Value ="11" Text="11" ></asp:ListItem>
    <asp:ListItem Value ="12" Text="12" ></asp:ListItem>
    <asp:ListItem Value ="13" Text="13" ></asp:ListItem>
    <asp:ListItem Value ="14" Text="14" ></asp:ListItem>
    <asp:ListItem Value ="15" Text="15" ></asp:ListItem>
    
    <asp:ListItem Value ="16" Text="16" ></asp:ListItem>
    <asp:ListItem Value ="17" Text="17"></asp:ListItem>
    <asp:ListItem Value ="18" Text="18"></asp:ListItem>
    <asp:ListItem Value ="19" Text="19"></asp:ListItem>
    <asp:ListItem Value ="20" Text="20" ></asp:ListItem>
    <asp:ListItem Value ="21" Text="21"></asp:ListItem>
    <asp:ListItem Value ="22" Text="22"></asp:ListItem>
    <asp:ListItem Value ="23" Text="23"></asp:ListItem>  
    
    <asp:ListItem Value ="24" Text="24" ></asp:ListItem>
    <asp:ListItem Value ="25" Text="25" ></asp:ListItem>
    <asp:ListItem Value ="26" Text="26" ></asp:ListItem>
    <asp:ListItem Value ="27" Text="27" ></asp:ListItem>
    <asp:ListItem Value ="28" Text="28" ></asp:ListItem>
    <asp:ListItem Value ="29" Text="29" ></asp:ListItem>
    <asp:ListItem Value ="30" Text="30" ></asp:ListItem>
    <asp:ListItem Value ="31" Text="31" ></asp:ListItem>
    
    <asp:ListItem Value ="32" Text="32" ></asp:ListItem>
    <asp:ListItem Value ="33" Text="33" ></asp:ListItem>
    <asp:ListItem Value ="34" Text="34" ></asp:ListItem>
    <asp:ListItem Value ="35" Text="35" ></asp:ListItem>
    <asp:ListItem Value ="36" Text="36" ></asp:ListItem>
    <asp:ListItem Value ="37" Text="37" ></asp:ListItem>
    <asp:ListItem Value ="38" Text="38" ></asp:ListItem>
    <asp:ListItem Value ="39" Text="39" ></asp:ListItem>
    
    <asp:ListItem Value ="40" Text="40"></asp:ListItem>
    <asp:ListItem Value ="41" Text="41"></asp:ListItem>
    <asp:ListItem Value ="42" Text="42"></asp:ListItem>
    <asp:ListItem Value ="43" Text="43"></asp:ListItem>
    <asp:ListItem Value ="44" Text="44"></asp:ListItem>
    <asp:ListItem Value ="45" Text="45"></asp:ListItem>
    <asp:ListItem Value ="46" Text="46"></asp:ListItem>
    <asp:ListItem Value ="47" Text="47"></asp:ListItem>  
    
    
    <asp:ListItem Value ="48" Text="48" ></asp:ListItem>
    <asp:ListItem Value ="49" Text="49" ></asp:ListItem>
    <asp:ListItem Value ="50" Text="50" ></asp:ListItem>
    <asp:ListItem Value ="51" Text="51" ></asp:ListItem>
    <asp:ListItem Value ="52" Text="52" ></asp:ListItem>
    <asp:ListItem Value ="53" Text="53" ></asp:ListItem>
    <asp:ListItem Value ="54" Text="54" ></asp:ListItem>
    <asp:ListItem Value ="55" Text="55" ></asp:ListItem>
    
    <asp:ListItem Value ="56" Text="56" ></asp:ListItem>
    <asp:ListItem Value ="57" Text="57" ></asp:ListItem>
    <asp:ListItem Value ="58" Text="58" ></asp:ListItem>
    <asp:ListItem Value ="59" Text="59" ></asp:ListItem>
    
</asp:DropDownList><asp:Label ID="lblMandatory" runat="server" CssClass="Mandatory" Text=" *" Font-Bold="true"></asp:Label>
    </div>
</div>


