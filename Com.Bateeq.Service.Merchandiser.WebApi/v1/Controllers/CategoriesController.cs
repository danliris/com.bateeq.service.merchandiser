using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Moonlay.NetCore.Lib;
using Com.Bateeq.Service.Merchandiser.WebApi.Helpers;
using Com.Bateeq.Service.Merchandiser.WebApi.v1.ViewModels;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.EntityFrameworkCore;

namespace Com.Bateeq.Service.Merchandiser.WebApi.v1.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/categories")]
    public class CategoriesController : Controller
    {
        private readonly CoreDbContext _context;
        private readonly CategoryService _service;
        private readonly string ApiVersion = "1.0";

        public CategoriesController(CoreDbContext context, CategoryService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetCategory(int Page = 1, int Size = 25, Dictionary<string, object> Order = null)
        {
            try
            {
                int TotalData = _context.Categories.Count();

                List<Category> Data = new Pageable<Category>(_context.Categories, Page - 1, Size).Data.ToList<Category>();
                int TotalPageData = Data.Count();

                Dictionary<string, object> Result = 
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.OK_STATUS_CODE, Common.OK_MESSAGE)
                    .Ok(Data, ViewModelMap, Page, Size, TotalData, TotalPageData);

                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Category = await _context.Categories.SingleOrDefaultAsync(m => m.Id == id);

            if (Category == null)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.NOT_FOUND_STATUS_CODE, Common.NOT_FOUND_MESSAGE)
                    .Fail();
                return NotFound(Result);
            }

            try
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.OK_STATUS_CODE, Common.OK_MESSAGE)
                    .Ok(Category, ViewModelMap);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory([FromRoute] int id, [FromBody] CategoryViewModel CategoryVM)
        {
            Category Category = ModelMap(CategoryVM);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Category.Id)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.BAD_REQUEST_STATUS_CODE, Common.BAD_REQUEST_MESSAGE)
                    .Fail();
                return BadRequest(Result);
            }

            try
            {
                await _service.UpdateAsync(id, Category);

                return NoContent();
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.BAD_REQUEST_STATUS_CODE, Common.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!CategoryExists(id))
                {
                    Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.NOT_FOUND_STATUS_CODE, Common.NOT_FOUND_MESSAGE)
                    .Fail();
                    return NotFound(Result);
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                        .Fail(e);
                    return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] CategoryViewModel CategoryVM)
        {
            Category Category = ModelMap(CategoryVM);

            try
            {
                await _service.CreateAsync(Category);

                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.CREATED_STATUS_CODE, Common.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(HttpContext.Request.Path, "/", Category.Id), Result);
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.BAD_REQUEST_STATUS_CODE, Common.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter<Category, CategoryViewModel>(ApiVersion, Common.INTERNAL_ERROR_STATUS_CODE, Common.INTERNAL_ERROR_MESSAGE)
                    .Fail(e);
                return StatusCode(Common.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        private CategoryViewModel ViewModelMap(Category Category)
        {
            CategoryViewModel CategoryVM = new CategoryViewModel();

            CategoryVM._id = Category.Id;
            CategoryVM._deleted = Category._IsDeleted;
            CategoryVM._active = Category.Active;
            CategoryVM._createdDate = Category._CreatedUtc;
            CategoryVM._createdBy = Category._CreatedBy;
            CategoryVM._createAgent = Category._CreatedAgent;
            CategoryVM._updatedDate = Category._LastModifiedUtc;
            CategoryVM._updatedBy = Category._LastModifiedBy;
            CategoryVM._updateAgent = Category._LastModifiedAgent;
            CategoryVM.code = Category.Code;
            CategoryVM.name = Category.Name;
            CategoryVM.description = Category.Description;

            return CategoryVM;
        }

        private Category ModelMap(CategoryViewModel CategoryVM)
        {
            Category Category = new Category();

            Category.Id = CategoryVM._id;
            Category._IsDeleted = CategoryVM._deleted;
            Category.Active = CategoryVM._active;
            Category._CreatedUtc = CategoryVM._createdDate;
            Category._CreatedBy = CategoryVM._createdBy;
            Category._CreatedAgent = CategoryVM._createAgent;
            Category._LastModifiedUtc = CategoryVM._updatedDate;
            Category._LastModifiedBy = CategoryVM._updatedBy;
            Category._LastModifiedAgent = CategoryVM._updateAgent;
            Category.Code = CategoryVM.code;
            Category.Name = CategoryVM.name;
            Category.Description = CategoryVM.description;

            return Category;
        }
    }
}