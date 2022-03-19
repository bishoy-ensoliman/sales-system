<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/GarasERP.Master" AutoEventWireup="true" CodeBehind="SalesHome.aspx.cs" Inherits="GarasSales.Sales.SalesHome" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <title>Sales Home</title>
    <script type="text/javascript">
        function delayFilter() {
            document.getElementById('<%= btnDelay.ClientID %>').click();
        }

        function todayFilter() {
            document.getElementById('<%= btnToday.ClientID %>').click();
        }

        function monitorFilter() {
            document.getElementById('<%= btnMonitor.ClientID %>').click();
        }
    </script>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="body" runat="server">
    <div class="container mt-3">
        <div class="row">
            <div class="col-lg-8">
                <div class="row">
                    <div class="outer">
                        <div class="inner">
                            <div class="container">
                                <div class="row py-3">
                                    <div class="col-md-6">
                                        <div id="bar-chart"></div>
                                        <p class="text-center mt-3 mb-1 blueColor robotoMed">TOP 3 SELLING PRODUCTS : </p>
                                        <div class="w-75 mx-auto">
                                            <div class="products robotoReg darkBlueColor">
                                                <p class="mb-0 px-4">
                                                    <small>1.PRODUCT 01</small>
                                                </p>
                                            </div>
                                            <div class="products robotoReg darkBlueColor">
                                                <p class="mb-0 px-4">
                                                    <small>1.PRODUCT 01</small>
                                                </p>
                                            </div>
                                            <div class="products robotoReg darkBlueColor">
                                                <p class="mb-0 px-4">
                                                    <small>1.PRODUCT 01</small>
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- start -->

                                    <div class="col-md-2 target">
                                        <p class="mb-0 darkBlueColor robotoReg">Target</p>
                                        <p class="target darkBlueColor robotoBold">
                                            <asp:Label ID="lblmyTarget" runat="server"></asp:Label>
                                        </p>
                                        <p class="mb-0 blueColor robotoReg">Achieved</p>
                                        <p class="target blueColor robotoBold">
                                            0
                                        </p>
                                        <div class="progress">
                                            <div class="progress-bar" role="progressbar" style="width: 25%" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
                                        </div>
                                        <h5 class="orangeColor ">20%</h5>
                                        <div class="numberCard p-2 py-3 mt-4 robotoBold darkBlueColor">
                                            <h1 class="mb-0 text-center target">3</h1>
                                            <p class="mb-0 text-center">DEALS</p>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-6 mt-3">

                                                <div id="pacman" style="width: 150px; height: 150px;" class="p-0 m-0">
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <p class="mb-0 blueColor robotoBold">CUSTOMER SATISFACTION</p>
                                                <h1 class="mb-0 blueColor">
                                                75%</h2>
                            <p class="mb-0 mt-5 orangeColor robotoBold">CLOSING RATE (win)</p>
                                                <h1 class="mb-0 orangeColor">
                                                67%</h2>
                                            </div>
                                        </div>
                                        <p class="text-center blueColor mt-5 mb-0">(13 D) Avg. Time To Close Deal</p>
                                    </div>
                                    <!-- end -->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mt-3 colCards">
                    <div class="col-lg-8 px-0">
                        <div class="outer h-100">
                            <div class="inner h-100">
                                <h4 class="p-3 robotoMed darkBlueColor">HR AND ADMIN</h4>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 pr-0">
                        <div class="outer blueColor">
                            <div class="inner text-center">
                                <a href="/Sales/Client/MyClients.aspx">
                                    <h4>MY CLIENTS</h4>
                                    <h5>
                                        <asp:Label ID="lblClientCount" runat="server"></asp:Label>
                                        CLIENTS</h5>
                                </a>
                                <canvas id="myChart"></canvas>
                                <button id="btnNewClient" runat="server" class="btn edit btn-light py-0 mt-3 primaryBlue shadow-sm " onclick="window.location.href='/Sales/Client/NewClient.aspx';return false;">
                                    <img src="/UI/Images/plus-symbol.png" style="height: 15px; width: 15px" visible="false">
                                    Add New Client
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 offset-md-6">
                        <div class="row mt-3">
                            <div class="col-md-4">
                                <div class="updatesCard p-3 text-center">
                                    <img src="/UI/Images/icons/open-folder-icon.png">
                                    <p class="mb-0 text-center"><small>50</small></p>
                                </div>
                                <p class="text-center mb-0"><small>OPEN PROJECTS</small></p>
                                <p class="text-center"><small>3 UPDATES</small></p>
                            </div>
                            <div class="col-md-4 clickableBottomCard" onclick="window.location.href='/Offers/MonitoringPage.aspx'">
                                <div class="updatesCard p-3 text-center">
                                    <img src="/UI/Images/icons/open-folder-icon.png">
                                    <p class="mb-0 text-center">
                                        <small>
                                            <asp:Label ID="lblOffersCount" runat="server"></asp:Label></small>
                                    </p>
                                </div>
                                <p class="text-center mb-0"><small>OFFERS</small></p>
                                <%--<p class="text-center"><small>5 UPDATES</small></p>--%>
                            </div>
                            <div class="col-md-4 clickableBottomCard" onclick="window.location.href='/Sales/Reports/Reports.aspx'">
                                <div class="updatesCard p-3 text-center">
                                    <img src="/UI/Images/icons/chat.png">
                                    <p class="mb-0 text-center"><small>
                                        <asp:Label ID="lblReportsCount" runat="server"></asp:Label></small></p>
                                </div>
                                <p class="text-center mb-0"><small>REPOTRS</small></p>
                                <p class="text-center"><small>
                                    <asp:Label ID="lbltodayReportCount" runat="server"></asp:Label>
                                    TODAY</small></p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="col-lg-4">
                <div class="row side">
                    <div class="col">
                        <div class="outer">
                            <div class="inner">
                                <div class="container">
                                    <div class="d-flex justify-content-between">
                                        <h4 class="mb-0">MY TEAM</h4>
                                        <p>
                                            <small>
                                                <asp:Label ID="lblTeamCount" runat="server" Text="0 members"></asp:Label></small>
                                        </p>
                                    </div>
                                    <asp:Repeater ID="rptrMyTeam" runat="server" OnItemDataBound="rptrMyTeam_ItemDataBound">
                                        <ItemTemplate>
                                            <div class="d-inline-block mb-0 ">
                                            <asp:HyperLink ID="lnkTeamMate" runat="server">
                                                <asp:Image ID="imgTeamMatePhoto" runat="server" class="userImg" />
                                            </asp:HyperLink>
                                                <p class='memberName mb-0'>
                                            <asp:Label ID="lblTeamMateName" runat="server"></asp:Label>
                                                    </p>
                                                </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <br />
                                    <div class="text-right">
                                        <a href="/Employees/Employees.aspx">show more</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row mt-3">
                    <div class="col">
                        <div class="outerSide">
                            <div class="innerSide">
                                <asp:UpdatePanel ID="updPnlTasks" runat="server">
                                    <ContentTemplate>


                                        <div class="container">
                                            <div class="d-flex justify-content-between">
                                            <h4 class="mb-0">MY TASKS</h4>
                                            <p>
                                                <small>
                                                    <asp:Label ID="lblTasksCount" runat="server" Text="0 tasks"></asp:Label></small>
                                            </p>
                                                </div>
                                            <!-- START -->
                                            <div class="row">
                                                <div class="col-md-4 px-2">
                                                    <asp:Button ID="btnDelay" runat="server" Style="display: none;" OnClick="btnDelay_Click" />
                                                    <div class="numberCard p-3" onclick="delayFilter();">
                                                        <div class="darkBlueColor clickableBottomCard">
                                                            <h1 class="mb-0 text-center">
                                                                <asp:Label ID="lblDelayCount" runat="server" Text="0"></asp:Label></h1>
                                                            <p class="mb-0 text-center">Delay</p>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 px-2">
                                                    <asp:Button ID="btnToday" runat="server" Style="display: none;" OnClick="btnToday_Click" />
                                                    <div class="numberCard p-3" onclick="todayFilter();">
                                                        <div class="darkBlueColor clickableBottomCard">
                                                            <h1 class="mb-0 text-center">
                                                                <asp:Label ID="lblTodayCount" runat="server" Text="0"></asp:Label></h1>
                                                            <p class="mb-0 text-center">Today</p>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 px-2">
                                                    <asp:Button ID="btnMonitor" runat="server" Style="display: none;" OnClick="btnMonitor_Click" />
                                                    <div class="numberCard p-3" onclick="monitorFilter();">
                                                        <div class="darkBlueColor clickableBottomCard">
                                                            <h1 class="mb-0 text-center">
                                                                <asp:Label ID="lblMonitoring" runat="server" Text="0"></asp:Label></h1>
                                                            <p class="mb-0 text-center">Future</p>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- END -->
                                            <ul class="list-group list-group-flush mt-4">
                                                <asp:Repeater ID="rptrTasks" runat="server" OnItemDataBound="rptrTasks_ItemDataBound">
                                                    <ItemTemplate>
                                                        <li class="list-group-item">
                                                            <div class="d-flex justify-content-between">
                                                            
                                                                <h5 class="mb-0">
                                                                    <asp:Label ID="lbltaskName" runat="server"></asp:Label></h5>
                                                                <asp:HyperLink ID="lnkTask" runat="server" CssClass="mb-0 mt-3">
                                                                <small>VIEW MORE</small>
                                                            </asp:HyperLink>
                                                                </div>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                            <div class="d-flex justify-content-between mt-3">
                                                <button type="button" class="btn edit px-4 primaryColorBlue shadow-sm text-white createTaskBtn"
                                                    onclick="window.location.href='/Tasks/CreateTask.aspx'">
                                                    Create Task</button>
                                                <a href="/Tasks/ViewTasks.aspx" class="mt-2">View All Tasks</a>
                                            </div>

                                            <%--<button id="btnTaskCalendar" onclick="btnTaskCalendarClicked();">Calendar View</button>--%>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        function drawCharts() {
            var myTarget = document.getElementById('<%= lblmyTarget.ClientID %>').textContent;
            myTarget = myTarget.substr(0, myTarget.indexOf(" "));
            var dmytarget = parseFloat(myTarget);
            var monTarget = dmytarget / 12.0;
            var barData = google.visualization.arrayToDataTable([
                ['Month', 'Target', 'Achieved'],
                ['JAN', monTarget, 0],
                ['FEB', monTarget, 0],
                ['MAR', monTarget, 0],
                ['APR', monTarget, 0],
                ['MAY', monTarget, 0],
                ['JUN', monTarget, 0],
                ['JUL', monTarget, 0],
                ['AUG', monTarget, 0],
                ['SEP', monTarget, 0],
                ['OCT', monTarget, 0],
                ['NOV', monTarget, 0],
                ['DEC', monTarget, 0]
            ]);
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
                    maxValue: monTarget,
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

            drawChart();
        }
        drawChartsHome();
    </script>
</asp:Content>



