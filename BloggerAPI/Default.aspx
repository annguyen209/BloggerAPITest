<%@ Page Title="Home Page" Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BloggerAPI._Default" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding-top: 2em;">
        <h3>Generate Post</h3>
        <asp:Button runat="server" CssClass="btn btn-danger" OnClick="btnRun_Click" ID="btnRun" Text="Run" Width="150" />
    </div>
    <div style="padding-top: 2em;">
        <h3>Generate Post With Checking Existence</h3>
        <asp:Button runat="server" CssClass="btn btn-danger" OnClick="btnRunCheckingExist_Click" ID="btnRunCheckingExist" Text="Run" Width="150" />
    </div>
    <div style="padding-top: 2em;">
        <h3>Edit Time Post</h3>
        <asp:Button runat="server" CssClass="btn btn-danger" OnClick="btnEditTime_Click" ID="btnEditTime" Text="Run" Width="150" />
    </div>
    <div style="padding-top: 2em;">
        <h3>Add Label For Non Label Post</h3>
        <asp:Button runat="server" CssClass="btn btn-danger" OnClick="btnAddLabel_Click" ID="btnAddLabel" Text="Run" Width="150" />
    </div>
    <div runat="server" id="Result" style="background: yellow; color: red"/>
</asp:Content>
