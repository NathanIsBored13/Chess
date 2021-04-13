using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Extentions
{
    static class Extentions
    {
        public static Tuple<Enum, string>[] GetEnumDescriptions(Type enumType) => Enum.GetValues(enumType).OfType<Enum>().Select(e => new Tuple<Enum, string>(e, e.GetDescriptor())).ToArray();

        public static string GetDescriptor(this Enum value) => (Enum.GetName(value.GetType(), value) is string name && value.GetType().GetField(name) is FieldInfo field && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr) ? attr.Description : value.ToString();

        public static IEnumerable<T> SubArray<T>(this IEnumerable<T> data, int skip, int leave)
        {
            for (int i = skip; i < data.Count() - leave; i++)
                yield return data.ElementAt(i);
        }

        public static IEnumerable<int> Enumerate(int start, int stop) => Enumerable.Range(Math.Min(start, stop), Math.Abs(start - stop));

        public static IEnumerable<Enum> GetPromotions() => Enum.GetValues(typeof(Chess.Type)).OfType<Enum>().Where(x => x.ToString() != "Pawn" && x.ToString() != "King");

        public static void SetWidthFromItems(this ComboBox comboBox)
        {
            double comboBoxWidth = 19;

            ComboBoxAutomationPeer peer = new ComboBoxAutomationPeer(comboBox);
            IExpandCollapseProvider provider = (IExpandCollapseProvider)peer.GetPattern(PatternInterface.ExpandCollapse);
            EventHandler eventHandler = null;
            eventHandler = new EventHandler(delegate
            {
                if (comboBox.IsDropDownOpen && comboBox.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    double width = 0;
                    foreach (var item in comboBox.Items)
                    {
                        ComboBoxItem comboBoxItem = comboBox.ItemContainerGenerator.ContainerFromItem(item) as ComboBoxItem;
                        comboBoxItem.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        if (comboBoxItem.DesiredSize.Width > width)
                            width = comboBoxItem.DesiredSize.Width;
                    }
                    comboBox.Width = comboBoxWidth + width;
                    comboBox.ItemContainerGenerator.StatusChanged -= eventHandler;
                    comboBox.DropDownOpened -= eventHandler;
                    provider.Collapse();
                }
            });
            comboBox.ItemContainerGenerator.StatusChanged += eventHandler;
            comboBox.DropDownOpened += eventHandler;
            provider.Expand();
        }
    }
}