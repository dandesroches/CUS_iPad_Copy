<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Place_Order.ascx.cs" Inherits="CUS_iPad.Place_Order" %>
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


<script language="javascript" type="text/javascript">
    function checkAgreeBox() {
        if (document.getElementById('chkAgree').checked == true) {
            document.getElementById('<%=btnPlace.ClientID%>').disabled = false;
        } else {
            document.getElementById('<%=btnPlace.ClientID%>').disabled = true;
        }
        return;
    }
</script>


<asp:Panel ID="pnlSelDevice" runat="server" Visible="true">
    <h1>Step 5 - Place Your Order</h1>
    <p>Please review your order for accuracy before proceeding.  To place your order, select a payment plan below and click the <strong>Place My Order Now</strong> button.  If any changes are necessary, click the <strong>Erase & Start Over</strong> button</p>
</asp:Panel>


<asp:Panel ID="pnlOrder" runat="server" Visible="true" CssClass="pnlOrder">
    <p class="selHeader">Your Selected Device:</p>
    <p class="currSel">
        <asp:Image ID="imgDevice" runat="server" style="float:left; margin:6px 20px 0px 40px;" />
        <strong><asp:Label ID="lblDevice" runat="server" /></strong><br />
        Finish: <asp:Label ID="lblFinish" runat="server" /><br />
        Model: <asp:Label ID="lblModel" runat="server" /><br />
        Connect: <asp:Label ID="lblConnect" runat="server" /><br />
        Carrier: <asp:Label ID="lblCarrier" runat="server" /><br />
        Price: <strong>$<asp:Label ID="lblPrice" runat="server" /></strong>
    </p>
    <p class="selNote">Includes Retina Display and 2-year AppleCare+ coverage.</p>
    <p>&nbsp;</p>
    <hr />
    <p class="selHeader">Select a Payment Option:</p>
    <asp:RadioButtonList ID="radioPayOption" runat="server" style="margin-left:30px;">
        <asp:ListItem Value="1" Text="Single Payment" />
        <asp:ListItem Value="2" Text="2 Payments" Selected="True" />
        <asp:ListItem Value="4" Text="4 Payments" />
    </asp:RadioButtonList>
    <p class="selNote">NOTE! The amount shown above will be added to your college bill, and is payable in equal<br />installments, according to the option selected. Payment(s) are due at the start of each semester.<br />Any remaining balance becomes immediately due if you are no longer enrolled at Springfield College.</p>
    <p style="text-align:center; margin-top:20px;"><input type="checkbox" id="chkAgree" onchange="checkAgreeBox();" /> I understand that clicking the 'Place My Order Now' button<br />will add the amount shown above to my college bill.</p>
    <center>
        <asp:Button ID="btnPlace" runat="server" OnClick="btnOrder_Click" Text="Place My Order Now" Enabled="false" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnErase" runat="server" OnClick="btnErase_Click" Text="Erase & Start Over" />
    </center>
</asp:Panel>

