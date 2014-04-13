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
		string filename = Path.Combine(tempDir, "Fonline2Season2.zip");
		string url = "http://www.mediafire.com/download/7k8l07d95ylpk8h/Fonline2Season2.zip";

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
			if (entry.FileName != "Fonline2/")
			{
				entry.FileName = entry.FileName.Replace("Fonline2/", string.Empty);
				entry.Extract(installDir, ExtractExistingFileAction.OverwriteSilently);
			}
		});
		zip.Dispose();

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