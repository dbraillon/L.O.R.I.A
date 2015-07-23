using Loria.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Entities
{
    public class TriggerItem
    {
        public int Id { get; set; }
        public int TriggerId { get; set; }

        public string Name { get; set; }
        public EItemType Type { get; set; }

        public virtual Trigger Trigger { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<ReceipeIn> ReceipeIns { get; set; }
    }
}
