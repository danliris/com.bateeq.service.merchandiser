using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public abstract class BasicOldViewModel
    {
        public string _id { get; set; }
        public bool _deleted { get; set; }
        public bool _active { get; set; }
        public DateTime _createdDate { get; set; }
        public string _createdBy { get; set; }
        public string _createAgent { get; set; }
        public DateTime _updatedDate { get; set; }
        public string _updatedBy { get; set; }
        public string _updateAgent { get; set; }
    }
}
