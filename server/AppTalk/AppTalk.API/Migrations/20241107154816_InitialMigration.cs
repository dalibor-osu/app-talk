using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppTalk.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";");
            
            migrationBuilder.EnsureSchema(
                name: "app_talk");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "app_talk",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    passwordHash = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "servers",
                schema: "app_talk",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servers", x => x.id);
                    table.ForeignKey(
                        name: "FK_servers_users_userId",
                        column: x => x.userId,
                        principalSchema: "app_talk",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                schema: "app_talk",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    serverId = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_rooms_servers_serverId",
                        column: x => x.serverId,
                        principalSchema: "app_talk",
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "serverMembers",
                schema: "app_talk",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    serverId = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serverMembers", x => x.id);
                    table.ForeignKey(
                        name: "FK_serverMembers_servers_serverId",
                        column: x => x.serverId,
                        principalSchema: "app_talk",
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_serverMembers_users_userId",
                        column: x => x.userId,
                        principalSchema: "app_talk",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                schema: "app_talk",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    roomId = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_messages_rooms_roomId",
                        column: x => x.roomId,
                        principalSchema: "app_talk",
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messages_users_userId",
                        column: x => x.userId,
                        principalSchema: "app_talk",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_messages_id",
                schema: "app_talk",
                table: "messages",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_messages_roomId",
                schema: "app_talk",
                table: "messages",
                column: "roomId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_userId",
                schema: "app_talk",
                table: "messages",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_id",
                schema: "app_talk",
                table: "rooms",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rooms_serverId",
                schema: "app_talk",
                table: "rooms",
                column: "serverId");

            migrationBuilder.CreateIndex(
                name: "IX_serverMembers_id",
                schema: "app_talk",
                table: "serverMembers",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_serverMembers_serverId",
                schema: "app_talk",
                table: "serverMembers",
                column: "serverId");

            migrationBuilder.CreateIndex(
                name: "IX_serverMembers_userId_serverId",
                schema: "app_talk",
                table: "serverMembers",
                columns: new[] { "userId", "serverId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_servers_id",
                schema: "app_talk",
                table: "servers",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_servers_userId",
                schema: "app_talk",
                table: "servers",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                schema: "app_talk",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_id",
                schema: "app_talk",
                table: "users",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                schema: "app_talk",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages",
                schema: "app_talk");

            migrationBuilder.DropTable(
                name: "serverMembers",
                schema: "app_talk");

            migrationBuilder.DropTable(
                name: "rooms",
                schema: "app_talk");

            migrationBuilder.DropTable(
                name: "servers",
                schema: "app_talk");

            migrationBuilder.DropTable(
                name: "users",
                schema: "app_talk");
        }
    }
}
