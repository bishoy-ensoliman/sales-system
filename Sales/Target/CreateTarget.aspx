<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPages/GarasERP.Master" AutoEventWireup="true" CodeBehind="CreateTarget.aspx.cs" Inherits="GarasSales.Target.CreateTarget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.4.0/Chart.min.js"></script>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.bundle.js"></script>
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
        <script type="text/javascript" src="https://www.google.com/jsapi"></script>--%>
    <script src="node_modules/jquery/dist/jquery.min.js"></script>
    <script src="node_modules/bootstrap/dist/js/bootstrap.bundle.js"></script>

    <title>Sales Plan</title>
    <script type="text/javascript">
        var totalID = '<%= tbxYearTarget.ClientID %>';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div id="divPage" runat="server" visible="false">
        <div class="container mt-5">
            <div class="row upperCard">
                <div class="col">
                    <div class="outerDiv">
                        <div class="innerDiv">
                            <div class="contaienr">
                                <div class="row">
                                    <div class="col-md-9">
                                        <div class="d-flex justify-content-between">
                                            <div>
                                                <p class="d-inline-block mr-4 target">New Sales Target for</p>
                                                <div class="dropdown d-inline-block">
                                                    <asp:DropDownList class="btn dropdown-toggle px-3 py-1" ID="ddlTargetYear" runat="server" OnSelectedIndexChanged="ddlTargetYear_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div>
                                                <button type="button" name="button" class="btn edit px-4 py-1" style="display: none;">Edit</button>
                                            </div>
                                        </div>

                                        <p class="mr-4 robotoMed statistics">Last 5 Years Sales Statistics</p>
                                        <div class="container">
                                            <div class="row robotoMed">
                                                <asp:Repeater ID="rptrLast5Years" runat="server" OnItemDataBound="rptrLast5Years_ItemDataBound" EnableViewState="true">
                                                    <ItemTemplate>
                                                        <div class="col-md-2 blueCards mr-2 py-2 ">
                                                            <img src="/UI/images/up-arrow.png" class="mb-1">
                                                            <asp:Label ID="lblYear" runat="server" class="mb-0 d-inline-block text-white ml-2"></asp:Label>
                                                            <div class="test d-inline-block ml-4"></div>
                                                            <p class="text-warning mb-0 ml-3">
                                                                <asp:Label ID="lblMoney" runat="server"></asp:Label>&nbsp;
                                                                <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                                                            </p>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                        <div class="container">
                                            <div class="row mt-5">
                                                <div class="col-md-2 mr-3">
                                                    <h2 class="mb-0 robotoBold mt-3">TOP 5</h2>
                                                    <p class="font-italic colorBlue">
                                                        SELLING
                                                            <br>
                                                        PRODUCTS
                                                    </p>
                                                </div>
                                                <div class="col-md-3 text-center">
                                                    <asp:Repeater ID="rptrTopSelling" runat="server" OnItemDataBound="rptrTopSelling_ItemDataBound">
                                                        <ItemTemplate>
                                                            <div class="products">
                                                                <p class="mb-0">
                                                                    <small>
                                                                        <asp:Label ID="lblProdName" runat="server"></asp:Label></small>
                                                                </p>
                                                            </div>

                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                                <div class="col-md-2 border-left">
                                                    <h2 class="mb-0 robotoBold mt-3">TOP 5</h2>
                                                    <p class="font-italic colorBlue">
                                                        PROFITABLE
                                                                <br>
                                                        PRODUCTS
                                                    </p>
                                                </div>
                                                <div class="col-md-3 text-center">
                                                    <asp:Repeater ID="rptrTopProfit" runat="server" OnItemDataBound="rptrTopProfit_ItemDataBound">
                                                        <ItemTemplate>
                                                            <div class="products">
                                                                <p class="mb-0">
                                                                    <small>
                                                                        <asp:Label ID="lblProdName" runat="server"></asp:Label></small>
                                                                </p>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 pl-0 mt-3">
                                        <div class="chart-container" style="position: relative; height: 20vh; width: 20vw">
                                            <canvas id="firstChart"></canvas>
                                        </div>
                                        <hr>
                                        <div class="chart-container" style="position: relative; height: 20vh; width: 20vw">
                                            <canvas id="secondChart"></canvas>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div class="container mt-5">
            <div class="row secondCard">
                <div class="col">
                    <div class="outerDiv">
                        <div class="innerDiv">
                            <div class=" head text-center">
                                <p class="d-inline-block mr-4 sales">SALES PLAN FOR</p>
                                <p class="d-inline-block px-3 py-0 date px-4 py-1">
                                    <strong>
                                        <asp:Label ID="lblPlanYear" runat="server"></asp:Label></strong>
                                </p>
                            </div>
                            <br>

                            <div class="contaienr">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="updtPnlTargetYear" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <p class="mr-4 d-inline-block target">
                                                    Target for
                                                    <asp:Label ID="lblTargetYear" runat="server"></asp:Label>

                                                </p>
                                                <%--<div class="products targetPrice d-inline-block px-4">
                                                <p class="mb-0">--%>
                                                <strong>
                                                    <!--onkeyup="changeFixed(totalID,'fixed','percent');"-->
                                                    <asp:TextBox ID="tbxYearTarget" runat="server" class="products targetPrice d-inline-block px-4" OnTextChanged="tbxYearTarget_TextChanged"
                                                        AutoPostBack="true" type="number" min='0'>
                                                    </asp:TextBox></strong>
                                                <%--</p>
                                            </div>--%>
                                                <strong>LE</strong>
                                                <asp:RequiredFieldValidator ID="ReqFldYearTarget" runat="server"
                                                    ControlToValidate="tbxYearTarget"
                                                    ErrorMessage="Year Target is required."
                                                    ForeColor="Red"
                                                    ValidationGroup="TargetCreate">
                                                </asp:RequiredFieldValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <br>
                                        <%--<label class="checkContainer">
                                                <input type="checkbox">
                                                <span class="checkmark"></span>
                                            </label>--%>
                                        <asp:UpdatePanel ID="updtPnlLocations" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <p class="d-inline-block">LOCATIONS</p>
                                                <div class="locations ml-5 border-bottom pb-3">
                                                    <div class="container text-center">
                                                        <asp:Repeater ID="rptrLocations" runat="server" OnItemDataBound="rptrLocations_ItemDataBound">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnLocID" runat="server" />
                                                                <div class="row w-100">
                                                                    <div class="col-lg-1 text-left">
                                                                        <p>
                                                                            <%# Container.ItemIndex + 1 %>.<asp:Label ID="lblBranchName" runat="server"></asp:Label>
                                                                        </p>
                                                                    </div>
                                                                    <div class="col-lg-1">
                                                                        <p>%</p>
                                                                    </div>
                                                                    <%--<span class="color px-3 mr-5">--%>
                                                                    <!--onkeyup='changeNextTbxs(totalID,"percent",<%# Container.ItemIndex %>,"fixed","percent");'-->
                                                                    <div class="col-lg-2 pl-0">
                                                                        <asp:TextBox ID="tbxPercent" class="percent color py-0 w-100" min="0" max="100" runat="server"
                                                                            type="number" OnTextChanged="tbxPercent_TextChanged" AutoPostBack="true">
                                                                        </asp:TextBox>
                                                                    </div>
                                                                    <div class="col-lg-3 pr-0">
                                                                        <p>FIXED AMOUNT</p>
                                                                    </div>
                                                                    <!--onkeyup='changeNextTbxs(totalID,"fixed",<%# Container.ItemIndex %>,"fixed","percent");'-->
                                                                    <div class="col-lg-2">
                                                                        <asp:TextBox ID="tbxAmount" class="fixed color py-0" min="0" runat="server" type="number"
                                                                            OnTextChanged="tbxAmount_TextChanged" AutoPostBack="true">
                                                                        </asp:TextBox>
                                                                    </div>

                                                                    <%--</span>--%>
                                                                    <div class="col-lg-1">
                                                                        <p>
                                                                            <asp:Label ID="lblCurrency" runat="server">LE</asp:Label>
                                                                        </p>
                                                                    </div>
                                                                    <asp:RequiredFieldValidator ID="ReqFldPercent" runat="server"
                                                                        ControlToValidate="tbxPercent"
                                                                        ErrorMessage="Percentage is required."
                                                                        ForeColor="Red"
                                                                        ValidationGroup="TargetCreate">
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="ReqFldAmount" runat="server"
                                                                        ControlToValidate="tbxAmount"
                                                                        ErrorMessage="Amount is required."
                                                                        ForeColor="Red"
                                                                        ValidationGroup="TargetCreate">
                                                                    </asp:RequiredFieldValidator>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="updtPnlProdPlan" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="row mt-4">
                                            <div class="col d-flex justify-content-between">
                                                <div class="title">
                                                    <label class="checkContainer">
                                                        <asp:CheckBox ID="chbxProdPlan" runat="server" OnCheckedChanged="chbxProdPlan_CheckedChanged" AutoPostBack="true" />
                                                        <span class="checkmark"></span>
                                                    </label>
                                                    <p class="d-inline-block">PRODUCT TYPE (CATEGORY)</p>
                                                </div>
                                                <%--<div class="buttons">
                                                <p class="d-inline-block orangeColor robotoMed">
                                                    <button type="button" class="btn mr-3 orangeBg text-white">+</button>Add Location
                                                </p>
                                                <p class="d-inline-block orangeColor robotoMed">
                                                    <button type="button" class="btn mx-3 orangeBg text-white remove">-</button>Remove Location
                                                </p>
                                            </div>--%>
                                            </div>
                                        </div>
                                        <div class="container">
                                            <div id="divProductDetails" runat="server">
                                                <div class="row">
                                                    <div class="col-lg-4">                                                        
                                                        <asp:HiddenField ID="hdnProdChartData" runat="server" />
                                                        <div id="bar-chart"></div>

                                                    </div>
                                                    <div class="col-lg-8">
                                                        <div class="row">
                                                            <asp:Repeater ID="rptrProdLocation" runat="server" OnItemDataBound="rptrProdLocation_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <div class="col-lg-6 mt-3">
                                                                        <div class=" d-inline-block productsLocation">
                                                                            <asp:HiddenField ID="hdnLocID" runat="server" />
                                                                            <p class="robotoBold d-inline-block">
                                                                                <asp:Label ID="lblProdLocation" runat="server"></asp:Label>
                                                                                <span class="priceBg px-5">
                                                                                    <asp:Label ID="lblProdLocTarget" runat="server"></asp:Label>
                                                                                </span>
                                                                                <asp:Label ID="lblCurrency" runat="server">LE</asp:Label>
                                                                            </p>
                                                                            <br />
                                                                            <asp:Repeater ID="rptrProducts" runat="server" OnItemDataBound="rptrProducts_ItemDataBound">
                                                                                <ItemTemplate>







                                                                                    <p class="mb-1">
                                                                                        <asp:HiddenField ID="hdnProdID" runat="server" />
                                                                                        <%# Container.ItemIndex + 1 %>. 
                                                                                        <asp:Label ID="lblProdName" runat="server" class="color py-0"></asp:Label>
                                                       
                                                            <%--<asp:DropDownList ID="ddlProducts" runat="server"
                                                                OnSelectedIndexChanged="ddlProducts_SelectedIndexChanged" AutoPostBack="true" class="color py-0">
                                                            </asp:DropDownList>--%>
                                                                                        % 
                                                                        <asp:TextBox ID="tbxProdPercent" class="color py-0 salesInput " runat="server" OnTextChanged="tbxProdPercent_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <%--</span>--%>= <%--<span class="px-4">--%>
                                                                                        <asp:TextBox ID="tbxProdFixed" class="color py-0 salesEqualInput" runat="server" OnTextChanged="tbxProdFixed_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                        <%--</span>--%><strong>LE</strong>
                                                                                    </p>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>

                                                </div>


                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="d-flex justify-content-end mt-3">
                                    <asp:Button ID="btnCreate" runat="server" ValidationGroup="TargetCreate" OnClick="btnCreate_Click" Text="Create" class="btn create orangeBg text-white edit px-4 py-1" />
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <script src="/UI/javascript/salesCharts.js"></script>
    <script src="/UI/javascript/salesPlan.js"></script>
    <asp:HiddenField ID="hdnFrstChart" runat="server" />
    <asp:HiddenField ID="hdnScndChart" runat="server" />
    <script type="text/javascript">
        drawBranchCharts('firstChart', document.getElementById('<%= hdnFrstChart.ClientID %>').value);
        drawBranchCharts('secondChart', document.getElementById('<%= hdnScndChart.ClientID %>').value);
    </script>
    <script type="text/javascript">
        function drawProdPlanChart() {
    setTimeout(function(){google.load('visualization', '1', {'callback':'drawChartsProd()', 'packages':['corechart']})}, 50);
    //google.load("visualization", "1.1", { packages: ['controls', 'corechart'] });
    google.setOnLoadCallback(drawChartsProd);
}

