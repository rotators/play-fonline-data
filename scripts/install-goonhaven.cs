using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using SharpCompress;
using SharpCompress.Reader;
using SharpCompress.Common;
using PlayFOnline;
using PlayFOnline.Core;
using PlayFOnline.Scripts;

public class Script : IInstallScript
{
    public bool Install(string game, string tempDir, string installDir)
    {
		string filename = Path.Combine(tempDir, "Goonhaven.rar");
		string url = "http://goon-haven.ru/downloads/GoonHaven.rar";

		ProgressDownloader downloader = new ProgressDownloader();
		downloader.Download(game, url, filename);

		if (!File.Exists(filename))
		{
			MessageBox.Show(filename + " not found after download!");
			return false;
		}

		using (Stream stream = File.OpenRead(filename))
		{
			var reader = ReaderFactory.Open(stream);
			while (reader.MoveToNextEntry())
			{
				if (!reader.Entry.IsDirectory)
				{
					string path = Path.Combine(installDir, reader.Entry.FilePath.Replace("Client\\", ""));
					path = path.Replace(Path.GetFileName(path), "");
					if (!Directory.Exists(path))
						Directory.CreateDirectory(path);
					reader.WriteEntryToDirectory(path, ExtractOptions.Overwrite);
				}
			}
		}
		return true;
    }
}