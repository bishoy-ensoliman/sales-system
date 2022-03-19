using GarasERP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GarasSales.Sales
{
    public partial class ClientProfile1 : System.Web.UI.Page
    {
        protected String myType="";
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
                if(!Page.IsPostBack)
                {
                    if (Page.Request.QueryString.Count > 0 && Page.Request.QueryString["CID"] != null)
                    {
                        string ID = Encrypt_Decrypt.Decrypt(Page.Request.QueryString["CID"].ToString(), key);
                        long CID = 0;
                        if (long.TryParse(ID, out CID) && CID > 0)
                        {
                            int permisionLevel = 0;
                            if (Common.CheckUserInRole(UserID, 5))
                                permisionLevel = 1; // view all clients from all branches
                            else
                            {
                                if (Common.CheckUserInRole(UserID, 6))
                                    permisionLevel = 2; // view all clients from my branche
                                else
                                {

                                    List<string> groups = new List<string>();
                                    groups.Add("SalesMen");
                                    if (Common.CheckUserInGroups(UserID, groups))
                                    {
                                        permisionLevel = 3; // view my clients
                                    }
                                }
                            }
                            if (permisionLevel > 0)
                            {
                                int BranchID = Common.GetUserBranchID(UserID);
                                V_Client_Useer client = new V_Client_Useer();
                                client.Where.ID.Value = CID;
                                if (permisionLevel == 2)
                                    client.Where.BranchID.Value = BranchID;
                                if (permisionLevel == 3)
                                    client.Where.SalesPersonID.Value = UserID;
                                if (client.Query.Load())
                                {
                                    if (client.DefaultView != null && client.DefaultView.Count > 0)
                                    {
                                        DIV_Content.Visible = true;
                                        Get_Client_Details(client);

                                        if (Common.CheckUserInRole(UserID, 1) && client.BranchID == BranchID)
                                            LBTN_Edit.Visible = true;
                                        else
                                            LBTN_Edit.Visible = false;
                                        //GetTextboxes(int.Parse(myID));
                                        LoadClientOrders(client.ID);
                                        LoadClientVolume(client.ID);
                                        LoadClientOpenOffer(client.ID);
                                        if ( client.SalesPersonID==UserID && Common.CheckUserInRole(UserID, 7) )//AddNewOfferRole
                                            btnCreateNewOffer.Visible = true;
                                        if(client.s_SupportedByCompany !="")
                                        {
                                            if(client.SupportedByCompany)
                                            {
                                                CHB_SupportedByCompany.Checked = true;
                                                TXT_SupportedBy.Visible = true;
                                                TXT_SupportedBy.Text = client.SupportedBy;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LBL_MSG.Visible = true;
                                        LBL_MSG.Text = "Please Select a correct Client";
                                    }
                                }
                                else
                                {
                                    LBL_MSG.Visible = true;
                                    LBL_MSG.Text = "Please Select a correct Client";
                                }
                            }
                            else
                            {
                                LBL_MSG.Visible = true;
                                LBL_MSG.Text = "You Don't have permission to view this client";
                            }
                        }
                        else
                        {
                            LBL_MSG.Visible = true;
                            LBL_MSG.Text = "Please Select a correct Client";
                        }
                    }
                    else
                    {
                        LBL_MSG.Visible = true;
                        LBL_MSG.Text = "Please Select a correct Client";
                    }
                }
                //String myString = HttpContext.Current.Request.Url.PathAndQuery;
                //myString = myString.Substring(myString.IndexOf('?') + 1);

               // String myID = getTaskLinkEnc(myString);
              //  Get_Client_Details(int.Parse(myID));

                //ShowBoxesBasedOnType(myType);
                //GetTextboxes(int.Parse(myID));
            }
            catch(Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "Page_Load:" + ex.Message;
            }

        }

        private void LoadClientOrders(long ClientID)
        {
            try
            {
                SalesOffer offer = new SalesOffer();
                offer.Where.ClientID.Value = ClientID;
                offer.Where.ClientApprove.Value = true;
                offer.Where.Status.Value = "Closed";
                offer.Aggregate.ID.Function = MyGeneration.dOOdads.AggregateParameter.Func.Count;
                if(offer.Query.Load())
                {
                    if (offer.DefaultView != null && offer.DefaultView.Count > 0)
                    {
                        LBL_ClientOrders.Text = offer.s_ID;
                    }
                    
                }
                
            }
            catch(Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "LoadClientOrders:" + ex.Message;
            }
        }

        private void LoadClientVolume(long ClientID)
        {
            try
            {
                SalesOffer offer = new SalesOffer();
                offer.Where.ClientID.Value = ClientID;
                offer.Where.ClientApprove.Value = true;
                offer.Where.Status.Value = "Closed";
                offer.Aggregate.OfferAmount.Function = MyGeneration.dOOdads.AggregateParameter.Func.Sum;
                if (offer.Query.Load())
                {
                    if (offer.DefaultView != null && offer.DefaultView.Count > 0)
                        LBL_clientBusinessVolume.Text = offer.s_OfferAmount;

                }

            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "LoadClientOrders:" + ex.Message;
            }
        }


        private void LoadClientOpenOffer(long ClientID)
        {
            try
            {
                SalesOffer offer = new SalesOffer();
                offer.Where.ClientID.Value = ClientID;
               
                offer.Where.Status.Value = "Closed";
                offer.Where.Status.Operator = MyGeneration.dOOdads.WhereParameter.Operand.NotEqual;
                offer.Aggregate.ID.Function = MyGeneration.dOOdads.AggregateParameter.Func.Count;
                if (offer.Query.Load())
                {
                    if (offer.DefaultView != null && offer.DefaultView.Count > 0)
                        LBL_clientOpenOffers.Text = offer.s_ID;

                }

            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "LoadClientOpenOffer:" + ex.Message;
            }
        }

        private void Get_Client_Details(V_Client_Useer client)
        {

            try {
                if (client.Logo != null)
                {
                    var img = client.Logo;
                    string base64String = Convert.ToBase64String((byte[])img);
                    IMG_ClientImage.ImageUrl = "data:image/jpg;base64," + base64String;
                }
                else
                {
                    IMG_ClientImage.ImageUrl = "~/UI/Images/male.png";
                }

                myType = client.Type;
                //DataView dt = client.DefaultView;
                LBL_ClientName.Text = client.Name.ToString().Trim();
                if (!string.IsNullOrEmpty(client.s_FirstContractDate))
                    LBL_ClientDuration.Text = client.FirstContractDate.ToString("yyyy");

                this.Page.Title = client.Name.ToString() + "'s Profile";
                LBL_ClientType.Text = myType;
                LBL_SalesBersonName.Text = client.FirstName + " " + client.LastName;
                if (client.Photo != null)
                {
                    var img = client.Photo;
                    string base64String = Convert.ToBase64String((byte[])img);
                    IMG_SalesManImage.ImageUrl = "data:image/jpg;base64," + base64String;
                }
                else
                {
                    IMG_SalesManImage.ImageUrl = "~/UI/Images/male.png";
                }

                switch (myType)
                {
                    case "Small Company (One Branch)":
                        {
                            lblCompanyName.Visible = true;
                            tbxCompanyName.Visible = true;
                            tbxCompanyName.Text = client.Name;

                            lblGroupName.Visible = false;
                            tbxGroupName.Visible = false;

                            lblBranch.Visible = false;
                            tbxBranch.Visible = false;
                        }
                        break;
                    case "Big Company (Multiple Branches)":
                        {
                            lblCompanyName.Visible = true;
                            tbxCompanyName.Visible = true;
                            tbxCompanyName.Text = client.Name;
                            lblBranch.Visible = true;
                            tbxBranch.Visible = true;
                            tbxBranch.Text = client.BranchName;
                            lblGroupName.Visible = true;
                            tbxGroupName.Visible = true;
                        }
                        break;

                    case "Group of Companies":
                        {
                            lblCompanyName.Visible = true;
                            tbxCompanyName.Visible = true;
                            tbxCompanyName.Text = client.Name;
                            lblBranch.Visible = true;
                            tbxBranch.Visible = true;
                            tbxBranch.Text = client.BranchName;

                            lblGroupName.Visible = true;
                            tbxGroupName.Visible = true;
                            tbxGroupName.Text = client.GroupName;
                        }
                        break;

                    case "Individual":
                        {
                            lblCompanyName.Visible = true;
                            tbxCompanyName.Visible = true;
                            tbxCompanyName.Text = client.Name;
                            lblGroupName.Visible = false;
                            tbxGroupName.Visible = false;

                            lblBranch.Visible = false;
                            tbxBranch.Visible = false;
                        }
                        break;
                }

                LoadClientAddresses(client.ID);
                TXT_Email.Text = client.Email;
                TXT_Website.Text = client.WebSite;
                loadClientPhones(client.ID);
                loadClientMobile(client.ID);
                loadClientFAX(client.ID);
                LoadClientContacts(client.ID);
                LoadClientSpeciality(client.ID);
                if(client.ConsultantType!="")
                    LBL_ConsultanatType.Text = client.ConsultantType;
                if (LBL_ConsultanatType.Text != "N/A")
                    LoadConsaltant(client.ID);
                LBL_FollowUpPeriod.Text = client.s_FollowUpPeriod;
                LoadAttachment(client.ID);
                tbxNote.Text = client.Note;
                
            }
            catch(Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "Get_Client:" + ex.Message;
            }

        }

        private void LoadAttachment(long iD)
        {
           try
            {
                DataTable FilesDT = new System.Data.DataTable();
                FilesDT.Columns.Add("FileName");
                FilesDT.Columns.Add("AttachmentPath");
                FilesDT.Columns.Add("Category");

                ClientAttachment attach = new ClientAttachment();
                attach.Where.ClientID.Value = iD;
                attach.Query.AddOrderBy(ClientAttachment.ColumnNames.Type, MyGeneration.dOOdads.WhereParameter.Dir.ASC);
                if(attach.Query.Load())
                {
                   
                    do
                    {
                        DataRow dr = FilesDT.NewRow();
                        dr["FileName"] = attach.FileName;
                        dr["AttachmentPath"] = attach.AttachmentPath;
                        dr["Category"] = attach.Type;
                       


                        FilesDT.Rows.Add(dr);
                    }
                    while (attach.MoveNext());

                    AttachmentsBC.BindAttachments(FilesDT.DefaultView);
                }
               
            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "LoadAttachment:" + ex.Message;
            }
        }

        private void LoadConsaltant(long iD)
        {
            try
            {
                DataTable Consulatant = new System.Data.DataTable();
                Consulatant.Columns.Add("ID");
                Consulatant.Columns.Add("HasCompany",typeof(bool));
                Consulatant.Columns.Add("Company");
                Consulatant.Columns.Add("ConsultantName");
                Consulatant.Columns.Add("ConsultantFor");
               

                ClientConsultant Const = new ClientConsultant();
                Const.Where.ClientID.Value = iD;
                Const.Where.Active.Value = true;
                if (Const.Query.Load())
                {
                    do
                    {
                        DataRow dr = Consulatant.NewRow();
                        dr["ID"] = Const.ID;
                        dr["Company"] = Const.Company;
                        dr["ConsultantName"] = Const.ConsultantName;
                        dr["ConsultantFor"] = Const.ConsultantFor;
                       
                        if(Const.s_Company!="")
                            dr["HasCompany"] = true;
                        else
                            dr["HasCompany"] = false;


                        Consulatant.Rows.Add(dr);
                    }
                    while (Const.MoveNext());
                }

                RPT_Consultant.DataSource = Consulatant;
                RPT_Consultant.DataBind();





            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "LoadConsaltant:" + ex.Message;
            }
        }

        private void LoadClientSpeciality(long iD)
        {
            try
            {
                V_ClientSpeciality specialty = new V_ClientSpeciality();
                specialty.Where.ClientID.Value = iD;
                specialty.Where.Active.Value = true;
                if (specialty.Query.Load())
                {
                    RPT_ContactSpciality.DataSource = specialty.DefaultView;
                    RPT_ContactSpciality.DataBind();
                }
            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "LoadClientSpeciality:" + ex.Message;
            }
        }

        private void LoadClientContacts(long iD)
        {
            try
            {
                ClientContactPerson contact = new ClientContactPerson();
                contact.Where.ClientID.Value = iD;
                contact.Where.Active.Value = true;
                if (contact.Query.Load())
                {
                    RPT_ContactPerson.DataSource = contact.DefaultView;
                    RPT_ContactPerson.DataBind();
                }
            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "LoadClientContacts:" + ex.Message;
            }
        }

        private void loadClientFAX(long iD)
        {
            try
            {
                ClientFax fax = new ClientFax();
                fax.Where.ClientID.Value = iD;
                fax.Where.Active.Value = true;
                if (fax.Query.Load())
                {
                    RPT_ContactFFAX.DataSource = fax.DefaultView;
                    RPT_ContactFFAX.DataBind();
                }
            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "loadClientFAX:" + ex.Message;
            }
        }

        private void loadClientMobile(long iD)
        {
            try
            {
                ClientMobile mobile = new ClientMobile();
                mobile.Where.ClientID.Value = iD;
                mobile.Where.Active.Value = true;
                if (mobile.Query.Load())
                {
                    RPT_ContactMobile.DataSource = mobile.DefaultView;
                    RPT_ContactMobile.DataBind();
                }
            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "loadClientMobile:" + ex.Message;
            }
        }

        private void loadClientPhones(long iD)
        {
            try
            {
                ClientPhone phone = new ClientPhone();
                phone.Where.ClientID.Value = iD;
                phone.Where.Active.Value = true;
                if(phone.Query.Load())
                {
                    RPT_ContactPhone.DataSource = phone.DefaultView;
                    RPT_ContactPhone.DataBind();
                }
            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "loadClientPhones:" + ex.Message;
            }
        }

        private void LoadClientAddresses(long iD)
        {
            try
            {
                DataTable AddrssDT = new System.Data.DataTable();
                AddrssDT.Columns.Add("ID");
                AddrssDT.Columns.Add("Country");
                AddrssDT.Columns.Add("Governate");
                AddrssDT.Columns.Add("Street");
                AddrssDT.Columns.Add("Bulding");
                AddrssDT.Columns.Add("Floor");
                AddrssDT.Columns.Add("Desc");
                AddrssDT.Columns.Add("Area");

                V_ClientAddress Address = new V_ClientAddress();
                Address.Where.ClientID.Value = iD;
                Address.Where.Active.Value = true;
                if (Address.Query.Load())
                {
                    do
                    {
                        DataRow dr = AddrssDT.NewRow();
                        dr["ID"] = Address.ID;
                        dr["Country"] = Address.Country;
                        dr["Governate"] = Address.Governorate;
                        dr["Street"] = Address.Address;
                        dr["Bulding"] = Address.BuildingNumber;
                        dr["Floor"] = Address.Floor;
                        dr["Desc"] = Address.Description;
                        dr["Area"] = Address.Area;

                        AddrssDT.Rows.Add(dr);
                    }
                    while (Address.MoveNext());
                }
               
            RPT_Address.DataSource = AddrssDT;
            RPT_Address.DataBind();

                


              
            }
            catch (Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "LoadClientAddresses:" + ex.Message;
            }
        }


        //protected void BindAllDDLs()
        //{
        //    BindCountryDDL();
        //    BindGovDDL();
        //    BindSpeciality();
        //    BindFollowUpPeriod();
        //    BindSalesMen();
        //}
        //protected void BindSalesMen()
        //{
        //    GarasERP.User user = new GarasERP.User();
        //    user.Where.ID.Value = 1;
        //    user.Where.ID.Operator = MyGeneration.dOOdads.WhereParameter.Operand.NotEqual;
        //    user.Query.Load();

        //    ddlAssignedTo.DataSource = user.DefaultView;
        //    ddlAssignedTo.DataTextField = GarasERP.User.ColumnNames.FirstName;
        //    ddlAssignedTo.DataValueField = GarasERP.User.ColumnNames.ID;
        //    ddlAssignedTo.DataBind();


        //}
        //protected void BindFollowUpPeriod()
        //{
        //    ddlFollowUpPeriod.DataSource = Enumerable.Range(1, 12);
        //    ddlFollowUpPeriod.DataBind();
        //}
        //protected void BindSpeciality()
        //{
        //    GarasERP.Speciality sp = new GarasERP.Speciality();
        //    sp.LoadAll();

        //    ddlSpeciality.DataSource = sp.DefaultView;
        //    ddlSpeciality.DataTextField = GarasERP.Country.ColumnNames.Name;
        //    ddlSpeciality.DataValueField = GarasERP.Country.ColumnNames.ID;
        //    ddlSpeciality.DataBind();
        //}
        //private void Get_Client_Details(long clientID)
        //{
        //    GarasERP.Client client = new GarasERP.Client();
        //    client.Where.ID.Value = clientID;
        //    client.Query.Load();
        //    myType = client.Type;
        //    //DataView dt = client.DefaultView;
        //    clientName.Text = client.Name.ToString();
        //    clientDuration.Text = client.CreationDate.ToString("yyyy");
        //    if (client.Logo != null)
        //    {
        //        var img = client.Logo;
        //        string base64String = Convert.ToBase64String((byte[])img);
        //        clientImage.ImageUrl = "data:image/jpg;base64," + base64String;
        //    }
        //    else
        //    {
        //        clientImage.ImageUrl = "~/UI/Images/male.png";
        //    }

        //    this.Page.Title = client.Name.ToString() + "'s Profile";


        //}
        private string getTaskLinkEnc(string clientID)
        {
            string queryStr = GarasERP.Encrypt_Decrypt.Decrypt(Server.UrlDecode(clientID), key);
            return queryStr;
        }

        //protected void lnkBtnEdit_Command(object sender, CommandEventArgs e)
        //{
        //    //if(lnkBtnEdit.CommandName == "Edit")
        //    //{
        //    //    //pnlClientInfo.Enabled = true;
        //    //    lnkBtnEdit.CommandName = "Cancel";
        //    //    lnkBtnEdit.Text = "Cancel";
        //    //}
        //    //else if(lnkBtnEdit.CommandName == "Cancel")
        //    //{
        //    //    //pnlClientInfo.Enabled = false;
        //    //    lnkBtnEdit.CommandName = "Edit";
        //    //    lnkBtnEdit.Text = "Edit";
        //    //}
        //}

        //protected void ShowBoxesBasedOnType(string myType)
        //{
        //    if (myType.Contains("Individual"))
        //    { 
        //        rbIndividual.Checked = true;
        //        rbSmall.Checked = false;
        //        rbBig.Checked = false;
        //        rbCompanies.Checked = false;
        //    }
        //    else if (myType.Contains("Group")) {
        //        rbIndividual.Checked = false;
        //        rbSmall.Checked = false;
        //        rbBig.Checked = false;
        //        rbCompanies.Checked = true;
        //    }
        //    else if(myType.Contains("Small"))
        //    {
        //        rbIndividual.Checked = false;
        //        rbSmall.Checked = true;
        //        rbBig.Checked = false;
        //        rbCompanies.Checked = false;
        //    }
        //    else if(myType.Contains("Big"))
        //    {
        //        rbIndividual.Checked = false;
        //        rbSmall.Checked = false;
        //        rbBig.Checked = true;
        //        rbCompanies.Checked = false;
        //    }
           
        //}
        //protected void LoadNameBasedOnType()
        //{
            
        //    //if (rbBig.Checked == true)
        //    //{
        //    //    lblCompanyName.Visible = true;
        //    //    tbxCompanyName.Visible = true;

        //    //    lblBranch.Visible = true;
        //    //    tbxBranch.Visible = true;

        //    //    lblGroupName.Visible = false;
        //    //    tbxGroupName.Visible = false;

        //    //    //selectedRadio = rbBig.Text;
        //    //    //selectedName = tbxCompanyName.Text;
        //    //}

        //    //else if (rbCompanies.Checked == true)
        //    //{
        //    //    lblGroupName.Visible = true;
        //    //    tbxGroupName.Visible = true;

        //    //    lblCompanyName.Visible = true;
        //    //    tbxCompanyName.Visible = true;

        //    //    lblBranch.Visible = false;
        //    //    tbxBranch.Visible = false;

        //    //    //selectedRadio = rbCompanies.Text;
        //    //    //selectedName = tbxCompanyName.Text;
        //    //}
        //    //else if (rbSmall.Checked == true)
        //    //{
        //    //    lblCompanyName.Visible = true;
        //    //    tbxCompanyName.Visible = true;

        //    //    lblGroupName.Visible = false;
        //    //    tbxGroupName.Visible = false;

        //    //    lblBranch.Visible = false;
        //    //    tbxBranch.Visible = false;

        //    //    //selectedRadio = rbSmall.Text;
        //    //    //selectedName = tbxCompanyName.Text;
        //    //}
        //    //else if (rbIndividual.Checked == true)
        //    //{
        //    //    lblGroupName.Visible = true;
        //    //    tbxGroupName.Visible = true;

        //    //    lblCompanyName.Visible = false;
        //    //    tbxCompanyName.Visible = false;
        //    //    lblBranch.Visible = false;
        //    //    tbxBranch.Visible = false;

        //    //    //selectedRadio = rbIndividual.Text;
        //    //    //selectedName = tbxGroupName.Text;
        //    //}
        //    //else
        //    //{
        //    //    lblGroupName.Visible = false;
        //    //    tbxGroupName.Visible = false;
        //    //    lblCompanyName.Visible = false;
        //    //    tbxCompanyName.Visible = false;
        //    //    lblBranch.Visible = false;
        //    //    tbxBranch.Visible = false;

        //    //}
        //}

        protected void GetTextboxes(int ID)
        {
            
            GarasERP.Client client = new GarasERP.Client();
            client.Where.ID.Value = ID;
            client.Query.Load();
            GarasERP.ClientAddress clientAddress = new GarasERP.ClientAddress();
            clientAddress.Where.ClientID.Value = ID;
            clientAddress.Query.Load();
            GarasERP.ClientPhone clientPhone = new GarasERP.ClientPhone();
            clientPhone.Where.ClientID.Value = ID;
            clientPhone.Query.Load();
            GarasERP.ClientMobile clientMobile = new GarasERP.ClientMobile();
            clientMobile.Where.ClientID.Value = ID;
            clientMobile.Query.Load();

            GarasERP.ClientContactPerson contact = new GarasERP.ClientContactPerson();
            contact.Where.ClientID.Value = ID;
            contact.Query.Load();

            GarasERP.ClientConsultant consultant = new GarasERP.ClientConsultant();
            consultant.Where.ClientID.Value = ID;
            consultant.Query.Load();

            if (client.DefaultView != null && client.DefaultView.Count > 0)
            {
                //if (rbBig.Checked == true)
                //{
                //    tbxCompanyName.Text = client.Name;
                //    tbxBranch.Text = client.BranchName;
                //}
                //else if (rbSmall.Checked == true)
                //{
                //    tbxCompanyName.Text = client.Name;
                //}
                //else if (rbCompanies.Checked)
                //{
                //    tbxGroupName.Text = client.GroupName;
                //    tbxCompanyName.Text = client.Name;
                //}
                //else
                //    tbxCompanyName.Text = client.GroupName;

                //tbxEmail.Text = client.Email;
                //tbxWebsite.Text = client.WebSite;
                //ddlAssignedTo.SelectedValue = client.SalesPersonID.ToString();
                //ddlFollowUpPeriod.SelectedValue = client.FollowUpPeriod.ToString();
                //tbxNote.Text = client.Note;
            }

            //if(clientAddress.DefaultView != null && clientAddress.DefaultView.Count > 0)
            //{
            //    tbxStreet.Text = clientAddress.Address;
            //    tbxBuilding.Text = clientAddress.BuildingNumber;
            //    tbxFloor.Text = clientAddress.Floor;
            //    tbxDesc.Text = clientAddress.Description;
            //    ddlCountry.SelectedValue = clientAddress.CountryID.ToString();
            //    ddlGovernate.SelectedValue = clientAddress.GovernorateID.ToString();
            //}

            //if(clientPhone.DefaultView != null && clientPhone.DefaultView.Count > 0)
            //{
            //    tbxPhone.Text = clientPhone.Phone;
            //}

            //if(clientMobile.DefaultView != null && clientMobile.DefaultView.Count > 0)
            //{
            //    tbxMobile.Text = clientMobile.Mobile;
            //}
            //if(contact.DefaultView != null && contact.DefaultView.Count > 0)
            //{
            //    tbxContactName.Text = contact.Name;
            //    tbxContactEmail.Text = contact.Email;
            //    tbxContactLocation.Text = contact.Location;
            //    tbxContactMobile.Text = contact.Mobile;
            //    tbxContactTitle.Text = contact.Title;
            //}
            //if(consultant.DefaultView != null && consultant.DefaultView.Count >0)
            //{
            //    GarasERP.ClientConsultantAddress consultantAddress = new GarasERP.ClientConsultantAddress();
            //    consultantAddress.Where.ConsultantID.Value = consultant.ID;
            //    consultantAddress.Query.Load();

            //    tbxConsultantName.Text = consultant.ConsultantName;

            //    tbxConsultantBuilding.Text = consultantAddress.BuildingNumber;
            //    tbxConsultantFloor.Text = consultantAddress.Floor;
            //    tbxConsultantStreet.Text = consultantAddress.Address;
            //    tbxConsultantDescription.Text = consultantAddress.Description;
            //    ddlConsultantCountry.SelectedValue = consultantAddress.CountryID.ToString();
            //    ddlConsultantGovernorate.SelectedValue = consultantAddress.GovernorateID.ToString();
            //}
            
        }

        //protected void BindCountryDDL()
        //{
        //    GarasERP.Country country = new GarasERP.Country();
        //    country.LoadAll();

        //    ddlCountry.DataSource = country.DefaultView;
        //    ddlCountry.DataTextField = GarasERP.Country.ColumnNames.Name;
        //    ddlCountry.DataValueField = GarasERP.Country.ColumnNames.ID;
        //    ddlCountry.DataBind();

        //    ddlConsultantCountry.DataSource = country.DefaultView;
        //    ddlConsultantCountry.DataTextField = GarasERP.Country.ColumnNames.Name;
        //    ddlConsultantCountry.DataValueField = GarasERP.Country.ColumnNames.ID;
        //    ddlConsultantCountry.DataBind();
        //}
        //protected void BindGovDDL()
        //{
        //    GarasERP.Governorate governorate = new GarasERP.Governorate();
        //    //governorate.LoadAll();
        //    governorate.Where.CountryID.Value = ddlCountry.SelectedValue;
        //    governorate.Query.Load();

        //    ddlGovernate.DataSource = governorate.DefaultView;
        //    ddlGovernate.DataTextField = GarasERP.Governorate.ColumnNames.Name;
        //    ddlGovernate.DataValueField = GarasERP.Governorate.ColumnNames.ID;
        //    ddlGovernate.DataBind();


        //    GarasERP.Governorate consultantGovernorate = new GarasERP.Governorate();
        //    consultantGovernorate.Where.CountryID.Value = ddlConsultantCountry.SelectedValue;
        //    consultantGovernorate.Query.Load();

        //    ddlConsultantGovernorate.DataSource = consultantGovernorate.DefaultView;
        //    ddlConsultantGovernorate.DataTextField = GarasERP.Governorate.ColumnNames.Name;
        //    ddlConsultantGovernorate.DataValueField = GarasERP.Governorate.ColumnNames.ID;
        //    ddlConsultantGovernorate.DataBind();
        //}

        //protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindGovDDL();
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void LBTN_Edit_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("EditClient.aspx?CID=" + Server.UrlEncode(Page.Request.QueryString["CID"].ToString()), false);
        }

      

        protected void RPT_Consultant_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    long ID = long.Parse( ((HiddenField)e.Item.FindControl("HDN_ID")).Value);
                    ClientConsultantEmail email = new ClientConsultantEmail();
                    email.Where.ConsultantID.Value = ID;
                    email.Where.Active.Value = true;
                    if (email.Query.Load())
                    {
                        ((Repeater)e.Item.FindControl("RPT_ConsContactEmail")).DataSource = email.DefaultView;
                        ((Repeater)e.Item.FindControl("RPT_ConsContactEmail")).DataBind();
                    }

                    ClientConsultantPhone Phone = new ClientConsultantPhone();
                    Phone.Where.ConsultantID.Value = ID;
                    Phone.Where.Active.Value = true;
                    if (Phone.Query.Load())
                    {
                        ((Repeater)e.Item.FindControl("RPT_ConsContactPhone")).DataSource = Phone.DefaultView;
                        ((Repeater)e.Item.FindControl("RPT_ConsContactPhone")).DataBind();
                    }

                    ClientConsultantMobile mobile = new ClientConsultantMobile();
                    mobile.Where.ConsultantID.Value = ID;
                    mobile.Where.Active.Value = true;
                    if (mobile.Query.Load())
                    {
                        ((Repeater)e.Item.FindControl("RPT_ConsContactMobile")).DataSource = mobile.DefaultView;
                        ((Repeater)e.Item.FindControl("RPT_ConsContactMobile")).DataBind();
                    }

                    ClientConsultantFax fax = new ClientConsultantFax();
                    fax.Where.ConsultantID.Value = ID;
                    fax.Where.Active.Value = true;
                    if (fax.Query.Load())
                    {
                        ((Repeater)e.Item.FindControl("RPT_ConsContactFFAX")).DataSource = fax.DefaultView;
                        ((Repeater)e.Item.FindControl("RPT_ConsContactFFAX")).DataBind();
                    }

                    DataTable AddrssDT = new System.Data.DataTable();
                    AddrssDT.Columns.Add("ID");
                    AddrssDT.Columns.Add("Country");
                    AddrssDT.Columns.Add("Governate");
                    AddrssDT.Columns.Add("Street");
                    AddrssDT.Columns.Add("Bulding");
                    AddrssDT.Columns.Add("Floor");
                    AddrssDT.Columns.Add("Desc");

                    V_ClientConsultantAddress Address = new V_ClientConsultantAddress();
                    Address.Where.ConsultantID.Value = ID;
                    Address.Where.Active.Value = true;
                    if (Address.Query.Load())
                    {
                        do
                        {
                            DataRow dr = AddrssDT.NewRow();
                            dr["ID"] = Address.ID;
                            dr["Country"] = Address.Country;
                            dr["Governate"] = Address.Governorate;
                            dr["Street"] = Address.Address;
                            dr["Bulding"] = Address.BuildingNumber;
                            dr["Floor"] = Address.Floor;
                            dr["Desc"] = Address.Description;


                            AddrssDT.Rows.Add(dr);
                        }
                        while (Address.MoveNext());

                        ((Repeater)e.Item.FindControl("RPT_ConsaltsntAddress")).DataSource = AddrssDT;
                        ((Repeater)e.Item.FindControl("RPT_ConsaltsntAddress")).DataBind();
                    }
                   
                    


                }
            }
            catch(Exception ex)
            {
                LBL_MSG.Visible = true;
                LBL_MSG.Text = "RPT_Consultant_ItemDataBound:" + ex.Message;
            }
        }

        protected void btnCreateNewOffer_Click(object sender, EventArgs e)
        {
            if (Page.Request.QueryString.Count > 0 && Page.Request.QueryString["CID"] != null)
            {
                Page.Response.Redirect("../../Offers/NewOffer.aspx?CID=" + Server.UrlEncode( Page.Request.QueryString["CID"]), false);
            }
        }
    }
}