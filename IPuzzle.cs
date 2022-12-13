namespace AdventOfCode2022
{

    public interface IPuzzle<T> : IPuzzle<T,T>
    {
    }
    public interface IPuzzle<T, P>
    {
        T Solve();
        P SolveNext();

        T FirstResult {get;}
        P SecondResult {get;}    
    }
    public interface IPuzzle : IPuzzle<int,long>
    {
    }
}