function drawChartsProd() {

    //var barData = google.visualization.arrayToDataTable([
    //    ['Type', 'Alex', 'Cairo'],
    //    ['Type1', 50, 600],
    //    ['Type2', 1370, 910],
    //    ['Type3', 660, 400],
    //    ['Type4', 1030, 540],
    //    ['Type5', 1000, 480],
    //]);
    try {
        var data = document.getElementById('<%= hdnProdChartData.ClientID %>').value;
        if (data != null && data != "") {
            data = data.replace(/'/g, '"');
            var barDataArray = JSON.parse(data);
            if (barDataArray.length != 0) {
                var barData = google.visualization.arrayToDataTable(barDataArray);
                // set bar chart options
                var barOptions = {
                    focusTarget: 'category',
                    backgroundColor: 'transparent',
                    colors: ['cornflowerblue', 'tomato'],
                    fontName: 'Open Sans',
                    chartArea: {
                        left: 50,
                        top: 10,
                        width: '100%',
                        height: '70%'
                    },
                    bar: {
                        groupWidth: '80%'
                    },
                    hAxis: {
                        textStyle: {
                            fontSize: 11
                        }
                    },
                    vAxis: {
                        minValue: 0,
                        maxValue: 1500,
                        baselineColor: '#DDD',
                        gridlines: {
                            color: '#DDD',
                            count: 4
                        },
                        textStyle: {
                            fontSize: 11
                        }
                    },
                    legend: {
                        position: 'bottom',
                        textStyle: {
                            fontSize: 12
                        }
                    },
                    animation: {
                        duration: 1200,
                        easing: 'out',
                        startup: true
                    }
                };
                // draw bar chart twice so it animates
                var barChart = new google.visualization.ColumnChart(document.getElementById('bar-chart'));
                //barChart.draw(barZeroData, barOptions);
                barChart.draw(barData, barOptions);
            }
            else {
                document.getElementById('bar-chart').innerHTML = "<p style='color:red;'>No Previous Data To Show</p>";
            }
        }
        else {
            document.getElementById('bar-chart').innerHTML = "<p style='color:red;'>No Previous Data To Show</p>";
            }
    } catch (ex) {

    }
}


    </script>

</asp:Content>
