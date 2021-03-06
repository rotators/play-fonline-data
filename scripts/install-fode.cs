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
		string filename = Path.Combine(tempDir, "FOUpdater.zip");
		string url = "http://fode.eu/files/download/2-fonline-desert-europe-game-client/";

		ProgressDownloader downloader = new ProgressDownloader();
		downloader.Download(game, url, filename);

		if (!File.Exists(filename))
		{
			MessageBox.Show(filename + " not found after download!");
			return false;
		}

		ZipFile zip = ZipFile.Read(filename);
		zip.ExtractSelectedEntries("*.*", "", installDir, ExtractExistingFileAction.OverwriteSilently);

		Process proc = Process.Start(installDir + "\\" + "FOUpdater.exe");
		while (!proc.HasExited)
		{
			if (Win32.WindowContainsTextString("FOUpdater v0.2.1", "Update not needed") ||
				Win32.WindowContainsTextString("FOUpdater v0.2.1", "Updated ")
				)
				proc.Kill();
		}
		return true;
	}
}