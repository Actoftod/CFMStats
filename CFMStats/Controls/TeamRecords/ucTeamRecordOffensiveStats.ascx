<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTeamRecordOffensiveStats.ascx.cs" Inherits="CFMStats.Controls.TeamRecords.ucTeamRecordOffensiveStats" %>


<div class="row">
    <div class="col-sm-6">
        <strong>Most Pass Yards</strong>
        <div class="table-responsive table-bordered-curved">
            <div id="tablePassYards" runat="server" visible="true"></div>
        </div>
    </div>
    <div class="col-sm-6">
        <strong>Most Rush Yards</strong>
        <div class="table-responsive table-bordered-curved">
            <div id="tableRushYards" runat="server" visible="true"></div>
        </div>
    </div>
</div>

<div class="hidden-xs">
    <br />
</div>

<div class="row">
    <div class="col-sm-6">
        <strong>Most Total Yards</strong>
        <div class="table-responsive table-bordered-curved">
            <div id="tableTotalYards" runat="server" visible="true"></div>
        </div>
    </div>
    <div class="col-sm-6">
        <strong>Most Touchdowns</strong>
        <div class="table-responsive table-bordered-curved">
            <div id="tableTouchdowns" runat="server" visible="true"></div>
        </div>
    </div>
</div>
