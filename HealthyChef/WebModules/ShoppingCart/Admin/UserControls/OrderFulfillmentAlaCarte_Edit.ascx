<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderFulfillmentAlaCarte_Edit.ascx.cs"
    Inherits="HealthyChef.WebModules.ShoppingCart.Admin.UserControls.OrderFulfillmentAlaCarte_Edit" %>

<%@ Register TagPrefix="hcc" TagName="ProfileNotesEdit" Src="~/WebModules/ShoppingCart/Admin/UserControls/UserProfileNote_Edit.ascx" %>

<link rel="stylesheet" type="text/css" href="/App_Themes/WebModules/cartadmin.css" />
<div class="main-content">
        <div class="main-content-inner">
<asp:LinkButton ID="lkbBack" runat="server" CssClass="btn btn-info b-10" Text="<< Back to Listing" PostBackUrl="~/WebModules/ShoppingCart/Admin/OrderFulfillment.aspx"
    CausesValidation="false" OnClientClick="MakeUpdateProg(true);" />
</div>
<div class="b-10">
<div class="fieldRow">
    <div class="fieldCol">
        <asp:Label ID="lblFeedback" runat="server" EnableViewState="false" />
        <table style="width: 100%;">
            <tr>
                <td>
                    <p>
                        <asp:Button ID="btnSave"  CssClass="btn btn-info" runat="server" Text="Save" OnClientClick="MakeUpdateProg(true);" Visible="false" />
                    </p>
                </td>
                <td>
                    <p class="label2">Customer  Name:&nbsp;</p>
                    <asp:Label ID="lblCustomerName" runat="server" /><br />
                    <p class="label2">Profile Name:&nbsp;</p>
                    <asp:Label ID="lblProfileName" runat="server" /><br />
                    <p class="label2">Order Number:&nbsp;</p>
                    <asp:Label ID="lblOrderNumber" runat="server" /><br />
                    <p class="label2">Delivery Date:&nbsp;</p>
                    <asp:Label ID="lblDeliveryDate" runat="server" />
                </td>
                <td>
                    <p class="label">Allergens:</p>
                    <asp:ListView ID="lvwAllrgs" runat="server" GroupItemCount="2">
                        <LayoutTemplate>
                            <ul>
                                <li id="groupPlaceHolder" runat="server"></li>
                            </ul>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <li id="itemPlaceHolder" runat="server"></li>
                        </GroupTemplate>
                        <ItemTemplate>
                            <li><%# Eval("Name") %></li>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            No Allergens.
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <hcc:ProfileNotesEdit ID="ProfileNotesEdit_AllergenNote" runat="server" CurrentNoteType="AllergenNote" ShowAllNotes="true" AllowDisplayToUser="true" UserDisplayNotesTitle="Allergen Notes:" />
                </td>
                <td>
                    <p class="label">Preferences:</p>
                    <asp:ListView ID="lvwPrefs" runat="server">
                        <LayoutTemplate>
                            <ul>
                                <li id="itemPlaceHolder" runat="server"></li>
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li><%# Eval("Name") %></li>
                        </ItemTemplate>
                        <EmptyDataTemplate>No Preferences.</EmptyDataTemplate>
                    </asp:ListView>
                </td>
                <td>
                    <hcc:ProfileNotesEdit ID="ProfileNotesEdit_PreferenceNote" runat="server" CurrentNoteType="PreferenceNote" ShowAllNotes="true" AllowDisplayToUser="true" UserDisplayNotesTitle="Preference Notes:" />
                </td>
            </tr>
        </table>
    </div>
</div>
    <div class="col-sm-12">
<asp:GridView ID="gvwALCItems" runat="server" DataKeyNames="CartItemID" class="table  table-bordered table-hover"
    AutoGenerateColumns="false" OnSelectedIndexChanged="gvwALCItems_SelectedIndexChanged">
    <Columns>
        <asp:BoundField DataField="OrderNumber" HeaderText="Order Number" />
        <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
        <asp:BoundField DataField="SimpleStatus" HeaderText="Status" />
        <asp:BoundField DataField="TotalItemPrice" HeaderText="Item Price" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
        <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-HorizontalAlign="Center" />
        <asp:CommandField ShowSelectButton="true" />
    </Columns>
</asp:GridView>
        </div>
    <div class="clearfix"></div>
    <div class="p-5">
<asp:Panel ID="pnlItemEdit" runat="server" Visible="false">
    <fieldset>
        <legend>Item Details</legend>
        <div class="fieldRow">
            <div class="fieldCol">
                <p class="label2 col-md-2">Is Cancelled:&nbsp;</p>
                <asp:CheckBox ID="chkIsCancelledDisplay" runat="server" ClientIDMode="Static" Enabled="false" /><br />
                <p class="label2 col-md-2">Is Complete:&nbsp;</p>
                <asp:CheckBox ID="chkIsComplete" runat="server" ClientIDMode="Static" /><br />
                <p class="label2 col-md-2">Item Name:&nbsp;</p>
                <div class="drop col-md-offset-2">
                <asp:DropDownList ID="ddlMenuItems" runat="server" AutoPostBack="true" width="400px" OnSelectedIndexChanged="ddlMenuItems_SelectedIndexChanged" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlMenuItems" Display="Dynamic"
                    Text="*" ErrorMessage="A menu item is required." SetFocusOnError="true" InitialValue="-1" EnableClientScript="false" />
                <asp:Panel ID="pnlMealSides" runat="server" Visible="false">
                    <%--<p class="">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>--%>
                    <asp:DropDownList ID="ddlMealSide1MenuItems" runat="server" AutoPostBack="true" /><br />
                    <%--<p class="">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</p>--%>
                    <asp:DropDownList ID="ddlMealSide2MenuItems" runat="server" AutoPostBack="true" />
                </asp:Panel>
                     </div>
                <asp:DropDownList ID="ddlOptions" runat="server" AutoPostBack="true" Enabled="false" />
                <asp:RequiredFieldValidator ID="rfvOptions" runat="server" ControlToValidate="ddlOptions" EnableClientScript="false"
                    Text="*" ErrorMessage="An option is required." SetFocusOnError="true" InitialValue="-1" Display="Dynamic" />
                   
                <div id="divPreferences" runat="server" visible="false">
                    <asp:CheckBoxList ID="cblPreferences" runat="server" />
                </div>
            </div>
        </div>
        <asp:ValidationSummary runat="server" />
    </fieldset>
</asp:Panel>
    </div>
</div>
    </div>
<style>
    .drop select {
        margin:3px 0px;
        width:300px;
    }
</style>