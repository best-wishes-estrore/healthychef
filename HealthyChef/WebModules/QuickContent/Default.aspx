<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BayshoreSolutions.WebModules.QuickContent.Default" Theme="WebModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>Quick Content Area Listing</h1>
            </div>
            <div class="clearfix"></div>
            <div class="row-fluid">
                <div class="col-sm-12">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table  table-bordered table-hover" AutoGenerateColumns="False" SkinID="DetailSkin">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Quick Content Areas
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <a href="javascript:showPopWin('<%# GetBaseUrl()%>','<%# GetBaseUrl() + "WebModules/QuickContent/PopupEditor.aspx?id=" + Eval("ContentName")%>', 800, 540, null, true);">
                                        <img src="/WebModules/Admin/Images/Icons/Small/Edit.gif" class="icon" />
                                        <%# Eval("ContentName")%></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
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
