using Loria.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Entities
{
    public class ActionItem
    {
        public int Id { get; set; }
        public int ActionId { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
        public EItemType Type { get; set; }

        public virtual Action Action { get; set; }
        public virtual ICollection<ReceipeOut> ReceipeOuts { get; set; }
    }
}
