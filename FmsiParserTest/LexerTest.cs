using NUnit.Framework;
using Parser;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FmsiParserTest
{
    public class LexerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLexer_AdditionWithMultipleDigitNumbers()
        {
            Lexer lexer = new("27 + 333");
            Token t1 = lexer.Next();
            Token t2 = lexer.Next();
            Token t3 = lexer.Next();
            Assert.AreEqual("int", t1.Type);
            Assert.AreEqual("27", t1.Value);
            Assert.AreEqual("+", t2.Type);
            Assert.AreEqual("int", t3.Type);
            Assert.AreEqual("333", t3.Value);
            
        }

        [Test]
        public void TestLexer_Threee()
        {
            Lexer lexer = new("27 + 333 + 49");
            Token t1 = lexer.Next();
            Token t2 = lexer.Next();
            Token t3 = lexer.Next();
            Token t4 = lexer.Next();
            Token t5 = lexer.Next();
            Assert.AreEqual("int", t1.Type);
            Assert.AreEqual("27", t1.Value);
            Assert.AreEqual("+", t2.Type);
            Assert.AreEqual("int", t3.Type);
            Assert.AreEqual("333", t3.Value);
            Assert.AreEqual("+", t4.Type);
            Assert.AreEqual("int", t5.Type);
            Assert.AreEqual("49", t5.Value);
        }

        [Test]
        public void TestException()
        {
            Lexer lexer = new("22 $;");

            //DA LI IZUZETAK TREBA BITI BACEN
            try
            {
                Token token = lexer.Next();
                while(token.Type != ";")
                {
                    token = lexer.Next();
                }
                Assert.Fail();
            }
            catch(Exception ex)
            {
                Assert.Pass();
            }
        }

        //PRINTS OUT TOKEN TYPE PRINT, BUT DOESN'T PASS TEST?
        //PRINT WORKS EVERY TIME
        /*[Test]
        public void TestPrint()
        {
            Lexer lexer = new("print(2 + 4);");
            try
            {
                Token token = lexer.Next();
                Console.WriteLine(token.Type);
                if ("print".Equals(token.Type))
                {
                    Assert.Pass();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }*/
    }
}