namespace AppTalk.Models.Database;

public static class Constants
{
    public static class UsersTable
    {
        public const string TableName = "users";
        public const string Prefix = "usr";

        public const string PasswordHash = "password_hash";
        public const string Email = "email";
    }

    public static class MessagesTable
    {
        public const string TableName = "messages";
        public const string Prefix = "msg";

        public const string Content = "content";
        public const string RoomId = "room_id";
        
        public const int MaxContentLength = 2048;
    }

    public static class RoomsTable
    {
        public const string TableName = "rooms";
        public const string Prefix = "rms";
    }

    public static class ServersTable
    {
        public const string TableName = "servers";
        public const string Prefix = "srvs";
    }

    public static class ServerMembersTable
    {
        public const string TableName = "server_members";
        public const string Prefix = "srvm";
    }
}