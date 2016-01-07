using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;

namespace function_test_1
{
	[Activity (Label = "Calculator_proto_3", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		private string infix = string.Empty;
		private string postfix = string.Empty;
		private string str_temp = string.Empty;
		private bool clear_text = false;
		private bool is_decimal = false;
		private bool has_operator = false;
		private string str_operator = string.Empty;
		private Stack<string> operator_stack = new Stack<string> ();
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			TextView answer = FindViewById<TextView> (Resource.Id.answer);

			// Get our button from the layout resource,
			Button one = FindViewById<Button> (Resource.Id.one);
			Button two = FindViewById<Button> (Resource.Id.two);
			Button three = FindViewById<Button> (Resource.Id.three);
			Button four = FindViewById<Button> (Resource.Id.four);
			Button five = FindViewById<Button> (Resource.Id.five);
			Button six = FindViewById<Button> (Resource.Id.six);
			Button seven = FindViewById<Button> (Resource.Id.seven);
			Button eight = FindViewById<Button> (Resource.Id.eight);
			Button nine = FindViewById<Button> (Resource.Id.nine);
			Button zero = FindViewById<Button> (Resource.Id.zero);
			Button point = FindViewById<Button> (Resource.Id.point);

			Button addition = FindViewById<Button> (Resource.Id.addition);
			Button subtraction = FindViewById<Button> (Resource.Id.subtraction);
			Button multiplication = FindViewById<Button> (Resource.Id.multiplication);
			Button division = FindViewById<Button> (Resource.Id.division);
			Button power = FindViewById<Button> (Resource.Id.power);

			Button clear = FindViewById<Button> (Resource.Id.clear);
			Button clearAll = FindViewById<Button> (Resource.Id.clearall);

			//TESTING
			Button infixButton = FindViewById<Button> (Resource.Id.infix);
			Button postfixButton = FindViewById<Button> (Resource.Id.postfix);
			Button answerButton = FindViewById<Button> (Resource.Id.answerButton);

			// and attach an event to it
			one.Click += new EventHandler (MyNumberClick);
			two.Click += new EventHandler (MyNumberClick);
			three.Click += new EventHandler (MyNumberClick);
			four.Click += new EventHandler (MyNumberClick);
			five.Click += new EventHandler (MyNumberClick);
			six.Click += new EventHandler (MyNumberClick);
			seven.Click += new EventHandler (MyNumberClick);
			eight.Click += new EventHandler (MyNumberClick);
			nine.Click += new EventHandler (MyNumberClick);
			zero.Click += new EventHandler (MyNumberClick);
			point.Click += new EventHandler (MyNumberClick);

			addition.Click += new EventHandler (MyOperatorClick);
			subtraction.Click += new EventHandler (MyOperatorClick);
			multiplication.Click += new EventHandler (MyOperatorClick);
			division.Click += new EventHandler (MyOperatorClick);

			//when clicked, will insert '^' into infix string
			//Takes number to left and multiplies itself y-1 times.
			//In postfix, the two numbers will be to the left of the operator
			//It will pop off the last 2 numbers, do the power operation, and return the answer to the stack
			power.Click += delegate
			{
				if (answer.Text.Length > 0) 
				{	
					clear_text = true;
					is_decimal = false;
					has_operator = true;
					answer.Text = "(^)";
					str_operator = "^";
				}
			};

//The infix string created dynamically.
			infixButton.Click += delegate {
				clear_text = true;
				answer.Text = infix;
			};

//TESTING
//Take infix and make postfix
//Deal with power oeprator precedence
			postfixButton.Click += delegate
			{
				postfix = string.Empty;
				for(int i = 0; i < infix.Length;i++)
				{
					//if the current token is not operator
					if(!"*+-/".Contains(infix[i].ToString()))
					{
						postfix += infix[i];
					}
					else 
					{
						postfix += ",";
						if(operator_stack.Count == 0)
						{
							operator_stack.Push(infix[i].ToString());
						}
						//determine to pop off stack or not
						else
						{
							//For addition/subtraction
							if("+-".Contains(infix[i].ToString()))
							{
								//if stack.peek is (+-) then pop to string
								if(operator_stack.Peek() == "+" || operator_stack.Peek() == "-" || operator_stack.Peek() == "*" || operator_stack.Peek() == "/")
								{
									postfix += operator_stack.Pop() + ",";
									operator_stack.Push(infix[i].ToString());
								}
							}
							//For power 
							else if(infix[i].ToString() == "^")
							{
								operator_stack.Push(infix[i].ToString());
							}
							//For multiplication/division
							else
							{
								if(operator_stack.Peek() == "*" || operator_stack.Peek() == "/")
								{
									postfix += operator_stack.Pop() + ",";
								}
								operator_stack.Push(infix[i].ToString());

							}
						}
					}
				} //end of for loop

				postfix += ",";

				while(operator_stack.Count != 0)
				{
					if(operator_stack.Count == 1)
						postfix += operator_stack.Pop();
					else
						postfix += operator_stack.Pop() + ",";
				}
				answer.Text = postfix;
				clear_text = true;
			};

//Displays the answer on screen
			answerButton.Click += delegate
			{
				//parse postfix string into string array with split (,)
				//If not an operator, cast to double and push to stack
				//when operator is hit, pop last 2 numbers, do calculation, push back to stack
				//at end of string, should have 1 number left on stack, that is answer
				string[] ans = postfix.Split(',');
				Stack<double> eval = new Stack<double>();
				double temp1;

				for(int i = 0; i< ans.Length;i++)
				{
					temp1 = double.NaN;

					if("*/+-".Contains(ans[i]))
					{

						switch(ans[i])
						{
						case("*"):
							{
								eval.Push(eval.Pop() * eval.Pop());
								break;
							}
						case ("+"):
							{
								eval.Push(eval.Pop() + eval.Pop());
								break;
							}
						case("-"):
							{
								temp1 = eval.Pop();
								eval.Push(eval.Pop() - temp1);
								break;
							}
						case("/"):
							{
								temp1 = eval.Pop();
								eval.Push(eval.Pop() / temp1);
								break;
							}
						}
					}
					else
					{
						//parse to double and push to stack
						eval.Push(double.Parse(ans[i]));
					}
				}
				answer.Text = eval.Pop().ToString();
				clear_text = true;
			};

//Clears the current text
			clear.Click += delegate {
				answer.Text = string.Empty;
			};
//Clears EVERYTHING
			clearAll.Click += delegate {
				infix = string.Empty;
				postfix = string.Empty;
				clear_text = false;
				is_decimal = false;
				has_operator = false;
				str_operator = string.Empty;
				answer.Text = string.Empty;
			};
		}
//Method for number buttons and decimal
		private void MyNumberClick (object sender, EventArgs e)
		{
			TextView answer = FindViewById<TextView> (Resource.Id.answer);
			string buttonText = ((Button)sender).Text;

			if (has_operator == true) {
				infix += str_operator;
				str_operator = string.Empty;
				has_operator = false;
				answer.Text = string.Empty;
			}

			if (buttonText == ".") {
				if (is_decimal != true) {
					if (answer.Text.Length == 0)
					{
						answer.Text = "0";
						infix += "0";
						clear_text = false;
					}
					is_decimal = true;
					answer.Text += buttonText;
					infix += buttonText;
				}	
			} else {
				if (clear_text == true) {
					clear_text = false;
					answer.Text = buttonText;
				} else {
					answer.Text += buttonText;
				}
				infix += buttonText;
			}
		}
//Method for Operator buttons
		private void MyOperatorClick (object sender, EventArgs e)
		{
			TextView answer = FindViewById<TextView> (Resource.Id.answer);
			string buttonText = ((Button)sender).Text;

			if (answer.Text.Length > 0) 
			{	
				clear_text = true;
				is_decimal = false;
				has_operator = true;
				answer.Text = "(" + buttonText + ")";
				str_operator = buttonText;
			}
		}
	}
}


