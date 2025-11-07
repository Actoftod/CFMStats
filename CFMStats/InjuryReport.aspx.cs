using System;
using System.Configuration;
using System.Data;
using System.Text;
using CFMStats.Classes;

namespace CFMStats
{
    public partial class InjuryReport : System.Web.UI.Page
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
                LoadInjuryReport();
            }
        }

        protected void ddlTeamSelector_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadInjuryReport();
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
        }

        private void LoadInjuryReport()
        {
            var sp = new StoredProc
            {
                Name = "InjuryReport_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));

            var teamId = int.Parse(ddlTeamSelector.SelectedValue);
            if (teamId > 0)
            {
                sp.ParameterSet.Parameters.AddWithValue("@teamId", teamId);
            }
            else
            {
                sp.ParameterSet.Parameters.AddWithValue("@teamId", DBNull.Value);
            }

            var ds = StoredProc.ShowMeTheData(sp);

            if (ds.Tables[0].Rows.Count == 0)
            {
                litInjuryReport.Text = "<div class='alert alert-success'>No injuries reported. All players are healthy!</div>";
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<table class='table table-striped table-bordered tablesorter'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th>Player</th>");
            sb.Append("<th>Position</th>");
            sb.Append("<th>Team</th>");
            sb.Append("<th>Injury Type</th>");
            sb.Append("<th>Weeks Out</th>");
            sb.Append("<th>IR Status</th>");
            sb.Append("<th>Age</th>");
            sb.Append("<th>Years Pro</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var isOnIR = Convert.ToBoolean(row["isOnIR"]);
                var rowClass = isOnIR ? " class='table-danger'" : "";

                sb.Append($"<tr{rowClass}>");
                sb.Append($"<td><a href='/Profile?id={row["playerId"]}&leagueId={Session["leagueId"]}'>{row["playerName"]}</a></td>");
                sb.Append($"<td>{row["position"]}</td>");
                sb.Append($"<td class='c{row["teamName"].ToString().Replace(" ", string.Empty)}'><div style='display:none;'>{row["teamName"].ToString().Replace(" ", string.Empty)}</div>{row["teamName"]}</td>");

                var injuryName = row["InjuryName"] != DBNull.Value ? row["InjuryName"].ToString() : "Unknown";
                sb.Append($"<td>{injuryName}</td>");

                var weeksOut = Convert.ToInt32(row["injuryLength"]);
                sb.Append($"<td>{(weeksOut > 0 ? weeksOut.ToString() : "-")}</td>");

                sb.Append($"<td>{(isOnIR ? "<span class='badge bg-danger'>IR</span>" : "-")}</td>");
                sb.Append($"<td>{row["age"]}</td>");
                sb.Append($"<td>{row["yearsPro"]}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            litInjuryReport.Text = sb.ToString();
        }
    }
}
