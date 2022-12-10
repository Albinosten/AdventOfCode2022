namespace AdventOfCode2022
{

    public interface IPuzzle<T>
    {
        T Solve();
        T SolveNext();

        T FirstResult {get;}
        T SecondResult {get;}    
    }
    public interface IPuzzle<T, P>
    {
        T Solve();
        P SolveNext();

        T FirstResult {get;}
        P SecondResult {get;}    
    }
    public interface IPuzzle
    {
        int Solve();
        long SolveNext();

        int FirstResult {get;}
        long SecondResult {get;}
    }
}