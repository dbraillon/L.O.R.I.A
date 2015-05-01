using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal.Entities
{
    public class Skill
    {
        public int Id { get; set; }
        public SkillRecipe Recipe { get; set; }
        public string FirstValue { get; set; }
        public string SecondValue { get; set; }
    }
}
