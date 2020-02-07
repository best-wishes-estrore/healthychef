<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" Theme="WebModules" AutoEventWireup="true"
    CodeBehind="ProductFeed.aspx.cs" Inherits="HealthyChef.WebModules.Reports.Admin.ProductFeed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>Product Feed
                </h1>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">
                </div>
            </div>
            <asp:Label ID="lblFeedback" runat="server" />
            <asp:ValidationSummary ID="ValSum1" runat="server" ValidationGroup="ProductFeedGroup" />
            <div class="col-sm-3">
                <div class="form-group">
                    <label>Start Delivery Date:</label>
                    <asp:TextBox runat="server" ID="txtStartDate" ClientIDMode="Static" CssClass="form-control"
                        ValidationGroup="ProductFeedGroup" Enabled="false" />
                    <asp:CompareValidator ID="cprVal1" runat="server" ControlToValidate="txtStartDate"
                        Display="Dynamic" ErrorMessage="Start Date must be a date." Operator="DataTypeCheck" Type="Date"
                        SetFocusOnError="true" ValidationGroup="ProductFeedGroup" />
                </div>
            </div>
            <div class="col-sm-3">
                <div class="form-group">
                    <label>End Delivery Date:</label>
                    <asp:TextBox runat="server" ID="txtEndDate" ClientIDMode="Static" CssClass="form-control"
                        ValidationGroup="ProductFeedGroup" Enabled="false" />
                    <asp:CompareValidator ID="cprVal2" runat="server" ControlToValidate="txtEndDate"
                        Display="Dynamic" ErrorMessage="End Date must be a date." Operator="DataTypeCheck" Type="Date"
                        SetFocusOnError="true" ValidationGroup="ProductFeedGroup" />
                </div>
            </div>
            <div class="col-sm-3">
                <div class="m-2 p-5">
                    <asp:Button runat="server" Text="Refresh" CssClass="btn btn-info" ID="ButtonRefresh" OnClick="ButtonRefresh_Click" />

                </div>
            </div>
            <div class="col-sm-12">
                <asp:CompareValidator ID="cprVal3" runat="server" ControlToValidate="txtStartDate" ControlToCompare="txtEndDate" Operator="LessThanEqual" Type="Date"
                    Display="Dynamic" ErrorMessage="Start Date must be less than end date."
                    SetFocusOnError="true" ValidationGroup="ProductFeedGroup" />
            </div>
            <div class="col-sm-12">
                <rsweb:ReportViewer ID="ProductFeed_ReportViewer" CssClass="reporter table table-bordered table-hover" runat="server" Font-Names="Verdana" Width="100%" Height="700px" Font-Size="8pt" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" ShowRefreshButton="false" PageCountMode="Actual">
                </rsweb:ReportViewer>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('productfeed', 'reports', 'storemanagement');
        });
    </script>
</asp:Content>

