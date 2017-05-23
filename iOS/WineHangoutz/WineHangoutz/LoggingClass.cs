using System;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WineHangoutz
{
	public static class LoggingClass
	{
		//public static DirectoryInfo CreateDirectory(String path)
		//{

		//}

		public static string LogPath = LoggingPathCreator();
		public static string LoggingPathCreator()
		{
			string LPath;
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var directoryname = Path.Combine(documents, "WH Logs");  // LogPath.Combine(documents, "NewDirectory");
			Directory.CreateDirectory(directoryname);

			LPath = directoryname;
			LPath = LPath + "/" + CurrentUser.RetreiveUserId() + ".csv";
			return LPath;
			//return LogPath;
		}
		public static void LogInfo(string info, int screenid)
		{
			var csv = new StringBuilder();
			var newLine = string.Format("{0},{1},{2},{3}", "Information", DateTime.Now, info, screenid);
			csv.AppendLine(newLine);
			File.AppendAllText(LogPath, csv.ToString());
		}
		public static void LogError(string error, int screenid, string lineerror)
		{
			var csv = new StringBuilder();
			var newLine = string.Format("{0},{1},{2},{3}", "Exception", DateTime.Now, error, screenid);
			csv.AppendLine(newLine);
			File.AppendAllText(LogPath, csv.ToString());
		}
		public static void LogServiceInfo(string Info, string servicename)
		{
			var csv = new StringBuilder();
			var newLine = string.Format("{0},{1},{2},{3}", "service", DateTime.Now, Info, servicename);
			csv.AppendLine(newLine);
			File.AppendAllText(LogPath, csv.ToString());
		}
		public static async void UploadErrorLogs()
		{
			try
			{
				StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
				CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
				CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
				CloudBlobContainer container = blobClient.GetContainerReference("userlogs");

				await container.CreateIfNotExistsAsync();

				CloudBlockBlob blob = container.GetBlockBlobReference(CurrentUser.RetreiveUserId() + ".csv"); //(path);

				CloudAppendBlob append = container.GetAppendBlobReference(CurrentUser.RetreiveUserId()+".csv");
				if (!await append.ExistsAsync())
				{
					await append.CreateOrReplaceAsync();
				}
				//await append.AppendBlockAsync("aksfhgaUKGdfkAUSFDAUSGFD");
				//await append.UploadTextAsync(string.Format("Exception,Test,Test,Test"));
				await append.AppendTextAsync(string.Format("Exception,Test1,Test1,Test1"));

				using (var fs = System.IO.File.Open(LogPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
				{

					await blob.UploadFromStreamAsync(fs);

				}
			}
			catch (Exception exe)
			{
				
			}
		}
	}
}
