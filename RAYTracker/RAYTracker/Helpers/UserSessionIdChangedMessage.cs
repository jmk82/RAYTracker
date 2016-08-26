namespace RAYTracker.Helpers
{
    public class UserSessionIdChangedMessage
    {
        public string NewUserSessionId { get; set; }
        public object Sender{ get; set; }

        public UserSessionIdChangedMessage(string id, object sender)
        {
            NewUserSessionId = id;
            Sender = sender;
        }
    }
}
