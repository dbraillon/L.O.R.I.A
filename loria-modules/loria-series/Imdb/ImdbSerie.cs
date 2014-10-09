using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Module.Series
{
    [DataContract(Name = "d")]
    public class ImdbSerie
    {
        [DataMember(Name = "y")]
        public string Year { get; set; }

        [DataMember(Name = "l")]
        public string Title { get; set; }

        [DataMember(Name = "q")]
        public string Type { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "s")]
        public string Stars { get; set; }

        [DataMember(Name = "i")]
        public ICollection<string> Image { get; set; }

        public ICollection<ImdbSeason> Seasons { get; set; }
    }
}
