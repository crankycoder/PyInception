using System;

using java.io;
using org.apache.poi.hslf.model;
using org.apache.poi.hslf.usermodel;

namespace Embed {
    class MyType {
        int val = 5;
        string str = "hello";

        MyType () {
            System.Console.WriteLine ("In ctor val is: {0}", val);
            System.Console.WriteLine ("In ctor str is: {0}", str);
        }

        MyType (int v, byte[] array) {
            System.Console.WriteLine ("In ctor (int, byte[]) got value: {0}, array len: {1}", v, array.Length);
        }

        void method () {
            Demo d = new Demo();
            d.do_stuff();
        }

        int Value {
            get {
                return val;
            }
        }

        string Message {
            get {
                return str;
            }
        }

        void Values (ref int v, ref string s) {
            System.Console.WriteLine ("In Values () v is {0}", v);
            System.Console.WriteLine ("In Values () s is: {0}", s);
            v = val;
            s = str;
        }

        static void Fail () {
            throw new Exception ();
        }

        static void Main() {
            MyType m = new MyType();
            m.method(); 
        }

    }
}


