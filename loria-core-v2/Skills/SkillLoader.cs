using Loria.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Skills
{
    public static class SkillLoader
    {
        public static SkillAction GetSkillAction(Skill skill)
        {
            if (skill.Recipe == SkillRecipe.Talk)
            {
                TalkSkill talkSkill = new TalkSkill();
                talkSkill.FirstValue = skill.FirstValue;
                talkSkill.SecondValue = skill.SecondValue;
                talkSkill.Recipe = skill.Recipe;

                return talkSkill;
            }

            return null;
        }
    }
}
