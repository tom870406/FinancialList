using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Financial_List.Models;
using Financial_List.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var parameters = new[]
        {
            new SqlParameter("@UserID", model.UserID),
            new SqlParameter("@Password", model.Password)
        };
        var result = (await _context.User
            .FromSqlRaw("EXEC LoginUser @UserID, @Password", parameters)
            .ToListAsync())
            .FirstOrDefault();

        if (result != null)
        {
            HttpContext.Session.SetString("UserID", result.UserID);
            HttpContext.Session.SetString("UserName", result.UserName);
            CommonData.UserID = result.UserID;
            CommonData.UserName = result.UserName;
            CommonData.Email = result.Email;
            CommonData.Account = result.Account;
            return RedirectToAction("Index", "LikeList");
        }

        ViewBag.Error = "登入失敗，請檢查帳號密碼。";
        return View(model);
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        try
        {
            bool _Checkdata = true;

            //check UserID
            if (model.UserID != null)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(model.UserID, @"^[A-Z]{1}[1-2]{1}[0-9]{8}$"))
                {
                    _Checkdata = false;
                    ModelState.AddModelError("UserID", "身分證字號格式錯誤");
                }

                var exists = _context.User.Any(u => u.UserID == model.UserID);
                if (exists)
                {
                    _Checkdata = false;
                    ModelState.AddModelError("UserID", "此身分證字號已註冊");
                }
            }
            else
            {
                _Checkdata = false;
                ModelState.AddModelError("UserID", "身分證字號不得為空");
            }

            //check Password
            if (model.Password == null)
            {
                _Checkdata = false;
                ModelState.AddModelError("Password", "密碼不得為空");
            }

            //check Email
            if (model.Email == null)
            {
                _Checkdata = false;
                ModelState.AddModelError("Email", "電子郵件不得為空");
            }
            else
            {
                if (!new EmailAddressAttribute().IsValid(model.Email))
                {
                    _Checkdata = false;
                    ModelState.AddModelError("Email", "Email 格式錯誤");
                }
            }

            //check Account
            if (model.Account == null)
            {
                _Checkdata = false;
                ModelState.AddModelError("Account", "扣款帳號不得為空");
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(model.Account, @"^[0-9]+$"))
                {
                    _Checkdata = false;
                    ModelState.AddModelError("Account", "扣款帳號僅能輸入數字");
                }
            }

            if (!_Checkdata)
            {
                return View(model);
            }

            var parameters = new[]
            {
                new SqlParameter("@UserID", model.UserID),
                new SqlParameter("@UserName", model.UserName),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@Account", model.Account),
                new SqlParameter("@Password", model.Password)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC RegisterUser @UserID, @UserName, @Email, @Account, @Password",
                parameters
            );

            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
