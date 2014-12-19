<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Review_Order.ascx.cs" Inherits="CUS_iPad.Review_Order" %>
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
    <h1>Your order has been completed!</h1>
    <p>We have received your order for the iPad device shown below.  If you have any questions regarding this order please contact <a href="mailto:somebody@SpringfieldCollege.edu">somebody@SpringfieldCollege.edu</a></p>
</asp:Panel>


<asp:Panel ID="pnlOrder" runat="server" Visible="true" CssClass="pnlOrder">
    <p class="selHeader">Your Selected Device:</p>
    <p class="currSel">
        <asp:Image ID="imgDevice" runat="server" style="float:left; margin:0px 20px 0px 40px;" />
        <strong><asp:Label ID="lblDevice" runat="server" /></strong><br />
        Finish: <asp:Label ID="lblFinish" runat="server" /><br />
        Model: <asp:Label ID="lblModel" runat="server" /><br />
        Connect: <asp:Label ID="lblConnect" runat="server" /><br />
        Carrier: <asp:Label ID="lblCarrier" runat="server" /><br />
        Price: <strong>$<asp:Label ID="lblPrice" runat="server" /></strong> (<asp:Label ID="lblPayments" runat="server" /> payments)<br />
    </p>
    <p class="selNote">Includes Retina Display and 2-year AppleCare+ coverage.</p>
</asp:Panel>

