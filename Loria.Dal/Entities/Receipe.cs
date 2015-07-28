using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Entities
{
    public class Receipe
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ReceipeIn> ReceipeIns { get; set; }
        public virtual ICollection<ReceipeOut> ReceipeOuts { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
