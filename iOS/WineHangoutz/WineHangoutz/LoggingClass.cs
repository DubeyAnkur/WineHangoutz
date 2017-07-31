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
		
		public static void LogInfo(string info, string screen)
		{
			var csv = new StringBuilder();
			var newLine = string.Format("{0},{1},{2},{3}", "Information", DateTime.Now, info, screen);
			csv.AppendLine(newLine);
			//File.AppendAllText(LogPath, csv.ToString());
			string logg = csv.ToString();
			UploadAsyncLogs(logg);
		}
		public static void LogError(string error, string screen, string lineerror)
		{
			var csv = new StringBuilder();
			var newLine = string.Format("{0},{1},{2},{3}", "Exception", DateTime.Now, error, screen);
			csv.AppendLine(newLine);
			//File.AppendAllText(LogPath, csv.ToString());
			string logg = csv.ToString();
			UploadAsyncLogs(logg);
		}
		public static void LogServiceInfo(string Info, string servicename)
		{
			var csv = new StringBuilder();
			var newLine = string.Format("{0},{1},{2},{3}", "service", DateTime.Now, Info, servicename);
			csv.AppendLine(newLine);
			//File.AppendAllText(LogPath, csv.ToString());
			string logg = csv.ToString();
			UploadAsyncLogs(logg);
		}

		public static async void UploadAsyncLogs(string log)
		{
			try
			{
				StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
				CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
				CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
				CloudBlobContainer container = blobClient.GetContainerReference("userlogs");
				//await container.CreateIfNotExistsAsync();
				string userid = CurrentUser.RetreiveUserId().ToString();
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
				CloudAppendBlob append = container.GetAppendBlobReference(userid+".csv");
				if (!await append.ExistsAsync())
				{
					await append.CreateOrReplaceAsync();
				}
				await append.AppendTextAsync(log);
			}
			catch (Exception ex)
			{
				//Console.WriteLine(ex.Message);
				LoggingClass.LogError(ex.Message, "Upload Log Blobs", ex.StackTrace);
			}
		}
	}
}
