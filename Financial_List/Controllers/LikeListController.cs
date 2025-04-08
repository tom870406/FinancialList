using Financial_List.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Financial_List.Models;
using Financial_List.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

public class LikeListController : Controller
{
    private readonly ApplicationDbContext _context;

    public LikeListController(ApplicationDbContext context)
    {
        _context = context;
    }
    private bool IsLoggedIn()
    {
        return HttpContext.Session.GetString("UserID") != null;
    }

    private void PopulateDropDownLists()
    {
        var users = _context.User
            .Where(u => u.UserID == HttpContext.Session.GetString("UserID"))
            .Select(u => new SelectListItem
            {
                Value = u.UserID,
                Text = $"{u.Account}"
            }).ToList();

        var products = _context.Product.Select(p => new SelectListItem
        {
            Value = p.ProductNo.ToString(),
            Text = $"{p.ProductName} - NT${p.Price} - 手續費率 {p.FeeRate * 100:F2}%"
        }).ToList();

        var productlist = _context.Product
        .Select(p => new { productNo = p.ProductNo, price = p.Price, feeRate = p.FeeRate })
        .ToList();

        ViewBag.Users = users;
        ViewBag.Products = products;
        ViewBag.ProductList = productlist;
    }

    private bool CheckData(LikeList _import_data)
    {
        try
        {
            bool _Checkdata = true;
            if (_import_data.ProductNo == null)
            {
                _Checkdata = false;
                ModelState.AddModelError("ProductNo", "請選擇商品");
            }

            if (_import_data.OrderAmount == null)
            {
                _Checkdata = false;
                ModelState.AddModelError("ProductNo", "請輸入數量");
            }

            if (_import_data.TotalFee == null)
            {
                _Checkdata = false;
                ModelState.AddModelError("ProductNo", "請輸入數量");
            }

            if (_import_data.TotalAmount == null)
            {
                _Checkdata = false;
                ModelState.AddModelError("ProductNo", "請輸入數量");
            }

            if (!_Checkdata)
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return false;
        }

        return true;
    }

    public async Task<IActionResult> Index(string userId)
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
        userId = HttpContext.Session.GetString("UserID") ?? userId;

        if (string.IsNullOrEmpty(userId))
        {
            PopulateDropDownLists();
            return View(new List<UserLikeListViewModel>());
        }

        var parameters = new[]
        {
            new SqlParameter("@UserID", userId),
        };

        var result = await _context.UserLikeListView
            .FromSqlRaw("EXEC GetUserLikeList @UserID", parameters)
            .ToListAsync();

        PopulateDropDownLists();
        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

        PopulateDropDownLists();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LikeList model)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            model.UserID = HttpContext.Session.GetString("UserID");

            if (!CheckData(model))
            {
                PopulateDropDownLists();
                return View(model);
            }

            //新增時須判斷該產品是否已新增
            var exists = await _context.LikeList.AnyAsync(u => u.UserID == model.UserID && u.ProductNo == model.ProductNo);
            if (exists)
            {
                ModelState.AddModelError("ProductNo", "該商品已存在於清單中");
                PopulateDropDownLists();
                return View(model);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@UserID", model.UserID),
                        new SqlParameter("@ProductNo", model.ProductNo),
                        new SqlParameter("@OrderAmount", model.OrderAmount),
                        new SqlParameter("@Account", model.Account),
                        new SqlParameter("@TotalFee", model.TotalFee),
                        new SqlParameter("@TotalAmount", model.TotalAmount)
                    };
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC InsertLikeItem @UserID, @ProductNo, @OrderAmount, @Account, @TotalFee, @TotalAmount",
                        parameters
                    );
                    await transaction.CommitAsync();

                    PopulateDropDownLists();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", $"新增失敗: {ex.Message}");
                    PopulateDropDownLists();
                    return View(model);
                }
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }

    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

        var parameters = new[]
        {
            new SqlParameter("@SN", id),
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC DeleteLikeItem @SN = {0}", parameters
        );
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");


        var parameters = new[]
{
            new SqlParameter("@UserID", CommonData.UserID),
            new SqlParameter("@SN", id),
        };

        var result = (await _context.LikeList
            .FromSqlRaw("EXEC GetUserLikeListBySN @UserID, @SN", parameters)
            .ToListAsync())
            .FirstOrDefault();
        
        if (result == null)
            return NotFound();

        PopulateDropDownLists();
        return View(result);
    }

    // 編輯喜好清單
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LikeList model)
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

        if (!CheckData(model))
        {
            PopulateDropDownLists();
            return View(model);
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@SN", model.SN),
                    new SqlParameter("@OrderAmount", model.OrderAmount),
                    new SqlParameter("@Account", model.Account),
                    new SqlParameter("@TotalFee", model.TotalFee),
                    new SqlParameter("@TotalAmount", model.TotalAmount)
                };
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateLikeItem @SN, @OrderAmount, @Account, @TotalFee, @TotalAmount",
                    parameters
                );

                await transaction.CommitAsync();

                PopulateDropDownLists();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"更新失敗: {ex.Message}");
                PopulateDropDownLists();
                return View(model);
            }
        }
    }
}
