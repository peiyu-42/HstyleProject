using HStyleApi.Models.EFModels;
using HStyleApi.Models.InfraStructures.Repositories;
using HStyleApi.Models.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System.Net;

namespace HStyleApi.Controllers
{
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
	[ApiController]
	public class PaypalController : ControllerBase
	{
		private readonly IConfiguration _config;
		private readonly OrderService _orderService;

		public PaypalController(IConfiguration config, AppDbContext db)
		{
			_config = config;
			_orderService = new OrderService(new OrderRepo(db));
		}

		[Authorize]
		[HttpPost("{orderId}")]
		public async Task<IActionResult> CreateOrder(int orderId)
		{
			
			var orderInfo = _orderService.GetOrder(orderId);
			//client是指我們申請的商城帳號，由appsetting設定，注入_config，包含ClientId和Secret
			var client = new PayPalHttpClient(new SandboxEnvironment(
				_config["PayPal:ClientId"],
				_config["PayPal:ClientSecret"]
			));

			var request = new OrdersCreateRequest();
			request.Prefer("return=representation");//這句表示請求發給paypal時，要返回詳細資訊而不是只有付款url
			request.RequestBody(BuildRequestBody(orderInfo.Total));

			var response = await client.Execute(request);
			

			if (response.StatusCode != System.Net.HttpStatusCode.Created)
			{
                return BadRequest();
			}

			var result = response.Result<PayPalCheckoutSdk.Orders.Order>();
			
			var paypalId = result.Id;
			var paypalLink = result.Links
					.Where(x => x.Rel == "approve")
				.Select(x => x.Href).FirstOrDefault();
			

			
			_orderService.SavePayInfo(orderId, paypalId);
			return Ok(new
			{
				paypalId,
				paypalLink
			});
		}

		private OrderRequest BuildRequestBody(int total)
		{
			var order = new OrderRequest()
			{
				CheckoutPaymentIntent = "CAPTURE",//表示消費者按下付款後會立即處理，AUTHORIZE則是僅授權，之後再手動處理
				ApplicationContext = new ApplicationContext
				{
					ReturnUrl = "https://localhost:7243/api/Paypal/return",
					CancelUrl = "https://localhost:7243/api/Paypal/cancel",
					Locale = "zh-TW",

				},
				PurchaseUnits = new List<PurchaseUnitRequest>
				{
					new PurchaseUnitRequest
					{
						ReferenceId = "test1",//識別用，可以亂取
						Description = "HStyle1",//顯示給消費者看得描述
						AmountWithBreakdown = new AmountWithBreakdown
						{
							CurrencyCode = "TWD",
							Value = total.ToString()
						}
					}
				}
			
			};
			return order;
		}

		[HttpGet("return")]
		public async Task<IActionResult> Return([FromQuery] string token, [FromQuery] string PayerID)
		{
			var client = new PayPalHttpClient(new SandboxEnvironment(
				_config["PayPal:ClientId"],
				_config["PayPal:ClientSecret"]
			));

			var request = new OrdersCaptureRequest(token);
			request.RequestBody(new OrderActionRequest());

			var response = await client.Execute(request);

			if (response.StatusCode != System.Net.HttpStatusCode.Created)
			{
				return BadRequest("付款失敗");
			}
			//todo紀錄付款人，修改訂單狀態，改向畫面網址

			_orderService.UpdateOrder(token);
			return Redirect("http://localhost:5173/ordercomplete");
		}

		[HttpGet("cancel")]
		public IActionResult Cancel()
		{
			//todo導向訂單頁面
			return Redirect("http://localhost:5173/account/orders");
		}

		//[HttpPost("{orderId}")]
		//public async Task<IActionResult> ConfirmOrder(string orderId)
		//{
		//	var client = new PayPalHttpClient(new SandboxEnvironment(
		//		_config["PayPal:ClientId"],
		//		_config["PayPal:ClientSecret"]
		//	));
		//	var request = new OrdersCaptureRequest(orderId);
		//	request.RequestBody(new OrderActionRequest());

		//	try
		//	{
		//		var response = await client.Execute(request);

		//		if (response.StatusCode == HttpStatusCode.Created)
		//		{
		//			// 付款成功，更新訂單狀態

		//			return Ok("付款成功");
		//		}
		//		else
		//		{
		//			// 付款失敗
		//			return BadRequest();
		//		}
		//	}
		//	catch
		//	{
		//		return NotFound("找不到新的付款紀錄，您尚未付款或紀錄已存檔");
		//	};
		//}

		//private async Task RegisterWebhook()
		//{
		//	try
		//	{
		//		var request = new WebhooksCreateRequest();
		//		request.RequestBody(new Webhook()
		//		{
		//			Url = "https://your-app.com/paypal-webhook", // Replace with your webhook URL
		//			EventTypes = new List<WebhookEventType>()
		//			{
		//				new WebhookEventType()
		//				{
		//					Name = "CHECKOUT.ORDER.APPROVED"
		//				}
		//			}
		//		});
		//		var response = await _client.Execute(request);
		//		var statusCode = response.StatusCode;
		//		var result = await response.Result<Webhook>();

		//		if (statusCode == HttpStatusCode.Created)
		//		{
		//			// Webhook registered successfully
		//		}
		//		else
		//		{
		//			// Webhook registration failed
		//		}
		//	}
		//	catch (HttpException ex)
		//	{
		//		throw new Exception(ex.Message);
		//	}
		//}

		//[HttpPost("Webhook")]
		//public IActionResult PayPalWebhook()
		//{
		//	var webhookId = Request.Headers["Paypal-Transmission-Id"];
		//	var webhookEvent = Request.Headers["Paypal-Transmission-Event"];
		//	var webhookTime = Request.Headers["Paypal-Transmission-Time"];

		//	// Process the webhook event here
		//	// ...

		//	return Ok();
		//}

	}
}
