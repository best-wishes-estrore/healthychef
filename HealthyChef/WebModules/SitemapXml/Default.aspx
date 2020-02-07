<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" StylesheetTheme="WebModules" Theme="WebModules" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BayshoreSolutions.WebModules.SitemapXml.Default" %>

<%@ Register Src="~/WebModules/SitemapXml/Controls/SitemapXmlEdit.ascx" TagPrefix="uc" TagName="SitemapXmlEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">

    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>Sitmap.xml Settings</h1>
            </div>
            <div class="row-fluid">
                <div class="col-sm-12 align-label">
                    <asp:CheckBox
                        ID="checkGenerateSitemap"
                        runat="server"
                        Text="Generate sitemap.xml"
                        AutoPostBack="true"
                        OnCheckedChanged="checkGenerateSitemap_CheckedChanged" />


                    <div class="form-group">
                        <asp:Label ID="labelFileName" runat="server" Text="File Name"></asp:Label>
                        <asp:TextBox ID="txtFileName" CssClass="form-control" runat="server" /><br />
                    </div>

                    <div class="col-sm-12">
                        <span style="color: Gray">Choose a filename for sitemap.xml file or leave blank to use '/sitemap.xml'.<br />
                            The file location will be '/sitemap.ashx' for sites running in classic mode.</span>
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <div class="m-2"></div>
            <div class="row-fluid">
                <div class="col-sm-12">
                    <fieldset>
                        <legend>
                            <span class="page-header">
                                <asp:Label ID="labelDefaultSettings" runat="server" Text="Default Page Settings"></asp:Label>
                            </span>
                        </legend>
                        <uc:SitemapXmlEdit ID="sitemapXmlEdit" runat="server" />

                    </fieldset>

                    <div class="col-sm-12">
                        <div class="m-2">
                            <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save Changes" CssClass="btn btn-info" CausesValidation="true"></asp:LinkButton>&nbsp;
		                <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-info" Text="Cancel"></asp:LinkButton>

                            <asp:LinkButton ID="btnAddAll" runat="server" Text="Add all pages to sitemap" OnClientClick="return confirm('Add all pages using above defaults?');" CssClass="btn btn-info" OnClick="btnAddAll_Click" CausesValidation="true"></asp:LinkButton>
                            <asp:LinkButton Style="margin-left: 20px;" ID="btnRemoveAll" runat="server" Text="Remove all pages from sitemap" OnClientClick="return confirm('Remove all pages from sitemap?');" CssClass="btn btn-danger" OnClick="btnRemoveAll_Click"></asp:LinkButton>
                        </div>
                    </div>

                    <bss:MessageBox ID="messageBox" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
    $(function () {
        ToggleMenus('system', undefined, undefined);
    });
    </script>
</asp:Content>
