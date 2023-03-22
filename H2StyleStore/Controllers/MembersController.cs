using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using H2StyleStore.Models.EFModels;
using H2StyleStore.Models.Services.Interfaces;
using H2StyleStore.Models.Services;
using H2StyleStore.Models.ViewModels;
using System.Web.Security;
using H2StyleStore.Models.Infrastructures.Repositories;
using H2StyleStore.Models.DTOs;
using H2StyleStore.filter;

namespace H2StyleStore.Controllers
{
	[AuthrizeHelper("3")]

	public class MembersController : Controller
    {
       private AppDbContext db = new AppDbContext();
        private IMemberRepository repository;
        private MemberService service;

        private H_ActivityService _activityService;
        private IH_ActivityRepository _repo;

        public MembersController()  
        {
            repository = new MemberRepository();
            service = new MemberService(repository);

            _repo = new H_ActivityRepository(db);
            _activityService = new H_ActivityService(_repo);
        }


        // GET: Members
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.PermissionsM);
            return View(members.ToList());
        }

        // GET: Members/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var service = new MemberService(repository);

            (bool IsSuccess, string ErrorMessage) response =
                service.CreateNewMember(model.ToRequestDto());

            if (response.IsSuccess)
            {
                // 建檔成功 redirect to confirm page
                return View("RegisterConfirm");                   //註冊成功就跳到這邊
            }
            else
            {
                ModelState.AddModelError(string.Empty, response.ErrorMessage);
                return View(model);
            }
        }

        public ActionResult ActiveRegister(int memberId, string confirmCode)
        {
            // var service = new MemberService(repository);
            service.ActiveRegister(memberId, confirmCode);

            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM model)   //12-26 2:05s Create LoginVM
        {
            // var service = new MemberService(repository);
            (bool IsSuccess, string ErrorMessage) response =
                service.Login(model.Account, model.Password);

            if (response.IsSuccess)
            {
                // 記住登入成功的會員
                var rememberMe = false;

                string returnUrl = ProcessLogin(model.Account, rememberMe, out HttpCookie cookie);

                Response.Cookies.Add(cookie);

                return Redirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, response.ErrorMessage);

            return this.View(model);
        }
		public ActionResult Logout()
		{
			Session.Abandon();
			FormsAuthentication.SignOut();
			return Redirect("/Members/Login");
		}


		// GET: Members/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.Permission_Id = new SelectList(db.PermissionsMs, "Permission_id", "Level");
            return View();
        }

        // POST: Members/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Account,Password,Phone_Number,Address,Gender,Birthday,Permission_Id,Jointime,Mail_verify,Mail_code,Total_H")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Permission_Id = new SelectList(db.PermissionsMs, "Permission_id", "Level", member.Permission_Id);
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.Permission_Id = new SelectList(db.PermissionsMs, "Permission_id", "Level", member.Permission_Id);
            return View(member);
        }

        // POST: Members/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Account,EncryptedPassword,Phone_Number,Address,Gender,Birthday,Permission_Id,Jointime,Mail_verify,Mail_code,Total_H")] Member member)// 密碼,生日會變成NULL
        {
            if (ModelState.IsValid)
            {
                var dbMember = db.Members.Where(x => x.Id == member.Id).SingleOrDefault();
                dbMember.Name = member.Name;
                dbMember.Email = member.Email;
                dbMember.Account = member.Account;
                dbMember.Phone_Number = member.Phone_Number;
                dbMember.Address = member.Address;
                dbMember.Gender = member.Gender;
                dbMember.Birthday = member.Birthday;
                dbMember.Mail_verify = member.Mail_verify;
                dbMember.Mail_code = member.Mail_code;
                dbMember.Total_H = member.Total_H;




                //db.Entry(member).State = EntityState.Modified;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Permission_Id = new SelectList(db.PermissionsMs, "Permission_id", "Level", member.Permission_Id);
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private string ProcessLogin(string account, bool rememberMe, out HttpCookie cookie)
        {
            var member = repository.GetByAccount(account);
            string roles = String.Empty; // 在本範例, 沒有用到角色權限,所以存入空白

            // 建立一張認證票
            FormsAuthenticationTicket ticket =
                new FormsAuthenticationTicket(
                    1,          // 版本別, 沒特別用處
                    account,
                    DateTime.Now,   // 發行日
                    DateTime.Now.AddMinutes(30), // 到期時間　　超過３０分鐘就會自動登出
                    rememberMe,     // 是否續存
                    roles,          // userdata
                    "/" // cookie位置
                );

            // 將它加密
            string value = FormsAuthentication.Encrypt(ticket);

            // 存入cookie
            cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

            // 取得return url
            string url = FormsAuthentication.GetRedirectUrl(account, true); //第二個引數沒有用處

            return url;
        }

    }
}
