namespace AOC.Y2019.Day15.P02
{
    public interface IIOPort
    {
        IOReadResult Read();

        /// <summary>
        /// Registers an interpreter to be executed when input becomes available.
        /// </summary>
        void RegisterInterpreterForInput(IInterpreter interpreter);

        void Write(long value);
    }
}