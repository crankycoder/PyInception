using System;

namespace Demo
{
    class MyClass {
        public void throw_exc()
        {
            Console.WriteLine("in throw_exc");
            try
            {
                throw new Exception("This is a test");
            } catch (Exception e)
            {
                Console.WriteLine("====Msg==============");
                Console.WriteLine(e.Message);
                Console.WriteLine("====Data==============");
                foreach (string k in e.Data.Keys) {
                    string v = (string) e.Data[k];
                    Console.WriteLine("["+k+"] = ["+v+"]");
                }
                Console.WriteLine("====Src==============");
                Console.WriteLine(e.Source);
                Console.WriteLine("====STrace==============");
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("====String==============");
                Console.WriteLine(e.ToString());
                Console.WriteLine("==================");
            }
             
        }

        static void Main () {
            MyClass obj = new MyClass();
            obj.throw_exc();
        }
    }
}
