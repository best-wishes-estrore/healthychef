<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="CustomerDetails.aspx.cs" Inherits="HealthyChef.WebModules.Reports.Admin.CustomerDetails" Theme="WebModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <center>
        <h1>Customers Details</h1>
    </center>
    <table width="100%" border="0">
        <tr>
            <td>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" ShowRefreshButton="false" Width="1200px" Height="800px" PageCountMode="Actual">
                </rsweb:ReportViewer>
            </td>
        </tr>
    </table>
</asp:Content>
