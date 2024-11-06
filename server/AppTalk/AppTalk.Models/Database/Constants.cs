namespace AppTalk.Models.Database;

public static class Constants
{
    public static class UserTable
    {
        public const string TableName = "users";
        public const string Prefix = "usr";

        public const string Username = "username";
        public const string PasswordHash = "password_hash";
        public const string Email = "email";
    }

    public static class MessageTable
    {
        public const string TableName = "messages";
        public const string Prefix = "msg";

        public const string Content = "content";
        public const string RoomId = "room_id";
    }

    public static class RoomTable
    {
        public const string TableName = "rooms";
        public const string Prefix = "rms";
    }

    public static class ServerTable
    {
        public const string TableName = "servers";
        public const string Prefix = "srvs";
    }

    public static class ServerMemberTable
    {
        public const string TableName = "server_members";
        public const string Prefix = "srvm";
    }
}