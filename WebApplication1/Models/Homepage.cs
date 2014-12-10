using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace WebApplication1.Models
{
    [SitecoreType(false, "{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}")]
    public class Homepage: BaseSitecoreModel
    {
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string Text { get; set; }
    }
}
