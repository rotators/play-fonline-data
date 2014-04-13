using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;
using PlayFOnline;
using PlayFOnline.Core;
using PlayFOnline.Scripts;

public class Script : IInstallScript
{
    public bool Install(string game, string tempDir, string installDir)
    {
            string filename = Path.Combine(tempDir, "TLA Mk2 Client.exe");
            string url = "https://downloader.disk.yandex.ru/disk/141c62a898a00de4458ecfd6c6adeb3a/534a333b/AOon8GHn2M9o9BoY0xWSCFNEVKJf2GRRHygsy5OF16UrwBMLfrpHUQ-k56jUr8ePiopA0jsOU7sDZwIZpHlexA%3D%3D?uid=0&filename=TLA%20Mk2%20Client.exe&disposition=attachment&hash=otY2WCTql9UNe62vR7OXQR5V7VcwdypR4oxI/MWV/14%3D&limit=0&content_type=application%2Fx-msdownload&fsize=579763028&hid=b3ffd84ff6bf27c913bed8a8efa93544&media_type=executable";

            ProgressDownloader downloader = new ProgressDownloader();
            downloader.Download(game, url, filename);

            if (!File.Exists(filename))
            {
                MessageBox.Show(filename + " not found after download!");
                return false;
            }

            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.WorkingDirectory = tempDir;
            procInfo.FileName = filename;

            Process proc = Process.Start(procInfo);
            System.Threading.Thread.Sleep(1000);
            while (!proc.HasExited)
            {
                Win32.GetChildWindows((IntPtr)proc.MainWindowHandle);
                List<IntPtr> handles = (List<IntPtr>)Win32.GetChildWindows((IntPtr)proc.MainWindowHandle).ToList();
                if (handles.Count() == 16)
                {
                    // Set path
                    //const uint WM_SETTEXT = 0x000C;
                    //Win32.SendMessageEx(handles[3], WM_SETTEXT, 0, installDir);

                    // Click install
                    const int BN_CLICKED = 245;
                    Win32.SendMessageEx(handles[10], BN_CLICKED, 0, 0);
                }
            }

            string tempInstall = Path.Combine(tempDir, "TLA Mk2 Client");

            if (!Directory.Exists(tempInstall))
            {
                MessageBox.Show(tempInstall + " not found!");
                return false;
            }

            Utils.DirectoryCopy(tempInstall, installDir, true);
            Directory.Delete(tempInstall, true);

            return true;
    }
}