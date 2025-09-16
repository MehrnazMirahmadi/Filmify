﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filmify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReleaseDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Films",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Films");
        }
    }
}
