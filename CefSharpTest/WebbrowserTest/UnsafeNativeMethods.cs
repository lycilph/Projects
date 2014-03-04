using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WebbrowserTest
{
    [StructLayout(LayoutKind.Sequential)]
    public class COMRECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
     
        public COMRECT()
        {
        }

        public COMRECT(Rectangle r)
        {
            left = r.X;
            top = r.Y;
            right = r.Right;
            bottom = r.Bottom;
        }

        public COMRECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public static COMRECT FromXYWH(int x, int y, int width, int height)
        {
            return new COMRECT(x, y, x + width, y + height);
        }

        public override string ToString()
        {
            return String.Concat(new object[] { "Left = ", left, " Top ", top, " Right = ", right, " Bottom = ", bottom });
        }
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public int message;
        public IntPtr wParam;
        public IntPtr lParam;
        public int time;
        public int pt_x;
        public int pt_y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        public int x;
        public int y;
        public POINT()
        {
        }

        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    [StructLayout( LayoutKind.Sequential )]
	public sealed class tagOleMenuGroupWidths
	{
		[MarshalAs( UnmanagedType.ByValArray, SizeConst = 6 )]
		public int[] widths;
		public tagOleMenuGroupWidths()
		{
			widths = new int[6];
		}
	}

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(@"B722BCCB-4E68-101B-A2BC-00AA00404770"), ComVisible(true)]
    public interface IOleCommandTarget
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryStatus(ref Guid pguidCmdGroup, int cCmds, [In, Out] OLECMD prgCmds, [In, Out] IntPtr pCmdText);
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Exec(ref Guid pguidCmdGroup, int nCmdID, int nCmdexecopt,
             [In, MarshalAs(UnmanagedType.LPArray)] object[] pvaIn,
             [Out, MarshalAs(UnmanagedType.LPArray)] object[] pvaOut);
    }

    [StructLayout(LayoutKind.Sequential)]
    public class OLECMD
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cmdID;
        [MarshalAs(UnmanagedType.U4)]
        public int cmdf;
    }

    [Flags]
    public enum DOCHOSTUIFLAG
    {
        DOCHOSTUIFLAG_DIALOG = 0x00000001,
        DOCHOSTUIFLAG_DISABLE_HELP_MENU = 0x00000002,
        DOCHOSTUIFLAG_NO3DBORDER = 0x00000004,
        DOCHOSTUIFLAG_SCROLL_NO = 0x00000008,
        DOCHOSTUIFLAG_DISABLE_SCRIPT_INACTIVE = 0x00000010,
        DOCHOSTUIFLAG_OPENNEWWIN = 0x00000020,
        DOCHOSTUIFLAG_DISABLE_OFFSCREEN = 0x00000040,
        DOCHOSTUIFLAG_FLAT_SCROLLBAR = 0x00000080,
        DOCHOSTUIFLAG_DIV_BLOCKDEFAULT = 0x00000100,
        DOCHOSTUIFLAG_ACTIVATE_CLIENTHIT_ONLY = 0x00000200,
        DOCHOSTUIFLAG_OVERRIDEBEHAVIORFACTORY = 0x00000400,
        DOCHOSTUIFLAG_CODEPAGELINKEDFONTS = 0x00000800,
        DOCHOSTUIFLAG_URL_ENCODING_DISABLE_UTF8 = 0x00001000,
        DOCHOSTUIFLAG_URL_ENCODING_ENABLE_UTF8 = 0x00002000,
        DOCHOSTUIFLAG_ENABLE_FORMS_AUTOCOMPLETE = 0x00004000,
        DOCHOSTUIFLAG_ENABLE_INPLACE_NAVIGATION = 0x00010000,
        DOCHOSTUIFLAG_IME_ENABLE_RECONVERSION = 0x00020000,
        DOCHOSTUIFLAG_THEME = 0x00040000,
        DOCHOSTUIFLAG_NOTHEME = 0x00080000,
        DOCHOSTUIFLAG_NOPICS = 0x00100000,
        DOCHOSTUIFLAG_NO3DOUTERBORDER = 0x00200000,
        DOCHOSTUIFLAG_DISABLE_EDIT_NS_FIXUP = 0x00400000,
        DOCHOSTUIFLAG_LOCAL_MACHINE_ACCESS_CHECK = 0x00800000,
        DOCHOSTUIFLAG_DISABLE_UNTRUSTEDPROTOCOL = 0x01000000
    }

    [StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public class DOCHOSTUIINFO
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;
        [MarshalAs(UnmanagedType.I4)]
        public int dwFlags;
        [MarshalAs(UnmanagedType.I4)]
        public int dwDoubleClick;
        [MarshalAs(UnmanagedType.I4)]
        public int dwReserved1;
        [MarshalAs(UnmanagedType.I4)]
        public int dwReserved2;
        public DOCHOSTUIINFO()
        {
            cbSize = Marshal.SizeOf(typeof(DOCHOSTUIINFO));
        }
    }

    public static class SRESULTS
    {
        public static readonly int S_OK = 0;
        public static readonly int S_FALSE = 1;
    }

    public sealed class UnsafeNativeMethods
    {
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(@"3050f3f0-98b5-11cf-bb82-00aa00bdce0b")]
        internal interface ICustomDoc
        {
            [PreserveSig]
            void SetUIHandler(IDocHostUIHandler pUIHandler);
        }

        [ComImport, Guid(@"00000117-0000-0000-C000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleInPlaceActiveObject
        {
            [PreserveSig]
            int GetWindow(out IntPtr hwnd);
            void ContextSensitiveHelp(int fEnterMode);
            [PreserveSig]
            int TranslateAccelerator([In] ref MSG lpmsg);
            void OnFrameWindowActivate(bool fActivate);
            void OnDocWindowActivate(int fActivate);
            void ResizeBorder([In] COMRECT prcBorder, [In] IOleInPlaceUIWindow pUIWindow, bool fFrameWindow);
            void EnableModeless(int fEnable);
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid(@"00000115-0000-0000-C000-000000000046")]
        public interface IOleInPlaceUIWindow
        {
            IntPtr GetWindow();
            [PreserveSig]
            int ContextSensitiveHelp(int fEnterMode);
            [PreserveSig]
            int GetBorder([Out] COMRECT lprectBorder);
            [PreserveSig]
            int RequestBorderSpace([In] COMRECT pborderwidths);
            [PreserveSig]
            int SetBorderSpace([In] COMRECT pborderwidths);
            void SetActiveObject([In, MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject pActiveObject, [In, MarshalAs(UnmanagedType.LPWStr)] string pszObjName);
        }

        [ComImport, Guid(@"00000116-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleInPlaceFrame
        {
            IntPtr GetWindow();
            [PreserveSig]
            int ContextSensitiveHelp(int fEnterMode);
            [PreserveSig]
            int GetBorder([Out] COMRECT lprectBorder);
            [PreserveSig]
            int RequestBorderSpace([In] COMRECT pborderwidths);
            [PreserveSig]
            int SetBorderSpace([In] COMRECT pborderwidths);
            [PreserveSig]
            int SetActiveObject([In, MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject pActiveObject, [In, MarshalAs(UnmanagedType.LPWStr)] string pszObjName);
            [PreserveSig]
            int InsertMenus([In] IntPtr hmenuShared, [In, Out] tagOleMenuGroupWidths lpMenuWidths);
            [PreserveSig]
            int SetMenu([In] IntPtr hmenuShared, [In] IntPtr holemenu, [In] IntPtr hwndActiveObject);
            [PreserveSig]
            int RemoveMenus([In] IntPtr hmenuShared);
            [PreserveSig]
            int SetStatusText([In, MarshalAs(UnmanagedType.LPWStr)] string pszStatusText);
            [PreserveSig]
            int EnableModeless(bool fEnable);
            [PreserveSig]
            int TranslateAccelerator([In] ref MSG lpmsg, [In, MarshalAs(UnmanagedType.U2)] short wID);
        }

        [ComImport, Guid(@"00000122-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleDropTarget
        {
            [PreserveSig]
            int OleDragEnter([In, MarshalAs(UnmanagedType.Interface)] object pDataObj, [In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In, MarshalAs(UnmanagedType.U8)] long pt, [In, Out] ref int pdwEffect);
            [PreserveSig]
            int OleDragOver([In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In, MarshalAs(UnmanagedType.U8)] long pt, [In, Out] ref int pdwEffect);
            [PreserveSig]
            int OleDragLeave();
            [PreserveSig]
            int OleDrop([In, MarshalAs(UnmanagedType.Interface)] object pDataObj, [In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In, MarshalAs(UnmanagedType.U8)] long pt, [In, Out] ref int pdwEffect);
        }

        [ComImport, Guid(@"BD3F23C0-D43E-11CF-893B-00AA00BDCE1A"), ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IDocHostUIHandler
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int ShowContextMenu([In, MarshalAs(UnmanagedType.U4)] int dwID, [In] POINT pt, [In, MarshalAs(UnmanagedType.Interface)] object pcmdtReserved, [In, MarshalAs(UnmanagedType.Interface)] object pdispReserved);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetHostInfo([In, Out] DOCHOSTUIINFO info);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int ShowUI([In, MarshalAs(UnmanagedType.I4)] int dwID, [In] IOleInPlaceActiveObject activeObject, [In] IOleCommandTarget commandTarget, [In] IOleInPlaceFrame frame, [In] IOleInPlaceUIWindow doc);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int HideUI();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int UpdateUI();
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int EnableModeless([In, MarshalAs(UnmanagedType.Bool)] bool fEnable);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int OnDocWindowActivate([In, MarshalAs(UnmanagedType.Bool)] bool fActivate);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int OnFrameWindowActivate([In, MarshalAs(UnmanagedType.Bool)] bool fActivate);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int ResizeBorder([In] COMRECT rect, [In] IOleInPlaceUIWindow doc, bool fFrameWindow);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int TranslateAccelerator([In] ref MSG msg, [In] ref Guid group, [In, MarshalAs(UnmanagedType.I4)] int nCmdID);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetOptionKeyPath([Out, MarshalAs(UnmanagedType.LPArray)] string[] pbstrKey, [In, MarshalAs(UnmanagedType.U4)] int dw);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetDropTarget([In, MarshalAs(UnmanagedType.Interface)] IOleDropTarget pDropTarget, [MarshalAs(UnmanagedType.Interface)] out IOleDropTarget ppDropTarget);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetExternal([MarshalAs(UnmanagedType.Interface)] out object ppDispatch);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int TranslateUrl([In, MarshalAs(UnmanagedType.U4)] int dwTranslate, [In, MarshalAs(UnmanagedType.LPWStr)] string strURLIn, [MarshalAs(UnmanagedType.LPWStr)] out string pstrURLOut);
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int FilterDataObject(IDataObject pDO, out IDataObject ppDORet);
        }
    }
}
