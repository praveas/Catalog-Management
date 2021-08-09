namespace Catalog.Settings
{
    
    public class MongoDbSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }

        // take a advantage to calculate the connection string that needed in order to talk to mongodb

        public string User { get; set; }
        public string Password {get; set; }
        public string ConnectionString 
        { 
            get
            {
                return $"mongodb://{User}:{Password}@{Host}:{Port}";
                // this is the syntax mongodb is expecting from us
            }
        }
    }
}
