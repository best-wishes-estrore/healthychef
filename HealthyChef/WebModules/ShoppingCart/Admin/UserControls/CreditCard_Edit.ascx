<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreditCard_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.CreditCard_Edit" %>
<style type="text/css">
    .cvvContainer
    {
        position: fixed;
        top: 0px;
        bottom: 0px;
        left: 0px;
        right: 0px;
        overflow: hidden;
        padding: 0;
        margin: 0;
        background-color: #333;
        filter: alpha(opacity=90);
        opacity: 0.9;
        z-index: 1000;
    }

    .cvvDisplay
    {
        text-align: center;
        vertical-align: middle;
        position: fixed;
        top: 10%;
        left: 25%;
        padding: 10px;
        width: 600px;
        height: 500px;
        z-index: 1001;
        background-color: #fff;
        border: solid 1px #000;
        filter: alpha(opacity=100);
        opacity: 1.0;
        overflow: hidden;
    }
</style>
<div>
    <asp:Label ID="lblFeedback" runat="server" ForeColor="Green" EnableViewState="false" />
    <asp:Label ID="lblErrorOnAuth" runat="server" ForeColor="Red" EnableViewState="false" />
</div>
<asp:Panel ID="pnlCardInfo" runat="server">
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:Label ID="lblNameOnCard" AssociatedControlID="txtNameOnCard" Text="Name on Card:" runat="server" />
            <asp:TextBox ID="txtNameOnCard" runat="server" Width="140px" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvNameOnCard" runat="server" ControlToValidate="txtNameOnCard"
                Text="*" Display="Dynamic" ErrorMessage="Name on Card is required." />
        </div>
    </div>
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:Label ID="Label1" AssociatedControlID="txtCCNumber" Text="Card Number:" runat="server" />
            <asp:TextBox ID="txtCCNumber" runat="server" Width="140px" MaxLength="19" />
            <asp:RequiredFieldValidator ID="rfvCCNumber" runat="server" ControlToValidate="txtCCNumber"
                Text="*" Display="Dynamic" ErrorMessage="Card Number is required." />
            <asp:CustomValidator ID="cvCardNumCCNumber" runat="server" ControlToValidate="txtCCNumber"
                Text="*" Display="Dynamic" />
        </div>
    </div>
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:Label ID="Label3" AssociatedControlID="ddlExpMonth" Text="Expiration Date:" runat="server" />
            <asp:DropDownList ID="ddlExpMonth" runat="server" CssClass="dropDownList col-sm-1">
                <asp:ListItem Text="---" Value="-1"></asp:ListItem>
                <asp:ListItem Value="1">01</asp:ListItem>
                <asp:ListItem Value="2">02</asp:ListItem>
                <asp:ListItem Value="3">03</asp:ListItem>
                <asp:ListItem Value="4">04</asp:ListItem>
                <asp:ListItem Value="5">05</asp:ListItem>
                <asp:ListItem Value="6">06</asp:ListItem>
                <asp:ListItem Value="7">07</asp:ListItem>
                <asp:ListItem Value="8">08</asp:ListItem>
                <asp:ListItem Value="9">09</asp:ListItem>
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="11">11</asp:ListItem>
                <asp:ListItem Value="12">12</asp:ListItem>
            </asp:DropDownList>
           <p style="float:left">&nbsp; / &nbsp; </p>
        <asp:DropDownList ID="ddlExpYear" runat="server" AppendDataBoundItems="false" CssClass="col-sm-1" />
            <asp:RequiredFieldValidator ID="rfvExpYear" runat="server" ControlToValidate="ddlExpYear"
                Text="*" Display="Dynamic" ErrorMessage="Year is required." InitialValue="-1" />
            <asp:RequiredFieldValidator ID="rfvExpMonth" runat="server" ControlToValidate="ddlExpMonth"
                Text="*" Display="Dynamic" ErrorMessage="Month is required." InitialValue="-1" />
            <asp:CustomValidator ID="cvExpMonth" runat="server" ControlToValidate="ddlExpMonth"
                Text="*" Display="Dynamic" />
        </div>
    </div>
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:Label ID="Label4" AssociatedControlID="txtCCAuthCode" Text="Card ID Code:" runat="server" />
            <asp:TextBox ID="txtCCAuthCode" runat="server" Width="40px" MaxLength="4" />

            <asp:RequiredFieldValidator ID="rfvCCAuthCode" runat="server" ControlToValidate="txtCCAuthCode"
                Text="*" Display="Dynamic" ErrorMessage="Card ID Code is required." />
            <asp:CustomValidator ID="cvCCAuthCode" runat="server" ControlToValidate="txtCCAuthCode"
                Text="*" Display="Dynamic" ErrorMessage="Enter a valid Card ID Code." />
            <a class="ccvInfo" onclick="javascript:CVVClick();">?</a>
            <div class="cvvContainer" style="display: none;">
                <div class="cvvDisplay">
                    <a class="ccvInfo" onclick="javascript:CVVClick();">Close</a>
                    <br />
                    <iframe src="/cvv.html" style="width: 100%; height: 100%;"></iframe>
                </div>
            </div>
        </div>
    </div>
    
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:Button ID="btnSave" runat="server" Text="Save Card" CssClass="pixels-button-user pixels-button-solid-green-user btn btn-info" Visible="false" />
        </div>
    </div>
    <div class="fieldRow">
        <div class="fieldCol">
            <asp:ValidationSummary ID="ValSum1" runat="server" />
            <asp:CustomValidator ID="cstValCardInfo0" runat="server" Enabled="false" Text="*" Display="Dynamic" />
        </div>
    </div>
     <asp:Label ID="lblcreditcardfeedback" runat="server" ForeColor="Green" />
</asp:Panel>
<script type="text/javascript">
    function CVVClick() {
        if ($(".cvvContainer").is(":visible")) {
            $(".cvvContainer").hide();
        }
        else {
            $(".cvvContainer").show();
        }
    }
</script>
