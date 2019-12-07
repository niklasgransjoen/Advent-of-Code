namespace AOC2019.Day07.P02
{
    public interface IIOPort
    {
        IOReadResult Read();

        /// <summary>
        /// Registers an interpreter to be executed when input becomes available.
        /// </summary>
        void RegisterInterpreterForInput(IInterpreter interpreter);

        void Write(int value);
    }
}