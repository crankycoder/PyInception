public class HelloWorld
{
    public String speak()
    {
        return "This is inside of java!\n";
    }

    public static void main(String[] argv)
    {
        HelloWorld h = new HelloWorld();
        System.out.println(h.speak());
    }
}

