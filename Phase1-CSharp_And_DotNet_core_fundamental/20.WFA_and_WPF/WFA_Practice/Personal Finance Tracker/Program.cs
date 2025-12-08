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
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show("UI Exception: " + e.Exception.Message);
            // log it if needed
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            MessageBox.Show("Non-UI Exception: " + ex?.Message);
        }
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            // Catch ALL UI thread exceptions
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            // Catch ALL non-UI thread exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

    }
}