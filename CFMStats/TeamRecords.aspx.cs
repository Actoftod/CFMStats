using CFMStats.Controls.TeamRecords;
using System;
using System.Web.UI;
using CFMStats.Classes;

namespace CFMStats
{
    public partial class TeamRecords : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Helper.StringNull(Request.QueryString["leagueId"]).Length == 0)
            {
                // no league, go back to start
                Response.Redirect("~/");
            }

            Session["leagueId"] = Helper.StringNull(Request.QueryString["leagueId"]);

        }

        protected void ddlDuration_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FetchRecords();
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlStatSelector_OnSelectedIndexChanged(null, null);
            }
        }

        protected void ddlStatSelector_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FetchRecords();
        }

        private void FetchRecords()
        {
            if (phStatHolder.Controls.Count > 0) { phStatHolder.Controls.Clear(); }

            var duration = Helper.StringNull(ddlDuration.SelectedItem.Value);

            switch (ddlStatSelector.SelectedItem.Value)
            {

                case "offensive":
                    OffensiveStats(duration);
                    break;

                case "defensive":
                    DefensiveStats(duration);
                    break;

                default:
                    break;
            }
        }


        private void OffensiveStats(string duration)
        {
            UserControl uc = (UserControl)Page.LoadControl("~/Controls/TeamRecords/ucTeamRecordOffensiveStats.ascx");

            string ClientId = uc.ClientID;
            Page.Controls.Add(uc);
            ClientId = uc.ClientID;

            var myUsercontrol = FindControl(ClientId) as ucTeamRecordOffensiveStats;
            myUsercontrol.Duration = duration;
            myUsercontrol.LeagueId = Helper.IntegerNull(Session["leagueId"]);

            phStatHolder.Controls.Add(uc);

        }

        private void DefensiveStats(string duration)
        {
            UserControl uc = (UserControl)Page.LoadControl("~/Controls/TeamRecords/ucTeamRecordDefensiveStats.ascx");

            string ClientId = uc.ClientID;
            Page.Controls.Add(uc);
            ClientId = uc.ClientID;

            var myUsercontrol = FindControl(ClientId) as ucTeamRecordDefensiveStats;
            myUsercontrol.Duration = duration;
            myUsercontrol.LeagueId = Helper.IntegerNull(Session["leagueId"]);

            phStatHolder.Controls.Add(uc);

        }

    }
}
