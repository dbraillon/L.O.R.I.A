using Loria.Dal;
using Loria.Dal.Entities;
using Loria.Dal.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Channels.VoiceChannel.Install
{
    public class Program
    {
        public static System.Guid VoiceChannelGuid = new System.Guid("5757b28e-b12e-4a33-a4ef-811a2672f51e");
        public static System.Guid SentenceRecognizedTriggerGuid = new System.Guid("745f0d08-4c29-4d6d-9b30-68db670c416e");

        public static Channel VoiceChannel = new Channel()
        {
            Id = VoiceChannelGuid,
            Name = "Voice",
            Description = "Talk and listen channel.",
            Triggers = new Trigger[] 
            {
                new Trigger()
                {
                    Id = SentenceRecognizedTriggerGuid,
                    Name = "Sentence recognized",
                    Description = "Loria recognizes a sentence.",
                    TriggerItems = new TriggerItem[]
                    {
                        new TriggerItem()
                        {
                            Name = "Phrase recognized",
                            Type = EItemType.String,
                            Ingredients = new Ingredient[]
                            {
                                new Ingredient() { Name = "AppendAt" },
                                new Ingredient() { Name = "PhraseRecognized" }
                            }
                        }
                    }
                }
            },
            Actions = new Action[] 
            { 
                new Action()
                {
                    Name = "Say a sentence",
                    Description = "Loria says a sentence.",
                    ActionItems = new ActionItem[]
                    {
                        new ActionItem()
                        {
                            Name = "Sentence",
                            Type = EItemType.String
                        }
                    }
                }
            }
        };

        static void Main(string[] args)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (!db.Channels.Any(x => x.Id == VoiceChannel.Id))
                {
                    db.Channels.Add(VoiceChannel);
                    db.SaveChanges();
                }
            }
        }
    }
}
