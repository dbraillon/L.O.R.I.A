using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Module.Series
{
    public class ImdbSeason
    {
        public int Number { get; set; }
        public ICollection<ImdbEpisode> Episodes { get; set; }
    }
}
