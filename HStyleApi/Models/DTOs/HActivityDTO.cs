using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
    public class HActivityDTO
    {
        public int HActivityId { get; set; }
        public string? ActivityName { get; set; }
        public string? ActivityDescribe { get; set; }
        public int HValue { get; set; }
    }

    public static class HActivityDTOExts
    {
        public static HActivityDTO ToDTO(this HActivity source)
        {
            return new HActivityDTO
            {
                HActivityId = source.HActivityId,
                ActivityName = source.ActivityName,
                ActivityDescribe = source.ActivityDescribe,
                HValue = source.HValue
            };
        }
    }
}
