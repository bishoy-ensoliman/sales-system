using GarasERP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarasSales.Sales.Reports
{
    public partial class VarifyReport : System.Web.UI.Page
    {
        DataTable ReportDT = new System.Data.DataTable();
        static string key = "SalesGarasPass";
        private long UserID
        {
            get
            {
                if (Session["UserID"] == null)
                {
                    return 0;
                }
                else
                {
                    string id = Session["UserID"].ToString();

                    return long.Parse(GarasERP.Encrypt_Decrypt.Decrypt(id, key));
                }
            }
            set
            {
                Session["UserID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindReviewDDL();
                    List<string> groups = new List<string>();
                    groups.Add("SalesManagers");
                    if (Common.CheckUserInGroups(UserID, groups))
                    {
                        
                        if (Page.Request.QueryString.Count > 0 && Page.Request.QueryString["RID"] != null)
                        {
                            string ID = GarasERP.Encrypt_Decrypt.Decrypt(Page.Request.QueryString["RID"].ToString(), key);
                            long RID = 0;
                            if (long.TryParse(ID, out RID) && RID > 0)
                            {
                                V_DailyReport report = new V_DailyReport();
                              
                                
                                report.Where.BranchID.Value = Common.GetUserBranchID(UserID);
                                report.Where.ID.Value = RID;
                                report.Where.Status.Value = "Pending Verification";
                               
                                if (report.Query.Load())
                                {
                                    if (report.DefaultView != null && report.DefaultView.Count > 0)
                                    {
                                        DIV_Content.Visible = true;
                                        LBL_MSG.Visible = false;

                                        LBL_TitleDate.Text = " " + report.ReprotDate.ToString("dd/MM/yyyy");

                                        LoadReportLines(RID);


                                    }
                                    else
                                    {
                                        LBL_MSG.Visible = true;
                                        LBL_MSG.Text = "Please Select Correct Report For review";
                                    }
                                }
                                else
                                {


                                    LBL_MSG.Visible = true;
                                    LBL_MSG.Text = "the report was already reviewed before.";
                                }
                            }
                            else
                            {
                                LBL_MSG.Visible = true;
                                LBL_MSG.Text = "Please Select Correct Report For review";
                            }
                        }
                        else
                        {

                            LBL_MSG.Visible = true;
                            LBL_MSG.Text = "Please Select Correct Report For review";
                        }
                    }
                    else
                    {

                        LBL_MSG.Visible = true;
                        LBL_MSG.Text = "Please Select Correct Report For review";
                    }
                }
                else
                {
                    if (ViewState["ReportDT"] != null)
                        ReportDT = (DataTable)ViewState["ReportDT"];

                }
            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "Page_Load: " + ex.Message;
            }

        }

        private System.Data.DataTable InitializeReportGridViewDataTable()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("ClientID");
            dt.Columns.Add("ClientName");
            dt.Columns.Add("ThroughID");
            dt.Columns.Add("ThroughName");
            dt.Columns.Add("From");
            dt.Columns.Add("To");
            dt.Columns.Add("Location");
            dt.Columns.Add("Reason");
            dt.Columns.Add("New", typeof(bool));
            dt.Columns.Add("NewClientAddress");
            dt.Columns.Add("NewClientTel");
            dt.Columns.Add("NewText");
            dt.Columns.Add("FileName");
            dt.Columns.Add("FileNameWZTime");
            dt.Columns.Add("FileExtension");
            dt.Columns.Add("FilePath");
            dt.Columns.Add("FileContents", typeof(byte[]));
            dt.Columns.Add("ContactPerson");
            dt.Columns.Add("ContactPersonMobile");
            dt.Columns.Add("NewClass");

            return dt;
        }

        private void LoadReportLines(long rID)
        {
            try
            {
                //bool ViewAll = Common.CheckUserInRole(UserID, 4); // view all daily reports
                ReportDT = InitializeReportGridViewDataTable();
                V_DailyReportLine_Through report = new V_DailyReportLine_Through();
                report.Where.DailyReportID.Value = rID;
                if (report.Query.Load())
                {
                    do
                    {
                        DataRow dr = ReportDT.NewRow();
                        dr["ID"] = report.ID;
                        dr["ClientID"] = report.s_ClientID;
                        dr["ClientName"] = report.ClientName;
                        dr["ThroughID"] = report.DailyReportThroughID;
                        dr["ThroughName"] = report.Name;
                        dr["From"] = report.s_FromTIme;
                        dr["To"] = report.s_ToTime;
                        dr["Location"] = report.Location;
                        dr["Reason"] = report.Reason;

                        dr["New"] = report.New;
                        dr["NewClientAddress"] = report.NewClientAddress;
                        dr["NewClientTel"] = report.NewClientTel;
                        dr["ContactPerson"] = report.ContactPerson;
                        dr["ContactPersonMobile"] = report.ContactPersonMobile;
                        if (report.New)
                        {
                            dr["NewText"] = "New";
                            dr["ClientName"] = report.NewClientName;
                            dr["NewClass"] = "badge badge-primary";
                            DailyReportAttachment attach = new DailyReportAttachment();
                            attach.Where.ReportLineID.Value = report.ID;
                            if (attach.Query.Load())
                            {
                                if (attach.DefaultView != null && attach.DefaultView.Count > 0)
                                {
                                    dr["FileName"] = attach.FileName;
                                    dr["FileNameWZTime"] = attach.FileName;
                                    dr["FileExtension"] = attach.FileExtenssion;
                                    dr["FilePath"] = attach.AttachmentPath;
                                    dr["FileContents"] = null;

                                }
                                else
                                {
                                    dr["FileName"] = "";
                                    dr["FileNameWZTime"] = "";
                                    dr["FileExtension"] = "";
                                    dr["FilePath"] = "";
                                    dr["FileContents"] = null;
                                }
                            }
                            else
                            {
                                dr["FileName"] = "";
                                dr["FileNameWZTime"] = "";
                                dr["FileExtension"] = "";
                                dr["FilePath"] = "";
                                dr["FileContents"] = null;
                            }
                        }
                        else
                        {
                            dr["NewText"] = "Old";
                            dr["NewClass"] = "badge badge-secondary";
                        }

                        ReportDT.Rows.Add(dr);
                    }
                    while (report.MoveNext());
                }
               
                    RPT_Line.DataSource = ReportDT;
                    RPT_Line.DataBind();


               
                ViewState["ReportDT"] = ReportDT;
            }
            catch (Exception ex)
            {
                LBL_MSG.Text = "LoadReportLines : " + ex.Message;
                LBL_MSG.Visible = true;
            }
        }

        protected void GV_Reports_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{




                //        if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == (DataControlRowState.Alternate | DataControlRowState.Normal))
                //        {
                //            Label LBL_Amount = (Label)e.Row.FindControl("LBL_Amount");


                //            while (LBL_Amount.Text.Contains(".") && LBL_Amount.Text.Trim().EndsWith("0"))
                //                LBL_Amount.Text = LBL_Amount.Text.Substring(0, LBL_Amount.Text.Length - 1);
                //            if (LBL_Amount.Text.Trim().EndsWith("."))
                //                LBL_Amount.Text = LBL_Amount.Text.Substring(0, LBL_Amount.Text.Length - 1);



                //        }


                //}
                //else
                //{
                //if (e.Row.RowType == DataControlRowType.Footer)
                //{
                //    if (e.Row.RowState == DataControlRowState.Normal)
                //    {
                //        DropDownList DDL_New = (DropDownList)e.Row.FindControl("DDL_New");
                //        DropDownList DDL_ClientName = (DropDownList)e.Row.FindControl("DDL_ClientName");

                //        RequiredFieldValidator RFV_ClientName = (RequiredFieldValidator)e.Row.FindControl("RFV_ClientName");


                //        Label LBL_NewClientName = (Label)e.Row.FindControl("LBL_NewClientName");
                //        TextBox TXT_NewClientName = (TextBox)e.Row.FindControl("TXT_NewClientName");
                //        RequiredFieldValidator RFV_NewClientName = (RequiredFieldValidator)e.Row.FindControl("RFV_NewClientName");
                //        Label LBL_NewTel = (Label)e.Row.FindControl("LBL_NewTel");
                //        TextBox TXT_Tel = (TextBox)e.Row.FindControl("TXT_Tel");
                //        Label LBL_NewAddress = (Label)e.Row.FindControl("LBL_NewAddress");
                //        TextBox TXT_NewAddress = (TextBox)e.Row.FindControl("TXT_NewAddress");
                //        DropDownList DDL_Through = (DropDownList)e.Row.FindControl("DDL_Through");

                //        if (DDL_New.SelectedValue == "0")
                //        {
                //            DDL_ClientName.Visible = true;
                //            RFV_ClientName.Enabled = true;
                //            LBL_NewClientName.Visible = false;
                //            TXT_NewClientName.Visible = false;
                //            RFV_NewClientName.Enabled = false;
                //            LBL_NewTel.Visible = false;
                //            TXT_Tel.Visible = false;
                //            LBL_NewAddress.Visible = false;
                //            TXT_NewAddress.Visible = false;

                //            DDL_ClientName.Items.Clear();
                //            Client client = new Client();
                //            client.Where.SalesPersonID.Value = UserID;

                //            if (client.Query.Load())
                //            {
                //                DDL_ClientName.DataTextField = Client.ColumnNames.Name;
                //                DDL_ClientName.DataValueField = Client.ColumnNames.ID;
                //                DDL_ClientName.DataSource = client.DefaultView;
                //                DDL_ClientName.DataBind();
                //            }

                //        }
                //        else
                //        {
                //            DDL_ClientName.Visible = false;
                //            RFV_ClientName.Enabled = false;
                //            LBL_NewClientName.Visible = true;
                //            TXT_NewClientName.Visible = true;
                //            RFV_NewClientName.Enabled = true;
                //            LBL_NewTel.Visible = true;
                //            TXT_Tel.Visible = true;
                //            LBL_NewAddress.Visible = true;
                //            TXT_NewAddress.Visible = true;
                //        }


                //        //load employee
                //        DDL_Through.Items.Clear();
                //        DailyReportThrough through = new DailyReportThrough();

                //        if (through.Query.Load())
                //        {
                //            DDL_Through.DataTextField = DailyReportThrough.ColumnNames.Name;
                //            DDL_Through.DataValueField = DailyReportThrough.ColumnNames.ID;
                //            DDL_Through.DataSource = through.DefaultView;
                //            DDL_Through.DataBind();
                //        }


                //    }
                //}


                //   }
            }
            catch (Exception ex)
            {
                //LBL_InnerMessage.Text = "GV_FinMasrof_RowDataBound:" + ex.Message;
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "GV_FinMasrof_RowDataBound:" + ex.Message;
                // Logger.LogException(ex);
            }
        }



        protected void BindReviewDDL()
        {
            DDl_Review.Items.Clear();
            for (int i = 25; i <= 100; i = i + 25)
                DDl_Review.Items.Add(i.ToString());
           
        }

        protected void BTN_Finish_Click(object sender, EventArgs e)
        {
            try
            {
                
                    if (Page.Request.QueryString.Count > 0 && Page.Request.QueryString["RID"] != null)
                    {
                        string ID = GarasERP.Encrypt_Decrypt.Decrypt(Page.Request.QueryString["RID"].ToString(), key);
                        long RID = 0;
                        bool hasNew = false;
                        if (long.TryParse(ID, out RID) && RID > 0)
                        {
                            DailyReport report = new DailyReport();
                       
                        report.Where.ID.Value = RID;
                        report.Where.Status.Value = "Pending Verification";
                        if (report.Query.Load())
                        {
                            if (report.DefaultView != null && report.DefaultView.Count > 0)
                            {
                                
                                report.Status = "Completed";
                                report.ModifiedDate = DateTime.Now;
                                report.ModifiedBy = UserID;
                               // report.Note = 
                                report.Review = double.Parse( DDl_Review.SelectedValue);
                                report.ReviewComment= TXT_Desc.Text.Trim();
                                report.ReviewDate = DateTime.Now;
                                report.Reviewed = true;
                                report.ReviewedBy = UserID;
                                
                                report.Save();

                                string UserName = "";
                                long BranchID = 0;
                                User user = new GarasERP.User();
                                if (user.LoadByPrimaryKey(UserID))
                                {
                                    UserName = user.FirstName + " " + user.LastName;
                                    if (user.s_BranchID != "")
                                        BranchID = user.BranchID;
                                }
                                
                            

                                Page.Response.Redirect("Reports.aspx", false);




                            }
                            else
                            {
                                LBL_MSG.Visible = true;
                                LBL_MSG.Text = "Please Select Correct Report For edit";
                            }
                        }
                        else
                        {
                            LBL_MSG.Visible = true;
                            LBL_MSG.Text = "Please Select Correct Report For edit";
                        }
                    }
                    else
                    {
                        LBL_MSG.Visible = true;
                        LBL_MSG.Text = "Please Select Correct Report For edit";
                    }
                }
                else
                {

                    LBL_MSG.Visible = true;
                    LBL_MSG.Text = "Please Select Correct Report For edit";
                }


                    ///////////////////
                
            }
            catch (Exception ex)
            {

                LBL_MSG.Visible = true;
                LBL_MSG.Text = "btn_Save:" + ex.Message;
                // Logger.LogException(ex);
            }
        }
    }
}