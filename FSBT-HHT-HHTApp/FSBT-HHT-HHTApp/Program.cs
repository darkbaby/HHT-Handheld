using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Terranova.API;
using DNWA.BHTCL;
using System.Globalization;
using System.Threading;

namespace Denso_HHT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [DllImport("coredll.dll", EntryPoint = "FindWindowW", SetLastError = true)]
        private static extern IntPtr FindWindowCE(string lpClassName, string lpWindowName);

        [DllImport("coredll.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        private readonly static int SW_HIDE = 0;
        private readonly static int SW_SHOW = 5;

        private static IntPtr hWin;

        public static bool isNonRealtime = true;

        public static List<Image> imagelist = new List<Image>();

        public static string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

        public static string LastScannedLocationFront = "";

        public static string LastScannedBarcodeFront = "";

        public static string LastScannedLocationWarehouse = "";

        public static string LastScannedBarcodeWarehouse = "";

        public static string LastScannedLocationFreshFood = "";

        public static string LastScannedBarcodeFreshFood = "";

        public static string LastScannedLocationProduct = "";

        public static string LastScannedBarcodeProduct = "";

        public static string LastScannedLocationProductPack = "";

        public static string LastScannedBarcodeProductPack = "";

        [MTAThread]
        static void Main()
        {                       
            int countProgram = 0;
            ProcessInfo[] list = ProcessCE.GetProcesses();
            foreach (ProcessInfo item in list)
            {
                string nameProcess = item.FullPath;
                if (item.FullPath.Contains("TheMall"))
                {
                    countProgram++;
                    if (countProgram == 2)
                    {
                        return;
                    }
                }
            }

            hWin = FindWindowCE("HHTaskBar", null);
            int result = ShowWindow(hWin, SW_HIDE);

            for (int i = 0; i < 12; i++)
            {
                Image temp = new Bitmap(appPath + @"\GIF\frame_" + i + "_delay-0.1s.png");
                imagelist.Add(temp);
            }

            if (!Directory.Exists(appPath + @"\temp"))
            {
                Directory.CreateDirectory(appPath + @"\temp");
            }

            Application.Run(new MainMenu());

            if (Directory.Exists(appPath + @"\temp"))
            {
                Directory.Delete(appPath + @"\temp",true);
            }
            result = ShowWindow(hWin, SW_SHOW);
        }
    }
}