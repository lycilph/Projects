using System.ComponentModel;

namespace Wpf_test
{
    [TypeConverter(typeof(EnumTypeConverter))]
    public enum FilterType
    {
        [EnumDisplayName("Filter on: Starts with A")]
        FilterOnA,
        [EnumDisplayName("Filter on: Starts with V")]
        FilterOnB,
        [EnumDisplayName("Filter on: Starts with B")]
        FilterOnC
    }
}
