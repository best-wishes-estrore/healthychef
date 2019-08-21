<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServerReport.aspx.cs" Inherits="HealthyChef.WebModules.Reports.Admin.ServerReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
           <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" class="table table-bordered table-hover"  Height="600px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" ShowRefreshButton="true" PageCountMode="Actual" AsyncRendering="false">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
