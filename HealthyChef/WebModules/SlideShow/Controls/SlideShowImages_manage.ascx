<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SlideShowImages_manage.ascx.cs"
    Inherits="BayshoreSolutions.WebModules.Cms.SlideShow.Controls.SlideShowImages_manage" %>
<%@ Register Src="~/WebModules/Components/PagePicker/PagePicker.ascx" TagName="PagePicker"
    TagPrefix="uc1" %>
<bss:MessageBox ID="Msg" runat="server" />
<asp:Repeater ID="ImagesList" runat="server" OnItemCommand="ImagesList_ItemCommand">
    <HeaderTemplate>
        <table width="100%">
    </HeaderTemplate>
    <ItemTemplate>
        <asp:PlaceHolder ID="phTextContent" runat="server" Visible='<%# this.IsJQuerySlidingTextContentSlideShow || IsGalleryViewSlideShow %>'>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <%= IsGalleryViewSlideShow ? "Picture Title" : "Content Name"%>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtContentName" Text='<%# Eval("SlideTextContentName") %>' />
                </td>
            </tr>
        </asp:PlaceHolder>
        <tr>
            <asp:PlaceHolder ID="phImageContent" runat="server" Visible='<%# !this.IsJQuerySlidingTextContentSlideShow %>'>
                <td style="width: 110px">
                    <img src='<%# ResolveUrl("~/image.ashx?size=100&file=" + System.Uri.EscapeDataString(((BayshoreSolutions.WebModules.SlideShow.SlideShowImage)Container.DataItem).GetFullPath())) %>'
                        alt="" />
                </td>
            </asp:PlaceHolder>
            <td style="vertical-align: middle;">
                <asp:PlaceHolder ID="phImageName" runat="server" Visible='<%# !this.IsJQuerySlidingTextContentSlideShow %>'>
                    <%# BayshoreSolutions.Common.Web.Html.InsertSoftBreaks((string)Eval("ImageFileName"))%>
                    <br />
                </asp:PlaceHolder>
                <asp:ImageButton ID="MoveUpButton" runat="server" ImageUrl="~/WebModules/Admin/Images/Icons/Small/UpArrow.gif"
                    ToolTip="Move Up" CommandArgument='<%# Eval("Id") %>' CommandName="MoveUp" CssClass="icon" />
                <asp:ImageButton ID="MoveDownButton" runat="server" ImageUrl="~/WebModules/Admin/Images/Icons/Small/DownArrow.gif"
                    ToolTip="Move Down" CommandArgument='<%# Eval("Id") %>' CommandName="MoveDown"
                    CssClass="icon" />
                <asp:PlaceHolder ID="phTextContentEdit" runat="server" Visible='<%# this.IsJQuerySlidingTextContentSlideShow || IsGalleryViewSlideShow %>'>
                    <a href="javascript:showPopWin('<%# GetBaseUrl()%>','<%# GetBaseUrl() + "WebModules/SlideShow/PopupEditor.aspx?id=" + Eval("Id")%>', 800, 540, null, true);">
                        <img src="/WebModules/Admin/Images/Icons/Small/Edit.gif" class="icon" />
                        Edit Content</a> </asp:PlaceHolder>
                <asp:LinkButton ID="DeleteButton" runat="server" Text="Delete" CommandArgument='<%# Eval("Id") %>'
                    CommandName="Delete" CausesValidation="false" />
                <asp:LinkButton ID="LinkButton1" runat="server" Text="Save" CommandArgument='<%# Eval("Id") %>'
                    CommandName="Save" CausesValidation="false" />
            </td>
        </tr>
        <tr runat="server" visible='<%# !this.IsGalleryViewSlideShow %>'>
            <td>
                Link Url
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtLink" Text='<%# Eval("linkUrl") %>' />
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table></FooterTemplate>
</asp:Repeater>
