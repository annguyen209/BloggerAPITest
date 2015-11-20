<%@ Page Title="Home Page" Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BloggerAPI._Default" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div style="padding-top: 2em;">
        <h3>Select Action</h3>
        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlAction" Width="40%">
            <asp:ListItem Text="---------- None ----------" Value="0" ></asp:ListItem>
            <asp:ListItem Text="Generate Post" Value="Generate Post" ></asp:ListItem>
            <asp:ListItem Text="Generate Post With Checking Existence" Value="Generate Post With Checking Existence" ></asp:ListItem>
            <asp:ListItem Text="Edit Time Post" Value="Edit Time Post" ></asp:ListItem>
            <asp:ListItem Text="Add Label For Non Label Post" Value="Add Label For Non Label Post" ></asp:ListItem>
        </asp:DropDownList>
        <asp:Button runat="server" CssClass="btn btn-danger" style="margin: 1em 0 20em 0;" OnClick="btnRunAction_Click" ID="btnRunAction" Text="Run Action" Width="150" />
    </div>
    <div runat="server" id="Result" style="background: yellow; color: red"/>
</asp:Content>
