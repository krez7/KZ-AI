using Bitboard;

namespace Krezb0t;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("KZ-Bot - v0.0.0");
        
        Magicboard board;
        bool correctInput = false;

        /////////MAIN MENU//////////
        Console.WriteLine("1) Play");
        Console.WriteLine("2) Start from Position");
        Console.Write("Choose an option: ");

        while(!correctInput){
            string? option = Console.ReadLine();
            if(option == "1"){
                correctInput = true;
                board = new Magicboard();
            }
            else if(option == "2"){
                Console.WriteLine("Enter FEN: ");
                string? FEN = Console.ReadLine();
            
                if(!string.IsNullOrEmpty(FEN)){
                    correctInput = true;
                    board = new Magicboard(FEN);           
                }
           }
           else{Console.WriteLine("Incorrect input");}
        }
        
        /////////Color selection/////////
        bool playerColor;
        correctInput = false;
        
        Console.Write("Select your color(b/w): ");
        while(!correctInput){
           string? colorStr = Console.ReadLine();
           if(colorStr == "b"){
               correctInput = true; 
               playerColor = Chessboard.white;
           }
           else if(colorStr == "w"){
               correctInput = true;
               playerColor = Chessboard.white;}
           else{Console.WriteLine("Incorrect input");}
        }
    }
}
