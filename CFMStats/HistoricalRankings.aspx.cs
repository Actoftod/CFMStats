using System;
using System.Configuration;
using System.Data;
using System.Text;
using CFMStats.Classes;

namespace CFMStats
{
    public partial class HistoricalRankings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Helper.StringNull(Request.QueryString["leagueId"]).Length == 0)
            {
                Response.Redirect("~/");
            }

            Session["leagueId"] = Helper.StringNull(Request.QueryString["leagueId"]);

            if (!IsPostBack)
            {
                LoadTeams();
            }
        }

        protected void ddlTeamSelector_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHistoricalRankings();
        }

        private void LoadTeams()
        {
            var sp = new StoredProc
            {
                Name = "TeamInfo_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));

            var ds = StoredProc.ShowMeTheData(sp);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlTeamSelector.Items.Add(new System.Web.UI.WebControls.ListItem(row["displayName"].ToString(), row["teamId"].ToString()));
            }

            if (ddlTeamSelector.Items.Count > 0)
            {
                LoadHistoricalRankings();
            }
        }

        private void LoadHistoricalRankings()
        {
            var teamId = int.Parse(ddlTeamSelector.SelectedValue);

            // Get team standings history
            var sp = new StoredProc
            {
                Name = "TeamStandings_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));
            sp.ParameterSet.Parameters.AddWithValue("@teamId", teamId);

            var ds = StoredProc.ShowMeTheData(sp);

            if (ds.Tables[0].Rows.Count == 0)
            {
                litHistoricalRankings.Text = "<div class='alert alert-info'>No historical data available for this team yet.</div>";
                return;
            }

            // Group by season
            var seasonData = ds.Tables[0].AsEnumerable()
                .GroupBy(r => r.Field<int>("seasonIndex"))
                .OrderByDescending(g => g.Key);

            var sb = new StringBuilder();
            sb.Append("<table class='table table-striped table-bordered tablesorter'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th>Season</th>");
            sb.Append("<th>Final Record</th>");
            sb.Append("<th>Division Rank</th>");
            sb.Append("<th>Conference Rank</th>");
            sb.Append("<th>Playoff Seed</th>");
            sb.Append("<th>Points For</th>");
            sb.Append("<th>Points Against</th>");
            sb.Append("<th>Point Diff</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (var season in seasonData)
            {
                var lastWeek = season.OrderByDescending(r => r.Field<int>("weekIndex")).FirstOrDefault();

                if (lastWeek != null)
                {
                    var wins = lastWeek.Field<int>("totalWins");
                    var losses = lastWeek.Field<int>("totalLosses");
                    var ties = lastWeek.Field<int>("totalTies");
                    var pf = lastWeek.Field<int>("totalPF");
                    var pa = lastWeek.Field<int>("totalPA");

                    sb.Append("<tr>");
                    sb.Append($"<td>{lastWeek["seasonIndex"]}</td>");
                    sb.Append($"<td>{wins}-{losses}-{ties}</td>");
                    sb.Append($"<td>{lastWeek["divisionRank"]}</td>");
                    sb.Append($"<td>{lastWeek["confRank"]}</td>");
                    sb.Append($"<td>{lastWeek["playoffSeed"]}</td>");
                    sb.Append($"<td>{pf}</td>");
                    sb.Append($"<td>{pa}</td>");
                    sb.Append($"<td>{pf - pa}</td>");
                    sb.Append("</tr>");
                }
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            litHistoricalRankings.Text = sb.ToString();
        }
    }
}
