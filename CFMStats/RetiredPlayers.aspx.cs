using System;
using System.Configuration;
using System.Data;
using System.Text;
using CFMStats.Classes;

namespace CFMStats
{
    public partial class RetiredPlayers : System.Web.UI.Page
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
                LoadRetiredPlayers();
            }
        }

        private void LoadRetiredPlayers()
        {
            var sp = new StoredProc
            {
                Name = "RetiredPlayers_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));

            var ds = StoredProc.ShowMeTheData(sp);

            if (ds.Tables[0].Rows.Count == 0)
            {
                litRetiredPlayers.Text = "<div class='alert alert-info'>No retired players yet. The Hall of Fame awaits its first inductee!</div>";
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<table class='table table-striped table-bordered tablesorter'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th>Player</th>");
            sb.Append("<th>Position</th>");
            sb.Append("<th>Last Team</th>");
            sb.Append("<th>Overall</th>");
            sb.Append("<th>Legacy Score</th>");
            sb.Append("<th>Years Pro</th>");
            sb.Append("<th>Retired Age</th>");
            sb.Append("<th>College</th>");
            sb.Append("<th>Draft</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                sb.Append("<tr>");
                sb.Append($"<td><a href='/Profile?id={row["playerId"]}&leagueId={Session["leagueId"]}'>{row["playerName"]}</a></td>");
                sb.Append($"<td>{row["position"]}</td>");
                sb.Append($"<td>{row["lastTeam"]}</td>");

                var overall = row["overallRating"] != DBNull.Value ? row["overallRating"].ToString() : "N/A";
                sb.Append($"<td>{overall}</td>");

                sb.Append($"<td>{row["legacyScore"]}</td>");
                sb.Append($"<td>{row["yearsPro"]}</td>");
                sb.Append($"<td>{row["age"]}</td>");
                sb.Append($"<td>{row["college"]}</td>");

                var draft = row["draftRound"] != DBNull.Value && Convert.ToInt32(row["draftRound"]) > 0
                    ? $"Rd {row["draftRound"]}, Pick {row["draftPick"]}"
                    : "Undrafted";
                sb.Append($"<td>{draft}</td>");

                sb.Append("</tr>");
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            litRetiredPlayers.Text = sb.ToString();
        }
    }
}
