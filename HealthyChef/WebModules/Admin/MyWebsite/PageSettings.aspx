<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true"
    Inherits="BayshoreSolutions.WebModules.Admin.MyWebsite.PageSettings" Title="Page Settings"
    StylesheetTheme="WebModules" Theme="WebModules" Codebehind="PageSettings.aspx.cs" %>
<%@ Register Assembly="BayshoreSolutions.WebModules" Namespace="BayshoreSolutions.WebModules.TemplateProperties" TagPrefix="wmtp" %>
<%@ Register Src="../../Components/PagePicker/PagePicker.ascx" TagName="PagePicker" TagPrefix="uc1" %>
<%@ Register Src="../Controls/PathNameEdit.ascx" TagName="PathNameEdit" TagPrefix="uc1" %>
<%@ Register Src="~/WebModules/SitemapXml/Controls/SitemapXmlEdit.ascx" TagName="SitemapXmlEdit" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="Server">
      <div class="page-header">
<h1>Page Settings</h1>
          </div>

<bss:MessageBox ID="Msg" runat="server" />

<div class="entity_edit" style="padding:15px">

<table style="width: 100%;">
<tr>
<td>
    <div class="field">
        <span>Title</span>
        <div class="help">Culture-dependent.</div>
        <div>
            <asp:TextBox ID="TitleTextBox" Width="300px" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
                ControlToValidate="TitleTextBox" />
        </div>
    </div>
    <div class="field">
        <span>Navigation Text</span>
        <div class="help">Culture-dependent.</div>
        <div>
            <asp:TextBox ID="NavigationTextTextBox" Width="300px" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                ControlToValidate="NavigationTextTextBox" />
        </div>
    </div>
    <div class="field">
        <%--<span></span>--%>
        <%--<div class="help"></div>--%>
        <div class="PageSettings">
            <asp:CheckBox ID="VisibleCheckBox" runat="server" />
            <label>Visible in navigation</label>
        </div>
    </div>
    <div class="field">
        <span>Keywords</span>
        <div class="help">Culture-dependent.</div>
        <div>
            <asp:TextBox ID="MetaKeywordsTextbox" Width="300px" runat="server" />
        </div>
    </div>
    <div class="field">
        <span>Description</span>
        <div class="help">Culture-dependent.</div>
        <div>
            <asp:TextBox ID="MetaDescriptionTextbox" Width="300px" runat="server" />
        </div>
    </div>
    <%--<div class="field">
        <span>Short Description</span>
        <div class="help">Culture-dependent.</div>
        <div>
            <asp:TextBox ID="DescriptionText" Width="300px" runat="server" />
            <div>Description used in the auto-generated sitemap. May be displayed in other pages lists.</div>
        </div>
    </div>--%>
    <div>
        <uc1:PathNameEdit ID="PathNameEditCtl" runat="server" />
    </div>
    <div id="ParentPage_div" runat="server" class="field">
        <span>Parent Page</span>
        <%--<div class="help"></div>--%>
        <div>
            <uc1:PagePicker ID="ParentPage" runat="server" IsRequired="true" />
        </div>
    </div>
    <div >
			<span>Sitemap.xml Settings</span>
			<fieldset>
				<uc1:SitemapXmlEdit ID="sitemapXmlEdit" runat="server" />
			</fieldset>
    </div>
    
    <div class="field">
        <span>Template</span>
        <%--<div class="help"></div>--%>
        <div>
            <asp:DropDownList ID="TemplatesDropDownList" runat="server" AppendDataBoundItems="true" />
        </div>
    </div>
    <div class="field">
        <span>Theme</span>
        <%--<div class="help"></div>--%>
        <div>
            <asp:DropDownList ID="ThemeDropDownList" runat="server" AppendDataBoundItems="True">
                <asp:ListItem Value="">Default</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div runat="server" id="TemplateProperties_div" class="field">
        <span><em><%= _page.Template %></em> Template Settings</span>
        <%--<div class="help"></div>--%>
        <div>
            <wmtp:TemplatePropertiesControl ID="TemplatePropertiesDisplay" runat="server"/>
        </div>
    </div>

</td>
</tr>
</table>

<div class="toolbar">
    <asp:Button ID="UpdatePageSettings" runat="server" 
        CausesValidation="True" 
        Text="Save" 
        OnClick="UpdatePageSettings_Click" 
        CssClass="saveButton btn btn-info"
    />
    <asp:Button ID="CancelPageSettings" runat="server" 
        Text="Cancel" 
        CausesValidation="False" 
        OnClick="CancelPageSettings_Click" 
        CssClass="cancelButton btn btn-danger"
    />
</div>
    
</div>
  <script type="text/javascript">
      $(function () {
          ToggleMenus('websites', undefined, undefined);
      });
    </script>  
</asp:Content>
