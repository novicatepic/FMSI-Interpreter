using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Parser
{
    class Program
    {
        public static void Main(string[] args)
        {
            string input = null;
            input = File.ReadAllText(args[0]);

            //Lexer lexer = new(input);
            Lexer lexer = new("print((5 - 3 - 4) + 1);");

            ParserClass parser = new(lexer);
            List<INode> list = parser.ExpressionList();

            Console.WriteLine("s");          
        }
    }

    //TEST FROM TEST.TXT
    /*5 + 4;
a = 25 * 3 + 7 + 2 * -1 + 5; b = 
52 - 25 - 1

	; 25;
print(a);
c = (((a))	) + (-3 + b * (b + a
))	/ 2	;
print(b);	print(c);
print( 4 * 3	-2 + 1 -2 -1);
d = 140 + (12/3+1);
print(b + c);
	print(d + a);
12 + 4;*/
}
