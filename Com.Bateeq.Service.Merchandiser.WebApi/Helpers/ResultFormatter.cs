using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Bateeq.Service.Merchandiser.WebApi.Helpers
{
    public class ResultFormatter<TModel, TViewModel>
    {
        public delegate TViewModel MapCallBack(TModel model);

        public Dictionary<string, object> Result { get; set; }

        public ResultFormatter(string ApiVersion, int StatusCode, string Message)
        {
            Result = new Dictionary<string, object>();
            AddResponseInformation(Result, ApiVersion, StatusCode, Message);
        }

        public Dictionary<string, object> Ok()
        {
            return Result;
        }

        public Dictionary<string, object> Ok(List<TModel> Data, MapCallBack Map, int Page, int Size, int TotalData, int TotalPageData)
        {
            Dictionary<string, object> Info = new Dictionary<string, object>();
            Info.Add("count", TotalPageData);
            Info.Add("page", Page);
            Info.Add("size", Size);
            Info.Add("total", TotalData);

            List<TViewModel> DataVM = new List<TViewModel>();

            foreach (TModel d in Data)
            {
                DataVM.Add(Map(d));
            }

            Result.Add("data", DataVM);
            Result.Add("info", Info);

            return Result;
        }

        public Dictionary<string, object> Ok(TModel Data, MapCallBack Map)
        {
            Result.Add("data", Map(Data));

            return Result;
        }

        public Dictionary<string, object> Fail()
        {
            return Result;
        }

        public Dictionary<string, object> Fail(Exception e)
        {
            Result.Add("exception", e);

            return Result;
        }

        public Dictionary<string, object> Fail(ServiceValidationExeption e)
        {
            Dictionary<string, string> Errors = new Dictionary<string, string>();

            foreach (ValidationResult error in e.ValidationResults)
            {
                Errors.Add(error.MemberNames.First(), error.ErrorMessage);
            }

            Result.Add("error", Errors);
            return Result;
        }

        public void AddResponseInformation(Dictionary<string, object> Result, string ApiVersion, int StatusCode, string Message)
        {
            Result.Add("apiVersion", ApiVersion);
            Result.Add("statusCode", StatusCode);
            Result.Add("message", Message);
        }
    }
}
