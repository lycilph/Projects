using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;

namespace DragAndDrop
{
    class DragAdorner : Adorner
    {
        public DragAdorner(UIElement adornedElement, UIElement adornment)
            : base(adornedElement)
        {
            m_AdornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            m_AdornerLayer.Add(this);
            m_Adornment = adornment;
            m_Adornment.Opacity = 0.8;
            IsHitTestVisible = false;
        }

        public Point MousePosition 
        {
            get { return m_MousePosition; }
            set
            {
                if (m_MousePosition != value)
                {
                    m_MousePosition = value;
                    m_AdornerLayer.Update(AdornedElement);
                }
            }
        }

        public Point Offset
        {
            get { return m_Offset; }
            set
            {
                if (m_Offset != value)
                {
                    m_Offset = value;
                    m_AdornerLayer.Update(AdornedElement);
                }
            }
        }

        public new double Opacity
        {
            get { return m_Adornment.Opacity; }
            set
            {
                if (m_Adornment.Opacity != value)
                {
                    m_Adornment.Opacity = value;
                }
            }
        }

        public void Detatch()
        {
            m_AdornerLayer.Remove(this);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            m_Adornment.Arrange(new Rect(finalSize));
            return finalSize;
        }
        
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(MousePosition.X + m_Offset.X, MousePosition.Y + m_Offset.Y));

            return result;
        }

        protected override Visual GetVisualChild(int index)
        {
            return m_Adornment;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            m_Adornment.Measure(constraint);
            return m_Adornment.DesiredSize;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        AdornerLayer m_AdornerLayer;
        UIElement m_Adornment;
        Point m_MousePosition;
        Point m_Offset = new Point(-4, -4);
    }
}
