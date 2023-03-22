using HStyleApi.Models.InfraStructures.NewEBpay;
using HStyleApi.Models.InfraStructures.Repositories;
using HStyleApi.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Web.Http;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace HStyleApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NewECpayController : Controller
	{
		private readonly OrderService _orderService;
		public NewECpayController(Models.EFModels.AppDbContext db)
		{
			_orderService = new OrderService(new OrderRepo(db));
		}
		[HttpPost("SetData")]
		public IActionResult SetChargeData(int orderId)
		{
			var orderInfo = _orderService.GetOrder(orderId);

			// 整理金流串接資料
			// 加密用金鑰
			string hashKey = "U9Ll8Qn6Tb6IoTceWv5VgES8cnYx4y1Q";
			string hashIV = "CQqaZJjVmSCz6s7P";

			// 金流接收必填資料
			string merchantID = "MS148335860";
			string tradeInfo = "";
			string tradeSha = "";
			string version = "2.0"; // 參考文件串接程式版本

			// tradeInfo 內容，導回的網址都需為 https 
			string respondType = "JSON"; // 回傳格式
			string timeStamp = ((int)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
			string merchantOrderNo = timeStamp + "_" + orderId; // 底線後方為訂單ID，解密比對用，不可重覆(規則參考文件)
			string amt = orderInfo.Total.ToString();
			string itemDesc = "HStyle精品服飾";
			string tradeLimit = "600"; // 交易限制秒數
			string notifyURL = " https://31bb-220-129-89-67.jp.ngrok.io/api/Paypal/return"; // NotifyURL 填後端接收藍新付款結果的 API 位置，如 : /api/users/getpaymentdata
			string returnURL = "http://localhost:5173/ordercomplete";  // 前端可用 Status: SUCCESS 來判斷付款成功，網址夾帶可拿來取得活動內容
			string email = "guitar1859@gmail.com"; // 通知付款完成用
			string loginType = "0"; // 0不須登入藍新金流會員

			// 將 model 轉換為List<KeyValuePair<string, string>>
			List<KeyValuePair<string, string>> tradeData = new List<KeyValuePair<string, string>>() {
			new KeyValuePair<string, string>("MerchantID", merchantID),
			new KeyValuePair<string, string>("RespondType", respondType),
			new KeyValuePair<string, string>("TimeStamp", timeStamp),
			new KeyValuePair<string, string>("Version", version),
			new KeyValuePair<string, string>("MerchantOrderNo", merchantOrderNo),
			new KeyValuePair<string, string>("Amt", amt),
			new KeyValuePair<string, string>("ItemDesc", itemDesc),
			new KeyValuePair<string, string>("TradeLimit", tradeLimit),
			new KeyValuePair<string, string>("NotifyURL", notifyURL),
			new KeyValuePair<string, string>("ReturnURL", returnURL),
			new KeyValuePair<string, string>("Email", email),
			new KeyValuePair<string, string>("LoginType", loginType)
			};

			// 將 List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
			var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
			// AES 加密
			tradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, hashKey, hashIV);
			// SHA256 加密
			tradeSha = CryptoUtil.EncryptSHA256($"HashKey={hashKey}&{tradeInfo}&HashIV={hashIV}");

			// 送出金流串接用資料，給前端送藍新用
			return Ok(new
			{
				Status = true,
				PaymentData = new
				{
					MerchantID = merchantID,
					TradeInfo = tradeInfo,
					TradeSha = tradeSha,
					Version = version
				}
			});
		}

		[HttpPost("GetData")]
		public HttpResponseMessage GetPaymentData(NewebPayReturn data)
		{
			// 付款失敗跳離執行
			var response = new HttpResponseMessage(HttpStatusCode.OK);
			if (!data.Status.Equals("SUCCESS")) return response;

			// 加密用金鑰
			string hashKey = "填入生成的 HashKey";
			string hashIV = "填入生成的 HashIV";
			// AES 解密
			string decryptTradeInfo = CryptoUtil.DecryptAESHex(data.TradeInfo, hashKey, hashIV);
			PaymentResult result = JsonConvert.DeserializeObject<PaymentResult>(decryptTradeInfo);
			// 取出交易記錄資料庫的訂單ID
			string[] orderNo = result.Result.MerchantOrderNo.Split('_');
			int logId = Convert.ToInt32(orderNo[1]);

			// 用取得的"訂單ID"修改資料庫此筆訂單的付款狀態為 true

			// 用取得的"訂單ID"寄出付款完成訂單成立，商品準備出貨通知信

			return response;
		}

	}
}
