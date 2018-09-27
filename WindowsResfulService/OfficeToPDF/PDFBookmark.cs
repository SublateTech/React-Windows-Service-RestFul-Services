using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace OfficeToPDF
{
    public class PDFBookmark
    {
        public int page { get; set; }
        public string title { get; set; }
        public List<PDFBookmark> children { get; set; }   
    }
}
