using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Ionic.Zip;
using PlayFO;
using PlayFO.Scripts;

public class Script : IInstallScript
{
    public bool Install(string game, string tempDir, string installDir)
    {
            String filename = tempDir + Path.DirectorySeparatorChar + "FOnlineReloaded-Full.zip";
            frmDownload download = new frmDownload(game, "http://www.fonline-reloaded.net/files/dl_fullclient.php", filename);
            if (!download.IsDisposed)
                download.ShowDialog();
            
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
            catch(ZipException ex)
            {
                MessageBox.Show(string.Format("Error when loading {0}: {1}", filename, ex.Message));
                return false;
            }

            zip.ToList().ForEach(entry =>
            {
                if (entry.FileName != "FOnline Reloaded/")
                {
                    entry.FileName = entry.FileName.Replace("FOnline Reloaded/", "");
                    entry.Extract(installDir);
                }
            });

            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.WorkingDirectory = installDir;
            procInfo.FileName = installDir + "\\" + "Updater.exe";
            Process proc = Process.Start(procInfo);
            while (!proc.HasExited)
            {
                IntPtr hwndChild = Win32.FindWindowEx((IntPtr)proc.MainWindowHandle, IntPtr.Zero, "TButton", "Check");

                const int BN_CLICKED = 245;
                Win32.SendMessage(hwndChild, BN_CLICKED, 0, 0);

                if(Win32.WindowContainsTextString(proc.MainWindowHandle, "Checking end."))
                    proc.Kill();
            }

            File.Delete(filename);

            return true;
    }
}


