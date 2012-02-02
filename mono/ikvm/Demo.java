import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import org.apache.poi.hslf.model.Picture;
import org.apache.poi.hslf.model.Slide;
import org.apache.poi.hslf.model.TextRun;
import org.apache.poi.hslf.usermodel.RichTextRun;
import org.apache.poi.hslf.usermodel.SlideShow;


public class Demo
{
    public String do_stuff() throws Throwable
    {
        FileInputStream fis = new FileInputStream(new File("demo.ppt"));
        SlideShow ss = new SlideShow(fis);
        Slide[] slides = ss.getSlides();
        for (Slide s: slides)
        {
            System.out.println(s.getTitle());
            TextRun[] textruns = s.getTextRuns();
            System.out.println("Text run length: " + textruns.length);
            for (TextRun t: textruns)
            {
                for (RichTextRun rt: t.getRichTextRuns())
                {
                    System.out.println("RT: [" + rt.getText() + "]");
                    
                    // Now change the text
                    rt.setText("FOO["+rt.getText()+"]BAR");
                }
            }
        }
        //
        // Now add a slide and inject an image 
        int i = ss.addPicture(new File("sample.jpg"), Picture.JPEG);
        Picture p = new Picture(i); 

        //set image position in the slide
        p.setAnchor(new java.awt.Rectangle(0, 0, 720, 480));

        Slide slide = ss.createSlide();
        slide.setSlideNumber(ss.getSlides().length); 
        slide.addShape(p);

        File fout = new File("demo-output.ppt");
        FileOutputStream fos = new FileOutputStream(fout);
        ss.write(fos);
        fos.flush();
        fos.close();
        return "Flushed out a new PPT slide";
    }

    public static void main(String[] argv) throws Throwable
    {
        Demo x = new Demo();
        x.do_stuff();
		System.exit(0);
    }
}
