namespace TechTalkIntegrationTests.Domain.Models.Dtos.Tasks
{
    public abstract class BaseTaskDto
    {
        public string Description { get; set; }
        public int Priority { get; set; }
        public bool Completed { get; set; }
    }
}
