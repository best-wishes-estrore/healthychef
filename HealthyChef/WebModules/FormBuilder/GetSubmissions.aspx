<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master"
    AutoEventWireup="true" CodeBehind="GetSubmissions.aspx.cs" Inherits="BayshoreSolutions.WebModules.FormBuilder.GetSubmissions"
    Theme="WebModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
<style type="text/css">.smallSize, .smallSize td { font-size: 11px; } </style> 
    <h3>
        Simple Form Submissions:</h3>
    <div style="float:right"><a href='<%=Request.QueryString.Get("ReturnUrl") %>'><< Back</a></div>
   <%-- <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptLocalization="true"
        EnableScriptGlobalization="true" />--%>
    <asp:MultiView ID="mvwSubmission" runat="server" ActiveViewIndex="0">
        <asp:View ID="vwList" runat="server">
        
            From Date:<asp:TextBox ID="txtStartDate" runat="server" Width="100" style="margin-right:0px;" />
            <asp:Image ID="StartDateImage" runat="server" ImageUrl="public/images/CalendarButton.png" />
        
           &nbsp;&nbsp;To Date:
            <asp:TextBox ID="txtEndDate" runat="server" Width="100" style="margin-right:0px;" />
            <asp:Image ID="EndDateCalendarImage" runat="server" ImageUrl="public/images/CalendarButton.png" />
            
            <ajax:CalendarExtender ID="StartDateCalendarExtender" runat="server" PopupButtonID="StartDateImage"
                TargetControlID="txtStartDate" />
            <ajax:CalendarExtender ID="EndDateCalendarExtender" runat="server" PopupButtonID="EndDateCalendarImage"
                TargetControlID="txtEndDate" />
                
            &nbsp;&nbsp;
            <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Refresh" />
            <bss:ExportButton runat="server" Text="Export to Excel" ID="bssExport" OnClick="bssExport_Click" />
            <br /><br />
            <div id="grdCharges" runat="server" style="width: 100%; height: 500px; overflow: auto;">
				<asp:GridView ID="grdSubmissions" runat="server" CssClass="smallSize" CellPadding="4" GridLines="Both" BorderColor="Black" BorderStyle="Solid" BorderWidth="1"
					AutoGenerateColumns="false" ShowHeader="true" Width="150%"
					AllowSorting="true" onrowdeleting="grdSubmissions_RowDeleting" OnRowCommand="grdSubmissions_RowCommand" OnSorting="grdSubmissions_Sorting"
					OnSelectedIndexChanged="grdSubmissions_OnSelectedIndexChanged" EmptyDataText="No matching records found.">
					<HeaderStyle BackColor="Black" Font-Bold="true" ForeColor="White" />
					<AlternatingRowStyle BackColor="LightGray" />
				</asp:GridView>
            </div>            
        </asp:View>
        <asp:View ID="vwDetail" runat="server">
            <div id="divDetail" runat="server" />
            <br />
            
            <asp:Button runat="server" ID="btnReturnToList" OnClick="btnReturnToList_Click" Text="Return to List" />
            
        </asp:View>
    </asp:MultiView>
</asp:Content>
