using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace HStyleApi.Controllers
{
	[EnableCors("AllowAny")]
	[Route("ws")]
	[ApiController]
	public class WebSocketController : ControllerBase
	{
		static ConcurrentDictionary<int, WebSocket> WebSockets = new ConcurrentDictionary<int, WebSocket>();

		//這邊用route有錯，不符合rest
		[HttpGet]
		public async Task Get()
		{
			if (HttpContext.WebSockets.IsWebSocketRequest)
			{
				using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
				await ProcessWebSocket(webSocket);
			}
			else
			{
				HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
			}
		}

		private async Task ProcessWebSocket(WebSocket webSocket)
		{
			WebSockets.TryAdd(webSocket.GetHashCode(), webSocket);
			var buffer = new byte[1024 * 4]; 
			var res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			string? UserName = null;
			while (!res.CloseStatus.HasValue)
			{
				UserName = "匿名";
				var cmd = Encoding.UTF8.GetString(buffer, 0, res.Count);
				JObject data = JObject.Parse(cmd);
				string? Name = Convert.ToString(data["userName"]);
				string? Message = $"{Convert.ToString(data["message"])} at {DateTime.Now}";
				if (!string.IsNullOrEmpty(Name))
				{
					UserName = Name;
				}
				Broadcast(JsonConvert.SerializeObject(new
				{
					userName = UserName,
					message = Message
				}));
				res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			}
			await webSocket.CloseAsync(res.CloseStatus.Value, res.CloseStatusDescription, CancellationToken.None);
			WebSockets.TryRemove(webSocket.GetHashCode(), out var removed); // removed允許未宣告
			Broadcast(JsonConvert.SerializeObject(new
			{
				userName = UserName,
				message = "離開聊天室"
			}));
		}

		private void Broadcast(string message)
		{
			var buffer = Encoding.UTF8.GetBytes(message);
			var data = new ArraySegment<byte>(buffer);
			Parallel.ForEach(WebSockets.Values, async (webSocket) =>
			{
				if (webSocket.State == WebSocketState.Open)
				{
					await webSocket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
				}
			});
		}
	}
}
