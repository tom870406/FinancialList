using Financial_List.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Financial_List.Models;
using Financial_List.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }
    private bool IsLoggedIn()
    {
        return HttpContext.Session.GetString("UserID") != null;
    }

    private bool CheckData(Product _import_data)
    {
        try
        {
            bool _Checkdata = true;

            if (_import_data.ProductName == null)
            {
                _Checkdata = false;
                ModelState.AddModelError("ProductName", "請輸入產品名稱");
            }

            if (_import_data.Price == null)
            {
                _Checkdata = false;
                ModelState.AddModelError("Price", "請輸入費用");
            }

            if (_import_data.FeeRate == null)
            {
                _Checkdata = false;
                ModelState.AddModelError("FeeRate", "請輸入手續費率");
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

        var parameters = new[]
        {
            new SqlParameter("@UserID", userId),
        };

        var result = await _context.ProductView
            .FromSqlRaw("EXEC GetAllProducts")
            .ToListAsync();

        
        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

        

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product model)
    {
        try
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");


            if (!CheckData(model))
            {
                
                return View(model);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@ProductName", model.ProductName),
                        new SqlParameter("@Price", model.Price),
                        new SqlParameter("@FeeRate", model.FeeRate),
                    };
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC InsertProduct @ProductName, @Price, @FeeRate",
                        parameters
                    );
                    await transaction.CommitAsync();

                    
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", $"新增失敗: {ex.Message}");
                    
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
            new SqlParameter("@ProductNo", id),
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC DeleteProduct @ProductNo = {0}", parameters
        );
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

        var parameters = new[]
{
            new SqlParameter("@ProductNo", id),
        };

        var result = (await _context.ProductView
            .FromSqlRaw("EXEC GetProductById @ProductNo", parameters)
            .ToListAsync())
            .FirstOrDefault();

        if (result == null)
            return NotFound();

        return View(result);
    }

    // 編輯喜好清單
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product model)
    {
        if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

        if (!CheckData(model))
        {
            
            return View(model);
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ProductName", model.ProductName),
                    new SqlParameter("@Price", model.Price),
                    new SqlParameter("@FeeRate", model.FeeRate),
                };
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateProduct @ProductName, @Price, @FeeRate",
                    parameters
                );

                await transaction.CommitAsync();

                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"更新失敗: {ex.Message}");
                
                return View(model);
            }
        }
    }
}
