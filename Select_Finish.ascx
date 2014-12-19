<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Select_Finish.ascx.cs" Inherits="CUS_iPad.Select_Finish" %>
<link href="../ICS/clientconfig/css/scStyles.css" rel="stylesheet" type="text/css" />
<link href="../ICS/Portlets/CUS/ICS/CUS_iPad/CUS_iPad_Styles.css" rel="stylesheet" type="text/css" />

 <asp:Panel ID="pnlErrorMsg" runat="server" cssClass="errorMsg" Visible="false">
    <asp:Label ID="lblErrMsg" runat="server" />
    &nbsp;&nbsp;&nbsp;&nbsp;
</asp:Panel>


 <asp:Panel ID="pnlConfirmMsg" runat="server" cssClass="confirmMsg" Visible="false">
    <asp:Label ID="lblConfirmMsg" runat="server" Text="Record added successfully." />
    &nbsp;&nbsp;&nbsp;&nbsp;
</asp:Panel>


<asp:Panel ID="pnlSelections" runat="server" Visible="true" CssClass="pnlSelections">
    <p class="selHeader">Your Current Selections:</p>
    <p class="currSel">
        <asp:Image ID="imgDevice" runat="server" style="height:80px; float:left; margin:6px 20px 10px 0px;" />
        <strong>Device: <asp:Label ID="lblDevice" runat="server" /></strong><br />
        Finish: <asp:Label ID="lblFinish" runat="server" /><br />
        Model: <asp:Label ID="lblModel" runat="server" /><br />
        Connect: <asp:Label ID="lblConnect" runat="server" /><br />
        Carrier: <asp:Label ID="lblCarrier" runat="server" /><br />
        Price: <strong><asp:Label ID="lblPrice" runat="server" /></strong><br />
    </p>
    <p class="selNote">* Includes Retina Display and<br />2-year AppleCare+ coverage.</p>
    <center><asp:Button ID="btnErase" runat="server" OnClick="btnErase_Click" Text="Erase & Start Over" /></center>
</asp:Panel>

<asp:Panel ID="pnlSelFinish" runat="server" Visible="true">
    <h1>Step 2 - Select a Finish</h1>
    <p>The iPad you select is available in the following finishes.</p>
    <asp:PlaceHolder ID="plhChoices" runat="server" />
</asp:Panel>

