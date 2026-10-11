using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartnerCommissions.Wallets.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                columns: table => new
                {
                    CommissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserExternalId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    EventExternalId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    AccruedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PaidAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.CommissionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_UserExternalId_PaidAt",
                table: "WalletTransactions",
                columns: new[] { "UserExternalId", "PaidAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletTransactions");
        }
    }
}
