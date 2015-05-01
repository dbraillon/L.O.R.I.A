using Loria.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loria.Configuration.Models.Abilities
{
    public class CreateViewModel
    {
        public string Name { get; set; }
        public AbilityType Type { get; set; }
        public string[] Senses { get; set; }
    }
}