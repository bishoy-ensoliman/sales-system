using GarasERP;
using MyGeneration.dOOdads;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarasSales.Sales
{
    public partial class SalesHome : System.Web.UI.Page
    {

        private static string photosPath = "/UI/Images/";
        private static readonly int TEAM_VIEW_COUNT = 5;
        private static readonly int TASK_VIEW_COUNT = 5;

        static string key = "SalesGarasPass";
        #region Session Variables Region
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

        private int UserDeptID
        {
            get
            {
                if (ViewState["UserDeptID"] == null)
                {
                    return -1;
                }
                return (int)ViewState["UserDeptID"];

            }
            set
            {
                ViewState["UserDeptID"] = value;
            }
        }

        private List<long> TeamIDs
        {
            get
            {
                if (ViewState["TeamIDs"] == null)
                {
                    return new List<long>();
                }
                return (List<long>)ViewState["TeamIDs"];

            }
            set
            {
                ViewState["TeamIDs"] = value;
            }
        }
        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {
            LoadDeptID();
            LoadTeam();
            LoadClientCount();
            LoadTasks();
            LoadTasksDelayCount();
            LoadTasksTodayCount();
            LoadTasksFutureCount();
            LoadOffersCount();
            LoadMyTarget();
            LoadReportsCount();
            LoadReportsTodayCount();
            btnNewClient.Visible = false;
            if (canAddClient())
            {
                btnNewClient.Visible = true;
            }
        }

        private void LoadReportsCount()
        {
            DailyReport dailyReport = new DailyReport();
            dailyReport.Where.UserID.Value = UserID;
            dailyReport.Where.Status.Value = "Not Filled";
            dailyReport.Query.Load();
            lblReportsCount.Text = dailyReport.RowCount.ToString();
        }

        private void LoadReportsTodayCount()
        {
            DailyReport dailyReport = new DailyReport();
            dailyReport.Where.UserID.Value = UserID;
            dailyReport.Where.Status.Value = "Not Filled";
            dailyReport.Where.ReprotDate.Value = DateTime.Now.Date;
            dailyReport.Query.Load();
            lbltodayReportCount.Text = dailyReport.RowCount.ToString();
        }

        private bool canAddClient()
        {
            return Common.CheckUserInRole(UserID, 1);
        }

        private string getUserGroup()
        {
            string groupName = "";
            Group_User gpUser = new Group_User();
            gpUser.Where.UserID.Value = UserID;
            gpUser.Where.Active.Value = true;
            gpUser.Query.AddOrderBy(Group_User.ColumnNames.GroupID, WhereParameter.Dir.ASC);
            gpUser.Query.Load();
            if(gpUser.RowCount > 0)
            {
                gpUser.Rewind();
                Group gp = new Group();
                if (gp.LoadByPrimaryKey(gpUser.GroupID))
                {
                    if (gp.Active)
                    {
                        groupName = gp.Name.Trim();
                    }
                }
            }
            return groupName;
        }

        private int getUserBranchID()
        {
            int branchID = -1;
            try
            {
                User me = new User();
                if (me.LoadByPrimaryKey(UserID))
                {
                    if (me.BranchID != null)
                    {
                        branchID = me.BranchID;
                    }
                }
            }catch(Exception ex){}
            return branchID;
        }

        private void LoadMyTarget()
        {
            //string userGroup = getUserGroup();
            decimal targetAmount = 0;
            SalesTarget salesTarget = new SalesTarget();
            salesTarget.Where.Active.Value = true;
            salesTarget.Where.Year.Value = DateTime.Now.Year;
            salesTarget.Query.Load();
            string curShortName = "";
            if (salesTarget.RowCount > 0)
            {
                Currency currency = new Currency();
                currency.LoadByPrimaryKey(salesTarget.CurrencyID);
                curShortName = currency.ShortName;
                List<string> gp = new List<string>();
                gp.Add("TopManagment");
                if(Common.CheckUserInGroups(UserID, gp) )
                {
                    targetAmount = salesTarget.Target;                            
                }
                else
                {
                    int myBranchID = getUserBranchID();
                    gp = new List<string>();
                    gp.Add("SalesManagers");
                    if (Common.CheckUserInGroups(UserID, gp))
                    {
                        SalesBranchTarget branchTarget = new SalesBranchTarget();
                        branchTarget.Where.Active.Value = true;
                        branchTarget.Where.BranchID.Value = myBranchID;
                        branchTarget.Where.TargetID.Value = salesTarget.ID;
                        branchTarget.Query.Load();
                        if (branchTarget.RowCount > 0)
                        {
                            targetAmount = branchTarget.Amount;
                        }
                    }
                    else
                    {
                        gp = new List<string>();
                        gp.Add("SalesMen");
                        if (Common.CheckUserInGroups(UserID, gp))
                        {
                            SalesBranchUserTarget userTarget = new SalesBranchUserTarget();
                            userTarget.Where.Active.Value = true;
                            userTarget.Where.BranchID.Value = myBranchID;
                            userTarget.Where.TargetID.Value = salesTarget.ID;
                            userTarget.Where.UserID.Value = UserID;
                            userTarget.Query.Load();
                            if (userTarget.RowCount > 0)
                            {
                                targetAmount = userTarget.Amount;
                            }
                        }
                    }
                }                
            }
            lblmyTarget.Text = ((double)targetAmount).ToString() + " " +curShortName;
        }

        //private void LoadOffersCount()
        //{
        //    SalesOffer salesOffer = new SalesOffer();
        //    salesOffer.Where.Active.Value = 1;
        //    salesOffer.Where.SalesPersonID.Value = UserID;
        //    salesOffer.Query.Load();
        //    lblOffersCount.Text = salesOffer.RowCount.ToString();
        //}

        protected void LoadOffersCount()
        {
            bool CanViewAllOffersAllBranches = GarasERP.Common.CheckUserInRole(UserID, 8);
            bool CanViewAllOffersMyBranch = GarasERP.Common.CheckUserInRole(UserID, 9);
            int offersCount = 0;
            if (CanViewAllOffersAllBranches)
            {
                offersCount = LoadOffersCountAllBranches();
            }
            else if (CanViewAllOffersMyBranch)
            {
                offersCount = LoadOffersCountMyBranches();
            }
            else
            {
                offersCount = LoadMyOffers();
            }
            lblOffersCount.Text = offersCount.ToString();
        }

        private int LoadOffersCountMyBranches()
        {
            int myBranchID = Common.GetUserBranchID(UserID);
            GarasERP.V_SalesOffer_User offer = new GarasERP.V_SalesOffer_User();
            //offer.Where.Status.Value = "Pricing";
            offer.Where.BranchID.Value = myBranchID;
            offer.Query.Load();
            return offer.RowCount;
        }

        private int LoadOffersCountAllBranches()
        {
            GarasERP.SalesOffer offer = new GarasERP.SalesOffer();
            offer.Query.Load();
            return offer.RowCount;
        }

        private int LoadMyOffers()
        {
            SalesOffer offer = new SalesOffer();
            offer.Where.SalesPersonID.Value = UserID;
            offer.Query.Load();
            return offer.RowCount;
        }

        private void LoadClientCount()
        {
            GarasERP.Client clients = new GarasERP.Client();
            clients.Where.SalesPersonID.Value = UserID;
            clients.Query.Load();
            lblClientCount.Text = clients.RowCount.ToString();
        }

        private void LoadTasksDelayCount()
        {
            string taskIDs = getMyTaskIDs();
            if (taskIDs != "")
            {
                Task userTasks = new Task();
                userTasks.Where.Active.Value = 1;
                userTasks.Where.ID.Value = taskIDs;
                userTasks.Where.ID.Operator = WhereParameter.Operand.In;
                userTasks.Where.ExpireDate.Value = DateTime.Now.Date;
                userTasks.Where.ExpireDate.Operator = WhereParameter.Operand.LessThan;
                // Order by modified date desc   
                //userTasks.Query.AddOrderBy(Task.ColumnNames.ModifiedDate, WhereParameter.Dir.DESC);
                userTasks.Query.Load();
                lblDelayCount.Text = userTasks.RowCount.ToString();
            }
            else
            {
                lblDelayCount.Text = "0";
            }
        }

        private void LoadTasksFutureCount()
        {
            string taskIDs = getMyTaskIDs();
            if (taskIDs != "")
            {
                Task userTasks = new Task();
                userTasks.Where.Active.Value = 1;
                //List<long> meWzTeam = TeamIDs;
                //meWzTeam.Add(UserID);
                userTasks.Where.ID.Value = taskIDs;
                userTasks.Where.ID.Operator = WhereParameter.Operand.In;
                //userTasks.Where.TaskUser.Value = UserID;
                userTasks.Where.ExpireDate.Value = DateTime.Now.Date;
                userTasks.Where.ExpireDate.Operator = WhereParameter.Operand.GreaterThan;
                // Order by modified date desc   
                //userTasks.Query.AddOrderBy(Task.ColumnNames.ModifiedDate, WhereParameter.Dir.DESC);
                userTasks.Query.Load();
                lblMonitoring.Text = userTasks.RowCount.ToString();
            }
            else
            {
                lblMonitoring.Text = "0";
            }
        }

        private string getMyTaskIDs()
        {
            string taskIDs = "";
            Dictionary<long, bool> taskIDDict = new Dictionary<long, bool>();
            TaskPermission taskPerm = new TaskPermission();
            taskPerm.Where.UserGroupID.Value = UserID;
            taskPerm.Where.IsGroup.Value = false;

            if (taskPerm.Query.Load())
            {
                if (taskPerm.DefaultView != null && taskPerm.DefaultView.Count > 0)
                {
                    do
                    {
                        if (taskIDs == "")
                        {
                            if (!taskIDDict.ContainsKey(taskPerm.TaskID))
                            {
                                taskIDDict.Add(taskPerm.TaskID, true);
                                taskIDs = "'" + taskPerm.TaskID + "'";
                            }
                        }
                        else
                        {
                            if (!taskIDDict.ContainsKey(taskPerm.TaskID))
                            {
                                taskIDDict.Add(taskPerm.TaskID, true);
                                taskIDs += ",'" + taskPerm.TaskID + "'";
                            }
                        }

                    } while (taskPerm.MoveNext());
                }
            }
            string gpIDs = "";
            Group_User gpUser = new Group_User();
            gpUser.Where.Active.Value = true;
            gpUser.Where.UserID.Value = UserID;
            if (gpUser.Query.Load())
            {
                if (gpUser.DefaultView != null && gpUser.DefaultView.Count > 0)
                {
                    do
                    {
                        if (gpIDs == "")
                        {
                            gpIDs = "'" + gpUser.GroupID + "'";
                        }
                        else
                        {
                            gpIDs += ",'" + gpUser.GroupID + "'";
                        }

                    } while (gpUser.MoveNext());
                }
            }
            if (gpIDs != "")
            {
                TaskPermission gpTasks = new TaskPermission();
                gpTasks.Where.UserGroupID.Value = gpIDs;
                gpTasks.Where.UserGroupID.Operator = WhereParameter.Operand.In;
                gpTasks.Where.IsGroup.Value = true;


                if (gpTasks.Query.Load())
                {
                    if (gpTasks.DefaultView != null && gpTasks.DefaultView.Count > 0)
                    {
                        do
                        {
                            if (taskIDs == "")
                            {
                                if (!taskIDDict.ContainsKey(gpTasks.TaskID))
                                {
                                    taskIDDict.Add(gpTasks.TaskID, true);
                                    taskIDs = "'" + gpTasks.TaskID + "'";
                                }
                            }
                            else
                            {
                                if (!taskIDDict.ContainsKey(gpTasks.TaskID))
                                {
                                    taskIDDict.Add(gpTasks.TaskID, true);
                                    taskIDs += ",'" + gpTasks.TaskID + "'";
                                }
                            }

                        } while (gpTasks.MoveNext());
                    }
                }
            }
            return taskIDs;
        }

        private void LoadTasksTodayCount()
        {
            string taskIDs = getMyTaskIDs();
            if (taskIDs != "")
            {
                Task userTasks = new Task();
                userTasks.Where.Active.Value = 1;
                userTasks.Where.ID.Value = taskIDs;
                userTasks.Where.ID.Operator = WhereParameter.Operand.In;
                userTasks.Where.ExpireDate.BetweenBeginValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                userTasks.Where.ExpireDate.BetweenEndValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                userTasks.Where.ExpireDate.Operator = WhereParameter.Operand.Between;
                userTasks.Query.Load();
                lblTodayCount.Text = userTasks.RowCount.ToString();
            }
            else
            {
                lblTodayCount.Text = "0";
            }
        }

        private void LoadDeptID()
        {
            GarasERP.User user = new GarasERP.User();
            if (user.LoadByPrimaryKey(UserID))
            {
                try
                {
                    UserDeptID = user.DepartmentID;
                }
                catch (Exception ex)
                {
                    UserDeptID = -1;
                }
            }
        }
        #region Tasks
        private void LoadTasks()
        {
            string taskIDs = getMyTaskIDs();
            if (taskIDs != "")
            {
                Task userTasks = new Task();
                userTasks.Where.Active.Value = 1;
                userTasks.Where.ID.Value = taskIDs;
                userTasks.Where.ID.Operator = WhereParameter.Operand.In;
                // Order by modified date desc   
                userTasks.Query.AddOrderBy(Task.ColumnNames.CreationDate, WhereParameter.Dir.DESC);
                userTasks.Query.Load();
                lblTasksCount.Text = userTasks.RowCount == 1 ? "1 task" : userTasks.RowCount + " tasks";
                rptrTasks.DataSource = userTasks.DefaultView.Table.Rows.Cast<System.Data.DataRow>().Take(TASK_VIEW_COUNT).CopyToDataTable().DefaultView;
                rptrTasks.DataBind();
            }
            else
            {
                rptrTasks.DataSource = null;
                rptrTasks.DataBind();
            }
        }

        protected void rptrTasks_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView task = ((DataRowView)e.Item.DataItem);
                    Label lbltaskNameRep = ((Label)e.Item.FindControl("lbltaskName"));
                    HyperLink lnkTaskRep = ((HyperLink)e.Item.FindControl("lnkTask"));
                    lbltaskNameRep.Text = task["Name"].ToString();
                    lnkTaskRep.NavigateUrl = getTaskLinkEnc(task["ID"].ToString());
                    lnkTaskRep.Target = "_blank";
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private string getTaskLinkEnc(string taskID)
        {
            string link = "";
            Task task = new Task();
            task.LoadByPrimaryKey(long.Parse(taskID));
            if (task.RowCount > 0 && task.s_TaskPage != "" && task.s_RefrenceNumber != "")
            {
                link = task.TaskPage+task.RefrenceNumber;
            }
            else
            {
                string url = "/Tasks/TaskDetails.aspx?Task=";
                string queryStr = Encrypt_Decrypt.Encrypt(taskID, UserID.ToString());
                link = url + Server.UrlEncode(queryStr);
            }
            return link;
        }

        #endregion

        #region Team
        private void LoadTeam()
        {
            try
            {
                GarasERP.User team = new GarasERP.User();
                team.Where.DepartmentID.Value = UserDeptID;
                team.Where.ID.Value = UserID;
                team.Where.ID.Operator = WhereParameter.Operand.NotEqual;
                // Order by creation date asc   
                team.Query.AddOrderBy(GarasERP.User.ColumnNames.CreationDate, WhereParameter.Dir.ASC);
                team.Query.Load();
                lblTeamCount.Text = team.RowCount == 1 ? "1 member" : team.RowCount + " members";
                if (team.RowCount > 0)
                {
                    List<long> myTeam = new List<long>();
                    team.Rewind(); //move to first record
                    do
                    {
                        myTeam.Add(team.ID);
                    } while (team.MoveNext());
                    TeamIDs = myTeam;

                    rptrMyTeam.DataSource = team.DefaultView
                                            .Cast<DataRowView>()
                                            .Take(TEAM_VIEW_COUNT).Select(r => r.Row);
                    rptrMyTeam.DataBind();
                }
                else
                {
                    rptrMyTeam.DataSource = null;
                    rptrMyTeam.DataBind();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        protected void rptrMyTeam_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRow teamMate = ((DataRow)e.Item.DataItem);
                    HyperLink lnkTeamMateRep = ((HyperLink)e.Item.FindControl("lnkTeamMate"));
                    Image imgTeamMatePhotoRep = ((Image)e.Item.FindControl("imgTeamMatePhoto"));
                    Label lblTeamMateNameRep = ((Label)e.Item.FindControl("lblTeamMateName"));
                    lnkTeamMateRep.NavigateUrl = "/Employees/EmployeeDetails.aspx?Employee=" + 
                                                 Encrypt_Decrypt.Encrypt(teamMate["ID"].ToString(),key);
                    imgTeamMatePhotoRep.ImageUrl = getTeamMatePhoto(teamMate["ID"].ToString());
                    imgTeamMatePhotoRep.ToolTip = teamMate["FirstName"].ToString() + " " + teamMate["LastName"].ToString();
                    lblTeamMateNameRep.Text = teamMate["FirstName"].ToString();
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }

        private string getTeamMatePhoto(string teamMateID)
        {
            User user = new User();
            string userPhotoPath = photosPath + "male.png";
            if (user.LoadByPrimaryKey(int.Parse(teamMateID)))
            {
                //userPhotoPath = photosPath + "photo_" + teamMateID + ".jpg";//assume jpg format
                try
                {
                    if (user.Photo != null && user.Photo.Length != 0)
                    {
                        //File.WriteAllBytes(Server.MapPath(userPhotoPath), user.Photo);
                        string base64String = Convert.ToBase64String(user.Photo);
                        userPhotoPath = "data:image/jpg;base64," + base64String;
                    }
                    else if (user.Gender == "Female")
                        userPhotoPath = photosPath + "female.png";
                    else
                        userPhotoPath = photosPath + "male.png";
                }
                catch (Exception ex)
                {
                    if (user.Gender == "Female")
                        userPhotoPath = photosPath + "female.png";
                    else
                        userPhotoPath = photosPath + "male.png";
                }
            }
            return userPhotoPath;
        }


        #endregion

        protected void btnDelay_Click(object sender, EventArgs e)
        {
            string taskIDs = getMyTaskIDs();
            if (taskIDs != "")
            {
                Task userTasks = new Task();
                userTasks.Where.Active.Value = 1;
                userTasks.Where.ID.Value = taskIDs;
                userTasks.Where.ID.Operator = WhereParameter.Operand.In;
                userTasks.Where.ExpireDate.Value = DateTime.Now.Date;
                userTasks.Where.ExpireDate.Operator = WhereParameter.Operand.LessThan;
                // Order by modified date desc   
                userTasks.Query.AddOrderBy(Task.ColumnNames.CreationDate, WhereParameter.Dir.DESC);
                userTasks.Query.Top = TASK_VIEW_COUNT;
                userTasks.Query.Load();
                //lblTasksCount.Text ="Delay: "+( userTasks.RowCount == 1 ? "1 task" : userTasks.RowCount + " tasks");
                rptrTasks.DataSource = userTasks.DefaultView;
                rptrTasks.DataBind();
            }
            else
            {
                rptrTasks.DataSource = null;
                rptrTasks.DataBind();
            }
        }

        protected void btnToday_Click(object sender, EventArgs e)
        {
            string taskIDs = getMyTaskIDs();
            if (taskIDs != "")
            {
                Task userTasks = new Task();
                userTasks.Where.Active.Value = 1;
                userTasks.Where.ID.Value = taskIDs;
                userTasks.Where.ID.Operator = WhereParameter.Operand.In;
                userTasks.Where.ExpireDate.BetweenBeginValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                userTasks.Where.ExpireDate.BetweenEndValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                userTasks.Where.ExpireDate.Operator = WhereParameter.Operand.Between;
                // Order by modified date desc   
                userTasks.Query.AddOrderBy(Task.ColumnNames.CreationDate, WhereParameter.Dir.DESC);
                userTasks.Query.Top = TASK_VIEW_COUNT;
                userTasks.Query.Load();
                //lblTasksCount.Text ="Today: "+( userTasks.RowCount == 1 ? "1 task" : userTasks.RowCount + " tasks");
                rptrTasks.DataSource = userTasks.DefaultView;
                rptrTasks.DataBind();
            }
            else
            {
                rptrTasks.DataSource = null;
                rptrTasks.DataBind();
            }
        }

        protected void btnMonitor_Click(object sender, EventArgs e)
        {
            string taskIDs = getMyTaskIDs();
            if (taskIDs != "")
            {
                Task userTasks = new Task();
                userTasks.Where.Active.Value = 1;
                userTasks.Where.ID.Value = taskIDs;
                userTasks.Where.ID.Operator = WhereParameter.Operand.In;
                userTasks.Where.ExpireDate.Value = DateTime.Now.Date;
                userTasks.Where.ExpireDate.Operator = WhereParameter.Operand.GreaterThan;
                // Order by modified date desc   
                userTasks.Query.AddOrderBy(Task.ColumnNames.CreationDate, WhereParameter.Dir.DESC);
                userTasks.Query.Top = TASK_VIEW_COUNT;
                userTasks.Query.Load();
                //lblTasksCount.Text = userTasks.RowCount == 1 ? "1 task" : userTasks.RowCount + " tasks";
                rptrTasks.DataSource = userTasks.DefaultView;
                rptrTasks.DataBind();
            }
            else
            {
                rptrTasks.DataSource = null;
                rptrTasks.DataBind();
            }
        }
    }
}