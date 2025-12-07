using Personal_Finance_Tracker.Model;

namespace Personal_Finance_Tracker
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        // For Grid
        public static List<ClsTransaction> TransactionData = new List<ClsTransaction>();
        // For Original source 
        public static List<ClsTransaction> SourceTransactionData = new List<ClsTransaction>();

        public static int SourceOfData = 1; // default by DB
        public const int SourceDB = 1;
        public const int SourceImportFile = 2;
        public static string? MCurrentCSV { get; set; }
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

    }
}