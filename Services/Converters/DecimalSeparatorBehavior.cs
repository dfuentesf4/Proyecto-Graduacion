using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.Converters
{
    public class DecimalSeparatorBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                string newText = e.NewTextValue;

                // Permitir entrada temporal con punto decimal y verificar solo si es un número completo
                if (!string.IsNullOrEmpty(newText))
                {
                    // Reemplazar comas por puntos pero permitir temporalmente puntos
                    newText = newText.Replace(",", ".");

                    // Permitir números vacíos o con un solo punto al inicio
                    if (newText == "." || decimal.TryParse(newText, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out _))
                    {
                        // No hacer nada, ya que la entrada es válida en su estado actual
                    }
                    else
                    {
                        // Revertir al valor anterior si la entrada no es válida
                        entry.Text = e.OldTextValue;
                    }
                }
            }
        }
    }
}
