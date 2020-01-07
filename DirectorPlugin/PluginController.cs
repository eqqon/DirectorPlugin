﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirectorPlugin
{

    public class PluginController : IDisposable
    {
        /// <summary>
        /// Overwrite this to initially set up the plugin (will be called before Initialize1) 
        /// </summary>
        public virtual void Initialize0() { }

        /// <summary>
        /// Overwrite this to set up anything that must be set up *after* Director initialized the plugin (will be called after Initialize0)
        /// For instance, binding to own properties should be done here.
        /// </summary>
        public virtual void Initialize1()
        {
        }

        /// <summary>
        /// A list of all plugin views that were generated by GetViewHook()
        /// Note: A director plugin can have more than one view, depending on how many Director-pages it is embedded.
        /// </summary>
        public List<FrameworkElement> Views { get; }=new List<FrameworkElement>();
        public Dictionary<string,ObservableValue> Properties { get; } = new Dictionary<string, ObservableValue>();

        // usually not overridden
        public virtual FrameworkElement GetView()
        {
            var view = GetViewHook();
            Views.Add(view);
            return view;
        }

        /// <summary>
        /// Overwrite this to create a view for the plugin that is embedded in Director.
        /// Note: A director plugin can have more than one view, depending on how many Director-pages it is embedded.
        /// </summary>
        protected virtual FrameworkElement GetViewHook()
        {
            return new DemoView();
        }

        // usually not overridden
        public virtual void UpdateViews()
        {
            foreach(var view in Views)
                UpdateView(view);
        }

        /// <summary>
        /// Overwrite this to set up the view with data upon first display and whenever view data changes
        /// </summary>
        /// <param name="view"></param>
        public virtual void UpdateView(FrameworkElement view)
        {
            var v = view as DemoView;
            v.StringPropertyTextBox.TextChanged += (x, y) => Properties["StringProperty"].Value = v.StringPropertyTextBox.Text;
            v.StringPropertyTextBox.Text = (string) Properties["StringProperty"].Value;
            v.BoolPropertyCheckBox.Click += (x, y) => Properties["BoolProperty"].Value = (v.BoolPropertyCheckBox.IsChecked==true);
            v.BoolPropertyCheckBox.IsChecked = (bool)Properties["BoolProperty"].Value;
            v.DoublePropertySlider.ValueChanged += (x, y) => Properties["DoubleProperty"].Value = v.DoublePropertySlider.Value;
            v.DoublePropertySlider.Value = (double)Properties["DoubleProperty"].Value;
        }


        // usually not overridden
        public virtual IEnumerable<ObservableValue> GetProperties()
        {
            foreach (var prop in GetPropertiesHook())
            {
                Properties[prop.Name] = prop;
                prop.ValueChanged += OnPropertyValueChanged;
                yield return prop;
            }
        }

        /// <summary>
        /// Override this to react to changes in observable value properties
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="old_value"></param>
        /// <param name="new_value"></param>
        protected virtual void OnPropertyValueChanged(ObservableValue prop, object old_value, object new_value)
        {
            UpdateViews();
        }

        /// <summary>
        /// Overwrite this to define the properties of the plugin
        /// </summary>
        protected virtual IEnumerable<ObservableValue> GetPropertiesHook()
        {
            yield return new ObservableValue() { Name = "StringProperty", Type = typeof(string), Value = "Initial value"};
            yield return new ObservableValue() { Name = "BoolProperty", Type = typeof(bool), Value = true };
            yield return new ObservableValue() { Name = "DoubleProperty", Type = typeof(double), Value = 50.0};
        }

        public virtual IEnumerable<string> GetEvents()
        {
            yield return "Event1";
            yield return "Event2";
        }

        /// <summary>
        /// overwrite this to react to configuration property value changes
        /// </summary>
        /// <param name="property_name">Name of the property defined in GetProperties()</param>
        /// <param name="value">The changed value (can be expected to be of type defined in GetProperties())</param>
        public virtual void PropertyValueChanged(string property_name, object value)
        {
            Log(nameof(PluginController), DirectorLogLevel.DEBUG, $"Property '{property_name}' changed to value '{value}'");
        }

        /// <summary>
        /// Director will subscribe this event to watch for logging requests of the plugin.
        /// Use Log(...) to fire the event.
        /// </summary>
        public event Action<string, int, string> LogRequested;

        /// <summary>
        /// Use this to insert log messages into Director's logging system
        /// </summary>
        /// <param name="logger">Director logger name</param>
        /// <param name="level">Director logging level</param>
        /// <param name="message">The message text to be logged</param>
        public virtual void Log(string logger, DirectorLogLevel level, string message)
        {
            LogRequested?.Invoke(logger, (int)level, message);
        }

        /// <summary>
        /// Overwrite this to finalize the plugin
        /// </summary>
        protected virtual void DisposeHook()
        {
        }

        public void Dispose()
        {
            DisposeHook();
        }
    }
}
