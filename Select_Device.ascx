<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Select_Device.ascx.cs" Inherits="CUS_iPad.Select_Device" %>
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


<asp:Panel ID="pnlSelDevice" runat="server" Visible="true">
    <h1>Step 1 - Select a Device</h1>
    <p>Springfield College students can purchase select Apple iPad devices, configured for their needs, and have them available for pickup at the start of the fall session.  Charges for these devices will appear on the student's college bill according to the payment plan selected. </p>
    <p><em>Note! All iPad devices purchased through this program will include a <strong>Retina Display</strong>, and <strong>2-year AppleCare+</strong> coverage.</em></p>
    <asp:PlaceHolder ID="plhChoices" runat="server" />
</asp:Panel>

