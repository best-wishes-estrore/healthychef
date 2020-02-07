<%@ Page Title="Sales Report" Language="C#" AutoEventWireup="true" Theme="WebModules" MasterPageFile="~/Templates/WebModules/Default.master"
    CodeBehind="SalesReport.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="HealthyChef.WebModules.Reports.Admin.SalesReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>Sales Detail Report</h1>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">
                    <asp:Label ID="lblFeedback" runat="server" />
                    <asp:ValidationSummary ID="ValSum1" runat="server" ValidationGroup="MealOrderGroup" />
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label>Start Date:</label>
                            <asp:TextBox runat="server" ID="txtStartDate" ClientIDMode="Static" CssClass="form-control datepicker1" ValidationGroup="MealOrderGroup" />
                            <asp:RequiredFieldValidator ID="rfvVal1" runat="server" ControlToValidate="txtStartDate"
                                Display="Dynamic" ErrorMessage="Start Date is required."
                                SetFocusOnError="true" ValidationGroup="MealOrderGroup" />
                            <asp:CompareValidator ID="cprVal1" runat="server" ControlToValidate="txtStartDate"
                                Display="Dynamic" ErrorMessage="Start Date must be a date." Operator="DataTypeCheck" Type="Date"
                                SetFocusOnError="true" ValidationGroup="MealOrderGroup" />
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label>End Date:</label>
                            <asp:TextBox runat="server" ID="txtEndDate" CssClass="form-control datepicker1" ClientIDMode="Static"
                                ValidationGroup="MealOrderGroup" />
                            <%--<ajax:CalendarExtender ID="CalExt1" runat="server" TargetControlID="txtStartDate" PopupPosition="TopRight" />--%>

                            <%--<ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate" PopupPosition="TopRight" />--%>
                            <asp:RequiredFieldValidator ID="rfvVal2" runat="server" ControlToValidate="txtEndDate"
                                Display="Dynamic" ErrorMessage="End Date is required."
                                SetFocusOnError="true" ValidationGroup="MealOrderGroup" />
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
            <div class="col-sm-12">
                
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" class="table table-bordered table-hover"  Height="600px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" ShowRefreshButton="true" PageCountMode="Actual" AsyncRendering="false">
                </rsweb:ReportViewer>
            </div>
            <script type="text/javascript">
                $(function () {
                    ToggleMenus('salesdetailreport', 'reports', 'storemanagement');
                });
            </script>
        </div>
    </div>
</asp:Content>
