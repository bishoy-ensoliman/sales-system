<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GarasERP.Master" AutoEventWireup="true" CodeBehind="VarifyReport.aspx.cs" Inherits="GarasSales.Sales.Reports.VarifyReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <meta charset="utf-8">
    <link rel="stylesheet" href="../../UI/node_modules/bootstrap/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="../../UI/CSS/style.css">
    <title></title>
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
                  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                      <ContentTemplate>
                       <div class="row my-4">
                            <asp:Label ID="LBL_MSG" Visible="false" ForeColor="Red" runat="server" Text=""></asp:Label>
                        </div>
                             
                  
                        <div runat="server" ID="DIV_Content" visible="false">
                             <%-- <asp:GridView ID="GV_Reports" HeaderStyle-CssClass="row primaryBlue robotoMed text-center" RowStyle-CssClass="row robotoMed text-center my-3" GridLines="None" ShowFooter="true" runat="server"
                        AutoGenerateColumns="false"  AllowSorting="false" AllowFiltering="false" 
                     Width="100%"
                    FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'" EnableViewState="true" 
                        CellPadding="10" CellSpacing="0" BorderWidth="0" AllowPaging="false"  EmptyDataText="there is no reports to display"  PagerSettings-Mode="NumericFirstLast" PagerStyle-HorizontalAlign="Left"
                                    OnRowDataBound="GV_Reports_RowDataBound" >
                        <HeaderStyle CssClass="row primaryBlue robotoMed text-center"  />
                        <RowStyle CssClass="row robotoMed text-center my-3"  />
                        <FooterStyle CssClass="row robotoMed text-center my-3"  />
                        <Columns>
                             <asp:TemplateField HeaderText="ID" SortExpression="ID" FooterStyle-CssClass="col-md-1" ItemStyle-CssClass="col-md-1"  HeaderStyle-CssClass="col-md-1" Visible="false" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_ID"  runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                </ItemTemplate>
                                 
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="New" FooterStyle-CssClass="col-md-1" ItemStyle-CssClass="col-md-1"  HeaderStyle-CssClass="col-md-1" >
                                <ItemTemplate>
                                   
                                     <asp:Label ID="Label1" runat="server" Text='<%# Eval("NewText") %>'></asp:Label>
                                </ItemTemplate>
                               
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Client" FooterStyle-CssClass="col-md-2" ItemStyle-CssClass="col-md-2"  HeaderStyle-CssClass="col-md-2" >
                                 <ItemTemplate>
                                    <asp:Label ID="LBL_ClientName" runat="server" Text='<%# Eval("ClientName") %>'></asp:Label></br>
                                  
                                    <asp:Label ID="LBL_NewClientTel" runat="server" Text='<%# Eval("NewClientTel") %>' Visible='<%# Eval("New") %>'></asp:Label></br>
                                       <asp:Label ID="LBL_NewClientAddress" runat="server" Text='<%# Eval("NewClientAddress") %>' Visible='<%# Eval("New") %>'></asp:Label>
                                      <asp:Label ID="LBL_CobractPerson" runat="server" Text='<%# Eval("ContactPerson") %>' ></asp:Label></br>
                                       <asp:Label ID="LBL_ContactPersonTel" runat="server" Text='<%# Eval("ContactPersonMobile") %>'></asp:Label>
                                     
                                     
                                     <asp:HyperLink ID="lnkAttachment" Text='<%# Eval("FileName") %>' NavigateUrl='<%# Eval("FilePath") %>' ToolTip="Click to Preview File" runat="server" Visible='<%# Eval("New") %>' Target="_blank"></asp:HyperLink>
                               </ItemTemplate>
                                 
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Through" FooterStyle-CssClass="col-md-2" ItemStyle-CssClass="col-md-2" HeaderStyle-CssClass="col-md-2" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_Through" runat="server" Text='<%# Eval("ThroughName") %>'></asp:Label>
                                </ItemTemplate>
                                 
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="From" FooterStyle-CssClass="col-md-1" ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="col-md-1" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_From" runat="server" Text='<%# Eval("From") %>'></asp:Label>
                                </ItemTemplate>
                                 
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="To" FooterStyle-CssClass="col-md-1" ItemStyle-CssClass="col-md-1" HeaderStyle-CssClass="col-md-1" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_To" runat="server" Text='<%# Eval("To") %>'></asp:Label>
                                </ItemTemplate>
                                
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Location" FooterStyle-CssClass="col-md-2" ItemStyle-CssClass="col-md-2" HeaderStyle-CssClass="col-md-2" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_Location" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
                                </ItemTemplate>
                                 
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Reason" FooterStyle-CssClass="col-md-2" ItemStyle-CssClass="col-md-2" HeaderStyle-CssClass="col-md-2" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_Reason" runat="server" Text='<%# Eval("Reason") %>'></asp:Label>
                                </ItemTemplate>
                                
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
                            
                     <div class=" mt-3">

                            <h5 class="d-inline-block orangeColor">Review Number :</h5>
                            <%--<textarea name="text" id="" cols="50" rows="5" placeholder="Write Your Text Here" class=" p-3 w-100"></textarea>--%>
                            <%--<asp:TextBox ID="TXT_Review" runat="server" TextMode="multiline" Columns="50" Rows="5" PlaceHolder="Write Your Text Here" class="textArea p-3 w-100"></asp:TextBox>--%>
                         <asp:DropDownList ID="DDl_Review" runat="server"></asp:DropDownList>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="DDl_Review" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <%-- <asp:CompareValidator ID="CompareValidator1" ControlToValidate="TXT_Review" Type="Double" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="should be a number"></asp:CompareValidator>--%>
                        </div>
				    <div class=" mt-3">

                            <h5 class="d-inline-block orangeColor">GENERAL NOTES :</h5>
                            <%--<textarea name="text" id="" cols="50" rows="5" placeholder="Write Your Text Here" class=" p-3 w-100"></textarea>--%>
                            <asp:TextBox ID="TXT_Desc" runat="server" TextMode="multiline" Columns="50" Rows="5" PlaceHolder="Write Your Text Here" class="textArea p-3 w-100"></asp:TextBox>
                            
                        </div>
                    

                      

                       <div class="row my-4">
				             <div class="col-md-4 offset-md-8 text-right">
                   
                                 <asp:Button CssClass="btn text-white save px-5 py-1 robotoBold primaryColorBlue" ID="BTN_Finish" runat="server" Text="Finish" OnClick="BTN_Finish_Click" />
                                 
                              </div>
                            </div>

                        </div>

                    
                    </ContentTemplate>
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
