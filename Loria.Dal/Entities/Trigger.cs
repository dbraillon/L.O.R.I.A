using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Entities
{
    public class Trigger
    {
        public Guid Id { get; set; }
        public int ChannelId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Channel Channel { get; set; }
        public virtual ICollection<TriggerItem> TriggerItems { get; set; }
    }
}
