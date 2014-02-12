using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.Windows;

namespace GongDragDropTest
{
    public class AdvancedDropHandler : DefaultDropHandler
    {
        private enum DropType { Default, PatternToCategory, CategoryToGroup };
        private DropType current_drop;

        public override void DragOver(IDropInfo dropInfo)
        {
            if (CanAcceptData(dropInfo))
            {
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                current_drop = DropType.Default;
            }
            else
            {
                var category_target = dropInfo.TargetItem as Category;
                if (category_target != null)
                {
                    var source_type = TypeUtilities.GetCommonBaseClass(ExtractData(dropInfo.Data));
                    if (source_type == typeof(Pattern))
                    {
                        dropInfo.Effects = DragDropEffects.Copy;
                        dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                        current_drop = DropType.PatternToCategory;
                        return;
                    }
                }

                var group_target = dropInfo.TargetItem as Group;
                if (group_target != null)
                {
                    var source_type = TypeUtilities.GetCommonBaseClass(ExtractData(dropInfo.Data));
                    if (source_type == typeof(Category))
                    {
                        dropInfo.Effects = DragDropEffects.Copy;
                        dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                        current_drop = DropType.CategoryToGroup;
                        return;
                    }
                }
            }
        }

        public override void Drop(IDropInfo dropInfo)
        {
            switch (current_drop)
            {
                case DropType.Default:
                    base.Drop(dropInfo);
                    break;
                case DropType.PatternToCategory:
                    HandlePatternToCategoryDrop(dropInfo);
                    break;
                case DropType.CategoryToGroup:
                    HandleCategoryToGroupDrop(dropInfo);
                    break;
            }
        }

        private void HandlePatternToCategoryDrop(IDropInfo dropInfo)
        {
            var destinationList = GetList(dropInfo.TargetCollection);
            var sourceList = GetList(dropInfo.DragInfo.SourceCollection);

            var data = ExtractData(dropInfo.Data);
            foreach (var o in data)
            {
                var index = sourceList.IndexOf(o);
                if (index != -1)
                    sourceList.RemoveAt(index);
            }

            var target = dropInfo.TargetItem as Category;
            foreach (var o in data)
            {
                var source = o as Pattern;
                target.Add(source);
            }
        }

        private void HandleCategoryToGroupDrop(IDropInfo dropInfo)
        {
            var destinationList = GetList(dropInfo.TargetCollection);
            var sourceList = GetList(dropInfo.DragInfo.SourceCollection);

            var data = ExtractData(dropInfo.Data);
            foreach (var o in data)
            {
                var index = sourceList.IndexOf(o);
                if (index != -1)
                    sourceList.RemoveAt(index);
            }

            var target = dropInfo.TargetItem as Group;
            foreach (var o in data)
            {
                var source = o as Category;
                target.Add(source);
            }
        }
    }
}
