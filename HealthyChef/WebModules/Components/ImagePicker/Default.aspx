<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="ImagePicker.ascx" TagName="ImagePicker"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:ImagePicker ID="ImagePicker1" runat="server" />
        <br />
        <uc1:ImagePicker ID="ImagePicker2" runat="server" />
    
    </div>
    </form>
</body>
</html>
