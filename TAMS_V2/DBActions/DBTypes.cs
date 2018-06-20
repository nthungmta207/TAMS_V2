namespace Database
{
    public struct ConnectionStringType
    {
        private bool WindowAuthencation;
        public bool IsWindowAuthencation
        {
            get { return WindowAuthencation; }
            set { WindowAuthencation = value; }
        }

        private bool Attachment;
        public bool IsAttachment
        {
            get { return Attachment; }
            set { Attachment = value; }
        }

        private string Servername ;
        public string Server
        {
            get { return Servername; }
            set { Servername = value; }
        }

        private string InstanceName;
        public string Instance
        {
            get { return InstanceName; }
            set { InstanceName = value; }
        }   

        private string Databasename ;
        public string Database
        {
            get {return Databasename;}
            set {Databasename = value;}
        }

        private string UserName ;
        public string User
        {
            get {return UserName;}
            set { UserName = value; }
        }

        private string Password ;
        public string Pass
        {
            get { return Password; }
            set { Password = value; }
        }

        private string PortNumber;
        public string Port
        {
            get { return PortNumber; }
            set { PortNumber = value; }
        }

        private string timeOut;
        public string TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }        
    }

    internal class ListItem
    {
        private string Fname;
        private string Fvalue;

        public string Name
        {
            set { Fname = value; }
            get { return Fname; }
        }
        public string Value
        {
            set { Fvalue = value; }
            get { return Fvalue; }
        }
    }
}