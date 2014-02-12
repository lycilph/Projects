using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.Windows;

namespace EditableTextBlockV2
{
    public class AdvancedDropHandler : DefaultDropHandler
    {
        private enum DropType { Default, PatternToCategory };
        private DropType current_drop;

        public override void DragOver(IDropInfo drop_info)
        {
            if (CanAcceptData(drop_info))
            {
                drop_info.Effects = DragDropEffects.Copy;
                drop_info.DropTargetAdorner = DropTargetAdorners.Insert;
                current_drop = DropType.Default;
            }
            else
            {
                var category_target = drop_info.TargetItem as CategoryViewModel;
                if (category_target != null)
                {
                    var source_type = TypeUtilities.GetCommonBaseClass(ExtractData(drop_info.Data));
                    if (source_type == typeof(PatternViewModel))
                    {
                        drop_info.Effects = DragDropEffects.Copy;
                        drop_info.DropTargetAdorner = DropTargetAdorners.Highlight;
                        current_drop = DropType.PatternToCategory;
                        return;
                    }
                }
            }
        }

        public override void Drop(IDropInfo drop_info)
        {
            switch (current_drop)
            {
                case DropType.Default:
                    base.Drop(drop_info);
                    break;
                case DropType.PatternToCategory:
                    HandlePatternToCategoryDrop(drop_info);
                    break;
            }
        }

        private void HandlePatternToCategoryDrop(IDropInfo drop_info)
        {
            var destination_list = GetList(drop_info.TargetCollection);
            var source_list = GetList(drop_info.DragInfo.SourceCollection);

            var data = ExtractData(drop_info.Data);
            foreach (var o in data)
            {
                var index = source_list.IndexOf(o);
                if (index != -1)
                    source_list.RemoveAt(index);
            }

            var target = drop_info.TargetItem as CategoryViewModel;
            foreach (var o in data)
            {
                var source = o as PatternViewModel;
                target.PatternViewModels.Add(source);
            }
        }
    }
}
