using Loria.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Dal
{
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            Ability introduceMyselfAbility = new Ability()
            {
                Name = "Introduce myself",
                Senses = Sense.Hearing,
                Type = AbilityType.Innate,
                Skills = new Skill[] 
                {
                    new Skill()
                    {
                        Recipe = SkillRecipe.Talk, 
                        FirstValue = "Je suis une semi-intelligence artificielle et je m'appelle Loria."
                    }
                },
                Stimulus = new Stimuli[] 
                { 
                    new Stimuli() { Value = "Loria, présente toi." }
                }
            };

            context.Abilities.Add(introduceMyselfAbility);

            base.Seed(context);
        }
    }
}
