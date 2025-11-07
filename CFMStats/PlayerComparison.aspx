<%@ Page Title="Player Comparison" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlayerComparison.aspx.cs" Inherits="CFMStats.PlayerComparison" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-sm-12">
            <h2>Player Comparison Tool</h2>
            <p>Compare two players side by side</p>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-6">
            <div class="input-group input-group-sm mb-3">
                <span class="input-group-text bg-secondary">Player 1</span>
                <asp:DropDownList ID="ddlPlayer1" runat="server" CssClass="form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlPlayers_OnSelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="input-group input-group-sm mb-3">
                <span class="input-group-text bg-secondary">Player 2</span>
                <asp:DropDownList ID="ddlPlayer2" runat="server" CssClass="form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlPlayers_OnSelectedIndexChanged">
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

            <asp:Literal ID="litComparison" runat="server"></asp:Literal>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlPlayer1" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlPlayer2" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
