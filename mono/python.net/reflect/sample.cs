using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// Code for building SimpleType.dll.
using System;
using System.Reflection;
using System.Globalization;
using Simple_Type;

namespace Simple_Type
{
    public class MySimpleClass
    {
        public void MyMethod(string str, int i)
        {
            Console.WriteLine("MyMethod parameters: {0}, {1}", str, i);
        }

        public void MyMethod(string str, int i, int j)
        {
            Console.WriteLine("MyMethod parameters: {0}, {1}, {2}",
                str, i, j);
        }
    }
}

namespace Custom_Binder
{
    class MyMainClass
    {
        static void Main2()
        {
            // Get the type of MySimpleClass.
            Type myType = typeof(MySimpleClass);

            // Get an instance of MySimpleClass.
            MySimpleClass myInstance = new MySimpleClass();
            CustomBinder myCustomBinder = new CustomBinder();

            // Get the method information for the particular overload 
            // being sought.
            MethodInfo myMethod = myType.GetMethod("MyMethod",
                BindingFlags.Public | BindingFlags.Instance,
                myCustomBinder, new Type[] {typeof(string), 
                typeof(int)}, null);
            Console.WriteLine(myMethod.ToString());

            // Invoke the overload.
            myType.InvokeMember("MyMethod", BindingFlags.InvokeMethod,
                myCustomBinder, myInstance,
                new Object[] { "Testing...", (int)32 });
        }
    }

    // ****************************************************
    //  A simple custom binder that provides no
    //  argument type conversion.
    // ****************************************************
    class CustomBinder : Binder
    {
        public override MethodBase BindToMethod(
            BindingFlags bindingAttr,
            MethodBase[] match,
            ref object[] args,
            ParameterModifier[] modifiers,
            CultureInfo culture,
            string[] names,
            out object state)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
            // Arguments are not being reordered.
            state = null;
            // Find a parameter match and return the first method with
            // parameters that match the request.
            foreach (MethodBase mb in match)
            {
                ParameterInfo[] parameters = mb.GetParameters();

                if (ParametersMatch(parameters, args))
                {
                    return mb;
                }
            }
            return null;
        }

        public override FieldInfo BindToField(BindingFlags bindingAttr,
            FieldInfo[] match, object value, CultureInfo culture)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
            foreach (FieldInfo fi in match)
            {
                if (fi.GetType() == value.GetType())
                {
                    return fi;
                }
            }
            return null;
        }

        public override MethodBase SelectMethod(
            BindingFlags bindingAttr,
            MethodBase[] match,
            Type[] types,
            ParameterModifier[] modifiers)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            // Find a parameter match and return the first method with
            // parameters that match the request.
            foreach (MethodBase mb in match)
            {
                ParameterInfo[] parameters = mb.GetParameters();
                if (ParametersMatch(parameters, types))
                {
                    return mb;
                }
            }

            return null;
        }

        public override PropertyInfo SelectProperty(
            BindingFlags bindingAttr,
            PropertyInfo[] match,
            Type returnType,
            Type[] indexes,
            ParameterModifier[] modifiers)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
            foreach (PropertyInfo pi in match)
            {
                if (pi.GetType() == returnType &&
                    ParametersMatch(pi.GetIndexParameters(), indexes))
                {
                    return pi;
                }
            }
            return null;
        }

        /*
         * This method is invoked to handle argument type conversion
         *
         * We'll need to extend this handle :
         *      * string->decimal->float->double
         */
        public override object ChangeType(
            object value,
            Type myChangeType,
            CultureInfo culture)
        {
            try
            {
                object newType;
                newType = Convert.ChangeType(value, myChangeType);
                return newType;
            }
            // Throw an InvalidCastException if the conversion cannot
            // be done by the Convert.ChangeType method.
            catch (InvalidCastException)
            {
                return null;
            }
        }

        public override void ReorderArgumentArray(ref object[] args,
            object state)
        {
            // No operation is needed here because BindToMethod does not
            // reorder the args array. The most common implementation
            // of this method is shown below.

            // ((BinderState)state).args.CopyTo(args, 0);
        }

        // Returns true only if the type of each object in a matches
        // the type of each corresponding object in b.
        private bool ParametersMatch(ParameterInfo[] a, object[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].ParameterType != b[i].GetType())
                {
                    return false;
                }
            }
            return true;
        }

        // Returns true only if the type of each object in a matches
        // the type of each corresponding entry in b.
        private bool ParametersMatch(ParameterInfo[] a, Type[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].ParameterType != b[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
    public class CustomBinderDriver
    {
        public static void Main()
        {
            Type t = typeof(CustomBinderDriver);
            CustomBinder binder = new CustomBinder();
            BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.Static;
            object[] args;

            // Case 1. Neither argument coercion nor member selection is needed.
            args = new object[] { };
            t.InvokeMember("PrintBob", flags, binder, null, args);

            // Only member selection is needed.
            args = new object[] { (long) 42 };
            t.InvokeMember("PrintValue", flags, binder, null, args);

            // Only member selection is needed.
            args = new object[] { "5.5" };
            t.InvokeMember("PrintValue", flags, binder, null, args);
        }

        public static void PrintBob()
        {
            Console.WriteLine("PrintBob");
        }

        public static void PrintValue(long value)
        {
            Console.WriteLine("PrintValue({0})", value);
        }

        public static void PrintValue(string value)
        {
            Console.WriteLine("PrintValue\"{0}\")", value);
        }

        public static void PrintNumber(double value)
        {
            Console.WriteLine("PrintNumber ({0})", value);
        }
    }

}

