<%@ Page Language="C#" MasterPageFile="~/Templates/WebModules/Default.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BayshoreSolutions.WebModules.Cms.FileManager._default" Title="File Manager" Theme="WebModules" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    
    <div style="text-align: right; font-size: 85%; margin-bottom: 5px;">
        Maximum file upload size is 
        <asp:Literal ID="MaxRequestLengthCtl" runat="server" /> 
        MB.
    </div>
    
    <div id="editorDiv" runat="server"></div>
     <script type="text/javascript">
    $(function () {
        ToggleMenus('system', undefined, undefined);
    });
    </script>
</asp:Content>
