<%@ Page Title="SlideShow Module" Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/WebModules/Default.master"
    CodeBehind="Edit.aspx.cs" Inherits="BayshoreSolutions.WebModules.SlideShow.Edit"
    Theme="WebModules" ValidateRequest="false" %>

<%@ Register Src="Controls/SlideShowImages_manage.ascx" TagPrefix="uc1" TagName="SlideShowImages_manage" %>
<%@ Register Src="~/WebModules/Components/ImagePicker/ImagePicker.ascx" TagPrefix="uc1"
    TagName="ImagePicker" %>
<%@ Register Src="~/WebModules/Components/TextEditor/TextEditor/TextEditorControl.ascx"
    TagName="TextEditor" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" runat="server">
    <div class="m-2">
    <div class="pull-right">
        <a id="MainMenuLink" runat="server" class="btn btn-info">Return to Main Menu</a>
    </div>
    <h3>
        <em>
            <asp:Literal ID="ModuleName" runat="server" /></em>
        <asp:Literal ID="ModulTypeName" runat="server" />
        Module</h3>
    <bss:MessageBox ID="Msg" runat="server" />
    <div class="entity_edit">
        <table style="width: 100%;">
            <tr>
                <td>
                    <div id="FlashFile_div" runat="server" class="field">
                        <asp:Label Text="Flash (.swf) file" ID="Label1" runat="server" AssociatedControlID="WidthCtl" />
                        <div class="help">
                            Special flash supplied by the developer.</div>
                        <div>
                            <uc1:ImagePicker ID="FlashFileCtl" runat="server" Width="145px" />
                        </div>
                    </div>
                    <div class="field">
                        <asp:Label Text="Width" ID="WidthLabel" runat="server" AssociatedControlID="WidthCtl" />
                        <div id="WidthHelp" runat="server" class="help">
                            Must match the flash width exactly.</div>
                        <div>
                            <asp:TextBox ID="WidthCtl" runat="server" />pixels
                            <asp:RequiredFieldValidator ID="WidthRequiredFieldValidator" runat="server" Text="Required"
                                ErrorMessage="Required" ControlToValidate="WidthCtl" Display="Dynamic" />
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="WidthCtl"
                                Operator="DataTypeCheck" Type="Integer" Text="Invalid" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="field">
                        <asp:Label Text="Height" ID="HeightLabel" runat="server" AssociatedControlID="HeightCtl" />
                        <div id="HeightHelp" runat="server" class="help">
                            Must match the flash height exactly.</div>
                        <div>
                            <asp:TextBox ID="HeightCtl" runat="server" />pixels
                            <asp:RequiredFieldValidator ID="HeightRequiredFieldValidator" runat="server" Text="Required"
                                ErrorMessage="Required" ControlToValidate="HeightCtl" Display="Dynamic" />
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="HeightCtl"
                                Operator="DataTypeCheck" Type="Integer" Text="Invalid" Display="Dynamic" />
                        </div>
                    </div>
                    <div id="DisplayOrder_div" runat="server" class="field">
                        <asp:Label Text="Display Order" ID="ImageDisplayOrderLabel" runat="server" AssociatedControlID="ImageDisplayOrderCtl" />
                        <%--<div class="help">Help text</div>--%>
                        <div>
                            <asp:DropDownList ID="ImageDisplayOrderCtl" runat="server">
                                <asp:ListItem Text="Sequential" Value="0" />
                                <asp:ListItem Text="Random" Value="1" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div id="DisplayTime_div" runat="server" class="field">
                        <asp:Label Text="Display Time" ID="ImageDisplayTimeLabel" runat="server" AssociatedControlID="ImageDisplayTimeCtl" />
                        <%--<div class="help">Help text</div>--%>
                        <div>
                            <asp:TextBox ID="ImageDisplayTimeCtl" runat="server" />seconds
                            <asp:RequiredFieldValidator ID="ImageDisplayTimeRequiredFieldValidator" runat="server"
                                Text="Required" ErrorMessage="Required" ControlToValidate="ImageDisplayTimeCtl"
                                Display="Dynamic" />
                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="ImageDisplayTimeCtl"
                                Operator="DataTypeCheck" Type="Integer" Text="Invalid" Display="Dynamic" />
                        </div>
                    </div>
                    <div id="FadeTime_div" runat="server" class="field">
                        <asp:Label Text="Fade Time" ID="ImageFadeTimeLabel" runat="server" AssociatedControlID="ImageFadeTimeCtl" />
                        <%--<div class="help">Help text</div>--%>
                        <div>
                            <asp:TextBox ID="ImageFadeTimeCtl" runat="server" />seconds
                            <asp:RequiredFieldValidator ID="ImageFadeTimeRequiredFieldValidator" runat="server"
                                Text="Required" ErrorMessage="Required" ControlToValidate="ImageFadeTimeCtl"
                                Display="Dynamic" />
                            <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="ImageFadeTimeCtl"
                                Operator="DataTypeCheck" Type="Integer" Text="Invalid" Display="Dynamic" />
                        </div>
                    </div>
                    <div id="NavType_div" runat="server" class="field">
                        <asp:Label Text="Nav Type" ID="ImageNavTypeLabel" runat="server" AssociatedControlID="ImageNavTypeCtl" />
                        <%--<div class="help">Help text</div>--%>
                        <div>
                            <asp:DropDownList ID="ImageNavTypeCtl" runat="server">
                                <asp:ListItem Value="0">None</asp:ListItem>
                                <asp:ListItem Value="1">Per Slide</asp:ListItem>
                                <asp:ListItem Value="2">Prev / Next Buttons</asp:ListItem>
                                <asp:ListItem Value="3">Per Slide &amp; Prev / Next Buttons</asp:ListItem>
                            </asp:DropDownList>
                            <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="ImageNavTypeCtl"
                                Operator="DataTypeCheck" Type="Integer" Text="Invalid" Display="Dynamic" />
                        </div>
                    </div>
                    <div id="WrapType_div" runat="server" class="field">
                        <asp:Label Text="Wrap Type" ID="ImageWrapTypeLabel" runat="server" AssociatedControlID="ImageWrapTypeCtl" />
                        <%--<div class="help">Help text</div>--%>
                        <div>
                            <asp:DropDownList ID="ImageWrapTypeCtl" runat="server">
                                <asp:ListItem Value="0">None</asp:ListItem>
                                <asp:ListItem Value="1">First</asp:ListItem>
                                <asp:ListItem Value="2">Last</asp:ListItem>
                                <asp:ListItem Value="3">Both</asp:ListItem>
                                <asp:ListItem Value="4">Circular</asp:ListItem>
                            </asp:DropDownList>
                            <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="ImageWrapTypeCtl"
                                Operator="DataTypeCheck" Type="Integer" Text="Invalid" Display="Dynamic" />
                        </div>
                    </div>
                    <div id="ClassName_div" runat="server" class="field">
                        <asp:Label runat="server" ID="ClassNameLabel" Text="CSS Class Name" ToolTip="Used in Jquery to identify the type of flex slideshow"></asp:Label><br />
                        <asp:TextBox runat="server" ID="ClassNameTextBox" />
                    </div>
                    <%-- x-Position, y-Position fields are hidden because I don't think they are useful to the end-user. --%>
                    <%--
    <div class="field">
        <asp:Label Text="x-Position" ID="ImageXPositionLabel" runat="server" AssociatedControlID="ImageXPositionCtl" />
        <div>
            <asp:TextBox ID="ImageXPositionCtl" runat="server"  />pixels
            <asp:RequiredFieldValidator ID="ImageXPositionRequiredFieldValidator" runat="server" Text="Required" ErrorMessage="Required" ControlToValidate="ImageXPositionCtl" Display="Dynamic" />
            <asp:CompareValidator ID="CompareValidator5" runat="server" 
                ControlToValidate="ImageXPositionCtl"
                Operator="DataTypeCheck"
                Type="Integer"
                Text="Invalid"
                Display="Dynamic"
            />
        </div>
    </div>
    <div class="field">
        <asp:Label Text="y-Position" ID="ImageYPositionLabel" runat="server" AssociatedControlID="ImageYPositionCtl" />
        <div>
            <asp:TextBox ID="ImageYPositionCtl" runat="server"  />pixels
            <asp:RequiredFieldValidator ID="ImageYPositionRequiredFieldValidator" runat="server" Text="Required" ErrorMessage="Required" ControlToValidate="ImageYPositionCtl" Display="Dynamic" />
            <asp:CompareValidator ID="CompareValidator6" runat="server" 
                ControlToValidate="ImageYPositionCtl"
                Operator="DataTypeCheck"
                Type="Integer"
                Text="Invalid"
                Display="Dynamic"
            />
        </div>
    </div>
                    --%>
                    <div class="field">
                        <div>
                            <asp:CheckBox ID="ImageLoopingCtl" runat="server" Text="Loop slide show" />
                        </div>
                    </div>
                    <div class="toolbar">
                        <asp:Button ID="SlideShow_Module_SaveButton" runat="server" CssClass="saveButton btn btn-info"
                            CausesValidation="true" Text="Save" ValidationGroup="" OnClick="SlideShow_Module_SaveButton_Click" />
                        <asp:Button ID="SlideShow_Module_CancelButton" runat="server" CssClass="cancelButton btn btn-danger"
                            CausesValidation="false" Text="Cancel" OnClick="SlideShow_Module_CancelButton_Click" />
                    </div>
                </td>
                <td style="vertical-align: top; border-left: 1px solid gray; padding: 0 0 0 20px;">
                    <asp:PlaceHolder ID="phImageManagement" runat="server">
                        <h4>
                            Images</h4>
                        <p class="help">
                            Images must be <strong>
                                <asp:Literal ID="AllowedImageTypes" runat="server" /></strong> format.
                        </p>
                        <asp:FileUpload ID="Image_FileUploadCtl" runat="server" />
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phContentManagement" runat="server">
                        <h4><%= IsGalleryViewSlideShow ? "Image Caption" : "Content" %></h4>
                        <p class="help">
                            <%= IsGalleryViewSlideShow ? "Image Caption" : "Content" %> must be no longer than 230 characters and should fit within the
                            sideshow panel or it will not show.
                        </p>
                        *<%= IsGalleryViewSlideShow ? "Picture Title" : "Content Name"%>:
                        <asp:TextBox ID="Content_Name" runat="server" ValidationGroup="SlideContent" />
                        <asp:RequiredFieldValidator ID="rfvContent_Name" runat="server" ControlToValidate="Content_Name"
                            ErrorMessage="*Required" ValidationGroup="SlideContent">*Required</asp:RequiredFieldValidator>
                        <uc1:TextEditor ID="SlideContent" runat="server" />
                    </asp:PlaceHolder>
                    <asp:Button ID="Image_SaveButton" runat="server" Text="Add Image" OnClick="Image_SaveButton_Click"
                        ValidationGroup="SlideContent" />
                    <br />
                    <br />
                    <uc1:SlideShowImages_manage ID="SlideShowImagesCtl" runat="server" />
                </td>
            </tr>
        </table>
    </div>
        </div>
     <script type="text/javascript">
        $(function () {
            ToggleMenus('websites', undefined, undefined);
        });
    </script>
</asp:Content>
