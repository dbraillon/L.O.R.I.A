using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Entities
{
    public class Channel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Action> Actions { get; set; }
        public virtual ICollection<Trigger> Triggers { get; set; }
    }
}
