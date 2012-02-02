import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import org.apache.poi.hslf.model.Picture;
import org.apache.poi.hslf.model.Slide;
import org.apache.poi.hslf.model.TextRun;
import org.apache.poi.hslf.model.TextBox;
import org.apache.poi.hslf.usermodel.RichTextRun;
import org.apache.poi.hslf.usermodel.SlideShow;

public class Demo
{
    FileInputStream _fis;
    SlideShow _ss;
    boolean _first_slide = true;

    public Demo(String template_filename)
    {
        try
        {
            _fis = new FileInputStream(new File(template_filename));
            _ss = new SlideShow(_fis);
        } catch (Exception e) {
        }
    }

    public void set_slidedeck_info(String title, String description)
    {
        int idx = 0;
        Slide[] slides = _ss.getSlides();
        Slide s = slides[0];
        TextRun[] textruns = s.getTextRuns();

        for (TextRun t: textruns)
        {
            for (RichTextRun rt: t.getRichTextRuns())
            {
                if (idx == 0) {
                    rt.setText(title);
                } else if (idx == 1) {
                    rt.setText(description);
                }
                idx += 1;
            }
        }
    }

    public void add_slide(String title, String png_image) 
    {
        Slide slide;
        Slide[] slides;
        TextBox title_box;
        try
        {
            // Now add a slide and inject an image 
            int i = _ss.addPicture(new File(png_image), Picture.PNG);
            Picture p = new Picture(i); 

            //set image position in the slide
            // p.setAnchor(new java.awt.Rectangle(0, 0, 720, 540));
            p.setAnchor(new java.awt.Rectangle(50, 100, 620, 390));

            if (_first_slide) {
                slides = _ss.getSlides();
                slide = slides[slides.length-1];
                _first_slide = false;
            }
            else {
                slide = _ss.createSlide();
            }
            slide.setSlideNumber(_ss.getSlides().length); 
            slide.addShape(p);

            // Create a title and set the title text
            title_box = slide.addTitle();
            title_box.setText(title);

        } catch (Exception e) {
            // Do stuff
        }
    }


    public void flush(String output_filename)
    {
        try{
            File fout = new File(output_filename);
            FileOutputStream fos = new FileOutputStream(fout);
            _ss.write(fos);
            fos.flush();
            fos.close();
        } catch (Exception e) {
            // 
            // 
            //
        }
    }

    public static void main(String[] args)
    {
        Demo d = new Demo("template1.ppt");
        d.set_slidedeck_info("Some Random Title", "By: Victor Ng\nMarch 18, 2011");

        d.add_slide("This is a title", "sample.png");

        d.flush("java-output.ppt");
    }

}
