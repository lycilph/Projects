using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditableTextBlockV2
{
    public interface IViewModel<TModel>
    {
        TModel Model { get; }
    }
}
