<%@ Page Title="AuthorizeNet Settings" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Templates/WebModules/Default.master"
    CodeBehind="Default.aspx.cs"
    Inherits="HealthyChef.WebModules.AuthNetModule._default"
    Theme="WebModules"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="p-5">
                <div class="pull-right">
                    <a id="MainMenuLink" runat="server" class="btn btn-info">Return to Main Menu</a>
                </div>
                <div class="clearfix"></div>
            </div>
            
            <div class="page-header">
                <h1>AuthorizeNet Settings</h1>
            </div>
            <div class="col-sm-6">
                <div class="authnet row">
                    <span class="authnet field">Name:</span>
                    <span class="authnet value">
                        <asp:Literal runat="server" ID="ltlAuthNetName"></asp:Literal></span>
                </div>

                <div class="authnet row">
                    <span class="authnet field">API Key:</span>
                    <span class="authnet value">
                        <asp:Literal runat="server" ID="ltlAuthNetApiKey"></asp:Literal></span>
                </div>

                <div class="authnet row">
                    <span class="authnet field">Transaction Key:</span>
                    <span class="authnet value">
                        <asp:Literal runat="server" ID="ltlAuthNetTransactionKey"></asp:Literal></span>
                </div>

                <div class="authnet row">
                    <span class="authnet field">Mode:</span>
                    <span class="authnet value">
                        <asp:Literal runat="server" ID="ltlAuthNetMode"></asp:Literal></span>
                </div>
                <div class="m-2">
                    <asp:Label ID="lblTest" runat="server" />
                </div>

                <%--<div class="authnet row">
        <span>Test Cards:</span>
    </div>

    
    <asp:ListView runat="server" ID="TestCards">
        <LayoutTemplate>
            <div class="authnet row">
                <div runat="server" id="itemPlaceHolder"></div>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <fieldset>
                <legend><asp:Literal runat="server" ID="litname" Text='<%#Eval("TestCardName")%>'></asp:Literal></legend>
                <div>
                    <span class="authnet field">Card Type:</span>
                    <span class="authnet value"><asp:Literal runat="server" ID="Literal1" Text='<%#Eval("TestCardType")%>'></asp:Literal></span>
                </div>
                <div>
                    <span class="authnet field">Card Number:</span>
                    <span class="authnet value"><asp:Literal runat="server" ID="Literal7" Text='<%#Eval("TestCardNumber")%>'></asp:Literal></span>
                </div>
                <div>
                    <span class="authnet field">CVV:</span>
                    <span class="authnet value"><asp:Literal runat="server" ID="Literal2" Text='<%#Eval("TestCardCVV")%>'></asp:Literal></span>
                </div>
                <div>
                    <span class="authnet field">Expiration Month Offset:</span>
                    <span class="authnet value"><asp:Literal runat="server" ID="Literal3" Text='<%#Eval("TestCardExpirationMonthOffset")%>'></asp:Literal></span>
                </div>
                <div>
                    <span class="authnet field">Expiration Year Offset:</span>
                    <span class="authnet value"><asp:Literal runat="server" ID="Literal4" Text='<%#Eval("TestCardExpirationYearOffset")%>'></asp:Literal></span>
                </div>
                <div>
                    <span class="authnet field">Expiration Date:</span>
                    <span class="authnet value"><asp:Literal runat="server" ID="Literal5" Text='<%#Eval("TestCardExpiration")%>'></asp:Literal></span>
                </div>
            </fieldset>
        </ItemTemplate>
    </asp:ListView>--%>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            ToggleMenus('system', undefined, undefined);
        });
    </script>
</asp:Content>
