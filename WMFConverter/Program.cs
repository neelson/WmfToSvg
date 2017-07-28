using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMFConverter
{
    class Program
    {
        static void Main(string[] args)
        {
		    string src = "";
		    string dest = "";
            //bool debug = false;
		    bool compatible = false;
		    bool replaceSymbolFont = false;


            System.IO.DirectoryInfo dic = new System.IO.DirectoryInfo(src);
            var files = dic.GetFiles();
            foreach(var file in files)
            {
                src = file.FullName;
                dest =  System.IO.Path.GetDirectoryName(file.FullName)+ "\\"+ System.IO.Path.GetFileNameWithoutExtension(file.Name) + ".svg";
                try
                {
                    System.IO.Stream input = new System.IO.FileStream(src, System.IO.FileMode.Open);
                    WMFConverter.Wmf.WmfParser parser = new WMFConverter.Wmf.WmfParser();
                    WMFConverter.Svg.SvgGdi gdi = new WMFConverter.Svg.SvgGdi(compatible);
                    gdi.ReplaceSymbolFont = replaceSymbolFont;

                    parser.Parse(input, gdi);


                    System.IO.Stream output = null;
                    try
                    {
                        output = new System.IO.FileStream(dest, System.IO.FileMode.Create);

                        gdi.Write(output);
                    }
                    finally
                    {
                        if (output != null)
                            output.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
		   

		    
        }
    }
}
