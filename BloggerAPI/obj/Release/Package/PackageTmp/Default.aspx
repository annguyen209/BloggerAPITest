<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BloggerAPI._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HyperLink ID="GotoAuthSubLink" runat="server"/>
    <asp:Button runat="server" CssClass="btn btn-primary" OnClick="btnRun_Click" ID="btnRun" Text="Run" />
</asp:Content>
