using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monaco;
using Windows.Storage.Pickers;

namespace Edam.UI.Catalog;

public class AppSession
{

    public static Window Window { get; set; }

    /// <summary>
    /// Get File.
    /// </summary>
    /// <param name="extensions">(optional) file extensions</param>
    /// <returns></returns>
    public static async Task<StorageFile?> GetFileAsync(string extensions = "*")
    {
        var filePicker = new FileOpenPicker();
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(Window);
        WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
        filePicker.FileTypeFilter.Add(extensions);
        var file = await filePicker.PickSingleFileAsync();
        return file;
    }

    /// <summary>
    /// Get File.
    /// </summary>
    /// <param name="extensions">(optional) file extensions</param>
    /// <returns></returns>
    public static async Task<StorageFolder?> GetFolderAsync(
        string extensions = "*")
    {
        var folderPicker = new FolderPicker();
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(Window);
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
        var folder = await folderPicker.PickSingleFolderAsync();
        return folder;
    }

}
