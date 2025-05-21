using MaterialSkin;
using MaterialSkin.Controls;

using System;
using System.Windows.Forms;

namespace Management.Forms.LoginFor
{
    public partial class LoginForm : MaterialForm
    {
      
        public LoginForm()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue400, Primary.Blue500,
                Primary.Blue500, Accent.LightBlue200,
                TextShade.WHITE);
        }
    }
}
