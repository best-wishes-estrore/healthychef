<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GiftCertificateAddControl.ascx.cs" Inherits="HealthyChef.WebModules.ShoppingCart.Controls.GiftCertificateAddControl" %>

<%@ Register TagPrefix="hcc" TagName="Address_Edit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Address_Edit.ascx" %>

<script type="text/javascript">
    $(document).ready(function () {
        var hasData = $("#hdnHasFormData").val();
        if (hasData == "true") {
            $("#divBuy").hide();
            $("#divPanel").show();
        }
        else {
            $("#divBuy").show();
            $("#divPanel").hide();
        }

        $("#divBuy").click(function () {
            $("#divPanel").show(1000);
            $("#divBuy").hide();
        });

        $("#divSave").click(function () {
            $("#hdnHasFormData").val("true");
            __doPostBack('divSave', '');
        });

        $("#divCancel").click(function () {
            $("#hdnHasFormData").val("false");
            __doPostBack('divCancel', '');
        });
    });
</script>

<style type="text/css">
    .content-gift-certificate
    {
    }

        .content-gift-certificate label
        {
            display: inline-block;
            min-width: 65px;
            text-align: right;
        }

        .content-gift-certificate input
        {
            display: inline-block;
        }
</style>

<asp:HiddenField ID="hdnHasFormData" runat="server" ClientIDMode="Static" Value="false" EnableViewState="true" />

<div id="divBuy" class="buttons">
    <a class="orange"><span>Buy Gift Certificate</span></a>
</div>
<div id="divPanel" style="display: none;">
    <asp:Panel ID="pnlGiftCertificateAdd" CssClass="content-gift-certificate" runat="server">
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lbl_txtAmount" AssociatedControlID="txtAmount" Text="Amount: $" runat="server" />
                <asp:TextBox ID="txtAmount" runat="server" MaxLength="10" Width="80px" />
                <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount"
                    Text="*" Display="Dynamic" ErrorMessage="An amount is required." SetFocusOnError="true" />
                <asp:CompareValidator ID="cprAmount" runat="server" ControlToValidate="txtAmount"
                    Text="*" Display="Dynamic" ErrorMessage="Amount must be numeric." SetFocusOnError="true"
                    Operator="DataTypeCheck" Type="Double" />
                <asp:CompareValidator ID="rngAmount" runat="server" ControlToValidate="txtAmount"
                    Text="*" Display="Dynamic" ErrorMessage="Amount must be greater than $0.00."
                    SetFocusOnError="true" Operator="GreaterThan" ValueToCompare="0.00" />
            </div>
        </div>
        <div>&nbsp;</div>
        <div class="fieldRow">
            <div class="fieldCol">
                Recipient Information:
                        <hcc:Address_Edit ID="AddressRecipient" runat="server" ShowSave="false" ShowValidationSummary="false" ShowIsBusiness="false" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lbl_txtRecipEmail" AssociatedControlID="txtRecipEmail" Text="Recipient Email:" runat="server" />
                <asp:TextBox ID="txtRecipEmail" Style="width: 100%;" runat="server" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lbl_txtRecipMessage" AssociatedControlID="txtRecipMessage" Text="Recipient Message:" runat="server" />
                <asp:TextBox ID="txtRecipMessage" runat="server" TextMode="MultiLine" Columns="40" Rows="3" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblRedeemedInfo" runat="server" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <div id="divSave" class="buttons">
                    <a class="orange"><span>Add to Cart</span></a>
                </div>
                <div id="divCancel" class="buttons">
                    <a class="orange"><span>Cancel</span></a>
                </div>
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:ValidationSummary ID="ValSumGiftCertificates1" runat="server" />
            </div>
        </div>
    </asp:Panel>
    <asp:Label ID="lblFeedback" runat="server" EnableViewState="false" ForeColor="Green" />
</div>
