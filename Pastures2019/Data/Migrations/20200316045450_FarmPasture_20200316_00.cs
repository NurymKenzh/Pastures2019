using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Pastures2019.Data.Migrations
{
    public partial class FarmPasture_20200316_00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FarmPasture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    tid = table.Column<int>(nullable: false),
                    CATOTE = table.Column<string>(nullable: true),
                    Farm = table.Column<string>(nullable: true),
                    NaturalArea = table.Column<string>(nullable: true),
                    PType = table.Column<string>(nullable: true),
                    Relief = table.Column<string>(nullable: true),
                    ThePresenceOfEconomicallySignificantContours = table.Column<string>(nullable: true),
                    LandAreaAccordingToLandUseAct = table.Column<string>(nullable: true),
                    ProjectiveCoverageFrom = table.Column<int>(nullable: true),
                    ProjectiveCoverageTo = table.Column<int>(nullable: true),
                    AveragePastureProductivitySpring = table.Column<decimal>(nullable: true),
                    AveragePastureProductivitySummer = table.Column<decimal>(nullable: false),
                    AveragePastureProductivityAutumn = table.Column<decimal>(nullable: false),
                    TypeOfGrazedAnimalsBreed = table.Column<string>(nullable: true),
                    TheNumberOfGrazedAnimalsGoals = table.Column<string>(nullable: true),
                    NumberOfGrazingDaysSpring = table.Column<int>(nullable: true),
                    NumberOfGrazingDaysSummer = table.Column<int>(nullable: true),
                    NumberOfGrazingDaysFall = table.Column<int>(nullable: true),
                    FloodingEatSourcesWells = table.Column<string>(nullable: true),
                    TheNeedForPastureFeedSpring = table.Column<int>(nullable: true),
                    TheNeedForPastureFeedSummer = table.Column<int>(nullable: true),
                    TheNeedForPastureFeedAutumn = table.Column<int>(nullable: true),
                    FeedStockOfUsedPasturesSpring = table.Column<int>(nullable: true),
                    FeedStockOfUsedPasturesSummer = table.Column<int>(nullable: true),
                    FeedStockOfUsedPasturesAutumn = table.Column<int>(nullable: true),
                    TheCostPer1HeadForTheBillingPeriodSpring = table.Column<decimal>(nullable: true),
                    TheCostPer1HeadForTheBillingPeriodSummer = table.Column<decimal>(nullable: true),
                    TheCostPer1HeadForTheBillingPeriodFall = table.Column<decimal>(nullable: true),
                    LoadSpring = table.Column<decimal>(nullable: true),
                    LoadSummer = table.Column<decimal>(nullable: true),
                    LoadFall = table.Column<decimal>(nullable: true),
                    ShortageSurplusOfPastureFeedSpring = table.Column<int>(nullable: true),
                    ShortageSurplusOfPastureFeedSummer = table.Column<int>(nullable: true),
                    ShortageSurplusOfPastureFeedAutumn = table.Column<int>(nullable: true),
                    RequiredAdditionalAreaIfNecessaryPasturesSpring = table.Column<int>(nullable: true),
                    RequiredAdditionalAreaIfNecessaryPasturesSummer = table.Column<int>(nullable: true),
                    RequiredAdditionalAreaIfNecessaryPasturesAutumn = table.Column<int>(nullable: true),
                    ThePresenceOfDegradedSitesIfAvailable = table.Column<string>(nullable: true),
                    RecommendationsForImprovingThisDegradedArea = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmPasture", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarmPasture");
        }
    }
}
