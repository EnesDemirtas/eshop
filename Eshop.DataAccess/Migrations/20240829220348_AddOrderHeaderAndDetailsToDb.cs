﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.DataAccess.Migrations;

/// <inheritdoc />
public partial class AddOrderHeaderAndDetailsToDb : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "OrderHeaders",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				ApplicationUserId = table.Column<string>(type: "TEXT", nullable: false),
				OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
				ShippingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
				OrderTotal = table.Column<double>(type: "REAL", nullable: false),
				OrderStatus = table.Column<string>(type: "TEXT", nullable: true),
				PaymentStatus = table.Column<string>(type: "TEXT", nullable: true),
				TrackingNumber = table.Column<string>(type: "TEXT", nullable: true),
				Carrier = table.Column<string>(type: "TEXT", nullable: true),
				PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
				PaymentDueDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
				PaymentIntentId = table.Column<string>(type: "TEXT", nullable: true),
				PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
				StreetAddress = table.Column<string>(type: "TEXT", nullable: false),
				City = table.Column<string>(type: "TEXT", nullable: false),
				State = table.Column<string>(type: "TEXT", nullable: false),
				PostalCode = table.Column<string>(type: "TEXT", nullable: false),
				Name = table.Column<string>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_OrderHeaders", x => x.Id);
				table.ForeignKey(
					name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
					column: x => x.ApplicationUserId,
					principalTable: "AspNetUsers",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "OrderDetails",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				OrderHeaderId = table.Column<int>(type: "INTEGER", nullable: false),
				ProductId = table.Column<int>(type: "INTEGER", nullable: false),
				Count = table.Column<int>(type: "INTEGER", nullable: false),
				Price = table.Column<double>(type: "REAL", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_OrderDetails", x => x.Id);
				table.ForeignKey(
					name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
					column: x => x.OrderHeaderId,
					principalTable: "OrderHeaders",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "FK_OrderDetails_Products_ProductId",
					column: x => x.ProductId,
					principalTable: "Products",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "IX_OrderDetails_OrderHeaderId",
			table: "OrderDetails",
			column: "OrderHeaderId");

		migrationBuilder.CreateIndex(
			name: "IX_OrderDetails_ProductId",
			table: "OrderDetails",
			column: "ProductId");

		migrationBuilder.CreateIndex(
			name: "IX_OrderHeaders_ApplicationUserId",
			table: "OrderHeaders",
			column: "ApplicationUserId");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "OrderDetails");

		migrationBuilder.DropTable(
			name: "OrderHeaders");
	}
}
