using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoC.Persistence;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.ContentSearch;

namespace WebApplication1.Models
{
    [SitecoreType(true, "{6fc5850f-9eec-4c42-b89f-ef1fef2915c5}")]
    public class BaseSitecoreModel: BaseEntity<Guid>
    {
        [SitecoreId]
        [IndexField(BuiltinFields.Group)]
        public override Guid Id { get; set; }

        [SitecoreInfo(SitecoreInfoType.FullPath)]
        public virtual string Path { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name)]
        [IndexField(BuiltinFields.Name)]
        public virtual string Name { get; set; }

        [IndexField(BuiltinFields.Parent)]
        public virtual Guid ParentId { get; set; }

        [SitecoreParent(InferType = true, IsLazy = true)]
        public virtual BaseSitecoreModel Parent { get; set; }

        [IndexField(BuiltinFields.AllTemplates)]
        [SitecoreInfo(SitecoreInfoType.BaseTemplateIds)]
        public virtual IEnumerable<Guid> TemplateIds { get; set; }

        [IndexField(BuiltinFields.Template)]
        [SitecoreInfo(SitecoreInfoType.TemplateId)]
        public virtual Guid TemplateId { get; set; }
    }
}
