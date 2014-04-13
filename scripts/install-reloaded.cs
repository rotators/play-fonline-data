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
            string filename = Path.Combine(tempDir, "FOnlineReloaded-Full.zip");
            string url = "http://www.fonline-reloaded.net/files/dl_fullclient.php";

            ProgressDownloader downloader = new ProgressDownloader();
            downloader.Download(game, url, filename);

            if (!File.Exists(filename))
            {
                MessageBox.Show(filename + " not found after download!");
                return false;
            }

            ZipFile zip;
            try
            {
                zip = ZipFile.Read(filename);
            }
            catch (ZipException ex)
            {
                MessageBox.Show(string.Format("Error when loading {0}: {1}", filename, ex.Message));
                return false;
            }

            zip.ToList().ForEach(entry =>
            {
                if (entry.FileName != "FOnline Reloaded/")
                {
                    entry.FileName = entry.FileName.Replace("FOnline Reloaded/", string.Empty);
                    entry.Extract(installDir, ExtractExistingFileAction.OverwriteSilently);
                }
            });
            zip.Dispose();

            string updater = Path.Combine(installDir, "Updater.exe");

            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.WorkingDirectory = installDir;
            procInfo.FileName = updater;
            if (!File.Exists(updater))
            {
                MessageBox.Show("{0} not found, can't update!", updater);
                return false;
            }

            Process proc = Process.Start(procInfo);
            while (!proc.HasExited)
            {
                IntPtr hwndChild = Win32.FindWindowEx((IntPtr)proc.MainWindowHandle, IntPtr.Zero, "TButton", "Check");

                const int BN_CLICKED = 245;
                Win32.SendMessage(hwndChild, BN_CLICKED, 0, 0);

                if (Win32.WindowContainsTextString(proc.MainWindowHandle, "Checking end."))
                    proc.Kill();
            }
            try
            {
                File.Delete(filename);
            }
            catch (IOException ex)
            {
                MessageBox.Show(string.Format("Error when deleting {0}: {1}", filename, ex.Message));
            }
            return true;
    }
}


