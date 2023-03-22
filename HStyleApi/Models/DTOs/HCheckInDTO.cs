using HStyleApi.Models.EFModels;

namespace HStyleApi.Models.DTOs
{
	public class HCheckInDTO
	{

		public int CheckInHId { get; set; }
		public int MemberId { get; set; }
		public int ActivityId { get; set; }
		public int CheckInTimes { get; set; }
		public DateTime LastTime { get; set; }
	}

	public static class HCheckInExts
	{
		public static HCheckInDTO ToHCheckInDTO(this HCheckIn source)
		{
			return new HCheckInDTO
			{
				MemberId = source.MemberId,
				ActivityId = source.ActivityId,
				CheckInTimes = source.CheckInTimes,
				LastTime = source.LastTime,
			};
		}
	}
}
