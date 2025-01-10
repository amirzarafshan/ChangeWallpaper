using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
//using System.Windows.Forms;

namespace ChangeWallpaper
{

    public class Data
    {
        public string wallpaper_changed = "true";
    }

    public class Fail
    {
        public string wallpaper_changed = "false";
    }

    public class ReportTrue
    {
        public string status { get; set; }

        public Data data;

        public int version { get; set; }

        public string name { get; set; }

        public DateTime creatdate { get; set; }

    }

    public class ReportFalse
    {
        public string status { get; set; }

        public Fail data;

        public int version { get; set; }

        public string name { get; set; }

        public DateTime creatdate { get; set; }

    }

    public sealed class Program
    {

        private static readonly int COLOR_DESKTOP = 1;
        private static readonly UInt32 SPI_SETDESKWALLPAPER = 0x14;
        private static readonly UInt32 SPIF_UPDATEINIFILE = 0x01;
        private static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;
        private const UInt32 SPI_GETDESKWALLPAPER = 0x73;

        [DllImport("user32.dll")]
        public static extern bool SetSysColors(int cElements, int[] lpaElements, int[] lpaRgbValues);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);
        private static readonly string RXMwallpaper = "rxm-wp.bmp";
        private static readonly string path = @"C:\Windows\Web\Wallpaper";
        private static readonly string pathOther = @"C:\Windows\Web\Wallpaper\Windows";
        private const int MAX_PATH = 260;

        public static void Main(string[] args)
        {
            SetSolidColor(Color.Black);
            SetWallpaper(GetRXMWallpaperPath(),Style.Center,2);
            CheckRXMWallpaperSet();
        }

        public static string GetRXMWallpaperPath()
        {
            string WallpaperPath = "";
            try
            {
                if (File.Exists(RXMwallpaper))
                {
                    //OS: WinXP or Vista   
                    if (Directory.Exists(path))
                    {
                        File.Copy(Path.Combine(Environment.CurrentDirectory, RXMwallpaper), Path.Combine(path, RXMwallpaper), true);
                        WallpaperPath = Path.Combine(path, RXMwallpaper);
                    }
                    //OS: other
                    else
                    {
                        File.Copy(Path.Combine(Environment.CurrentDirectory, RXMwallpaper), Path.Combine(pathOther, RXMwallpaper), true);
                        WallpaperPath = Path.Combine(pathOther, RXMwallpaper);
                    }
                }
                return WallpaperPath;
            
             }
             catch
             {
                //ignore
                return null;
             }
        }

        public enum Style : int
        {
            Tile,
            Center,
            Strech,
            Fill,
            Fit
        }

        public static void SetSolidColor(Color color)
        {
            // Remove the current wallpaper
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, "", SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            // Set the new desktop solid color for the current session
            int[] elements = {COLOR_DESKTOP};
            int[] colors = {ColorTranslator.ToWin32(color)};
            SetSysColors(elements.Length, elements, colors);

            // Save value in registry so that it will persist
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Colors", true);
            key.SetValue(@"Background", string.Format("{0} {1} {2}", color.R, color.G, color.B));
        }

        public static void SetWallpaper(String path, Style style, int attemptToRun)
        {
            do
            {
                    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
                    switch (style)
                    {
                        case Style.Center:
                            key.SetValue(@"WallpaperStyle", "0");
                            key.SetValue(@"TileWallpaper", "0");
                            break;

                        case Style.Tile:
                            key.SetValue(@"WallpaperStyle", "0");
                            key.SetValue(@"TileWallpaper", "1");
                            break;

                        case Style.Strech:
                            key.SetValue(@"WallpaperStyle", "2");
                            key.SetValue(@"TileWallpaper", "0");
                            break;

                        //Win 7 and later
                        case Style.Fill:
                            key.SetValue(@"WallpaperStyle", "10");
                            key.SetValue(@"TileWallpaper", "0");
                            break;
                      
                        case Style.Fit:
                            key.SetValue(@"WallpaperStyle", "6");
                            key.SetValue(@"TileWallpaper", "0");
                            break;
                    }
                    key.Close();
                attemptToRun--;

            } while (attemptToRun > 0);
        }

        public static string GetCurrentWallpaper()
        {
            string currentWallpaper = new string('\0', MAX_PATH);
            SystemParametersInfo(SPI_GETDESKWALLPAPER, Convert.ToUInt32(currentWallpaper.Length), currentWallpaper, 0);
            return currentWallpaper.Substring(0, currentWallpaper.IndexOf('\0'));
        }

        public static bool CheckRXMWallpaperSet()
        {
            if (GetCurrentWallpaper().Equals(Path.Combine(path, RXMwallpaper)) || GetCurrentWallpaper().Equals(Path.Combine(pathOther, RXMwallpaper)))
            {
                using (StreamWriter writer = new StreamWriter("status.rxmr"))
                {
                    writer.WriteLine(GenerateReportSuccess());
                    return true;
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter("status.rxmr"))
                {
                    writer.WriteLine(GenerateReportFail());
                    return true;
                }

            }
        }

        public static string GenerateReportSuccess()
        {
            Data data = new Data();

            ReportTrue report = new ReportTrue();
            {
                report.status = "success";
                report.data = data;
                report.version = 1;
                report.name = "changeWallpaper_status";
                report.creatdate = DateTime.Now.ToLocalTime();
            }

            return JsonConvert.SerializeObject(report).ToString();

        }

        public static string GenerateReportFail()
        {
            Fail fail = new Fail();

            ReportFalse report = new ReportFalse();
            {
                report.status = "fail";
                report.data = fail;
                report.version = 1;
                report.name = "changeWallpaper_status";
                report.creatdate = DateTime.Now.ToLocalTime();
            }

            return JsonConvert.SerializeObject(report).ToString();

        }

    }
}
