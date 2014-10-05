
namespace Loria.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (LoriaCore lCore = new LoriaCore())
            {
                lCore.StartListening();

                System.Console.Read();
            }
        }
    }
}
