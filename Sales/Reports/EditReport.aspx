<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GarasERP.Master" AutoEventWireup="true" CodeBehind="EditReport.aspx.cs" Inherits="GarasSales.Sales.Reports.NewDailyReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <meta charset="utf-8">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" integrity="sha384-mzrmE5qonljUremFsqc01SB46JvROS7bZs3IO2EmfFsd15uHvIt+Y8vEf7N7fWAU"
            crossorigin="anonymous">
    <link rel="stylesheet" href="../../UI/node_modules/bootstrap/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="../../UI/CSS/style.css">
    <title></title>
    <script type = "text/javascript">
    function ClientItemSelected(sender, e) {
        $get("<%=HDN_ClientID.ClientID %>").value = e.get_value();
        
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
     <div class="container mt-5 mb-5 reportsScreen">
      <div class="row upperCard">
        <div class="col">
          <div class="outerDiv">
            <div class="innerDiv pb-5">

              <div class="container">
                <h3 class="primaryBlue robotoMed">Daily Sales Reports
                    <asp:Label ID="LBL_TitleDate" runat="server" Text=""></asp:Label>
                </h3>
                 
                            
                        
                
                        
                          

                       <div class="row my-4">
                            <asp:Label ID="LBL_MSG" Visible="false" ForeColor="Red" runat="server" Text=""></asp:Label>
                        </div>
                           <%--<button class="btn text-white robotoMed createBtn primaryColorBlue" data-toggle="modal" data-target=".bd-example-modal-lg">Create New Report Line</button>--%>
                           <%--<div class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">--%>
                          <div >
                                <!-- POPUP MODEL -->
                                <%--<div class="modal-dialog modal-lg modal-dialog-centered">--%>
                              <div>
                                    <div class="modal-content">
                                           <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                                            <ContentTemplate>--%>
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="exampleModalCenterTitle">New Report Line</h5>
                                           <%-- <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span>
                                            </button>--%>
                                        </div>
                                        <div class="modal-body">
                                            <div class="container-fluid">
                                                <div class="row">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">Old/New</p>
                                                    </div>
                                                    <div class="col-md-4 text-left">
                                                        <div class="dropdown ">
                                                            <asp:DropDownList ID="DDL_New" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_New_SelectedIndexChanged" >
                                                                <asp:ListItem Text="Old" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="New" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="HDN_LineID" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">Client Name</p>
                                                    </div>
                                                    <div class="col-md-9 text-left">
                                                        
                                                        <%--<asp:DropDownList ID="DDL_ClientName" CssClass="form-control reportInputs" runat="server"  ></asp:DropDownList>--%>
                                                         <asp:TextBox ID="TXT_OldClientName" runat="server" CssClass="greyInput px-5 clinetInfoInput" ></asp:TextBox>
                                                          <AjaxToolkit:AutoCompleteExtender ServiceMethod="SearchCients" MinimumPrefixLength="2"
CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" TargetControlID="TXT_OldClientName" ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" OnClientItemSelected = "ClientItemSelected">

                                                          </AjaxToolkit:AutoCompleteExtender>  
                                                        <asp:RequiredFieldValidator ID="RFV_ClientName" ValidationGroup="MasrofGA" ControlToValidate="TXT_OldClientName" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="HDN_ClientID" runat="server" /> 
                                                       
                                                        <asp:TextBox ID="TXT_NewClientName" runat="server" CssClass="greyInput px-5 clinetInfoInput" Visible="false"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RFV_NewClientName"  ValidationGroup="MasrofGA" ControlToValidate="TXT_NewClientName" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" Enabled="false" ></asp:RequiredFieldValidator>
                                                       
                                                    </div>
                                                </div>

                                                <div class="row" runat="server" visible="false" id="Div_Addres">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">Client Address</p>
                                                    </div>
                                                    <div class="col-md-9 text-left">
                                                        
                                                        <asp:TextBox ID="TXT_NewAddress" runat="server" CssClass="greyInput px-5 clinetInfoInput" Visible="false"></asp:TextBox></br>
                                                    </div>
                                                </div>
                                                <div runat="server" visible="false" id="Div_Tel" class="row">
                                                    <div class="col-md-3 text-left">
                                                        
                                                        <p class="robotoReg">Client Phone</p>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <%--<input type="text" class="greyInput w-100 px-3 emailInput" placeholder="Phone">--%>
                                                        <asp:TextBox ID="TXT_Tel" runat="server" CssClass="greyInput w-100 px-3 emailInput" placeholder="Phone" Visible="false"></asp:TextBox></br>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">Contact Person Name</p>
                                                    </div>
                                                    <div class="col-md-9 text-left">
                                                        <%--<input type="text" class="greyInput px-5 clinetInfoInput">--%>
                                                        <asp:TextBox ID="TXT_ContactPerson" runat="server" CssClass="greyInput px-5 clinetInfoInput"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RFV_ContactPerson" ValidationGroup="MasrofGA" ControlToValidate="TXT_ContactPerson" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" Enabled="false" ></asp:RequiredFieldValidator></br>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">Contact Person Phone</p>
                                                    </div>
                                                    <div class="col-md-3 text-left">
                                                        <%--<input type="text" class="greyInput w-100 px-3 emailInput" placeholder="Phone">--%>
                                                         <asp:TextBox ID="TXT_ContactPersonTel" runat="server" CssClass="greyInput w-100 px-3 emailInput"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RFV_ContactPersonTel" ValidationGroup="MasrofGA" ControlToValidate="TXT_ContactPersonTel" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" Enabled="false" ></asp:RequiredFieldValidator></br>
                                     
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">Through</p>
                                                    </div>
                                                    <div class="col-md-4 text-left">
                                                        <div class="dropdown ">
                                                            <asp:DropDownList ID="DDL_Through" CssClass="form-control reportInputs" runat="server"  ></asp:DropDownList>
                       
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="MasrofGA" ControlToValidate="DDL_Through" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ></asp:RequiredFieldValidator>
                                                           
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">From</p>
                                                    </div>
                                                    <div class="col-md-5 text-left">
                                                        
                                                         <asp:TextBox ID="TXT_From" runat="server" CssClass="greyInput px-5 clinetInfoInput"></asp:TextBox>
                                                        <%-- <asp:RequiredFieldValidator ID="RFV_From" ForeColpx="Red" ValidationGroup="MasrofGA" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ControlToValidate="TXT_From"></asp:RequiredFieldValidator>--%>
                                                         <asp:CompareValidator ID="CV_From" ForeColor="Red" ValidationGroup="MasrofGA" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ControlToValidate="TXT_From" Operator="DataTypeCheck" Type="Double" ></asp:CompareValidator>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">To</p>
                                                    </div>
                                                    <div class="col-md-5 text-left">
                                                        
                                                         <asp:TextBox ID="TXT_To" runat="server"  CssClass="greyInput px-5 clinetInfoInput " ></asp:TextBox>
                                                         <%--<asp:RequiredFieldValidator ID="RFV_To" ForeColor="Red" ValidationGroup="MasrofGA" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ControlToValidate="TXT_To"></asp:RequiredFieldValidator>--%>
                                                         <asp:CompareValidator ID="CV_To" ForeColor="Red" ValidationGroup="MasrofGA" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ControlToValidate="TXT_To" Operator="DataTypeCheck" Type="Double" ></asp:CompareValidator>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">Location</p>
                                                    </div>
                                                    <div class="col-md-9 text-left">
                                                        
                                                         <asp:TextBox ID="TXT_Location" runat="server" CssClass="greyInput px-5 clinetInfoInput"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RFV_Location" ForeColor="Red" ValidationGroup="MasrofGA" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ControlToValidate="TXT_Location"></asp:RequiredFieldValidator>
                                    
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">Reason</p>
                                                    </div>
                                                    <div class="col-md-9 text-left">
                                                        
                                                        <asp:TextBox ID="TXT_Reason" TextMode="MultiLine" Rows="3" runat="server"  CssClass="greyInput px-5 clinetInfoInput w-100" ></asp:TextBox>
                                                    </div>
                                                </div>

                                           <%--  </div>
                                        </div>--%>

                                        
                                       <%-- </ContentTemplate>
                                         </asp:UpdatePanel>--%>
                                       <%-- <div>
                                            <div>--%>
                                                <div runat="server" id="Div_Attachment" visible="false" class="row mt-2">
                                                    <div class="col-md-3 text-left">
                                                        <p class="robotoReg">Attachment</p>
                                                    </div>
                                                    <div class="col-md-5 text-left ml-2">
                                                        <asp:FileUpload ID="FU_File" Visible="false" runat="server" />
                                                        <%--<AjaxToolkit:AjaxFileUpload ID="FU_File1" MaximumNumberOfFiles="1" OnUploadStart="FU_File_UploadStart" OnUploadComplete="FU_File_UploadComplete" Mode="Auto" runat="server" />--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="BTN_NewLine" CssClass="btn text-white save px-5 py-1 robotoBold primaryColorBlue" runat="server" Text="Add New Line" ValidationGroup="MasrofGA" OnClick="BTN_NewLine_Click" />
                                             <asp:Button ID="BTN_EditLine" Visible="false" CssClass="btn text-white save px-5 py-1 robotoBold primaryColorBlue" runat="server" Text="Edit Line" ValidationGroup="MasrofGA" OnClick="BTN_EditLine_Click" />
                                             <asp:Button ID="BTN_Cancel" Visible="false" CssClass="btn text-white save px-5 py-1 robotoBold primaryColorBlue" runat="server" Text="Cancel Edit" CausesValidation="false" OnClick="BTN_Cancel_Click" />
                                        </div>

                                    </div>
                                </div>
                            </div>  
                   <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                      <ContentTemplate>
                        <div runat="server" ID="DIV_Content" visible="false">
                            <%--  <asp:GridView ID="GV_Reports" HeaderStyle-CssClass="row primaryBlue robotoMed text-center" RowStyle-CssClass="row robotoMed text-center my-3" GridLines="None" ShowFooter="true" runat="server"
                        AutoGenerateColumns="false" OnPageIndexChanging="GV_Reports_PageIndexChanging"  AllowSorting="false" AllowFiltering="false" 
                     Width="100%"
                    FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'" EnableViewState="true" 
                        CellPadding="10" CellSpacing="0" BorderWidth="0"  AllowPaging="false"  EmptyDataText="there is no reports to display"  PagerSettings-Mode="NumericFirstLast" PagerStyle-HorizontalAlign="Left"
                                  OnRowDeleting="GV_Reports_RowDeleting" OnRowCommand="GV_Reports_RowCommand" OnRowDataBound="GV_Reports_RowDataBound" >
                        <HeaderStyle CssClass="row primaryBlue robotoMed text-center"  />
                        <RowStyle CssClass="row robotoMed text-center my-3"  />
                        <FooterStyle CssClass="row robotoMed text-center my-3"  />
                        <Columns>
                             <asp:TemplateField HeaderText="ID" SortExpression="ID" FooterStyle-CssClass="col-md-1" ItemStyle-CssClass="col-md-1"  HeaderStyle-CssClass="col-md-1" Visible="false" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_ID"  runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                </ItemTemplate>
                                  <FooterTemplate>
                                    <asp:Label ID="LBL_ID" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="New" FooterStyle-CssClass="col-md-1" ItemStyle-CssClass="col-md-1"  HeaderStyle-CssClass="col-md-1" >
                                <ItemTemplate>
                                   <asp:Label ID="LBL_No" runat="server" Text='<%#Container.DataItemIndex + 1%>'></asp:Label>
                                     <asp:Label ID="Label1" runat="server" Text='<%# Eval("NewText") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="DDL_New" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DDL_New_SelectedIndexChanged" >
                                        <asp:ListItem Text="Old" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="New" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Client" FooterStyle-CssClass="col-md-2" ItemStyle-CssClass="col-md-2"  HeaderStyle-CssClass="col-md-2" >
                                 <ItemTemplate>
                                    <asp:Label ID="LBL_ClientName" runat="server" Text='<%# Eval("ClientName") %>'></asp:Label></br>
                                  
                                    <asp:Label ID="LBL_NewClientTel" runat="server" Text='<%# Eval("NewClientTel") %>' Visible='<%# Eval("New") %>'></asp:Label></br>
                                       <asp:Label ID="LBL_NewClientAddress" runat="server" Text='<%# Eval("NewClientAddress") %>' Visible='<%# Eval("New") %>'></asp:Label>
                                      <asp:Label ID="LBL_CobractPerson" runat="server" Text='<%# Eval("ContactPerson") %>' ></asp:Label></br>
                                       <asp:Label ID="LBL_ContactPersonTel" runat="server" Text='<%# Eval("ContactPersonMobile") %>'></asp:Label>
                                     
                                     <asp:HyperLink ID="lnkAttachment" Text='<%# Eval("FileName") %>' NavigateUrl='<%# Eval("FilePath") %>' ToolTip="Click to Preview File" runat="server" Visible='<%# ( ( Eval("ID").ToString()!="-1") && (bool)Eval("New") )?true:false%>' Target="_blank"></asp:HyperLink>
                                     <asp:Label ID="LBL_File" runat="server" Text='<%# Eval("FileName") %>' Visible='<%# ( ( Eval("ID").ToString()=="-1") && (bool)Eval("New") )?true:false%>'></asp:Label>
                               </ItemTemplate>
                                 <FooterTemplate>
                                    <asp:DropDownList ID="DDL_ClientName" CssClass="form-control reportInputs" runat="server"  ></asp:DropDownList>
                       
                                    <asp:RequiredFieldValidator ID="RFV_ClientName" ValidationGroup="MasrofGA" ControlToValidate="DDL_ClientName" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ></asp:RequiredFieldValidator>
                                      <asp:Label ID="LBL_ContactPerson" runat="server" Text="Contact Person :" ></asp:Label>
                                    <asp:TextBox ID="TXT_ContactPerson" runat="server" Width="40px" CssClass="reportInputs mt-0 pt-0"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RFV_ContactPerson" ValidationGroup="MasrofGA" ControlToValidate="TXT_ContactPerson" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" Enabled="false" ></asp:RequiredFieldValidator></br>
                                      <asp:Label ID="LBL_ContactPersonTel" runat="server" Text="Contact Person Mobile :" ></asp:Label>
                                    <asp:TextBox ID="TXT_ContactPersonTel" runat="server" Width="40px" CssClass="reportInputs mt-0 pt-0"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RFV_ContactPersonTel" ValidationGroup="MasrofGA" ControlToValidate="TXT_ContactPersonTel" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" Enabled="false" ></asp:RequiredFieldValidator></br>
                                     
                                     
                                     <asp:Label ID="LBL_NewClientName" runat="server" Text="Client Name :" Visible="false"></asp:Label>
                                    <asp:TextBox ID="TXT_NewClientName" runat="server" Width="40px" CssClass="reportInputs mt-0 pt-0" Visible="false"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RFV_NewClientName" ValidationGroup="MasrofGA" ControlToValidate="TXT_NewClientName" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" Enabled="false" ></asp:RequiredFieldValidator></br>
                                      <asp:Label ID="LBL_NewTel" runat="server" Text=" Tel :" Visible="false"></asp:Label>
                                    <asp:TextBox ID="TXT_Tel" runat="server" Width="40px" CssClass="reportInputs mt-0 pt-0" Visible="false"></asp:TextBox></br>
                                     <asp:Label ID="LBL_NewAddress" runat="server" Text="Address :" Visible="false"></asp:Label>
                                    <asp:TextBox ID="TXT_NewAddress" runat="server" Width="40px" CssClass="reportInputs mt-0 pt-0" Visible="false"></asp:TextBox></br>
                                     <asp:FileUpload ID="FU_File" Visible="false" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Through" FooterStyle-CssClass="col-md-2" ItemStyle-CssClass="col-md-2" HeaderStyle-CssClass="col-md-2" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_Through" runat="server" Text='<%# Eval("ThroughName") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                    <asp:DropDownList ID="DDL_Through" CssClass="form-control reportInputs" runat="server"  ></asp:DropDownList>
                       
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="MasrofGA" ControlToValidate="DDL_Through" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="From" FooterStyle-CssClass="col-md-1" ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="col-md-1" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_From" runat="server" Text='<%# Eval("From") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                  
                                     <asp:TextBox ID="TXT_From" runat="server" Width="40px" CssClass="reportInputs mt-0 pt-0"></asp:TextBox>
                                 
                                     <asp:CompareValidator ID="CV_From" ForeColor="Red" ValidationGroup="MasrofGA" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ControlToValidate="TXT_From" Operator="DataTypeCheck" Type="Double" ></asp:CompareValidator>
                                </FooterTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="To" FooterStyle-CssClass="col-md-1" ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="col-md-1" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_To" runat="server" Text='<%# Eval("To") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                  
                                     <asp:TextBox ID="TXT_To" runat="server" Width="40px"  CssClass="reportInputs " ></asp:TextBox>
                                     
                                     <asp:CompareValidator ID="CV_To" ForeColor="Red" ValidationGroup="MasrofGA" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ControlToValidate="TXT_To" Operator="DataTypeCheck" Type="Double" ></asp:CompareValidator>
                                </FooterTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Location" FooterStyle-CssClass="col-md-2" ItemStyle-CssClass="col-md-2" HeaderStyle-CssClass="col-md-2" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_Location" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                  
                                     <asp:TextBox ID="TXT_Location" runat="server" Width="150px" CssClass="reportInputs"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RFV_Location" ForeColor="Red" ValidationGroup="MasrofGA" runat="server" ErrorMessage="*" Display="Dynamic" SetFocusOnError="true" ControlToValidate="TXT_Location"></asp:RequiredFieldValidator>
                                    
                                </FooterTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Reason" FooterStyle-CssClass="col-md-2" ItemStyle-CssClass="col-md-2" HeaderStyle-CssClass="col-md-2" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_Reason" runat="server" Text='<%# Eval("Reason") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                  
                                     <asp:TextBox ID="TXT_Reason" TextMode="MultiLine" Rows="3" runat="server"  CssClass="reportInputs" ></asp:TextBox>
                                    
                                    
                                </FooterTemplate>
                            </asp:TemplateField>

                             
                              <asp:TemplateField HeaderText="Action"  FooterStyle-CssClass="col-md-1" ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="col-md-1" >
                               
                               
                               
                                <ItemTemplate>
                                    <asp:ImageButton ID="BTN_Delete"  runat="server" ToolTip="Delete" CommandName="Delete" CausesValidation="false" OnClientClick="return confirm('Are Yoou Sure ?');" ImageUrl="~/UI/Images/icons/delete.gif" />
                                    
                                </ItemTemplate>
                                 <FooterTemplate>
                                     <asp:ImageButton ID="BTN_Add" Width="25px" Height="25px" runat="server" ToolTip="Add" CommandName="Insert" CausesValidation="true" ValidationGroup="MasrofGA" ImageUrl="~/UI/Images/plus-symbol.png" />
                                    
                                   
                                </FooterTemplate>
                            </asp:TemplateField>
                            



                   
                        </Columns>
                    </asp:GridView>--%>


                            <asp:Repeater ID="RPT_Line" runat="server">
                                <HeaderTemplate>
                                    <div class="Report my-4">
                                    <div class="row">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="col-lg-12">

                                            <div class="d-flex justify-content-end mt-2">
                                               <%-- <button type="button" class="btn bg-transparent" data-toggle="modal" data-target="#exampleModal">
                                                    <i class="fas fa-edit primaryBlue fa-lg"></i>
                                                </button>--%>
                                               <%-- <asp:ImageButton ID="btn_Edit" CommandArgument='<%#Container.ItemIndex %>' OnCommand="btn_Edit_Command" Width="50px" Height="40px" CssClass="btn bg-transparent" ImageUrl="~/UI/Images/edit.png" runat="server" />--%>
                                                
                                              
                                                <%--<button type="button" class="btn bg-transparent">
                                                    <i class="fas fa-trash-alt text-danger fa-lg"></i>
                                                </button>--%>
                                                <asp:ImageButton ID="BTN_Delete" CommandArgument='<%#Container.ItemIndex %>' OnCommand="BTN_Delete_Command" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this line');" Width="50px" Height="40px" CssClass="btn bg-transparent" ImageUrl="~/UI/Images/icons/delete.gif" runat="server" />
                                            </div>
                                        
                                        <asp:HiddenField ID="HDN_ID" runat="server" Value='<%# Eval("ID") %>' />
                                            <h4><%# Eval("ClientName") %>
                                                <span class='<%# Eval("NewClass") %>'><%# Eval("NewText") %></span>
                                            </h4>
                                            <h6 class="d-inline-block robotoBold">Clinet Address : </h6>
                                            <p class="m-0 d-inline-block"><%# Eval("NewClientAddress") %></p>
                                            <h6 class="d-inline-block ml-5 robotoBold">Clinet Phone : </h6>
                                            <p class="m-0 d-inline-block"><%# Eval("NewClientTel") %></p>
                                            <h6 class="d-inline-block ml-5 robotoBold">Contact Person Name : </h6>
                                            <p class="m-0 d-inline-block"><%# Eval("ContactPerson") %></p>
                                            <br>
                                            <h6 class="d-inline-block robotoBold">Contact Person Mobile : </h6>
                                            <p class="m-0 d-inline-block"><%# Eval("ContactPersonMobile") %></p>

                                            <h6 class="d-inline-block ml-5 robotoBold">From : </h6>
                                            <p class="m-0 d-inline-block "><%# Eval("From") %></p>
                                            <span class="mx-3">-</span>
                                            <h6 class="d-inline-block robotoBold">To : </h6>
                                            <p class="m-0 d-inline-block"><%# Eval("To") %></p>
                                            <h6 class="d-inline-block ml-5 robotoBold">Location : </h6>
                                            <p class="m-0 d-inline-block"><%# Eval("Location") %></p>
                                            <br>
                                            <h6 class="d-inline-block ml-5 robotoBold">Through : </h6>
                                            <p class="m-0 d-inline-block"><%# Eval("ThroughName") %></p>
                                            <br>
                                            <h6 class="d-inline-block robotoBold">Reason : </h6>
                                            <p class="m-0 d-inline-block"><%# Eval("Reason") %></p>

                                        <br>
                                        <h6 class="d-inline-block robotoBold">Attachment : </h6>
                                        <p>
                                        <asp:HyperLink ID="lnkAttachment" Text='<%# Eval("FileName") %>' NavigateUrl='<%# Eval("FilePath") %>' ToolTip="Click to Preview File" runat="server" Visible='<%# ( ( Eval("ID").ToString()!="-1") && (bool)Eval("New") )?true:false%>' Target="_blank"></asp:HyperLink>
                                         <asp:Label ID="LBL_File" runat="server" Text='<%# Eval("FileName") %>' Visible='<%# ( ( Eval("ID").ToString()=="-1") && (bool)Eval("New") )?true:false%>'></asp:Label>
                                            </p>
                                            <!-- <div class="d-flex justify-content-end mt-4">
                                                <button type="button" class="btn primaryColorBlue text-white robotoMed createBtn px-4 mr-2 py-1">Edit</button>
                                                <button type="button" class="btn btn-danger robotoMed createBtn px-3 py-1">Delete</button>
                                            </div> -->
                                            <hr>

                                        </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                      </div>
                                </div>
                                </FooterTemplate>
                            </asp:Repeater>

                            <div class="row my-4">
				             <div class="col-md-4 offset-md-8 text-right">
                   
                                 <asp:Button CssClass="btn text-white save px-5 py-1 robotoBold primaryColorBlue" ID="BTN_Save" runat="server" Text="Save" OnClick="BTN_Save_Click" />
                                 <asp:Button CssClass="btn text-white save px-5 py-1 robotoBold primaryColorBlue" ID="btn_Finish" runat="server" Text="Finish" OnClick="btn_Finish_Click" />
                              </div>
                            </div>


                        </div>

                    
                    </ContentTemplate>
                       <Triggers>
                           <asp:PostBackTrigger ControlID="BTN_NewLine" />
                       </Triggers>
                      
                </asp:UpdatePanel>


                   

              </div>

            </div>
          </div>
        </div>
      </div>
      </div>

     <script src="../../UI/node_modules/jquery/dist/jquery.min.js"></script>
    <script src="../../UI/node_modules/bootstrap/dist/js/bootstrap.bundle.js"></script>

</asp:Content>
