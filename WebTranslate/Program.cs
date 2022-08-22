using YamlDotNet.Serialization;

namespace Ilyfairy.Tools.WinFormTranslate
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            bool isHide = args.Any(v => string.Equals(v, "-nogui", StringComparison.OrdinalIgnoreCase));
            MainForm mainForm = new(isHide);
            
            Application.Run(mainForm);
        }
    }
}