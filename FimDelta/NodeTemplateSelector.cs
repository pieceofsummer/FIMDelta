using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using FimDelta.ViewModel;

namespace FimDelta
{
    class NodeTemplateSelector : DataTemplateSelector
    {

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ObjectNode)
                return (DataTemplate)((FrameworkElement)container).FindResource("ObjectNode");
            if (item is AttributeNode)
                return (DataTemplate)((FrameworkElement)container).FindResource("AttributeNode");
            if (item is ReferencedByNode)
                return (DataTemplate)((FrameworkElement)container).FindResource("RefdByNode");
            if (item is GroupByNode)
                return (DataTemplate)((FrameworkElement)container).FindResource("GroupByNode");
            return base.SelectTemplate(item, container);
        }

    }
}
