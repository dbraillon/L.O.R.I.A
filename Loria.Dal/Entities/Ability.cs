using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Entities
{
    public enum AbilityType
    {
        Innate, Learned
    }

    public class Ability
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AbilityType Type { get; set; }
        public Sense Senses { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Stimuli> Stimulus { get; set; }
    }
}
