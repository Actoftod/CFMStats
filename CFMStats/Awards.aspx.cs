using System;
using System.Configuration;
using System.Data;
using System.Text;
using CFMStats.Classes;

namespace CFMStats
{
    public partial class Awards : System.Web.UI.Page
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
                LoadSeasons();
                LoadAwards();
            }
        }

        protected void ddlSeasonSelector_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAwards();
        }

        protected void ddlAwardType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAwards();
        }

        private void LoadSeasons()
        {
            var sp = new StoredProc
            {
                Name = "Seasons_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));

            var ds = StoredProc.ShowMeTheData(sp);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ddlSeasonSelector.Items.Add(new System.Web.UI.WebControls.ListItem(row["seasonIndex"].ToString(), row["seasonIndex"].ToString()));
            }
        }

        private void LoadAwards()
        {
            var sp = new StoredProc
            {
                Name = "Awards_select",
                DataConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                ParameterSet = new System.Data.SqlClient.SqlCommand()
            };

            sp.ParameterSet.Parameters.AddWithValue("@leagueId", Helper.IntegerNull(Session["leagueId"]));

            var seasonIndex = int.Parse(ddlSeasonSelector.SelectedValue);
            if (seasonIndex > 0)
            {
                sp.ParameterSet.Parameters.AddWithValue("@seasonIndex", seasonIndex);
            }
            else
            {
                sp.ParameterSet.Parameters.AddWithValue("@seasonIndex", DBNull.Value);
            }

            var awardType = ddlAwardType.SelectedValue;
            if (!string.IsNullOrEmpty(awardType))
            {
                sp.ParameterSet.Parameters.AddWithValue("@awardType", awardType);
            }
            else
            {
                sp.ParameterSet.Parameters.AddWithValue("@awardType", DBNull.Value);
            }

            var ds = StoredProc.ShowMeTheData(sp);

            if (ds.Tables[0].Rows.Count == 0)
            {
                litAwards.Text = "<div class='alert alert-info'>No awards found. Awards can be manually added as seasons progress.</div>";
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<table class='table table-striped table-bordered tablesorter'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th>Season</th>");
            sb.Append("<th>Award</th>");
            sb.Append("<th>Player</th>");
            sb.Append("<th>Position</th>");
            sb.Append("<th>Team</th>");
            sb.Append("<th>Date</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{row["seasonIndex"]}</td>");
                sb.Append($"<td>{GetAwardDisplayName(row["awardType"].ToString())}</td>");
                sb.Append($"<td><a href='/Profile?id={row["playerId"]}&leagueId={Session["leagueId"]}'>{row["playerName"]}</a></td>");
                sb.Append($"<td>{row["position"]}</td>");
                sb.Append($"<td class='c{row["teamName"].ToString().Replace(" ", string.Empty)}'><div style='display:none;'>{row["teamName"].ToString().Replace(" ", string.Empty)}</div>{row["teamName"]}</td>");
                sb.Append($"<td>{Convert.ToDateTime(row["CreatedOn"]).ToString("MMM dd, yyyy")}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            litAwards.Text = sb.ToString();
        }

        private string GetAwardDisplayName(string awardType)
        {
            switch (awardType)
            {
                case "MVP":
                    return "Most Valuable Player";
                case "OPOY":
                    return "Offensive Player of the Year";
                case "DPOY":
                    return "Defensive Player of the Year";
                case "OROY":
                    return "Offensive Rookie of the Year";
                case "DROY":
                    return "Defensive Rookie of the Year";
                case "ProBowl":
                    return "Pro Bowl";
                default:
                    return awardType;
            }
        }
    }
}
