namespace PolicyManagementWebApi.Models
{
    public interface IMongoPolicyDbSettings
    {
        public string ConnectionString { get; set; }
        public string MongoPolicyDbName { get; set; }
        public string SearchPolicyCollectionName { get; set; }
    }
}
