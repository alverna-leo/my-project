namespace JobProviderApp.Model
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Salary { get; set; }
        public int JobProviderId { get; set; } // Foreign Key
        public JobProvider JobProvider { get; set; }
    }
}
