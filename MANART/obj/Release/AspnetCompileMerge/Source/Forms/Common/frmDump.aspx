<%@ Page Title="Dump page" Language="C#" MasterPageFile="~/Header.Master" AutoEventWireup="true" CodeBehind="frmDump.aspx.cs" Inherits="MANART.Forms.Common.frmDump" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Content/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <iframe id="frame1" scrolling="auto" runat="server" frameborder="0"  style="width :100%;height:400px" cssclass="TextBoxForString">
</iframe>
</asp:Content>
