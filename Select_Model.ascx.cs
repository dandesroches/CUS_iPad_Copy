using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jenzabar.Common.Web.UI.Controls;
using Jenzabar.Portal.Framework.Web.UI;
using Jenzabar.Common.Globalization;  // added for FWK_Globalizations table
using Jenzabar.ICS.Web.Portlets.Common; // added for setting (and preference) check
using System.Text;  // Needed for Stringbuilder
using Jenzabar.Portal.Framework;  // Needed for PortalUser.Current.Username
using System.Configuration; // For the ConfigurationManager
using System.Data; // general ADO.NET
using System.Data.Common; // general/common ADO.NET
using System.Data.Odbc; // ODBC Specific Implementation of the ADO.NET Standard/Specification

namespace CUS_iPad
{
    public partial class Select_Model : PortletViewBase
    {
        string connString = ConfigurationManager.ConnectionStrings["ERP"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            lblConfirmMsg.Text = "Year=" + this.ParentPortlet.PortletViewState["year"].ToString() + ", ";
            lblConfirmMsg.Text += "Sess=" + this.ParentPortlet.PortletViewState["sess"].ToString() + ", ";
            lblConfirmMsg.Text += "ID=" + PortalUser.Current.HostID.TrimStart('0') + ", ";
            lblConfirmMsg.Text += "OrderNum=" + this.ParentPortlet.PortletViewState["ordernum"].ToString() + ", ";
            pnlConfirmMsg.Visible = true;
            */

            showCurrentSelections();

            string YEAR = this.ParentPortlet.PortletViewState["year"].ToString();
            string SESS = this.ParentPortlet.PortletViewState["sess"].ToString();
            string DEVICE = this.ParentPortlet.PortletViewState["device"].ToString();
            string ID = PortalUser.Current.HostID.TrimStart('0');

            if ((YEAR == "") || (SESS == ""))
            {
                lblErrMsg.Text = "ERROR: Year/Session not found (ipadparam).  Please notify the TSC if the error persits.";
                pnlErrorMsg.Visible = true;
                pnlSelModel.Visible = false;
            }
            else
            {
                OdbcConnection conn = new OdbcConnection();
                conn.ConnectionString = connString;
                conn.Open();


                // Read the iPad Device records, and display to user
                OdbcCommand command = new OdbcCommand();
                command.Connection = conn;
                command.CommandText = "select * from sc_ipadmodel_table where device=? order by connect desc, seq, price";
                command.Parameters.Add(new OdbcParameter("device", DEVICE));
                try
                {
                    OdbcDataReader modRec = command.ExecuteReader();
                    if (modRec.HasRows)
                    {
                        Table modTable = new Table();
                        TableRow modRow = new TableRow();
                        TableCell modCell = new TableCell();

                        string prevConnect = "";
                        while (modRec.Read())
                        {
                            string thisConnect = modRec["connect"].ToString().Trim();
                            if (thisConnect != prevConnect)
                            {
                                modRow.Cells.Add(modCell);
                                modCell = new TableCell();
                                Label lblConn = new Label();
                                lblConn.Text = "<h1 class='connHead'>" + modRec["connect"].ToString().Trim() + "</h1>";
                                modCell.Controls.Add(lblConn);
                                prevConnect = thisConnect;
                            }
                            Panel pnlModel = new Panel();
                            pnlModel.CssClass = "pnlModel";

                            LinkButton lnkModel = new LinkButton();
                            lnkModel.Click += new EventHandler(btnSelect_Click);
                            lnkModel.CommandArgument = modRec["model"].ToString().Trim() + ", " + modRec["connect"].ToString().Trim();
                            lnkModel.Text = "<h1>" + modRec["Model"].ToString().Trim() + " - <span class='modPrice'>$" + modRec["Price"].ToString().Trim() + "</span></h1>";
                            pnlModel.Controls.Add(lnkModel);

                            modCell.Controls.Add(pnlModel);
                        }

                        modRow.Cells.Add(modCell);
                        modTable.Rows.Add(modRow);
                        plhChoices.Controls.Add(modTable);

                    }
                    else { }
                }
                catch //(Exception ex)
                {
                    lblErrMsg.Text = "ERROR: Data retrieval error (ipaddevice).  Please notify the TSC if the error persits.";
                    //lblErrMsg.Text += "<br>" + ex.ToString();
                    pnlErrorMsg.Visible = true;
                    conn.Close();
                    return;
                }
            }

        }


