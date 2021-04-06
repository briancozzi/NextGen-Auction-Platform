using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class add_biddingplatform_entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    UniqueId = table.Column<Guid>(nullable: false),
                    Address1 = table.Column<string>(maxLength: 256, nullable: false),
                    Address2 = table.Column<string>(maxLength: 256, nullable: true),
                    City = table.Column<string>(maxLength: 25, nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    ZipCode = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AuctionBidders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UniqueId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    AuctionId = table.Column<int>(nullable: false),
                    BidderName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionBidders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionBidders_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UniqueId = table.Column<Guid>(nullable: false),
                    CategoryName = table.Column<string>(nullable: false),
                    CategoryType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UniqueId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    AuctionHistoryId = table.Column<int>(nullable: false),
                    InvoiceStatus = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UniqueId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 64, nullable: false),
                    LastName = table.Column<string>(maxLength: 64, nullable: false),
                    PhoneNo = table.Column<string>(nullable: false),
                    Logo = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAccounts_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UniqueId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    CreditCardNo = table.Column<string>(nullable: false),
                    CVV = table.Column<string>(nullable: false),
                    ExpiryMonth = table.Column<string>(nullable: false),
                    ExpiryYear = table.Column<string>(nullable: false),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardDetails_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardDetails_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuctionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UniqueId = table.Column<Guid>(nullable: false),
                    AuctionBidderId = table.Column<int>(nullable: false),
                    AuctionItemId = table.Column<int>(nullable: false),
                    BidAmount = table.Column<double>(nullable: false),
                    BidStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionHistory_AuctionBidders_AuctionBidderId",
                        column: x => x.AuctionBidderId,
                        principalTable: "AuctionBidders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UniqueId = table.Column<Guid>(nullable: false),
                    ItemType = table.Column<string>(nullable: false),
                    ItemNumber = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    ProcurementState = table.Column<string>(nullable: true),
                    Visibility = table.Column<string>(nullable: true),
                    TypeOfDonor = table.Column<string>(nullable: false),
                    DonorUserId = table.Column<long>(nullable: false),
                    DisplayNameOnItem = table.Column<string>(nullable: true),
                    TypeOfSolicitor = table.Column<string>(nullable: true),
                    SolicitorUserId = table.Column<long>(nullable: false),
                    FairMarketValue_FMV = table.Column<double>(nullable: false),
                    StartingBidValue = table.Column<double>(nullable: false),
                    BidStepIncrementValue = table.Column<double>(nullable: false),
                    AcquisitionValue = table.Column<double>(nullable: false),
                    BuyNowPrice = table.Column<double>(nullable: false),
                    ItemCertificateNotes = table.Column<string>(nullable: true),
                    MainImageName = table.Column<string>(nullable: false),
                    VideoLink = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppAccountEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UniqueId = table.Column<Guid>(nullable: false),
                    AppAccountId = table.Column<int>(nullable: false),
                    EventName = table.Column<string>(maxLength: 256, nullable: false),
                    EventDate = table.Column<DateTime>(nullable: false),
                    EventStartDateTime = table.Column<DateTime>(nullable: false),
                    EventEndDateTime = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    MobileNo = table.Column<string>(nullable: true),
                    AddressId = table.Column<int>(nullable: false),
                    EventUrl = table.Column<string>(nullable: true),
                    TimeZone = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAccountEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAccountEvents_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppAccountEvents_AppAccounts_AppAccountId",
                        column: x => x.AppAccountId,
                        principalTable: "AppAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    UniqueId = table.Column<Guid>(nullable: false),
                    InvoiceId = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<string>(nullable: false),
                    PaymentMethod = table.Column<string>(nullable: false),
                    CardDetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_CardDetails_CardDetailId",
                        column: x => x.CardDetailId,
                        principalTable: "CardDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ItemGallery",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    UniqueId = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    ImageName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGallery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemGallery_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UniqueId = table.Column<Guid>(nullable: false),
                    AppAccountId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false),
                    AuctionType = table.Column<string>(nullable: true),
                    AuctionStartDateTime = table.Column<DateTime>(nullable: false),
                    AuctionEndDateTime = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auctions_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auctions_AppAccounts_AppAccountId",
                        column: x => x.AppAccountId,
                        principalTable: "AppAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Auctions_AppAccountEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "AppAccountEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AuctionItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UniqueId = table.Column<Guid>(nullable: false),
                    AuctionId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionItems_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_StateId",
                table: "Addresses",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UniqueId",
                table: "Addresses",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppAccountEvents_AddressId",
                table: "AppAccountEvents",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAccountEvents_AppAccountId",
                table: "AppAccountEvents",
                column: "AppAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAccountEvent_UniqueId",
                table: "AppAccountEvents",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppAccounts_AddressId",
                table: "AppAccounts",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAccount_UniqueId",
                table: "AppAccounts",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionHistory_UniqueId",
                table: "AuctionBidders",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionBidders_UserId",
                table: "AuctionBidders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionHistory_AuctionBidderId",
                table: "AuctionHistory",
                column: "AuctionBidderId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionHistory_UniqueId",
                table: "AuctionHistory",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItems_AuctionId",
                table: "AuctionItems",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItems_ItemId",
                table: "AuctionItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItem_UniqueId",
                table: "AuctionItems",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_AddressId",
                table: "Auctions",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_AppAccountId",
                table: "Auctions",
                column: "AppAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_EventId",
                table: "Auctions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Auction_UniqueId",
                table: "Auctions",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_AddressId",
                table: "CardDetails",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_CardDetail_UniqueId",
                table: "CardDetails",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_UserId",
                table: "CardDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_UniqueId",
                table: "Categories",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_UniqueId",
                table: "Invoices",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UserId",
                table: "Invoices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGallery_ItemId",
                table: "ItemGallery",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGallery_UniqueId",
                table: "ItemGallery",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_UniqueId",
                table: "Items",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_CardDetailId",
                table: "PaymentTransactions",
                column: "CardDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_InvoiceId",
                table: "PaymentTransactions",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransaction_UniqueId",
                table: "PaymentTransactions",
                column: "UniqueId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuctionHistory");

            migrationBuilder.DropTable(
                name: "AuctionItems");

            migrationBuilder.DropTable(
                name: "ItemGallery");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "AuctionBidders");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "CardDetails");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "AppAccountEvents");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AppAccounts");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
