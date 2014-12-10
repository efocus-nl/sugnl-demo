using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace WebApplication1.Models
{
    [SitecoreType(true, "{21ea8b82-3247-4388-865c-8a995e397dd3}")]
    public class TweetMessage: BaseSitecoreModel
    {
        
        [SitecoreField("{8a3f9be9-d8a5-4687-ad72-ddb1d49def32}", SitecoreFieldType.SingleLineText)]
        public virtual string TweetText { get; set; }
    }
}
