using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace WpfBlendApp
{
    class _properties
    {
        public int Iteration;

        public delegate object PropertyGetDelegate(object obj);
        public delegate void PropertySetDelegate(object obj, object value);

        public _properties() { }

        protected PropertyGetDelegate GetPropertyGetter(string typeName, string propertyName)
        {
            Type t = Type.GetType(typeName);
            PropertyInfo pi = t.GetProperty(propertyName);
            MethodInfo getter = pi.GetGetMethod();

            DynamicMethod dm = new DynamicMethod("GetValue", typeof(object), new Type[] { typeof(object) }, typeof(object), true);
            ILGenerator lgen = dm.GetILGenerator();

            lgen.Emit(OpCodes.Ldarg_0);
            lgen.Emit(OpCodes.Call, getter);

            if (getter.ReturnType.GetTypeInfo().IsValueType)
            {
                lgen.Emit(OpCodes.Box, getter.ReturnType);
            }

            lgen.Emit(OpCodes.Ret);

            return dm.CreateDelegate(typeof(PropertyGetDelegate)) as PropertyGetDelegate;
        }

        protected PropertySetDelegate GetPropertySetter(string typeName, string propertyName)
        {
            Type t = Type.GetType(typeName);
            PropertyInfo pi = t.GetProperty(propertyName);
            MethodInfo setter = pi.GetSetMethod(false);

            DynamicMethod dm = new DynamicMethod("SetValue", typeof(object), new Type[] { typeof(object) }, typeof(object), true);
            ILGenerator lgen = dm.GetILGenerator();

            lgen.Emit(OpCodes.Ldarg_0);
            lgen.Emit(OpCodes.Ldarg_1);

            Type parameterType = setter.GetParameters()[0].ParameterType;

            if (parameterType.GetTypeInfo().IsValueType)
            {
                lgen.Emit(OpCodes.Unbox_Any, parameterType);
            }

            lgen.Emit(OpCodes.Call, setter);
            lgen.Emit(OpCodes.Ret);

            return dm.CreateDelegate(typeof(PropertySetDelegate)) as PropertySetDelegate;
        }

        public bool MeasureTestA()
        {
            var sb = new StringBuilder("Mark Duncan Farragher");
            PropertyInfo pi = sb.GetType().GetProperty("Length");
            for (int i = 0; i < Iteration; i++)
            {
                var length = pi.GetValue(sb);
                if (!21.Equals(length))
                    throw new InvalidOperationException("");
            }
            return true;
        }

        public bool MeasureTestB()
        {
            var sb = new StringBuilder("Mark Duncan Farragher");
            var getter = GetPropertyGetter("System.Text.StringBuilder", "Length");
            for (int i = 0; i < Iteration; i++)
            {
                var length = getter(sb);
                if (!21.Equals(length))
                    throw new InvalidOperationException("");
            }
            return true;
        }

        public bool MeasureTestC()
        {
            var sb = new StringBuilder("Mark Duncan Farragher");
            for (int i = 0; i < Iteration; i++)
            {
                var length = sb.Length;
                if (!21.Equals(length))
                    throw new InvalidOperationException("");
            }
            return true;
        }
    }
}
