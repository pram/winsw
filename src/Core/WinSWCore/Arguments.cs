namespace winsw
{
    public class Arguments
    {
        public string Action { get; set; }
        public bool cliMode { get; set; } = true;
        public string username { get; set; }
        public string password { get; set; }
        public bool allowLoginAsService { get; set; }
    }
}