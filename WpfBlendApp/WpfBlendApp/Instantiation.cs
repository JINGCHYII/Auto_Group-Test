using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace WpfBlendApp
{
    class Instantiation
    {
        public int Iteration;

        public delegate object ConstructorDelegate();

        public Instantiation() { }

        protected ConstructorDelegate GetConstructor(string typeName)
        {
            Type t = Type.GetType(typeName);
            ConstructorInfo ctor = t.GetConstructor(new Type[0]);

            string methodName = t.Name + "Ctor";
            DynamicMethod dm = new DynamicMethod(methodName, t, new Type[0], typeof(Activator));
            ILGenerator lgen = dm.GetILGenerator();
            lgen.Emit(OpCodes.Newobj, ctor);
            lgen.Emit(OpCodes.Ret);

            ConstructorDelegate creator = (ConstructorDelegate)dm.CreateDelegate(typeof(ConstructorDelegate));

            return creator;
        }

        public bool MeasureTestA()
        {
            var type = Type.GetType("System.Text.StringBuilder");
            for (int i = 0; i < Iteration; i++)
            {
                var obj = Activator.CreateInstance(type);
                if (obj.GetType() != typeof(System.Text.StringBuilder))
                    throw new InvalidOperationException("123");
            }
            return true;
        }

        public bool MeasureTestB()
        {
            var constructor = GetConstructor("System.Text.StringBuilder");
            for (int i = 0; i < Iteration; i++)
            {
                var obj = constructor();
                if (obj.GetType() != typeof(System.Text.StringBuilder))
                    throw new InvalidOperationException("123");
            }
            return true;
        }

        public bool MeasureTestC()
        {
            for (int i = 0; i < Iteration; i++)
            {
                var obj = new System.Text.StringBuilder();
                if (obj.GetType() != typeof(System.Text.StringBuilder))
                    throw new InvalidOperationException("123");
            }
            return true;
        }
    }
}
