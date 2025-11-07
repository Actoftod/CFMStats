using System;
using System.Configuration;
using System.Data;
using System.Text;
using CFMStats.Classes;

namespace CFMStats
{
    public partial class SeasonArchives : System.Web.UI.Page
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
                LoadSeasonArchives();
            }
        }

        private void LoadSeasonArchives()
        {
            var sp = new StoredProc
            {
                Name = "Seasons_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));

            var ds = StoredProc.ShowMeTheData(sp);

            if (ds.Tables[0].Rows.Count == 0)
            {
                litSeasonArchives.Text = "<div class='alert alert-info'>No season data available yet.</div>";
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<table class='table table-striped table-bordered tablesorter'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th>Season</th>");
            sb.Append("<th>Quick Links</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            var leagueId = Session["leagueId"];

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var season = row["seasonIndex"].ToString();

                sb.Append("<tr>");
                sb.Append($"<td><strong>Season {season}</strong></td>");
                sb.Append("<td>");
                sb.Append($"<a href='/TeamRankings.aspx?leagueId={leagueId}&season={season}' class='btn btn-sm btn-primary'>Standings</a> ");
                sb.Append($"<a href='/Schedule.aspx?leagueId={leagueId}&season={season}' class='btn btn-sm btn-primary'>Schedule</a> ");
                sb.Append($"<a href='/PlayerRecords.aspx?leagueId={leagueId}&season={season}' class='btn btn-sm btn-primary'>Records</a> ");
                sb.Append($"<a href='/Players.aspx?leagueId={leagueId}&season={season}' class='btn btn-sm btn-primary'>Players</a>");
                sb.Append("</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            litSeasonArchives.Text = sb.ToString();
        }
    }
}
