<%@ Page Title="Home Page" Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BloggerAPI._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="ErrorPan" style="background: yellow; color: red"/>
    <asp:Button runat="server" CssClass="btn btn-primary" OnClick="btnRun_Click" ID="btnRun" Text="Run" />
    <div runat="server" id="Result" style="background: yellow; color: red"/>
</asp:Content>
