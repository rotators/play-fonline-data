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
		String filename = tempDir + Path.DirectorySeparatorChar + "FOUpdater.zip";
		PlayFO.frmDownload download = new PlayFO.frmDownload(game, "http://fode.eu/files/download/2-fonline-desert-europe-game-client/", filename);
		if (!download.IsDisposed)
			download.ShowDialog();

		if (!File.Exists(filename))
		{
			MessageBox.Show(filename + " not found after download!");
			return false;
		}

		ZipFile zip = ZipFile.Read(filename);
		zip.ExtractSelectedEntries("*.*", "", installDir);

		Process proc = Process.Start(installDir + "\\" + "FOUpdater.exe");
		while (!proc.HasExited)
		{
			if (GetWindowText.WindowContainsTextString("FOUpdater v0.2.1", "Update not needed") ||
				GetWindowText.WindowContainsTextString("FOUpdater v0.2.1", "Updated ")
				)
				proc.Kill();
		}
		return true;
	}
}