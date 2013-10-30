using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Album.Models
{
    public class Photo
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public long Size { get; set; }
        public bool Active { get; set; }

        public string DisplaySize
        {
            get
            {
                double x;
                int v;

                if (Size < 1024)
                    return string.Format("{0} bytes", Size);

                else if (Size < 1048576)
                {
                    x = Size / 1024.0;
                    v = (int)x;
                    return string.Format("{0} KB", v);
                }

                else if (Size < 1073741824)
                {
                    x = Size / 1048576.0;
                    v = (int)x;
                    return string.Format("{0} MB", v);
                }

                else
                {
                    x = Size / 1073741824.0;
                    v = (int)x;
                    return string.Format("{0} GB", v);
                }
            }
        }
    }

    public class PhotoContainer
    {
        public List<int> Indicators { get; set; }
        public List<Photo> Photos { get; set; }
    }
}