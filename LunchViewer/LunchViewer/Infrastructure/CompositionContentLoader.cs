using System;
using System.ComponentModel.Composition;
using FirstFloor.ModernUI.Windows;
using NLog;

namespace LunchViewer.Infrastructure
{
    class CompositionContentLoader : DefaultContentLoader
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override object LoadContent(Uri uri)
        {
            logger.Debug(string.Format("Loading and composing ({0})", uri));

            var content = base.LoadContent(uri);

            if (content != null)
                CompositionService.Instance.Container.ComposeParts(content);

            return content;
        }
    }
}
