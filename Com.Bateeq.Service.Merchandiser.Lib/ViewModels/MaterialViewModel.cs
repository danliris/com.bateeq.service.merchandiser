using System;

namespace Com.Bateeq.Service.Merchandiser.Lib.ViewModels
{
    public class MaterialViewModel
    {
        public int Id { get; set; }

        public bool _IsDeleted { get; set; }

        public bool Active { get; set; }

        public DateTime _CreatedUtc { get; set; }

        public string _CreatedBy { get; set; }

        public string _CreatedAgent { get; set; }

        public DateTime _LastModifiedUtc { get; set; }

        public string _LastModifiedBy { get; set; }

        public string _LastModifiedAgent { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Composition { get; set; }

        public string Construction { get; set; }

        public string Width { get; set; }

        public string Yarn { get; set; }

        public MaterialCategoryViewModel Category { get; set; }
    }

    public class MaterialCategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
