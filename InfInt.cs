using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ProgAssignment2
{
    class InfInt : IComparable<InfInt>
    {
        private List<int> data;
        private bool isNegative;

        // encapsulation 
        public bool IsNegative { get => isNegative; set => isNegative = value; }
        public List<int> Data { get => data; set => data = value; }

        // creating default constructor
        public InfInt()
        {
            this.isNegative = true;
            this.Data = new List<int>();
        }

        // creating parametrized constructor
        public InfInt(string line)
        {
            if (line.StartsWith("-"))
            {
                this.isNegative = true;
                line = line.Trim('-');  // removing sign 
                this.data = line.Select(c => c - '0').ToList(); // converting string array into int list
            }
            else
            {
                this.isNegative = false;
                this.data = line.Select(c => c - '0').ToList(); // converting string array into int list
            }
        }

        // implemnting IComparable interface
        public int CompareTo(InfInt obj)
        {
            // comparing only with datas, not with sign

            // when their length is equal
            if (this.data.Count() == obj.data.Count())
            {
                for (int i = 0; i < this.data.Count(); i++)
                {
                    if (this.data[i] != obj.data[i])
                    {
                        return this.data[i].CompareTo(obj.data[i]);
                    }
                }
            }

            // when first's length is smaller
            if (this.data.Count() < obj.data.Count())
            {
                return -1;
            }
            // when first's length is bigger
            else if (this.data.Count() > obj.data.Count())
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public void Result(InfInt operand, string opr)
        {
            switch (opr)
            {
                case "+":
                    Plus(operand);
                    break;
                case "-":
                    Minus(operand);
                    break;
                case "*":
                    Times(operand);
                    break;
                default:
                    break;
            }
        }

        public void Plus(InfInt operand)
        {
            InfInt result = new InfInt();
            int sum = 0;
            int carry = 0;
            List<int> smaller = new List<int>();  // saving data which is smaller in length
            List<int> larger = new List<int>();  // saving data which is larger in length

            // when both operands are negative or positive
            if ((isNegative == false && operand.isNegative == false) || (isNegative == true && operand.isNegative == true))
            {
                if (data.Count()>operand.data.Count())  // when num1 > num2 by data
                {
                    smaller = operand.data;
                    larger = data;

                } else
                {
                    smaller = data;
                    larger = operand.data;
                }

                // reversing datas to make adding easir and from starting index
                smaller.Reverse();
                larger.Reverse();

                for(int i=0; i<smaller.Count(); i++)
                {
                    sum = smaller[i] + larger[i] + carry;
                    if (sum >= 10)
                    {
                        carry = 1;
                        sum %= 10;
                    }
                    else
                    {
                        carry = 0;
                    }
                   
                    result.data.Insert(0, sum);
                }


                // checking carry after iterations
                if (carry == 0)
                {
                    for (int i = smaller.Count(); i < larger.Count(); i++)
                    {
                        result.data.Insert(0, larger[i]);
                    }
                } 
                else
                {
                    int stoppedAt = smaller.Count() - 1;
                    while (carry == 1)
                    {
                        if (larger.Count - 1 == stoppedAt)
                        {
                            result.data.Insert(0, carry);
                            carry = 0;
                        } else
                        {
                            sum = larger[stoppedAt + 1] + carry;
                            if (sum >= 10)
                            {
                                carry = 1;
                                sum %= 10;
                                
                            } else
                            {
                                carry = 0;
                            }
                            result.data.Insert(0, sum);
                            stoppedAt++;
                        }
                    }

                    if(larger.Count() - 1 != stoppedAt)
                    {
                        for(int i=stoppedAt+1; i<larger.Count; i++)
                        {
                            result.data.Insert(0, larger[i]);
                        }
                    }
                }

                // determining data's sign
                if (isNegative == false && operand.isNegative == false)
                {
                    result.isNegative = false;
                }
                else
                {
                    result.isNegative = true;
                }

                result.Print();

            } else
            {
                /*
                 * since postive number plus negative number is same as
                 * substractring two positve numbers
                 * +num1 + (-num2) <=> +num1 - +num2
                 * -num1 + +num2 <=> +num2 - +num1
                 */
                
                if(isNegative==false && operand.isNegative == true)
                {
                    operand.isNegative = false;
                    Minus(operand);
                }
                else
                {
                    // swap num2 and num1 to get (num2 - num1)
                    isNegative = false;
                    InfInt temp = new InfInt();
                    temp.data = data;
                    temp.isNegative = isNegative;
                    this.data = operand.data;
                    this.isNegative = operand.isNegative;
                    operand.data = temp.data;
                    operand.isNegative = temp.isNegative;                    
                    Minus(operand);
                }
                
            }
        }

        public void Minus(InfInt operand)
        {
            InfInt result = new InfInt();
            int difference = 0;
            int borrow = 0;
            List<int> smaller = new List<int>();  // saving data which is smaller by data
            List<int> larger = new List<int>();  // saving data which is larger by data

            /*
             * when first operand is positive and second is negative
             * is same as +num1 - (-num2) = +num1 + num2
            */
            if(isNegative==false && operand.isNegative == true)
            {
                operand.isNegative = false;
                Plus(operand);
            }
            else if (isNegative==true && operand.isNegative == false)
            {
                /*
                 * -num1 - (+num2) <=> (-num1) + (-num2) <=> - ((+num1) + (+num2))
                 * To have final result negative, I will assign second operand - sign and call Plus() method
                 */
                operand.isNegative = true;
                Plus(operand);
            } else
            {
                /*
                 * converting case (-num1) - (-num2)  to (-num1) + (+num2) <=> (+num2) - (+num1)
                 * only one case is left, when (+num1) - (+num2)
                 * as a result, both cases are converted to one case
                 */
                if(isNegative==true && operand.isNegative == true)
                {
                    isNegative = false;
                    operand.isNegative = false;

                    // swaping num1 and num2 to have (+num) - (num2)
                    InfInt temp = new InfInt();
                    temp.data = data;
                    temp.isNegative = isNegative;
                    this.data = operand.data;
                    this.isNegative = operand.isNegative;
                    operand.data = temp.data;
                    operand.isNegative = temp.isNegative;
                }

                // now we have (+num1) - (num2) input

                // if num1 is greater or equal to num2
                if (this.CompareTo(operand) >= 0)
                {
                    result.isNegative = false;
                    smaller = operand.data;
                    larger = data;
                } 
                else  // num1 is less than num2
                {
                    result.isNegative = true;
                    smaller = data;
                    larger = operand.data;
                }

                // reversing datas to make subtraction easir and from starting index
                smaller.Reverse();
                larger.Reverse();

                for(int i=0; i<smaller.Count(); i++)
                {
                    difference = larger[i] - smaller[i];
                    if (difference >= 0)
                    {
                        result.data.Insert(0, difference);
                        borrow = 0;
                    } else
                    {
                        // find new avalaible borrow
                        for(int j=i+1; j<larger.Count(); j++)
                        {
                            if (larger[j] != 0)
                            {
                                larger[j] -= 1;
                                borrow = 1;
                                difference = (borrow * 10 + larger[i]) - smaller[i];
                                result.data.Insert(0, difference);
                                borrow = 0;
                                // new borrow is found, leave the loop
                                break;
                            } else
                            {
                                larger[j] = 9;                              
                            }
                        }


                    }
                }

                // copying remaining data from larger number
                for(int i=smaller.Count(); i < larger.Count(); i++)
                {
                    result.data.Insert(0, larger[i]);
                }
                // formating first digit
                if (result.data.First() == 0 && result.data.Count > 1) 
                {
                    result.data.Remove(0);
                }

                // printing the result
                result.Print();
            }

        }

        public void Times(InfInt operand)
        {
            InfInt result = new InfInt();
            List<int> smaller = new List<int>();  // saving data which is smaller in length
            List<int> larger = new List<int>();  // saving data which is larger in length

            // when num1 > num2 by data
            if (data.Count() >= operand.data.Count())  
            {
                smaller = operand.data;
                larger = data;

            }
            else
            {
                smaller = data;
                larger = operand.data;
            }

            // defining result's sign
            if (isNegative != operand.isNegative)
            {
                result.isNegative = true;
            }
            else
            {
                result.isNegative = false;
            }

            // creating array of List<int> to save intermadiate products
            var products = new List<int>[smaller.Count()];
            for(int i=0; i < smaller.Count(); i++)
            {
                products[i] = new List<int>();
            }
          
            //reversing datas to make subtraction easir and from starting index
            smaller.Reverse();
            larger.Reverse();

            for (int i=0; i<smaller.Count(); i++)
            {
                int carry = 0;
                for (int j=0; j<larger.Count(); j++)
                {
                    int temp = smaller[i] * larger[j] + carry;
                    if (temp < 10)
                    {
                        products[i].Insert(0, temp);
                        carry = 0;
                    } else
                    {
                        carry = temp / 10;
                        temp %= 10;
                        products[i].Insert(0, temp);
                    }
                }
                // checking for excess carry
                if (carry > 0)
                {
                    products[i].Insert(0, carry);
                }

                // adding zeros for addition
                for(int z=0; z<i; z++)
                {
                    products[i].Add(0);
                }
            }

            // if multiplier is one digit number
            if (smaller.Count() == 1)
            {
                result.data = products[0];
            }
            else
            {
                // adding intermadiate products
                result.data = PlusForTimes(products, smaller.Count());
            }

            // printing the result
            result.Print();
        }

        public List<int> SumOfLists(List<int> num1 , List<int> num2)
        {
            List<int> result = new List<int>();

            List<int> smaller = new List<int>();  // saving data which is smaller in length
            List<int> larger = new List<int>();  // saving data which is larger in length

            int sum = 0;
            int carry = 0;

            if (num1.Count() > num2.Count())  // when num1 > num2 by data
            {
                smaller = num2;
                larger = num1;
            }
            else
            {
                smaller = num1;
                larger = num2;
            }

            for (int i = 0; i < smaller.Count(); i++)
            {
                sum = smaller[i] + larger[i] + carry;
                if (sum >= 10)
                {
                    carry = 1;
                    sum %= 10;
                }
                else
                {
                    carry = 0;
                }

                result.Insert(0, sum);
            }


            // checking carry after iterations
            if (carry == 0)
            {
                for (int i = smaller.Count(); i < larger.Count(); i++)
                {
                    result.Insert(0, larger[i]);
                }
            }
            else
            {
                int stoppedAt = smaller.Count() - 1;
                while (carry == 1)
                {
                    if (larger.Count - 1 == stoppedAt)
                    {
                        result.Insert(0, carry);
                        carry = 0;
                    }
                    else
                    {
                        sum = larger[stoppedAt + 1] + carry;
                        if (sum >= 10)
                        {
                            carry = 1;
                            sum %= 10;

                        }
                        else
                        {
                            carry = 0;
                        }
                        result.Insert(0, sum);
                        stoppedAt++;
                    }
                }

                if (larger.Count() - 1 != stoppedAt)
                {
                    for (int i = stoppedAt + 1; i < larger.Count; i++)
                    {
                        result.Insert(0, larger[i]);
                    }
                }
            }

            result.Reverse();
            return result;
        }

        public List<int> PlusForTimes(List<int> [] products, int size)
        {
            List<int> result = new List<int>();

            // reversing lists in products[] to make adding easier
            for(int i=0; i<size; i++)
            {
                products[i].Reverse();
            }

            // copying first row of array in result
            result = products[0];

            for (int i = 0; i < size - 1; i++)
            {
                result = SumOfLists(result, products[i + 1]);
            }


            // reversing result for normalized answer
            result.Reverse();

            return result;
        }

        public void Print()
        {
            if (isNegative == true)
            {
                Console.Write("-");
            }

            foreach(int num in data)
            {
                Console.Write(num);
            }

            Console.WriteLine();
        }
    }
}