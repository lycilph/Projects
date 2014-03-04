using mshtml;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Navigation;
namespace MetroWebbrowser.Shell.Views
{
    public partial class ShellView : UnsafeNativeMethods.IDocHostUIHandler
    {
        public ShellView()
        {
            InitializeComponent();

            browser.LoadCompleted += OnLoadCompleted;
        }

        private void OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            //var doc = browser.Document as UnsafeNativeMethods.ICustomDoc;
            //if (doc != null)
            //    doc.SetUIHandler(this);

            var assembly = Assembly.GetExecutingAssembly();
            var resource = assembly.GetManifestResourceNames();

            var s = assembly.GetManifestResourceStream("MetroWebbrowser.Resources.jquery.jscrollpane.css");
            var sr = new StreamReader(s);
            var text = sr.ReadToEnd();

            var doc = browser.Document as IHTMLDocument2;
            var css = doc.createStyleSheet();
            css.cssText = text;
            css.media = "all";

            var css2 = doc.createStyleSheet();
            css2.cssText = ".scroll-pane { width: 100%; height: 200px; overflow: auto; }";

            var script = (IHTMLScriptElement)doc.createElement("SCRIPT");
            script.type = "text/javascript";
            script.src = @"http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js";

            var script2 = (IHTMLScriptElement)doc.createElement("SCRIPT");
            script2.type = "text/javascript";
            script2.text = "$(function() { $('.body').jScrollPane(); });";

            var doc3 = browser.Document as IHTMLDocument3;
            var nodes = doc3.getElementsByTagName("head");

            foreach (var node in nodes)
            {
                var head = (IHTMLDOMNode)node;

                head.appendChild((IHTMLDOMNode)script);
                head.appendChild((IHTMLDOMNode)script2);
            }

            var a = doc3.documentElement.innerHTML;
        }

        public int ShowContextMenu(int dwID, POINT pt, object pcmdtReserved, object pdispReserved)
        {
            return SRESULTS.S_FALSE;
        }

        public int GetHostInfo(DOCHOSTUIINFO info)
        {
            info.cbSize = Marshal.SizeOf(typeof(DOCHOSTUIINFO));
            info.dwFlags = (int)(DOCHOSTUIFLAG.DOCHOSTUIFLAG_NO3DOUTERBORDER |
                                 DOCHOSTUIFLAG.DOCHOSTUIFLAG_NO3DBORDER |
                                 DOCHOSTUIFLAG.DOCHOSTUIFLAG_FLAT_SCROLLBAR |
                                 DOCHOSTUIFLAG.DOCHOSTUIFLAG_THEME);
            return SRESULTS.S_OK;
        }

        public int ShowUI(int dwID, UnsafeNativeMethods.IOleInPlaceActiveObject activeObject, IOleCommandTarget commandTarget, UnsafeNativeMethods.IOleInPlaceFrame frame, UnsafeNativeMethods.IOleInPlaceUIWindow doc)
        {
            return SRESULTS.S_FALSE;
        }

        public int HideUI()
        {
            return SRESULTS.S_OK;
        }

        public int UpdateUI()
        {
            return SRESULTS.S_OK;
        }

        public int EnableModeless(bool fEnable)
        {
            return SRESULTS.S_OK;
        }

        public int OnDocWindowActivate(bool fActivate)
        {
            return SRESULTS.S_OK;
        }

        public int OnFrameWindowActivate(bool fActivate)
        {
            return SRESULTS.S_OK;
        }

        public int ResizeBorder(COMRECT rect, UnsafeNativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow)
        {
            return SRESULTS.S_OK;
        }

        public int TranslateAccelerator(ref MSG msg, ref System.Guid group, int nCmdID)
        {
            return SRESULTS.S_FALSE;
        }

        public int GetOptionKeyPath(string[] pbstrKey, int dw)
        {
            return SRESULTS.S_FALSE;
        }

        public int GetDropTarget(UnsafeNativeMethods.IOleDropTarget pDropTarget, out UnsafeNativeMethods.IOleDropTarget ppDropTarget)
        {
            ppDropTarget = pDropTarget;
            return SRESULTS.S_FALSE;
        }

        public int GetExternal(out object ppDispatch)
        {
            ppDispatch = this;
            return SRESULTS.S_FALSE;
        }

        public int TranslateUrl(int dwTranslate, string strURLIn, out string pstrURLOut)
        {
            pstrURLOut = strURLIn;
            return SRESULTS.S_FALSE;
        }

        public int FilterDataObject(System.Windows.IDataObject pDO, out System.Windows.IDataObject ppDORet)
        {
            ppDORet = pDO;
            return SRESULTS.S_FALSE;
        }
    }
}
