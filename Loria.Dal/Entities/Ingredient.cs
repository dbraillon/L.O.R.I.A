using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public int TriggerItemId { get; set; }

        public string Name { get; set; }
        public string Value
        {
            get
            {
                return string.Format("{{{0}}}", Name);
            }

            set
            {
                Value = value;
            }
        }

        public virtual TriggerItem TriggerItem { get; set; }
    }
}
