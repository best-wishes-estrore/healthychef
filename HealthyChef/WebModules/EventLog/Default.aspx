<%@ Page Language="C#" Title="System Event Log" MasterPageFile="~/Templates/WebModules/default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BayshoreSolutions.WebModules.Cms.EventLog._default" Theme="WebModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="main-content">
        <div class="main-content-inner">
            <div class="page-header">
                <h1>System Event Log</h1>
            </div>
            <div class="col-sm-12">
                <p id="uxDetailsMenu" runat="server">
                    <a href='?'>Browse</a>
                </p>
                <p id="uxListHelp" runat="server">
                    The most recent system events are listed below.
                </p>
            </div>
            <div class="col-sm-12">
                <asp:GridView ID="uxEventLogList" CssClass="table table-bordered table-hover" runat="server" DataKeyNames="EventId" SkinID="DetailSkin"
                    AutoGenerateColumns="false" OnRowDeleting="uxEventLogList_RowDeleting"
                    AllowPaging="true" PageSize="50" OnPageIndexChanging="uxEventLogList_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Event/Exception">
                            <ItemTemplate>
                                <a href='<%# Request.CurrentExecutionFilePath + "?EventId=" + Eval("EventId") %>'><%# Eval("Message") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="EventTime" HeaderText="Date" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="Delete" OnClientClick="javascript: return confirm('Delete the item?');"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>No events have been logged yet.</EmptyDataTemplate>
                    <PagerSettings Mode="NumericFirstLast" Position="Top" PageButtonCount="6" />
                </asp:GridView>
            </div>
            <div class="clearfix"></div>
            <div class="col-sm-12">
                <div class="table-responsive">
                    <asp:DetailsView ID="uxDetailsView" CssClass="table table-bordered table-hover" runat="server" AutoGenerateRows="False">
                        <Fields>
                            <asp:BoundField DataField="ExceptionType" HeaderText="ExceptionType" HeaderStyle-Font-Bold="true" />
                            <asp:BoundField DataField="EventTime" HeaderText="EventTime" HeaderStyle-Font-Bold="true" />
                            <asp:HyperLinkField HeaderText="RequestUrl" DataTextField="RequestUrl" DataNavigateUrlFields="RequestUrl" HeaderStyle-Font-Bold="true" />
                            <asp:BoundField HeaderText="EventType" DataField="EventType" HeaderStyle-Font-Bold="true" />
                            <asp:BoundField HeaderText="EventSequence" DataField="EventSequence" HeaderStyle-Font-Bold="true" />
                            <asp:BoundField HeaderText="EventOccurrence" DataField="EventOccurrence" HeaderStyle-Font-Bold="true" />
                            <asp:BoundField HeaderText="EventCode" DataField="EventCode" HeaderStyle-Font-Bold="true" />
                            <asp:BoundField HeaderText="EventDetailCode" DataField="EventDetailCode" HeaderStyle-Font-Bold="true" />
                            <asp:BoundField HeaderText="AppPath" DataField="ApplicationPath" HeaderStyle-Font-Bold="true" />
                            <asp:BoundField HeaderText="AppVirtualPath" DataField="ApplicationVirtualPath" HeaderStyle-Font-Bold="true" />
                            <asp:BoundField HeaderText="MachineName" DataField="MachineName" HeaderStyle-Font-Bold="true" />
                            <asp:TemplateField HeaderText="Message" HeaderStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <pre><%# Eval("Message") %></pre>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Details" HeaderStyle-Font-Bold="true">
                                <ItemTemplate>
                                    <pre><%# Eval("Details") %></pre>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
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
