using System;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using astute.CoreModel;
using astute.CoreServices;
using astute.Models;
using astute.Repository;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJWTAuthentication _jWTAuthentication;
        #endregion

        #region Ctor
        public CategoryController(ICategoryService categoryService,
            IConfiguration configuration,
            ICommonService commonService,
            IHttpContextAccessor httpContextAccessor,
            IJWTAuthentication jWTAuthentication)
        {
            _categoryService = categoryService;
            _configuration = configuration;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
            _jWTAuthentication = jWTAuthentication;
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
                    if (result == 2)
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
                    else
                    {
                        var result_p = await _categoryService.InsertCategoryValuePricing(result, (int)category_Value.Cat_Id);
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.CategoryValueCreated
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
                if (result == 1)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.CategoryValueDeleted
                    });
                }
                if (result == 547)
                {
                    return Conflict(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.ReferenceFoundError
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
        [Authorize]
        public async Task<IActionResult> Get_Category_Value_Max_Order_No(int cat_Id)
        {
            var result = await _categoryService.Get_Category_Value_Max_Order_No(cat_Id);

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

        #region Column Master
        [HttpGet]
        [Route("get_column_master")]
        [Authorize]
        public async Task<IActionResult> Get_Column_Master()
        {
            try
            {
                var result = await _categoryService.Get_Column_Master();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Column_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion
        
        #region Import Master
        [HttpGet]
        [Route("get_import_master")]
        [Authorize]
        public async Task<IActionResult> Get_Import_Master()
        {
            try
            {
                var result = await _categoryService.Get_Import_Master();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Import_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("create_import_master")]
        [Authorize]
        public async Task<IActionResult> Create_Import_Master(Import_Master import_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                    if (user_Id > 0)
                    {
                        var result = await _categoryService.Insert_Import_Master(import_Master, user_Id ?? 0);
                        if (result == 1)
                        {
                            return Ok(new
                            {
                                statusCode = HttpStatusCode.OK,
                                message = CoreCommonMessage.ImportMasterCreated
                            });
                        }
                        else if (result == 2)
                        {
                            return Conflict(new
                            {
                                statusCode = HttpStatusCode.Conflict,
                                message = CoreCommonMessage.ImportMasterExists,
                            });
                        }
                    }
                    return Unauthorized();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Import_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("update_import_master")]
        [Authorize]
        public async Task<IActionResult> Update_Import_Master(Import_Master import_Master)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _categoryService.Update_Import_Master(import_Master);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ImportMasterUpdated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_Import_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_import_master")]
        [Authorize]
        public async Task<IActionResult> Delete_Import_Master(int id)
        {
            try
            {
                var result = await _categoryService.Delete_Import_Master(id);
                if (result == 1)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.ImportMasterDeleted
                    });
                }
                else if (result == 2)
                {
                    return BadRequest(new
                    {
                        statusCode = HttpStatusCode.Conflict,
                        message = CoreCommonMessage.DeleteImportDetail
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
                await _commonService.InsertErrorLog(ex.Message, "Delete_Import_Master", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion

        #region Import Detail
        [HttpGet]
        [Route("get_import_detail")]
        [Authorize]
        public async Task<IActionResult> Get_Import_Detail()
        {
            try
            {
                var result = await _categoryService.Get_Import_Detail();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Import_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("create_import_detail")]
        [Authorize]
        public async Task<IActionResult> Create_Import_Detail(Import_Detail import_Detail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _categoryService.Insert_Import_Detail(import_Detail);
                    if (result == 1)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ImportDetailCreated
                        });
                    }
                    else if (result == 2)
                    {
                        return Conflict(new
                        {
                            statusCode = HttpStatusCode.Conflict,
                            message = CoreCommonMessage.ImportDetailExists,
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Import_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("update_import_detail")]
        [Authorize]
        public async Task<IActionResult> Update_Import_Detail(Import_Detail import_Detail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _categoryService.Update_Import_Detail(import_Detail);
                    if (result > 0)
                    {
                        return Ok(new
                        {
                            statusCode = HttpStatusCode.OK,
                            message = CoreCommonMessage.ImportDetailUpdated
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Update_Import_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_import_detail")]
        [Authorize]
        public async Task<IActionResult> Delete_Import_Detail(int id)
        {
            try
            {
                var result = await _categoryService.Delete_Import_Detail(id);
                if (result == 1)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.ImportDetailDeleted
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
                await _commonService.InsertErrorLog(ex.Message, "Delete_Import_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        #endregion
        
        #region Import Excel
        [HttpGet]
        [Route("get_import_excel")]
        [Authorize]
        public async Task<IActionResult> Get_Import_Excel()
        {
            try
            {
                var result = await _categoryService.Get_Import_Excel();
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Import_Excel", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        
        [HttpGet]
        [Route("get_import_master_detail")]
        [Authorize]
        public async Task<IActionResult> Get_Import_Master_Detail(int import_Id)
        {
            try
            {
                var result = await _categoryService.Get_Import_Master_Detail(import_Id);
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
                await _commonService.InsertErrorLog(ex.Message, "Get_Import_Master_Detail", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("create_update_import_excel")]
        [Authorize]
        public async Task<IActionResult> Create_Update_Import_Excel(Import_Excel import_Excel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = CoreService.Get_Authorization_Token(_httpContextAccessor);
                    int? user_Id = _jWTAuthentication.Validate_Jwt_Token(token);
                    if (user_Id > 0)
                    {
                        var Import_Master_Result = await _categoryService.Insert_Import_Master(import_Excel.Import_Master, user_Id ?? 0);
                        if (Import_Master_Result > 0)
                        {

                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("Import_Id", typeof(int));
                            dataTable.Columns.Add("Import_Det_Id", typeof(int));
                            dataTable.Columns.Add("Column_Name", typeof(int));
                            dataTable.Columns.Add("Excel_Column_No", typeof(int));
                            dataTable.Columns.Add("Required", typeof(bool));

                            foreach (var item in import_Excel.Import_Details)
                            {
                                DataRow dr = dataTable.NewRow();
                                dr["Import_Id"] = Import_Master_Result;
                                dr["Import_Det_Id"] = item.Import_Det_Id;
                                dr["Column_Name"] = item.Column_Name;
                                dr["Excel_Column_No"] = item.Excel_Column_No;
                                dr["Required"] = item.Required;

                                dataTable.Rows.Add(dr);
                            }
                            var result = await _categoryService.Insert_Update_Import_Excel(dataTable);

                            if (result > 0)
                            {
                                return Ok(new
                                {
                                    statusCode = HttpStatusCode.OK,
                                    message = CoreCommonMessage.ImportExcelCreated
                                });
                            }
                        }
                    }
                    return Unauthorized();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                await _commonService.InsertErrorLog(ex.Message, "Create_Import_Excel", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("delete_import_excel")]
        [Authorize]
        public async Task<IActionResult> Delete_Import_Excel(int id)
        {
            try
            {
                var result = await _categoryService.Delete_Import_Excel(id);
                if (result > 0)
                {
                    return Ok(new
                    {
                        statusCode = HttpStatusCode.OK,
                        message = CoreCommonMessage.ImportExcelDeleted
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
                await _commonService.InsertErrorLog(ex.Message, "Delete_Import_Excel", ex.StackTrace);
                return Ok(new
                {
                    message = ex.Message
                });
            }
        }

        #endregion

        #endregion
    }
}
