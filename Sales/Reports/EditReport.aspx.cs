using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GarasERP;
using System.Data;
using System.Collections;
using System.IO;

namespace GarasSales.Sales.Reports
{
    public partial class NewDailyReport : System.Web.UI.Page
    {
        System.Data.DataTable ReportDT = new System.Data.DataTable();
        ArrayList DeletedReport = new ArrayList();
        static string key = "SalesGarasPass";
        static string SID = "0";
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
                SID = GarasERP.Encrypt_Decrypt.Decrypt(Session["UserID"].ToString(), key);
                if (!IsPostBack)
                {
                    ViewState["DeletedMasrof"] = DeletedReport;
                    if (Page.Request.QueryString.Count>0 && Page.Request.QueryString["RID"] !=null)
                    {
                        string ID = GarasERP.Encrypt_Decrypt.Decrypt(Page.Request.QueryString["RID"].ToString(), key);
                        long RID = 0;
                        if(long.TryParse(ID,out RID) && RID >0)
                        {
                            DailyReport report = new DailyReport();
                            report.Where.ID.Value = RID;
                            report.Where.UserID.Value = UserID;
                            report.Where.Status.Value = "Not Filled";
                            if(report.Query.Load())
                            {
                                if(report.DefaultView!=null && report.DefaultView.Count>0)
                                {
                                    DIV_Content.Visible = true;
                                    LBL_MSG.Visible = false;
                                    
                                    LBL_TitleDate.Text = " "+report.ReprotDate.ToString("dd/MM/yyyy");

                                    //DDL_ClientName.Items.Clear();
                                    //GarasERP.Client client = new GarasERP.Client();
                                    //client.Where.SalesPersonID.Value = UserID;

                                    //if (client.Query.Load())
                                    //{
                                    //    DDL_ClientName.DataTextField = GarasERP.Client.ColumnNames.Name;
                                    //    DDL_ClientName.DataValueField = GarasERP.Client.ColumnNames.ID;
                                    //    DDL_ClientName.DataSource = client.DefaultView;
                                    //    DDL_ClientName.DataBind();
                                    //}


                                    DDL_Through.Items.Clear();
                                    DailyReportThrough through = new DailyReportThrough();

                                    if (through.Query.Load())
                                    {
                                        DDL_Through.DataTextField = DailyReportThrough.ColumnNames.Name;
                                        DDL_Through.DataValueField = DailyReportThrough.ColumnNames.ID;
                                        DDL_Through.DataSource = through.DefaultView;
                                        DDL_Through.DataBind();
                                    }

                                    LoadReportLines(RID);
                                    

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
                }
                else
                {
                    if (ViewState["ReportDT"] != null)
                        ReportDT = (DataTable)ViewState["ReportDT"];
                    if (ViewState["DeletedReport"] != null)
                        DeletedReport = (ArrayList) ViewState["DeletedReport"]  ;
                }
            }
            catch(Exception ex)
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
            dt.Columns.Add("FileContents",typeof(byte[]));
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
                if (ReportDT.Rows.Count > 0)
                {
                    RPT_Line.DataSource = ReportDT;
                    RPT_Line.DataBind();
                    btn_Finish.Visible = true;
                    BTN_Save.Visible = true;

                }
                else
                {

                    //DataRow dr = ReportDT.NewRow();
                    //dr["ID"] = -2;
                    //dr["ClientID"] ="ssss";
                    //dr["ClientName"] = "sssss";
                    //dr["ThroughID"] ="ssss";
                    //dr["ThroughName"] ="ssss";
                    //dr["From"] = 1.0;
                    //dr["To"] = 1.0;
                    //dr["Location"] = "ssss";
                    //dr["Reason"] = "sssss";
                    //dr["New"] = false;
                    //dr["NewClientAddress"] = "NewClientAddress";
                    //dr["NewClientTel"] = "NewClientTel";
                    //dr["NewText"] = "New";
                    //dr["FileName"] = "";
                    //dr["FileNameWZTime"] = "";
                    //dr["FileExtension"] = "";
                    //dr["FilePath"] = "";
                    //dr["FileContents"] = null;
                    //dr["ContactPerson"] = "";
                    //dr["ContactPersonMobile"] = "";

                    //ReportDT.Rows.Add(dr);
                    //GV_Reports.DataSource = ReportDT;
                    //GV_Reports.DataBind();
                   // GV_Reports.Rows[0].Visible = false;

                    btn_Finish.Visible = false;
                    BTN_Save.Visible = false;
                }
                ViewState["ReportDT"] = ReportDT;
            }
            catch (Exception ex)
            {
                LBL_MSG.Text = "LoadReportLines : " + ex.Message;
                LBL_MSG.Visible = true;
            }
        }

        protected void GV_Reports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        //protected void GV_Reports_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    try
        //    {
        //        string ID = ReportDT.Rows[e.RowIndex + (GV_Reports.PageIndex * GV_Reports.PageSize)]["ID"].ToString();
               
        //        if (ID != "-1")
        //        {
        //            DeletedReport.Add(ID);
        //            ViewState["DeletedReport"] = DeletedReport;
                   
        //        }
        //        ReportDT.Rows[e.RowIndex + (GV_Reports.PageIndex * GV_Reports.PageSize)].Delete();



        //        if (ReportDT.Rows.Count > 0)
        //        {
        //            GV_Reports.DataSource = ReportDT;

        //            GV_Reports.DataBind();
                   
        //        }
        //        else
        //        {

        //            DataRow dr = ReportDT.NewRow();
        //            dr["ID"] = -2;
        //            dr["ClientID"] = "ssss";
        //            dr["ClientName"] = "sssss";
        //            dr["ThroughID"] = "ssss";
        //            dr["ThroughName"] = "ssss";
        //            dr["From"] = 1.0;
        //            dr["To"] = 1.0;
        //            dr["Location"] = "ssss";
        //            dr["Reason"] = "sssss";
        //            dr["New"] = false;
        //            dr["NewClientAddress"] = "NewClientAddress";
        //            dr["NewClientTel"] = "NewClientTel";
        //            dr["NewText"] = "New";
        //            dr["FileName"] = "";
        //            dr["FileNameWZTime"] = "";
        //            dr["FileExtension"] = "";
        //            dr["FilePath"] = "";
        //            dr["FileContents"] = null;
        //            dr["ContactPerson"] = "";
        //            dr["ContactPersonMobile"] = "";

        //            ReportDT.Rows.Add(dr);
        //            GV_Reports.DataSource = ReportDT;
        //            GV_Reports.DataBind();
        //            GV_Reports.Rows[0].Visible = false;
        //            //GV_Countener.Enabled = false;
        //            //  BTN_Cancel.Visible = true;
        //          //  BTN.Visible = false;

        //        }


        //        ViewState["ReportDT"] = ReportDT;

                


        //    }
        //    catch (Exception ex)
        //    {
               
        //        LBL_MSG.Text = "GV_Reports_RowDeleting:" + ex.Message;
        //        LBL_MSG.Visible = true;
        //       // Logger.LogException(ex);

        //    }
        //}

        //protected void GV_Reports_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        switch (e.CommandName)
        //        {
        //            case "Insert":

        //                DropDownList DDL_New = (DropDownList)GV_Reports.FooterRow.FindControl("DDL_New");
        //                //  DateTimeControl DT_KashfDate = (DateTimeControl)GV_Reports.FooterRow.FindControl("DT_KashfDate");
        //                DropDownList DDL_ClientName = (DropDownList)GV_Reports.FooterRow.FindControl("DDL_ClientName");
        //                TextBox TXT_NewClientName = (TextBox)GV_Reports.FooterRow.FindControl("TXT_NewClientName");
        //                TextBox TXT_Tel = (TextBox)GV_Reports.FooterRow.FindControl("TXT_Tel");
        //                TextBox TXT_NewAddress = (TextBox)GV_Reports.FooterRow.FindControl("TXT_NewAddress");
        //                DropDownList DDL_Through = (DropDownList)GV_Reports.FooterRow.FindControl("DDL_Through");
        //                TextBox TXT_From = (TextBox)GV_Reports.FooterRow.FindControl("TXT_From");
        //                TextBox TXT_To = (TextBox)GV_Reports.FooterRow.FindControl("TXT_To");
        //                TextBox TXT_Location = (TextBox)GV_Reports.FooterRow.FindControl("TXT_Location");
        //                TextBox TXT_Reason = (TextBox)GV_Reports.FooterRow.FindControl("TXT_Reason");
        //                FileUpload FU_File = (FileUpload)GV_Reports.FooterRow.FindControl("FU_File");
        //                TextBox TXT_ContactPerson = (TextBox)GV_Reports.FooterRow.FindControl("TXT_ContactPerson");
        //                TextBox TXT_ContactPersonTel = (TextBox)GV_Reports.FooterRow.FindControl("TXT_ContactPersonTel");

                       
        //                //if (TXT_From.Text.Trim() != "" && TXT_To.Text.Trim() != "")
        //                //{

        //                if (ReportDT.Rows.Count > 0 && ReportDT.Rows[0]["ID"].ToString() == "-2")
        //                    {
        //                        ReportDT.Rows[0].Delete();
        //                    }

        //                    DataRow dr = ReportDT.NewRow();
        //                    dr["ID"] = -1;

        //                    if(DDL_New.SelectedValue=="0")
        //                    {
        //                        dr["ClientID"] = DDL_ClientName.SelectedValue;
        //                        dr["ClientName"] = DDL_ClientName.SelectedItem.Text;
        //                        dr["New"] = false;
        //                        dr["FileName"] = "";
        //                    dr["FileNameWZTime"] = "";
        //                    dr["FileExtension"] = "";
        //                        dr["FilePath"] = "";
        //                        dr["FileContents"] = null;

        //                    }
        //                    else
        //                    {
        //                        dr["ClientID"] = "-1";
        //                        dr["ClientName"] = TXT_NewClientName.Text.Trim();
        //                        dr["NewClientAddress"] = TXT_NewAddress.Text.Trim();
        //                        dr["NewClientTel"] = TXT_Tel.Text.Trim();
        //                        dr["New"] = true;
        //                    if (FU_File.HasFile)
        //                    {
        //                        dr["FileName"] = Path.GetFileName(FU_File.PostedFile.FileName);
        //                        dr["FileNameWZTime"] = DateTime.Now.ToFileTime() + "_" + Path.GetFileName(FU_File.PostedFile.FileName);
        //                        dr["FileExtension"] = Path.GetExtension(FU_File.PostedFile.FileName);
        //                        dr["FilePath"] = FU_File.FileName;
        //                        dr["FileContents"] = FU_File.FileBytes;
        //                    }
        //                    else
        //                    {
        //                        dr["FileName"] = "";
        //                        dr["FileNameWZTime"] = "";
        //                        dr["FileExtension"] = "";
        //                        dr["FilePath"] = "";
        //                        dr["FileContents"] = null;
        //                    }
        //                }

        //                    dr["ThroughID"] = DDL_Through.SelectedValue;
        //                    dr["ThroughName"] = DDL_Through.SelectedItem.Text;
        //                    dr["From"] = TXT_From.Text.Trim() ;
        //                    dr["To"] = TXT_To.Text.Trim();
        //                    dr["Location"] = TXT_Location.Text.Trim();
        //                    dr["Reason"] = TXT_Reason.Text.Trim();
                           
                               
        //                    dr["NewText"] = DDL_New.SelectedItem.Text;
        //                dr["ContactPerson"] = TXT_ContactPerson.Text;
        //                dr["ContactPersonMobile"] = TXT_ContactPersonTel.Text;

        //                ReportDT.Rows.Add(dr);
        //                    GV_Reports.DataSource = ReportDT;
        //                    GV_Reports.DataBind();

        //                    ViewState["ReportDT"] = ReportDT;

        //                    LBL_MSG.Text = "";
        //                    LBL_MSG.Visible = false;

        //                    btn_Finish.Visible = true;
        //                    BTN_Save.Visible = true;



        //                //}
        //                //else
        //                //{
        //                //    LBL_MSG.Text = "Please add a correcct visit";
        //                //    LBL_MSG.Visible = true;
        //                //}
        //                break;

        //            case "Update":
        //                break;

        //            case "Cancel":
        //                break;

        //            case "Delete":
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
              
        //        LBL_MSG.Visible = true;
        //        LBL_MSG.Text = "GV_Reports_RowCommand:" + ex.Message;
        //       // Logger.LogException(ex);
        //    }
        //}

        //protected void GV_Reports_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        //if (e.Row.RowType == DataControlRowType.DataRow)
        //        //{




        //        //        if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == (DataControlRowState.Alternate | DataControlRowState.Normal))
        //        //        {
        //        //            Label LBL_Amount = (Label)e.Row.FindControl("LBL_Amount");


        //        //            while (LBL_Amount.Text.Contains(".") && LBL_Amount.Text.Trim().EndsWith("0"))
        //        //                LBL_Amount.Text = LBL_Amount.Text.Substring(0, LBL_Amount.Text.Length - 1);
        //        //            if (LBL_Amount.Text.Trim().EndsWith("."))
        //        //                LBL_Amount.Text = LBL_Amount.Text.Substring(0, LBL_Amount.Text.Length - 1);



        //        //        }


        //        //}
        //        //else
        //        //{
        //        if (e.Row.RowType == DataControlRowType.Footer)
        //        {
        //            if (e.Row.RowState == DataControlRowState.Normal)
        //            {
        //                DropDownList DDL_New = (DropDownList)e.Row.FindControl("DDL_New");
        //                DropDownList DDL_ClientName = (DropDownList)e.Row.FindControl("DDL_ClientName");

        //                RequiredFieldValidator RFV_ClientName = (RequiredFieldValidator)e.Row.FindControl("RFV_ClientName");


        //                Label LBL_NewClientName = (Label)e.Row.FindControl("LBL_NewClientName");
        //                TextBox TXT_NewClientName = (TextBox)e.Row.FindControl("TXT_NewClientName");
        //                RequiredFieldValidator RFV_NewClientName = (RequiredFieldValidator)e.Row.FindControl("RFV_NewClientName");
        //                Label LBL_NewTel = (Label)e.Row.FindControl("LBL_NewTel");
        //                TextBox TXT_Tel = (TextBox)e.Row.FindControl("TXT_Tel");
        //                Label LBL_NewAddress = (Label)e.Row.FindControl("LBL_NewAddress");
        //                TextBox TXT_NewAddress = (TextBox)e.Row.FindControl("TXT_NewAddress");
        //                DropDownList DDL_Through = (DropDownList)e.Row.FindControl("DDL_Through");
        //                FileUpload FU_File = (FileUpload)e.Row.FindControl("FU_File");

        //                if (DDL_New.SelectedValue == "0")
        //                {
        //                    DDL_ClientName.Visible = true;
        //                    RFV_ClientName.Enabled = true;
        //                    LBL_NewClientName.Visible = false;
        //                    TXT_NewClientName.Visible = false;
        //                    RFV_NewClientName.Enabled = false;
        //                    LBL_NewTel.Visible = false;
        //                    TXT_Tel.Visible = false;
        //                    LBL_NewAddress.Visible = false;
        //                    TXT_NewAddress.Visible = false;
        //                    FU_File.Visible = false;

        //                    DDL_ClientName.Items.Clear();
        //                    GarasERP.Client client = new GarasERP.Client();
        //                    client.Where.SalesPersonID.Value = UserID;

        //                    if (client.Query.Load())
        //                    {
        //                        DDL_ClientName.DataTextField = GarasERP.Client.ColumnNames.Name;
        //                        DDL_ClientName.DataValueField = GarasERP.Client.ColumnNames.ID;
        //                        DDL_ClientName.DataSource = client.DefaultView;
        //                        DDL_ClientName.DataBind();
        //                    }

        //                }
        //                else
        //                {
        //                    DDL_ClientName.Visible = false;
        //                    RFV_ClientName.Enabled = false;
        //                    LBL_NewClientName.Visible = true;
        //                    TXT_NewClientName.Visible = true;
        //                    RFV_NewClientName.Enabled = true;
        //                    LBL_NewTel.Visible = true;
        //                    TXT_Tel.Visible = true;
        //                    LBL_NewAddress.Visible = true;
        //                    TXT_NewAddress.Visible = true;
        //                    FU_File.Visible = true;
        //                }


        //                //load employee
        //                DDL_Through.Items.Clear();
        //                DailyReportThrough through = new DailyReportThrough();

        //                if (through.Query.Load())
        //                {
        //                    DDL_Through.DataTextField = DailyReportThrough.ColumnNames.Name;
        //                    DDL_Through.DataValueField = DailyReportThrough.ColumnNames.ID;
        //                    DDL_Through.DataSource = through.DefaultView;
        //                    DDL_Through.DataBind();
        //                }


        //            }
        //        }


        //     //   }
        //    }
        //    catch (Exception ex)
        //    {
        //        //LBL_InnerMessage.Text = "GV_FinMasrof_RowDataBound:" + ex.Message;
        //        LBL_MSG.Visible = true;
        //        LBL_MSG.Text = "GV_FinMasrof_RowDataBound:" + ex.Message;
        //       // Logger.LogException(ex);
        //    }
        //}

        protected void BTN_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ReportDT.Rows.Count > 0 && ReportDT.Rows[0]["ID"].ToString() != "-2")
                {
                    if (Page.Request.QueryString.Count > 0 && Page.Request.QueryString["RID"] != null)
                    {
                        string ID = GarasERP.Encrypt_Decrypt.Decrypt(Page.Request.QueryString["RID"].ToString(), key);
                        long RID = 0;
                        if (long.TryParse(ID, out RID) && RID > 0)
                        {
                            DailyReport report = new DailyReport();
                            report.Where.ID.Value = RID;
                            report.Where.UserID.Value = UserID;
                            report.Where.Status.Value = "Not Filled";
                            if (report.Query.Load())
                            {
                                if (report.DefaultView != null && report.DefaultView.Count > 0)
                                {
                                    foreach (string MID in DeletedReport)
                                    {
                                        DailyReportLine line = new DailyReportLine();
                                        if(line.LoadByPrimaryKey(long.Parse(MID)))
                                        {
                                            DailyReportAttachment attach = new DailyReportAttachment();
                                            attach.Where.ReportLineID.Value = line.ID;
                                            if(attach.Query.Load())
                                            {
                                                do
                                                {
                                                    if (System.IO.File.Exists(attach.AttachmentPath))
                                                    {
                                                        // Use a try block to catch IOExceptions, to
                                                        // handle the case of the file already being
                                                        // opened by another process.
                                                        try
                                                        {
                                                            System.IO.File.Delete(attach.AttachmentPath);
                                                        }
                                                        catch 
                                                        {
                                                            
                                                        }
                                                    }


                                                } while (attach.MoveNext());
                                                attach.Rewind();
                                                attach.DeleteAll();
                                                attach.Save();
                                            }
                                            
                                            line.MarkAsDeleted();
                                            line.Save();
                                        }
                                    }
                                    foreach (DataRow row in ReportDT.Rows)
                                    {

                                        if (row["ID"].ToString() == "-1")
                                        {
                                          

                                            DailyReportLine line = new DailyReportLine();
                                            line.AddNew();
                                            if (row["NewText"].ToString() != "New")
                                            {
                                                line.ClientID = long.Parse(row["ClientID"].ToString());
                                                line.New = false;
                                            }
                                            else
                                                line.New = true;
                                           line.CreationDate = DateTime.Now;
                                            line.DailyReportID = RID;
                                            line.DailyReportThroughID = int.Parse(row["ThroughID"].ToString());
                                            if (row["From"].ToString() != "")
                                                line.FromTIme = double.Parse(row["From"].ToString());
                                            if (row["To"].ToString() != "")
                                                line.ToTime = double.Parse(row["To"].ToString());
                                            line.Location = row["Location"].ToString();
                                            line.ModifiedDate = DateTime.Now;
                                            line.Reason = row["Reason"].ToString();
                                            line.NewClientAddress= row["NewClientAddress"].ToString();
                                            line.NewClientTel= row["NewClientTel"].ToString();
                                            line.Reviewed = false;
                                            line.NewClientName= row["ClientName"].ToString();
                                            line.ContactPerson = row["ContactPerson"].ToString();
                                            line.ContactPersonMobile = row["ContactPersonMobile"].ToString();
                                            line.Save();
                                            long LineID = line.ID;
                                            if (row["FileName"].ToString() !="")
                                            {
                                                DailyReportAttachment attachment = new DailyReportAttachment();
                                                attachment.AddNew();
                                                string virtualPath = "~/Attachments/Reports/" + RID + "/";
                                                var folder = Server.MapPath("~/Attachments/Reports/");
                                                var clientFolder = Server.MapPath(virtualPath);
                                                if (!Directory.Exists(folder))
                                                {
                                                    Directory.CreateDirectory(folder);
                                                }
                                                if (!Directory.Exists(clientFolder))
                                                {
                                                    Directory.CreateDirectory(clientFolder);
                                                }

                                                // string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                                                // string fileNameWzTimeStamp = DateTime.Now.ToFileTime() + "_" + fileName;
                                                // string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);//fileName.Split('.').Last();
                                                attachment.ReportLineID = LineID;
                                                attachment.AttachmentPath = virtualPath + row["FileNameWZTime"].ToString();
                                                if (attachment.AttachmentPath == DBNull.Value.ToString())
                                                    attachment.AttachmentPath = "N/A";
                                                attachment.CreatedBy = UserID;
                                                attachment.CreationDate = DateTime.Now;
                                                attachment.Active = true;
                                                attachment.FileName = row["FileName"].ToString();
                                                attachment.FileExtenssion = row["FileExtension"].ToString();
                                                attachment.Type = "NewClient";//***change value if needed
                                                using (System.IO.FileStream fs = System.IO.File.Create(clientFolder+ row["FileNameWZTime"].ToString()))
                                                {
                                                    byte[] content = (byte[])row["FileContents"];
                                                    fs.Write(content, 0, content.Length);
                                                }

                                               // FileUpload1.PostedFile.SaveAs((clientFolder) + fileNameWzTimeStamp);
                                                attachment.Save();
                                            }


                                            

                                           


                                        }
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
            }
            catch (Exception ex)
            {

                LBL_MSG.Visible = true;
                LBL_MSG.Text = "btn_Save:" + ex.Message;
                // Logger.LogException(ex);
            }







        }

        protected void btn_Finish_Click(object sender, EventArgs e)
        {

            try
            {
                if (ReportDT.Rows.Count > 0 && ReportDT.Rows[0]["ID"].ToString() != "-2")
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
                            report.Where.UserID.Value = UserID;
                            report.Where.Status.Value = "Not Filled";
                            if (report.Query.Load())
                            {
                                if (report.DefaultView != null && report.DefaultView.Count > 0)
                                {
                                    foreach (string MID in DeletedReport)
                                    {
                                        DailyReportLine line = new DailyReportLine();
                                        if (line.LoadByPrimaryKey(long.Parse(MID)))
                                        {
                                            DailyReportAttachment attach = new DailyReportAttachment();
                                            attach.Where.ReportLineID.Value = line.ID;
                                            if (attach.Query.Load())
                                            {
                                                do
                                                {
                                                    if (System.IO.File.Exists(attach.AttachmentPath))
                                                    {
                                                        // Use a try block to catch IOExceptions, to
                                                        // handle the case of the file already being
                                                        // opened by another process.
                                                        try
                                                        {
                                                            System.IO.File.Delete(attach.AttachmentPath);
                                                        }
                                                        catch
                                                        {

                                                        }
                                                    }


                                                } while (attach.MoveNext());
                                                attach.Rewind();
                                                attach.DeleteAll();
                                                attach.Save();
                                            }
                                            line.MarkAsDeleted();
                                            line.Save();
                                        }
                                    }
                                    foreach (DataRow row in ReportDT.Rows)
                                    {

                                        if (row["ID"].ToString() == "-1")
                                        {


                                            DailyReportLine line = new DailyReportLine();
                                            line.AddNew();
                                            if (row["NewText"].ToString() != "New")
                                            {
                                                line.ClientID = long.Parse(row["ClientID"].ToString());
                                                line.New = false;
                                            }
                                            else
                                            {
                                                line.New = true;
                                                hasNew = true;
                                            }
                                            line.CreationDate = DateTime.Now;
                                            line.DailyReportID = RID;
                                            line.DailyReportThroughID = int.Parse(row["ThroughID"].ToString());
                                            if(row["From"].ToString()!="")
                                                line.FromTIme = double.Parse(row["From"].ToString());
                                            if (row["To"].ToString() != "")
                                                line.ToTime = double.Parse(row["To"].ToString());
                                            line.Location = row["Location"].ToString();
                                            line.ModifiedDate = DateTime.Now;
                                            line.Reason = row["Reason"].ToString();
                                            line.NewClientAddress = row["NewClientAddress"].ToString();
                                            line.NewClientTel = row["NewClientTel"].ToString();
                                            line.Reviewed = false;
                                            line.NewClientName = row["ClientName"].ToString();
                                            line.ContactPerson = row["ContactPerson"].ToString();
                                            line.ContactPersonMobile = row["ContactPersonMobile"].ToString();

                                            line.Save();
                                            long LineID = line.ID;
                                            if (row["FileName"].ToString() != "")
                                            {
                                                DailyReportAttachment attachment = new DailyReportAttachment();
                                                attachment.AddNew();
                                                string virtualPath = "~/Attachments/Reports/" + RID + "/";
                                                var folder = Server.MapPath("~/Attachments/Reports/");
                                                var clientFolder = Server.MapPath(virtualPath);
                                                if (!Directory.Exists(folder))
                                                {
                                                    Directory.CreateDirectory(folder);
                                                }
                                                if (!Directory.Exists(clientFolder))
                                                {
                                                    Directory.CreateDirectory(clientFolder);
                                                }

                                                // string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                                                // string fileNameWzTimeStamp = DateTime.Now.ToFileTime() + "_" + fileName;
                                                // string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);//fileName.Split('.').Last();
                                                attachment.ReportLineID = LineID;
                                                attachment.AttachmentPath = virtualPath + row["FileNameWZTime"].ToString();
                                                if (attachment.AttachmentPath == DBNull.Value.ToString())
                                                    attachment.AttachmentPath = "N/A";
                                                attachment.CreatedBy = UserID;
                                                attachment.CreationDate = DateTime.Now;
                                                attachment.Active = true;
                                                attachment.FileName = row["FileName"].ToString();
                                                attachment.FileExtenssion = row["FileExtension"].ToString();
                                                attachment.Type = "NewClient";//***change value if needed
                                                using (System.IO.FileStream fs = System.IO.File.Create(clientFolder + row["FileNameWZTime"].ToString()))
                                                {
                                                    byte[] content = (byte[])row["FileContents"];
                                                    fs.Write(content, 0, content.Length);
                                                }

                                                // FileUpload1.PostedFile.SaveAs((clientFolder) + fileNameWzTimeStamp);
                                                attachment.Save();
                                            }


                                        }
                                    }

                                    //update client report date

                                    DailyReportLine AllLines = new DailyReportLine();
                                    AllLines.Where.DailyReportID.Value = report.ID;
                                    AllLines.Where.New.Value = false;
                                    if(AllLines.Query.Load())
                                    {
                                        if (AllLines.DefaultView != null && AllLines.DefaultView.Count>0)
                                        {
                                            do
                                            {
                                                if (AllLines.s_ClientID != "")
                                                {
                                                    GarasERP.Client client = new GarasERP.Client();

                                                    if (client.LoadByPrimaryKey(AllLines.ClientID))
                                                    {
                                                        client.LastReportDate = report.ReprotDate;
                                                        client.Save();
                                                    }
                                                }
                                            } while (AllLines.MoveNext());
                                        }
                                    }


                                    report.Status = "Pending Verification";
                                    report.ModifiedDate = DateTime.Now;
                                    report.Save();

                                    string UserName = "";
                                    long BranchID = 0;
                                    User user = new GarasERP.User();
                                    if (user.LoadByPrimaryKey(UserID))
                                    {
                                        UserName = user.FirstName + " " + user.LastName;
                                        if(user.s_BranchID !="")
                                            BranchID = user.BranchID;
                                    }
                                    string URL = System.Web.Configuration.WebConfigurationManager.AppSettings["RootWeb"].ToString();
                                    Common.sendGroupBranchNotifications(BranchID, "SalesManagers", "New Daily Report (" + UserName + " " + report.ReprotDate.ToString("dd/MM/yyyy") + ")" 
                                                            , "New Daily Report (" + UserName + " " + report.ReprotDate.ToString("dd/MM/yyyy") + ")" , URL + "/Sales/Reports/VarifyReport.aspx?RID="+ Page.Request.QueryString["RID"].ToString());

                                    if(hasNew)
                                        Common.sendGroupBranchNotifications(BranchID, "Secretary", "New Client in Daily Report (" + UserName + " " + report.ReprotDate.ToString("dd/MM/yyyy") + ")"
                                                            , "New Client in Daily Report (" + UserName + " " + report.ReprotDate.ToString("dd/MM/yyyy") + ")", URL+ "/Sales/Reports/ViewReport.aspx?RID="+ Page.Request.QueryString["RID"].ToString());

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
            }
            catch (Exception ex)
            {

                LBL_MSG.Visible = true;
                LBL_MSG.Text = "btn_Save:" + ex.Message;
                // Logger.LogException(ex);
            }


           
        }


    


        private void sendSecNotifications(long reportID, string SalesMan, string ReportDate)
        {
            try
            {
                Group gp = new Group();
                gp.Where.Name.Value = "SalesManagers";
                gp.Where.Active.Value = 1;
                if (gp.Query.Load())
                {
                    if (gp.DefaultView != null && gp.DefaultView.Count > 0)
                    {
                        Group_User gpUser = new Group_User();
                        gpUser.Where.Active.Value = 1;
                        gpUser.Where.GroupID.Value = gp.ID;
                        if (gpUser.Query.Load())
                        {
                            string titleNotify = "New Daily Report (" + SalesMan + " " + ReportDate + ")";
                            string descNotify = "New Daily Report (" + SalesMan + " " + ReportDate + ")";

                            if (gpUser.DefaultView != null && gpUser.DefaultView.Count > 0)
                            {

                                do
                                {
                                    Common.sendNotifications(gpUser.UserID, titleNotify, descNotify,
                                                      "Sales/Reports/VarifyReport.aspx?RID=" + Server.UrlEncode(Encrypt_Decrypt.Encrypt(reportID.ToString(), key)));
                                } while (gpUser.MoveNext());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                LBL_MSG.Visible = true;
                LBL_MSG.Text = "sendBranchNotifications:" + ex.Message;
                // Logger.LogException(ex);
            }
        }
        protected void DDL_New_SelectedIndexChanged(object sender, EventArgs e)
        {
           try
            {
                

                if (DDL_New.SelectedValue == "0")
                {
                    //  DDL_ClientName.Visible = true;
                    TXT_OldClientName.Visible = true;
                    RFV_ClientName.Enabled = true;
                   // LBL_NewClientName.Visible = false;
                    TXT_NewClientName.Visible = false;
                    RFV_NewClientName.Enabled = false;
                    Div_Tel.Visible = false;
                    TXT_Tel.Visible = false;
                    //LBL_NewAddress.Visible = false;
                   TXT_NewAddress.Visible = false;
                    Div_Addres.Visible = false;
                    FU_File.Visible = false;
                    Div_Attachment.Visible = false;

                    //DDL_ClientName.Items.Clear();
                    //GarasERP.Client client = new GarasERP.Client();
                    //client.Where.SalesPersonID.Value = UserID;

                    //if (client.Query.Load())
                    //{
                    //    DDL_ClientName.DataTextField = GarasERP.Client.ColumnNames.Name;
                    //    DDL_ClientName.DataValueField = GarasERP.Client.ColumnNames.ID;
                    //    DDL_ClientName.DataSource = client.DefaultView;
                    //    DDL_ClientName.DataBind();
                    //}

                }
                else
                {
                    //  DDL_ClientName.Visible = false;
                    TXT_OldClientName.Visible = false;
                    RFV_ClientName.Enabled = false;
                   // LBL_NewClientName.Visible = true;
                    TXT_NewClientName.Visible = true;
                    RFV_NewClientName.Enabled = true;
                    Div_Tel.Visible = true;
                    TXT_Tel.Visible = true;
                    //LBL_NewAddress.Visible = true;
                    TXT_NewAddress.Visible = true;
                    Div_Addres.Visible = true;
                    Div_Attachment.Visible = true;
                    FU_File.Visible = true;
                }

            }
            catch (Exception ex)
            {

                LBL_MSG.Visible = true;
                LBL_MSG.Text = "GV_Reports_RowCommand:" + ex.Message;
                // Logger.LogException(ex);
            }     
        }

        protected void BTN_NewLine_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (ReportDT.Rows.Count > 0 && ReportDT.Rows[0]["ID"].ToString() == "-2")
                {
                    ReportDT.Rows[0].Delete();
                }

                DataRow dr = ReportDT.NewRow();
                dr["ID"] = -1;

                if (DDL_New.SelectedValue == "0")
                {
                    if (HDN_ClientID.Value != "")
                    {
                        //dr["ClientID"] = DDL_ClientName.SelectedValue;
                        //dr["ClientName"] = DDL_ClientName.SelectedItem.Text;
                        dr["ClientID"] = HDN_ClientID.Value;
                        dr["ClientName"] = TXT_OldClientName.Text;
                        dr["New"] = false;
                        dr["FileName"] = "";
                        dr["FileNameWZTime"] = "";
                        dr["FileExtension"] = "";
                        dr["FilePath"] = "";
                        dr["FileContents"] = null;
                        dr["NewClass"] = "badge badge-secondary";
                        HDN_ClientID.Value = "";
                        TXT_OldClientName.Text = "";
                        LBL_MSG.Visible = false;
                        LBL_MSG.Text = "";
                    }
                    else
                    {
                        LBL_MSG.Visible = true;
                        LBL_MSG.Text = "Please Select a correct Client";
                        return;
                    }

                }
                else
                {
                    dr["ClientID"] = "-1";
                    dr["ClientName"] = TXT_NewClientName.Text.Trim();
                    dr["NewClientAddress"] = TXT_NewAddress.Text.Trim();
                    dr["NewClientTel"] = TXT_Tel.Text.Trim();
                    dr["New"] = true;
                    dr["NewClass"] = "badge badge-primary";
                    if (FU_File.HasFile)
                    {
                        dr["FileName"] = Path.GetFileName(FU_File.PostedFile.FileName);
                        dr["FileNameWZTime"] = DateTime.Now.ToFileTime() + "_" + Path.GetFileName(FU_File.PostedFile.FileName);
                        dr["FileExtension"] = Path.GetExtension(FU_File.PostedFile.FileName);
                        dr["FilePath"] = FU_File.FileName;
                        dr["FileContents"] = FU_File.FileBytes;
                    }
                    else
                    {
                        dr["FileName"] = "";
                        dr["FileNameWZTime"] = "";
                        dr["FileExtension"] = "";
                        dr["FilePath"] = "";
                        dr["FileContents"] = null;
                    }





                    LBL_MSG.Visible = false;
                    LBL_MSG.Text = "";
                }

                dr["ThroughID"] = DDL_Through.SelectedValue;
                dr["ThroughName"] = DDL_Through.SelectedItem.Text;
                dr["From"] = TXT_From.Text.Trim();
                dr["To"] = TXT_To.Text.Trim();
                dr["Location"] = TXT_Location.Text.Trim();
                dr["Reason"] = TXT_Reason.Text.Trim();


                dr["NewText"] = DDL_New.SelectedItem.Text;
                dr["ContactPerson"] = TXT_ContactPerson.Text;
                dr["ContactPersonMobile"] = TXT_ContactPersonTel.Text;

                ReportDT.Rows.Add(dr);
                //GV_Reports.DataSource = ReportDT;
                //GV_Reports.DataBind();
                RPT_Line.DataSource = ReportDT;
                RPT_Line.DataBind();

                ViewState["ReportDT"] = ReportDT;

                LBL_MSG.Text = "";
                LBL_MSG.Visible = false;

                btn_Finish.Visible = true;
                BTN_Save.Visible = true;
                TXT_ContactPerson.Text = "";
                TXT_ContactPersonTel.Text = "";
                TXT_From.Text = "";
                TXT_Location.Text = "";
                TXT_NewAddress.Text = "";
                TXT_NewClientName.Text = "";
                TXT_OldClientName.Text = "";
                TXT_Reason.Text = "";
                TXT_Tel.Text = "";
                TXT_To.Text = "";
                
            }
            catch (Exception ex)
            {

                LBL_MSG.Visible = true;
                LBL_MSG.Text = "BTN_NewLine_Click" + ex.Message;
                // Logger.LogException(ex);
            }
        }

      

        protected void btn_Edit_Command(object sender, CommandEventArgs e)
        {
            try
            {
                // string ID = ReportDT.Rows[e.RowIndex + (GV_Reports.PageIndex * GV_Reports.PageSize)]["ID"].ToString();
                int index = int.Parse(e.CommandArgument.ToString());
                string ID = ReportDT.Rows[index]["ID"].ToString();
                BTN_NewLine.Visible = false;
                BTN_EditLine.Visible = true;
                BTN_Cancel.Visible = true;

               // HDN_LineID.Value = 




            }
            catch (Exception ex)
            {

                LBL_MSG.Text = "btn_Edit_Command:" + ex.Message;
                LBL_MSG.Visible = true;
                // Logger.LogException(ex);

            }
        }

        protected void BTN_Delete_Command(object sender, CommandEventArgs e)
        {
            try
            {
                // string ID = ReportDT.Rows[e.RowIndex + (GV_Reports.PageIndex * GV_Reports.PageSize)]["ID"].ToString();
                int index = int.Parse(e.CommandArgument.ToString());
                string ID = ReportDT.Rows[index]["ID"].ToString();
                if (ID != "-1")
                {
                    DeletedReport.Add(ID);
                    ViewState["DeletedReport"] = DeletedReport;

                }
                ReportDT.Rows[index].Delete();

                RPT_Line.DataSource = ReportDT;
                RPT_Line.DataBind();

                if(ReportDT.Rows.Count==0)
                {
                    btn_Finish.Visible = false;
                    
                }


                ViewState["ReportDT"] = ReportDT;




            }
            catch (Exception ex)
            {

                LBL_MSG.Text = "GV_Reports_RowDeleting:" + ex.Message;
                LBL_MSG.Visible = true;
                // Logger.LogException(ex);

            }
        }

        protected void BTN_EditLine_Click(object sender, EventArgs e)
        {

        }

        protected void BTN_Cancel_Click(object sender, EventArgs e)
        {

        }


        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchCients(string prefixText, int count)
        {
            List<string> customers = new List<string>();
            try
            {
                GarasERP.Client client = new GarasERP.Client();
                client.Where.Name.Value = "%" +prefixText + "%";
                client.Where.Name.Operator = MyGeneration.dOOdads.WhereParameter.Operand.Like;
                client.Where.SalesPersonID.Value = SID;
                if (client.Query.Load())
                {
                    if(client.DefaultView!=null && client.DefaultView.Count>0)
                    {
                        do
                        {
                            string item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(client.Name, client.s_ID);
                            customers.Add(item);
                        } while (client.MoveNext());
                    }
                }
                return customers;
            }
            catch
            {
                return customers;
            }
           
             
            
        }

     
    }
}