        private void showCurrentSelections()
        {
            string YEAR = this.ParentPortlet.PortletViewState["year"].ToString();
            string SESS = this.ParentPortlet.PortletViewState["sess"].ToString();
            string ID = PortalUser.Current.HostID.TrimStart('0');
            string ORDER = this.ParentPortlet.PortletViewState["ordernum"].ToString();
            string DEVICE = "";
            string FINISH = "";
            string MODEL = "";
            string CONNECT = "";
            string CARRIER = "";
            string IMAGE = "";
            int PAYMENTS = 0;
            decimal PRICE = 0;

            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = connString;
            conn.Open();

            OdbcCommand command = new OdbcCommand();
            command.Connection = conn;
            command.Parameters.Clear();
            command.CommandText = "select t1.*, t2.dev_image_url from sc_ipadorder_rec t1 join sc_ipaddevice_table t2 on t1.device=t2.device where sc_ipadorder_no=?";
            command.Parameters.Add(new OdbcParameter("sc_ipadorder_no", ORDER));
            try
            {
                OdbcDataReader orderRec = command.ExecuteReader();
                if (orderRec.HasRows)
                {
                    while (orderRec.Read())
                    {
                        DEVICE = orderRec["device"].ToString().Trim();
                        this.ParentPortlet.PortletViewState["device"] = DEVICE;
                        FINISH = orderRec["finish"].ToString().Trim();
                        this.ParentPortlet.PortletViewState["finish"] = FINISH;
                        MODEL = orderRec["model"].ToString().Trim();
                        CONNECT = orderRec["connect"].ToString().Trim();
                        CARRIER = orderRec["carrier"].ToString().Trim();
                        IMAGE = orderRec["dev_image_url"].ToString().Trim();
                        PRICE = Convert.ToDecimal(orderRec["price"]);
                        PAYMENTS = Convert.ToInt16(orderRec["pay_option"]);
                    }
                }
                else
                {
                    lblErrMsg.Text = "ERROR: Order record not found.  Notify the TSC if the error persits.";
                    pnlErrorMsg.Visible = true;
                    pnlConfirmMsg.Visible = false;
                    conn.Close();
                    return;
                }
            }
            catch //(Exception ex)
            {
                lblErrMsg.Text = "ERROR: Data retrieval error (ipadorder).  Notify the TSC if the error persits.";
                //lblErrMsg.Text += "<br>" + ex.ToString();
                pnlErrorMsg.Visible = true;
                pnlConfirmMsg.Visible = false;
                conn.Close();
                return;
            }

            imgDevice.ImageUrl = IMAGE;
            lblDevice.Text = DEVICE;
            if (FINISH != "") { lblFinish.Text = FINISH; }
            if (MODEL != "") { lblModel.Text = MODEL; }
            if (CONNECT != "") { lblConnect.Text = CONNECT; }
            if (CARRIER != "") { lblCarrier.Text = CARRIER; }
            if (PRICE != 0) { lblPrice.Text = PRICE.ToString(); }

        }

