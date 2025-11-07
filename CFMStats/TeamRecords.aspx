<%@ Page Title="Team Records" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TeamRecords.aspx.cs" Inherits="CFMStats.TeamRecords" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">

        <div class="col-xs-6 col-sm-6">
            <div class="input-group input-group-sm mb-3">
                <span class="input-group-text bg-secondary" id="basic-addon1">Stat Type</span>
                <asp:DropDownList ID="ddlStatSelector" runat="server" CssClass="form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlStatSelector_OnSelectedIndexChanged">
                    <asp:ListItem Text="Offensive Stats" Value="offensive"></asp:ListItem>
                    <asp:ListItem Text="Defensive Stats" Value="defensive"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="col-xs-6 col-sm-6">
            <div class="input-group input-group-sm mb-3">
                <span class="input-group-text bg-secondary" id="basic-addon2">Duration</span>
                <asp:DropDownList ID="ddlDuration" runat="server" CssClass="form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlDuration_OnSelectedIndexChanged">
                    <asp:ListItem Text="Game" Value="game"></asp:ListItem>
                    <asp:ListItem Text="Season" Value="season"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div style="text-align: center;">
                        <label class="badge bg-warning">... LOADING ...</label>
                        <label class="badge bg-danger">... LOADING ...</label>
                        <label class="badge bg-success">... LOADING ...</label><br />
                    </div>
                    <br />
                </ProgressTemplate>
            </asp:UpdateProgress>


            <asp:PlaceHolder ID="phStatHolder" runat="server"></asp:PlaceHolder>


        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlStatSelector" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlDuration" EventName="SelectedIndexChanged" />
        </Triggers>

    </asp:UpdatePanel>



    <%-- tablesorter --%>
    <link href="Content/tablesorter/theme.ice.min.css" rel="stylesheet" />
    <script src="Scripts/jquery.tablesorter.min.js"></script>

    <script>


        //Initial bind
        $(document).ready(function () {
            BindControlEvents();
        });

        //Re-bind for callbacks
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            BindControlEvents();
        });


        function BindControlEvents() {
            $('table').tablesorter({
                theme: 'ice',
                widthFixed: true
            });
        }



    </script>
</asp:Content>
