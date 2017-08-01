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
		public static string LogPath;
		public static string userid;
		public static void pathcre()
		{
				userid=CurrentUser.RetreiveUserId().ToString();
				if (userid == "0")
				{
					if (CurrentUser.GuestId == "0" || CurrentUser.GuestId == null)
					{
						userid = "DefaultLogs";
					}
					else 
					{
						userid = "g_" + CurrentUser.GuestId;
					}
				}
				var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				//var cache = Path.Combine("Library/Caches/", "WineHangoutz");
				var filename = Path.Combine(documents, userid+".csv");
				LogPath = filename;
		}
		public static void Logtime(string time,string screen)
		{
			//pathcre();
			var csv = new StringBuilder();
			var newLine = string.Format("{0},{1},{2}", "Time", time, screen);
			csv.AppendLine(newLine);
			//try
			//{
			//	File.AppendAllText(LogPath, csv.ToString());
			//}
			//catch (Exception ex)
			//{ 
			//}
			string logg = csv.ToString();
			UploadAsyncLogs(logg);
			
		}
		public static void LogInfo(string info, string screen)
		{
			//pathcre();
			var csv = new StringBuilder();
			var newLine = string.Format("{0},{1},{2},{3}", "Information", DateTime.Now, info, screen);
			csv.AppendLine(newLine);
			//try
			//{
			//	File.AppendAllText(LogPath, csv.ToString());
			//}
			//catch (Exception ex)
			//{ 
			//}
			string logg = csv.ToString();
			UploadAsyncLogs(logg);
		}
		public static void LogError(string error, string screen, string lineerror)
		{
            //pathcre();
			var csv = new StringBuilder();
			var newLine = string.Format("{0},{1},{2},{3}", "Exception", DateTime.Now, error, screen);
			csv.AppendLine(newLine);
			//try
			//{
			//	File.AppendAllText(LogPath, csv.ToString());
			//}
			//catch (Exception ex)
			//{ 
			//}
			string logg = csv.ToString();
			UploadAsyncLogs(logg);
		}
		public static void LogServiceInfo(string Info, string servicename)
		{
            //pathcre();
			var csv = new StringBuilder();
			var newLine = string.Format("{0},{1},{2},{3}", "service", DateTime.Now, Info, servicename);
			csv.AppendLine(newLine);
			//try
			//{
			//	File.AppendAllText(LogPath, csv.ToString());
			//}
			//catch (Exception ex)
			//{ 
			//}
			string logg = csv.ToString();
			UploadAsyncLogs(logg);
		}
		public static async void UploadAsyncLogs(string log)
		{
			try
			{
				pathcre();
				StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
				CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
				CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
				CloudBlobContainer container = blobClient.GetContainerReference("userlogs");
				//await container.CreateIfNotExistsAsync();
				//File.AppendAllText(LogPath, log);
				CloudAppendBlob append = container.GetAppendBlobReference(userid+".csv");
				if (!await append.ExistsAsync())
				{
					await append.CreateOrReplaceAsync();
				}
				await append.AppendTextAsync(log);
			}
			catch (Exception ex)
			{
				//var csv = new StringBuilder();
				//var newLine = string.Format("{0},{1},{2},{3}", "Exception", DateTime.Now, ex.Message, "Logging class");
				//csv.AppendLine(newLine);
				//File.AppendAllText(LogPath, csv.ToString());
				//LoggingClass.LogError(ex.Message, "Upload Log Blobs", ex.StackTrace);
			}
		}
		public static async void UploadLogs()
		{
			try
			{
				StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
				CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
				CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
				CloudBlobContainer container = blobClient.GetContainerReference("detaileduserlogs");
				await container.CreateIfNotExistsAsync();
				CloudBlockBlob blob = container.GetBlockBlobReference(userid + ".csv");
				//LoggingClass.LogInfo("Updated profile picture",screenid);
				using (var fs = System.IO.File.Open(LogPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
				 {
			              await blob.UploadFromStreamAsync(fs);
				//LoggingClass.LogInfo("Profile picture uploaded into blob",screenid);
           		 }

			}
			catch (Exception ex)
			{
				//var csv = new StringBuilder();
				//var newLine = string.Format("{0},{1},{2},{3}", "Exception", DateTime.Now, ex.Message, "Logging class");
				//csv.AppendLine(newLine);
				//File.AppendAllText(LogPath, csv.ToString());
				//LoggingClass.LogError(ex.Message, "Upload Log Blobs", ex.StackTrace);
			}
		}
	}
}
