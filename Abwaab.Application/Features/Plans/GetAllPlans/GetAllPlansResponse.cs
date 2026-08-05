namespace Abwaab.Application.Features.Plans.GetAllPlans
{
    public class GetAllPlansResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly ExpieryDate { get; set; }
        public int TempDurationInDays { get; set; }
        public int MaxPropertiesCountAtSameTime { get; set; }
        public int MaxStardPropertiesCountAtSameTime { get; set; }
        public int MaxImagesCount { get; set; }
        public int MaxVideosCount { get; set; }
    }
}
