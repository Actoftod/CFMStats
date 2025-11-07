<%@ Page Title="Season Archives" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeasonArchives.aspx.cs" Inherits="CFMStats.SeasonArchives" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-sm-12">
            <h2>Season Archives</h2>
            <p>Browse historical season data</p>
        </div>
    </div>

    <div class="table-responsive">
        <asp:Literal ID="litSeasonArchives" runat="server"></asp:Literal>
    </div>

    <%-- tablesorter --%>
    <link href="Content/tablesorter/theme.ice.min.css" rel="stylesheet" />
    <script src="Scripts/jquery.tablesorter.min.js"></script>

    <script>

        $(document).ready(function () {
            $('table').tablesorter({
                theme: 'ice',
                widthFixed: true
            });
        });

    </script>

</asp:Content>
