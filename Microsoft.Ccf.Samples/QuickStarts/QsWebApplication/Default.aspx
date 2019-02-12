<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"  ValidateRequest="false" Inherits="Microsoft.Ccf.QuickStarts._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Quick Start Web Application</title>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 536px; border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none;" id="TABLE1">
            <tr>
                <td style="width: 102px; border-top-style: none; font-family: Arial; border-right-style: none; border-left-style: none; border-bottom-style: none;">
        
        First Name:</td>
                <td>
                <asp:TextBox ID="txtFirstName" runat="server" Width="100%"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 102px; border-top-style: none; font-family: Arial; border-right-style: none; border-left-style: none; border-bottom-style: none;">
        Last Name:</td>
                <td>
        <asp:TextBox ID="txtLastName" runat="server" Width="100%"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 102px; border-top-style: none; font-family: Arial; border-right-style: none; border-left-style: none; border-bottom-style: none;">
        Street Name:</td>
                <td>
        <asp:TextBox ID="txtAddress" runat="server" Width="370px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 102px; border-top-style: none; font-family: Arial; border-right-style: none;
                    border-left-style: none; border-bottom-style: none">
        CustomerID:</td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" Width="100%"></asp:TextBox></td>
            </tr>
        </table>
        &nbsp;&nbsp;<br />
        &nbsp;<asp:Button ID="update" runat="server" Text="Update" OnClick="update_Click" /><br />
        <br />
        &nbsp;
        <p></p>
    </form>
</body>
</html>
