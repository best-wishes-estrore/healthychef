<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerCalendar.aspx.cs" Theme="WebModules"
    MasterPageFile="~/Templates/WebModules/Default.master" Inherits="HealthyChef.WebModules.Reports.Admin.CustomerCalendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>Customer Meal Calendar Report
                </h1>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">
                    <asp:Label ID="lblFeedback" runat="server" />
                    <asp:ValidationSummary ID="ValSum1" runat="server" ValidationGroup="MealOrderGroup" />
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label>Start Date:</label>
                            <asp:TextBox runat="server" ID="txtStartDate" ClientIDMode="Static" CssClass="form-control datepicker1"
                                ValidationGroup="MealOrderGroup" />
                            <asp:CompareValidator ID="cprVal1" runat="server" ControlToValidate="txtStartDate"
                                Display="Dynamic" ErrorMessage="Start Date must be a date." Operator="DataTypeCheck" Type="Date"
                                SetFocusOnError="true" ValidationGroup="MealOrderGroup" />
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label>End Date:</label>
                            <asp:TextBox runat="server" ID="txtEndDate" CssClass="form-control datepicker1" ClientIDMode="Static" ValidationGroup="MealOrderGroup" />
                            <%--<ajax:CalendarExtender ID="CalExt1" runat="server" TargetControlID="txtStartDate" PopupPosition="TopRight" />
                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate" PopupPosition="TopRight" />--%>
                            <asp:CompareValidator ID="cprVal2" runat="server" ControlToValidate="txtEndDate"
                                Display="Dynamic" ErrorMessage="End Date must be a date." Operator="DataTypeCheck" Type="Date"
                                SetFocusOnError="true" ValidationGroup="MealOrderGroup" />

                        </div>
                    </div>
                    <div class="col-sm-12">
                        <asp:CompareValidator ID="cprVal3" runat="server" ControlToValidate="txtStartDate" ControlToCompare="txtEndDate" Operator="LessThanEqual" Type="Date"
                            Display="Dynamic" ErrorMessage="Start Date must be less than end date."
                            SetFocusOnError="true" ValidationGroup="MealOrderGroup" />
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" class="table table-bordered table-hover" Height="600px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana"
                WaitMessageFont-Size="14pt" ShowRefreshButton="true" Width="100%" PageCountMode="Actual" OnReportRefresh="ButtonRefresh_Click">
            </rsweb:ReportViewer>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('customercalendar', 'reports', 'storemanagement');
        });
    </script>
</asp:Content>
