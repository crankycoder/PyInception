using System;

using java.io;
using org.apache.poi.hslf.model;
using org.apache.poi.hslf.usermodel;

namespace Embed {
    class MyType {

        Demo d;
        MyType () {
        }

        void init_template(string template)
        {
            d = new Demo(template);
        }


        void set_slidedeck_info(string title, string description)
        {
            d.set_slidedeck_info(title, description);

        }

        void add_slide(String title, String png_file)
        {
            d.add_slide(title, png_file);
        }

        void flush(string output_file)
        {
            d.flush(output_file);
        }
    }
}


