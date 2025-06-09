using ArticlesTestTask.Services.Interfaces;

namespace ArticlesTestTask.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
