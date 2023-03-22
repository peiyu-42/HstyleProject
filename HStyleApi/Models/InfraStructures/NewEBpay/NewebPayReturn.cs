namespace HStyleApi.Models.InfraStructures.NewEBpay
{
	public class NewebPayReturn
	{
		public string Status { get; set; }
		public string MerchantID { get; set; }
		public string Version { get; set; }
		public string TradeInfo { get; set; }
		public string TradeSha { get; set; }
	}
	public class PaymentResult
	{
		public string Status { get; set; }
		public string Message { get; set; }
		public Result Result { get; set; }
	}

	public class Result
	{
		public string MerchantID { get; set; }
		public string Amt { get; set; }
		public string TradeNo { get; set; }
		public string MerchantOrderNo { get; set; }
		public string RespondType { get; set; }
		public string IP { get; set; }
		public string EscrowBank { get; set; }
		public string PaymentType { get; set; }
		public string RespondCode { get; set; }
		public string Auth { get; set; }
		public string Card6No { get; set; }
		public string Card4No { get; set; }
		public string Exp { get; set; }
		public string TokenUseStatus { get; set; }
		public string InstFirst { get; set; }
		public string InstEach { get; set; }
		public string Inst { get; set; }
		public string ECI { get; set; }
		public string PayTime { get; set; }
		public string PaymentMethod { get; set; }
	}
}
