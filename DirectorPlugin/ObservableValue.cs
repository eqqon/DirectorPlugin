using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPlugin
{
    public class ObservableValue
    {

        public string Name { get; set; }

        public event Action<ObservableValue, object, object> ValueChanged;

        private object _value;

        public object Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;
                var old_value = _value;
                _value = value;
                ValueChanged?.Invoke(this, old_value, value);
            }
        }

        public Type Type { get; set; } = typeof(string);
    }
}
