using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Entities
{
    public class ReceipeIn
    {
        public int Id { get; set; }
        public int ReceipeId { get; set; }
        public int TriggerItemId { get; set; }

        public string Value { get; set; }

        public virtual Receipe Receipe { get; set; }
        public virtual TriggerItem TriggerItem { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
