using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationTest
{
    public class TestModel : ValidationModelBase
    {
        public int Prop1
        {
            get { return Get(() => Prop1); }
            set
            {
                // Validate property
                if (value < 10 || value > 100)
                    AddError("Prop1", "Prop1 must be between 10 and 100");
                else
                    RemoveError("Prop1");

                // Update property
                Set(() => Prop1, value);
            }
        }

        public TestModel()
        {
            Prop1 = 25;
        }
    }
}
