<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--<meta runat="server" id="meta1" http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />--%>
    <link rel="Stylesheet" href="Stylesheets/Defaultpage.css" />
    <title></title>
       
</head>
<body>
    <form id="form1" runat="server">
    <div id="tdTop">
        <asp:Image ID="secMasterLogo" ImageUrl="~/Images/IVP.gif" runat="server" />
    </div>
    
    <div id="content" runat="server">
        <center>
            <asp:Panel ID="pnlBase" runat="server">
                <table id="tblContent">
                    <tr>
                        <td style="width:200px;">
                            <asp:Label ID="svnUrl" runat="server" Text="SVN Url" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtSvnUrl" Width="250px" runat="server" Text="http://ivp-build.ivp.co.in:8080/svn/SCM/SCM/BUILD/SECMASTER" />
                            </td>
                            <td style="color:Red; text-align:left;">*
                            <asp:RequiredFieldValidator ID="rfvSvnUrl" runat="server" ControlToValidate="txtSvnUrl" ErrorMessage="Svn Url is Required" ValidationGroup="SvnCheck" />
                            <asp:CustomValidator ID="customSvnUrl" runat="server" ControlToValidate="txtSvnUrl" 
                                    ErrorMessage="Invalid Svn Url"
                                    onservervalidate="customSvnUrl_ServerValidate" ValidationGroup="SvnCheck"/>
                            </td>
                        
                    </tr>
                    <tr>
                        <td style="width:200px;">
                            Choose
                        </td>
                        <td style="text-align: left;">
                            <asp:RadioButtonList ID="optionList" runat="server" al>
                                <asp:ListItem Text="Batch" Value="Batch" />
                                <asp:ListItem Text="Updates" Value="Updates" Selected="true" />
                                <asp:ListItem Text="Installation Build" Value="Install" />
                            </asp:RadioButtonList>
                        </td>
                        <td></td>
                    </tr>
                    <tr><td></td><td></td>
                    <td style="text-align:right;">
                            <asp:Button ID="btnCheckSvn" runat="server" Text="Check Url" 
                                onclick="btnCheckSvn_Click" ValidationGroup="SvnCheck"/>
                        </td>
                    </tr>
                    </table>
                    </center>
                    <asp:Panel ID="updatePanel" runat="server" Visible="false">
                    <center>
                <table id="tblUpdatePanel">
                <tr style="text-align: left;">
                        <td id="test1" runat="server">
                            <asp:Label ID="lblRevision" runat="server" Text="Revision ID" />
                        </td>
                        <td id="test2" runat="server">
                            <asp:TextBox ID="txtRevision" Width="196px" runat="server" />
                        </td>
                        <td style="color:Red;" id="test3" runat="server">*
                            <asp:RequiredFieldValidator ID="rfvRevision" runat="server" ControlToValidate="txtRevision" ErrorMessage="Revision is Required" ValidationGroup="RevisionCheck"/>
                            </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;width:200px;">
                             <asp:Label ID="lblSecMasterVersion" runat="server" Text="Security Master Version" />
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlSecMasterVersion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSecMasterVersion_SelectedIndexChanged" Width="200px" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: left;width:200px;">
                             <asp:Label ID="lblRadVersion" runat="server" Text="RAD Version" />
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlRadVersion" Width="200px" runat="server" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: left;width:200px;">
                             <asp:Label ID="lblRefMasterVersion" runat="server" Text="Ref Master Version" />
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlRefMasterVersion" Width="200px" runat="server" />
                        </td>
                        <td></td>
                    </tr>
                    <tr><td></td><td></td><td></td></tr>
                    <tr><td></td><td></td><td></td></tr>
                    
                    <tr>
                        <td>
                        </td>
                        <td></td>
                        <td style="text-align: right;">
                            <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" ValidationGroup="RevisionCheck"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;width:200px;">
                            <asp:Label ID="lblDownload" runat="server" />
                        </td>
                        <td></td>
                        <td style="text-align: right;">
                            <asp:Button ID="Button1" Text="Download" runat="server" />
                        </td>
                    </tr>
                </table>
                </center>
            </asp:Panel>
        </asp:Panel>
        
    </div>
    </form>
</body>
</html>
