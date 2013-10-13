using Loria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace L.O.R.I.A_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (LCore lCore = new LCore())
            {
                //lCore.Listen();
                lCore.EmulateAsk("Loria il est quelle heure");
                Console.Read();
            }
        }
        /*
    {
            Thread thread = new Thread(Start);
            thread.Start();

            Console.Read();
            stop = true;
        }

        static void Start()
        {
            // Create a new SpeechRecognitionEngine instance.
            SpeechRecognizer recognizer = new SpeechRecognizer();

            // Create a simple grammar that recognizes "red", "green", or "blue".
            Choices colors = new Choices();
            colors.Add(new string[] { "Allume la lumière", "vert", "bleu" });

            // Create a GrammarBuilder object and append the Choices object.
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(colors);

            // Create the Grammar instance and load it into the speech recognition engine.
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);

            // Register a handler for the SpeechRecognized event.
            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);

            while (!stop)
            {
                Thread.Sleep(500);
            }

            recognizer.Dispose();
        }

        // Create a simple handler for the SpeechRecognized event.
        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Speech recognized: " + e.Result.Text);
        }*/
    }
}
