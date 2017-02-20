using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Hangout.Models;

namespace WineHangouts
{
    [Activity(Label = "Testing App", MainLauncher =false, Icon = "@drawable/icon")]
    public class TestingActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


        //    public static async Task<bool> SaveCache(Stream data, string id)
        //{
        //    try
        //    {
        //        //cache folder in local storage
        //        IFolder rootFolder = FileSystem.Current.LocalStorage;
        //        var folder = await rootFolder.CreateFolderAsync("Cache",
        //            CreationCollisionOption.OpenIfExists);
        //        //save cached data
        //        IFile file = await folder.CreateFileAsync(id, CreationCollisionOption.ReplaceExisting);
        //        byte[] buffer = new byte[data.Length];
        //        data.Read(buffer, 0, buffer.Length);
        //        using (Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
        //        {
        //            stream.Write(buffer, 0, buffer.Length);
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        //some logging
        //        return false;
        //    }
        //}

        //public static async Task<Stream> LoadCache(string id)
        //{
        //    //cache folder in local storage
        //    IFolder rootFolder = FileSystem.Current.LocalStorage;
        //    var folder = await rootFolder.CreateFolderAsync("Cache",
        //        CreationCollisionOption.OpenIfExists);

        //    var isExists = await folder.CheckExistsAsync(id);

        //    if (isExists == ExistenceCheckResult.FileExists)
        //    {
        //        //file exists - load it from cache
        //        IFile file = await folder.GetFileAsync(id);
        //        return await file.OpenAsync(FileAccess.Read);
        //    }
        //}
        //    return null;
        //}
}
    }
}