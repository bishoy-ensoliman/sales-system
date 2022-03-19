using GarasERP;
using MyGeneration.dOOdads;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarasSales.Sales.Target
{
    public partial class TargetDistribution : System.Web.UI.Page
    {
        enum SortDir
        {
            None = 0,
            ASC = 1,
            DESC = 2
        }
        #region ViewStates
        static string key = "SalesGarasPass";

        private SortDir NameSortDir
        {
            get
            {
                if (ViewState["NameSortDir"] == null)
                {
                    ViewState["NameSortDir"] = SortDir.None;
                }
                return (SortDir)ViewState["NameSortDir"];
            }
            set
            {
                ViewState["NameSortDir"] = value;
            }
        }

        private SortDir LastTargetSortDir
        {
            get
            {
                if (ViewState["LastTargetSortDir"] == null)
                {
                    ViewState["LastTargetSortDir"] = SortDir.None;
                }
                return (SortDir)ViewState["LastTargetSortDir"];
            }
            set
            {
                ViewState["LastTargetSortDir"] = value;
            }
        }

        private SortDir AvgTargetSortDir
        {
            get
            {
                if (ViewState["AvgTargetSortDir"] == null)
                {
                    ViewState["AvgTargetSortDir"] = SortDir.None;
                }
                return (SortDir)ViewState["AvgTargetSortDir"];
            }
            set
            {
                ViewState["AvgTargetSortDir"] = value;
            }
        }

        private SortDir NewTargetSortDir
        {
            get
            {
                if (ViewState["NewTargetSortDir"] == null)
                {
                    ViewState["NewTargetSortDir"] = SortDir.None;
                }
                return (SortDir)ViewState["NewTargetSortDir"];
            }
            set
            {
                ViewState["NewTargetSortDir"] = value;
            }
        }

        private int TargetID
        {
            get
            {
                if (ViewState["TargetID"] == null)
                {
                    return -1;
                }
                else
                {
                    return (int)ViewState["TargetID"];
                }
            }
            set
            {
                ViewState["TargetID"] = value;
            }
        }

        private double TargetAmount
        {
            get
            {
                if (ViewState["TargetAmount"] == null)
                {
                    return 0;
                }
                else
                {
                    return (double)ViewState["TargetAmount"];
                }
            }
            set
            {
                ViewState["TargetAmount"] = value;
            }
        }

        private int TargetCurrencyID
        {
            get
            {
                if (ViewState["TargetCurrencyID"] == null)
                {
                    return -1;
                }
                else
                {
                    return (int)ViewState["TargetCurrencyID"];
                }
            }
            set
            {
                ViewState["TargetCurrencyID"] = value;
            }
        }

        private int BranchID
        {
            get
            {
                if (ViewState["BranchID"] == null)
                {
                    return -1;
                }
                else
                {
                    return (int)ViewState["BranchID"];
                }
            }
            set
            {
                ViewState["BranchID"] = value;
            }
        }

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

                    return long.Parse(Encrypt_Decrypt.Decrypt(id, key));
                }
            }
            set
            {
                Session["UserID"] = value;
            }
        }

        private DataTable VSSalesDist
        {
            get
            {
                if (ViewState["SalesDist"] == null)
                {
                    return new DataTable();
                }
                else
                {
                    return (DataTable)ViewState["SalesDist"];
                }
            }
            set
            {
                ViewState["SalesDist"] = value;
            }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Common.CheckUserInRole(UserID, 3))
                {
                    if (!Page.IsPostBack)
                    {
                        TargetID = getTargetIDfromQS();
                        //string targetIDs = getBranchTargetIDs();
                        //if(targetIDs != "")
                        //{
                        LoadTargetYearsDDL();
                        //}
                        LoadTargetDist();
                        btnSortName_Click(null, null);
                    }
                    divPage.Visible = true;
                }
                else
                {
                    divPage.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "NoPriveledge", "alert('You Donot have sufficient priveledges!');window.location.href='/Sales/SalesHome.aspx';", true);
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private void LoadTargetDist()
        {
            try
            {
                if (TargetID != -1)
                {
                    if (targetAlreadyExists())
                    {
                        btnSave.Text = "Edit";
                    }
                    else
                    {
                        btnSave.Text = "Save";
                    }
                    BranchID = getBranchIDfromUser();
                    SalesBranchTarget salesBranchTarget = getBranchTarget();//must load TargetID and BranchID first
                    if (salesBranchTarget == null)
                    {
                        TargetAmount = 0;
                        TargetCurrencyID = -1;
                    }
                    else
                    {
                        TargetAmount = (double)salesBranchTarget.Amount;
                        TargetCurrencyID = salesBranchTarget.CurrencyID;
                    }
                    lblBranchTarget.Text = TargetAmount.ToString();
                    //string targetIDs = getLast5YearsTargetIDs();
                    string salesMenIDs = getAllSalesMenIDs();
                    string salesMenInBranchIDs = getMyBranchSalesMenIDs(salesMenIDs);
                    DataTable userLastTargets = getLastUserTargets(salesMenInBranchIDs, BranchID);
                    if (salesMenIDs != "")
                    {
                        LoadSalesMenTable(salesMenIDs, userLastTargets);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page),
                        "NoTarget", "alert('Target is invalid!');", true);
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private void LoadTargetYearsDDL()
        {
            try
            {
                ddlTargetYear.Items.Clear();
                SalesTarget target = new SalesTarget();
                target.Where.Active.Value = 1;
                target.Where.Year.Value = DateTime.Now.Year;
                target.Where.Year.Operator = WhereParameter.Operand.GreaterThanOrEqual;
                target.Query.AddOrderBy(SalesTarget.ColumnNames.Year, WhereParameter.Dir.ASC);
                target.Query.Load();
                if (target.RowCount > 0)
                {
                    target.Rewind();
                    do
                    {
                        ddlTargetYear.Items.Add(new ListItem(target.Year.ToString(), target.ID.ToString()));
                    } while (target.MoveNext());
                }
                if (TargetID != -1)
                {
                    ddlTargetYear.SelectedValue = TargetID.ToString();
                }
                else
                {
                    ddlTargetYear.SelectedIndex = 0;
                    TargetID = int.Parse(ddlTargetYear.SelectedValue);
                }
                ddlTargetYear_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        //private DataTable getLastUserTargets(string salesMenInBranchIDs, int branchID)
        //{
        //    DataTable lastUserTargets = new DataTable();
        //    lastUserTargets.Columns.Add("SalesManID", typeof(long));
        //    lastUserTargets.Columns.Add("TargetYear", typeof(int));
        //    lastUserTargets.Columns.Add("Target", typeof(double));
        //    lastUserTargets.AcceptChanges();
        //    if (salesMenInBranchIDs != "")
        //    {
        //        SalesBranchUserTarget userTarget = new SalesBranchUserTarget();
        //        userTarget.Where.BranchID.Value = branchID;
        //        userTarget.Where.UserID.Value = salesMenInBranchIDs;
        //        userTarget.Where.UserID.Operator = WhereParameter.Operand.In;
        //        //userTarget.Where.Active.Value = 1;
        //        //userTarget.Where.TargetID.Value = targetIDs;
        //        //userTarget.Where.TargetID.Operator = WhereParameter.Operand.In;
        //        userTarget.Query.AddOrderBy(SalesBranchUserTarget.ColumnNames.TargetID, WhereParameter.Dir.DESC);
        //        //userTarget.Query.Top = 5;
        //        userTarget.Query.Load();
        //        if(userTarget.RowCount > 0)
        //        {                    
        //            userTarget.Rewind();
        //            do
        //            {
        //                DataRow dr = lastUserTargets.NewRow();
        //                dr["SalesManID"] = userTarget.UserID;
        //                dr["TargetYear"] = getTargetYear(userTarget.TargetID);
        //                dr["Target"] = userTarget.Amount;
        //                lastUserTargets.Rows.Add(dr);
        //            } while (userTarget.MoveNext());
        //            lastUserTargets.AcceptChanges();
        //        }
        //    }
        //    return lastUserTargets;
        //}

        private DataTable getLastUserTargets(string salesMenInBranchIDs, int branchID)
        {
            DataTable lastUserTargets = new DataTable();
            try
            {
                lastUserTargets.Columns.Add("SalesManID", typeof(long));
                lastUserTargets.Columns.Add("TargetYear", typeof(int));
                lastUserTargets.Columns.Add("Target", typeof(double));
                lastUserTargets.AcceptChanges();
                if (salesMenInBranchIDs != "")
                {
                    V_SalesBranchUserTarget_TargetYear userTarget = new V_SalesBranchUserTarget_TargetYear();
                    userTarget.Where.BranchID.Value = branchID;
                    userTarget.Where.UserID.Value = salesMenInBranchIDs;
                    userTarget.Where.UserID.Operator = WhereParameter.Operand.In;
                    userTarget.Where.Year.Value = ddlTargetYear.SelectedItem.Text;
                    userTarget.Where.Year.Operator = WhereParameter.Operand.LessThan;
                    //userTarget.Where.Active.Value = 1;
                    //userTarget.Where.TargetID.Value = targetIDs;
                    //userTarget.Where.TargetID.Operator = WhereParameter.Operand.In;
                    userTarget.Query.AddOrderBy(V_SalesBranchUserTarget_TargetYear.ColumnNames.Year, WhereParameter.Dir.DESC);
                    //userTarget.Query.Top = 5;
                    userTarget.Query.Load();
                    if (userTarget.RowCount > 0)
                    {
                        userTarget.Rewind();
                        int yearCount = 0;
                        int year = int.Parse(ddlTargetYear.SelectedItem.Text);
                        do
                        {
                            if (year != userTarget.Year)
                            {
                                year = userTarget.Year;
                                yearCount++;
                            }
                            if (yearCount == 6)
                            {
                                break;
                            }
                            DataRow dr = lastUserTargets.NewRow();
                            dr["SalesManID"] = userTarget.UserID;
                            dr["TargetYear"] = userTarget.Year;
                            dr["Target"] = userTarget.Amount;
                            lastUserTargets.Rows.Add(dr);
                        } while (userTarget.MoveNext());
                        lastUserTargets.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return lastUserTargets;
        }

        private int getTargetYear(int targetID)
        {
            SalesTarget target = new SalesTarget();
            target.LoadByPrimaryKey(targetID);
            return target.Year;
        }

        private string getMyBranchSalesMenIDs(string salesMenIDs)
        {
            string myBranchSalesMenIDs = "";
            try
            {
                User salesMen = new User();
                salesMen.Where.Active.Value = 1;
                salesMen.Where.BranchID.Value = BranchID;
                salesMen.Where.ID.Value = salesMenIDs;
                salesMen.Where.ID.Operator = WhereParameter.Operand.In;
                if (salesMen.Query.Load())
                {
                    if (salesMen.DefaultView != null && salesMen.DefaultView.Count > 0)
                    {
                        do
                        {
                            if (myBranchSalesMenIDs == "")
                                myBranchSalesMenIDs = "'" + salesMen.ID + "'";
                            else
                                myBranchSalesMenIDs += ",'" + salesMen.ID + "'";

                        } while (salesMen.MoveNext());
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return myBranchSalesMenIDs;
        }

        private string getAllSalesMenIDs()
        {
            string salesMenIDs = "";
            try
            {
                Group gp = new Group();
                gp.Where.Active.Value = 1;
                gp.Where.Name.Value = "SalesMen";
                gp.Query.Load();
                if (gp.RowCount > 0)
                {
                    Group_User gpUser = new Group_User();
                    gpUser.Where.Active.Value = 1;
                    gpUser.Where.GroupID.Value = gp.ID;
                    if (gpUser.Query.Load())
                    {
                        if (gpUser.DefaultView != null && gpUser.DefaultView.Count > 0)
                        {
                            do
                            {
                                if (salesMenIDs == "")
                                    salesMenIDs = "'" + gpUser.UserID + "'";
                                else
                                    salesMenIDs += ",'" + gpUser.UserID + "'";

                            } while (gpUser.MoveNext());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return salesMenIDs;
        }

        private string getLast5YearsTargetIDs()
        {
            string targetIDs = "";
            try
            {
                string targetYears = "";
                SalesTarget thisTarget = new SalesTarget();
                thisTarget.LoadByPrimaryKey(TargetID);
                int targetYear = thisTarget.Year;
                SalesTarget target = new SalesTarget();
                target.Where.Active.Value = true;
                target.Where.Year.Value = targetYear;
                target.Where.Year.Operator = WhereParameter.Operand.LessThan;
                target.Query.AddOrderBy(SalesTarget.ColumnNames.Year, WhereParameter.Dir.DESC);
                target.Query.Top = 5;
                if (target.Query.Load())
                {
                    if (target.DefaultView != null && target.DefaultView.Count > 0)
                    {
                        do
                        {
                            if (targetIDs == "")
                                targetIDs = "'" + target.ID + "'";
                            else
                                targetIDs += ",'" + target.ID + "'";

                            if (targetYears == "")
                                targetYears = "'" + target.Year + "'";
                            else
                                targetYears += ",'" + target.Year + "'";

                        } while (target.MoveNext());
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return targetIDs;
        }

        private string getBranchTargetIDs()
        {
            string targetIDs = "";
            try
            {
                SalesBranchTarget branchTarget = new SalesBranchTarget();
                branchTarget.Where.Active.Value = 1;
                branchTarget.Where.BranchID.Value = BranchID;
                if (branchTarget.Query.Load())
                {
                    if (branchTarget.DefaultView != null && branchTarget.DefaultView.Count > 0)
                    {
                        do
                        {
                            if (targetIDs == "")
                            {
                                targetIDs = "'" + branchTarget.TargetID + "'";
                            }
                            else
                            {
                                targetIDs += ",'" + branchTarget.TargetID + "'";
                            }

                        } while (branchTarget.MoveNext());
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return targetIDs;
        }

        private SalesBranchTarget getBranchTarget()
        {
            try
            {
                SalesBranchTarget branchTarget = new SalesBranchTarget();
                branchTarget.Where.Active.Value = 1;
                branchTarget.Where.BranchID.Value = BranchID;
                branchTarget.Where.TargetID.Value = TargetID;
                branchTarget.Query.Load();
                if (branchTarget.RowCount > 0)
                {
                    return branchTarget;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return null;
        }

        private int getBranchIDfromUser()
        {
            int branchID = -1;
            try
            {
                User user = new User();
                user.LoadByPrimaryKey(UserID);
                branchID = user.BranchID;
            }
            catch (Exception exc)
            {
                //Logger.LogException(exc);
                branchID = -1;
            }
            return branchID;
        }

        private int getTargetIDfromQS()
        {
            try
            {
                if (Request.QueryString["Target"] != null)
                {
                    string qstr = Request.Url.Query;
                    string targetEnc = qstr.Substring(qstr.IndexOf("Target=") + 7);//Request.QueryString["Target"].ToString();
                    int targetID = -1;
                    int.TryParse(Encrypt_Decrypt.Decrypt(targetEnc, key), out targetID);
                    return targetID;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return -1;
        }

        private void LoadSalesMenTable(string salesMenIDs, DataTable userLastTargets)
        {
            try
            {
                User salesMen = new User();
                salesMen.Where.Active.Value = 1;
                salesMen.Where.BranchID.Value = BranchID;
                salesMen.Where.ID.Value = salesMenIDs;
                salesMen.Where.ID.Operator = WhereParameter.Operand.In;
                salesMen.Query.Load();
                int salesMenNum = salesMen.RowCount;
                Dictionary<long, double> userTarget = getThisTargetsDistribution();
                if (salesMenNum > 0)
                {
                    DataTable dt = createTargetDistTAble();
                    salesMen.Rewind(); //move to first record
                    do
                    {
                        double[] salesManLastTargets = getLastTargetsFromDT(userLastTargets.Select("SalesManID = " + salesMen.ID));
                        DataRow dr = dt.NewRow();
                        dr["UserID"] = salesMen.ID;
                        dr["UserName"] = salesMen.FirstName + " " + salesMen.LastName;
                        dr["LastYearTarget"] = salesManLastTargets[0];
                        dr["AvgLast5Years"] = salesManLastTargets[1];
                        //dr["Commitment"] = 25000;//***change hard coded values
                        if (btnSave.Text == "Edit" && userTarget.ContainsKey(salesMen.ID))
                        {
                            dr["NewTarget"] = userTarget[salesMen.ID];
                        }
                        else
                        {
                            dr["NewTarget"] = TargetAmount * 1.0 / salesMenNum;
                        }
                        dr["tobeSaved"] = true;
                        dt.Rows.Add(dr);
                    } while (salesMen.MoveNext());
                    dt.AcceptChanges();
                    VSSalesDist = dt;
                    rptrSalesMen.DataSource = VSSalesDist;
                    rptrSalesMen.DataBind();
                }
                else
                {
                    rptrSalesMen.DataSource = null;
                    rptrSalesMen.DataBind();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private double[] getLastTargetsFromDT(DataRow[] dataRows)
        {
            double[] lastYearWzSum = new double[2];
            lastYearWzSum[0] = 0;// (double)dataRows[0]["Target"];
            lastYearWzSum[1] = 0;
            try
            {
                int maxYear = 0;
                foreach (DataRow dr in dataRows)
                {
                    if (maxYear < (int)dr["TargetYear"])
                    {
                        maxYear = (int)dr["TargetYear"];
                        lastYearWzSum[0] = (double)dr["Target"];
                    }
                    lastYearWzSum[1] += (double)dr["Target"];
                }
                lblLastYear.Text = maxYear == 0 ? (int.Parse(ddlTargetYear.SelectedItem.Text)-1).ToString(): maxYear.ToString();
                if(dataRows.Length != 0)
                    lastYearWzSum[1] /= dataRows.Length;
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return lastYearWzSum;
        }

        private DataTable createTargetDistTAble()
        {
            DataTable newTable = new DataTable();
            newTable.Columns.Add("UserID", typeof(long));
            newTable.Columns.Add("UserName", typeof(string));
            newTable.Columns.Add("LastYearTarget", typeof(double));
            newTable.Columns.Add("AvgLast5Years", typeof(double));
            newTable.Columns.Add("Commitment", typeof(double));
            newTable.Columns.Add("NewTarget", typeof(double));
            newTable.Columns.Add("tobeSaved", typeof(bool));
            newTable.AcceptChanges();
            return newTable;
        }

        protected void chbxEqual_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbxEqual.Checked)
                {
                    equatTobeSavedMen();
                    rebindSalesMenRptr();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private void equatTobeSavedMen()
        {
            try
            {
                DataTable dt = VSSalesDist;
                if (dt != null && dt.Rows.Count > 0)
                {
                    int tobeSavedCount = dt.Select("tobeSaved = true").Length;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if ((bool)dr["tobeSaved"])
                        {
                            dr["NewTarget"] = TargetAmount * 1.0 / tobeSavedCount;
                        }
                        else
                        {
                            dr["NewTarget"] = 0;
                        }
                    }
                    dt.AcceptChanges();
                    VSSalesDist = dt;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void chbxVar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbxVar.Checked)
                {
                    rebindSalesMenRptr();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private void rebindSalesMenRptr()
        {
            try
            {
                if (VSSalesDist.Rows.Count > 0)
                {
                    rptrSalesMen.DataSource = VSSalesDist;
                    rptrSalesMen.DataBind();
                }
                else
                {
                    rptrSalesMen.DataSource = null;
                    rptrSalesMen.DataBind();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void btnSortName_Click(object sender, EventArgs e)
        {
            try
            {
                if (NameSortDir == SortDir.None || NameSortDir == SortDir.DESC)
                {
                    NameSortDir = SortDir.ASC;
                    DataView dv = VSSalesDist.DefaultView;
                    dv.Sort = "UserName ASC";
                    VSSalesDist = dv.ToTable();
                }
                else
                {
                    NameSortDir = SortDir.DESC;
                    DataView dv = VSSalesDist.DefaultView;
                    dv.Sort = "UserName DESC";
                    VSSalesDist = dv.ToTable();
                }
                rebindSalesMenRptr();
                LastTargetSortDir = SortDir.None;
                AvgTargetSortDir = SortDir.None;
                NewTargetSortDir = SortDir.None;
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void btnSortYear_Click(object sender, EventArgs e)
        {
            try
            {
                if (LastTargetSortDir == SortDir.None || LastTargetSortDir == SortDir.DESC)
                {
                    LastTargetSortDir = SortDir.ASC;
                    DataView dv = VSSalesDist.DefaultView;
                    dv.Sort = "LastYearTarget ASC";
                    VSSalesDist = dv.ToTable();
                }
                else
                {
                    LastTargetSortDir = SortDir.DESC;
                    DataView dv = VSSalesDist.DefaultView;
                    dv.Sort = "LastYearTarget DESC";
                    VSSalesDist = dv.ToTable();
                }
                rebindSalesMenRptr();
                NameSortDir = SortDir.None;
                AvgTargetSortDir = SortDir.None;
                NewTargetSortDir = SortDir.None;
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void btnSortAvgYears_Click(object sender, EventArgs e)
        {
            try
            {
                if (AvgTargetSortDir == SortDir.None || AvgTargetSortDir == SortDir.DESC)
                {
                    AvgTargetSortDir = SortDir.ASC;
                    DataView dv = VSSalesDist.DefaultView;
                    dv.Sort = "AvgLast5Years ASC";
                    VSSalesDist = dv.ToTable();
                }
                else
                {
                    AvgTargetSortDir = SortDir.DESC;
                    DataView dv = VSSalesDist.DefaultView;
                    dv.Sort = "AvgLast5Years DESC";
                    VSSalesDist = dv.ToTable();
                }
                rebindSalesMenRptr();
                NameSortDir = SortDir.None;
                LastTargetSortDir = SortDir.None;
                NewTargetSortDir = SortDir.None;
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void btnSortCommit_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void btnSortTarget_Click(object sender, EventArgs e)
        {
            try
            {
                if (NewTargetSortDir == SortDir.None || NewTargetSortDir == SortDir.DESC)
                {
                    NewTargetSortDir = SortDir.ASC;
                    DataView dv = VSSalesDist.DefaultView;
                    dv.Sort = "NewTarget ASC";
                    VSSalesDist = dv.ToTable();
                }
                else
                {
                    NewTargetSortDir = SortDir.DESC;
                    DataView dv = VSSalesDist.DefaultView;
                    dv.Sort = "NewTarget DESC";
                    VSSalesDist = dv.ToTable();
                }
                rebindSalesMenRptr();
                NameSortDir = SortDir.None;
                LastTargetSortDir = SortDir.None;
                AvgTargetSortDir = SortDir.None;
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void rptrSalesMen_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView salesManTarget = ((DataRowView)e.Item.DataItem);
                    CheckBox chbxSalesManRep = ((CheckBox)e.Item.FindControl("chbxSalesMan"));
                    chbxSalesManRep.Checked = (bool)salesManTarget["tobeSaved"];
                    chbxSalesManRep.InputAttributes.Add("itemIndx", e.Item.ItemIndex.ToString());
                    Label lblSalesManNameRep = ((Label)e.Item.FindControl("lblSalesManName"));
                    lblSalesManNameRep.Text = salesManTarget["UserName"].ToString();
                    Label lblLastYearTargetRep = ((Label)e.Item.FindControl("lblLastYearTarget"));
                    lblLastYearTargetRep.Text = salesManTarget["LastYearTarget"].ToString();
                    Label lblAvg5YearsRep = ((Label)e.Item.FindControl("lblAvg5Years"));
                    lblAvg5YearsRep.Text = salesManTarget["AvgLast5Years"].ToString();
                    TextBox tbxNewTargetRep = ((TextBox)e.Item.FindControl("tbxNewTarget"));
                    tbxNewTargetRep.Text = salesManTarget["NewTarget"].ToString();
                    if (chbxEqual.Checked || !chbxSalesManRep.Checked)
                    {
                        tbxNewTargetRep.ReadOnly = true;
                    }
                    else
                    {
                        tbxNewTargetRep.ReadOnly = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkTotalTargetDist())
                {
                    distributeTarget();
                    ScriptManager.RegisterStartupScript(Page, typeof(Page),
                        "TargetDistributed", "alert('Target is distributed Successfully');"
                        + "window.location.href='/Sales/SalesHome.aspx';", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page),
                            "TargetDistTotalSum", "alert('Target distributions doesnot sum up to total target of " + TargetAmount + " !');", true);
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private void distributeTarget()
        {
            try
            {
                int targetYear = int.Parse(ddlTargetYear.SelectedItem.Text);
                DataTable dt = VSSalesDist;
                foreach (DataRow dr in dt.Rows)
                {
                    //branchUserTarget.Where.Active.Value = 1;
                    SalesBranchUserTarget branchUserTarget = new SalesBranchUserTarget();
                    branchUserTarget.Where.UserID.Value = (long)dr["UserID"];
                    branchUserTarget.Where.TargetID.Value = TargetID;
                    branchUserTarget.Where.BranchID.Value = BranchID;
                    branchUserTarget.Query.Load();
                    //if ((bool)dr["tobeSaved"])
                    {
                        DateTime now = DateTime.Now;
                        string notifyTitle = " Target Redistributed ";
                        string notifyDesc = " is now redistributed and your target is ";
                        if (branchUserTarget.RowCount == 0)
                        {
                            notifyTitle = " Target distributed ";
                            notifyDesc = " is now distributed and your target is ";
                            branchUserTarget.AddNew();
                            branchUserTarget.CreationDate = now;
                            branchUserTarget.CreatedBy = UserID;
                        }
                        branchUserTarget.Active = true;
                        branchUserTarget.TargetID = TargetID;
                        branchUserTarget.BranchID = BranchID;
                        branchUserTarget.UserID = (long)dr["UserID"];
                        double newTarget = (double)dr["NewTarget"];
                        branchUserTarget.Percentage = newTarget * 100.0 / TargetAmount;
                        branchUserTarget.Amount = (decimal)newTarget;
                        branchUserTarget.Modified = now;
                        branchUserTarget.ModifiedBy = UserID;
                        branchUserTarget.CurrencyID = TargetCurrencyID;
                        branchUserTarget.Save();
                        branchUserTarget.AcceptChanges();
                        Common.sendNotifications((long)dr["UserID"], targetYear + notifyTitle + "(" + newTarget + ")",
                            targetYear + notifyDesc + newTarget + "\nNote:\n" + txtAreaComment.Text, "");
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private bool targetAlreadyExists()
        {
            try
            {
                SalesBranchUserTarget branchUserTarget = new SalesBranchUserTarget();
                branchUserTarget.Where.Active.Value = true;
                branchUserTarget.Where.TargetID.Value = TargetID;
                branchUserTarget.Where.BranchID.Value = BranchID;
                branchUserTarget.Query.Load();
                return branchUserTarget.RowCount > 0;
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return false;
        }

        private Dictionary<long, double> getThisTargetsDistribution()
        {
            Dictionary<long, double> userTarget = new Dictionary<long, double>();
            try
            {
                SalesBranchUserTarget branchUserTarget = new SalesBranchUserTarget();
                branchUserTarget.Where.Active.Value = true;
                branchUserTarget.Where.TargetID.Value = TargetID;
                branchUserTarget.Where.BranchID.Value = BranchID;
                branchUserTarget.Query.Load();
                if (branchUserTarget.RowCount > 0)
                {
                    branchUserTarget.Rewind();
                    do
                    {
                        userTarget.Add(branchUserTarget.UserID, (double)branchUserTarget.Amount);
                    } while (branchUserTarget.MoveNext());
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return userTarget;
        }

        private bool checkTotalTargetDist()
        {
            try
            {
                DataTable dt = VSSalesDist;
                double totalDistributed = 0.0;
                foreach (DataRow dr in dt.Rows)
                {
                    if ((bool)dr["tobeSaved"])
                    {
                        totalDistributed += (double)dr["NewTarget"];
                    }
                }
                if (Math.Abs(totalDistributed - TargetAmount) <= 0.0000000001)
                    return true;
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
            return false;
        }

        protected void chbxSalesMan_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chbxSalesManRep = ((CheckBox)sender);
                int itemIndx = int.Parse(chbxSalesManRep.InputAttributes["itemIndx"]);
                DataTable dt = VSSalesDist;
                DataRow dr = dt.Rows[itemIndx];
                dr["tobeSaved"] = chbxSalesManRep.Checked;
                dr["NewTarget"] = chbxSalesManRep.Checked ? dr["NewTarget"] : 0;
                dt.AcceptChanges();
                VSSalesDist = dt;
                if (chbxEqual.Checked)
                {
                    equatTobeSavedMen();
                }
                rebindSalesMenRptr();
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void ddlTargetYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TargetID = int.Parse(ddlTargetYear.SelectedValue);
                LoadTargetDist();
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void tbxNewTarget_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox textBoxTarget = sender as TextBox;
                if (textBoxTarget != null)
                {
                    int changedIndex = ((RepeaterItem)textBoxTarget.NamingContainer).ItemIndex;
                    DataTable dt = VSSalesDist;
                    DataRow curntDR = dt.Rows[changedIndex];
                    double amount = 0;
                    double.TryParse(textBoxTarget.Text, out amount);
                    curntDR["NewTarget"] = amount;
                    VSSalesDist = dt;

                    dt = VSSalesDist;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        double usersDistTarget = 0;
                        int alreadyChanged = 0;
                        for (int i = 0; i <= changedIndex; i++)
                        {
                            if ((bool)dt.Rows[i]["tobeSaved"])
                            {
                                usersDistTarget += (double)dt.Rows[i]["NewTarget"];
                                alreadyChanged++;
                            }
                        }
                        int tobeSavedCount = dt.Select("tobeSaved = true").Length - alreadyChanged;
                        double remainingTargetPerUser = (TargetAmount - usersDistTarget) / tobeSavedCount;
                        for (int i = changedIndex + 1; i < dt.Rows.Count; i++)
                        {
                            if ((bool)dt.Rows[i]["tobeSaved"])
                            {
                                dt.Rows[i]["NewTarget"] = remainingTargetPerUser;
                            }
                            else
                            {
                                dt.Rows[i]["NewTarget"] = 0;
                            }
                        }
                        dt.AcceptChanges();
                        VSSalesDist = dt;
                    }
                    rebindSalesMenRptr();

                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void btnRedistributeTarget_Click(object sender, EventArgs e)
        {
            try
            {
                chbxEqual.Checked = true;
                chbxVar.Checked = false;
                chbxEqual_CheckedChanged(null, null);
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }
    }
}