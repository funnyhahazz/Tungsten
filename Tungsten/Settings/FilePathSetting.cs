﻿using System.Windows.Controls;
using System.Windows;
using System;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Tungsten.Settings
{
    public class FilePathSetting : Setting
    {
        public string Value { get; private set; }
        public Action<string> OnChangeEvent { get; private set; }
        public string Filter { get; private set; }
        public string DefaultExt { get; private set; }

        public FilePathSetting(string name, string identifier, string defaultValue = "", string filter = "DLL Files|*.dll", string defaultExt = ".dll", Action<string> onChange = null) : base(name, identifier)
        {
            OnChangeEvent = onChange;
            Filter = filter;
            DefaultExt = defaultExt;
            Value = GetValue(defaultValue);
            if (OnChangeEvent != null)
                OnChangeEvent(Value);
        }

        public override Grid GetComponent()
        {
            Grid grid = base.GetComponent();
            TextBox textBox = new TextBox
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5),
                Width = 300,
                Text = Value
            };
            textBox.TextChanged += (s, e) =>
            {
                Value = textBox.Text;
                if (OnChangeEvent != null)
                    OnChangeEvent(Value);

                if (SaveManager.Instance != null)
                    SaveManager.Instance.Save(Identifier, Value);
            };
            Button btn = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0,5,310,5),
                Background = new SolidColorBrush(Color.FromRgb(25, 27, 33)),
                Content = new Path
                {
                    Data = Geometry.Parse("M7 16H6C4.89543 16 4 15.1046 4 14V7C4 5.89543 4.89543 5 6 5H18C19.1046 5 20 5.89543 20 7V14C20 15.1046 19.1046 16 18 16H17M12 20V9M12 9L15 12M12 9L9 12"),
                    Stroke = new SolidColorBrush(Color.FromRgb(197, 199, 211)),
                    StrokeThickness = 1.5,
                    StrokeEndLineCap = PenLineCap.Round,
                    StrokeStartLineCap = PenLineCap.Round,
                    Width = 15,
                    Stretch = Stretch.Uniform,
                    Margin = new Thickness(5,6,5,4),
                }
            };
            btn.Click += (s, e) =>
            {
                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = Filter,
                    DefaultExt = DefaultExt
                };

                bool? result = ofd.ShowDialog();
                if (result == true)
                {
                    Value = ofd.FileName;
                    textBox.Text = Value;
                }
            };
            grid.Children.Add(textBox);
            grid.Children.Add(btn);
            return grid;
        }

    }
}
