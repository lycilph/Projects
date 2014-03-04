using Caliburn.Micro.ReactiveUI;
using CefSharp;
using CefSharp.Wpf;
using ReactiveUI;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;

namespace MetroCefSharpTest.Shell.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : ReactiveScreen, IShell, IRequestHandler, IWebBrowser
    {
        private static string home_url = "http://www.google.com";

        private string _Address = home_url;
        public string Address
        {
            get { return _Address; }
            set { this.RaiseAndSetIfChanged(ref _Address, value); }
        }

        private IWpfWebBrowser _WebBrowser = null;
        public IWpfWebBrowser WebBrowser
        {
            get { return _WebBrowser; }
            set { this.RaiseAndSetIfChanged(ref _WebBrowser, value); }
        }

        public ShellViewModel()
        {
            DisplayName = "Shell";
        }

        public void Test()
        {
            WebBrowser.RequestHandler = this;
            WebBrowser.Address = home_url;

        }

        void WebBrowser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            System.Diagnostics.Debug.Print(e.Message);
        }

        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        public bool GetDownloadHandler(IWebBrowser browser, out IDownloadHandler handler)
        {
            throw new System.NotImplementedException();
        }

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType navigationType, bool isRedirect)
        {
            return false;
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            using (var client = new WebClient())
            {
                var data = client.DownloadData(requestResponse.Request.Url);
                var sr = new MemoryStream(data);
                requestResponse.RespondWith(sr, "text/html");
                return true;
            }

            return false;
        }

        public void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, System.Net.WebHeaderCollection headers)
        {
            throw new System.NotImplementedException();
        }


        public bool CanGoBack
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool CanGoForward
        {
            get { throw new System.NotImplementedException(); }
        }

        public event ConsoleMessageEventHandler ConsoleMessage;

        public object EvaluateScript(string script)
        {
            throw new System.NotImplementedException();
        }

        public object EvaluateScript(string script, System.TimeSpan? timeout)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteScriptAsync(string script)
        {
            throw new System.NotImplementedException();
        }

        public bool IsBrowserInitialized
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool IsLoading
        {
            get { throw new System.NotImplementedException(); }
        }

        public void Load(string url)
        {
            throw new System.NotImplementedException();
        }

        public event LoadCompletedEventHandler LoadCompleted;

        public event LoadErrorEventHandler LoadError;

        public void LoadHtml(string html, string url)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterJsObject(string name, object objectToBind)
        {
            throw new System.NotImplementedException();
        }

        public IRequestHandler RequestHandler
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public string Title
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public string TooltipText
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
