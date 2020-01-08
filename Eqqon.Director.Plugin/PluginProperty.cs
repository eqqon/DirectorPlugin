using System;

namespace Eqqon.Director.Plugin
{
    public class PluginProperty
    {

        public string Name { get; set; }

        public event Action<PluginProperty, object, object> ValueChanged;

        private object _value;

        private bool _is_in_set_value = false;
        public object Value
        {
            get => _value;
            set
            {
                if (object.Equals(_value, value))
                    return;
                if (_is_in_set_value)
                    return;
                try
                {
                    _is_in_set_value = true;
                    var old_value = _value;
                    _value = value;
                    ValueChanged?.Invoke(this, old_value, value);
                }
                finally
                {
                    _is_in_set_value = false;
                }
            }
        }

        public Type Type { get; set; } = typeof(string);
    }
}
