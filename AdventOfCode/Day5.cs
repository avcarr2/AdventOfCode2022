using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode;

public class CargoStack
{
    public Stack<Cargo> CargoLl { get; set; }
    public int StackPosition { get; set; }
    public LinkedList<Cargo> CargoList { get; set; }

    public CargoStack()
    {
        CargoLl = new Stack<Cargo>(); 
        CargoList = new LinkedList<Cargo>();
    }
}

public class Cargo
{
    public string Name { get; set; }
    public Cargo(string name)
    {
        Name = name;
    }
}

public static class CargoStackExtensions
{
    public static void ExecuteInstruction(this List<CargoStack> stackList, 
        int cargoQuantity, int stackPosInit, int stackPosFinal)
    {
        int initIndex = stackList.FindIndex(i => i.StackPosition == stackPosInit);
        int finIndex = stackList.FindIndex(i => i.StackPosition == stackPosFinal);

        for (int i = 0; i < cargoQuantity; i++)
        {
            var temp = stackList[initIndex].CargoLl.Pop(); 
            stackList[finIndex].CargoLl.Push(temp);
        }
    }
    public static void ExecuteInstructionPart2(this List<CargoStack> stackList,
        int cargoQuantity, int stackPosInit, int stackPosFinal)
    {
        int initIndex = stackList.FindIndex(i => i.StackPosition == stackPosInit);
        int finIndex = stackList.FindIndex(i => i.StackPosition == stackPosFinal);

        List<Cargo> tempList = new(); 
        for (int i = 0; i < cargoQuantity; i++)
        {
            tempList.Add(stackList[initIndex].CargoLl.Pop());
        }

        tempList.Reverse();
        for (int i = 0; i < tempList.Count(); i++)
        {
            stackList[finIndex].CargoLl.Push(tempList[i]);
        }
    }

    public static int[] InstructionsLineToIntTupe(this string instructionLine)
    {
        var line = instructionLine.Split(" ");
        int[] results = (new[] { line[1], line[3], line[5] })
            .Select(i => int.Parse(i)).ToArray();
        return results; 
    }

    public static void GetTopOfStack(this List<CargoStack> stackList)
    {
        if (stackList[0].CargoList.Count == 0) ; 
        foreach (var stack in stackList)
        {
            Console.WriteLine(stack.CargoLl.Peek().Name);
        } 
    }
    public static void GetTopOfLinkedList(this List<CargoStack> stackList)
    {
        foreach (var stack in stackList)
        {
            Console.WriteLine(stack.CargoList.Last.Value.Name);
        } 
    }

}