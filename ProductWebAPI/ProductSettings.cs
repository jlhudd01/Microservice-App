namespace ProductWebAPI
{
    public class ProductSettings
    {
        public bool UseCustomiztionData { get; set;}
        public string ConnectionString { get; set; }
        public string EventBusConnection { get; set; }
        public int GracePeriodTime {get; set;}
        public int CheckUpdateTime {get;set;}
    }
}