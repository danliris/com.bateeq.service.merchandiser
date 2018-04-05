using Com.Bateeq.Service.Merchandiser.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class ArticleSeasonViewModel : BasicOldViewModel
    {
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
