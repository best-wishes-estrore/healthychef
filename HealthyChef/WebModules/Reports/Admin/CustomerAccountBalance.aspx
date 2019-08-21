<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="CustomerAccountBalance.aspx.cs"
    Inherits="HealthyChef.WebModules.Reports.Admin.CustomerAccountBalance" Theme="WebModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>Customer Account Balance
                </h1>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">

                    <asp:Label ID="lblFeedback" runat="server" />
                    <asp:ValidationSummary ID="ValSum1" runat="server" ValidationGroup="MealOrderGroup" />

                    <div class="col-sm-3">
                        <div class="form-group">
                            <label>Delivery Date:</label>
                            <asp:TextBox runat="server" ID="txtStartDate" ClientIDMode="Static" CssClass="form-control datepicker1" ValidationGroup="MealOrderGroup"></asp:TextBox>
                            <%--<ajax:CalendarExtender ID="CalExt1" runat="server" TargetControlID="txtStartDate" PopupPosition="TopRight" />--%>
                            <asp:RequiredFieldValidator ID="rfvVal1" runat="server" ControlToValidate="txtStartDate"
                                Display="Dynamic" ErrorMessage="Date is required." SetFocusOnError="true" ValidationGroup="MealOrderGroup" />
                            <asp:CompareValidator ID="cprVal1" runat="server" ControlToValidate="txtStartDate"
                                Display="Dynamic" ErrorMessage="Start Date must be a date." Operator="DataTypeCheck" Type="Date"
                                SetFocusOnError="true" ValidationGroup="MealOrderGroup" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="col-sm-12">
                <rsweb:ReportViewer ID="ReportViewer1" class="table table-bordered table-hover" runat="server" Height="600px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" ShowRefreshButton="true" Width="100%" PageCountMode="Actual">
                </rsweb:ReportViewer>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('customeraccountbalance', 'reports', 'storemanagement');
        });
    </script>
</asp:Content>


