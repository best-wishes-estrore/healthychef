<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="ActiveCustomers.aspx.cs" Inherits="HealthyChef.WebModules.Reports.Admin.ActiveCustomers" Theme="WebModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>Active Customers</h1>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">
                    <asp:Label ID="lblFeedback" runat="server" />
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label>Start Date:</label>
                            <asp:TextBox runat="server" ID="txtStartDate" ClientIDMode="Static" CssClass="form-control datepicker1" ValidationGroup="CustomerGroup" />
                            <%--<ajax:CalendarExtender ID="CalExt1" runat="server" TargetControlID="txtStartDate" PopupPosition="TopRight" />--%>
                            <asp:CompareValidator ID="cprVal1" runat="server" ControlToValidate="txtStartDate"
                                Display="Dynamic" ErrorMessage="Start Date must be a date." Operator="DataTypeCheck" Type="Date"
                                SetFocusOnError="true" ValidationGroup="CustomerGroup" />
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label>End Date:</label>
                            <asp:TextBox runat="server" ID="txtEndDate" ClientIDMode="Static" CssClass="form-control datepicker1"
                                ValidationGroup="CustomerGroup" />
                            <asp:CompareValidator ID="cprVal2" runat="server" ControlToValidate="txtEndDate"
                                Display="Dynamic" ErrorMessage="End Date must be a date." Operator="DataTypeCheck" Type="Date"
                                SetFocusOnError="true" ValidationGroup="CustomerGroup" />
                            
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <%--<ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate" PopupPosition="TopRight" />--%>
                            <label>Product Type:</label>
                            <asp:DropDownList runat="server" ID="ddlProductTypes" CssClass="form-control" ValidationGroup="CustomerGroup" />
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="m-2 p-5">
                            <asp:Button runat="server" CssClass="btn btn-info" Text="Refresh" OnClick="RefreshReport" ID="ButtonRefresh" />
                        </div>
                    </div>
                </div>
                    <div class="m-2 p-5">
                        <asp:CompareValidator ID="cprVal3" runat="server" ControlToValidate="txtStartDate" ControlToCompare="txtEndDate" Operator="LessThanEqual" Type="Date"
                                Display="Dynamic" ErrorMessage="Start Date must be less than end date."
                                SetFocusOnError="true" ValidationGroup="CustomerGroup" />
                    </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="auto-style1">
            <rsweb:ReportViewer ID="rvwCustomers" class="table table-bordered table-hover" runat="server" Height="600px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" ShowRefreshButton="false" Width="100%" PageCountMode="Actual">
            </rsweb:ReportViewer>
        </div>
    </div>
    <script type="text/javascript">
    $(function () {
        ToggleMenus('activecustomers', 'reports', 'storemanagement');
    });
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="header">
    <style type="text/css">
        .auto-style1 {
            position: relative;
            min-height: 1px;
            float: left;
            width: 100%;
            margin-left: 40px;
            padding-left: 15px;
            padding-right: 15px;
        }
    </style>
</asp:Content>

