using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Com.Moonlay.Models;
using System.ComponentModel.DataAnnotations;
using Com.Moonlay.NetCore.Lib.Service;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using Com.Bateeq.Service.Merchandiser.Lib.Exceptions;

namespace Com.Bateeq.Service.Merchandiser.WebApi.Helpers
{
    public abstract class BasicController<TDbContext, TService, TViewModel, TModel> : Controller
        where TDbContext : DbContext
        where TViewModel : class
        where TService : BasicService<TDbContext, TModel>, IMap<TModel, TViewModel> 
        where TModel : StandardEntity, IValidatableObject
    {
        protected TService Service { get; }
        private string ApiVersion { get; set; }

        public BasicController(TService Service, string ApiVersion)
        {
            this.Service = Service;
            this.ApiVersion = ApiVersion;
        }

        [HttpGet]
        public IActionResult Get(int Page = 1, int Size = 25, string Order = "{}", [Bind(Prefix = "Select[]")]List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            try
            {
                Tuple<List<TModel>, int, Dictionary<string, string>, List<string>> Data = Service.ReadModel(Page, Size, Order, Select, Keyword, Filter);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok<TModel, TViewModel>(Data.Item1, Service.MapToViewModel, Page, Size, Data.Item2, Data.Item1.Count, Data.Item3, Data.Item4);

                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await Service.ReadModelById(Id);

            if (model == null)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                    .Fail();
                return NotFound(Result);
            }

            try
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok<TModel, TViewModel>(model, Service.MapToViewModel);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put([FromRoute] int Id, [FromBody] TViewModel ViewModel)
        {
            try
            {
                this.Validate(ViewModel);
                TModel model = Service.MapToModel(ViewModel);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (Id != model.Id)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                        .Fail();
                    return BadRequest(Result);
                }

                using (var transaction = this.Service.DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await Service.UpdateModel(Id, model);
                        transaction.Commit();
                    }
                    catch (ServiceValidationExeption e)
                    {
                        transaction.Rollback();
                        throw new ServiceValidationExeption(e.ValidationContext, e.ValidationResults);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message, e.InnerException);
                    }
                }

                return NoContent();
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!Service.IsExists(Id))
                {
                    Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                    .Fail();
                    return NotFound(Result);
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                        .Fail();
                    return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TViewModel ViewModel)
        {
            try
            {
                this.Validate(ViewModel);
                TModel model = Service.MapToModel(ViewModel);

                using (var transaction = this.Service.DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await Service.CreateModel(model);
                        transaction.Commit();
                    }
                    catch (ServiceValidationExeption e)
                    {
                        transaction.Rollback();
                        throw new ServiceValidationExeption(e.ValidationContext, e.ValidationResults);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message, e.InnerException);
                    }
                }

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(HttpContext.Request.Path, "/", model.Id), Result);
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var transaction = this.Service.DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await Service.DeleteModel(Id);
                        transaction.Commit();
                    }
                    catch (DbReferenceNotNullException e)
                    {
                        transaction.Rollback();
                        throw new DbReferenceNotNullException(e.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message, e.InnerException);
                    }
                }

                return NoContent();
            }
            catch (DbReferenceNotNullException e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        void Validate(TViewModel viewModel)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(viewModel, this.Service.ServiceProvider, null);

            if (!Validator.TryValidateObject(viewModel, validationContext, validationResults, true))
                throw new ServiceValidationExeption(validationContext, validationResults);
        }
    }
}