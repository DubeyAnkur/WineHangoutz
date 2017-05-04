using System;
using System.Text;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Android.Util;

namespace WineHangouts
{
    public static class LoggingClass
    {
        static string logspath = CreateDirectoryForLogs();
        public static string CreateDirectoryForLogs()
        {
            try
            {
                App._dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory("WineHangouts"), "winehangouts/logs");

                if (!App._dir.Exists())
                {
                    App._dir.Mkdirs();
                }
                logspath = App._dir.ToString() + "/" + CurrentUser.getUserId() + ".csv";
            }
            catch (Exception exe)
            {
                Log.Error("Error", exe.Message);
            }
            return logspath;
        }
        public static void LogInfo(string info,int screenid)
        {
            try
            {
                var csv = new StringBuilder();
                var newLine = string.Format("{0},{1},{2},{3}", "Information", DateTime.Now, info,screenid);
                csv.AppendLine(newLine);
                File.AppendAllText(logspath, csv.ToString());
            }
            catch (Exception exe)
            {
                Log.Error("Error", exe.Message);
            }
        }
        public static void LogServiceInfo(string info, string servicename)
        {
            try
            {
                var csv = new StringBuilder();
                var newLine = string.Format("{0},{1},{2},{3}", "Service", DateTime.Now, info, servicename);
                csv.AppendLine(newLine);
                File.AppendAllText(logspath, csv.ToString());
            }
            catch (Exception exe)
            {
                Log.Error("Error", exe.Message);
            }
        }
        public static void LogError(string error,int screenid,string lineno)
        {
            try
            {
                var csv = new StringBuilder();
                var newLine = string.Format("{0},{1},{2},{3},{4}", "Exception", DateTime.Now, error,lineno,screenid);
                csv.AppendLine(newLine);
                File.AppendAllText(logspath, csv.ToString());
            }
            catch (Exception exe)
            {
                Log.Error("Error", exe.Message);
            }
        }
        public static async void UploadErrorLogs(string path)
        {
            try
            {
                StorageCredentials sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
                CloudStorageAccount storageaccount = new CloudStorageAccount(sc, true);
                CloudBlobClient blobClient = storageaccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("userlogs");

                await container.CreateIfNotExistsAsync();

                CloudBlockBlob blob = container.GetBlockBlobReference(CurrentUser.getUserId() + ".csv"); //(path);


                using (var fs = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                {
                    await blob.UploadFromStreamAsync(fs);
                }
            }
            catch (Exception exe)
            {
                Log.Error("Error", exe.Message);
            }
        }
    }
}