namespace Mcc.Repository.Mongo
{
    public class MongoOptions
    {
        public const string Key = "mongo";
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