        protected void btnErase_Click(object sender, EventArgs e)
        {

            string ORDER = this.ParentPortlet.PortletViewState["ordernum"].ToString();
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = connString;
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter();
            conn.Open();
            OdbcCommand command = new OdbcCommand();
            command.Connection = conn;
            command.Parameters.Clear();
            command.CommandText = "delete sc_ipadorder_rec where sc_ipadorder_no=?";
            command.Parameters.Add(new OdbcParameter("sc_ipadorder_no", ORDER));
            try
            {
                command.ExecuteNonQuery();
            }
            catch //(Exception ex)
            {
                lblErrMsg.Text = "ERROR: Unable to delete selections.  Please notify the TSC if the error persists.";
                //lblErrMsg.Text += "<br />" + ex.Message.ToString() + "<br />" + ex.ToString();
                pnlErrorMsg.Visible = true;
            }
            this.ParentPortlet.NextScreen("Select_Device");
        }


        protected void btnSelect_Click(object sender, EventArgs e)
        {
            string ORDER = this.ParentPortlet.PortletViewState["ordernum"].ToString();
            string DEVICE = this.ParentPortlet.PortletViewState["device"].ToString();
            string ModelConnect = ((LinkButton)sender).CommandArgument;
            string MODEL = ModelConnect.Substring(0, ModelConnect.IndexOf(", "));
            string CONNECT = ModelConnect.Substring(ModelConnect.IndexOf(", ") + 2);
            this.ParentPortlet.PortletViewState["model"] = MODEL;
            this.ParentPortlet.PortletViewState["connect"] = CONNECT;
            decimal PRICE = 0;
            string NEEDCARRIER = "Y";

            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = connString;
            conn.Open();
            OdbcCommand command = new OdbcCommand();

            // Read the Model record to get the price and req_carrier values
            command = new OdbcCommand();
            command.Connection = conn;
            command.CommandText = "select req_carrier, price from sc_ipadmodel_table where device=? and model=? and connect=?";
            command.Parameters.Clear();
            command.Parameters.Add(new OdbcParameter("device", DEVICE));
            command.Parameters.Add(new OdbcParameter("model", MODEL));
            command.Parameters.Add(new OdbcParameter("connect", CONNECT));
            try
            {
                OdbcDataReader modelRec = command.ExecuteReader();
                if (modelRec.HasRows)
                {
                    while (modelRec.Read())
                    {
                        PRICE = Convert.ToDecimal(modelRec["price"].ToString().Trim());
                        NEEDCARRIER = modelRec["req_carrier"].ToString().Trim();
                    }
                }
                else
                {
                    lblErrMsg.Text = "ERROR: Model record not found.  Notify the TSC if the error persits.";
                    pnlErrorMsg.Visible = true;
                    pnlConfirmMsg.Visible = false;
                    conn.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "ERROR: Data retrieval error (ipadmodel).  Notify the TSC if the error persits.";
                lblErrMsg.Text += "<br>" + ex.ToString();
                pnlErrorMsg.Visible = true;
                pnlConfirmMsg.Visible = false;
                conn.Close();
                return;
            }





            // Add the Step 3 info
            command = new OdbcCommand();
            command.Connection = conn;
            command.Parameters.Clear();
            command.CommandText = "update sc_ipadorder_rec set model=?, connect=?, order_step='3', price=? where sc_ipadorder_no=?";
            command.Parameters.Add(new OdbcParameter("model", MODEL));
            command.Parameters.Add(new OdbcParameter("connect", CONNECT));
            command.Parameters.Add(new OdbcParameter("price", PRICE));
            command.Parameters.Add(new OdbcParameter("sc_ipadorder_no", ORDER));
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "ERROR: Unable to update the order record.  Notify the TSC if the error persits.";
                lblErrMsg.Text += "<br>" + ex.ToString();
                pnlErrorMsg.Visible = true;
                pnlConfirmMsg.Visible = false;
                conn.Close();
                return;
            }
           
            //determine where to go next
            if (NEEDCARRIER == "Y")
            {
                this.ParentPortlet.NextScreen("Select_Carrier");
            }
            else
            {
                this.ParentPortlet.PortletViewState["carrier"] = "none"; 
                this.ParentPortlet.NextScreen("Place_Order");
            }
        
        }





    }
}