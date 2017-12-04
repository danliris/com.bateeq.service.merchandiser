using System;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class MaterialViewModel
    {
        public int _id { get; set; }

        public bool _deleted { get; set; }

        public bool _active { get; set; }

        public DateTime _createdDate { get; set; }

        public string _createdBy { get; set; }

        public string _createAgent { get; set; }

        public DateTime _updatedDate { get; set; }

        public string _updatedBy { get; set; }

        public string _updateAgent { get; set; }

        public string code { get; set; }

        public string name { get; set; }

        public string description { get; set; }
    }
}
