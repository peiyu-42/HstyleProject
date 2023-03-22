using ScheduleWork.EFModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ScheduleWork
{
	internal class Program
	{
		private static AppDbContext _db = new AppDbContext();
		static void Main(string[] args)
		{
			DateTime today = DateTime.Now;
			// 發放生日H幣
			//HcoinForBirth(today);
			// 發放購物活動H幣
			//HcoinForOrder(today);

			// 影片的排程
			ChangeIsOnShelf(today);

			// 文章的排程
			ChangePon(today);

		}

		/// <summary>
		/// 發送H幣給當月生日的會員，若今年已發送則不會有紀錄，每天執行一次
		/// </summary>
		/// <param name="today">今天的日期</param>
		private static void HcoinForBirth(DateTime today)
		{
			// 生日活動Id
			int activityId = 2;
			// 生日活動發放的H幣
			int activity_value = _db.H_Activities
				.SingleOrDefault(a => a.H_Activity_Id.Equals(activityId))
				.H_Value;
			// 這個月份生日的會員
			var memberInBirth = _db.Members
				.Where(m => m.Birthday != null && m.Birthday.Value.Month == today.Month)
				.ToList();
			// 這個月份生日的會員的Id
			var memberInBirthId = memberInBirth.Select(m => m.Id);
			// 生日活動今年已發放過的紀錄
			var memberBirthInActivity = _db.H_Source_Details
				.Where(d => d.Activity_Id.Equals(activityId) && d.Event_Time.Year.Equals(today.Year))
				.ToList();

			// 這個月還沒發放過H幣的會員
			var memberNotHcoin = memberBirthInActivity.Select(m => m.Member_Id).Except(memberInBirthId);

			foreach (var member in memberInBirth)
			{
				//var sendHcoin = memberBirthInActivity.Where(a => a.Member_Id == member.Id);
				try
				{
					if (!memberBirthInActivity.Any(a => a.Member_Id == member.Id))
					{
						// 加入Member中的Total_H
						Member memberData = _db.Members.SingleOrDefault(m => m.Id == member.Id);
						memberData.Total_H += activity_value;
						_db.SaveChanges();

						// 新增一筆紀錄
						H_Source_Details detail = new H_Source_Details()
						{
							Member_Id = member.Id,
							Activity_Id = activityId,
							Difference_H = activity_value,
							Event_Time = today,
							Total_H_SoFar = memberData.Total_H
						};
						_db.H_Source_Details.Add(detail);
						_db.SaveChanges();

						Console.WriteLine(member.Account.ToString() + "獲得生日活動H幣");
					}				
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			}
		}

		/// <summary>
		/// 購物滿額，就送H幣，訂單結案且已送貨後30天發放，若有人退訂就手動新增刪除H幣的紀錄
		/// </summary>
		/// <param name="today">今天的時間</param>
		private static void HcoinForOrder(DateTime today)
		{
			//AppDbContext _db = new AppDbContext();
			// 購物額滿的金額
			int fullPriceId = 4;
			int fullPrice = _db.H_Activities.SingleOrDefault(a => a.H_Activity_Id == fullPriceId).H_Value;
			// 購物滿額發送的H幣，以購買金額的(多少)%來做為發放的金額
			int hCoinId = 5;
			double hCoinPercent = _db.H_Activities.SingleOrDefault(a => a.H_Activity_Id == hCoinId).H_Value / 100.0;
			// 已發送過H幣的訂單(三個月內)
			DateTime inDays = today.AddDays(-90);
			var orderInSources = _db.H_Source_Details
				.Where(d => d.Event_Time > inDays && d.Activity_Id.Equals(hCoinId))
				.ToList();
			// 取得符合條件(發貨時間一個月後，訂單狀態為已完成)的訂單資訊
			int orderState = 5; // 訂單狀態為"已完成"的Id
			DateTime beforeDays = Convert.ToDateTime(today.AddDays(-30).ToString("yyyy-MM-dd")); // 送貨時間為30天前
			var orders = _db.Orders
				.Where(o => DbFunctions.TruncateTime(o.ShippedDate).Value.CompareTo(beforeDays) < 1
				&& DbFunctions.TruncateTime(o.ShippedDate).Value.CompareTo(beforeDays) > -1
				&& o.Status_id.Equals(orderState))
				.ToList();

			foreach (var order in orders)
			{
				try
				{
					if (!orderInSources.Any(s => s.Remark.Contains("訂單編號:" + order.Order_id.ToString())))
					{
						// 取得會員資料
						Member memberData = _db.Members.SingleOrDefault(m => m.Id == order.Member_id);
						// 訂單金額
						int price = _db.Orders.SingleOrDefault(o => o.Order_id == order.Order_id).Total;
						if (memberData != null && price >= fullPrice)
						{
							// 計算發送的H幣，白金會員
							double calHCoin = price * hCoinPercent;
							// 依據會員等級發放
							if (memberData.Permission_Id == 1)
							{
								// 銀級會員
								calHCoin *= 0.5;
							}
							else if (memberData.Permission_Id == 2)
							{
								// 金級會員
								calHCoin *= 0.75;
							}
							int hCoin = (int)Math.Round(calHCoin, 0, MidpointRounding.AwayFromZero);
							// 加入Member中的Total_H						
							memberData.Total_H += hCoin;
							_db.SaveChanges();

							// 新增一筆紀錄
							H_Source_Details detail = new H_Source_Details()
							{
								Member_Id = order.Member_id,
								Activity_Id = hCoinId,
								Difference_H = hCoin,
								Event_Time = today,
								Total_H_SoFar = memberData.Total_H,
								Remark = "訂單編號:" + order.Order_id.ToString()
							};
							_db.H_Source_Details.Add(detail);
							_db.SaveChanges();

							Console.WriteLine($"訂單:{order.Order_id.ToString()}，已發送貨幣");
						}
					}
				}
				catch (Exception ex)
				{
					throw new Exception(ex.Message);
				}
			}
		}


		/// <summary>
		/// 檢查IsOnShelf欄位是否符合上架時間，若不符合則改動
		/// </summary>
		public static void ChangeIsOnShelf(DateTime today)
		{
			//AppDbContext _db = new AppDbContext();
			IEnumerable<Video> data = _db.Videos;

			foreach (Video video in data)
			{
				video.IsOnShelff = null;
				if (video.OnShelffTime == null && video.OffShelffTime == null)
				{
					video.IsOnShelff = true;
				}
				else if (video.OnShelffTime.HasValue && video.OffShelffTime == null)
				{
					if (today >= video.OnShelffTime)
					{
						video.IsOnShelff = true;
					}
					else
					{
						video.IsOnShelff = false;
					}
				}
				else if (video.OnShelffTime == null && video.OffShelffTime.HasValue)
				{
					if (today < video.OffShelffTime)
					{
						video.IsOnShelff = true;
					}
					else
					{
						video.IsOnShelff = false;
					}
				}
				else
				{
					if (today >= video.OnShelffTime && today < video.OffShelffTime)
					{
						video.IsOnShelff = true;
					}
					else
					{
						video.IsOnShelff = false;
					}
				}
			}
			_db.SaveChanges();
			Console.WriteLine("已完成上下架排程");

			Console.WriteLine("已上架影片:") ;
			var videoDataOn = data.Where(v => v.IsOnShelff == true).Select(v => v.Title).ToList();
			foreach (var item in videoDataOn)
			{
				Console.WriteLine(item);
			}
			var videoDataOff = data.Where(v => v.IsOnShelff == true).Select(v => v.Title).ToList();

			Console.WriteLine("已下架影片:");
			foreach (var item in videoDataOff)
			{
				Console.WriteLine(item);
			}
			Console.ReadKey();
		}

		/// <summary>
		/// 檢查PON欄位是否符合上架時間，若不符合則改動
		/// </summary>
		public static void ChangePon(DateTime today)
		{
			IEnumerable<Essay> data = _db.Essays;

			foreach (Essay essay in data)
			{
				essay.PON = null;
				if (essay.UpLoad == null && essay.Removed == null)
				{
					essay.PON = true;
				}
				else if (essay.UpLoad.HasValue && essay.Removed == null)
				{
					if (today >= essay.UpLoad)
					{
						essay.PON = true;
					}
					else
					{
						essay.PON = false;
					}

				}
				else if (essay.UpLoad == null && essay.Removed.HasValue)
				{
					if (today < essay.Removed)
					{
						essay.PON = true;
					}
					else
					{
						essay.PON = false;
					}
				}
				else
				{
					if (today >= essay.UpLoad && today < essay.Removed)
					{
						essay.PON = true;
					}
					else
					{
						essay.PON = false;
					}
				}

			}
			_db.SaveChanges();
		}
	}
}
