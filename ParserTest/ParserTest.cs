using NUnit.Framework;
using Parser;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestParser_ParsingExpressionWholeCoverage()
        {
            Lexer lexer = new Lexer("5 + 4;\na = 25 * 3 + 7 * 2 -1 + 5; b = \n52 - 25 - 1;  25;" +
                "\nprint(a);c = (((a))  ) + ( - 3 + b * (b + \na)) / 2; print(b); print(c);");
            ParserClass parser = new(lexer);

            try
            {
                //8 expressions processed
                List<INode> list = parser.ExpressionList();
                Assert.IsTrue(list.Count == 8);
                //Associativity of operators modeled
                Assert.IsTrue(list[6].Value == 26);
            }
            catch(Exception e)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestParser_ThrowErrorInvalidExpression()
        {
            //Missing one ;
            Lexer lexer = new Lexer("5 + 4;\na = 25 * 3 + 7 * 2 -1 + 5; b = \n52 - 25 - 1;  25;" +
                "\nprint(a);c = (((a))  ) + ( - 3 + b * (b + \na)) / 2 print(b); print(c);");
            ParserClass parser = new(lexer);
            try
            {
                List<INode> list = parser.ExpressionList();
                Assert.Fail();
            }
            catch(Exception e)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestParser_ThrowErrorMultipleOperatorsNextToEachOther()
        {
            Lexer lexer = new Lexer("2 ++ 4;");
            ParserClass parser = new(lexer);
            try
            {
                List<INode> list = parser.ExpressionList();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestParser_CheckAssociativityPlusAndMinus()
        {
            Lexer lexer = new Lexer("7 -2+   3 -4 + 10 +2 - 1 -4;");
            ParserClass parser = new(lexer);
            List<INode> list = parser.ExpressionList();
            Assert.IsTrue(list[0].Value == 11);
            lexer = null; parser = null; list.Clear();
            lexer = new Lexer("1 + (2 * 4 - 3 + (2*2)) - 5;");
            parser = new(lexer);
            list = parser.ExpressionList();
            Assert.IsTrue(list[0].Value == 5);
            lexer = null; parser = null; list.Clear();
            lexer = new Lexer("1 + (2 * 4 - 3 + (2-2 + 1) * (3-1*6)) - 5;");
            parser = new(lexer);
            list = parser.ExpressionList();
            Assert.IsTrue(list[0].Value == -2);
            lexer = null; parser = null; list.Clear();
            lexer = new Lexer("-3 + (-4 * (3 - 1 -1 + 2 / 2));");
            parser = new(lexer);
            list = parser.ExpressionList();
            Assert.IsTrue(list[0].Value == -11);
        }

        [Test]
        public void TestParser_ModificationTest()
        {
            Lexer lexer = new Lexer("(5 - 3 - 4) + 1;");
            ParserClass parserClass = new ParserClass(lexer);
            List<INode> list = parserClass.ExpressionList();
            Assert.IsTrue(list[0].Value == -1);
        }
    }
}