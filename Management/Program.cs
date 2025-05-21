using Google.Protobuf.WellKnownTypes;
using Management.Forms.LoginFor;

namespace Management
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
            //AntdUI.Localization.DefaultLanguage = "zh-CN";
            ////若文字不清晰，切换其他渲染方式
            //AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            //AntdUI.Config.TextRenderingHighQuality = true;

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //App.Run(services =>
            //{
            //    services.AddControlServices(Assembly.GetExecutingAssembly());
            //});
        }
    }
}