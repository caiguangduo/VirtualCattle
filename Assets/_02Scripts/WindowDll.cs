#if UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using Ookii.Dialogs;
using System.Windows.Forms;
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public string filter;
    public string customFilter;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public IntPtr file;
    public int maxFile = 0;
    public string fileTitle;
    public int maxFileTitle = 0;
    public string initialDir;
    public string title;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public string defExt;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public string templateName;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}
public enum OpenFileNameFlags
{
    OFN_HIDEREADONLY = 0x4,
    OFN_FORCESHOWHIDDEN = 0x10000000,
    OFN_ALLOWMULTISELECT = 0x200,
    OFN_EXPLORER = 0x80000,
    OFN_FILEMUSTEXIST = 0x1000,
    OFN_PATHMUSTEXIST = 0x800,
    OFN_NOCHANGEDIR = 0x00000008
}
public class WindowDll
{

    [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int nCmdShow);

    public static string[] ShowOpenFileDialog(string dialogTitle, string filter, bool allowMultiSelect)
    {
        VistaOpenFileDialog dialog = new VistaOpenFileDialog();
        dialog.Title = dialogTitle;
        dialog.Filter = filter;// "All files (*.*)|*.*";"Text files (*.txt)|*.txt|All files (*.*)|*.*";
        dialog.Multiselect = allowMultiSelect;
        if (dialog.ShowDialog(new WindowWrapper(GetActiveWindow())) == DialogResult.OK)
        {
            if (allowMultiSelect)
                return dialog.FileNames;
            return new string[] { dialog.FileName };
        }
        else
        {
            return null;
        }
    }

    public static TaskDialogButton ShowWarningDialog(string dialogTitle, string content, string ensureString, string cancelString)
    {
        TaskDialog taskDialog = new TaskDialog();
        taskDialog.Content = content;
        taskDialog.WindowTitle = dialogTitle;
        TaskDialogButton button1 = new TaskDialogButton(ButtonType.Custom);
        button1.Text = ensureString;
        TaskDialogButton button2 = new TaskDialogButton(ButtonType.Custom);
        button2.Text = cancelString;
        taskDialog.Buttons.Add(button1);
        taskDialog.Buttons.Add(button2);
        return taskDialog.ShowDialog(new WindowWrapper(GetActiveWindow()));
    }

    public static string ShowSaveFileDialog(string dialogTitle, string filter, string defaultExt)
    {
        VistaSaveFileDialog dialog = new VistaSaveFileDialog();
        dialog.Title = dialogTitle;
        dialog.DefaultExt = defaultExt;
        dialog.Filter = filter;
        if (dialog.ShowDialog(new WindowWrapper(GetActiveWindow())) == DialogResult.OK)
        {
            return dialog.FileName;
        }
        else
        {
            return null;
        }
    }

    public static string ShowFolderBrowserDialog(string dialogTitle)
    {
        VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
        dialog.Description = dialogTitle;
        dialog.UseDescriptionForTitle = true;
        if (dialog.ShowDialog(new WindowWrapper(GetActiveWindow())) == DialogResult.OK)
        {
            return dialog.SelectedPath;
        }
        else
        {
            return null;
        }
    }
}

public class WindowWrapper : System.Windows.Forms.IWin32Window
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="handle">Handle to wrap</param>
    public WindowWrapper(IntPtr handle)
    {
        _hwnd = handle;
    }

    /// <summary>
    /// Original ptr
    /// </summary>
    public IntPtr Handle
    {
        get { return _hwnd; }
    }

    private IntPtr _hwnd;
}
#endif