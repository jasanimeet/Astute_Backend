using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace astute.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CategoryController : ControllerBase
    {

        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        #endregion

        #region Ctor
        public CategoryController(ICategoryService categoryService,
            ISupplierService supplierService,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            ICommonService commonService)
        {
            _categoryService = categoryService;
            _supplierService = supplierService;
            _environment = environment;
            _configuration = configuration;
            _commonService = commonService;
        }
        #endregion

        #region Methods
        #region Category Master
        [HttpGet]
        [Route("getcategory")]
        [Authorize]
        public async Task<IActionResult> GetCategory(int catId, int colId)
        {
            try
            {
                var result = await _categoryService.GetCategory(catId, colId);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetCategory", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createcategory")]
        [Authorize]
        public async Task<IActionResult> CreateCategory(Category_Master category_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _categoryService.InsertCategory(category_Master);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CategoryMasterCreated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CategoryExists,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateCategory", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatecategory")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(Category_Master category_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _categoryService.UpdateCategory(category_Master);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CategoryMasterUpdated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateCategory", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletecategory")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategory(id);
                if (result == 1)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.CategoryMasterDeleted
                    });
                }
                else if (result == 2)
                {
                    return BadRequest(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = "First remove category value reference."
                    });

                }
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "DeleteCategory", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Category Values
        [HttpGet]
        [Route("GetAllCategoryValues")]
        [Authorize]
        public async Task<IActionResult> GetAllCategoryValues(int catId)
        {
            try
            {
                var result = await _categoryService.GetCategoryValuesByCatId(catId);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetAllCategoryValues", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_active_category_values")]
        [Authorize]
        public async Task<IActionResult> Get_Active_Category_Values(int catId)
        {
            try
            {
                var result = await _categoryService.Get_Active_Category_Values(catId);
                if (result != null && result.Count > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Get_Active_Category_Values", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("getcategoryvaluebycatvalid")]
        [Authorize]
        public async Task<IActionResult> GetCategoryValueByCatValId(int catValId)
        {
            try
            {
                var result = await _categoryService.GetCategoryValueByCatValId(catValId);
                if (result != null)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.DataSuccessfullyFound,
                        data = result
                    });
                }
                return Ok(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = CoreCommonMessage.DataNotFound,
                    data = result,
                });
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "GetCategoryValueByCatValId", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("createcategoryvalue")]
        [Authorize]
        public async Task<IActionResult> CreateCategoryValue([FromForm] Category_Value category_Value, IFormFile Icon_Url)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Icon_Url != null && Icon_Url.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/CategoryValueIcon");
                        if (!(Directory.Exists(filePath)))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(Icon_Url.FileName);
                        string fileExt = Path.GetExtension(Icon_Url.FileName);

                        string strFile = category_Value.Cat_Name + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                        using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                        {
                            await Icon_Url.CopyToAsync(fileStream);
                        }
                        category_Value.Icon_Url = strFile;
                    }
                    var result = await _categoryService.InsertCategoryValue(category_Value);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CategoryValueCreated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CategoryValueExists
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CategoryValueOrderNoExists
                        });
                    }
                    else if (result == 4)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CategoryValueSortNoExists
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "CreateCategoryValue", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("updatecategoryvalue")]
        [Authorize]
        public async Task<IActionResult> UpdateCategoryValue([FromForm] Category_Value category_Value, IFormFile Icon_Url)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Icon_Url != null && Icon_Url.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/CategoryValueIcon");
                        if (!(Directory.Exists(filePath)))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string fileName = Path.GetFileNameWithoutExtension(Icon_Url.FileName);
                        string fileExt = Path.GetExtension(Icon_Url.FileName);

                        string strFile = category_Value.Cat_Name + "_" + DateTime.UtcNow.ToString("ddMMyyyyHHmmss") + fileExt;
                        using (var fileStream = new FileStream(Path.Combine(filePath, strFile), FileMode.Create))
                        {
                            await Icon_Url.CopyToAsync(fileStream);
                        }
                        category_Value.Icon_Url = strFile;
                    }
                    var result = await _categoryService.UpdateCategoryValue(category_Value);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CategoryValueUpdated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CategoryValueExists
                        });
                    }
                    else if (result == 3)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CategoryValueOrderNoExists
                        });
                    }
                    else if (result == 4)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.CategoryValueSortNoExists
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "UpdateCategoryValue", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("deletecategoryvalue")]
        [Authorize]
        public async Task<IActionResult> DeleteCategoryValue(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryValue(id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.CategoryValueDeleted
                    });
                }
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    message = CoreCommonMessage.ParameterMismatched
                });
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.ReferenceFoundError
                    });
                }
                await _commonService.InsertErrorLog(ex.Message, "UpdateCategoryValue", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("changestatuscategoryvalue")]
        [Authorize]
        public async Task<IActionResult> ChangeStatusCategoryValue(int cat_Val_Id, bool status)
        {
            try
            {
                var result = await _categoryService.ChangeStatus(cat_Val_Id, status);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.StatusChangedSuccessMessage
                    });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "ChangeStatusCategoryValue", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("get_category_value_max_order_no")]
        public async Task<IActionResult> Get_Category_Value_Max_Order_No()
        {
            var result = await _categoryService.Get_Category_Value_Max_Order_No();

            return Ok(new
            {
                statusCode = HttpStatusCode.OK,
                order_no = result
            });
        }
        #endregion

        #region Export Excel Ctegory Values
        public async Task<IActionResult> ExportCategoryValueExcel(int catId)
        {
            DataTable dtSumm = new DataTable();
            DataTable dt = await _categoryService.GetCategororyValues(catId);

            string filename = "Sunrise_Diamonds_" + DateTime.UtcNow.ToString("ddMMyyyy-HHmmss") + ".xlsx";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files/CategoryExcel/");
            EpExcelExport.CreateExcel(dt.DefaultView.ToTable(), filePath, filePath + filename, colorType: "Fancy");
            var excelPath = _configuration["LocalPath"] + "Files/CategoryExcel/" + filename;
            return Ok(excelPath);
        }

        [HttpPost]
        [Route("readexcelhiperlink")]
        public async Task<IActionResult> ReadExcelHiperLink()
        {
            string filename = "SRK.xlsx";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/LoaderImages/");
            string filePath = Path.Combine(folderPath, filename);
            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0];

            string cellAddress = "C2";

            // Retrieve the hyperlink from the cell
            ExcelHyperLink hyperlink = (ExcelHyperLink)worksheet.Cells[cellAddress].Hyperlink;
            string linkUrl = string.Empty;
            if (hyperlink != null)
            {
                linkUrl = hyperlink.AbsoluteUri;
            }
            return Ok(new
            {
                StatusCode = HttpStatusCode.OK,
                message = CoreCommonMessage.DataSuccessfullyFound,
                URL = linkUrl
            });
        }
        #endregion        
        #endregion
    }
}
