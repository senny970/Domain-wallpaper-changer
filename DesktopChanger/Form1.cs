using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace DesktopChanger
{
    public partial class Form1 : Form
    {
        /*[DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSDisconnectSession(IntPtr hServer, int sessionId, bool bWait);
        const int WTS_CURRENT_SESSION = -1;
        static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;*/

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 2;
            CreateWallpapersDir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        static public string filename;
        static public string path;
        static public string copyPath;

        static void SetUpDefaultWallpaper()
        {
            string RegFile = Directory.GetCurrentDirectory() + "/" + "Default_Desktop_Wallpaper.reg";
            if (File.Exists(RegFile))
            {
                Process.Start(RegFile);
            }

            if (!File.Exists(RegFile))
            {
                MessageBox.Show("Файл Default_Desktop_Wallpaper.reg не найден!", "Wallpaper Changer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void LogOff()
        {
            DialogResult MBresult;
            MBresult = MessageBox.Show("Для того что бы изминения вступили в силу, нужно выйти с системы!\n\nВыйти с системы?", "Wallpaper Changer", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (MBresult == DialogResult.Yes)
            {
                //Process.Start(Environment.SystemDirectory + "/" + "logoff.exe");
                //File.Open(Environment.SystemDirectory + "/" + "logoff.exe", FileMode.Open);

                //if (!WTSDisconnectSession(WTS_CURRENT_SERVER_HANDLE, WTS_CURRENT_SESSION, false))
                //throw new Win32Exception();
                ExitWindowsEx(0, 0);
            }
        }
        static void SetUpWallpaper(string file)
        {
            string WallpaperPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "Wallpaper", null);
            if (WallpaperPath != null)
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System", "Wallpaper", file);
            }
            MessageBox.Show("Обои заданы! \n\n" + file, "Wallpaper Changer", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LogOff();
        }
        static public void CreateWallpapersDir()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + " /wallpapers"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/wallpapers");
            }
        }
        static void CopyFile(string path, string name)
        {
            CreateWallpapersDir();
            string appFolder = Directory.GetCurrentDirectory();
            copyPath = appFolder + "/wallpapers/" + name;
            File.Copy(path, copyPath, true);
            SetUpWallpaper(copyPath);
        }
        public static void OpenFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Image files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                filename = Path.GetFileName(path);
                CopyFile(path, filename);
            }
            else if (result == DialogResult.Cancel)
            {
                DialogResult MBresult;
                MBresult = MessageBox.Show("Задать обои по умолчанию?", "Wallpaper Changer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (MBresult == DialogResult.Yes)
                {
                    SetUpDefaultWallpaper();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetUpDefaultWallpaper();
            LogOff();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wallpaper Changer by Senny\n10.10.2019", "Wallpaper Changer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
