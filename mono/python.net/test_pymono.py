from pymono import PyMono
from threading import Thread
import time
import sys

mono  = PyMono('invoke.dll')

class Worker(Thread):
    def __init__(self, sign):
        self._sign = sign
        Thread.__init__(self)

    def run(self):
        time.sleep(1)
        for i in range(2000):
            slideshow = mono.create_slideshow()
            slideshow.init_template('template1.ppt')
            slideshow.set_slidedeck_info("This is AWESOME", 'By: Victor Ng\nMarch 19, 2011')
            slideshow.add_slide('title 1', 'sample.png')
            slideshow.add_slide('title 2', 'sample.png')
            slideshow.add_slide('title 3', 'sample.png')
            fname = '/tmp/victor-%d-%d.ppt' % (abs(self.ident), i)
            slideshow.flush(fname)
            sys.stdout.write(self._sign)
            sys.stdout.flush()


for i in ['.', 'o', 'O', '0', 'X']:
    x = Worker(i)
    x.start()
