using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Lexer
    {
        private readonly string source;
        private int sourcePosition;
        public Lexer(string input)
        {
            sourcePosition = 0;
            source = input;
        }

        public Token Next()
        {
            string buffer = "";

            if (sourcePosition >= source.Length)
            {
                return new Token("EOF", null);
            }
            else if (sourcePosition > source.Length)
            {
                throw new Exception();
            }

            while (sourcePosition < source.Length && char.IsWhiteSpace(source[sourcePosition]))
            {
                sourcePosition++;
            }

            //PROVJERA DA LI SMO NAISLI NA PRINT ILI NA IDENTIFIKATOR
            //U SKLADU SA TIM VRACAMO ODGOVARAJUCI TOKEN, DA BI KASNIJE ZNALI DA LI ISPISUJEMO NESTO ILI NE
            if (char.IsLetter(source[sourcePosition]))
            {
                //UKOLIKO JE NEKO SLOVO PRIJE NEKOG DRUGOG, I UKOLIKO RAZMAK POSTOJI, BACAMO IZUZETAK, PREPOZNAJEMO KAO LEKSICKU GRESKU
                if (isLetterBefore())
                {
                    throw new Exception();
                }

                do
                {
                    buffer += source[sourcePosition];
                    if(char.IsWhiteSpace(source[sourcePosition]))
                    {
                        throw new Exception();
                    }
                    sourcePosition++;
                } while (sourcePosition < source.Length && (char.IsLetter(source[sourcePosition])));

                if (buffer == "print")
                {
                    return new Token("print", null);
                }
                else
                {
                    return new Token("identificator", buffer);
                }
            }

            if (char.IsDigit(source[sourcePosition]))
            {
                do
                {
                    buffer += source[sourcePosition];
                    sourcePosition++;
                } while (sourcePosition < source.Length && char.IsDigit(source[sourcePosition]));

                return new Token("int", buffer);
            }

            if(source[sourcePosition] == '+')
            {
                sourcePosition++;
                return new Token("+", null);
            }

            if (source[sourcePosition] == '*')
            {
                sourcePosition++;
                return new Token("*", null);
            }

            
            if(source[sourcePosition] == '-')
            {
                //UKOLIKO SMO NAISLI NA MINUS KAO OPERATOR
                if(isMinusBefore())
                {
                    sourcePosition++;
                    return new Token("-", null);
                }
                //UKOLIKO SE MINUS ODNOSI NA TO DA JE BROJ NEGATIVAN
                //CITAMO TAJ BROJ I VRACAMO TOKEN KAO NEGATIVAN BROJ
                else
                {
                    buffer += '-';
                    sourcePosition++;
                    while(sourcePosition < source.Length && char.IsWhiteSpace(source[sourcePosition]))
                    {
                        sourcePosition++;
                    }

                    while(sourcePosition < source.Length && char.IsDigit(source[sourcePosition]))
                    {
                        buffer += source[sourcePosition];
                        sourcePosition++;
                    }
                    return new Token("int", buffer);
                }
            }

            if(source[sourcePosition] == '/')
            {
                sourcePosition++;
                return new Token("/", null);
            }

            if(source[sourcePosition] == '(')
            {
                sourcePosition++;
                return new Token("(", null);
            }

            if(source[sourcePosition] == ')')
            {
                sourcePosition++;
                return new Token(")", null);
            }

            if(source[sourcePosition] == '=')
            {
                sourcePosition++;
                return new Token("=", null);
            }

            if (source[sourcePosition] == ';')
            {
                sourcePosition++;
                return new Token(";", null);
            }

            throw new Exception();
        }


        private bool isLetterBefore()
        {
            int tempCounter = sourcePosition;
            tempCounter--;
            while (tempCounter > 0 && char.IsWhiteSpace(source[tempCounter]))
            {
                tempCounter--;
            }
            if (tempCounter < 0)
            {
                return false;
            }

            if (!char.IsLetter(source[tempCounter]))
            {
                return false;
            }
            return true;
        }

        //POMOCNA FUNCKIJA KOJA POMAZE PREPOZNAVANJE MINUSA
        //AKO SMO NAISLI NA NJEGA I DA LI JE ON OPERATOR ILI JE UZ NEKI BROJ
        private bool isMinusBefore()
        {
            int tempCounter = sourcePosition;
            tempCounter--;
            while(tempCounter > 0 && char.IsWhiteSpace(source[tempCounter]))
            {
                tempCounter--;
            }
            if(tempCounter < 0)
            {
                return false;
            }
            if(source[tempCounter] == ')')
            {
                return true;
            }

            if(!char.IsDigit(source[tempCounter]))
            {
                return false;
            }
            return true;

        }
    }
}
