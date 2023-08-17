using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class ParserClass
    {
        public Lexer Lexer { get; set; }
        private Token token;
        private bool afterAssignment = false;
        private string id = "";
        private Dictionary<string, int> dictionary = new();
        private List<string> rememberPlusesAndMinuses = new();
        private bool isPrintable = false;
        int openBracketCounter = 0;
        int helpCounter = 0;

        public ParserClass(Lexer lexer)
        {
            Lexer = lexer;
            Next();
        }

        //POMOCNA FUNKCIJA ZA CITANJE NAREDNOG TOKENA, JER SE CESTO KORISTI
        private void Next()
        {
            token = Lexer.Next();
        }

        //ExpressionList -> EOF | Expression ; ExpressionList
        public List<INode> ExpressionList()
        {
            List<INode> expressions = new();
            while (token.Type != "EOF")
            {
                INode expression = Expression();
                expressions.Add(expression);
                //UKOLIKO SMO IZRACUNALI VRIJEDNOST ZA NEKI IDENTIFIKATOR, SACUVACEMO TU VRIJEDNOST U MAPU
                if (id != "")
                {
                    dictionary[id] = expression.Value;
                    id = "";
                }
                //UKOLIKO SMO NAISLI NA PRINT, ISPISACEMO TO NESTO STO SE TREBA ISPISATI
                if(isPrintable)
                {
                    Console.WriteLine(expression.Value);
                    isPrintable = false;
                } 
                //RESETUJEMO NEKE ODREDJENE STAVKE
                afterAssignment = false;
                rememberPlusesAndMinuses.Clear();
                if (token.Type != ";")
                {
                    throw new Exception();
                }

                Next();
            }

            return expressions;
        }

        //Expression -> int + int
        //Kako za proizvoljan broj integera?
        //Expression -> int + Expression | int
        //Expression -> Term + Expression | Term
        public INode Expression()
        {
            //AKO DODJELJUJEMO NESTO NECEMU
            if (token.Type == "identificator" && !afterAssignment)
            {
                id = token.Value;
                Next();
                //KADA SMO NAISLI NA JEDNAKO, AFTER ASSINGMENT POSTAJE TRUE, JER SMO LOGICNO PROSLI ZNAK JEDNAKO
                if (token.Type == "=")
                {
                    Next();
                    afterAssignment = true;
                }
                else
                {
                    throw new Exception();
                }
            }
            //ZELIMO ISPISATI NESTO, TO I NAGLASIMO POMOCU isPrintable
            if (token.Type == "print")
            {
                isPrintable = true;
                afterAssignment = true;
                //string printString = token.Value;
                Next();
            }

            INode term = Term();

            if (token.Type == "+")
            {
                if((!isPrintable && openBracketCounter > 0) || (isPrintable && openBracketCounter > 1))
                {
                    helpCounter++;
                }
                //PAMTIMO PLUSEVE I MINUSE, DA BISMO IZBJEGLI PROBLEM KADA IMAMO NPR. 25 - 5 + 1
                rememberPlusesAndMinuses.Add("+");
                Next();
                INode expression = Expression();
                string first = "", second = "";
                //CITAMO PO DVA OPERATORA
                if(rememberPlusesAndMinuses.Count > 1)
                {
                    second = rememberPlusesAndMinuses.ElementAt(rememberPlusesAndMinuses.Count - 1);
                    first = rememberPlusesAndMinuses.ElementAt(rememberPlusesAndMinuses.Count - 2);
                    rememberPlusesAndMinuses.RemoveAt(rememberPlusesAndMinuses.Count - 1);
                }
                //AKO SMO IMALI IZRAZ 25 - 5 + 1 PREVODIMO GA U 25 - ( 5 - 1), DA IZBJEGNEMO KOMPLIKACIJE
                if(second == "+" && first == "-")
                {
                    BinaryOperation binOp = new(left: term, right: expression, Operation: "-");
                    return binOp;
                }
                //U SUPROTNOM, OBICNO SABIRANJE
                else
                {
                    BinaryOperation binOp = new(left: term, right: expression, Operation: "+");
                    return binOp;
                }
            }

            if (token.Type == "-")
            {
                if ((!isPrintable && openBracketCounter > 0) || (isPrintable && openBracketCounter > 1))
                {
                    helpCounter++;
                }
                rememberPlusesAndMinuses.Add("-");
                Next();
                INode expression = Expression();
                string first = "", second = "";
                if (rememberPlusesAndMinuses.Count > 1)
                {
                    second = rememberPlusesAndMinuses.ElementAt(rememberPlusesAndMinuses.Count - 1);
                    first = rememberPlusesAndMinuses.ElementAt(rememberPlusesAndMinuses.Count - 2);
                    rememberPlusesAndMinuses.RemoveAt(rememberPlusesAndMinuses.Count - 1);
                }
                //UKOLIKO SMO IMALI NPR 52 - 5 - 2 PREVODIMO U 52 - (5 + 2)
                if (second == "-" && first == "-")
                {
                    BinaryOperation binOp = new(left: term, right: expression, Operation: "+");
                    return binOp;
                }
                //U SUPROTNOM, OBICNO ODUZIMANJE
                else
                {
                    BinaryOperation binOp = new(left: term, right: expression, Operation: "-");
                    return binOp;
                }
            }

            return term;
        }

        //Term -> Factor * Term | Factor
        public INode Term()
        {
            INode factor = Factor();

            Next();

            if (token.Type == "*")
            {
                Next();
                INode term = Term();
                BinaryOperation binOp = new(left: factor, right: term, Operation: "*");
                return binOp;
            }

            if (token.Type == "/")
            {
                Next();
                INode term = Term();
                BinaryOperation binOp = new(left: factor, right: term, Operation: "/");
                return binOp;
            }

            return factor;
        }

        //Factor -> int
        public INode Factor()
        {
            INode elem = new();
            //AKO SMO NAISLI NA OTVORENU ZAGRADU, RACUNAMO TO STO JE UNUTAR NJE KAO POSEBAN IZRAZ
            if (token.Type == "(")
            {
                openBracketCounter++;
                Next();
                elem = Expression();
            }


            if (token.Type != ")")
            {
                if (token.Type != "int" && token.Type != "identificator")
                {
                    throw new Exception();
                }
                //PROVJERAVAMO DA LI JE TOKEN INT ILI POSTOJI VEC KAO IZRACUNAT U MAPI
                //UKOLIKO GA NISMO IZRACUNALI, BACAMO IZUZETAK
                if (token.Type == "int")
                {
                    IntNode intNode = new(int.Parse(token.Value));
                    return intNode;
                }
                else
                {
                    if (dictionary.ContainsKey((token.Value)))
                    {
                        IntNode intNode = new(dictionary[token.Value]);
                        return intNode;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            //AKO SMO IZRACUNALI NESTO U ZAGRADI, TAJ IZRAZ CEMO I VRATITI, PROPAGIRATI NAZAD!
            else
            {
                openBracketCounter--;
                if(openBracketCounter == 0 || isPrintable && openBracketCounter == 1)
                {
                    /*if(rememberPlusesAndMinuses.Count > 0)
                    {
                        for (int i = rememberPlusesAndMinuses.Count - 1; i >= helpCounter; i--)
                        {
                            //Console.WriteLine(rememberPlusesAndMinuses.ElementAt(i));
                            rememberPlusesAndMinuses.RemoveAt(i);
                        }
                    }*/

                    
                    rememberPlusesAndMinuses.Clear();
                }
                helpCounter = 0;
                return elem;
            }
        }
    }
}
