using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WpfBlendApp
{
    public sealed class EnumerateExtension : MarkupExtension
    {
        public Type type { get; set; }

        public EnumerateExtension(Type type) => this.type = type;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (type.IsEnum)
            {
                return type.GetEnumValues();
            }
            throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
