using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GolobeTravelApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblHotelEntities",
                columns: table => new
                {
                    EntityId = table.Column<string>(type: "text", nullable: false),
                    EntityName = table.Column<string>(type: "text", nullable: true),
                    EntityLocation = table.Column<string>(type: "text", nullable: true),
                    EntityHierarchy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblHotelEntities", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "TblUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CognitoId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblHotelData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityId = table.Column<string>(type: "text", nullable: false),
                    HotelId = table.Column<string>(type: "text", nullable: false),
                    HotelDetailsId = table.Column<string>(type: "text", nullable: false),
                    HotelName = table.Column<string>(type: "text", nullable: true),
                    HotelStars = table.Column<int>(type: "integer", nullable: true),
                    HotelLongitude = table.Column<double>(type: "double precision", nullable: true),
                    HotelLatitude = table.Column<double>(type: "double precision", nullable: true),
                    HotelDistance = table.Column<string>(type: "text", nullable: true),
                    HotelRelevantPoiDistance = table.Column<string>(type: "text", nullable: true),
                    HotelReviewsScore = table.Column<double>(type: "double precision", nullable: true),
                    HotelReviewsTotal = table.Column<int>(type: "integer", nullable: true),
                    HotelReviewsDesc = table.Column<string>(type: "text", nullable: true),
                    HotelLowestPrice = table.Column<double>(type: "double precision", nullable: true),
                    HotelPartnerName = table.Column<string>(type: "text", nullable: true),
                    HotelImageUrls = table.Column<string>(type: "text", nullable: true),
                    HotelAdditionalImageUrls = table.Column<string>(type: "text", nullable: true),
                    HotelHotelDescription = table.Column<string>(type: "text", nullable: true),
                    HotelAmenities = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    HotelCheckinTime = table.Column<string>(type: "text", nullable: true),
                    HotelCheckoutTime = table.Column<string>(type: "text", nullable: true),
                    HotelNation = table.Column<string>(type: "text", nullable: true),
                    HotelCity = table.Column<string>(type: "text", nullable: true),
                    HotelStreetAddress = table.Column<string>(type: "text", nullable: true),
                    HotelDistrict = table.Column<string>(type: "text", nullable: true),
                    HotelPostcode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblHotelData", x => x.Id);
                    table.UniqueConstraint("AK_TblHotelData_HotelId", x => x.HotelId);
                    table.ForeignKey(
                        name: "FK_TblHotelData_TblHotelEntities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "TblHotelEntities",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblUserBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Guests = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblUserBookings_TblUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblUserFavorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUserFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblUserFavorites_TblUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblHotelBookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Nights = table.Column<int>(type: "integer", nullable: false),
                    HotelId = table.Column<string>(type: "text", nullable: false),
                    CheckinDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CheckoutDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Adults = table.Column<int>(type: "integer", nullable: false),
                    Children = table.Column<int>(type: "integer", nullable: false),
                    Rooms = table.Column<int>(type: "integer", nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ServiceFee = table.Column<decimal>(type: "numeric", nullable: false),
                    Taxes = table.Column<decimal>(type: "numeric", nullable: false),
                    NightlyRate = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblHotelBookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_TblHotelBookings_TblHotelData_HotelId",
                        column: x => x.HotelId,
                        principalTable: "TblHotelData",
                        principalColumn: "HotelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblHotelBookings_TblUser_UserId",
                        column: x => x.UserId,
                        principalTable: "TblUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblHotelReviews",
                columns: table => new
                {
                    ReviewId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ReviewUserType = table.Column<string>(type: "text", nullable: false),
                    ReviewScoreDesc = table.Column<string>(type: "text", nullable: true),
                    ReviewTitle = table.Column<string>(type: "text", nullable: true),
                    ReviewText = table.Column<string>(type: "text", nullable: true),
                    RewviewCommentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HotelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblHotelReviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_TblHotelReviews_TblHotelData_HotelId",
                        column: x => x.HotelId,
                        principalTable: "TblHotelData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblHotelBookings_HotelId",
                table: "TblHotelBookings",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_TblHotelBookings_UserId",
                table: "TblHotelBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TblHotelData_EntityId",
                table: "TblHotelData",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TblHotelReviews_HotelId",
                table: "TblHotelReviews",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_TblUserBookings_UserId",
                table: "TblUserBookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TblUserFavorites_UserId",
                table: "TblUserFavorites",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblHotelBookings");

            migrationBuilder.DropTable(
                name: "TblHotelReviews");

            migrationBuilder.DropTable(
                name: "TblUserBookings");

            migrationBuilder.DropTable(
                name: "TblUserFavorites");

            migrationBuilder.DropTable(
                name: "TblHotelData");

            migrationBuilder.DropTable(
                name: "TblUser");

            migrationBuilder.DropTable(
                name: "TblHotelEntities");
        }
    }
}
