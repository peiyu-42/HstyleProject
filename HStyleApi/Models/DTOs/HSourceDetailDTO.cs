using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
    public class HSourceDetailDTO
    {
        public int SourceHId { get; set; }
        public int MemberId { get; set; }
        public int ActivityId { get; set; }
        public int DifferenceH { get; set; }
        public DateTime EventTime { get; set; }
        public int? TotalHSoFar { get; set; }
        public string? Remark { get; set; }
        public int? EmployeeId { get; set; }
    }

    public static class HSourceDetailDTOExts
    {
        public static HSourceDetailDTO ToDTO(this HSourceDetail source)
        {
            return new HSourceDetailDTO
            {
                SourceHId = source.SourceHId,
                MemberId = source.MemberId,
                ActivityId = source.ActivityId,
                DifferenceH = source.DifferenceH,
                EventTime = source.EventTime,
                TotalHSoFar = source.TotalHSoFar,
                Remark = source.Remark,
                EmployeeId = source.EmployeeId
            };
        }

        public static HSourceDetail ToDB(this HSourceDetailDTO dto)
        {
            return new HSourceDetail
            {
                SourceHId = dto.SourceHId,
                MemberId = dto.MemberId,
                ActivityId = dto.ActivityId,
                DifferenceH = dto.DifferenceH,
                EventTime = dto.EventTime,
                TotalHSoFar = dto.TotalHSoFar,
                Remark = dto.Remark,
                EmployeeId = dto.EmployeeId
            };
        }
    }
}
