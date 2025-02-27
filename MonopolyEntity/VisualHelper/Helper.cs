﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace MonopolyEntity.VisualHelper
{
    public static class Helper
    {
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent is T parentAsT)
                {
                    return parentAsT;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        public static Point GetElementLocation(UIElement element, WrapPanel wrapPanel)
        {
            if (!wrapPanel.Children.Contains(element))
                throw new ArgumentException("cant find the elems location in WrapPanel.");

            GeneralTransform transform = element.TransformToAncestor(wrapPanel);
            Point location = transform.Transform(new Point(0, 0));

            return location; 
        }

        public static Point GetElementLocationRelativeToPage(UIElement element, Page page)
        {
            if (element == null || page == null)
                throw new ArgumentNullException("Smth wrong with given page");

            GeneralTransform transform = element.TransformToAncestor(page);
            Point location = transform.Transform(new Point(0, 0));

            return location; 
        }

        public static List<SolidColorBrush> GetColorsQueue()
        {
            List<SolidColorBrush> res = new List<SolidColorBrush>();
            res.Add((SolidColorBrush)Application.Current.Resources["FirstUserColor"]);
            res.Add((SolidColorBrush)Application.Current.Resources["SecondUserColor"]);
            res.Add((SolidColorBrush)Application.Current.Resources["ThirdUserColor"]);
            res.Add((SolidColorBrush)Application.Current.Resources["FourthUserColor"]);
            res.Add((SolidColorBrush)Application.Current.Resources["FifthUserColor"]);

            return res;
        }
    }
}
