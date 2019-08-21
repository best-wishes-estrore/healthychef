<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SitemapXmlEdit.ascx.cs" Inherits="BayshoreSolutions.WebModules.SitemapXml.Controls.SitemapXmlEdit" %>

<table class="sitemapxml">
    <tr>
         <td width="50%">
            <asp:CheckBox ID="checkSitemapXmlIncludePage" runat="server" AutoPostBack="true" OnCheckedChanged="checkSitemapXmlIncludePage_CheckedChanged" Checked="true" Text="Include Page in sitemap.xml" /><br />
        </td>
    </tr>
	<tr>       
		<td width="50%">
			<asp:CheckBox ID="checkSitemapXmlIncludeLastMod" runat="server" Checked="true" Text="Include lastmod" AutoPostBack="true" OnCheckedChanged="checkSitemapXmlIncludeLastmod_CheckedChanged" /> 
		</td>
		<td>
			<asp:Label ID="labelOverride" runat="server" AssociatedControlID="txtSitemapXmlLastMod">override:</asp:Label> <asp:TextBox ID="txtSitemapXmlLastMod" runat="server"  />
			<asp:CustomValidator
				id="cvSitemapXmlLastMod"
				runat="server"
				ControlToValidate="txtSitemapXmlLastMod"
				Display="Dynamic"
				OnServerValidate="cvSitemapXmlLastMod_ServerValidate"
				ErrorMessage="<br />Please use this format (or leave blank):  yyyy/mm/dd, e.g. 2010/03/17"
				></asp:CustomValidator>
				<br />
				<span style="color:Gray">The sitemap lastmod element will contain the most recent modification date of the page or contained modules unless an override is specified.</span>
		</td>
	</tr>
	<tr>
		<td>
			<asp:CheckBox ID="checkSitemapXmlIncludeChangeFreq" runat="server" AutoPostBack="true" OnCheckedChanged="checkSitemapXmlIncludeChangeFreq_CheckedChanged" Checked="true" Text="Include changefreq" />
		</td>
		<td>
			<asp:DropDownList ID="ddlChangeFreq" runat="server">
				<asp:ListItem Text="always" Value="always"></asp:ListItem>
				<asp:ListItem Text="hourly" Value="hourly"></asp:ListItem>
				<asp:ListItem Text="daily" Value="daily"></asp:ListItem>
				<asp:ListItem Text="weekly" Value="weekly"></asp:ListItem>
				<asp:ListItem Text="monthly" Value="monthly" Selected="True"></asp:ListItem>
				<asp:ListItem Text="yearly" Value="yearly"></asp:ListItem>
				<asp:ListItem Text="never" Value="never"></asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td>
			<asp:CheckBox ID="checkSitemapXmlIncludePriority" runat="server" AutoPostBack="true" OnCheckedChanged="checkSitemapXmlIncludePriority_CheckedChanged" Text="Include Priority" />
		</td>
		<td>
			<asp:DropDownList ID="ddlSitemapXmlPriorityOnes" CssClass="col-sm-5" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSitemapXmlPriorityOnes_SelectedIndexChanged">
				<asp:ListItem Text="1" Value="1"></asp:ListItem>
				<asp:ListItem Text="0" Value="0" Selected="True"></asp:ListItem>
			</asp:DropDownList>
            <asp:DropDownList  CssClass="col-sm-5
                " ID="ddlSitemapXmlPriorityTenths" runat="server">
				<asp:ListItem Text="9" Value="9"></asp:ListItem>
				<asp:ListItem Text="8" Value="8"></asp:ListItem>
				<asp:ListItem Text="7" Value="7"></asp:ListItem>
				<asp:ListItem Text="6" Value="6"></asp:ListItem>
				<asp:ListItem Text="5" Value="5"></asp:ListItem>
				<asp:ListItem Text="4" Value="4"></asp:ListItem>
				<asp:ListItem Text="3" Value="3"></asp:ListItem>
				<asp:ListItem Text="2" Value="2"></asp:ListItem>
				<asp:ListItem Text="1" Value="1"></asp:ListItem>
				<asp:ListItem Text="0" Value="0"></asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
</table>
