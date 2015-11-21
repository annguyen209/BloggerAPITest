<%@ Page Title="Home Page" Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BloggerAPI._Default" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div style="padding-top: 2em;">
        <h3>Select Action</h3>
        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlAction" Width="40%">
            <asp:ListItem Text="---------- None ----------" Value="0" ></asp:ListItem>
            <asp:ListItem Text="Gplus Share All Posts Today" Value="Gplus Share All Posts Today" ></asp:ListItem>
            <asp:ListItem Text="Generate Post CSAuthor" Value="Generate Post CSAuthor" ></asp:ListItem>
            <asp:ListItem Text="Generate Post CSAuthor With Checking Existence" Value="Generate Post CSAuthor With Checking Existence" ></asp:ListItem>
            <asp:ListItem Text="Update Time All Post By Label" Value="Update Time All Post By Label" ></asp:ListItem>
            <asp:ListItem Text="Add Label For Non Label Post" Value="Add Label For Non Label Post" ></asp:ListItem>
        </asp:DropDownList>
        <asp:Button runat="server" CssClass="btn btn-danger" style="margin: 1em 0 20em 0;" OnClick="btnRunAction_Click" ID="btnRunAction" Text="Run Action" Width="150" />
    </div>
    <div runat="server" id="Result" style="background: yellow; color: red"/>
</asp:Content>
