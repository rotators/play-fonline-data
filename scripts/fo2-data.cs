using System;
using System.Windows.Forms;
using PlayFOnline.Scripts;
using DATLib;

public class Script : IResolveScript
{
	private bool FileExists(DAT dat, string fileName)
	{
		return dat.FileList.Exists(x => x.FileName.ToLower() == fileName);
	}

    public bool IsValidResource(string name, string filePath)
	{
	   DatReaderError status;
	   string datPath = filePath;
	   DAT loadedDat = DATReader.ReadDat(datPath, out status);
	   if (status.Error != DatError.Success)
	   {
		   MessageBox.Show("Error loading " + datPath + ": " + Environment.NewLine + status.Message);
		   return false;
	   }
	   bool valid = false;
	   if (name.ToLower() == "master.dat")
	   {
		   valid = (this.FileExists(loadedDat, "ai.txt") &&
					this.FileExists(loadedDat, "vault13.gam") &&
					this.FileExists(loadedDat, "maps.txt") &&
					this.FileExists(loadedDat, "adb001.frm") &&
					this.FileExists(loadedDat, "acavcol1.frm")
			   );
	   }
	   if (name.ToLower() == "critter.dat")
	   {
		   valid = (this.FileExists(loadedDat, "critters.lst") &&
					this.FileExists(loadedDat, "hanpwraa.frm") &&
					this.FileExists(loadedDat, "hanpwral.frm") &&
					this.FileExists(loadedDat, "hanpwrbi.frm") &&
					this.FileExists(loadedDat, "hanpwrbl.frm")
			   );
	   }
	   loadedDat.Close();
	   return valid;
	}
}
