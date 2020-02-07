<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" Theme="WebModules" AutoEventWireup="true"
    CodeBehind="MealLabels.aspx.cs" Inherits="HealthyChef.WebModules.Reports.Admin.MealLabels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>Meal Labels
                </h1>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12">
                </div>
            </div>
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
                    <%--<ajax:CalendarExtender ID="CalExt1" runat="server" TargetControlID="txtStartDate" PopupPosition="TopRight" />--%>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="form-group">
                    <label>End Date:</label>
                    <asp:TextBox runat="server" ID="txtEndDate" ClientIDMode="Static" CssClass="form-control datepicker1"
                        ValidationGroup="MealOrderGroup" />
                    <asp:CompareValidator ID="cprVal2" runat="server" ControlToValidate="txtEndDate"
                        Display="Dynamic" ErrorMessage="End Date must be a date." Operator="DataTypeCheck" Type="Date"
                        SetFocusOnError="true" ValidationGroup="MealOrderGroup" />
                    <%--<ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate" PopupPosition="TopRight" />--%>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="form-group">
                    <label>Meal Type:</label>
                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlMealTypes" ValidationGroup="MealOrderGroup" />
                </div>
            </div>
            <div class="col-sm-3">
                <div class="m-2 p-5">
                    <asp:Button runat="server" Text="Refresh" CssClass="btn btn-info" OnClick="ReportViewer1_ReportRefresh" ID="ButtonRefresh" />

                </div>
            </div>
            <div class="col-sm-12">
                <asp:CompareValidator ID="cprVal3" runat="server" ControlToValidate="txtStartDate" ControlToCompare="txtEndDate" Operator="LessThanEqual" Type="Date"
                    Display="Dynamic" ErrorMessage="Start Date must be less than end date."
                    SetFocusOnError="true" ValidationGroup="MealOrderGroup" />
            </div>
            <div class="col-sm-12">
                <rsweb:ReportViewer ID="ReportViewer1" CssClass="reporter table table-bordered table-hover" runat="server" Font-Names="Verdana" Width="100%" Height="700px" Font-Size="8pt" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" ShowRefreshButton="false" PageCountMode="Actual">
                </rsweb:ReportViewer>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('meallabelreport', undefined, 'storemanagement');
        });
    </script>
    <style>
         /*table {
            width:100% !important;
        }*/
         #Pcfb4c633a9d24433baf095186101cc47_4_oReportDiv .A37ac8019780e471dad389678867735a2160 {
             color:#000 !important;
         }
    </style>
</asp:Content>
