namespace WinFormsApp1
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
        }

        public static void SendKeys(string keys)
        {
            System.Windows.Forms.SendKeys.SendWait(keys);
        }
    }
}