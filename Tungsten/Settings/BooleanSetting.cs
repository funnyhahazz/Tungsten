﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace Tungsten.Settings
{
    public class BooleanSetting : Setting
    {
        public bool Value { get; private set; }
        public Action<bool> OnChangeEvent { get; private set; }

        public BooleanSetting(string name, string identifier, bool value, Action<bool> onChange = null) : base(name, identifier)
        {
            OnChangeEvent = onChange;
            Value = GetValue(value);
            if (OnChangeEvent != null)
                OnChangeEvent(Value);
        }

        public override Grid GetComponent()
        {
            Grid grid = base.GetComponent();
            CheckBox checkBox = new CheckBox
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(2),
                IsChecked = Value
            };
            checkBox.Checked += CheckBox_CheckedChanged;
            checkBox.Unchecked += CheckBox_CheckedChanged;
            grid.Children.Add(checkBox);
            return grid;
        }

        private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Value = checkBox.IsChecked.GetValueOrDefault();
            if (OnChangeEvent != null)
                OnChangeEvent(Value);

            if (SaveManager.Instance != null)
                SaveManager.Instance.Save(Identifier, Value);
        }

    }
}
