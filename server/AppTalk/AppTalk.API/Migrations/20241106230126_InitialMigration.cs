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
                    password_hash = table.Column<string>(type: "text", nullable: false),
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
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servers", x => x.id);
                    table.ForeignKey(
                        name: "FK_servers_users_user_id",
                        column: x => x.user_id,
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
                    server_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_rooms_servers_server_id",
                        column: x => x.server_id,
                        principalSchema: "app_talk",
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "server_members",
                schema: "app_talk",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    server_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_server_members", x => x.id);
                    table.ForeignKey(
                        name: "FK_server_members_servers_server_id",
                        column: x => x.server_id,
                        principalSchema: "app_talk",
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_server_members_users_user_id",
                        column: x => x.user_id,
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
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_messages_rooms_room_id",
                        column: x => x.room_id,
                        principalSchema: "app_talk",
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messages_users_user_id",
                        column: x => x.user_id,
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
                name: "IX_messages_room_id",
                schema: "app_talk",
                table: "messages",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_user_id",
                schema: "app_talk",
                table: "messages",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_id",
                schema: "app_talk",
                table: "rooms",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rooms_server_id",
                schema: "app_talk",
                table: "rooms",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "IX_server_members_id",
                schema: "app_talk",
                table: "server_members",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_server_members_server_id",
                schema: "app_talk",
                table: "server_members",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "IX_server_members_user_id_server_id",
                schema: "app_talk",
                table: "server_members",
                columns: new[] { "user_id", "server_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_servers_id",
                schema: "app_talk",
                table: "servers",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_servers_user_id",
                schema: "app_talk",
                table: "servers",
                column: "user_id");

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
                name: "server_members",
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
