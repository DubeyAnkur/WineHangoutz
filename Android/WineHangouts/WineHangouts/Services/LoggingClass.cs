using System;
using System.Text;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Android.Util;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using Android.App;

namespace WineHangouts
{
    public static class LoggingClass
    {
        //static string logspath = CreateDirectoryForLogs();
		public static StorageCredentials sc;
		public static CloudStorageAccount storageaccount;
		public static CloudBlobClient blobClient;
		public static CloudBlobContainer container;
		public static CloudAppendBlob append;
        public static string userid=CurrentUser.getUserId();
        public static void pathcre()
        {
            if (userid == "0"||userid==null)
            {
                userid =CurrentUser.GuestId;
                if (userid == "0"|| userid == null)
                {
                    userid = "DefaultLogs";
                }
            }
        }
		public static async void UploadErrorLogs()
		{
            pathcre();
			try
			{

				sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
				storageaccount = new CloudStorageAccount(sc, true);
				blobClient = storageaccount.CreateCloudBlobClient();
				container = blobClient.GetContainerReference("userlogs");

				//var csv = new StringBuilder();
				//var newLine = string.Format("Exception", DateTime.Now, "Test", "Test", "Test");
				//csv.AppendLine(newLine);
				//File.AppendAllText(logspath, csv.ToString());

				await container.CreateIfNotExistsAsync();
				//CloudBlockBlob blob = container.GetBlockBlobReference(CurrentUser.getUserId() + ".csv"); //(path);
				//if (CurrentUser.getUserId() == null)
				//{
					append = container.GetAppendBlobReference(userid+".csv");
				//}
				//else
				//{
				//	append = container.GetAppendBlobReference(CurrentUser.getUserId() + ".csv");
				//}
				if (!await append.ExistsAsync())
				{
					await append.CreateOrReplaceAsync();
				}


				//await append.AppendBlockAsync("aksfhgaUKGdfkAUSFDAUSGFD");
				//await append.UploadTextAsync(string.Format("Exception,Test,Test,Test"));
				//await append.AppendTextAsync(  string.Format("Exception,Test1,Test1,Test1 "+"\n"));
				//await append.AppendTextAsync(string.Format("{0},{1},{2},{3},{4}", "Exception", DateTime.Now, lineno, screenid + "\n"));

				//using (var fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
				//            {
				//                await blob.UploadFromStreamAsync(fs);
				//            }
			}
			catch (Exception exe)
			{
				Log.Error("Error", exe.Message);
			}
			
		}
		//public static string CreateDirectoryForLogs()
  //      {
  //          try
  //          {
  //              App._dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory("WineHangouts"), "winehangouts/logs");

  //              if (!App._dir.Exists())
  //              {
  //                  App._dir.Mkdirs();
  //              }
  //              logspath = App._dir.ToString() + "/" + CurrentUser.getUserId() + ".csv";
  //          }
  //          catch (Exception exe)
  //          {
  //              Log.Error("Error", exe.Message);
  //          }
  //          return logspath;
  //      }
        public static void LogInfo(string info,int screenid)
		{
            pathcre();
            try
			{
				
				append = container.GetAppendBlobReference(userid + ".csv");
				DateTime date1 = DateTime.UtcNow;
			
			append.AppendTextAsync(string.Format("{0},{1},{2},{3}", "Info", date1.ToString("MM/dd/yyyy hh:mm:ss.fff"), info, screenid + "\n"));

			}
			catch (Exception exe)
			{
				Log.Error("Error", exe.Message);
			}

		}
		public static  void LogInfoEx(string info,int screenid)
		{
            pathcre();
            try
			{
				
				DateTime date = DateTime.UtcNow;
				append = container.GetAppendBlobReference(userid + ".csv");
				var tasks = new Task[1];
				for (int i = 0; i < 1; i++)


					tasks[i] =  append.AppendTextAsync(string.Format("{0},{1},{2},{3}", "Info", date.ToString("MM/dd/yyyy hh:mm:ss.fff"), info,screenid + "\n"));
				Task.WaitAll(tasks);

			}
			catch (Exception exe)
			{
				Log.Error("Error", exe.Message);
			}
		}
		public static void LogServiceInfo(string info, string servicename)
        {
            pathcre();
            try
            {
				UploadErrorLogs();
		        append = container.GetAppendBlobReference(userid + ".csv");
				DateTime date1 = DateTime.UtcNow;
				var tasks = new Task[1];
				for (int i = 0; i < 1; i++)


					tasks[i]= append.AppendTextAsync(string.Format("{0},{1},{2},{3}", "Service", date1.ToString("MM/dd/yyyy hh:mm:ss.fff"), info, servicename +"\n"));
				Task.WaitAll(tasks);
				//var csv = new StringBuilder();
				//            var newLine = string.Format("{0},{1},{2},{3}", "Service", DateTime.Now, info, servicename);
				//            csv.AppendLine(newLine);
				//            File.AppendAllText(logspath, csv.ToString());
				//await append.AppendTextAsync(string.Format("{0},{1},{2},{3},{4}", "Exception", DateTime.Now, lineno, screenid + "\n"));
			}
            catch (Exception exe)
            {
                Log.Error("Error", exe.Message);
            }
        }
        public static void LogError(string error,int screenid,string lineno)
        {
            pathcre();
            try
            {
				append = container.GetAppendBlobReference(userid+ ".csv");
				DateTime date2 = DateTime.UtcNow;
				append.AppendTextAsync(string.Format("{0},{1},{2},{3},{4}", "Exception", date2.ToString("MM/dd/yyyy hh:mm:ss.fff"), error, lineno, screenid + "\n"));
				//var csv = new StringBuilder();
				//var newLine = string.Format("{0},{1},{2},{3},{4}", "Exception", DateTime.Now, error,lineno,screenid);
				//csv.AppendLine(newLine);
				//File.AppendAllText(logspath, csv.ToString());
				
			}
            catch (Exception exe)
            {
                Log.Error("Error", exe.Message);
            }
        }
		public static void LogTime(string info,string time)
		{
            pathcre();
            try
			{
				append = container.GetAppendBlobReference(CurrentUser.getUserId() + ".csv");
				DateTime date2 = DateTime.UtcNow;
				append.AppendTextAsync(string.Format("{0},{1},{2},{3}", "Time", date2.ToString("MM/dd/yyyy hh:mm:ss.fff"), info, time+ "\n"));
				//var csv = new StringBuilder();
				//var newLine = string.Format("{0},{1},{2},{3},{4}", "Exception", DateTime.Now, error,lineno,screenid);
				//csv.AppendLine(newLine);
				//File.AppendAllText(logspath, csv.ToString());

			}
			catch (Exception exe)
			{
				Log.Error("Error", exe.Message);
			}
		}
	}
}