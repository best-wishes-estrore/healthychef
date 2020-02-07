<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileLedger_Edit.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.UserProfileLedger_Edit" %>
<asp:Panel ID="pnlAddTransaction" runat="server" DefaultButton="btnSaveTransaction">
    <div class="page-header">
        <h1>Add a Transaction:</h1>
    </div>

    <div class="col-sm-3">
        <div class="form-group">
            <label>Type:</label>
            <asp:DropDownList ID="ddlXactTypes" runat="server" CssClass="form-control" ValidationGroup="AddTransactionGroup" AutoPostBack="true" onchange="MakeUpdateProg(true);" />
            <asp:RequiredFieldValidator ID="rfvXactTypes" runat="server" ControlToValidate="ddlXactTypes"
                Display="Dynamic" Text="*" ErrorMessage="An type is required." SetFocusOnError="true" InitialValue="-1"
                ValidationGroup="AddTransactionGroup" />
        </div>
    </div>
    <div class="col-sm-3">
        <div id="divXactAmount" runat="server">
            <div class="form-group">
                <label>Amount:</label>
                <asp:TextBox ID="txtXactAmount" runat="server" MaxLength="10" CssClass="form-control" ValidationGroup="AddTransactionGroup" />
                <asp:RequiredFieldValidator ID="rfvXactAmount" runat="server" ControlToValidate="txtXactAmount"
                    Display="Dynamic" Text="*" ErrorMessage="An amount is required." SetFocusOnError="true"
                    ValidationGroup="AddTransactionGroup" />
                <asp:CompareValidator ID="cmpXactAmount" runat="server" ControlToValidate="txtXactAmount"
                    Display="Dynamic" Text="*" ErrorMessage="Amount must be numeric." SetFocusOnError="true"
                    ValidationGroup="AddTransactionGroup" Type="Double" Operator="DataTypeCheck" />
            </div>
        </div>
    </div>

    <div id="divXactGCRedeem" runat="server" visible="false">
        <div class="col-sm-3">
            <div class="form-group">
                <label>Redeem Code:</label>
                <asp:TextBox ID="txtXactGCRedeem" runat="server" CssClass="form-control" MaxLength="10" />
                <asp:RequiredFieldValidator ID="rfvXactGCRedeem" runat="server" ControlToValidate="txtXactGCRedeem"
                    Display="Dynamic" Text="*" ErrorMessage="Redeem Code is required for Gift Certificate credits."
                    SetFocusOnError="true" ValidationGroup="AddTransactionGroup" />
                <asp:CustomValidator ID="cstXactGCRedeem" runat="server" ControlToValidate="txtXactGCRedeem"
                    Display="Dynamic" Text="*" ErrorMessage="The gift certificate redemption code entered does not exist, or has already been redeemed."
                    SetFocusOnError="true" ValidationGroup="AddTransactionGroup" />
            </div>
        </div>
    </div>
    <div class="col-sm-3">
        <div class="form-group">
            <label>Comments:</label>
            <asp:TextBox ID="txtXactDesc" runat="server" MaxLength="1000" CssClass="form-control" ValidationGroup="AddTransactionGroup" /><br />
            <asp:Button ID="btnSaveTransaction" runat="server" Text="Save Transaction" CssClass="btn btn-info" ValidationGroup="AddTransactionGroup"
                OnClientClick="javascript:return confirm('Are you sure that you want to add this transaction.')" />
            <asp:CustomValidator ID="cstVal1" runat="server" Display="Dynamic" Text="*" ErrorMessage="This transaction appears to be a duplicate."
                SetFocusOnError="true" ValidationGroup="AddTransactionGroup" EnableClientScript="false" />
            <asp:ValidationSummary ID="ValSum1" runat="server" ValidationGroup="AddTransactionGroup" DisplayMode="List" />
            <asp:Label ID="lblXactFeedback" runat="server" ForeColor="Red" EnableViewState="false" />
        </div>
    </div>
</asp:Panel>
<div class="clearfix"></div>
<hr />
<h3>Ledger</h3>
<div>
    <asp:DropDownList ID="ddlTransactionAges" runat="server" AutoPostBack="true">
        <asp:ListItem Text="All Transactions" Value="-1" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Last 30 Days of Transactions" Value="30"></asp:ListItem>
        <asp:ListItem Text="Last 90 Days of Transactions" Value="90"></asp:ListItem>
        <asp:ListItem Text="Last 6 Months of Transactions" Value="180"></asp:ListItem>
        <asp:ListItem Text="Previous Year of Transactions" Value="365"></asp:ListItem>
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;
    Current Account Balance:
    <asp:Label ID="lblAccountBalance" runat="server" />
</div>
<asp:ListView runat="server" ID="lvLedger" DataKeyNames="LedgerID">
    <LayoutTemplate>
        <table class="table table-bordered table hover">
            <tr>
                <th>Date</th>
                <th>Type</th>
                <th>Amount</th>
                <th>Credit from Balance</th>
                <th>Payment</th>
                <th>Remaining Balance</th>
                <th>Notes</th>
            </tr>
            <tr runat="server" id="itemPlaceHolder" />
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Eval("CreatedDate") %></td>
            <td><%# HealthyChef.Common.Enums.GetEnumDescription((HealthyChef.Common.Enums.LedgerTransactionType)Eval("TransactionType")) %></td>
            <td><%# Convert.ToDecimal(Eval("TotalAmount").ToString()).ToString("c") %></td>
            <td><%# Convert.ToDecimal(Eval("CreditFromBalance").ToString()).ToString("c") %></td>
            <td><%# Convert.ToDecimal(Eval("PaymentDue").ToString()).ToString("c") %></td>
            <td><%# Convert.ToDecimal(Eval("PostBalance").ToString()).ToString("c") %></td>
            <td><%# Eval("Description") %> By: <%# System.Web.Security.Membership.GetUser(Guid.Parse(Eval("CreatedBy").ToString())).UserName %></td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr style="background-color: #ddd;">
            <td><%# Eval("CreatedDate") %></td>
            <td><%# HealthyChef.Common.Enums.GetEnumDescription((HealthyChef.Common.Enums.LedgerTransactionType)Eval("TransactionType")) %></td>
            <td><%# Convert.ToDecimal(Eval("TotalAmount").ToString()).ToString("c") %></td>
            <td><%# Convert.ToDecimal(Eval("CreditFromBalance").ToString()).ToString("c") %></td>
            <td><%# Convert.ToDecimal(Eval("PaymentDue").ToString()).ToString("c") %></td>
            <td><%# Convert.ToDecimal(Eval("PostBalance").ToString()).ToString("c") %></td>
            <td><%# Eval("Description") %> By: <%# System.Web.Security.Membership.GetUser(Guid.Parse(Eval("CreatedBy").ToString())).UserName %></td>
        </tr>
    </AlternatingItemTemplate>
    <EmptyDataTemplate>There are no transactions for this user.</EmptyDataTemplate>
</asp:ListView>
<div class="updateProgressContainer">
</div>
<div class="updateProgressDisplay">
    <img src="/App_Themes/HealthyChef/Images/Spinning_wheel_throbber.gif" alt="Loading..." />Loading....
</div>
