using Microsoft.AspNetCore.Components.WebView.WindowsForms;

namespace Pos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Set the form to fullscreen and remove the window buttons
            this.WindowState = FormWindowState.Normal;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create and configure the BlazorWebView
            var bwv = new BlazorWebView()
            {
                Dock = DockStyle.Fill,
                HostPage = "wwwroot/index.html",
                Services = Startup.Services!,
                StartPath = "/"
            };

            // Add the Blazor component
            bwv.RootComponents.Add<Main>("#app");

            // Add the BlazorWebView to the form's controls
            Controls.Add(bwv);
        }
    }
}
