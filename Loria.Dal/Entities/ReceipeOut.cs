using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Entities
{
    public class ReceipeOut
    {
        public int Id { get; set; }
        public int ReceipeId { get; set; }
        public int ActionItemId { get; set; }

        public string Value { get; set; }

        public virtual Receipe Receipe { get; set; }
        public virtual ActionItem ActionItem { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
