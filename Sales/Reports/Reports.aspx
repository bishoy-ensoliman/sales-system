<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GarasERP.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="GarasSales.Sales.Reports.Reports" %>
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
                <h3 class="primaryBlue robotoMed">Daily Sales Reports</h3>
              


                   <asp:GridView ID="GV_Reports" HeaderStyle-CssClass="row primaryBlue robotoMed text-center" RowStyle-CssClass="row robotoMed text-center my-3" GridLines="None" ShowFooter="false" runat="server"
                        AutoGenerateColumns="false" OnPageIndexChanging="GV_Reports_PageIndexChanging"  AllowSorting="false" AllowFiltering="false" 
                    FilterDataFields="ID,ReportDate,UserName,ModifiedDate" Width="100%"
                    FilteredDataSourcePropertyName="FilterExpression" FilteredDataSourcePropertyFormat="{1} = '{0}'" EnableViewState="true" 
                        CellPadding="10" CellSpacing="0" BorderWidth="0" PageSize="20" AllowPaging="true"  EmptyDataText="there is no reports to display"  PagerSettings-Mode="NumericFirstLast" PagerStyle-HorizontalAlign="Left">

                        <Columns>
                             <asp:TemplateField HeaderText="ID" SortExpression="ID" ItemStyle-CssClass="col-md-1"  HeaderStyle-CssClass="col-md-1" Visible="false" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_ID"  runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" SortExpression="ReportDate" ItemStyle-CssClass="col-md-2"  HeaderStyle-CssClass="col-md-2" >
                                <ItemTemplate>
                                    <asp:HyperLink ID="HL_ReportDate" runat="server" NavigateUrl='<%# Eval("ViewURL") %>' ><%# Eval("ReportDate") %></asp:HyperLink>
                                   <%-- <asp:Label ID="LBL_ReportDate" runat="server" Text='<%# Eval("ReportDate") %>'></asp:Label>--%>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="User Name" SortExpression="UserName" ItemStyle-CssClass="col-md-3" HeaderStyle-CssClass="col-md-3" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_UserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Status" SortExpression="Status" ItemStyle-CssClass="col-md-2" HeaderStyle-CssClass="col-md-2" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_Status" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Modified Date" SortExpression="ModifiedDate" ItemStyle-CssClass="col-md-3" HeaderStyle-CssClass="col-md-3" >
                                <ItemTemplate>
                                    <asp:Label ID="LBL_ModifiedDate" runat="server" Text='<%# Eval("ModifiedDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            


                            <asp:TemplateField HeaderText="Eval" >
                             
                                <ItemTemplate>


                                   <%-- <button class="btn btn-primary edit py-0 px-3">Edit<img src="../../UI/Images/editBtn.png" class="ml-2" height="15px" width="15px"/></button>--%>
                                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-primary edit py-0 px-3" NavigateUrl='<%# Eval("URL") %>' visible='<%# Eval("CanEdit") %>'>Edit<img src="../../UI/Images/editBtn.png" class="ml-2" height="15px" width="15px"/></asp:HyperLink>

                                    <asp:HyperLink ID="HyperLink2" runat="server" CssClass="btn btn-primary edit py-0 px-3" NavigateUrl='<%# Eval("URL") %>' visible='<%# Eval("CanVer") %>'>Verify<img src="../../UI/Images/icons/review.png" class="ml-2" height="15px" width="15px"/></asp:HyperLink>
                                    <asp:Label ID="LBL_Eval" runat="server" Text='<%# Eval("Review") %>' visible='<%# Eval("Reviewed") %>'></asp:Label>
                                    
                                    <%--<a runat="server" ID="BTN_Edit" visible='<%# Eval("CanEdit") %>' onclick=" return showDialog('addBankBranch.aspx?BBID='+ <%# Eval("ID")  %> )"><img alt="Complete" src="../../UI/Images/editBtn.png" /></a>--%>
                                  
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField FooterStyle-VerticalAlign="Top" ItemStyle-VerticalAlign="Top" >
                              
                                <ItemTemplate>
                                   
                                    <asp:ImageButton ID="BTN_Delete" runat="server" ToolTip="حذف" CommandName="Delete" CausesValidation="false" OnClientClick="return confirm('هل انت متاكد من مسح هذا الفرع ؟');" ImageUrl="/Style Library/ar-sa/images/Icons/delete.gif" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                            

                   
                        </Columns>
                    </asp:GridView>



                


              </div>

            </div>
          </div>
        </div>
      </div>
         </div>

     <script src="../../UI/node_modules/jquery/dist/jquery.min.js"></script>
    <script src="../../UI/node_modules/bootstrap/dist/js/bootstrap.bundle.js"></script>

</asp:Content>
