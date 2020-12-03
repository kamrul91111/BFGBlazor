using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BFGBlazor.Data.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Residential",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    RateType = table.Column<int>(nullable: false),
                    RepaymentType = table.Column<int>(nullable: false),
                    PropertyUse = table.Column<int>(nullable: false),
                    BaseInitialRate = table.Column<double>(nullable: false),
                    InitialRate = table.Column<double>(nullable: false),
                    DiscountInitialRate = table.Column<double>(nullable: false),
                    BaseRevertRate = table.Column<double>(nullable: false),
                    RevertRate = table.Column<double>(nullable: false),
                    DiscountRevertRate = table.Column<double>(nullable: false),
                    LvrMin = table.Column<double>(nullable: false),
                    LvrMax = table.Column<long>(nullable: false),
                    LoanMin = table.Column<double>(nullable: false),
                    LoanMax = table.Column<double>(nullable: false),
                    TermMax = table.Column<long>(nullable: false),
                    FixedTerm = table.Column<long>(nullable: false),
                    IntroTerm = table.Column<long>(nullable: false),
                    IoMaxPeriod = table.Column<long>(nullable: false),
                    IoMinPeriod = table.Column<long>(nullable: false),
                    ProPackDiscount = table.Column<long>(nullable: false),
                    Lep = table.Column<long>(nullable: false),
                    ProPack = table.Column<int>(nullable: false),
                    Smsf = table.Column<int>(nullable: false),
                    CreditImpaired = table.Column<int>(nullable: false),
                    WhiteLabel = table.Column<int>(nullable: false),
                    LineOfCredit = table.Column<int>(nullable: false),
                    Construction = table.Column<int>(nullable: false),
                    LmiCapitalization = table.Column<int>(nullable: false),
                    LmiRequiredOver = table.Column<double>(nullable: false),
                    RedrawFacility = table.Column<int>(nullable: false),
                    AdditionalRepayments = table.Column<int>(nullable: false),
                    Offset = table.Column<int>(nullable: false),
                    SplitLoan = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residential", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeLoans",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Abbr = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    ResidentialId = table.Column<Guid>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeLoans_Residential_ResidentialId",
                        column: x => x.ResidentialId,
                        principalTable: "Residential",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComparisonRateEstimate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ComparisonRate = table.Column<double>(nullable: false),
                    HomeLoansId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComparisonRateEstimate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComparisonRateEstimate_HomeLoans_HomeLoansId",
                        column: x => x.HomeLoansId,
                        principalTable: "HomeLoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fee",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Value = table.Column<double>(nullable: false),
                    HomeLoansId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fee_HomeLoans_HomeLoansId",
                        column: x => x.HomeLoansId,
                        principalTable: "HomeLoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Gateway",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    HomeLoansId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gateway", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gateway_HomeLoans_HomeLoansId",
                        column: x => x.HomeLoansId,
                        principalTable: "HomeLoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lender",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Abbr = table.Column<string>(nullable: true),
                    Turnaround = table.Column<long>(nullable: false),
                    HomeLoansId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lender", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lender_HomeLoans_HomeLoansId",
                        column: x => x.HomeLoansId,
                        principalTable: "HomeLoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaximumBorrowingEstimate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaximumBorrowing = table.Column<double>(nullable: false),
                    HomeLoansId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaximumBorrowingEstimate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaximumBorrowingEstimate_HomeLoans_HomeLoansId",
                        column: x => x.HomeLoansId,
                        principalTable: "HomeLoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonRateEstimate_HomeLoansId",
                table: "ComparisonRateEstimate",
                column: "HomeLoansId");

            migrationBuilder.CreateIndex(
                name: "IX_Fee_HomeLoansId",
                table: "Fee",
                column: "HomeLoansId");

            migrationBuilder.CreateIndex(
                name: "IX_Gateway_HomeLoansId",
                table: "Gateway",
                column: "HomeLoansId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeLoans_ResidentialId",
                table: "HomeLoans",
                column: "ResidentialId");

            migrationBuilder.CreateIndex(
                name: "IX_Lender_HomeLoansId",
                table: "Lender",
                column: "HomeLoansId");

            migrationBuilder.CreateIndex(
                name: "IX_MaximumBorrowingEstimate_HomeLoansId",
                table: "MaximumBorrowingEstimate",
                column: "HomeLoansId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComparisonRateEstimate");

            migrationBuilder.DropTable(
                name: "Fee");

            migrationBuilder.DropTable(
                name: "Gateway");

            migrationBuilder.DropTable(
                name: "Lender");

            migrationBuilder.DropTable(
                name: "MaximumBorrowingEstimate");

            migrationBuilder.DropTable(
                name: "HomeLoans");

            migrationBuilder.DropTable(
                name: "Residential");
        }
    }
}
