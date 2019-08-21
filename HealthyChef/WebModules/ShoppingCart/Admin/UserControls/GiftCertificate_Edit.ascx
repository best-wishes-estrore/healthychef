<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GiftCertificate_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.GiftCertificate_Edit" %>

<%@ Register TagPrefix="hcc" TagName="Address_Edit" Src="~/WebModules/ShoppingCart/Admin/UserControls/Address_Edit.ascx" %>

<div class="plan_edit">
    <asp:Panel ID="pnlGiftCertificateEdit" runat="server" DefaultButton="btnSave">
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblCreatedInfo" runat="server" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <label class="col-sm-2">Redeem Code:</label>
                <asp:Label ID="lblRedeemCode" runat="server" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:HiddenField runat="server" ID="CartId" runat="server" />

                <label class="col-sm-2">Amount: $</label><asp:TextBox ID="txtAmount" runat="server" MaxLength="10" />
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
        <div class="fieldRow">
            <div class="fieldCol">
                <hcc:Address_Edit ID="AddressRecipient" runat="server" ShowValidationSummary="false" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <label class="col-sm-2">Recipient Email:</label>
                <asp:TextBox ID="txtRecipEmail" runat="server" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <label class="col-sm-2">Recipient Message:</label>
                <asp:TextBox ID="txtRecipMessage" runat="server" TextMode="MultiLine" Columns="50" Rows="3" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol col-sm-2">
                <asp:CheckBox ID="chkSentToRecip" runat="server" Text="Sent to Recipient?" Visible="false" ClientIDMode="Static" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:Label ID="lblRedeemedInfo" CssClass="col-sm-2" runat="server" />
            </div>
        </div>
        <div class="fieldRow">
            <div class="fieldCol">
                <asp:ValidationSummary ID="ValSumGiftCertificates1" runat="server" />
            </div>
        </div>
    </asp:Panel>
    <div class="fieldRow">
        <div class="fieldCol">
            <div class="m-2">
                <asp:Button ID="btnSave" runat="server" class="btn btn-info" OnClientClick="javascript:BtnSaveClick();" Text="Save" Visible="true" />
               <%-- <button type="button" class="btn btn-info" ng-click="SaveGiftCertificate()" id="SaveButton">Save</button>--%>
                <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" CausesValidation="false" Visible="false" />
                <a href="/WebModules/ShoppingCart/Admin/GiftCertificationManager.aspx" class="btn btn-danger">Cancel</a>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    $("#chkSentToRecip").change(function () {
        var c = confirm("Are you sure that you want to mark this item as having been sent to the recipient?");

        if (!c) {
            $("#chkSentToRecip").prop('checked', !($("#chkSentToRecip").is(":checked")));
        }
    });

    $(document).ready(function () {
        //hide save button for address
        $('#SaveAddressButton').hide();
        $('#ctl00_Body_UserProfileEdit1_ProfileCartEdit1_GiftCertEdit1_AddressRecipient_SaveAddressButton').hide();

        //hide save button for main form
        var _activeTab = localStorage.getItem('currenttab');
        if (_activeTab !== '' && _activeTab !== undefined && _activeTab !== null) {
            if (_activeTab === '1') {
                $('#SaveButton').addClass('disabled');
            }
        }

    });
    function BtnSaveClick() {
        $("#ctl00_Body_UserProfileEdit1_ProfileCartEdit1_GiftCertEdit1_btnSave").css('visibility', 'hidden');
    }
       
</script>