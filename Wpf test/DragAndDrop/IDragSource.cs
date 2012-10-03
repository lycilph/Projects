using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragAndDrop
{
    public interface IDragSource
    {
        void StartDrag(DragInfo dragInfo);
    }
}
